/**
 * HOMER - Responsive Admin Theme
 * Copyright 2015 Webapplayers.com
 *
 */

$(document).ready(function () {

    // Add special class to minimalize page elements when screen is less than 768px
    setBodySmall();

    // Handle minimalize sidebar menu
    $('.hide-menu').click(function (event) {
        event.preventDefault();
        if ($(window).width() < 769) {
            $("body").toggleClass("show-sidebar");
        } else {
            $("body").toggleClass("hide-sidebar");
        }
    });

    // Initialize metsiMenu plugin to sidebar menu
    $('#side-menu').metisMenu();

    // Initialize iCheck plugin
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });

    // Initialize animate panel function
    $('.animate-panel').animatePanel();

    // Function for collapse hpanel
    $('.showhide').click(function (event) {
        event.preventDefault();
        var hpanel = $(this).closest('div.hpanel');
        var icon = $(this).find('i:first');
        var body = hpanel.find('div.panel-body');
        var footer = hpanel.find('div.panel-footer');
        body.slideToggle(300);
        footer.slideToggle(200);

        // Toggle icon from up to down
        icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
        hpanel.toggleClass('').toggleClass('panel-collapse');
        setTimeout(function () {
            hpanel.resize();
            hpanel.find('[id^=map-]').resize();
        }, 50);
    });

    // Function for close hpanel
    $('.closebox').click(function (event) {
        event.preventDefault();
        var hpanel = $(this).closest('div.hpanel');
        hpanel.remove();
    });

    // Open close right sidebar
    $('.right-sidebar-toggle').click(function () {
        $('#right-sidebar').toggleClass('sidebar-open');
    });

    // Function for small header
    $('.small-header-action').click(function (event) {
        event.preventDefault();
        var icon = $(this).find('i:first');
        var breadcrumb = $(this).parent().find('#hbreadcrumb');
        $(this).parent().parent().parent().toggleClass('small-header');
        breadcrumb.toggleClass('m-t-lg');
        icon.toggleClass('fa-arrow-up').toggleClass('fa-arrow-down');
    });

    // Set minimal height of #wrapper to fit the window
    setTimeout(function () {
        fixWrapperHeight();
    });

    // Sparkline bar chart data and options used under Profile image on left navigation panel
    $("#sparkline1").sparkline([5, 6, 7, 2, 0, 4, 2, 4, 5, 7, 2, 4, 12, 11, 4], {
        type: 'bar',
        barWidth: 7,
        height: '30px',
        barColor: '#62cb31',
        negBarColor: '#53ac2a'
    });

    // Initialize tooltips
    $('.tooltip-demo').tooltip({
        selector: "[data-toggle=tooltip]"
    })

    // Initialize popover
    $("[data-toggle=popover]").popover();

    // Move modal to body
    // Fix Bootstrap backdrop issu with animation.css
    $('.modal').appendTo("body")

    // Select all checkboxes in Health History Report Pop-up
    $("a:contains('Select all')", "#frmHealthHistory").click(function () {
        $("input[type='checkbox']:not(:disabled)", "#frmHealthHistory").prop('checked', true);
    });
    // Deselect all checkboxes in Health History Report Pop-up
    $("a:contains('Deselect all')", "#frmHealthHistory").click(function () {
        $("input[type='checkbox']:not(:disabled)", "#frmHealthHistory").prop('checked', false);
    });

    // Select all checkboxes in Health History Report Pop-up
    $("a:contains('Select all')", "#frmShareHealthHistory").click(function () {
        $("input[type='checkbox']:not(:disabled)", "#frmShareHealthHistory").prop('checked', true);
    });
    // Deselect all checkboxes in Health History Report Pop-up
    $("a:contains('Deselect all')", "#frmShareHealthHistory").click(function () {
        $("input[type='checkbox']:not(:disabled)", "#frmShareHealthHistory").prop('checked', false);
    });
});

$(window).bind("load", function () {
    // Remove splash screen after load
    $('.splash').css('display', 'none')
})

$(window).bind("resize click", function () {

    // Add special class to minimalize page elements when screen is less than 768px
    setBodySmall();

    // Waint until metsiMenu, collapse and other effect finish and set wrapper height
    setTimeout(function () {
        fixWrapperHeight();
    }, 300);
})

function fixWrapperHeight() {

    // Get and set current height
    var headerH = 62;
    var navigationH = $("#navigation").height();
    var contentH = $(".content").height();

    // Set new height when contnet height is less then navigation
    if (contentH < navigationH) {
        $("#wrapper").css("min-height", navigationH + 'px');
    }

    // Set new height when contnet height is less then navigation and navigation is less then window
    if (contentH < navigationH && navigationH < $(window).height()) {
        $("#wrapper").css("min-height", $(window).height() - headerH + 'px');
    }

    // Set new height when contnet is higher then navigation but less then window
    if (contentH > navigationH && contentH < $(window).height()) {
        $("#wrapper").css("min-height", $(window).height() - headerH + 'px');
    }
}


function setBodySmall() {
    if ($(this).width() < 769) {
        $('body').addClass('page-small');
    } else {
        $('body').removeClass('page-small');
        $('body').removeClass('show-sidebar');
    }
}

function ExportHealthHistory() {
    //e.preventDefault();  //stop the browser from following
    var str = "";
    $("input[type='checkbox']", "#frmHealthHistory").each(function (idx, ui) {
        if ($(this).is(':checked'))
            str = str + (str != "" ? "%2C" : "") + $(this).val();
    });
    $('#myModal').modal('toggle');
    //alert(str);
    window.location.href = ROOT + 'Common/ExportPdf?strModules=' + str;
}



// Animate panel function
$.fn['animatePanel'] = function () {

    var element = $(this);
    var effect = $(this).data('effect');
    var delay = $(this).data('delay');
    var child = $(this).data('child');

    // Set default values for attrs
    if (!effect) { effect = 'zoomIn' };
    if (!delay) { delay = 0.06 } else { delay = delay / 10 };
    if (!child) { child = '.row > div' } else { child = "." + child };

    //Set defaul values for start animation and delay
    var startAnimation = 0;
    var start = Math.abs(delay) + startAnimation;

    // Get all visible element and set opactiy to 0
    var panel = element.find(child);
    panel.addClass('opacity-0');

    // Get all elements and add effect class
    panel = element.find(child);
    panel.addClass('animated-panel').addClass(effect);

    // Add delay for each child elements
    panel.each(function (i, elm) {
        start += delay;
        var rounded = Math.round(start * 10) / 10;
        $(elm).css('animation-delay', rounded + 's')
        // Remove opacity 0 after finish
        $(elm).removeClass('opacity-0');
    });
}

function GetFirstAttachment(iFileType,Id)
{
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Common/GetFirstAttachment",
        data: JSON.stringify({ fileType: iFileType, id: Id }),
        dataType: "json",
        success: function (result) {
            //alert(result);
            if (result !== undefined && result != null && result != "") {
                var finalPath = "/Images"
                switch (iFileType) {
                    case 6:
                        finalPath = finalPath + "/Procedures/" + result;
                }
                var link = document.createElement('a');
                link.href = finalPath;
                link.download = result;
                link.click();
                //window.location.href = finalPath;

            }
            else
            {
                swal({
                    title: "No File Attached!",
                    text: "There is no attachment corresponding to this record."
                });
            }
        },
        error: function (result) {
            swal({
                title: "Oops!",
                text: "We are unable to process your request. Try again later."
            });
        }
    });
}
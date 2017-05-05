var i;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "S.No.", field: "sno", width: 50 },
                        //{ title: "Entered By", type: "icon", icon: "glyphicon glyphicon-user", width: 100, align: "center", cssClass: "text-info" },
            { title: "Entered By", width: 100, align: "center", cssClass: "fa fa-user" },
            { field: "ImmunizationDate", title: "Taken On", tmpl: "{strImmunizationDate}", sortable: true, width: 150 },
            { field: "ImmunizationName", title: "Immunization", sortable: true },
            { field: "Comments", title: "Comments", sortable: true },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
        //    { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
            { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-off", tooltip: "Archive", events: { "click": Remove } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });

    grid.on('rowDataBound', function (e, $row, id, record) {
        if (record.SourceId == 2) {
            $(".fa-user", $row).attr("class", "fa fa-user-md text-info");
        }
        else
            $(".fa-user-md", $row).attr("class", "fa fa-user");
        //alert($row.html());
    });

    $("#btnAddPlayer").on("click", Add);
    $("#btnSearch").on("click", Search);

    $("#txtMasterSrch").keyup(function () {
        BindImmunizationType($("#txtMasterSrch").val(), 2);
    });

    $("#frmSaveImmunization").validate({
        rules:
        {
            ImmunizationDate:
            {
                required: true
            }
        },
        messages: {
            ImmunizationDate: {
                required: "Immunization Date is required."
            }
        },
        submitHandler: function (form) {
            form.submit();
        },
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent());
        }
    });
    $("#btnSave").click(function () {
        if ($(this).text() == "Next") {
            if ($("#frmSelImmunization").valid()) {
                $("#frmSelImmunization").hide();
                $("#txtImmunizationName").val($('#drpImmunizationTypes').find(":selected").text());
                $("input[datacolumn='ImmunizationsTypeId']").val($("#drpImmunizationTypes").find(":selected").val());
                $("#frmSaveImmunization").show();
                $(this).text("Save");
            }
        }
        else {
            if ($("#frmSaveImmunization").valid()) {
                SaveImmunizationDetails();
            }
        }
    });


    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        endDate: '+0d'
    });
    $('.input-group.date').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        endDate: '+0d'
    });

    $("a[class*='hd']", ".btn-group").hide();

    $(".btn-group .next").click(function () {
        $("a[class*='hd']", ".btn-group").show();
        $("a", ".btn-group").not(".hd").not(".next").not(".prev").hide();
        $(this).attr("disabled", "disabled");
        $(".btn-group .prev").removeAttr("disabled");
    });

    $(".btn-group .prev").click(function () {
        $("a[class*='hd']", ".btn-group").hide();
        $("a", ".btn-group").not(".hd").not(".next").not(".prev").show();
        $(this).attr("disabled", "disabled");
        $(".btn-group .next").removeAttr("disabled");
    });

    $(".btn-group .btn").not(".next").not(".prev").click(function () {
        $(".btn-group .btn").removeClass("active");
        $(this).addClass("active");
        BindImmunizationType($(this).text(), 1);
        $("#drpImmunizationTypes").html("");
    });

    //BindImmunizationType("A", 1);

});

var $loading = $('#drpImmunizationTypes');
$(document)
  .ajaxStart(function () {
      $loading.hide().after('<div style="text-align: center;"><img src="' + ROOT + 'Images/iloading.gif" /></div>');
  })
  .ajaxStop(function () {
      $loading.show().next().remove();
  });

function incr() {
    i = i + 1;
    return i;
}

function BindImmunizationType(str, resType) {
    var data = JSON.stringify(str);
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Immunization/GetImmunizationTypes",
        data: data,
        dataType: "json",
        success: function (data) {
            var html = [];
            $.each(data, function (key, value) {
                html.push('<option value="' + value.ImmunizationsTypeId + '">' + value.ImmunizationName + '</option>');
            });
            $("#drpImmunizationTypes").html("");
            $("#drpImmunizationTypes").append(html.join(''));

            if (resType == 2) {
                $(".btn-group a", "#frmSelImmunization").removeClass("active");
            }
            else
                $("#txtMasterSrch").val("");
        },
        error: function (result) {

        }
    });
}


function DeleteImmunization(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Immunization/DeleteImmunization",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {

                swal({
                    title: "Success!",
                    text: "Immunization record archived successfully!",
                    type: "success"
                });

                grid.reload();


            }
            else {
                swal({
                    title: "Oops!",
                    text: "Something went wrong! Please try again.",
                    type: "error"
                });

            }
        },
        error: function (result) {
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type: "error"
            });
        }
    });
}

function SaveImmunizationDetails() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveImmunization").val();
    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveImmunization").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    var obj = "{\"__RequestVerificationToken\": '" + antiForgeryToken + "',\"oImmunizationViewModel\": {" + vData + "}}";
    $.ajax({
        type: 'post', url: ROOT + "Immunization/SaveImmunization",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#ImmunizationModal").modal("hide");
            if (result > 0) {
                grid.reload();
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        text: "Immunization saved successfully!",
                        type: "success"
                    });
                });
            }
            else {
                swal({
                    title: "Oops!",
                    text: "Something went wrong! Please try again.",
                    type: "error"
                });
            }
        },
        error: function () {
            $("#ImmunizationModal").modal("hide");
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type: "error"
            });
        }
    });
}

var grid;
function Add() {
    $("#txtMasterSrch").val("");
    $("#txtImmunizationName").val("");
    $("#txtImmunizationDate").val("");
    $("#txtComments").val("");
    $("#frmSelImmunization").show();
    $("#frmSaveImmunization").hide();
    $("#frmShowImmunization").hide();
    $("#drpImmunizationTypes")[0].selectedIndex = -1;
    $("#btnSave").text("Next");
    $("#btnSave").show();
    $('input').removeClass('error');
    $("#frmSelImmunization").validate().resetForm();
    $("#frmSaveImmunization").validate().resetForm();
    BindImmunizationType("A", 1);
    $('.input-group.date').datepicker('update');
    $("#myModalLabel").html("Add Immunization");
    $("#ImmunizationModal").modal("show");
}
function Edit(e) {
    $("#frmSelImmunization").hide();
    $("#frmSaveImmunization").show();
    $("#frmShowImmunization").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Immunization");
    EditImmunization(e.data.id);
}

function Remove(e) {
    swal({
        title: "Are you sure?",
        text: "This Immunization will be archived!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, archive it!"
    },
            function () {
                DeleteImmunization(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {
    $("#frmSelImmunization").hide();
    $("#frmSaveImmunization").hide();
    $("#frmShowImmunization").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Immunization Details");
    ShowImmunizationDetails(e.data.id);
}

function ShowImmunizationDetails(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Immunization/GetImmunizationById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowImmunization").text(ui);
                });
                $("#ImmunizationModal").modal("show");
            }
            else {
                swal({
                    title: "Oops!",
                    text: "We were unable to fetch the details! Please try again.",
                    type: "error"
                });
            }
        },
        error: function (result) {
            swal({
                title: "Oops!",
                text: "We were unable to fetch the details! Please try again.",
                type: "error"
            });
        }
    });
}

function EditImmunization(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Immunization/GetImmunizationById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    //console.log("p==");
                    //console.log(p);
                    //console.log("ui==");
                    //console.log(ui);
                    $("input[datacolumn='" + p + "']", "#frmSaveImmunization").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveImmunization").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveImmunization").val(ui);
                });
                $("#ImmunizationModal").modal("show");
            }
            else {
                swal({
                    title: "Oops!",
                    text: "We were unable to fetch the details! Please try again.",
                    type: "error"
                });
            }
        },
        error: function (result) {
            swal({
                title: "Oops!",
                text: "We were unable to fetch the details! Please try again.",
                type: "error"
            });
        }
    });
}
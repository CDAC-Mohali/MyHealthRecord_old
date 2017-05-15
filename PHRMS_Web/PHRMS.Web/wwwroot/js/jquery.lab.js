var i;
var uploadOk = 0;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "S.No.", field: "sno", width: 50 },
                        //{ title: "Entered By", type: "icon", icon: "glyphicon glyphicon-user", width: 100, align: "center", cssClass: "text-info" },
            { title: "Entered By", width: 100, align: "center", cssClass: "fa fa-user" },
            { field: "PerformedDate", title: " Test Performed Date", tmpl: "{strPerformedDate}", sortable: true },
            { field: "TestName", title: "Test Name", sortable: true },
            { field: "Result", title: "Result", sortable: true },
            { field: "Unit", title: "Unit", sortable: true, width: 100 },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
           // { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
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

    $("#btnAddTest").on("click", AddTest);
    $("#btnSearch").on("click", Search);
    $("#txtMasterSrch").keyup(function () {
        BindLabTestMaster($("#txtMasterSrch").val(), 2);
    });

    $("#frmSaveTest").validate({


        rules:
        {
            TestName:
            {
                required: true
            },
            PresTestId:
            {
                required: true
            },
            TestPerformed:
            {
                required: true
            }
            //Result:
            //{
            //    required: true
            //},
            //Unit:
            //{
            //    required: true
            //}
        },
        messages: {
            TestName: {
                required: "Test Name is required."
            },
            PresTestId: {
                required: "Test Name is required."
            },
            TestPerformed: {
                required: "Test Performed is required."
            }
            //Result: {
            //    required: "Result is required."
            //},
            //Unit: {
            //    required: "Unit is required."
            //}

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
            if ($("#frmSelTest").valid()) {
                $("#frmSelTest").hide();
                $("#txtTestName").val($('#drpTestTypes').find(":selected").text());
                $("input[datacolumn='TestId']").val($("#drpTestTypes").find(":selected").val());
                $("#btnSave").text("Save Test");
                $("#frmSaveTest").show();
            }
        }
        else if ($(this).text() == "Save Test") {
            if ($("#frmSaveTest").valid()) {
                SaveTestDetails();
            }
        }
        else {

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
        BindLabTestMaster($(this).text(), 1);
        $("#drpTestTypes").html("");
    });

    // BindLabTestMaster("A", 1);
});

var $loading = $('#drpTestTypes');
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

function BindLabTestMaster(str, resType) {
    var data = JSON.stringify(str);
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Lab/GetLabTestMaster",
        data: data,
        dataType: "json",
        success: function (data) {
            var html = [];
            $.each(data, function (key, value) {
                html.push('<option value="' + value.Id + '">' + value.TestName + '</option>');
            });
            $("#drpTestTypes").html("");
            $("#drpTestTypes").append(html.join(''));

            if (resType == 2) {
                $(".btn-group a", "#frmSelTest").removeClass("active");
            }
            else
                $("#txtMasterSrch").val("");
        },
        error: function (result) {

        }
    });
}


function DeleteResult(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Lab/DeleteResult",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {
                swal({
                    title: "Success!",
                    text: "Test Result record archived successfully!",
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

function SaveTestDetails() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveTest").val();

    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveTest").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    var vFiles = "[";
    $("#divGallery img", "#frmSaveTest").each(function (idx, ele) {
        vFiles += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("name"));
    });
    vFiles = vFiles + "]";
    vData = vData + ",lstFiles:" + vFiles;
    var obj = "{\"__RequestVerificationToken\": '" + antiForgeryToken + "',\"oMedicationViewModel\": {" + vData + "}}";
    $.ajax({
        type: 'post', url: ROOT + "Lab/SaveTest",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#LabModal").modal("hide");
            if (result > 0) {
                grid.reload();
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        text: "Test saved successfully!",
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
            $("#LabModal").modal("hide");
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type: "error"
            });
        }
    });
}


var grid;
function AddTest() {
    $("#txtMasterSrch").val("");
    $("#txtTestName").val("");
    $("#txtDatePerformed").val("");
    $("#txtResult").val("");
    $("textarea[datacolumn='Comments']").val("");
    $("#divGallery", "#frmSaveTest").html("");
    $("#txtUnit").val("");
    $("#frmSelTest").show();
    $("#frmSaveTest").hide();
    $("#frmShowResult").hide();
    $("#btnSave").text("Next");
    $("#btnSave").show();
    $('input').removeClass('error');
    $("#frmSelTest").validate().resetForm();
    $("#frmSaveTest").validate().resetForm();

    //  $('#frmSelTest').validationEngine('hide');
    //$.validator.messages.required = '';
    //  $('#frmSelTest').validate({});
    //  $(".error").css('display','none');
    $(".btn-group .btn").removeClass("active");
    //   $(".btn-group .btn:first").addClass("active");
    BindLabTestMaster("A", 1);
    $('.input-group.date').datepicker('update');
    $("#myModalLabel").html("Add Test");
    $("#LabModal").modal("show");
}


function Edit(e) {

    $("#frmSelTest").hide();
    $("#frmSaveTest").show();
    $("#frmShowResult").hide();
    $("#btnSave").text("Save Test");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Test Result");
    EditResult(e.data.id);
}

function Remove(e) {
    swal({
        title: "Are you sure?",
        text: "This Lab Test will be archived!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, archive it!"
    },
            function () {
                DeleteResult(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {
    $("#frmSelTest").hide();
    $("#frmSaveTest").hide();
    $("#frmSaveTestResult").hide();
    $("#frmShowResult").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Lab Test Result Details");
    ShowResultDetails(e.data.id);
}

function ShowResultDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Lab/GetResultById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowResult").text(ui);
                });
                $("#divGallery", "#frmShowResult").html("");
                $("#divDigi", "#frmShowResult").html("");
                if (data.lstFileModels != null && data.lstFileModels !== undefined) {
                    $(".locker-saver", "#divDigi").each(function () {
                        //alert();
                        $(this).remove();
                    });
                 
                    $(data.lstFileModels).each(function (idx, ele) {
                        if (ele.FileName.indexOf(".pdf") >= 0) {

                            $("#divURL").attr("href", "Images//LabReports//" + ele.FileName);
                            $("#divGallery").hide();
                            $("#divURL").show();
                          
                        }
                        else {
                            $("#divURL").hide();
                            $("#divGallery").show();
                            $("#divGallery", "#frmShowResult").append("<div><a href='" + ROOT + "Images\\LabReports\\" + ele.FileName + "' data-gallery=''><img name='" + ele.FileName + "' src='" + ROOT + "Images\\LabReports\\" + ele.FileName + "'></a></div>");
                        }
                        $("#divDigi", "#frmShowResult").append("<div style='margin-top: 64px;'><a id='share_id' href=" + BasePath + 'Images/LabReports/' + ele.FileName + " class='locker_saver_sm'></a></div>");
                    });
                    
                    $('script[src="https://devservices.digitallocker.gov.in/savelocker/api/1/savelocker.js"]').remove();
                    $('script[src="https://devservices.digitallocker.gov.in/requester/api/1/dl.js"]').remove();
                    SetTimeStamp();
                }
                $("#LabModal").modal("show");
                //alert($(".dl_pop").html());
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

function EditResult(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Lab/GetResultById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveTest").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveTest").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveTest").val(ui);
                });
                $("#LabModal").modal("show");
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

function fileupdate() {
    var x = document.getElementById("txtFileName");
    var txt = "";
    if ('files' in x) {
        if (x.files.length == 0) {
            txt = "Select one file.";
        } else {
            for (var i = 0; i < x.files.length; i++) {
                var file = x.files[i];
                if (file.type == "image/jpeg" || file.type == "image/gif" || file.type == "image/png") {
                    uploadOk = 1;
                }
                else {
                    uploadOk = 0;
                    txt = "Only jpeg, jpg, gif and png formats are alowed!";
                }
            }
        }
    }
    else {
        if (x.value == "") {
            txt += "Select one or more files.";
        } else {
            txt += "The files property is not supported by your browser!";
            txt += "<br>The path of the selected file: " + x.value; // If the browser does not support the files property, it will return the path of the selected file instead.
        }
    }

    if (uploadOk == 0) {
        swal({
            title: "Note!",
            text: txt,
            type: "warning"
        });
        $('#txtFileName').val('');
    }
}

function SaveFiles() {
    var file = document.getElementById("txtFileName");
    if (uploadOk == 1) {
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4) {
                status = xhr.status;
                data = JSON.parse(xhr.responseText);
                if (status == 200) {
                    swal({
                        title: "Success!",
                        text: "Image Uploaded Successfully!",
                        type: "success"
                    });
                    $("#txtFileName").val("");
                    $("#divGallery", "#frmSaveTest").append("<a href='javascript:void(0)'><img name='" + data.name + "' src='" + ROOT + data.path + "'></a>");
                }
                else {
                    swal({
                        title: "Oops!",
                        text: "Failed to upload image!",
                        type: "error"
                    });
                }
            }
        }
        xhr.open("POST", ROOT + 'Lab/UploadFile', true);
        xhr.setRequestHeader("X_FILENAME", file.files[0].name);
        xhr.setRequestHeader("X_DIRECTORY", "LabReports");
        xhr.send(file.files[0]);
    }
}


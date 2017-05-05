var i;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "Sno", field: "sno", width: 50 },
             //{ title: "Entered By", type: "icon", icon: "glyphicon glyphicon-user", width: 100, align: "center", cssClass: "text-info" },
             { title: "Entered By", width: 100, align: "center", cssClass: "fa fa-user" },
           { field: "EndDate", title: "Date of Procedure", tmpl: "{strEndDate}", sortable: true, width: 100 },
            { field: "ProcedureName", title: "Procedure Name", sortable: true },
            { field: "SurgeonName", title: "Diagnosed by Doctor/Hospital", width: 200, sortable: true },
            { title: "View", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
            /*{
                title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-download-alt", tooltip: "View", events: {
                    "click": function (e) {
                        GetFirstAttachment(6, e.data.id);
                    }
                }
            },*/
      //      { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
            { title: "Archive", field: "Delete", width: 34, type: "icon", icon: "glyphicon-off", tooltip: "Archive", events: { "click": Remove } }
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

    $("#frmSaveProcedure").validate({
        rules:
            {
                EndedOn:
                    {
                        required:
                            true

                    }

            },
        messages:
        {
            Result:
                {
                    EndedOn:
                        "Ended On is required."

                }

        },
        submitHandler: function (form) {
            form.submit();
        },
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent());
        }
    });



    $("#btnAddProcedure").on("click", Add);
    $("#btnSearch").on("click", Search);

    $("#txtMasterSrch").keyup(function () {
        BindProcedureMaster($("#txtMasterSrch").val(), 2);
    });

    $("#btnSave").click(function () {
        if ($(this).text() == "Next") {
            if ($("#frmSelProcedure").valid()) {
                $("#frmSelProcedure").hide();
                $("#txtProcedureName").val($('#drpProcedureTypes').find(":selected").text());
                $("input[datacolumn='ProcedureType']").val($("#drpProcedureTypes").val());
                $("#divGallery").html("");
                $("#frmSaveProcedure").show();
                $(this).text("Save");
            }
        }
        else {

            if ($("#txtFileName").val != null && $("#txtFileName").val != "" && $("#txtFileName").val != undefined && $("#btnSave").hasClass("upload")) {
                SaveFiles();
            }
            else if ($("#frmSaveProcedure").valid()) {
                SaveProcedureDetails();
            }
        }
    });


    $('.datepicker').datepicker({
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
        BindProcedureMaster($(this).text(), 1);
        $("#drpProcedureTypes").html("");
    });

    //BindProcedureMaster("A", 1);
});


var $loading = $('#drpProcedureTypes');
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
function BindProcedureMaster(str, resType) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Procedures/GetProcedureMaster",
        data: JSON.stringify(str),
        dataType: "json",
        success: function (data) {
            var html = [];
            $.each(data, function (key, value) {
                html.push('<option value="' + value.Id + '">' + value.ProcedureName + '</option>');
            });
            $("#drpProcedureTypes").html("");
            $("#drpProcedureTypes").append(html.join(''));

            if (resType == 2) {
                $(".btn-group a", "#frmSelProcedure").removeClass("active");
            }
            else
                $("#txtMasterSrch").val("");
        },
        error: function (result) {

        }
    });
}
function DeleteAllergy(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Procedures/DeleteProcedure",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {
                swal({
                    title: "Success!",
                    text: "Procedure archived successfully!",
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

function SaveProcedureDetails() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveProcedure").val();
    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveProcedure").not("#txtProcedureName").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    var vFiles = "[";
    $("#divGallery img", "#frmSaveProcedure").each(function (idx, ele) {
        vFiles += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("name"));
    });
    vFiles = vFiles + "]";
    vData = vData + ",lstFiles:" + vFiles;
    $.ajax({
        type: 'post', url: ROOT + "Procedures/SaveProcedure",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#procedureModal").modal("hide");
            if (result > 0) {
                grid.reload();
                swal({
                    title: "Success!",
                    text: "Procedure saved successfully!",
                    type: "success"
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
            $("#procedureModal").modal("hide");
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
    $("#playerId").val("");
    $("#name").val("");
    $("#placeOfBirth").val("");
    $("#dateOfBirth").val("");
    $("#txtMasterSrch").val("");
    $("#drpProcedureTypes").children('option').show();
    $("#frmSelProcedure").show();
    $("#frmSaveProcedure").hide();
    $("#frmShowProcedure").hide();
    $("#drpProcedureTypes")[0].selectedIndex = -1;
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $("input[type='text']", "#frmSaveProcedure").not(".Static").val("");
    $("#btnSave").text("Next");
    $("textarea[datacolumn='Comments']").val("");
    $("#txtFileName").val("");

    $("#btnSave").show();
    $('input').removeClass('error');
    $("#frmSelProcedure").validate().resetForm();
    $("#frmSaveProcedure").validate().resetForm();
    BindProcedureMaster("A", 1);
    $('.datepicker').datepicker('update');
    $("#myModalLabel").html("Add Procedure");
    $("#procedureModal").modal("show");
}
function Edit(e) {
    $("#frmSelProcedure").hide();
    $("#frmSaveProcedure").show();
    $("#frmShowProcedure").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Procedure");
    EditAllergy(e.data.id);
}

function Remove(e) {
    swal({
        title: "Are you sure?",
        text: "This procedure will be archived!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, archive it!"
    },
            function () {
                DeleteAllergy(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {
    $("#frmSelProcedure").hide();
    $("#frmSaveProcedure").hide();
    $("#frmShowProcedure").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Procedure Details");
    ShowAllergyDetails(e.data.id);
}



function ShowAllergyDetails(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Procedures/GetProcedureById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowProcedure").text(ui);
                });
                $("#divGallery", "#frmShowProcedure").html("");
                $("#divDigi", "#frmShowProcedure").html("");
                if (data.lstFileModels != null && data.lstFileModels !== undefined) {
                    $(".locker-saver", "#divDigi").each(function () {
                        //alert();
                        $(this).remove();
                    });
                    $(data.lstFileModels).each(function (idx, ele) {
                        if (ele.FileName.indexOf(".pdf") >= 0) {

                            $("#divURL").attr("href", "Images//Procedures//" + ele.FileName);
                            $("#divGallery").hide();
                            $("#divURL").show();
                        }
                        else {
                            $("#divURL").hide();
                            $("#divGallery").show();
                            $("#divGallery", "#frmShowProcedure").append("<a href='" + ROOT + "Images\\Procedures\\" + ele.FileName + "' data-gallery=''><img name='" + ele.FileName + "' src='" + ROOT + "Images\\Procedures\\" + ele.FileName + "'></a>");
                        }
                        $("#divDigi", "#frmShowProcedure").append("<div style='margin-top: 64px;'><a id='share_id' href=" + BasePath + 'Images/Procedures/' + ele.FileName + " class='locker_saver_sm'></a></div>");

                    });
                    $('script[src="https://devservices.digitallocker.gov.in/savelocker/api/1/savelocker.js"]').remove();
                    $('script[src="https://devservices.digitallocker.gov.in/requester/api/1/dl.js"]').remove();
                    SetTimeStamp();
                }
                $("#procedureModal").modal("show");
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

function EditAllergy(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Procedures/GetProcedureById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveProcedure").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveProcedure").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveProcedure").val(ui);
                });
                $("#procedureModal").modal("show");
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

var uploadOk = 0;

function fileupdate() {
    var x = document.getElementById("txtFileName");
    var txt = "";
    if ('files' in x) {
        if (x.files.length == 0) {
            txt = "Select one file.";
        } else {
            for (var i = 0; i < x.files.length; i++) {
                var file = x.files[i];
                if (file.type == "image/jpeg" || file.type == "image/gif" || file.type == "image/png" || file.type == "application/pdf") {
                    uploadOk = 1;
                }
                else {
                    uploadOk = 0;
                    txt = "Only ppdf,jpeg, jpg, gif and png formats are alowed!";
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
                    $("#divGallery", "#frmSaveProcedure").append("<a href='javascript:void(0)'><img name='" + data.name + "' src='" + ROOT + data.path + "'></a>");
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
        xhr.open("POST", ROOT + 'Procedures/UploadFile', true);
        xhr.setRequestHeader("X_FILENAME", file.files[0].name);
        xhr.setRequestHeader("X_DIRECTORY", "Procedures");
        xhr.send(file.files[0]);
    }
}

var i;
var jqXHRData;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "S.No.", field: "sno", width: 50 },
           // { title: "Entered By", type: "icon", icon: "glyphicon glyphicon-user", width: 100, align: "center", cssClass: "text-info" },
           { title: "Entered By", width: 100, align: "center", cssClass: "fa fa-user" },
           { field: "PresDate", title: "Prescription Date", tmpl: "{strPresDate}", sortable: true, width: 150 },
           { field: "DocName", title: "Doctor's Name", sortable: true },
            { field: "ClinicName", title: "Hospital/Clinic's Name", sortable: true },
            { field: "Prescription", title: "Prescription Details", sortable: true },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
       //     { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
            { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-off", tooltip: "Archive", events: { "click": Remove } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });

    grid.on('rowDataBound', function (e, $row, id, record) {
        if (record.SourceId == 2) {
            $(".fa-user", $row).attr("class", "fa fa-user-md text-info");
        }
        else if (record.SourceId == 5) {
            $(".fa-user", $row).attr("class", "fa fa-user-md text-success");
        }
        else {
            $(".fa-user-md", $row).attr("class", "fa fa-user");
        }
        //alert($row.html());
    });

    $("#btnAddPlayer").on("click", Add);
    $("#btnSearch").on("click", Search);
    $("#frmSavePrescription").validate({
        rules:
        {
            DocName:
            {
                required: true
            },
            ClinicName:
                {
                    required: true
                },
            Address:
                {
                    required: true
                },
            DocPhone:
               {
                   required: true,
                   number: true
               },
            PrescriptionDate:
                {
                    required: true
                }

        },
        messages: {
            DocName: {
                required: "Doctor Name is required"
            },
            ClinicName: {
                required: "Clinic Name is required."
            },
            Address:
            {
                required: "Address is required."
            },
            DocPhone:
            {
                required: "Phone Number is required.",
                number: "Invalid phone number."
            },
            PrescriptionDate:
            {
                required: "Prescription Date is required."
            }

        },
        submitHandler: function (form) {
            form.submit();
        },
        errorPlacement: function (error, element) {
            if (element.attr("name") == "DocName")
                error.insertAfter(element);
            else if (element.attr("name") == "ClinicName")
                error.insertAfter(element);
            else if (element.attr("name") == "Prescription")
                error.insertAfter(element);
            else
                error.insertAfter(element.parent());
        }




    });





    $("#btnSave").click(function () {

        if ($("#frmSavePrescription").valid()) {
            SavePrescription();
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

});



function incr() {
    i = i + 1;
    return i;
}

function DeletePrescription(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Eprescription/DeletePrescription",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        text: "Prescription record archived successfully!",
                        type: "success"
                    },
                    function () {
                        grid.reload();
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
        error: function (result) {
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type: "error"
            });
        }
    });
}
function SavePrescription() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSavePrescription").val();
    var vData = "";
    $("input[type='text'],textarea,select", "#frmSavePrescription").not("#txtPrescriptionName").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    var vFiles = "[";
    $("#divGallery img", "#frmSavePrescription").each(function (idx, ele) {
        vFiles += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("name"));
    });
    vFiles = vFiles + "]";
    vData = vData + ',"lstFiles":' + vFiles;
    var obj = "{\"__RequestVerificationToken\": '" + antiForgeryToken + "',\"oPrescriptionViewModel\": {" + vData + "}}";
    $.ajax({
        type: 'post', url: ROOT + "Eprescription/SavePrescription",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#PrescriptionModal").modal("hide");
            if (result > 0) {
                grid.reload();
                swal({
                    title: "Success!",
                    text: "Prescription saved successfully!",
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
            $("#PrescriptionModal").modal("hide");
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type: "error"
            });
        }
    });
}

var grid;
function getAge(dateString) {
    var today = new Date();
    var birthDate = new Date(dateString);
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }
    return age;
}

function Add() {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Eprescription/GetPersonById",
        success: function (data) {
            var age = getAge(data.DOB);
            data['PatientAge'] = age;
            data['PatientName'] = data.FirstName + ' ' + data.LastName;
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmSavePrescription").text(ui);
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
    $('input').removeClass('error');
    $("#frmSavePrescription").validate().resetForm();
    $("#txtDoctorName").val("");
    $("#txtClinicName").val("");
    $("#txtAddress").val("");
    $("#txtPhone").val("");
    $("#txtPrescription").val("");
    $("#txtPrescDate").val("");
    $("#divGallery", "#frmSavePrescription").html("");
    $("#frmSavePrescription").show();
    $("#frmShowPrescription").hide();
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $("input[type='text']", "#frmSavePrescription").not(".Static").val("");
    $("#btnSave").text("Save");
    $('.input-group.date').datepicker('update');
    $("#btnSave").show();
    $("#myModalLabel").html("Add Prescription");
    $("#PrescriptionModal").modal("show");
}
function Edit(e) {
    $("#frmSelPrescription").hide();
    $("#frmSavePrescription").show();
    $("#frmShowPrescription").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Prescription");
    EditPrescription(e.data.id);
}

function Remove(e) {
    swal({
        title: "Are you sure?",
        text: "This Prescription will be archived!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, archive it!"
    },
            function () {
                DeletePrescription(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {
    $("#frmSavePrescription").hide();
    $("#frmShowPrescription").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Prescription Details");
    ShowPrescriptionDetails(e.data.id);
}

function ShowPrescriptionDetails(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Eprescription/GetPrescriptionById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {

                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowPrescription").text(ui);
                });
                $("#divGallery", "#frmShowPrescription").html("");
                if (data.lstFileModels != null && data.lstFileModels !== undefined) {

                    $(data.lstFileModels).each(function (idx, ele) {
                        $("#divGallery", "#frmShowPrescription").append("<a href='" + ROOT + "Images//ePrescription//" + ele.FileName + "' data-gallery=''><img name='" + ele.FileName + "' src='" + ROOT + "Images//ePrescription//" + ele.FileName + "'></a>");
                    });

                }

                if (data.SourceId == 2) {
                    $(".URL").hide();
                    $(".attachment").hide();
                    $('.Pattachment').show();
                    $("#divURL").show();
                    $("#divURL").attr("href", "/Eprescription/ViewDetail?EPrescriptionId=" + data.Id);
                }
                else if (data.SourceId == 5) {
                    $(".URL").show();
                    $(".attachment").hide();
                    $('.Pattachment').hide();
                    //  $("#divURL").text("/Eprescription/ShowReport?EPrescriptionId=" + data.Id);
                    $("#divURL").attr("href", "/Eprescription/ShowReport?EPrescriptionId=" + data.Id);
                }
                else if (data.SourceId == 100030) {
                    if (data.lstFileModels != null && data.lstFileModels !== undefined) {
                        $(".URL").show();
                        $(".attachment").hide();
                        $('.Pattachment').hide();
                        //  $("#divURL").text("/Eprescription/ShowReport?EPrescriptionId=" + data.Id);
                        //   $("#divURL").attr("href", "/Eprescription/ShowReport?EPrescriptionId=" + data.Id);
                        $(data.lstFileModels).each(function (idx, ele) {
                            $("#divURL").attr("href", "Images//ePrescription//" + ele.FileName);
                            //  $("#divGallery", "#frmShowPrescription").append("<a href='" + ROOT + "Images//ePrescription//" + ele.FileName + "' data-gallery=''><img name='" + ele.FileName + "' src='" + ROOT + "Images//ePrescription//" + ele.FileName + "'></a>");
                        });
                    }
                }
                else {
                    $(".URL").hide();
                    $(".attachment").show();
                    $('.Pattachment').hide();
                }
                $("#PrescriptionModal").modal("show");
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

function EditPrescription(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Eprescription/GetPrescriptionById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSavePrescription").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSavePrescription").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSavePrescription").val(ui);
                });
                $("#PrescriptionModal").modal("show");
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

function fileupdatelegacy() {

    var x = document.getElementById("txtFileName");
    var txt = "";
    if ('files' in x) {
        if (x.files.length == 0) {
            txt = "Select one file.";
        } else {
            for (var i = 0; i < x.files.length; i++) {
                txt += "<br><strong>" + (i + 1) + ". file</strong><br>";
                var file = x.files[i];
                if ('name' in file) {
                    txt += "name: " + file.name + "<br>";
                }
                if ('size' in file) {
                    txt += "size: " + file.size + " bytes <br>";
                }
                if (file.type == "image/jpeg" || file.type == "image/png") {
                    uploadOk = 1;
                }
                else {
                    uploadOk = 0;
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
    document.getElementById("demo").innerHTML = txt;
    $("#btnSave").addClass("upload");

}



function SaveFileslegacy() {
    var file = document.getElementById("txtFileName")
    if (uploadOk == 1) {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", '/Eprescription/UploadFile', true);
        xhr.setRequestHeader("X_FILENAME", file.files[0].name);
        xhr.send(file.files[0]);

        swal({
            title: "Success!",
            text: "Image Uploaded Successfully!",
            type: "success"
        });
        uploadOk = 0;
        $("#PrescriptionModal").hide();
    }
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
                if (file.type == "image/jpeg" || file.type == "image/gif" || file.type == "image/png" || file.type == "application/pdf") {
                    uploadOk = 1;
                }
                else {
                    uploadOk = 0;
                    txt = "Only pdf,jpeg, jpg, gif and png formats are alowed!";
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
                    $("#divGallery", "#frmSavePrescription").append("<a href='javascript:void(0)'><img name='" + data.name + "' src='" + ROOT + data.path + "'></a>");
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
        xhr.setRequestHeader("X_DIRECTORY", "ePrescription");
        xhr.send(file.files[0]);
    }
}
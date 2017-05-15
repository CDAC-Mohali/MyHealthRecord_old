var i, skip = 0;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "S.No.", field: "sno", width: 50 },
                        //{ title: "Entered By", type: "icon", icon: "glyphicon glyphicon-user", width: 100, align: "center", cssClass: "text-info" },
                        { title: "Entered By", width: 100, align: "center", cssClass: "fa fa-user" },
                                    { field: "PrescribedDate", title: "Prescribed Date", tmpl: "{strPrescribedDate}", sortable: true, width: 150 },
            { field: "MedicineName", title: "Medication Name", sortable: true },
            { field: "TakingMedicine", tmpl: "{strTakingMedicine}", title: "Still Taking Medication?", sortable: true, width: 200 },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
         //   { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
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
        BindMedicationType($("#txtMasterSrch").val(), 2);
    });

    $("#frmSaveMedication").validate({
        rules:
        {
            MedicineName:
            {
                required: true
            },
            DatePrescribed:
            {
                required: true
            },
            DateDispensed:
            {
                required: true
            },
            Provider:
            {
                required: true
            },
            Route:
            {
                required: true
            },

            Strength:
           {
               required: true,
               number: true
           },
            DosageValue:
                {
                    required: true
                },
            DosageUnit:
           {
               required: true
           },
            Frequency:
           {
               required: true
           }

        },
        messages: {
            MedicineName: {
                required: "Name is required."
            },
            DatePrescribed: {
                required: "Prescription Date is required."
            },
            DateDispensed: {
                required: "Dispensation Date is required."
            },
            Provider: {
                required: "Provider is required."
            },
            Route: {
                required: "Route is required."
            },
            Strength: {
                required: "Strength is required."
            },
            DosageUnit: {
                required: "Dosage unit is required."
            },
            DosageValue: {
                required: "Dosage Value is required"
            },
            Frequency: {
                required: "Frequency is required."
            },


            Strength: {
                required: "Strength must be Numeric."
            },

        },
        submitHandler: function (form) {
            form.submit();
        },
        errorPlacement: function (error, element) {
            if (element.attr("name") == "Strength") {
                error.insertAfter(element);
            }
            else {
                error.insertAfter(element.parent());
            }

        }
    });
    $("#btnSave").click(function () {
        if ($(this).text() == "Next") {
            if ($("#frmSelMedication").valid()) {
                $("#frmSelMedication").hide();

                $("#txtMedicationName").val($('#drpMedicationTypes').find(":selected").text());
                $("input[datacolumn='MedicineType']").val($("#drpMedicationTypes").find(":selected").val());
                $("#frmSaveMedication").show();
                $(this).text("Save");
            }
        }
        else {
            if ($("#frmSaveMedication").valid()) {
                SaveMedicationDetails();
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
        skip = 0;
        $("#txtMasterSrch").val('')
        BindMedicationType($(this).text(), 1);
        $("#drpMedicationTypes").html("");
    });

    BindMedicationRoute();
    BindDosageValue();
    BindDosageUnit();
    BindFrequency();

    // BindMedicationType("A", 1);
});

var $loading = $('#drpMedicationTypes');
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
function GetLimitedRecords() {
    // $("a", "#frmSelMedication").click(function () {
    skip = skip + 100;

    BindMedicationType($(".active", "#frmSelMedication").text(), 1);

    //   });
}


function BindMedicationType(str, resType) {
    if ($("#txtMasterSrch").val() != "") {
        str = $("#txtMasterSrch").val();
    }
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Medication/GetMedicationMaster",
        data: JSON.stringify(str + "," + skip),
        dataType: "json",
        success: function (data) {
            var html = [];
            $.each(data, function (key, value) {
                html.push('<option value="' + value.Id + '">' + value.MedicineName + '</option>');
            });
            $("#drpMedicationTypes").html("");
            $("#drpMedicationTypes").append(html.join(''));

            //if (resType == 2) {
            //    //   $(".btn-group a", "#frmSelMedication").removeClass("active");
            //}
            //else
            //    $("#txtMasterSrch").val("");
        },
        error: function (result) {

        }
    });
}

function BindMedicationRoute() {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Medication/GetRoutes",
        data: "{}",
        dataType: "json",
        success: function (data) {
            $.each(data, function (key, value) {
                $("#drpRoute").append($("<option></option>").val(value.Id).html(value.Route));
            });
        },
        error: function (result) {

        }
    });
}


function BindDosageValue() {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Medication/GetDosageValues",
        data: "{}",
        dataType: "json",
        success: function (data) {
            $.each(data, function (key, value) {
                $("#drpDosageValue").append($("<option></option>").val(value.Id).html(value.DosValue));

            });
        },
        error: function (result) {

        }
    });
}

function BindDosageUnit() {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Medication/GetDosageUnits",
        data: "{}",
        dataType: "json",
        success: function (data) {
            $.each(data, function (key, value) {
                $("#drpDosageUnit").append($("<option></option>").val(value.Id).html(value.DosUnit));
            });

        },
        error: function (result) {

        }
    });
}


function BindFrequency() {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Medication/GetFrequencies",
        data: "{}",
        dataType: "json",
        success: function (data) {
            $.each(data, function (key, value) {
                $("#drpFrequency").append($("<option></option>").val(value.Id).html(value.Frequency));
            });
        },
        error: function (result) {

        }
    });
}



function DeleteMedicine(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Medication/DeleteMedicine",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {

                swal({
                    title: "Success!",
                    text: "Medication record archieved successfully!",
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

function SaveMedicationDetails() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveMedication").val();

    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveMedication").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    vData += ",\"TakingMedicine\":\"" + $("#rdoStillHaveYes").is(":checked") + "\"";
    var vFiles = "[";
    $("#divGallery img", "#frmSaveMedication").each(function (idx, ele) {
        vFiles += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("name"));
    });
    vFiles = vFiles + "]";
    vData = vData + ",lstFiles:" + vFiles;
    var obj = "{\"__RequestVerificationToken\": '" + antiForgeryToken + "',\"oMedicationViewModel\": {" + vData + "}}";
    $.ajax({
        type: 'post', url: ROOT + "Medication/SaveMedication",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#medicationModal").modal("hide");
            if (result > 0) {
                grid.reload();
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        text: "Medication saved successfully!",
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
            $("#MedicationModal").modal("hide");
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
    $("#Id").val("");
    $("#txtMedicineName").val("");
    $("#rdoStillHaveYes").click();
    $("#txtDatePrescribed").val("");
    $("#txtMasterSrch").val("");
    $("#txtDateDispensed").val("");
    $("#txtProvider").val("");
    $("input[type='text']").val("");
    $("textarea[datacolumn='LabelInstructions']").val("");
    $("textarea[datacolumn='Notes']").val("");
    $("textarea").text("");
    $("#frmSaveMedication select").find('option:eq(0)').prop('selected', true);
    $("#divGallery", "#frmSaveMedication").html("");
    $("#drpRoute").children('option').show();
    $("#drpDosageValue").children('option').show();
    $("#drpDosageUnit").children('option').show();
    $("#drpFrequency").children('option').show();
    $("#frmSelMedication").show();
    $("#frmSaveMedication").hide();
    $("#frmShowMedication").hide();
    $("#drpMedicationTypes")[0].selectedIndex = -1;
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $("#btnSave").text("Next");
    $("#btnSave").show();
    $('input').removeClass('error');
    $("#frmSelMedication").validate().resetForm();
    $("#frmSaveMedication").validate().resetForm();
    BindMedicationType("A", 1);
    $('.datepicker').datepicker('update');
    $("#myModalLabel").html("Add Medication");
    $("#medicationModal").modal("show");
}
function Edit(e) {

    $("#frmSelMedication").hide();
    $("#frmSaveMedication").show();
    $("#frmShowMedication").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Medication");
    EditMedication(e.data.id);
}

function Remove(e) {
    swal({
        title: "Are you sure?",
        text: "This Medication will be archived!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, archive it!"
    },
            function () {
                DeleteMedicine(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {
    $("#frmSelMedication").hide();
    $("#frmSaveMedication").hide();
    $("#frmShowMedication").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Medication Details");
    ShowMedicationDetails(e.data.id);
}

function ShowMedicationDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Medication/GetMedicineById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowMedication").text(ui);
                });
                $("#divGallery", "#frmShowMedication").html("");
                $("#divDigi", "#frmShowMedication").html("");
                if (data.lstFileModels != null && data.lstFileModels !== undefined) {
                    $(".locker-saver", "#divDigi").each(function () {
                        //alert();
                        $(this).remove();
                    });
                    $(data.lstFileModels).each(function (idx, ele) {
                     
                        if (ele.FileName.indexOf(".pdf") >= 0) {
                           
                            $("#divURL").attr("href", "Images//Medidocs//" + ele.FileName);
                            $("#divGallery").hide();
                            $("#divURL").show();
                        }
                        else {
                            $("#divURL").hide();
                            $("#divGallery").show();
                            $("#divGallery", "#frmShowMedication").append("<a href='" + ROOT + "Images\\Medidocs\\" + ele.FileName + "' data-gallery=''><img name='" + ele.FileName + "' src='" + ROOT + "Images\\Medidocs\\" + ele.FileName + "'></a>");
                        }
                        $("#divDigi", "#frmShowMedication").append("<div style='margin-top: 64px;'><a id='share_id' href=" + BasePath + 'Images/Medidocs/' + ele.FileName + " class='locker_saver_sm'></a></div>");

                    });
                    $('script[src="https://devservices.digitallocker.gov.in/savelocker/api/1/savelocker.js"]').remove();
                    $('script[src="https://devservices.digitallocker.gov.in/requester/api/1/dl.js"]').remove();
                    SetTimeStamp();
                }
                $("#medicationModal").modal("show");
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

function EditMedication(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Medication/GetMedicineById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveMedication").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveMedication").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveMedication").val(ui);
                });
                $("#medicationModal").modal("show");
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
                    $("#divGallery", "#frmSaveMedication").append("<a href='javascript:void(0)'><img name='" + data.name + "' src='" + ROOT + data.path + "'></a>");
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
        xhr.setRequestHeader("X_DIRECTORY", "Medidocs");
        xhr.send(file.files[0]);
    }
}
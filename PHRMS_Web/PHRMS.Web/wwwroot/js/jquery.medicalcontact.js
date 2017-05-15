var i;
var uploadOk = 0;
var validator;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "S.No.", field: "sno", width: 50 },
            { field: "ContactName", title: "Contact Name", sortable: true },
            { field: "MedContType", title: "Speciality", tmpl: "{strMedContType}", sortable: true },
            { field: "PrimaryPhone", title: "Mobile", sortable: true },
              { field: "EmailAddress", title: "Email Address", sortable: true },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
            { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
            { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": Remove } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });


    $("#btnAddContact").on("click", AddContact);
    $("#btnSearch").on("click", Search);
    $("#txtMasterSrch").keyup(function () {

    });

    validator = $("#frmSaveContact").validate({


        rules:
        {
            ContactName:
            {
                required: true,
                lettersonly: true,
           },
            ClinicName:
                {
                    required: true,

                },
            MedContType:
            {
                required: true,
                min: 1
            },
            PrimaryPhone:
            {
                required: true,
                number: true,
                minlength: 10,
            },
            PIN:
            {
                number: true,
                minlength: 6,
            },
            EmailAddress:
             {
                 required: true,
                 //    EmailAddress: true
             },
            State:
           {
               required: true,
               //    EmailAddress: true
           },
        },
        messages: {
            ContactName: {
                required: "Contact Name is required",
                lettersonly: "Please enter only alphbets"
            },
            MedContType: {
                required: "Contact Type is required.",
                min: "Please select the Type"
            },
            PrimaryPhone: {
                required: "Primary Phone is Required",
                minlength: "Please check the phone number"
            },

            ClinicName:{
                required: "Hospital/Clinic Name is Required",
                
            },

            EmailAddress: {
                required: "Email Address is Required",
                //    EmailAddress: "Please check the Email Address"
            },
        },
        submitHandler: function (form) {
            form.submit();
        }//,
        //errorPlacement: function (error, element) {
        //    error.insertAfter(element.parent());
        //}
    });

    jQuery.validator.addMethod("lettersonly", function (value, element) {
        return this.optional(element) || /^[a-z\s]+$/i.test(value);
    }, "Only alphabetical characters");

    $("#btnSave").click(function () {

      
        if ($(this).text() == "Save") {
            if ($("#frmSaveContact").valid()) {
               
                if ($('#FormType').val() == "ADD") {               
               
                  
                    $.ajax({
                        type: "POST",
                        url: "/Account/DoesMobileExistMedical",
                        data: '{"PrimaryPhone":' + '"'+ $("#txtPrimaryPhone").val() + '"' + ',"EmailAddress":' + '"' + $("input[datacolumn='EmailAddress']").val() + '"' + "}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                 
                        
                            if (data == 0) {


                                SaveTestDetails();

                            } else if (data == 1) {
                                swal({
                                    title: "Oops!",
                                    text: "Mobile number or email address already exists.",
                                    type: "error"
                                });

                            }

                        },
                        error: function (result) {
                            swal({
                                title: "Oops!",
                                text: "Mobile number or email address already exists.",
                                type: "error"
                            });

                        }
                    });
                } else {
                    SaveTestDetails();
                }




            }
        }
        else {
            SaveTestDetails();
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



function DeleteResult(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "MedicalContactRecords/DeleteContact",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {
                swal({
                    title: "Success!",
                    text: "Record deleted successfully!",
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

   
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveContact").val();

    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveContact").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });

    var obj = "{\"__RequestVerificationToken\": '" + antiForgeryToken + "',\"oMedicationViewModel\": {" + vData + "}}";


    $.ajax({
        type: 'post', url: ROOT + "MedicalContactRecords/SaveContacts",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#MedicalContactModal").modal("hide");
            if (result > 0) {
                grid.reload();
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        text: "Contact saved successfully!",
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
            $("#MedicalContactModal").modal("hide");
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type: "error"
            });
        }
    });

  




}


var grid;
function AddContact() {
    $('#FormType').val('ADD')
    $("#txtPrimaryPhone").removeAttr("disabled");
    $("#txtEmailAddress").removeAttr("disabled");
    $("#txtContactName").val("");
    $('#txtContactType').prop('selectedIndex', 0);
    $("#txtPrimaryPhone").val("");
    $("#frmSaveContact").show();
    $("#frmShowContact").hide();
    $("#btnSave").show();
    $("#myModalLabel").html("Add Medical Contact");
    $("#MedicalContactModal").modal("show");
    $("input[type='text']", "#frmSaveContact").removeClass("error");
    validator.resetForm();
}


function Edit(e) {

    $("#txtPrimaryPhone").attr("disabled", "disabled");
    $("#txtEmailAddress").attr("disabled", "disabled");
    $('#FormType').val('EDIT')
    $("#frmSaveContact").show();
    $("#frmShowContact").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Medical Contact Details");
    EditResult(e.data.id);
}

function Remove(e) {
    swal({
        title: "Are you sure?",
        text: "You will loose all information associated with this Test Result!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!"
    },
            function () {
                DeleteResult(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {

    $("#frmSaveContact").hide();
    $("#frmShowContact").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Contact Details");
    ShowResultDetails(e.data.id);
}

function ShowResultDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "MedicalContactRecords/GetContactById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowContact").text(ui);
                });
                $("#divGallery", "#frmShowContact").html("");

                $("#MedicalContactModal").modal("show");
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
        url: ROOT + "MedicalContactRecords/GetContactById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveContact").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveContact").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveContact").val(ui);
                });
                $("#MedicalContactModal").modal("show");
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
                    $("#divGallery", "#frmSaveContact").append("<a href='javascript:void(0)'><img name='" + data.name + "' src='" + ROOT + data.path + "'></a>");
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
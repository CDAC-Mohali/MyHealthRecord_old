var i;
var uploadOk = 0;
var validator;
var dataCity;
$("#txtDistrictName").change(function () {

    dataCity = $("#txtDistrictName :selected").text();
    grid.destroy();
    grid = $("#grid").grid({
        dataKey: "City",
        
        uiLibrary: "bootstrap",
        dataSource: { url: ROOT + "OpenEMR/GetOpenEMRGridList?city=" + dataCity },
        columns: [
            { title: "S.No.", field: "sno",width:60},
            { field: "Hospital_name", title: "Hospital Name", sortable: true},
            { field: "Hospital_address", title: "Hospital Address", sortable: true },
            { field: "Hospital_contact", title: "Hospital Contact", sortable: true},
            { field: "Admin_contact", title: "Admin Contact", sortable: true },
            { field: "Reg_Date", title: "Registration Date", sortable: true},
            { title: "", field: "Edit", type: "icon", icon: "glyphicon-download",width: 45, tooltip: "Sync", events: { "click": Edit } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });
    grid.reload();
});
$(document).ready(function () {
    i = 0;
    dataCity = $("#txtDistrictName :selected").text();   
    grid = $("#grid").grid({
        dataKey: "City",
        uiLibrary: "bootstrap",
        dataSource: { url: ROOT + "OpenEMR/GetOpenEMRGridList?city=" + dataCity },
        columns: [
            { title: "S.No.", field: "sno", width: 60 },
            { field: "Hospital_name", title: "Hospital Name", sortable: true},
            { field: "Hospital_address", title: "Hospital Address", sortable: true },
            { field: "Hospital_contact", title: "Hospital Contact", sortable: true },
            { field: "Admin_contact", title: "Admin Contact", sortable: true },
            { field: "Reg_Date", title: "Registration Date", sortable: true },
            { title: "", field: "Edit", type: "icon", icon: "glyphicon-download", width: 45, tooltip: "Sync", events: { "click": Edit } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });
    });

   
    jQuery.validator.addMethod("lettersonly", function (value, element) {
        return this.optional(element) || /^[a-z\s]+$/i.test(value);
    }, "Only alphabetical characters");

    $("#btnSave").click(function () {
        if ($(this).text() == "Save") {
            if ($("#frmSaveContact").valid()) {
                SaveTestDetails();
            }
        }
        else {
            SaveTestDetails();
        }
    });




    $("#txtStateName").change(function () {
        var data = this.value;
        GetDistrictById(data);
        function GetDistrictById(id) {
            $.ajax({
                type: "Post",
                contentType: "application/json; charset=utf-8",
                url: ROOT + "OpenEMR/GetDistrictsById",
                data: id,
                dataType: "json",
                success: function (data) {
                    $("#txtDistrictName").empty();
                    $("#txtDistrictName").append($('<option></option>').val('0').html('---Select District---'));
                    for (var i = 0; i < data.length; i++) {
                      
                        $("#txtDistrictName").append($('<option></option>').val(data[i].Value).html(data[i].Text));
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

        };
    });
    
  
   
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


function Edit() {
   
    EditResult();
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

function EditResult() {

    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "OpenEMR/GetUserDetailsForOpenEMR",
        success: function (data) {

            if (data != null) {
  
                var url = "http://10.228.12.66:8080/openemr/interface/phrms/service.php?eid=" + data.Email + "&pid=" + data.MobileNo ;
                $.ajax({
                    type: "Get",
                    url: url,
                  
                    dataType: "jsonp",
                    success: function (data) {
                        setTimeout(function () {
                            swal({
                                title: "Success!",
                                text: "OpenEMR Data Extracted in respective modules",
                                type: "success"
                            });
                        });
                        console.log("OpenEMR Data Extracted");
                        
                    },
                    error: function (data) {
                        swal({
                            title: "Success!",
                            text: "OpenEMR Data Extracted in respective modules",
                            type: "success"
                        });
                        console.log("OpenEMR Data Extracted");
                     
                    }
                });





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



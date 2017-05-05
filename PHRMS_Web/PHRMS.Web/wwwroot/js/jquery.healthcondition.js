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
                        { field: "DiagnosisDate", title: "Diagnosis Date", tmpl: "{strDiagnosisDate}", sortable: true, width: 150 },
            { field: "HealthCondition", title: "Problem Name", sortable: true },
            //{ field: "ServiceDate", title: "Service Date", tmpl: "{strServiceDate}", sortable: true, width: 150 },
            { field: "StillHaveCondition", tmpl: "{strStillHaveCondition}", title: "Still have Problem?", sortable: true, width: 200 },
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
    $("#btnUploadFile").on("click", UploadFile);
    $("#btnSearch").on("click", Search);

    $("#txtMasterSrch").keyup(function () {
        BindHealthConditionType($("#txtMasterSrch").val(), 2);
    });

    jQuery.validator.addMethod("greaterThan",
function (value, element, params) {
    console.log(new Date(value) > new Date($(params).val()));
    if (!/Invalid|NaN/.test(new Date(value))) {
        return new Date(value) > new Date($(params).val());
    }

}, 'Must be greater than Diagnosis Date.');

    $("#frmSaveHealthCondition").validate({
        rules:
        {
            HealthCondition:
            {
                required: true
            },
            DiagnosisDate:
            {
                required: true
            },
            //ServiceDate:
            //{
            //    required: true,
            //    //   greaterThan: "#txtDiagnosisDate"
            //},
            Provider:
            {
                required: true
            }

        },
        messages: {
            HealthCondition: {
                required: "Problem is required."
            },
            DiagnosisDate: {
                required: "Diagnosis Date is required."
            },
            //ServiceDate: {
            //    required: "Service Date is required."
            //},
            Provider: {
                required: "Provider is required."
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
            if ($("#frmSelHealthCondition").valid()) {
                $("#frmSelHealthCondition").hide();
                $("#txtHealthCondition").val($('#drpHealthConditionTypes').find(":selected").text());
                $("input[datacolumn='ConditionType']").val($("#drpHealthConditionTypes").find(":selected").val());
                $("#frmSaveHealthCondition").show();
                $(this).text("Save");
            }
        }
        else {
            if ($("#txtFileName").val != null && $("#txtFileName").val != "" && $("#txtFileName").val != undefined && $("#btnSave").hasClass("upload")) {
                SaveFiles();
            }
            else if ($("#frmSaveHealthCondition").valid()) {
                SaveHealthConditionDetails();
            }
        }
    });
    //var nowDate = new Date();
    //var today = new Date(nowDate.getFullYear(), nowDate.getMonth(), nowDate.getDate(), 0, 0, 0, 0);
    $('#txtDiagnosisDate').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        endDate: '+0d'
        //onSelect: function (selected, evnt) {
        //    alert(selected);
        //}
    });


    //$('#txtDiagnosisDate').datepicker().on('changeDate', function (ev) {

    //    $('#txtServiceDate').datepicker('update', ev.date);
    //});





    //$('.input-group.date').datepicker({
    //    format: 'dd/mm/yyyy',
    //    autoclose: true
    //});

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
        BindHealthConditionType($(this).text(), 1);
    });

    //BindHealthConditionType("A", 1);

});

function incr() {
    i = i + 1;
    return i;
}

function BindHealthConditionType(str, resType) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "HealthCondition/GetHealthConditionTypes",
        data: JSON.stringify(str),
        dataType: "json",
        success: function (data) {
            var html = [];
            $.each(data, function (key, value) {
                html.push('<option value="' + value.Id + '">' + value.HealthCondition + '</option>');
            });
            $("#drpHealthConditionTypes").html("");
            $("#drpHealthConditionTypes").append(html.join(''));

            if (resType == 2) {
                $(".btn-group a", "#frmSelHealthCondition").removeClass("active");
            }
            else
                $("#txtMasterSrch").val("");
        },
        error: function (result) {

        }
    });
}





function DeleteHealthCondition(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "HealthCondition/DeleteHealthCondition",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {

                swal({
                    title: "Success!",
                    text: "Problem record archived successfully!",
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

function SaveHealthConditionDetails() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveHealthCondition").val();

    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveHealthCondition").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    vData += ",\"StillHaveCondition\":\"" + $("#rdoStillHaveYes").is(":checked") + "\"";
    var obj = "{\"__RequestVerificationToken\": '" + antiForgeryToken + "',\"oHealthConditionViewModel\": {" + vData + "}}";
    $.ajax({
        type: 'post', url: ROOT + "HealthCondition/SaveHealthCondition",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#HealthConditionModal").modal("hide");
            if (result > 0) {
                grid.reload();
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        text: "Problem saved successfully!",
                        type: "success"
                    });
                });
            }
            else {
                swal({
                    title: "Oops!",
                    //text: "Something went wrong! Please try again.",
                    text: "do not allow HTML TAGS!",
                    type: "error"
                });
            }
        },
        error: function () {
            $("#HealthConditionModal").modal("hide");
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type: "error"
            });
        }
    });
}

var grid;
function UploadFile(e) {

    $("#frmSaveFile").show();
    $("#btnSave").text("Upload File");
    $("#btnSave").addClass("upload");
    $("#btnSave").show();
    $("#frmSelHealthCondition").hide();
    $("#myModalLabel").html("Upload File(s)");
    $("#HealthConditionModal").modal("show");

}

function Add() {

    $("#rdoStillHaveYes").click();
    $("#Id").val("");
    $("#txtMasterSrch").val("");
    $("#txtHealthCondition").val("");
    $("#txtDiagnosisDate").val("");
    $("#txtServiceDate").val("");
    $("#txtProvider").val("");
    $("#txtNotes").val("");
    $("#frmSelHealthCondition").show();
    $("#frmSaveHealthCondition").hide();
    $("#frmShowHealthCondition").hide();
    $("#drpHealthConditionTypes")[0].selectedIndex = -1;
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $("#btnSave").text("Next");
    $("#btnSave").show();
    $('input').removeClass('error');
    $("#frmSelHealthCondition").validate().resetForm();
    $("#frmSaveHealthCondition").validate().resetForm();
    //$(".error").css('display', 'none');
    BindHealthConditionType("A", 1);
    $('.datepicker').datepicker('update');
    $("#myModalLabel").html("Add Problem");
    $("#HealthConditionModal").modal("show");
}
function Edit(e) {

    $("#frmSelHealthCondition").hide();
    $("#frmSaveHealthCondition").show();
    $("#frmShowHealthCondition").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Problem");
    EditHealthCondition(e.data.id);
}

function Remove(e) {
    swal({
        title: "Are you sure?",
        text: "This Problem will be archived!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, archive it!"
    },
            function () {
                DeleteHealthCondition(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {
    $("#frmSelHealthCondition").hide();
    $("#frmSaveHealthCondition").hide();
    $("#frmShowHealthCondition").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Problem Details");
    ShowHealthConditionDetails(e.data.id);
}

function ShowHealthConditionDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "HealthCondition/GetHealthConditionById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowHealthCondition").text(ui);
                });
                $("#HealthConditionModal").modal("show");
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

function EditHealthCondition(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "HealthCondition/GetHealthConditionById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveHealthCondition").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveHealthCondition").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveHealthCondition").val(ui);

                });
                $("#HealthConditionModal").modal("show");
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

}



function SaveFiles() {
    var file = document.getElementById("txtFileName")
    if (uploadOk == 1) {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", '/HealthCondition/UploadFile', true);
        xhr.setRequestHeader("X_FILENAME", file.files[0].name);
        xhr.send(file.files[0]);

        swal({
            title: "Success!",
            text: "Image Uploaded Successfully!",
            type: "success"
        });
        uploadOk = 0;
        $("#HealthConditionModal").hide();
        grid.reload();
    }
}

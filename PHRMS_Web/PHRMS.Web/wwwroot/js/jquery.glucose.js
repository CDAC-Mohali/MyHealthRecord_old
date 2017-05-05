var i;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "Sno", field: "sno", width: 50 },
            { field: "CollectionDate", title: "Date", tmpl: "{strCollectionDate}", sortable: true, width: 150 },
            { field: "Result", title: "Result", sortable: true },
            { field: "ValueType", title: "Value Type", sortable: true },
            { field: "Comments", title: "Comments", sortable: true },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
        //    { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditBloodGlucose } },
            { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-off", tooltip: "Archive", events: { "click": RemoveBloodGlucose } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });



    $("#btnAddBloodGlucose").on("click", AddBloodGlucose);
    $("#btnSearch").on("click", Search);
    $("#frmSaveBloodGlucose").validate({
        rules:
            {
                Result:
                    {
                        required:
                            true,
                        number: true
                    },
                ValueType:
                {
                     required:
                         true
                },
                CollectionDate:
                {
                    required:
                        true
                }

            },
        messages:
        {
            Result:
                {
                    required:
                        "Result is required."
                },
             ValueType:
                {
                    required:
                        "Value Type is required."
                },
            
            CollectionDate:
                {
                    required:
                        "Date is required."
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
     
        if ($("#frmSaveBloodGlucose").valid()) {
            SaveBloodGlucose();
        }
    });


    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        endDate: '+0d'
    });

    $('.input-group.date').datepicker({
        format: 'dd/mm/yyyy',
        autoclose:
        true,
          endDate: '+0d'
    });
    UpdateChart();

});

function incr() {
    i = i + 1;
    return i;
}


function DeleteBloodGlucoseAndPulse(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/DeleteBloodGlucose",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {

                swal({
                    title: "Success!",
                    text: "Blood Glucose record archived successfully!",
                    type: "success"
                });

                grid.reload();
                UpdateChart();

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

function SaveBloodGlucose() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveBloodGlucose").val();

    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveBloodGlucose").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    console.log(vData);
    $.ajax({
        type: 'post', url: ROOT + "Wellness/SaveBloodGlucose",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#BPModal").modal("hide");
            if (result > 0) {
                grid.reload();
                UpdateChart();
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        text: "Blood Glucose saved successfully!",
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
            $("#BPModal").modal("hide");
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type: "error"
            });
        }
    });
}

var grid;
function AddBloodGlucose() {
    $("#Id").val("");
    $("#txtResult").val("");
    $("#txtGoal").val("");
    $("#txtCollectionDate").val("");
    $("#txtComments").val("");
    $('#selValueType').prop('selectedIndex', 0);
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $("#frmSaveBloodGlucose").show();
    $("#frmShowBloodGlucose").hide();
    $("#btnSave").show();
    $('input').removeClass('error');
    $("#frmSaveBloodGlucose").validate().resetForm();
    $('.input-group.date').datepicker('update');
    $("#myModalLabel").html("Add Blood Glucose");
    $("#BPModal").modal("show");
}
function EditBloodGlucose(e) {
    $("#frmSaveBloodGlucose").show();
    $("#frmShowBloodGlucose").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Blood Glucose");
    EditBloodGlucoseDetails(e.data.id);
}

function RemoveBloodGlucose(e) {
    swal({
        title: "Are you sure?",
        text: "This Blood Glucose will be archived!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, archive it!"
    },
            function () {
                DeleteBloodGlucoseAndPulse(e.data.id);
                grid.reload();
                UpdateChart();
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {

    $("#frmSaveBloodGlucose").hide();
    $("#frmShowBloodGlucose").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Blood Glucose Details");
    ShowBPDetails(e.data.id);
}

function ShowBPDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetBloodGlucoseById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowBloodGlucose").text(ui);
                });
                $("#BPModal").modal("show");
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

function EditBloodGlucoseDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetBloodGlucoseById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveBloodGlucose").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveBloodGlucose").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveBloodGlucose").val(ui);
                });
                $("#BPModal").modal("show");
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

function UpdateChart() {
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetGlucoseData",
        success: function (data) {
            if (data) {
                var glucosedata = [];
                var temp = {};
                for (var i in data) {
                    temp["label"] = data[i][1].substring(0, 9);
                    temp["value"] = parseInt(data[i][0]);
                    glucosedata.push(temp);
                    temp = {};
                }
                console.log(glucosedata);
            }
            else
                return 0;
            $(".glucose-chart-container").insertFusionCharts({
                type: "line",
                width: "350",
                height: "400",
                dataFormat: "json",
                dataSource: {
                    chart: {
                        caption: "Blood Glucose",
                        theme: "ocean"
                    },
                    data: glucosedata
                }

            });
        }
    });
}

function SaveFiles() {
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

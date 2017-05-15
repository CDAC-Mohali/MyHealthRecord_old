var i;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "Sno", field: "sno", width: 50 },
            { field: "CollectionDate", title: "Date", tmpl: "{strCollectionDate}", sortable: true },
            { field: "ResSystolic", title: "Result Systolic", sortable: true },
            { field: "ResDiastolic", title: "Result Diastolic", sortable: true },
            { field: "ResPulse", title: "Result Pulse", sortable: true },
            { field: "Comments", title: "Comments", sortable: true },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
          //  { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditBloodPressure } },
            { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-off", tooltip: "Archive", events: { "click": RemoveBloodPressure } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });

    $.validator.addMethod('lessThanEqual', function (value, element, param) {
        return this.optional(element) || parseInt(value) <= parseInt($(param).val());
    }, "The diastolic must be less than systolic");

    $("#btnAddBloodPressure").on("click", AddBloodPressure);
    $("#btnSearch").on("click", Search);
    $("#frmSaveBloodPressure").validate({
        rules:
            {
                ResSystolic:
                    {
                        required:
                            true,
                        number: true,
                        max: 260,
                        min: 90
                    },
                ResDiastolic:
                {
                    required:true,
                    number: true,
                    max: 260,
                    min: 60,
                    lessThanEqual: "#txtResSystolic"
                },
                ResPulse:
                   {
                       required:
                           true,
                       number: true,
                       max: 200,
                       min:60
                   },
                CollectionDate:
                {
                    required:
                        true
                }

            },
        messages:
        {
            ResSystolic:
                   {
                       required:
                           "Systolic  is required.",
                       number:"Enter numeric value only and value should be less than 260",
                       //number: "Enter a numeric value less than 260",
                       max: "Enter numeric value only and value should be less than 260"
                   },
            ResDiastolic:
            {
                required:
                     "Diastolic  is required.",
                number: "Enter numeric value only and value should be less than 260",
                max: "Enter numeric value only and value should be less than 260"
            },
            ResPulse:
               {
                   required:
                       "Pulse  is required.",
                   number: "Enter a numeric value less than 200",
                   max: "Enter a numeric value less than 200"
               },
            GoalSystolic:
            {
                required:
                    "Systolic  is required."
            },
            GoalDiastolic:
               {
                   required:
                       "Diastolic  is required."
               },
            GoalPulse:
            {
                required:
                    "Pulse  is required."
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

        if ($("#frmSaveBloodPressure").valid()) {
            SaveBloodPressure();
        }
    });


    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        endDate: '+0d'
    });

    UpdateChart();
});

function incr() {
    i = i + 1;
    return i;
}


function DeleteBloodPressureAndPulse(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/DeleteBloodPressureAndPulse",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {

                swal({
                    title: "Success!",
                    text: "Blood Pressure record archived successfully!",
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

function SaveBloodPressure() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveBloodPressure").val();

    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveBloodPressure").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    console.log(vData);
    $.ajax({
        type: 'post', url: ROOT + "Wellness/SaveBloodPressure",
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
                        text: "Blood Pressure saved successfully!",
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
function AddBloodPressure() {
    $("#Id").val("");
    $("#txtResSystolic").val("");
    $("#txtResDiastolic").val("");
    $("#txtResPulse").val("");
    $("#txtGoalSystolic").val("");
    $("#txtGoalDiastolic").val("");
    $("#txtGoalPulse").val("");
    $("#txtCollectionDate").val("");
    $("#txtComments").val("");
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $("#frmSaveBloodPressure").show();
    $("#frmShowBloodPressure").hide();
    $("#btnSave").show();
    $('input').removeClass('error');
    $("#frmSaveBloodPressure").validate().resetForm();
    $('.datepicker').datepicker('update');
    $("#myModalLabel").html("Add Blood Pressure");
    $("#BPModal").modal("show");
}
function EditBloodPressure(e) {
    $("#frmSaveBloodPressure").show();
    $("#frmShowBloodPressure").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Blood Pressure");
    EditBloodPressureDetails(e.data.id);
}

function RemoveBloodPressure(e) {
    swal({
        title: "Are you sure?",
        text: "This Blood Pressure  record will be archived!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, archive it!"
    },
            function () {
                DeleteBloodPressureAndPulse(e.data.id);
                grid.reload();
                UpdateChart();
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {

    $("#frmSaveBloodPressure").hide();
    $("#frmShowBloodPressure").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Blood Pressure Details");
    ShowBPDetails(e.data.id);
}

function ShowBPDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetBloodPressureById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowBloodPressure").text(ui);
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

function EditBloodPressureDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetBloodPressureById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveBloodPressure").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveBloodPressure").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveBloodPressure").val(ui);
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

function UpdateChart() {
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetBPandPulseData",
        success: function (data) {
            if (data) {
                var sysdata = [];
                var diadata = [];
                var pulsedata = [];
                var temp = {};
                var categories = [];
                var category = [];
                for (var i in data) {
                    temp["label"] = data[i].Date.substring(0, 9);
                    category.push(temp);
                    temp = {};
                    temp["value"] = data[i].Systolic;
                    sysdata.push(temp);
                    temp = {};
                    temp["value"] = data[i].Diastolic;
                    diadata.push(temp);
                    temp = {};
                    temp["value"] = data[i].Pulse;
                    pulsedata.push(temp);
                }
                temp = {};
                temp["category"] = category;
                categories.push(temp);

            }
            else
                return 0;
            $(".bp-chart-container").insertFusionCharts({
                type: "mscombi2d",
                width: "350",
                height: "425",
                dataFormat: "json",
                dataSource: {
                    "chart": {
                        "caption": "Blood Pressure and Pulse",
                        "xaxisname": "Date",
                        "yaxisname": "Blood Pressure and Pulse",
                        "theme": "zune",
                    },
                    "categories": categories,
                    "dataset": [
                        {
                            "seriesname": "Systolic (in mmHg)",
                            "data": sysdata
                        },
                        {
                            "seriesname": "Diastolic (in mmHg)",

                            "showvalues": "0",
                            "data": diadata
                        },
                        {
                            "seriesname": "Pulse",
                            "renderas": "line",
                            "showvalues": "0",
                            "data": pulsedata
                        }
                    ]
                }
            });
        }
    });
}
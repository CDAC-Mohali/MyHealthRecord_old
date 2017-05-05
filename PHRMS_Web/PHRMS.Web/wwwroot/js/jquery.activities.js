var i;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            //{ field: "Id", tmpl: "{strStartDate}",sortable: true, hidden: false },
            { title: "Sno", field: "sno", width: 50 },
            { title: "Entered By", width: 100, align: "center", cssClass: "fa fa-user" },
            { field: "strCollectionDate", title: "Date", sortable: true, width: 120 },
            { field: "ActivityName", title: "Activity Name", sortable: true, width: 180 },
            { field: "PathName", title: "Path/Area/Pool Name", sortable: true },
            { field: "Distance", title: "Distance (Km)", sortable: true, width: 120 },
            //{ field: "StartTime", title: "Start Time", sortable: true, width: 100 },
            { field: "FinishTime", title: "Total Time(Minutes)", sortable: true,     width: 180 },
            //{ field: "Result", title: "Result", sortable: true },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
        //    { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
            { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-off", tooltip: "Archive", events: { "click": Remove } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });
    grid.on('rowDataBound', function (e, $row, id, record) {
      
        if (record.SourceId == 1) {
            $(".fa-mobile", $row).attr("class", "fa fa-user");

        } else if (record.SourceId == 3) {
          
            $(".fa-user", $row).attr("class", "fa fa-apple");


        } else if (record.SourceId == 4) {
         
            $(".fa-user", $row).attr("class", "fa fa-android");

}
        else
            $(".fa-user", $row).attr("class", "fa fa-mobile text-info");

        //alert($row.html());
    });


    $("#btnAddActivities").on("click", Add);
    $("#btnSearch").on("click", Search);

    $("#txtMasterSrch").keyup(function () {
        $("#drpActivitiesTypes").prop('selectedIndex', -1);
        $("#drpActivitiesTypes").children('option').show();
        $("#drpActivitiesTypes").children("option[text^=" + $("#txtMasterSrch").val() + "]").show()
        var val = $("#txtMasterSrch").val();
        $("#drpActivitiesTypes > option").each(function () {
            if (this.text.toLowerCase().indexOf(val.toLowerCase()) == -1) {
                $(this).hide();
            }
        });
    });


    $("#frmSaveActivities").validate({
        rules:
            {
                PathName:
                    {
                        required:
                            true
                    },
                Distance:
                {
                    required:
                        true,
                    number: true
                },
                FinishTime:
                {
                    required:
                        true
                },
                Hours:
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
            PathName:
                {
                    required:
                        "Path Name is required."
                },
            Distance:
            {
                required:
                    "Distance is required."
            },
            FinishTime:
            {
                required:
                    "Finish Time is required."
            },
            Hours:
            {
                required:
                    "Time is required."
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
        if ($(this).text() == "Next") {
            if ($("#frmSelActivities").valid()) {
                $("#frmSelActivities").hide();
                $("input[datacolumn='ActivityId']").val($("#drpActivitiesTypes").find(":selected").val());
                $("#Id").val("");
                $("#txtPathName").val("");
                $("#txtDistance").val("");
                $("#txtFinishTime").val("");
                $("#txtCollectionDate").val("");
                $("#txtComments").val("");
                if ($("#drpActivitiesTypes").find(":selected").val() == "5")
                    $("#lblPath").text("Pool Name:");
                else
                    $("#lblPath").text("Name of Path/Area:");
                $("#frmSaveActivities").show();
                $(this).text("Save");
            }
        }
        else {

            if ($("#frmSaveActivities").valid()) {
                SaveActivitiesDetails();
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

    UpdateChart();

    $('.duration-picker').duration_picker();
    $('.duration-picker').change(function () {
        console.log('I changed');
        //$("#txtFinishTime").innerHTML = $('.duration-picker').value;

        var hour = parseFloat($("#jdp-hours").text());
        var min = parseFloat($("#jdp-minutes").text());
        var total = hour * 60 + min;
        $("#txtFinishTime").val(total);
    });

});
function UpdateChart() {
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetActivityData",
        success: function (data) {
            var swimming = cycling = running = steps = walking = 0;
            for (var i = 0; i < data.length ; i++) {

                switch (data[i][3]) {

                    case "1": walking += parseFloat(data[i][0]);
                        break;
                    case "2": running += parseFloat(data[i][0]);
                        break;
                    case "3": cycling += parseFloat(data[i][0]);
                        break;
                    case "4": swimming += parseFloat(data[i][0]);
                        break;
                        //case "5": swimming += parseFloat(data[i][0]);
                        //    break;
                    default: break;
                }

            }

            var weightdata = [];

            if (data != "") {
                var temp = {};
                temp["label"] = "Walking + Steps";
                temp["value"] = walking;
                weightdata.push(temp);
                temp = {};
                temp["label"] = "Running";
                temp["value"] = running;
                weightdata.push(temp);
                temp = {};
                temp["label"] = "Cycling";
                temp["value"] = cycling;
                weightdata.push(temp);
                temp = {};
                //temp["label"] = "steps";
                //temp["value"] = steps;
                //weightdata.push(temp);
                //temp = {};
                temp["label"] = "Swimming";
                temp["value"] = swimming;
                weightdata.push(temp);
                temp = '';
            }

            $("#donut_single").insertFusionCharts({
                type: "pie2d",
                width: "450",
                height: "425",
                dataFormat: "json",
                dataSource: {
                    chart: {
                        caption: "Activities",

                        startingangle: "120",
                        showlabels: "0",
                        showlegend: "1",
                        enablemultislicing: "0",
                        slicingdistance: "15",
                        showpercentvalues: "1",
                        showpercentintooltip: "0",
                        plottooltext: "Activity : $label Total Distance : $datavalue",
                        theme: "fint"
                    },
                    data: weightdata
                }
            });
            walking = 0; swimming = 0; cycling = 0; running = 0; steps = 0;

        }
    })
}
function incr() {
    i = i + 1;
    return i;
}

function BindActivityType() {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetActivitiesMaster",
        data: "{}",
        dataType: "json",
        success: function (data) {
            $("#drpActivitiesTypes").empty();
            $.each(data, function (key, value) {
                $("#drpActivitiesTypes").append($("<option></option>").val(value.ActivityId).html(value.ActivityName));
            });
        },
        error: function (result) {

        }
    });
}



function DeleteActivities(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/DeleteActivities",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {

                swal({
                    title: "Success!",
                    text: "Activity record archived successfully!",
                    type:
                    "success"
                },
                function () {
                    grid.reload();
                    UpdateChart();
                });

            }
            else {
                swal({
                    title: "Oops!",
                    text: "Something went wrong! Please try again.",
                    type:
                    "error"
                });

            }
        },
        error: function (result) {
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type:
                "error"
            });
        }
    });
}

function SaveActivitiesDetails() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveActivities").val();

    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveActivities").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });

    var obj = "{\"__RequestVerificationToken\": '" + antiForgeryToken + "',\"oMedicationViewModel\": {" + vData + "}}";
    //alert("{" + vData + "}");
    $.ajax({
        type: 'post', url: ROOT + "Wellness/SaveActivities",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#ActivitiesModal").modal("hide");
            if (result > 0) {
                grid.reload();
                UpdateChart();
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        text: "Activities saved successfully!",
                        type:
                            "success"
                    });
                });
            }
            else {
                swal({
                    title: "Oops!",
                    text: "Something went wrong! Please try again.",
                    type:
                    "error"
                });
            }
        },
        error: function () {
            $("#ActivitiesModal").modal("hide");
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type:
                "error"
            });
        }
    });
}

var grid;

function Add() {
    $("#Id").val("");
    $("#txtPathName").val("");
    $("#txtDistance").val("");
    $("#txtFinishTime").val("");
    $("#txtCollectionDate").val("");
    $("#txtComments").val("");
    $("#jdp-hours").text("0");
    $("#jdp-minutes").text("0");
    $("#frmSelActivities").show();
    $("#frmSaveActivities").hide();
    $("#frmShowActivities").hide();
    $("#drpActivitiesTypes")[0].selectedIndex = -1;
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $('input').removeClass('error');
    $("#frmSelActivities").validate().resetForm();
    $("#frmSaveActivities").validate().resetForm();
    $("#btnSave").text("Next");
    $("#btnSave").show();
    $("#txtMasterSrch").val('');
    $("#drpActivitiesTypes").prop('selectedIndex', -1);
    $("#drpActivitiesTypes").children('option').show();
    $('.input-group.date').datepicker('update');
    $("#myModalLabel").html("Add Activity Result");
    $("#hours").val('');
    $("#minutes").val('');
    $("#ActivitiesModal").modal("show");
}
function Edit(e) {

    $("#frmSelActivities").hide();
    $("#frmSaveActivities").show();
    $("#frmShowActivities").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Activities");
    EditActivities(e.data.id);
}

function Remove(e) {
    swal({
        title: "Are you sure?",
        text: "This Activity will be archived!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText:
        "Yes, archive it!"
    },
            function () {
                DeleteActivities(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {
    $("#frmSelActivities").hide();
    $("#frmSaveActivities").hide();
    $("#frmShowActivities").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Activities Details");
    ShowActivitiesDetails(e.data.id);
}

function ShowActivitiesDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetActivitiesById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowActivities").text(ui);
                });
                $("#ActivitiesModal").modal("show");
            }
            else {
                swal({
                    title: "Oops!",
                    text: "We were unable to fetch the details! Please try again.",
                    type:
                    "error"
                });
            }
        },
        error: function (result) {
            swal({
                title: "Oops!",
                text: "We were unable to fetch the details! Please try again.",
                type:
                "error"
            });
        }
    });
}

function EditActivities(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetActivitiesById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveActivities").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveActivities").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveActivities").val(ui);
                });
                $("#ActivitiesModal").modal("show");
            }
            else {
                swal({
                    title: "Oops!",
                    text: "We were unable to fetch the details! Please try again.",
                    type:
                    "error"
                });
            }
        },
        error: function (result) {
            swal({
                title: "Oops!",
                text: "We were unable to fetch the details! Please try again.",
                type:
                "error"
            });
        }
    });
}


$("#hours").TouchSpin({

});
$("#minutes").TouchSpin({
    //verticalbuttons: true,
    //verticalupclass: 'glyphicon glyphicon-plus',
    //verticaldownclass: 'glyphicon glyphicon-minus'
});


function OnchangeTime() {
    var total = 0;
    var hour = parseFloat($("#hours").val());
    var min = parseFloat($("#minutes").val());

    if (isNaN(hour)) {
        $("#hours").val(0);
        hour = 0;
    }
    if (isNaN(min)) {
        $("#minutes").val(0);
        min = 0;
    }
    total = hour * 60 + min;
    $("#txtFinishTime").val(total);
}

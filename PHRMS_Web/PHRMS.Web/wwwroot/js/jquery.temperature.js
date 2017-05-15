var i;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "Sno", field: "sno", width: 50 },
            { field: "Result", title: "Result (in Fahrenheit)", sortable: true },
            { field: "Goal", title: "Goal (in Fahrenheit)", sortable: true },
            { field: "Comments", title: "Comments", sortable: true },
            { field: "CollectionDate", title: "Date", tmpl: "{strCollectionDate}", sortable: true, width: 150 },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
            { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditTemperature } },
            { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": RemoveTemperature } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });



    $("#btnAddTemperature").on("click", AddTemperature);
    $("#btnSearch").on("click", Search);
    $("#frmSaveTemperature").validate({
        rules:
            {
                Result:
                    {
                        required:
                            true
                    },
                Goal:
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
            Goal:
                {
                    required:
                        "Goal is required."
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
        if ($("#frmSaveTemperature").valid()) {
            SaveTemperature();
        }
    });


    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true
    });


});

function incr() {
    i = i + 1;
    return i;
}


function DeleteTemperature(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/DeleteTemperature",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {

                swal({
                    title: "Success!",
                    text: "Temperature record deleted successfully!",
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

function SaveTemperature() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveTemperature").val();

    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveTemperature").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    console.log(vData);
    $.ajax({
        type: 'post', url: ROOT + "Wellness/SaveTemperature",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#TemperatureModal").modal("hide");
            if (result > 0) {
                grid.reload();
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        text: "Temperature saved successfully!",
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
            $("#TemperatureModal").modal("hide");
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type: "error"
            });
        }
    });
}

var grid;
function AddTemperature() {
    $("#Id").val("");
    $("#txtResult").val("");
    $("#txtGoal").val("");
    $("#txtCollectionDate").val("");
    $("#txtComments").val("");
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $("#frmSaveTemperature").show();
    $("#frmShowTemperature").hide();
    $("#btnSave").show();
    $("#myModalLabel").html("Add Temperature");
    $("#TemperatureModal").modal("show");
}
function EditTemperature(e) {
    $("#frmSaveTemperature").show();
    $("#frmShowTemperature").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Temperature");
    EditTemperatureDetails(e.data.id);
}

function RemoveTemperature(e) {
    swal({
        title: "Are you sure?",
        text: "You will loose all information associated with this Record!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!"
    },
            function () {
                DeleteTemperature(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {

    $("#frmSaveTemperature").hide();
    $("#frmShowTemperature").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Temperature Details");
    ShowTemperatureDetails(e.data.id);
}

function ShowTemperatureDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetTemperatureById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowTemperature").text(ui);
                });
                $("#TemperatureModal").modal("show");
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

function EditTemperatureDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetTemperatureById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveTemperature").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveTemperature").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveTemperature").val(ui);
                });
                $("#TemperatureModal").modal("show");
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
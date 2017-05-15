var i;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "Sno", field: "sno", width: 50 },
            { field: "Result", title: "Result (in Hours)", sortable: true },
            { field: "Goal", title: "Goal (in Hours)", sortable: true },
            { field: "Comments", title: "Comments", sortable: true },
            { field: "CollectionDate", title: "Date", tmpl: "{strCollectionDate}", sortable: true, width: 150 },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
            { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditSleep } },
            { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": RemoveSleep } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });



    $("#btnAddSleep").on("click", AddSleep);
    $("#btnSearch").on("click", Search);
    $("#frmSaveSleep").validate({
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
        if ($("#frmSaveSleep").valid()) {
            SaveSleep();
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


function DeleteSleep(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/DeleteSleep",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {

                swal({
                    title: "Success!",
                    text: "Sleep record deleted successfully!",
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

function SaveSleep() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveSleep").val();

    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveSleep").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    console.log(vData);
    $.ajax({
        type: 'post', url: ROOT + "Wellness/SaveSleep",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#SleepModal").modal("hide");
            if (result > 0) {
                grid.reload();
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        text: "Sleep saved successfully!",
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
            $("#SleepModal").modal("hide");
            swal({
                title: "Oops!",
                text: "Something went wrong! Please try again.",
                type: "error"
            });
        }
    });
}

var grid;
function AddSleep() {
    $("#Id").val("");
    $("#txtResult").val("");
    $("#txtGoal").val("");
    $("#txtCollectionDate").val("");
    $("#txtComments").val("");
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $("#frmSaveSleep").show();
    $("#frmShowSleep").hide();
    $("#btnSave").show();
    $("#myModalLabel").html("Add Sleep");
    $("#SleepModal").modal("show");
}
function EditSleep(e) {
    $("#frmSaveSleep").show();
    $("#frmShowSleep").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Sleep");
    EditSleepDetails(e.data.id);
}

function RemoveSleep(e) {
    swal({
        title: "Are you sure?",
        text: "You will loose all information associated with this Record!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!"
    },
            function () {
                DeleteSleep(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {

    $("#frmSaveSleep").hide();
    $("#frmShowSleep").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Sleep Details");
    ShowSleepDetails(e.data.id);
}

function ShowSleepDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetSleepById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowSleep").text(ui);
                });
                $("#SleepModal").modal("show");
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

function EditSleepDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetSleepById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveSleep").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveSleep").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveSleep").val(ui);
                });
                $("#SleepModal").modal("show");
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


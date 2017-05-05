var i;
var bmi = 0;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "Sno", field: "sno", width: 50 },
                        { field: "CollectionDate", title: "Date", tmpl: "{strCollectionDate}", sortable: true, width: 150 },
            { field: "Result", title: "Weight (in Kg)", sortable: true },
            { field: "Goal", title: "Height (in cm)", sortable: true },
            { field: "BMI", title: "BMI", sortable: true },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
      //      { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditWeight } },
            { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-off", tooltip: "Archive", events: { "click": RemoveWeight } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });


    jQuery.validator.addMethod("CheckBMIValue",
    function (value, element) {
        if (parseFloat(bmi) > 40 || parseFloat(bmi) < 10) {
            // $("#pBMI").text("0");
            // $("#txtResult").val("");
            // $("#txtGoal").val("");
            return false;
        }
        else {
            return true;

        }
    }, 'Invalid BMI, Kindly fill correct weight and height.');


    $("#btnAddWeight").on("click", AddWeight);
    $("#btnSearch").on("click", Search);
    $("#frmSaveWeight").validate({
        ignore: [],
        rules:
            {
                Result:
                    {
                        required:
                            true,
                        number: true

                    },
                Goal:
                {
                    required:
                        true
                    ,
                    number: true
                },
                valBMI:
                {
                    CheckBMIValue: true
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
                        "Weight is required.",
                    number:
                       "Weight must be a number."
                },
            Goal:
                {
                    required:
                        "Height is required."
                    ,
                    number:
                       "Height must be a number."
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
        if ($("#frmSaveWeight").valid()) {

         
            SaveWeight();
        }
    });


    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        endDate: '+0d'
    });


    $("#txtResult,#txtGoal").change(function (idx) {

        if ($.isNumeric($("#txtResult").val()) && $.isNumeric($("#txtGoal").val())) {
            ht = $("#txtGoal").val() / 100;
            bmi = $("#txtResult").val() / (ht * ht).toFixed(2);
            bmi = Math.round(bmi * 100) / 100;
            $("#pBMI").text(bmi);
        }
    });
});

function incr() {
    i = i + 1;
    return i;
}


function DeleteWeight(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/DeleteWeight",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {

                swal({
                    title: "Success!",
                    text: "Weight/Height record archived successfully!",
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

function SaveWeight() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveWeight").val();
    var vData = "";
    $("input[type='text'],textarea,select", "#frmSaveWeight").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    console.log(vData);
    $.ajax({
        type: 'post', url: ROOT + "Wellness/SaveWeight",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {

         
            $("#WeightModal").modal("hide");
            if (result > 0) {
                grid.reload();
                setTimeout(function () {
                    swal({
                        title: "Success!",
                        //text: "Weight saved successfully!",
                        text:"BMI saved successfully!",
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
            $("#WeightModal").modal("hide");
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
    $("#myModalLabel").html("Upload File(s)");
    $("#WeightModal").modal("show");

}
function AddWeight() {
    $("#Id").val("");
    $("#txtResult").val("");
    $("#txtGoal").val("");
    $("#txtCollectionDate").val("");
    $("#txtComments").val("");
    $("#pBMI").text("0");
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $("#frmSaveWeight").show();
    $("#frmShowWeight").hide();
    $("#btnSave").show();
    $("#frmSaveWeight").validate().resetForm();
    $('input').removeClass('error');
    $('.datepicker').datepicker('update');
    $("#myModalLabel").html("Add Details");
    $("#WeightModal").modal("show");
}
function EditWeight(e) {
    $("#frmSaveWeight").show();
    $("#frmShowWeight").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Weight");
    EditWeightDetails(e.data.id);
}

function RemoveWeight(e) {
    swal({
        title: "Are you sure?",
        text: "This Weight/Height will be archived!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, archive it!"
    },
            function () {
                DeleteWeight(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {

    $("#frmSaveWeight").hide();
    $("#frmShowWeight").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Weight Details");
    ShowWeightDetails(e.data.id);
}

function ShowWeightDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetWeightById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowWeight").text(ui);
                });
                $("#WeightModal").modal("show");
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

function EditWeightDetails(Id) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetWeightById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveWeight").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveWeight").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveWeight").val(ui);
                });
                $("#WeightModal").modal("show");
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


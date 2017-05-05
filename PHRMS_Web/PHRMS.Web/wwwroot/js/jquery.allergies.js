var i;
$(document).ready(function () {
    i = 0;
    grid = $("#grid").grid({
        dataKey: "Id",
        uiLibrary: "bootstrap",
        columns: [
            { title: "S.No.", field: "sno", width: 50 },
            { title: "Entered By", width: 100, align: "center", cssClass: "fa fa-user" },
            { field: "AllergyName", title: "Allergy Name", sortable: true },
            { field: "DurationId", title: "From", tmpl: "{strDuration}", sortable: true, width: 200 },
            //{ field: "StartDate", title: "Start Date", tmpl: "{strStartDate}", sortable: true, width: 120 },
            //{ field: "EndDate", title: "End Date", tmpl: "{strEndDate}", sortable: true, width: 120 },
            { field: "Still_Have", tmpl: "{strStill_Have}", title: "Still Have Allergy?", sortable: true, width: 200 },
            { title: "", field: "View", width: 34, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } },
         //   { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": Edit } },
            { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-off", tooltip: "Deactivate", events: { "click": Remove } }
        ],
        pager: { enable: true, limit: 10, sizes: [10, 15, 20] }
    });

    grid.on('rowDataBound', function (e, $row, id, record) {
        //alert(record.AllergyName + "   " + record.SourceId);
        if (record.SourceId == 2) {
            $(".fa-user", $row).attr("class", "fa fa-user-md text-info");
        }
        else
            $(".fa-user-md", $row).attr("class", "fa fa-user");
        //alert($row.html());
    });

    //$(".glyphicon-user").each(function (idx, ui) {
    //    alert($(this).attr("class"));
    //});

    $("#btnAddPlayer").on("click", Add);
    $("#btnSearch").on("click", Search);

    $("#txtMasterSrch").keyup(function () {

        BindAllergyType($("#txtMasterSrch").val(), 2);
    });

    $("#frmSaveAllergy").validate({
        rules:
        {
            //AllergyEndDate:
            //{
            //    required: true
            //},
            //AllergyStartDate:
            //    {
            //        required: true
            //    },
            Severity:
                {
                    required: true
                }
        },
        messages: {
            //AllergyEndDate: {
            //    required: "Allergy Ended On is required"
            //},
            //AllergyStartDate: {
            //    required: "Allergy Started On is required."
            //},
            Severity:
            {
                required: "Allergy Severity is required."
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
            if ($("#frmSelAllergy").valid()) {
                $("#frmSelAllergy").hide();
                $("#txtAllergyName").val($('#drpAllergyTypes').find(":selected").text());
                $("input[datacolumn='AllergyType']").val($("#drpAllergyTypes").val());
                $("#frmSaveAllergy").show();
                $(this).text("Save");
            }
        }
        else {
            if ($("#frmSaveAllergy").valid()) {
                SaveAllergyDetails();
            }
        }
    });

    //$(document).on('change', 'input:radio[name^="StillHaveAllergy"]', function (event) {
    //    if ($(this).val() === 'Yes') {
    //        $(".hdDivED").hide();
    //        $("input[name*=AllergyEndDate]", ".hdDivED").rules("remove", "required");
    //    }
    //    else {
    //        $(".hdDivED").show();
    //        $("input[name*=AllergyEndDate]", ".hdDivED").rules("add", "required");
    //    }
    //});


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
        BindAllergyType($(this).text(), 1);
        $("#drpAllergyTypes").html("");
    });

    //BindAllergyType("A", 1);
    BindAllergySeverity();
    BindAllergyDuration();
});

var $loading = $('#drpAllergyTypes');
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

function BindAllergyType(str, resType) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Allergies/GetAllergyTypes",
        data: JSON.stringify(str),
        dataType: "json",
        success: function (data) {
            var html = [];
            $.each(data, function (key, value) {
                html.push('<option value="' + value.Id + '">' + value.AllergyName + '</option>');
            });
            $("#drpAllergyTypes").html("");
            $("#drpAllergyTypes").append(html.join(''));
            if (resType == 2) {
                $(".btn-group a", "#frmSelAllergy").removeClass("active");
            }
            else
                $("#txtMasterSrch").val("");
        },
        error: function (result) {

        }
    });
}


function BindAllergySeverity() {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Allergies/GetAllergySeverities",
        data: "{}",
        dataType: "json",
        success: function (data) {
            var StarRange = 1;
            var EndRange = 5;
            var ary = [];

            $.each(data, function (key, value) {

                ary[key] = value.Severity.replace("symptom ", "");

                //        $("#drpSeverity").append($("<option></option>").val(value.Id).html(value.Severity));
            });


            $('#drpSeverity').jRange({
                from: StarRange,
                to: EndRange,
                step: 1,
                scale: ary,
                format: '%s',
                width: 300,
                showLabels: false,
                snap: true,
                isRange: false,
                //   snap:false
            });
        },
        error: function (result) {

        }
    });
}
//$(document).ready(function () {
//    var ss = ["aa", "bb", "cc", "ddd"];
//    $('.single-slider').jRange({
//        from: StarRange,
//        to: EndRange,
//        step: 1,
//        scale: [Array],
//        format: '%s',
//        width: 300,
//        showLabels: false,
//        snap: true,
//        isRange: false,
//        //   snap:false
//    });

//});

function BindAllergyDuration() {
    var StarRange = 1;
    var EndRange = 4;
    var ary = [];



    ary[0] = "<&nbsp;3";
    ary[1] = "3&nbsp;to&nbsp;6";
    ary[2] = "6&nbsp;to&nbsp;12";
    ary[3] = ">&nbsp;12";
    $('#drpDurationId').jRange({
        from: StarRange,
        to: EndRange,
        step: 1,
      
        scale: ary,
        format: '%s',
        width: 300,
        showLabels: false,
        snap: true,
        isRange: false,
        //   snap:false
    });
    //$.ajax({
    //    type: "Post",
    //    contentType: "application/json; charset=utf-8",
    //    url: ROOT + "Allergies/GetAllergyDurationList",
    //    data: "{}",
    //    dataType: "json",
    //    success: function (data) {
    //        var StarRange = 1;
    //        var EndRange = 4;
    //        var ary = [];

    //        $.each(data, function (key, value) {



    //            //        $("#drpSeverity").append($("<option></option>").val(value.Id).html(value.Severity));
    //        });

    //        ary[key] = value.Duration;
    //        $('#drpDurationId').jRange({
    //            from: StarRange,
    //            to: EndRange,
    //            step: 1,
    //            scale: ary,
    //            format: '%s',
    //            width: 300,
    //            showLabels: false,
    //            snap: true,
    //            isRange: false,
    //            //   snap:false
    //        });

    //        //$.each(data, function (key, value) {
    //        //    $("#drpDurationId").append($("<option></option>").val(value.Id).html(value.Duration));
    //        //});
    //    },
    //    error: function (result) {

    //    }
    //});
}


function DeleteAllergy(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Allergies/DeleteAllergy",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data > 0) {
                swal({
                    title: "Success!",
                    text: "Allergy record deactivated successfully!",
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

function SaveAllergyDetails() {
    var antiForgeryToken = $($.find('[name= "__RequestVerificationToken"]'), "#frmSaveAllergy").val();
    var vData = "";
    $("input[type='text'],input[type='hidden'],textarea,select", "#frmSaveAllergy").not("#txtAllergyName").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
    });
    vData += ",\"Still_Have\":\"" + $("#rdoStillHaveYes").is(":checked") + "\"";
    var obj = "{\"__RequestVerificationToken\": '" + antiForgeryToken + "',\"oAllergyViewModel\": {" + vData + "}}";
    $.ajax({
        type: 'post', url: ROOT + "Allergies/SaveAllergy",
        contentType: "application/json; charset=utf-8",
        data: "{" + vData + "}",
        dataType: "json",
        success: function (result) {
            $("#allergyModal").modal("hide");
            if (result > 0) {
                grid.reload();
                swal({
                    title: "Success!",
                    text: "Allergy saved successfully!",
                    type: "success"
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
            $("#allergyModal").modal("hide");
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
    $("#playerId").val("");
    $("#name").val("");
    $("#placeOfBirth").val("");
    $("#dateOfBirth").val("");
    $("#txtMasterSrch").val("");
    $("#txtNotes").val("");
    $("#drpAllergyTypes").children('option').show();
    $("#frmSelAllergy").show();
    $("#frmSaveAllergy").hide();
    $("#frmShowAllergy").hide();
    $('#drpSeverity').prop('selectedIndex', 0);
    $("#drpAllergyTypes")[0].selectedIndex = -1;
    $("input[datacolumn='Id']").val("00000000-0000-0000-0000-000000000000");
    $("input[type='text']", "#frmSaveAllergy").not(".Static").val("");
    $("#rdoStillHaveYes").prop('checked', true);
    $(".hdDivED").show();
    $("#btnSave").text("Next");
    $("#btnSave").show();
    $('input').removeClass('error');
    $("#frmSelAllergy").validate().resetForm();
    BindAllergyType("A", 1);
    $("#myModalLabel").html("Add Allergy");
    $("#allergyModal").modal("show");
    //  $("#drpDurationId")
   
    $('#drpDurationId').jRange('setValue', '2');
    $('#drpSeverity').jRange('setValue', '2');

    
}
function Edit(e) {
    $("#frmSelAllergy").hide();
    $("#frmSaveAllergy").show();
    $("#frmShowAllergy").hide();
    $("#btnSave").text("Save");
    $("#btnSave").show();
    $("#myModalLabel").html("Edit Allergy");
    EditAllergy(e.data.id);
}

function Remove(e) {
    swal({
        title: "Are you sure?",
        text: "This allergy will be Deactivated!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, archive it!"
    },
            function () {
                DeleteAllergy(e.data.id);
            });
}
function Search() {
    grid.reload({ searchString: $("#search").val() });
}

function View(e) {
    $("#frmSelAllergy").hide();
    $("#frmSaveAllergy").hide();
    $("#frmShowAllergy").show();
    $("#btnSave").hide();
    $("#myModalLabel").html("Allergy Details");
    ShowAllergyDetails(e.data.id);
}

function ShowAllergyDetails(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Allergies/GetAllergyById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("p[datacolumn='" + p + "']", "#frmShowAllergy").text(ui);
                });
                $("#allergyModal").modal("show");
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

function EditAllergy(Id) {
    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Allergies/GetAllergyById",
        data: JSON.stringify(Id),
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (p, ui) {
                    $("input[datacolumn='" + p + "']", "#frmSaveAllergy").val(ui);
                    $("textarea[datacolumn='" + p + "']", "#frmSaveAllergy").text(ui);
                    $("select[datacolumn='" + p + "']", "#frmSaveAllergy").val(ui);
                });
                $("#allergyModal").modal("show");
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
$(document).ready(function () {
    $("#btnNext").click(function () {

        $("#DocPatientId").val('0');
        //var phonenumber = $('#Sphone_number').val();
        //var phoncount = $('#Sphone_number').val().length;

        //if (phonenumber == "") {
        //    toastr.error('Please fill Phone number', 'Inconceivable!')
        //    return false;
        //}
        //else if (phoncount != 10) {
        //    toastr.error('Please fill valid Phone number', 'Inconceivable!')
        //    return false;
        //}
        //else {

        $.ajax({
            type: "Post",
            contentType: "application/json; charset=utf-8",
            url: "/Patient/CheckPatientByDoc",
            data: '{"PhoneNumber":' + '"' + $("#Sphone_number").val() + '"' + ',"EmailAddress":' + '"' + $("#Semail").val() + '"' + "}",
            dataType: "json",
            success: function (data) {
                $('#frmAddPatient').trigger("reset");
                if (data.status == 1) {
                    if (data != null && data !== undefined) {
                        var EmailId = data.response.EmailAddress
                        if (EmailId == undefined) {
                            EmailId = "";
                        }

                        $("#myModalLabel", "#allergyModal").text("Patient : " + data.response.FirstName);
                        $("center", "#allergyModal").text("DoB: " + data.response.strDOB + " | " + "Gender: " + ((data.response.Gender == "M") ? "Male" : ((data.response.Gender == "F") ? "FeMale" : "Not Specified")) + " | " + "Mobile: " + data.response.PhoneNumber + " | " + "Email: " + EmailId);
                        $.each(data.response, function (p, ui) {
                            //alert(p + ": " + ui);
                            //alert("radio: " + $("input[datacolumn='" + p + "']", "#allergyModal").is(':radio'));
                            if ($("input[datacolumn='" + p + "']", "#allergyModal").is(':radio'))
                                $("input[datacolumn='" + p + "'][value='" + ui + "']", "#allergyModal").attr("checked", "checked")
                            else {
                                $("select[datacolumn='" + p + "']", "#allergyModal").val(ui);
                                $("input[datacolumn='" + p + "']", "#allergyModal").val(ui);
                            }
                        });
                        $("#allergyModal").modal("show");
                        toastr.success('All the patient details have been retrieved.', 'Patient Already Exists!')
                    }
                    else {
                        toastr.error('I do not think that word means what you think it means.', 'Inconceivable!')
                    }
                }
                else {
                    $("input[datacolumn='PhoneNumber']", "#allergyModal").val($("#Sphone_number").val());
                    $("input[datacolumn='EmailAddress']", "#allergyModal").val($("#Semail").val());
                    //toastr.info('I do not think that word means what you think it means.', 'Inconceivable!')
                    toastr.success('New Patient', 'New Patient!')
                    $("#allergyModal").modal("show");
                }
            },
            error: function (result) {
                toastr.error('I do not think that word means what you think it means.', 'Inconceivable!')
            }
        });
        //  }
    });

    $("#RegPatient").validate({
        rules:
        {
            FirstName:
            {
                required: true
            },
            LastName:
            {
                required: true
            },
            strDOB:
            {
                required: true
            },
            phone_number:
            {
                required: true
            },
            State:
            {
                required: true,
                min: 1
            },

        },
        messages: {
            FirstName: {
                required: "Name of the Patient is required."
            },
            LastName:
            {
                required: "Last Name is required."
            },
            strDOB: {
                required: "DoB is required."
            },
            phone_number: {
                required: "Phone Number is required."
            },
            State:
            {
                required: "State is required.",
                min: "Please select the Type"
            },


        },
        submitHandler: function (form) {
            form.submit();
        }
    });

    $('.input-group.date').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true
    });
});


function GetPatientDetails(PatId) {

    $.ajax({
        type: "Post",
        contentType: "application/json; charset=utf-8",
        url: "/Patient/GetPatientById",
        data: '{"PatId":"' + PatId + '"}',
        dataType: "json",
        success: function (data) {
            $('#frmAddPatient').trigger("reset");
            if (data.status == 1) {
                if (data != null && data !== undefined) {
                    var EmailId = data.response.EmailAddress
                    if (EmailId == undefined) {
                        EmailId = "";
                    }

                    var replace = data.response.strDOB.split('/');
                    //  alert(replace[0])
                    dob = new Date(replace[2] + "-" + replace[1] + "-" + replace[0]);

                    var today = new Date();
                    var age = Math.floor((today - dob) / (365.25 * 24 * 60 * 60 * 1000));

                    $("#myModalLabel", "#allergyModal").text("Patient : " + data.response.FirstName);
                    $("center", "#allergyModal").text("Age: " + age + " years |" + "Gender: " + ((data.response.Gender == "M") ? "Male" : ((data.response.Gender == "F") ? "FeMale" : "Not Specified")) + " | " + "Mobile: " + data.response.PhoneNumber + " | " + "Email: " + EmailId);
                    $.each(data.response, function (p, ui) {
                        //alert(p + ": " + ui);
                        //alert("radio: " + $("input[datacolumn='" + p + "']", "#allergyModal").is(':radio'));
                        if ($("input[datacolumn='" + p + "']", "#allergyModal").is(':radio'))
                            $("input[datacolumn='" + p + "'][value='" + ui + "']", "#allergyModal").attr("checked", "checked")
                        else {
                            $("select[datacolumn='" + p + "']", "#allergyModal").val(ui);
                            $("input[datacolumn='" + p + "']", "#allergyModal").val(ui);
                        }
                    });
                    $("#allergyModal").modal("show");
                    toastr.success('All the patient details have been retrieved.', 'Patient Already Exists!')
                }
                else {
                    toastr.error('I do not think that word means what you think it means.', 'Inconceivable!')
                }
            }
            else {
                $("input[datacolumn='PhoneNumber']", "#allergyModal").val($("#Sphone_number").val());
                $("input[datacolumn='EmailAddress']", "#allergyModal").val($("#Semail").val());

                toastr.info('I do not think that word means what you think it means.', 'Inconceivable!')
                $("#allergyModal").modal("show");
            }
        },
        error: function (result) {
            toastr.error('I do not think that word means what you think it means.', 'Inconceivable!')

        }
    });
}

function OnSuccess(data) {
    if (data === 1) {
        $("#allergyModal").modal("toggle");
        toastr.info('You can write prescription here.', 'Processing...');
        window.location.href = '/Prescription/Index';
        //$('.nav-tabs a[href="#tab-2"]').parent().show();
        //$('.nav-tabs a[href="#tab-2"]').tab('show');
    }
    else
        toastr.error('Request cannot be processed, please try after some time.', 'Regret!');
}

function OnBegin() {

    var phonenumber = $('#phone_number').val();
    var phoncount = $('#phone_number').val().length;

    if (phonenumber == "") {
        toastr.error('Please fill Phone number', 'Inconceivable!');
        $('#phone_number').focus();
        return false;
    }
    else if (phoncount != 10) {
        toastr.error('Please fill valid Phone number', 'Inconceivable!');
        $('#phone_number').focus();
        return false;
    }

}

function OnFailure(xhr, status) {
    toastr.error('Request cannot be processed, please try after some time.', 'Regret!')
}
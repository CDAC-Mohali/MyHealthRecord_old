var vData = "";
$(document).ready(function () {
    var options = {

        url: function (strImmmn) {
            return "/Prescription/GetImmunizationTypes";
        },

        getValue: function (element) {
            return element.ImmunizationName;
        },

        ajaxSettings: {
            dataType: "json",
            method: "POST",
            data: {
                dataType: "json"
            }
        },

        preparePostData: function (data) {
            data.strImmmn = $("#searchImm").val();
            return data;
        },

        list: {

            match: {

                enabled: true

            },

            maxNumberOfElements: 50,

            requestDelay: 400,
            onSelectItemEvent: function () {
                var value = $("#searchImm").getSelectedItemData().ImmunizationsTypeId;
                $("#keyImm").val(value).trigger("change");

            }

        }
        //maxNumberOfElements: 50,
        //requestDelay: 400
    };
    $("#searchImm").easyAutocomplete(options);

    var labOptions = {

        url: function (str) {
            return "/Prescription/GetLabTestMaster";
        },

        getValue: function (element) {
            return element.TestName;
        },

        ajaxSettings: {
            dataType: "json",
            method: "POST",
            data: {
                dataType: "json"
            }
        },

        preparePostData: function (data) {
            data.str = $("#searchLab").val();
            return data;
        },

        list: {

            match: {

                enabled: true

            },

            maxNumberOfElements: 50,

            requestDelay: 400,

            onSelectItemEvent: function () {

                var value = $("#searchLab").getSelectedItemData().Id;



                $("#keyLab").val(value).trigger("change");

            }
        }
        //maxNumberOfElements: 50,
        //requestDelay: 400
    };
    $("#searchLab").easyAutocomplete(labOptions);

    var procOptions = {

        url: function (str) {
            return "/Prescription/GetProcedureMaster";
        },

        getValue: function (element) {
            return element.ProcedureName;
        },

        ajaxSettings: {
            dataType: "json",
            method: "POST",
            data: {
                dataType: "json"
            }
        },

        preparePostData: function (data) {
            data.str = $("#searchProc").val();
            return data;
        },

        list: {

            match: {

                enabled: true

            },

            maxNumberOfElements: 50,

            requestDelay: 400,

            onSelectItemEvent: function () {

                var value = $("#searchProc").getSelectedItemData().Id;



                $("#keyProc").val(value).trigger("change");

            }

        }
        //maxNumberOfElements: 50,
        //requestDelay: 400
    };
    $("#searchProc").easyAutocomplete(procOptions);

    var mediOptions = {

        url: function (str) {
            return "/Prescription/GetMedicationMaster";
        },

        getValue: function (element) {
            return element.MedicineName;
        },

        ajaxSettings: {
            dataType: "json",
            method: "POST",
            data: {
                dataType: "json"
            }
        },

        preparePostData: function (data) {
            data.str = $("#search").val();
            return data;
        },

        list: {

            match: {

                enabled: true

            },

            maxNumberOfElements: 50,

            requestDelay: 400,

            onSelectItemEvent: function () {

                var value = $("#search").getSelectedItemData().Id;



                $("#key").val(value).trigger("change");

            }

        }
        //maxNumberOfElements: 50,
        //requestDelay: 400
    };
    $("#search").easyAutocomplete(mediOptions);

    var allergyOptions = {

        url: function (str) {
            return "/Prescription/GetAllergyTypes";
        },

        getValue: function (element) {
            return element.AllergyName;
        },

        ajaxSettings: {
            dataType: "json",
            method: "POST",
            data: {
                dataType: "json"
            }
        },

        preparePostData: function (data) {
            data.str = $("#searchAll").val();
            return data;
        },

        list: {

            match: {

                enabled: true

            },

            maxNumberOfElements: 50,

            requestDelay: 400,

            onSelectItemEvent: function () {

                var value = $("#searchAll").getSelectedItemData().Id;



                $("#keyAll").val(value).trigger("change");

            }

        }
        //maxNumberOfElements: 50,
        //requestDelay: 400
    };



    var ProblemOptions = {

        url: function (str) {
            return "/Prescription/GetProbleType";
        },

        getValue: function (element) {
            return element.ProblemName;
        },

        ajaxSettings: {
            dataType: "json",
            method: "POST",
            data: {
                dataType: "json"
            }
        },

        preparePostData: function (data) {
            data.str = $("#searchProbAll").val();
            return data;
        },

        list: {

            match: {

                enabled: true

            },

            maxNumberOfElements: 50,

            requestDelay: 400,

            onSelectItemEvent: function () {

                var value = $("#searchProbAll").getSelectedItemData().Id;



                $("#ProbkeyAll").val(value).trigger("change");

            }

        }
        //maxNumberOfElements: 50,
        //requestDelay: 400
    };

    $("#searchAll").easyAutocomplete(allergyOptions);

    $("#searchProbAll").easyAutocomplete(ProblemOptions);


    $(".dectext").keydown(function (e) {
        checkDecimal(e);
    });



    function checkDecimal(e) {

        var key = e.which;

        // backspace, tab, left arrow, up arrow, right arrow, down arrow, delete, numpad decimal pt, period, enter
        if (key != 8 && key != 9 && key != 37 && key != 38 && key != 39 && key != 40 && key != 46 && key != 110 && key != 190 && key != 13) {
            if (key < 48) {
                e.preventDefault();
            }
            else if (key > 57 && key < 96) {
                e.preventDefault();
            }
            else if (key > 105) {
                e.preventDefault();
            }
        }
    };


    $(".hidden-print").click(function () {
        var ProbDiag = $('#ProblemDiagnosis').val();
        var Systolic = $('#Systolic').val();
        var Diastolic = $('#Diastolic').val();
        var Pulse = $('#Pulse').val();

        if (ProbDiag == "") {
            toastr.error('Please fill Problem Diagnosis', 'Inconceivable!');
            $('#atCollapes6').click();
            $('#ProblemDiagnosis').focus();
            return false;

        }
        else if ((Systolic != "") && (Diastolic == "")) {
            toastr.error('Please fill Diastolic', 'Inconceivable!');
            $('#atCollapes2').click();
            $('#Diastolic').focus();
            return false;
        }
        else if ((Diastolic != "") && (Systolic == "")) {
            toastr.error('Please fill Systolic', 'Inconceivable!');
            $('#atCollapes2').click();
            $('#Systolic').focus();
            return false;
        }
        else if (parseFloat(Diastolic) > parseFloat(Systolic)) {
            toastr.error('Please fill Correct value  Diastolic & Systolic', 'Inconceivable!');
            $('#atCollapes2').click();

            return false;

        }
        else {
           
            toastr.success("Showing Preview", "Preview");
            PreviewPrescription();

            CallForPreview();
            $('#Presifno').hide();
        }
        //PreviewPrescription();
    });

    function CallForPreview() {

        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "/Prescription/PreviewEMR",
            type: "Post",
            data: '{"oEMRComplete":' + vData + '}',
            dataType: 'json',
            success: function (data) {
                $("#prescriptionDetail").hide();
                $(".nav-tabs").hide();
                $(".fa-bars").click();
                //$("#PrvPrint").show();
                $("#mainpreviewsection").show();
                $("#previewsection").empty();
                $("#previewsection").append(data.message);
                //   window.open("data:text/html;charset=utf-8," + data.message, "", "_blank")
            },
            error: function (result) {
                toastr.error('I do not think that word means what you think it means.', 'Inconceivable!')
            }
        });
        //$.ajax(options).done(function (data) {
        //    alert(data);
        ////    window.open("data:text/html;charset=utf-8," + data, "", "_blank")
        // //   var wnd = window.open("about:blank", "", "_blank");
        //  //  wnd.document.write(html);
        //});


    }
    $("#PrcBack").click(function () {
        $("#prescriptionDetail").show();
        $("#mainpreviewsection").hide();
        $(".fa-bars").click();
        $(".nav-tabs").show();
        $("#previewsection").empty();
        $('#Presifno').show();
    });
    $("#PrvSave").click(function () {
        SavePrescription();
    });
    //$("#PrvPrint").click(function () {
    //    SavePrescription();
    //});

    $("table").on("click", ".fa-trash", function () {
        $(this).parent().parent().remove();
    });

});

function addImm(oForm) {

    //var taken_on = oForm.elements['taken_on'].value;

    //var ImmComments = oForm.elements['ImmComments'].value;

    var name = $("#searchImm").val();

    var ImmVal = $("#keyImm").val();

    if (ImmVal.length > 0 && name.length > 0) {

        //alert(procVal);

        //$("#mainForm").append("<input type='hidden' name='ImmVal' value='" + ImmVal + "'>");

        //j++;

        // $("#Imm1").attr("required", "required");

        // $("#Imm2").attr("required", "required");

        //$("#mainForm").append("<input type='hidden' name='FImmName[]' value='" + name + "'>");

        // $("#mainForm").append("<input type='hidden' name='Ftaken_on[]' value='"+taken_on+"'>");

        // $("#mainForm").append("<input type='hidden' name='FImmComments[]' value='"+ImmComments+"'>");

        $("#recordTable").append("<tr>" +

            "<td>" + name + "<input type='hidden' name='TypeId' value='" + ImmVal + "' /><input type='hidden' name='ModuleId' value='8' /> </td>" +

            "<td>" + "Immunization" + "</td><td style='cursor:pointer;'><i class='fa fa-trash'></i></td></tr>");
        $("#searchImm").val("");
        $("#keyImm").val("");
        toastr.success("Immunization Records Appended", "Immunization Added");

    }

    else {

        toastr.error("", "Select Immunization From List");

    }

}

function addLabs(oForm) {



    //var performedOn = oForm.elements['performedOn'].value;

    //var LabComments = oForm.elements['LabComments'].value;

    //var result = oForm.elements['result'].value;

    //var unit = oForm.elements['unit'].value;

    var name = $("#searchLab").val();

    var LabVal = $("#keyLab").val();

    if (LabVal.length > 0 && name.length > 0) {

        //$("#mainForm").append("<input type='hidden' name='LabVal' value='"+LabVal+"'>");

        //alert(procVal);

        //$("#mainForm").append("<input type='hidden' name='FLabName[]' value='" + name + "'>");

        //$("#mainForm").append("<input type='hidden' name='FperformedOn[]' value='"+performedOn+"'>");

        //$("#mainForm").append("<input type='hidden' name='Fresult[]' value='"+result+"'>");

        //$("#mainForm").append("<input type='hidden' name='Funit[]' value='"+unit+"'>");

        //$("#mainForm").append("<input type='hidden' name='FLabComments[]' value='"+LabComments+"'>");

        //j++;

        $("#recordTable").append("<tr>" +

        "<td>" + name + "<input type='hidden' name='TypeId' value='" + LabVal + "' /><input type='hidden' name='ModuleId' value='11' /></td>" +

        "<td>" + "Lab/Tests" + "</td><td style='cursor:pointer;'><i class='fa fa-trash'></i></td>" + "</tr>");

        $("#searchLab").val("");
        $("#keyLab").val("");
        toastr.success("Lab Records Appended", "Lab Records Added");

    }

    else {

        toastr.error("", "Select Lab-Test From List");

    }

}

function addProc(oForm) {



    //var ProcStart = oForm.elements['ProcStart'].value;

    //var ProcComments = oForm.elements['ProcComments'].value;

    //var ProcEnd = oForm.elements['ProcEnd'].value;

    //var surgeon = oForm.elements['surgeon'].value;

    var ProcName = $("#searchProc").val();

    var procVal = $("#keyProc").val();

    if (procVal.length > 0 && ProcName.length > 0) {

        //$("#mainForm").append("<input type='hidden' name='ProcVal' value='"+procVal+"'>");

        //alert(procVal);

        //$("#mainForm").append("<input type='hidden' name='FProcName[]' value='" + ProcName + "'>");

        //$("#mainForm").append("<input type='hidden' name='FProcStart[]' value='"+ProcStart+"'>");

        //$("#mainForm").append("<input type='hidden' name='FProcEnd[]' value='"+ProcEnd+"'>");

        //$("#mainForm").append("<input type='hidden' name='FProcComments[]' value='"+ProcComments+"'>");

        //$("#mainForm").append("<input type='hidden' name='Fsurgeon[]' value='"+surgeon+"'>");

        //j++;

        $("#recordTable").append("<tr>" +

            "<td>" + ProcName + "<input type='hidden' name='TypeId' value='" + procVal + "' /><input type='hidden' name='ModuleId' value='10' /></td>" +

            "<td>" + "Procedures" + "</td><td style='cursor:pointer;'><i class='fa fa-trash'></i></td>" + "</tr>");
        $("#searchProc").val("");
        $("#keyProc").val("");
        toastr.success("Procedure Records Appended", "Procedure Records Added");

    }

    else {

        toastr.error("", "Select Procedure From List");

    }

}

function addAll(oForm) {

    var AllVal;

    var still_have = oForm.elements['Still_Have'].value;

    var AllComments = oForm.elements['Comments'].value;

    var severity = oForm.elements['Severity'].value;

    var from_time = oForm.elements['DurationId'].value;

    var name = oForm.elements['XAllName'].value;

    AllVal = oForm.elements["AllergyType"].value;

    if (AllVal.length > 0 && name.length > 0) {

        //$("#mainForm").append("<input type='hidden' name='AllVal' value='"+AllVal+"'>");

        //alert(procVal);

        if (still_have.length > 0 && severity.length > 0 && from_time.length > 0) {

            //$("#mainForm").append("<input type='hidden' name='FAllName[]' value='" + name + "'>");

            //$("#mainForm").append("<input type='hidden' name='Fstill_have[]' value='" + still_have + "'>");

            //$("#mainForm").append("<input type='hidden' name='Fseverity[]' value='" + severity + "'>");

            //$("#mainForm").append("<input type='hidden' name='Ffrom_time[]' value='" + from_time + "'>");

            //$("#mainForm").append("<input type='hidden' name='FAllComments[]' value='" + AllComments + "'>");

            //allergyCount++;

            $("#clinicTable").append("<tr>" +
                "<td>" + name + "<input type='hidden' name='AllergyType' value='" + AllVal + "' />"
                + "<input type='hidden' name='Still_Have' value='" + still_have + "' /><input type='hidden' name='Severity' value='" + severity + "' />"
                + "<input type='hidden' name='DurationId' value='" + from_time + "' /></td><input type='hidden' name='Comments' value='" + AllComments + "' /></td>" +
                "<td>" + $('#from').find(":selected").text() + "</td>" +
                "<td>" + $('#severity').find(":selected").text() + "</td><td style='cursor:pointer;'><i class='fa fa-trash'></i></td></tr>");

            $("#searchAll").val("");
            $("#keyAll").val("");
            $("#Comments").val("");

            toastr.success("Allergy Records Appended", "Allergy Added & Synced");

        }

        else {

            toastr.error("Kindly Make Sure You Have Entered the Severity, Duration and Still Have Parameters Correctly!", "Error in Allergy Submission");

        }

    }

    else {

        toastr.error("", "Select Allergy From List");

    }

}


function addAllProb(oForm) {

    var AllVal;

    var still_haveProb = oForm.elements['Still_Have_Prob'].value;

    var AllComments = oForm.elements['ProbComments'].value;

    var Probdate = oForm.elements['Probdate'].value;

    var ProbDiagby = oForm.elements['ProbDiagby'].value;

    var name = oForm.elements['XProbAllName'].value;

    AllVal = oForm.elements["ProbType"].value;

    if (AllVal.length > 0 && name.length > 0) {



        if (still_haveProb.length > 0 && Probdate.length > 0 && ProbDiagby.length > 0) {

            $("#ProbTable").append("<tr>" +
                 "<td>" + name + "<input type='hidden' name='ConditionType' value='" + AllVal + "' /><input type='hidden' name='StillHaveCondition' value='" + still_haveProb + "' /><input type='hidden' name='DiagnosisDate' value='" + Probdate + "' /><input type='hidden' name='Provider' value='" + ProbDiagby + "' /><input type='hidden' name='Notes' value='" + AllComments + "' /></td>" +
                  "<td>" + Probdate + "</td>" +
                   "<td>" + ProbDiagby + "</td>" +

                "<td style='cursor:pointer;'><i class='fa fa-trash'></i></td></tr>");

            $("#searchProbAll").val("");
            $("#ProbkeyAll").val("");
            $("#Probdate").val("");
            $("#ProbDiagby").val("");
            $("#ProbComments").val("");
            toastr.success(" Problem Records Appended", " Problem Added & Synced");

        }

        else {

            toastr.error("Kindly Make Sure You Have Entered the Diagnosis Date, Diagnosed By  Have Parameters Correctly!", "Error in  Problem Submission");

        }

    }

    else {

        toastr.error("", "Select Problem From List");

    }

}


function addMed(oForm) {

    var MedID = oForm.elements['XMedID'].value;

    var name;

    name = oForm.elements['XMedName'].value;

    //console.log(MedID);
    if (MedID.length > 0 && name.length > 0) {

        var str = "<tr><td><input type='text' value='" + name + "' name='MedName[]' id='medName' datacolumn='MedName' class='form-control'/> </td>" +

            "<input type='hidden' value='" + MedID + "' name='MedId[]' datacolumn='MedicineType' class='form-control'/>" +

            "<input type='hidden' value='2' name='SourceId' datacolumn='SourceId' />" +

            "<td style=\"width:5%\"><Select name=\"MedAmount[]\" class=\"form-control\" datacolumn='Frequency'><option value=\"1\">1 time per day</option><option value=\"2\">1 time per day in the morning</option><option value=\"3\">1 time per day in the evening</option><option value=\"4\">1 time per day at bedtime</option><option value=\"5\">2 times per day</option><option value=\"6\">3 times per day</option><option value=\"7\">4 times per day</option><option value=\"8\">Every hour</option><option value=\"9\">Every 2 hours</option><option value=\"10\">Every 3 hours</option><option value=\"11\">Every 4 hours</option><option value=\"12\">Every 6 hours</option><option value=\"13\">Every 8 hours</option><option value=\"14\">Every 12 hours</option><option value=\"15\">Every 24 hours</option><option value=\"16\">Every other day</option><option value=\"17\">1 time per week</option><option value=\"18\">Every two weeks</option><option value=\"19\">Every 28 days</option><option value=\"20\">Every 30 days</option><option value=\"21\">As needed</option></Select></td>" +

            "<td><input datacolumn='Strength' type=\"text\" placeholder=\"Strength\" name=\"MedStrength[]\" class=\"form-control \"></td>" +

            "<td><select datacolumn='DosValue' name=\"MedDose[]\" class=\"form-control\"><option value=\"1\">1/4</option><option value=\"2\">1/2</option><option value=\"3\">1</option><option value=\"4\">1 1/2</option><option value=\"5\">2</option><option value=\"6\">3</option><option value=\"7\">4</option><option value=\"8\">5</option><option value=\"9\">6</option><option value=\"10\">7</option><option value=\"11\">8</option><option value=\"12\">9</option><option value=\"13\">10</option></select></td>" +

            "<td><select datacolumn='DosUnit' id=\"UnitList\" name=\"MedDoseUnit[]\" class=\"form-control\"><option value=\"1\">tablet(s)</option><option value=\"2\">drop(s)</option><option value=\"3\">capsule(s)</option><option value=\"4\">tsp(s)</option><option value=\"5\">tbsp(s)</option><option value=\"6\">puff(s)</option><option value=\"7\">application(s)</option></select>" +

            "<td><select datacolumn='Route' name=\"MedRoute[]\" class=\"form-control\"><option value=\"1\">By mouth</option><option value=\"2\">To eyes</option><option value=\"3\">To skin</option><option value=\"4\">To ears</option><option value=\"5\">To nose</option><option value=\"6\">Into the muscle</option><option value=\"7\">Injection</option><option value=\"8\">Inhaled</option><option value=\"9\">To mucous membrane</option><option value=\"10\">Intravenous</option><option value=\"11\">Into a joint</option><option value=\"12\">To vagina</option><option value=\"13\">Into the skin</option><option value=\"14\">Rectal</option><option value=\"15\">Implant</option><option value=\"16\">Under the tongue</option><option value=\"17\">Hemodialysis</option><option value=\"18\">Epidural</option><option value=\"19\">Into an artery</option><option value=\"20\">Into the eye</option><option value=\"21\">Into the bladder</option><option value=\"22\">Into the uterus</option><option value=\"23\">To tongue</option><option value=\"24\">To urethra</option><option value=\"25\">Into the trachea</option><option value=\"26\">To inner cheek</option><option value=\"27\">Dental</option><option value=\"28\">Into the penis</option><option value=\"29\">Into the peritoneum</option><option value=\"30\">Irrigation</option><option value=\"31\">Intrathecal</option><option value=\"32\">Into the pleura</option><option value=\"33\">In Vitro</option><option value=\"34\">Misc</option><option value=\"35\">Perfusion</option><option value=\"36\">Combination</option></select></td>" +

            "<td><input datacolumn='DispensedDate' type=\"date\" placeholder=\"End Date\" name=\"MedDate[]\" class=\"form-control\"></td><td style='cursor:pointer;'><i class='fa fa-trash'></i></td></tr>";

        $("#med-box").append(str);
        $("#search").val("");

        toastr.success("", "Medicine Added");

    }

    else {

        toastr.error("", "Select Medicine Name First");

    }

    /*    }

     else{

     toastr.error("Please Fill in All the Fields", "Adding Medication Failed");

     } */

}



function PreviewPrescription() {

    vData = '{"MedicalHistory":{';
    $("textarea", "#collapse1").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("name")) + ":" + JSON.stringify($(this).val());
        $("span[name='" + $(this).attr("name") + "']", "#collapse11").html($(this).val());
    });
    vData += '},"EMRVitals":{';
    $("input[type='text'],input[type='hidden'],textarea,select", "#collapse2").each(function (idx, ele) {
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("name")) + ":" + JSON.stringify($(this).val());
        $("span[name='" + $(this).attr("name") + "']", "#collapse21").html($(this).val());
    });
    vData += '}';
    var advice = "[";
    $("tr", "#recordTable").each(function (idx, ui) {
        if (advice == "[")
            advice += "{"
        else
            advice += ",{"
        $("input[type='hidden']", ui).each(function (ndx, ele) {
            advice += (ndx == 0 ? "" : ",") + JSON.stringify($(this).attr("name")) + ":" + JSON.stringify($(this).val());
        });
        advice += "}";
    });
    advice += "]"
    if (advice !== "[]") {
        vData += ',"Advice":' + advice;
    }
    // $("#recordTable th:last-child, #recordTable td:last-child").remove();
    // $("#prvRecordTable").html($("#recordTable").html());
    var allergies = "[";
    $("tr", "#clinicTable").each(function (idx, ui) {
        if (allergies == "[")
            allergies += "{"
        else
            allergies += ",{"
        $("input[type='hidden']", ui).each(function (ndx, ele) {
            allergies += (ndx == 0 ? "" : ",") + JSON.stringify($(this).attr("name")) + ":" + JSON.stringify($(this).val());
        });
        allergies += "}";
    });
    allergies += "]"
    if (allergies !== "[]") {
        vData += ',"Allergies":' + allergies;
    }
    ///////////////////////////////////////////////

    var Problem = "[";
    $("tr", "#ProbTable").each(function (idx, ui) {
        if (Problem == "[")
            Problem += "{"
        else
            Problem += ",{"
        $("input[type='hidden']", ui).each(function (ndx, ele) {
            Problem += (ndx == 0 ? "" : ",") + JSON.stringify($(this).attr("name")) + ":" + JSON.stringify($(this).val());
        });
        Problem += "}";
    });
    Problem += "]"
    if (Problem !== "[]") {
        vData += ',"Problem":' + Problem;
    }
    // $("#clinicTable th:last-child, #clinicTable td:last-child").remove();
    // $("#prvClinicTable").html($("#clinicTable").html());
    var medication = "[", tr = "";
    $("tr", "#med-box").each(function (idx, ui) {
        if (medication == "[")
            medication += "{";
        else
            medication += ",{";
        tr = "<tr><td>" + $("#medName", ui).val() + "</td>";
        $("input[type='text'],input[type='hidden'],input[type='date'],textarea,select", ui).not("#medName").each(function (ndx, ele) {
            var attr = $(ele).attr('datacolumn');

            // For some browsers, `attr` is undefined; for others,
            // `attr` is false.  Check for both.
            if (typeof attr !== typeof undefined && attr !== false) {
                medication += (ndx == 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
            }

            //if ($(this).is("input"))
            //    tr += "<td>" + $(this).val() + "</td>";
            //else
            //    tr += "<td>" + $(this).find(":selected").text() + "</td>";

        });
        tr += "<td>" + $("select[datacolumn='Frequency']", ui).find(":selected").text() + "</td>" + "<td>" + $("input[datacolumn='Strength']", ui).val() + "</td>"
        + "<td>" + $("select[datacolumn='DosValue']", ui).find(":selected").text() + "<td>" + $("select[datacolumn='DosUnit']", ui).find(":selected").text()
        + "<td>" + $("select[datacolumn='Route']", ui).find(":selected").text() + "<td>" + $("input[datacolumn='DispensedDate']", ui).val() + "</td></tr>";
        $("#prvMed-box", "#collapse41").append(tr);
        medication += "}";
    });
    vData += ',"PhysicalExamination":"' + $("#PhysicalExamination").val() + '","ProblemDiagnosis":"' + $("#ProblemDiagnosis").val() + '","OtherAdvice":"' + $("#OtherAdvice").val() + '"';
    medication += "]";
    if (medication !== "[]") {
        vData += ',"Medications":' + medication;
    }
    vData += ',"Appointment":{'
    $("input[type='text'],input[type='hidden'],input[type='date'],textarea,select", "#collapse5").each(function (idx, ele) {
        //alert(JSON.stringify($(this).attr("name")));
        vData += (idx == 0 ? "" : ",") + JSON.stringify($(this).attr("name")) + ":" + JSON.stringify($(this).val());
        $("span[name='" + $(this).attr("name") + "']", "#collapse51").html($(this).val());
    });
    $("#nxtAppDate").text($("#adate").val() + " " + $("select[name='Hours']").find(":selected").text() + " " + $("select[name='Mins']").find(":selected").text() + " " + $("select[name='meridiem']").find(":selected").text());
    vData += '}}';

    //console.log(vData);

    //  $("#divMain").hide(); $("#divPrv").show();

}
function PrintEprescription() {
    //var printContents = document.getElementById("mainpreviewsection").innerHTML;
    //var originalContents = document.body.innerHTML;

    //document.body.innerHTML = printContents;
    var ProbDiag = $('#ProblemDiagnosis').val();
    if (ProbDiag == "") {
        toastr.error('Please fill Problem Diagnosis', 'Inconceivable!')
        $('#ProblemDiagnosis').focus();
        return false;

    } else {

      
        window.print();

    }
    // document.body.innerHTML = originalContents;
    //  window.print("prescriptionDetail");
}
function SavePrescription() {
    
    var ProbDiag = $('#ProblemDiagnosis').val();
    if (ProbDiag == "") {
        toastr.error('Please fill Problem Diagnosis', 'Inconceivable!')
        $('#ProblemDiagnosis').focus();
        return false;

    } else {
        toastr.success("In Processing....", "Save Details!");

        $.ajax({
            type: "Post",
            contentType: "application/json; charset=utf-8",
            url: "/Prescription/SaveEMRComplete",
            data: '{"oEMRComplete":' + vData + '}',
            dataType: "json",
            success: function (data) {
                //if (data.status == 1) {                
                toastr.success('Prescription saved successfully.', 'Success!');
                $("#PrvSave").hide();
                $("#PrvPrint").show();
                $("#PrclstBack").show();
                $("#PrcBack").hide();

                //          setTimeout(
                //function () {
                //    window.location.href = '/Prescription/CloseSession';
                //}, 3000);

                //}
                //else {
                //toastr.error('I do not think that word means what you think it means.', 'Inconceivable!')
                //}
            },
            error: function (result) {
                toastr.error('I do not think that word means what you think it means.', 'Inconceivable!')
            }
        });
    }
}

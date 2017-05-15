/**
 * Created by TheWorkStation on 24-06-2016.
 */
    var j = 0;
var ProcOptions = {
    url: b+"assets/Snomed/procedure.json",

    getValue: "ProcedureName",

    list: {
        match: {
            enabled: true
        },
        maxNumberOfElements: 50,
        requestDelay: 1000,
        onSelectItemEvent: function() {
            var value = $("#searchProc").getSelectedItemData().Id;

            $("#keyProc").val(value).trigger("change");
        }
    }
};
$("#searchProc").easyAutocomplete(ProcOptions);
$("#searchProc").parent().removeAttr("style");

var LabOptions = {
    url: b+"assets/Snomed/investigator.json",

    getValue: "TestName",

    list: {
        match: {
            enabled: true
        },
        maxNumberOfElements: 50,
        requestDelay: 1000,
        onSelectItemEvent: function() {
            var value = $("#searchLab").getSelectedItemData().Id;

            $("#keyLab").val(value).trigger("change");
        }
    }
};
$("#searchLab").easyAutocomplete(LabOptions);
$("#searchLab").parent().removeAttr("style");

var AllOptions = {
    url: b+"assets/Snomed/allergy.json",

    getValue: "AllergyName",

    list: {
        match: {
            enabled: true
        },
        maxNumberOfElements: 50,
        requestDelay: 1000,
        onSelectItemEvent: function() {
            var value = $("#searchAll").getSelectedItemData().Id;

            $("#keyAll").val(value).trigger("change");
        }
    }
};
$("#searchAll").easyAutocomplete(AllOptions);
$("#searchAll").parent().removeAttr("style");


var ImmOptions = {
    url: b+"assets/Snomed/immunization.json",

    getValue: "ImmunizationName",

    list: {
        match: {
            enabled: true
        },
        maxNumberOfElements: 50,
        requestDelay: 1000,
        onSelectItemEvent: function() {
            var value = $("#searchImm").getSelectedItemData().ImmunizationsTypeId;

            $("#keyImm").val(value).trigger("change");
        }
    }
};
$("#searchImm").easyAutocomplete(ImmOptions);
$("#searchImm").parent().removeAttr("style");

var ConOptions = {
    url: b+"assets/Snomed/conditions.json",

    getValue: "HealthCondition",

    list: {
        match: {
            enabled: true
        },
        maxNumberOfElements: 50,
        requestDelay: 1000,
        onSelectItemEvent: function() {
            var value = $("#searchCon").getSelectedItemData().Id;

            $("#keyCon").val(value).trigger("change");
        }
    }
};
$("#searchCon").easyAutocomplete(ConOptions);
$("#searchCon").parent().removeAttr("style");




// Controller for Add Immunization
function addImm(oForm){
    var ImmVal;
    var taken_on = oForm.elements['taken_on'].value;
    var ImmComments = oForm.elements['ImmComments'].value;
    var name = oForm.elements['XImmName'].value;
    ImmVal = oForm.elements["ImmVal"].value;
    if(ImmVal.length > 0 && name.length > 0){
        //alert(procVal);
        if(taken_on.length>0){
            $("#mainForm").append("<input type='hidden' name='ImmVal' value='"+ImmVal+"'>");
            j++;
            $("#Imm1").attr("required", "required");
            $("#Imm2").attr("required", "required");
            $("#mainForm").append("<input type='hidden' name='FImmName[]' value='"+name+"'>");
            $("#mainForm").append("<input type='hidden' name='Ftaken_on[]' value='"+taken_on+"'>");
            $("#mainForm").append("<input type='hidden' name='FImmComments[]' value='"+ImmComments+"'>");
            $("#recordTable").append("<tr>"+
                "<td>"+j+"</td>"+
                "<td>"+name+"</td>"+
                "<td>"+"Immunization"+"</td></tr>");
            /*$.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data:'{"ImmunizationsTypeId":"'+ImmVal+'","Comments":"'+ImmComments+'","ImmunizationDate":"'+taken_on+'","DeleteFlag":false,"UserId":"'+UserId+'","SourceId":"2"}',
                url: u+"api/Immunization/AddImmunization",
                async: false,
                success: function (data) {
                    toastr.success("Immunization Records Appended","Immunization Added");
                }
            });//*/
                    toastr.success("Immunization Records Appended","Immunization Added");
        }
        else {
            toastr.error("Error in Immunization Submission", "Please Enter The Date Taken On!");
        }
    }
    else {
        toastr.error("","Select Immunization From List")
    }
}

$.ajax({
    type: "Get",
    contentType: "application/json; charset=utf-8",
    url: u+"api/allergy/GetAllergyDuration",
    success: function (data) {
        // alert(JSON.stringify(data));
        var str, arr;
        var c = data.response.length;
        str = JSON.stringify(data);
        arr = JSON.parse(str);
        var i;
        for (i=0; i<c;i++){
            //arr.response[i].TestName=arr.response[i].TestName.replace("\"","");
            //arr.response[i].TestName=arr.response[i].TestName.replace("\"","");
            $("#from").append("<option value="+ arr.response[i].Id+">" + arr.response[i].Duration + "</option>");
        }
    }
});
$.ajax({
    type: "Get",
    contentType: "application/json; charset=utf-8",
    url: u+"api/allergy/GetAllergySeverity",
    success: function (data) {
        //  alert(JSON.stringify(data));
        var str, arr;
        var c = data.response.length;
        str = JSON.stringify(data);
        arr = JSON.parse(str);
        var i;
        for (i=0; i<c;i++){
            //arr.response[i].TestName=arr.response[i].TestName.replace("\"","");
            //arr.response[i].TestName=arr.response[i].TestName.replace("\"","");
            $("#severity").append("<option value="+ arr.response[i].Id+">" + arr.response[i].Severity + "</option>");
        }
    }
});
// Controller for Add Immunization
function addAll(oForm){
    var AllVal;
    var still_have = oForm.elements['still_have'].value;
    var AllComments = oForm.elements['AllComments'].value;
    var severity = oForm.elements['severity'].value;
    var from_time = oForm.elements['from_time'].value;
    var name = oForm.elements['XAllName'].value;
    AllVal = oForm.elements["AllVal"].value;
    if(AllVal.length > 0 && name.length > 0){
        //$("#mainForm").append("<input type='hidden' name='AllVal' value='"+AllVal+"'>");
        //alert(procVal);
        if(still_have.length>0 && severity.length>0 && from_time.length>0){
            $("#mainForm").append("<input type='hidden' name='FAllName[]' value='"+name+"'>");
            $("#mainForm").append("<input type='hidden' name='Fstill_have[]' value='"+still_have+"'>");
            $("#mainForm").append("<input type='hidden' name='Fseverity[]' value='"+severity+"'>");
            $("#mainForm").append("<input type='hidden' name='Ffrom_time[]' value='"+from_time+"'>");
            $("#mainForm").append("<input type='hidden' name='FAllComments[]' value='"+AllComments+"'>");
            j++;
            $("#recordTable").append("<tr>"+
                "<td>"+j+"</td>"+
                "<td>"+name+"</td>"+
                "<td>"+"Allergy"+"</td>"
                +"</tr>");
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data:'{"AllergyType":"'+AllVal+'","Comments":"'+AllComments+'","Still_Have":'+still_have+',"Severity":"'+severity+'","DurationId":"'+from_time+'","DeleteFlag":false,"UserId":"'+UserId+'"}',
                url: u+"api/allergy/AddAllergy",
                async: false,
                success: function (data) {
                    toastr.success("Allergy Records Appended","Allergy Added & Synced");
                }
            });
        }
        else {
            toastr.error("Kindly Make Sure You Have Entered the Severity, Duration and Still Have Parameters Correctly!", "Error in Allergy Submission");
        }
    }
    else {
        toastr.error("","Select Allergy From List")
    }
}
//Controller for Labs
function addLabs(oForm){
    var LabVal;
    var performedOn = oForm.elements['performedOn'].value;
    var LabComments = oForm.elements['LabComments'].value;
    var result = oForm.elements['result'].value;
    var unit = oForm.elements['unit'].value;
    var name = oForm.elements['XLabName'].value;
    LabVal = oForm.elements["LabVal"].value;
    if(LabVal.length > 0 && name.length > 0){
        //$("#mainForm").append("<input type='hidden' name='LabVal' value='"+LabVal+"'>");
        //alert(procVal);
        if(performedOn.length>0 && result.length>0 && unit.length>0){
            $("#mainForm").append("<input type='hidden' name='FLabName[]' value='"+name+"'>");
             $("#mainForm").append("<input type='hidden' name='FperformedOn[]' value='"+performedOn+"'>");
             $("#mainForm").append("<input type='hidden' name='Fresult[]' value='"+result+"'>");
             $("#mainForm").append("<input type='hidden' name='Funit[]' value='"+unit+"'>");
             $("#mainForm").append("<input type='hidden' name='FLabComments[]' value='"+LabComments+"'>");
             j++;
             $("#recordTable").append("<tr>"+
             "<td>"+j+"</td>"+
             "<td>"+name+"</td>"+
             "<td>"+"Labs-Tests"+"</td>"
             +"</tr>");
            console.log('{"TestId":"'+LabVal+'","Comments":"'+LabComments+'","PerformedDate":"'+ performedOn +'","Result":"'+result+'","Unit":"'+unit+'","DeleteFlag":false,"UserId":"'+UserId+'","SourceId":"2"}');
            /*$.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: '{"TestId":"'+LabVal+'","Comments":"'+LabComments+'","PerformedDate":"'+ performedOn +'","Result":"'+result+'","Unit":"'+unit+'","DeleteFlag":false,"UserId":"'+UserId+'","SourceId":"2"}',
                url: u+"api/Lab/AddLabTest",
                success: function (data) {
                    toastr.success("Lab Records Appended","Lab Records Added")
                },
                error: function (error){
                    toastr.error("Internal Server 500 Error", "Contact PHR Admin to Report");
                }
            });//*/
            toastr.success("Lab Records Appended","Lab Records Added");
        }
        else {
            toastr.error("Kindly Make Sure You Have Entered the Perfomed On Date, Results and Unit Parameters Correctly!", "Error in Lab-Tests Submission");
        }
    }
    else {
        toastr.error("","Select Lab-Test From List")
    }
}
//Controller for Labs
function addProc(oForm){
    var procVal;
    var ProcStart = oForm.elements['ProcStart'].value;
    var ProcComments = oForm.elements['ProcComments'].value;
    var ProcEnd = oForm.elements['ProcEnd'].value;
    var surgeon = oForm.elements['surgeon'].value;
    var ProcName = oForm.elements['XProcName'].value;
    procVal = oForm.elements["ProcVal"].value;
    if(procVal.length > 0 && ProcName.length > 0){
        //$("#mainForm").append("<input type='hidden' name='ProcVal' value='"+procVal+"'>");
        //alert(procVal);
        if(ProcStart.length>0 && ProcEnd.length>0 && surgeon.length>0){
            $("#mainForm").append("<input type='hidden' name='FProcName[]' value='"+ProcName+"'>");
            $("#mainForm").append("<input type='hidden' name='FProcStart[]' value='"+ProcStart+"'>");
            $("#mainForm").append("<input type='hidden' name='FProcEnd[]' value='"+ProcEnd+"'>");
            $("#mainForm").append("<input type='hidden' name='FProcComments[]' value='"+ProcComments+"'>");
            $("#mainForm").append("<input type='hidden' name='Fsurgeon[]' value='"+surgeon+"'>");
            j++;
            $("#recordTable").append("<tr>"+
                "<td>"+j+"</td>"+
                "<td>"+name+"</td>"+
                "<td>"+"Procedures"+"</td>"
                +"</tr>");
            console.log('{"ProcedureType":"'+procVal+'","Comments":"'+ProcComments+'","StartDate":"'+ProcStart+'","EndDate":"'+ProcEnd+'","DeleteFlag":false,"UserId":"'+UserId+'","SourceId":"2"}');
            /*$.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data:'{"ProcedureType":"'+procVal+'","Comments":"'+ProcComments+'","StartDate":"'+ProcStart+'","EndDate":"'+ProcEnd+'","DeleteFlag":false,"UserId":"'+UserId+'","SourceId":"2"}',
                url: u+"api/Procedures/AddProcedure",
                success: function (data) {
                    toastr.success("Procedure Records Appended", "Procedure Records Added");
                },
                error: function (error){
                    toastr.error("Internal Server 500 Error", "Contact PHR Admin to Report");
                }
            });//*/
            toastr.success("Procedure Records Appended", "Procedure Records Added");
        }
        else {
            toastr.error("Kindly Make Sure You Have Entered the Start and End Dates and Surgeon name correctly!", "Error in Procedure Submission Submission");
        }
    }
    else {
        toastr.error("","Select Procedure From List")
    }
}

// Medication Controller
var Medarr, Medc, Medcounter=0;
function addMed(oForm){
    var MedID = oForm.elements['XMedID'].value;
    var name;
    name = oForm.elements['XMedName'].value;
    console.log(MedID);
    if(Medcounter==0){
        Medcounter++;
    }
    if(MedID.length>0 && name.length>0){
        var str = "<tr>"+
            "<!--<td style=\"width:3%\">"+Medcounter+"</td>-->"+
            "<td><input type='text' value='"+ name +"' name='MedName[]' class='form-control'/> </td>"+
            "<input type='hidden' value='"+ MedID +"' name='MedId[]' datacolumn='MedicineType' class='form-control'/>"+
            "<input type='hidden' value='2' name='SourceId' datacolumn='SourceId' />"+
            "<td style=\"width:5%\"><Select name=\"MedAmount[]\" class=\"form-control\" datacolumn='Frequency'><option value=\"1\">1 time per day</option><option value=\"2\">1 time per day in the morning</option><option value=\"3\">1 time per day in the evening</option><option value=\"4\">1 time per day at bedtime</option><option value=\"5\">2 times per day</option><option value=\"6\">3 times per day</option><option value=\"7\">4 times per day</option><option value=\"8\">Every hour</option><option value=\"9\">Every 2 hours</option><option value=\"10\">Every 3 hours</option><option value=\"11\">Every 4 hours</option><option value=\"12\">Every 6 hours</option><option value=\"13\">Every 8 hours</option><option value=\"14\">Every 12 hours</option><option value=\"15\">Every 24 hours</option><option value=\"16\">Every other day</option><option value=\"17\">1 time per week</option><option value=\"18\">Every two weeks</option><option value=\"19\">Every 28 days</option><option value=\"20\">Every 30 days</option><option value=\"21\">As needed</option></Select></td>"+
            "<td><input datacolumn='Strength' type=\"text\" placeholder=\"Timings\" name=\"MedStrength[]\" class=\"form-control\"></td>"+
            "<td><select datacolumn='DosValue' name=\"MedDose[]\" class=\"form-control\"><option value=\"1\">1/4</option><option value=\"2\">1/2</option><option value=\"3\">1</option><option value=\"4\">1 1/2</option><option value=\"5\">2</option><option value=\"6\">3</option><option value=\"7\">4</option><option value=\"8\">5</option><option value=\"9\">6</option><option value=\"10\">7</option><option value=\"11\">8</option><option value=\"12\">9</option><option value=\"13\">10</option></select></td>"+
            "<td><select datacolumn='DosUnit' id=\"UnitList\" name=\"MedDoseUnit[]\" class=\"form-control\"><option value=\"1\">tablet(s)</option><option value=\"2\">drop(s)</option><option value=\"3\">capsule(s)</option><option value=\"4\">tsp(s)</option><option value=\"5\">tbsp(s)</option><option value=\"6\">puff(s)</option><option value=\"7\">application(s)</option></select>"+
            "<td><select datacolumn='Route' name=\"MedRoute[]\" class=\"form-control\"><option value=\"1\">By mouth</option><option value=\"2\">To eyes</option><option value=\"3\">To skin</option><option value=\"4\">To ears</option><option value=\"5\">To nose</option><option value=\"6\">Into the muscle</option><option value=\"7\">Injection</option><option value=\"8\">Inhaled</option><option value=\"9\">To mucous membrane</option><option value=\"10\">Intravenous</option><option value=\"11\">Into a joint</option><option value=\"12\">To vagina</option><option value=\"13\">Into the skin</option><option value=\"14\">Rectal</option><option value=\"15\">Implant</option><option value=\"16\">Under the tongue</option><option value=\"17\">Hemodialysis</option><option value=\"18\">Epidural</option><option value=\"19\">Into an artery</option><option value=\"20\">Into the eye</option><option value=\"21\">Into the bladder</option><option value=\"22\">Into the uterus</option><option value=\"23\">To tongue</option><option value=\"24\">To urethra</option><option value=\"25\">Into the trachea</option><option value=\"26\">To inner cheek</option><option value=\"27\">Dental</option><option value=\"28\">Into the penis</option><option value=\"29\">Into the peritoneum</option><option value=\"30\">Irrigation</option><option value=\"31\">Intrathecal</option><option value=\"32\">Into the pleura</option><option value=\"33\">In Vitro</option><option value=\"34\">Misc</option><option value=\"35\">Perfusion</option><option value=\"36\">Combination</option></select></td>"+
            "<td><input datacolumn='DispensedDate' type=\"date\" placeholder=\"End Date\" name=\"MedDate[]\" class=\"form-control\"></td>"+
            "<!--<td align=\"center\" style=\"padding-top: 0.8%;\"><a data-toggle=\"tooltip\" title=\"Save\" href='javascript: saveMed();'><span class=\"glyphicon glyphicon-check\"></span></a> | <a data-toggle=\"tooltip\" title=\"Delete\"><span class=\"glyphicon glyphicon-trash\"></span></a></td>-->"+
            "</tr>";
        $("#med-box").append(str);
        toastr.success("","Medicine Added");
    }
    else {
        toastr.error("", "Select Medicine Name First");
    }
    /*    }
     else{
     toastr.error("Please Fill in All the Fields", "Adding Medication Failed");
     } */
}

function getMedListing(a,b){
    $("#med-list option").remove();
    $('.pagination').on('click', 'li', function() {
        $('.pagination li.active').removeClass('active');
        $(this).addClass('active');
    });
    if(a == "A"){
        var options = {
            url: b+"assets/RxNorms/rxnorm-a.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "B"){
        var options = {
            url: b+"assets/RxNorms/rxnorm-b.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "C"){
        var options = {
            url: b+"assets/RxNorms/rxnorm-c.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "D"){
        var options = {
            url: b+"assets/RxNorms/rxnorm-d.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "E"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-e.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "F"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-f.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "G"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-g.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "H"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-h.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "I"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-i.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "J"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-j.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "K"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-k.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "L"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-l.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "M"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-m.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "N"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-n.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "O"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-o.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "P"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-p.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "Q"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-q.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "R"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-r.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "S"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-s.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "T"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-t.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "U"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-u.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "V"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-v.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "W"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-w.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "X"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-x.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "Y"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-y.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
    if(a == "Z"){

        var options = {
            url: b+"assets/RxNorms/rxnorm-z.json",

            getValue: "MedicineName",

            list: {
                match: {
                    enabled: true
                },
                maxNumberOfElements: 50,
                requestDelay: 1000,
                onSelectItemEvent: function() {
                    var value = $("#search").getSelectedItemData().Id;

                    $("#key").val(value).trigger("change");
                }
            }
        };


        $("#search").easyAutocomplete(options);
        $("#search").parent().removeAttr("style");
    }
}
function enableEditP(){
    $("#phistory").removeAttr("disabled");
}
function enableEditF(){
    $("#fhistory").removeAttr("disabled");
}
function saveAppointment(hour, min, meridian, date){

}
function complete(UserId,oForm,d){
    var myForm = document.forms.mainForm;
    var personal_history = oForm.elements['personal_history'].value;
    var family_history = oForm.elements['family_history'].value;
    var physical_examination = oForm.elements['physical_examination'].value;
    //Prescription Attributes
    var problem_diagnosis = oForm.elements['problem_diagnosis'].value;
    var systolic = oForm.elements['systolic'].value;
    var diasystolic = oForm.elements['diasystolic'].value;
    var pulse = oForm.elements['pulse'].value;
    var weight = oForm.elements['weight'].value;
    var respiratory_rate = oForm.elements['respiratory_rate'].value;
    var temperature = oForm.elements['temperature'].value;
    var spo2 = oForm.elements['spo2'].value;
    var glucose = oForm.elements['glucose'].value;
    var other_advice = oForm.elements['other_advice'].value;

    if(!(problem_diagnosis.length>0)){
        toastr.error("Enter Problem Diagnosis To Continue!", "Please Fill the Required Fields");
    }
    else{
    //Immunization Records
    /*var ImmVal = oForm.elements['ImmVal'].value;
     var taken_on = oForm.elements['taken_on'].value;
     var ImmComments = oForm.elements['ImmComments'].value;
     //Allergy Records
     var AllVal = oForm.elements['AllVal'].value;
     var still_have = oForm.elements['still_have'].value;
     var from_time = oForm.elements['from_time'].value;
     var severity = oForm.elements['severity'].value;
     var AllComments = oForm.elements['AllComments'].value;
     //Procedure Records
     var ProcVal = oForm.elements['ProcVal'].value;
     var ProcStart = oForm.elements['ProcStart'].value;
     var ProcEnd = oForm.elements['ProcEnd'].value;
     var surgeon = oForm.elements['surgeon'].value;
     var ProcComments = oForm.elements['ProcComments'].value;
     //Lab Records
     var LabVal = oForm.elements['LabVal'].value;
     var performedOn = oForm.elements['performedOn'].value;
     var result = oForm.elements['result'].value;
     var unit = oForm.elements['unit'].value;
     var LabComments = oForm.elements['LabComments'].value;*/
        var ADate = oForm.elements['adate'].value;
        var AHour = oForm.elements['ahour'].value;
        var AMin = oForm.elements['amins'].value;
        var AHalf = oForm.elements['ahalf'].value;
        var AFees = oForm.elements['afees'].value;
     var vData = '';
    $("tr","#med-box").each(function(idx,ui){
        //alert($(ui).html());
        vData = vData == '' ? '{' : vData;
        $("input[type='text'],input[type='date'],input[type='hidden'],textarea,select", ui).each(function (idx, ele) {
            var attr = $(this).attr('datacolumn');
            if (typeof attr !== typeof undefined && attr !== false) {
                vData += ($(this).attr("datacolumn") == "MedicineType" ? vData == "{" ? "" : ",{" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
            }
            //vData += (ivdx !== 0 ? "" : ",") + JSON.stringify($(this).attr("datacolumn")) + ":" + JSON.stringify($(this).val());
        });
        vData = vData + '}';
    });
    //console.log(vData);
    console.log('{"SourceId":"2","EMRVitals":{"ProbDiag":"'+problem_diagnosis+'","Systolic":"'+systolic+'","Diastolic":"'+diasystolic+'","Pulse":"'+pulse+'","Weight":"'+weight+'","RespiratoryRate":"'+respiratory_rate+'","SpO2":"'+spo2+'","Glucose":"'+glucose+'","OtherAdvice":"'+other_advice+'","PhyExam":"'+physical_examination+'"},"MedicalHistory":{"SourceId":"2","PersonalHistory":"'+personal_history+'","FamilyHistory":"'+family_history+'"},"Medications":['+vData+'],"Appointment":{"UserId":"'+UserId+'","DocId":"'+d+'","Date":"'+ADate+'","Hours":"'+AHour+'","Mins":"'+AMin+'","meridiem":"'+AHalf+'","NetFee":"'+AFees+'","Visited":false},"SourceId":"2","UserId":"'+UserId+'","DocId":"'+d+'"}');
    var final = '{"EMRVitals":{"SourceId":"2","ProbDiag":"'+problem_diagnosis+'","Systolic":"'+systolic+'","Diastolic":"'+diasystolic+'","Pulse":"'+pulse+'","Weight":"'+weight+'","RespiratoryRate":"'+respiratory_rate+'","SpO2":"'+spo2+'","Glucose":"'+glucose+'","OtherAdvice":"'+other_advice+'","PhyExam":"'+physical_examination+'"},"MedicalHistory":{"SourceId":"2","PersonalHistory":"'+personal_history+'","FamilyHistory":"'+family_history+'"},"Medications":['+vData+'],"Appointment":{"UserId":"'+UserId+'","DocId":"'+d+'","Date":"'+ADate+'","Hours":"'+AHour+'","Mins":"'+AMin+'","meridiem":"'+AHalf+'","NetFee":"'+AFees+'","Visited":false},"SourceId":"2","UserId":"'+UserId+'","DocId":"'+d+'"';
    $("#mainForm").append("<input type='hidden' value='"+final+"' name='final' />");
    console.log(final);
    document.getElementById("mainForm").submit();
    }

}
function finalize(printFlag, f, b, u){
    toastr.info("Syncing Data with PHRMS ...", "Saving Data!");
    f+=',"Prescription_Copy":"'+TransferPDF()+'"}';
    console.log(f);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        async: false,
        data: f,
        url: u+"api/Prescription/SaveEMRComplete",
        success: function(data){
            if(data){
                toastr.success("Syncing Sucessful!", "Saved!");
                window.location=b+"ePrescription/CloseSession";
            }
            else{
                toastr.error("Syncing Failed!", "Oops! Unable to Save Data!");
            }
        }
    });
    if(printFlag){
        window.print();
    }
}
function TransferPDF(){
    var strPDF;
    var pdf = new jsPDF('p', 'pt', 'letter');
            // source can be HTML-formatted string, or a reference
            // to an actual DOM element from which the text will be scraped.
            source = $('.wrapper')[0];

            // we support special element handlers. Register them with jQuery-style 
            // ID selector for either ID or node name. ("#iAmID", "div", "span" etc.)
            // There is no support for any other type of selectors 
            // (class, of compound) at this time.
            specialElementHandlers = {
                // element with id of "bypass" - jQuery style selector
                '#bypassme': function(element, renderer) {
                    // true = "handled elsewhere, bypass text extraction"
                    return true
                }
            };
            margins = {
                top: 80,
                bottom: 60,
                left: 40,
                width: 522
            };
            // all coords and widths are in jsPDF instance's declared units
            // 'inches' in this case
            pdf.fromHTML(
                    source, // HTML string or DOM elem ref.
                    margins.left, // x coord
                    margins.top, {// y coord
                        'width': margins.width, // max width of content on PDF
                        'elementHandlers': specialElementHandlers
                    },
            function(dispose) {
                // dispose: object with X, Y of the last line add to the PDF 
                //          this allow the insertion of new lines after html
            strPDF = pdf.output("datauristring");
            }
            , margins);
            return strPDF;

}
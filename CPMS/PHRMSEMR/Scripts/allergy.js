/**
 * Created by TheWorkStation on 30-04-2016.
 */


$.ajax({
    type: "Get",
    contentType: "application/json; charset=utf-8",
    url: u+"api/allergy/GetAllergyNameListing",
    success: function (data) {
          //alert(JSON.stringify(data));
        var str, arr;
        var c = data.response.length;
        str = JSON.stringify(data);
        arr = JSON.parse(str);
        var i;
        for (i=0; i<c;i++){
            //arr.response[i].TestName=arr.response[i].TestName.replace("\"","");
            //arr.response[i].TestName=arr.response[i].TestName.replace("\"","");
            $("#allergy").append("<option value="+ arr.response[i].Id+">" + arr.response[i].AllergyName + "</option>");
        }
    }
});
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
function getData(){
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: u+"api/Allergy/GetAllergyData/" + UserId,
        success: function (data) {
            // alert(JSON.stringify(data));
            var str, arr;
            var c = data.response.length;
            str = JSON.stringify(data);
            arr = JSON.parse(str);
            var i;
            for (i=0; i<c;i++){
                if(arr.response[i].Still_Have == false){
                    arr.response[i].Still_Have="No"
                }
                else if(arr.response[i].Still_Have == true){
                    arr.response[i].Still_Have ="Yes"
                }
                $("#UserProfile").append("<tr>"+
                    "<td>"+(i+1)+"</td>"+
                    "<td>"+arr.response[i].AllergyName+"</td>"+
                    "<td>"+arr.response[i].Still_Have+"</td>"+
                    "<td>"+arr.response[i].strSeverity+"</td>"+
                    "<td>"+arr.response[i].strDuration+"</td>"+
                    "<td>"+arr.response[i].Comments+"</td>"+
                    "</tr>");
            }
        }
    });
}
getData();
function addAllergy(){
    $("#AllergyList").fadeIn();
}

var procVal;
function showNextAddProcedureStep(oForm){
    procVal = oForm.elements["pval"].value;
    if(procVal.length > 0){
        //alert(procVal);
        $("#allergy").hide();
        $("#next").fadeOut();
        $("#procedureForm1").fadeIn();
        $("#fsubmit").fadeIn();
        $("#procedureForm").append("<input type='hidden' id='playerId' name='pid' value='"+procVal+"'>")
    }
}

function submitForm(form1){
    var still_have=form1.elements['still_have'].value;
    var comments=form1.elements['comments'].value;
    var severity=form1.elements['severity'].value;
    var duration=form1.elements['from'].value;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data:'{"AllergyType":"'+procVal+'","Comments":"'+comments+'","Still_Have":'+still_have+',"Severity":"'+severity+'","DurationId":"'+duration+'","DeleteFlag":false,"UserId":"'+UserId+'"}',
        url: u+"api/allergy/AddAllergy",
        success: function (data) {
            //alert(JSON.stringify(data));
            $("#UserProfile tr").remove();
            getData();
            $("#TestsList").css("display","none");
        }
    });
}

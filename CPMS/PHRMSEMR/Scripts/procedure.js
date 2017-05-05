/**
 * Created by TheWorkStation on 17-04-2016.
 */

$.ajax({
    type: "Get",
    contentType: "application/json; charset=utf-8",
    url: u+"api/Procedures/GetProcedureNameListing",
    success: function (data) {
        //    alert(JSON.stringify(data));
        var str, arr;
        var c = data.response.length;
        str = JSON.stringify(data);
        arr = JSON.parse(str);
        var i;
        for (i=0; i<c;i++){
            //arr.response[i].TestName=arr.response[i].TestName.replace("\"","");
            //arr.response[i].TestName=arr.response[i].TestName.replace("\"","");
            $("#procedures").append("<option value="+ arr.response[i].Id+">" + arr.response[i].ProcedureName + "</option>");
        }
    }
});
function getData(){
$.ajax({
    type: "GET",
    contentType: "application/json; charset=utf-8",
    url: u+"api/Procedures/GetProcedureData/" + UserId,
    success: function (data) {
    //alert(JSON.stringify(data));
    var str, arr;
    var c = data.response.length;
    str = JSON.stringify(data);
    arr = JSON.parse(str);
    var i;
    for (i=0; i<c;i++){
        if(arr.response[i].SurgeonName == null){
            arr.response[i].SurgeonName = "-";
        }
        $("#UserProfile").append("<tr>"+
            "<td>"+(i+1)+"</td>"+
            "<td>"+arr.response[i].ProcedureName+"</td>"+
            "<td>"+arr.response[i].strStartDate+"</td>"+
            "<td>"+arr.response[i].strEndDate+"</td>"+
            "<td>"+arr.response[i].Comments+"</td>"+
            "</tr>");
    }
}
});
}
getData();
function addProcedure(){
    $("#procedureList").fadeIn();
}

var procVal;
function showNextAddProcedureStep(oForm){
    procVal = oForm.elements["pval"].value;
    if(procVal.length > 0){
    //alert(procVal);
    $("#list").hide();
    $("#next").fadeOut();
    $("#procedureForm1").fadeIn();
    $("#fsubmit").fadeIn();
    $("#procedureForm").append("<input type='hidden' id='playerId' name='pid' value='"+procVal+"'>")
    }
}

function submitForm(form1){
    var sdate=form1.elements['sdate'].value;
    var edate=form1.elements['edate'].value;
    var surgeon=form1.elements['surgeon'].value;
    var comments=form1.elements['comments'].value;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data:'{"ProcedureType":"'+procVal+'","Comments":"'+comments+'","StartDate":"'+sdate+'","EndDate":"'+edate+'","DeleteFlag":false,"UserId":"'+UserId+'","SourceId":"2"}',
        url: u+"api/Procedures/AddProcedure",
        success: function (data) {
            //alert(JSON.stringify(data));
            $("#UserProfile tr").remove();
            getData();
            $("#procedureList").css("display","none");
        }
    });
}

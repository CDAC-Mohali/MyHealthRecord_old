/**
 * Created by TheWorkStation on 30-04-2016.
 */

$.ajax({
    type: "Get",
    contentType: "application/json; charset=utf-8",
    url: u+"api/Lab/GetLabTestNameListing",
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
            $("#tests").append("<option value="+ arr.response[i].Id+">" + arr.response[i].TestName + "</option>");
        }
    }
});
function getData(){
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: u+"api/Lab/GetLabData/" + UserId,
        success: function (data) {
           // alert(JSON.stringify(data));
            var str, arr;
            var c = data.response.length;
            str = JSON.stringify(data);
            arr = JSON.parse(str);
            var i;
            for (i=0; i<c;i++){
                $("#UserProfile").append("<tr>"+
                    "<td>"+(i+1)+"</td>"+
                    "<td>"+arr.response[i].TestName+"</td>"+
                    "<td>"+arr.response[i].strPerformedDate+"</td>"+
                    "<td>"+arr.response[i].Comments+"</td>"+
                    "<td>"+arr.response[i].Result+"</td>"+
                    "<td>"+arr.response[i].Unit+"</td>"+
                    "</tr>");
            }
        }
    });
}
getData();
function addTest(){
    $("#TestsList").fadeIn();
}

var procVal;
function showNextAddProcedureStep(oForm){
    procVal = oForm.elements["pval"].value;
    if(procVal.length > 0){
    //alert(procVal);
    $("#tests").hide();
    $("#next").fadeOut();
    $("#procedureForm1").fadeIn();
    $("#fsubmit").fadeIn();
    $("#procedureForm").append("<input type='hidden' id='playerId' name='pid' value='"+procVal+"'>")
    }
}

function submitForm(form1){
    var pdate=form1.elements['pdate'].value;
    var comments=form1.elements['comments'].value;
    var result=form1.elements['result'].value;
    var unit=form1.elements['unit'].value;
   // var str = '{"TestId":"'+procVal+'","Comments":"'+comments+'","PerformedDate":"'+pdate+'","Result":"'+result+'","Unit":"'+unit+'","DeleteFlag":false,"UserId":"920a72ca-fca5-4f93-b42a-42911bc26397"}';
     //  alert(str);
        $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
            data: '{"TestId":"'+procVal+'","Comments":"'+comments+'","PerformedDate":"'+ pdate +'","Result":"'+result+'","Unit":"'+unit+'","DeleteFlag":false,"UserId":"'+UserId+'","SourceId":"2"}',
            url: u+"api/Lab/AddLabTest",
        success: function (data) {
          //  alert(JSON.stringify(data));
            $("#UserProfile tr").remove();
            getData();
            $("#TestsList").css("display","none");
        }
    });
}

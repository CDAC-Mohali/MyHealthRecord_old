/**
 * Created by TheWorkStation on 30-06-2016.
 */
    $.ajax({
        type       : "GET",
        contentType: "application/json; charset=utf-8",
        url        : u+"api/HealthCondition/GetHealthConditionData/" + UserId,
        success    : function (data) {
            //alert(JSON.stringify(data));
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
                    "<td>"+arr.response[i].HealthCondition+"</td>"+
                    "<td>"+arr.response[i].strStillHaveCondition+"</td>"+
                    "<td>"+arr.response[i].strDiagnosisDate+"</td>"+
                    "<td>"+arr.response[i].Notes+"</td>"+
                    "<td>"+arr.response[i].Provider+"</td>"+
                    "</tr>");
            }
        }
    });

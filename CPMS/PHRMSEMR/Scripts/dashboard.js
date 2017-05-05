/**
 * Created by TheWorkStation on 23-06-2016.
 */
function getAppointmentListing(x, b, u){
    console.log(x);
    var d = new Date();
    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();
    var date = curr_year + "-" + curr_month + "-" + curr_date;
    //console.log(date);
    //console.log('{"DocId":"'+ x +'", "Date":"'+ date +'"}');
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: '{"DocId":"'+ x +'", "Date":"'+ date +'"}',
        url: u+"api/Doctor/GetAppointmentList",
        success: function(data){
            //console.log(JSON.stringify(data));
            var arr, str, c;
            c = data.data.length;
            str = JSON.stringify(data.data);
            arr = JSON.parse(str);
            //console.log(c);
            var i, vis;
            for (i=0; i<c;i++){
                if(data.data[i].Visited == true){
                    vis = "  <a href='#'><i class=\"fa fa-check text-success\"></i></a>";
                }
                else{
                    vis = "  <a href='#'><i class=\"glyphicon glyphicon-remove text-navy\"></i></a>";
                }
                $("#AppointmentList").append("<tr><td style='color: red;'><a href=\"javascript: reVisit('"+data.data[i].MobileNo+"',b, u, x);\">"+data.data[i].strPatientName+"</a></td>"+
                    "<td>"+data.data[i].Hours+":"+data.data[i].Mins+" "+data.data[i].meridiem+"</td>"+
                    "<td>"+data.data[i].MobileNo+"</td>"+
                    "<td>"+data.data[i].Age+"/"+data.data[i].Gender+"</td>"+
                    "<td>"+vis+"</td>"+
                    "</tr>");
            }
        },
        error: function (error){

        }
    });
}

function reVisit(m, b, u, d){
    toastr.info("Contacting Server to Fetch Data ...","Fetching Data");
    console.log('{"DocId":"'+d+'","MobileNo":"'+m+'"}');
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data:  '{"DocId":"'+d+'","MobileNo":"'+m+'"}',
        url: u+"api/Account/GetRegisteredUser",
        success: function (data){
            //alert(JSON.stringify(data));
            if(data.status == 0){
                toastr.error("We Did not Find the User in Our Records. Kindly Register the User to Generate Records and Prescriptions.", "Oops! User Not Found!");
            }
            else if(data.status == 1){
                var cStr = getCounters(data.response.UserId, u);
                window.location=b+"WalkIn/StartSession/type/old/email/"+data.response.Email+"/phone/"+data.response.MobileNo+"/uid/"+data.response.UserId+"/fname/"+data.response.FirstName+"/lname/"+data.response.LastName+"/age/"+data.response.Age+"/sex/"+data.response.strGender + cStr;
            }
            else{
                toastr.error("Something Went Wrong!","Ouch!");
            }
        }
    });
}
function getCounters(UserId, u){
    var AllergyCounter=0, ImmunizationCounter=0, LabsCounter=0, ProcedureCounter= 0, ConditionsCounter=0;
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: u+"api/Allergy/GetAllergyData/" + UserId,
        async: false,
        success: function (data) {
            AllergyCounter = data.response.length;
        }
    });
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: u+"api/Immunization/GetImmunizationData/" + UserId,
        async: false,
        success: function (data) {
            ImmunizationCounter = data.response.length;
        }
    });
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: u+"api/Lab/GetLabData/" + UserId,
        async: false,
        success: function (data) {
            LabsCounter = data.response.length;
        }
    });
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: u+"api/Procedures/GetProcedureData/" + UserId,
        async: false,
        success: function (data) {
            ProcedureCounter = data.response.length;
        }
    });
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: u+"api/HealthCondition/GetHealthConditionData/" + UserId,
        async: false,
        success: function (data) {
            ConditionsCounter = data.response.length;
        }
    });
    var CounterStr = "/AllergyCounter/"+AllergyCounter+"/ProcedureCounter/"+ProcedureCounter+"/ImmunizationCounter/"+ImmunizationCounter+"/LabsCounter/"+LabsCounter+"/ConditionCounter/"+ConditionsCounter;
    return CounterStr;
}

function getGraphData(u,d){
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: u+"api/Account/GetWonthwisePatientCount",
        data: JSON.stringify(d),
        success: function (data) {
            console.log(JSON.stringify(data));
        }
    });
}
$(function () {

    /**
     * Flot charts data and options
     */
    var data1 = [ [1, 55], [2, 48], [3,40], [3, 36], [4, 40], [5, 60], [6, 50], [7, 51], [7, 51], [8, 51], [9, 51], [11, 51], [12, 51] ];
    //var data2 = [ [0, 56], [1, 49], [2, 41], [3, 38], [4, 46], [5, 67], [6, 57], [7, 59] ];

    var chartUsersOptions = {
        series: {
            splines: {
                show: true,
                tension: 0.4,
                lineWidth: 1,
                fill: 0.4
            },
        },
        grid: {
            tickColor: "#f0f0f0",
            borderWidth: 1,
            borderColor: 'f0f0f0',
            color: '#6a6c6f'
        },
        colors: [ "#62cb31", "#efefef"],
    };

    $.plot($("#flot-line-chart"), [data1], chartUsersOptions);

    /**
     * Flot charts 2 data and options
     */
    /*var chartIncomeData = [
        {
            label: "line",
            data: [ [1, 10], [2, 26], [3, 16], [4, 36], [5, 32], [6, 51] ]
        }
    ];

    var chartIncomeOptions = {
        series: {
            lines: {
                show: true,
                lineWidth: 0,
                fill: true,
                fillColor: "#64cc34"

            }
        },
        colors: ["#62cb31"],
        grid: {
            show: false
        },
        legend: {
            show: false
        }
    };

    $.plot($("#flot-income-chart"), chartIncomeData, chartIncomeOptions);

*/

});

(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
        (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
    m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
})(window,document,'script','//www.google-analytics.com/analytics.js','ga');

ga('create', 'UA-4625583-2', 'webapplayers.com');
ga('send', 'pageview');

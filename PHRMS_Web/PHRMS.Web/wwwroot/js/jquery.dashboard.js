function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}
$(document).ready(function () {


    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: '{"Text" : "mwbY7bDGdBhtrnTQYOigeChUmc1K3QTnAUfEgGFgAWt88hKA6aCRIXhxnQ1yg3BCayK44EWdkUQcBByEQChFXfCB776aQsG0BIlQgQgE8qO26X1h8cEUep8ngRBnOy74E9QgRgEAC8SvOfQkh7FDBDmS43PmGoIiKUUEGkMEC/PJHgxw0xH74yx/3XnaYRJgMB8obxQW6kL9QYEJ0FIFgByfIL7/IQAlvQwEpnAC7DtLNJCKUoO/w45c44GwCXiAFB/OXAATQryUxdN4LfFiwgjCNYg+kYMIEFkCKDs6PKAIJouyGWMS1FSKJOMRB/BoIxYJIUXFUxNwoIkEKPAgCBZSQHQ1A2EWDfDEUVLyADj5AChSIQW6gu10bE/JG2VnCZGfo4R4d0sdQoBAHhPjhIB94v/wRoRKQWGRHgrhGSQJxCS+0pCZbEhAAOw==", "userID" : "6c9ecd43-1300-4c02-8a06-c1a56f6f098f"}',
        url: ServiceURL + "api/Account/UpdateUserImage",
        success: function (data) {
            //   alert(JSON.stringify(data));
        }
    });


    //alert('{"TestId":"2","Comments":"Test","PerformedDate":"01/04/2016","Result":"123","Unit":"mg","DeleteFlag":false,"UserId":"920a72ca-fca5-4f93-b42a-42911bc26397"}');

    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/UpdateAnalytics",
        success: function (data) {
            //console.log(data);
            document.getElementById("allergies").innerHTML = data["AllergiesCount"];
            document.getElementById("immunization").innerHTML = data["ImmunizationsCount"];
            document.getElementById("labs").innerHTML = data["LabsCount"];
            document.getElementById("medications").innerHTML = data["MedicationsCount"];
            document.getElementById("procedures").innerHTML = data["ProceduresCount"];

            if (data["BPLatestSys"]) {
                document.getElementById("BPLatest").innerHTML = data["BPLatestSys"] + "/" + data["BPLatestDia"] + " mmHg";
            }
            if (data["GlucoseLatest"]) {
                document.getElementById("GlucoseLatest").innerHTML = data["GlucoseLatest"] + " mg/dl";
            }
            if (data["LastBPCollectionDate"]) {
                document.getElementById("LastBpCollectionDate").innerHTML = "- Last Collection Date: " + data["LastBPCollectionDate"];
            }
            if (data["LastGlucoseCollectionDate"]) {
                document.getElementById("LastGlucoseCollectionDate").innerHTML = "- Last Collection Date: " + data["LastGlucoseCollectionDate"];
            }

            if (data["LastActivityType"]) {
                document.getElementById("LastActivityType").innerHTML =
                   "- Last Activity : " + data["LastActivityType"]
            }
            if (data["LastActivityDistance"]) {
                document.getElementById("LastActivityDistance").innerHTML =
                      "Distance : " + data["LastActivityDistance"] + " " + "km"

            }
            if (data["LastActivityCollectionDate"]) {
                document.getElementById("LastActivityCollectionDate").innerHTML =
                " on: " + data["LastActivityCollectionDate"];
            }

        }
    });
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetLatestAllergies",
        success: function (data) {
            if (data.length == 2) {
                document.getElementById("AllergyFirst").innerHTML = data[0][0];
                document.getElementById("SeverityFirst").innerHTML = "- Severity: " + data[0][1];
                document.getElementById("AllergySecond").innerHTML = data[1][0];
                document.getElementById("SeveritySecond").innerHTML = "- Severity: " + data[1][1];
                document.getElementById("LastAllergyAddedOn").innerHTML = "-Added on: " + data[0][2];
            }
            else if (data.length == 1) {
                document.getElementById("AllergyFirst").innerHTML = data[0][0];
                document.getElementById("SeverityFirst").innerHTML = "- Severity: " + data[0][1];
                document.getElementById("LastAllergyAddedOn").innerHTML = "-Added on: " + data[0][2];
            }
        }
    });
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetLatestProcedures",
        success: function (data) {
            if (data.length == 2) {
                document.getElementById("ProcedureFirst").innerHTML = data[0][0];
                document.getElementById("ProcedureStartedOnFirst").innerHTML = "- Done on: " + data[0][1].substring(0, 9);
                document.getElementById("ProcedureSecond").innerHTML = data[1][0];
                document.getElementById("ProcedureStartedOnSecond").innerHTML = "- Done on: " + data[1][1].substring(0, 9);
            }
            else if (data.length == 1) {
                document.getElementById("ProcedureFirst").innerHTML = data[0][0];
                document.getElementById("ProcedureStartedOnFirst").innerHTML = "- Done on: " + data[0][1].substring(0, 9);
            }
        }
    });
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetLatestMedications",
        success: function (data) {
            if (data.length == 2) {

                document.getElementById("MedicationFirst").innerHTML = data[0][0];
                document.getElementById("MedicationStartedOnFirst").innerHTML = "- Prescribed Date: " + data[0][1].substring(0, 9);
                document.getElementById("MedicationSecond").innerHTML = data[1][0];
                document.getElementById("MedicationStartedOnSecond").innerHTML = "- Prescribed Date: " + data[1][1].substring(0, 9);
            }
            else if (data.length == 1) {
                document.getElementById("MedicationFirst").innerHTML = data[0][0];
                document.getElementById("MedicationStartedOnFirst").innerHTML = "- Prescribed Date: " + data[0][1].substring(0, 9);
            }
        }
    });
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetLatestLabs",
        success: function (data) {

            if (data.length == 2) {

                document.getElementById("TestFirst").innerHTML = data[0][0];
                document.getElementById("TestPerformedFirst").innerHTML = "- Date Performed: " + data[0][1].substring(0, 9);
                document.getElementById("TestSecond").innerHTML = data[1][0];
                document.getElementById("TestPerformedSecond").innerHTML = "- Date Performed: " + data[1][1].substring(0, 9);
            }
            else if (data.length == 1) {
                document.getElementById("TestFirst").innerHTML = data[0][0];
                document.getElementById("TestPerformedFirst").innerHTML = "- Date Performed: " + data[0][1].substring(0, 9);
            }
        }
    });
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetLatestImmunizations",
        success: function (data) {

            if (data.length == 2) {
                document.getElementById("ImmunizationFirst").innerHTML = data[0][0];
                document.getElementById("ImmunizationTakenOnFirst").innerHTML = "- Taken on: " + data[0][1].substring(0, 9);
                document.getElementById("ImmunizationSecond").innerHTML = data[1][0];
                document.getElementById("ImmunizationTakenOnSecond").innerHTML = "- Taken on: " + data[1][1].substring(0, 9);
            }
            else if (data.length == 1) {
                document.getElementById("ImmunizationFirst").innerHTML = data[0][0];
                document.getElementById("ImmunizationTakenOnFirst").innerHTML = "- Taken on: " + data[0][1].substring(0, 9);
            }

        }
    });
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetLatestActivities",
        success: function (data) {

            if (data) {
                document.getElementById("ActivitiesDistance").innerHTML = data + " Km";
            }
        }

    });

    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetGlucoseData",
        success: function (data) {
            if (data) {
                var glucosedata = [];
                var temp = {};
                for (var i in data) {
                    temp["label"] = data[i][1];
                    temp["value"] = parseInt(data[i][0]);
                    glucosedata.push(temp);
                    temp = {};
                }

            }
            else
                return 0;
            $(".glucose-chart-container").insertFusionCharts({
                type: "line",
                width: "350",
                height: "400",
                dataFormat: "json",
                dataSource: {
                    chart: {
                        caption: "Blood Glucose",
                        theme: "ocean"
                    },
                    data: glucosedata
                }

            });
        }
    });


    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetBPandPulseData",
        success: function (data) {
            var sysdata = [];
            var diadata = [];
            var pulsedata = [];

            if (data) {
                var temp = {};
                var categories = [];
                var category = [];
                for (var i in data) {
                    temp["label"] = data[i].Date;
                    category.push(temp);
                    temp = {};
                    temp["value"] = data[i].Systolic;
                    sysdata.push(temp);
                    temp = {};
                    temp["value"] = data[i].Diastolic;
                    diadata.push(temp);
                    temp = {};
                    temp["value"] = data[i].Pulse;
                    pulsedata.push(temp);
                }
                temp = {};
                temp["category"] = category;
                categories.push(temp);


            }
            else
                return 0;

            var dataset = [];
            if (sysdata != "" || diadata != "" || diadata != "") {

                dataset = [
                            {
                                "seriesname": "Systolic (in mmHg)",
                                "data": sysdata
                            },
                            {
                                "seriesname": "Diastolic (in mmHg)",

                                "showvalues": "0",
                                "data": diadata
                            },
                            {
                                "seriesname": "Pulse",
                                "renderas": "line",
                                "showvalues": "0",
                                "data": pulsedata
                            }
                ];
            }

            $(".bp-chart-container").insertFusionCharts({
                type: "mscombi2d",
                width: "350",
                height: "425",
                dataFormat: "json",
                dataSource: {
                    "chart": {
                        "caption": "Blood Pressure and Pulse",
                        "xaxisname": "Date",
                        "yaxisname": "Blood Pressure and Pulse",
                        "theme": "zune",
                    },
                    "categories": categories,
                    "dataset": dataset
                }
            });
        }
    });

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Wellness/GetBMIForGraph",
        success: function (data) {
            if (data) {
                var weightdata = [];
                var temp = {};
                $.each(data, function (key, value) {
                    var res = value.split(",");
                    temp["label"] = res[0];
                    temp["value"] = res[1];
                    weightdata.push(temp);
                    temp = {};
                });
            }
            else
                return 0;
            $(".activity-chart-container").insertFusionCharts({
                type: "line",
                width: "350",
                height: "400",
                dataFormat: "json",
                dataSource: {
                    "chart": {
                        "caption": "BMI",
                        "theme": "ocean",
                    },
                    "data": weightdata
                }
            });
        }
    });


    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetActivityData",
        success: function (data) {

            if (data) {
                var weightdata = [];
                var temp = {};
                for (var i in data) {
                    if (data[i][3] == "4") {
                        temp["label"] = data[i][1];
                        temp["value"] = parseInt(data[i][0]);
                        weightdata.push(temp);
                        temp = {};
                    }
                }

            }
            else
                return 0;
            //$(".activity-chart-container").insertFusionCharts({
            //    type: "column2d",
            //    width: "350",
            //    height: "400",
            //    dataFormat: "json",
            //    dataSource: {
            //        chart: {
            //            caption: "Steps Taken",
            //            theme: "fint"
            //        },
            //        data: weightdata
            //    }
        }
    });

    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetActivityData",
        success: function (data) {
            var swimming = cycling = running = walking = 0;
            for (var i = 0; i < data.length ; i++) {

                switch (data[i][3]) {

                    case "1": walking += parseFloat(data[i][0]);
                        break;
                    case "2": running += parseFloat(data[i][0]);
                        break;
                    case "3": cycling += parseFloat(data[i][0]);
                        break;
                    case "4": swimming += parseFloat(data[i][0]);
                        break;
                        //case "5": swimming += parseFloat(data[i][0]);

                    default: break;
                }

            }
            var weightdata = [];

            if (data != "") {
                var temp = {};
                temp["label"] = "Walking + Steps";
                temp["value"] = walking;
                weightdata.push(temp);
                temp = {};
                temp["label"] = "Running";
                temp["value"] = running;
                weightdata.push(temp);
                temp = {};
                temp["label"] = "Cycling";
                temp["value"] = cycling;
                weightdata.push(temp);
                temp = {};
                //temp["label"] = "steps";
                //temp["value"] = steps;
                //weightdata.push(temp);
                //temp = {};
                temp["label"] = "Swimming";
                temp["value"] = swimming;
                weightdata.push(temp);
            }

            $("#donut_single").insertFusionCharts({
                type: "pie2d",
                width: "425",
                height: "425",
                dataFormat: "json",
                dataSource: {
                    chart: {
                        caption: "Activities",

                        startingangle: "120",
                        showlabels: "0",
                        showlegend: "1",
                        enablemultislicing: "0",
                        slicingdistance: "15",
                        showpercentvalues: "1",
                        showpercentintooltip: "0",
                        plottooltext: "Activity : $label Total Distance : $datavalue",
                        theme: "fint"
                    },
                    data: weightdata
                }
            });
            walking = 0; swimming = 0; cycling = 0; running = 0; //steps = 0;

        }
    })

    GetHealthTip();
});


(function () {

    setTimeout(function () {
        var aTags = document.getElementsByTagName("tspan");
        var searchText = "FusionCharts XT Trial";
        var found;
        for (var i = 0; i < aTags.length; i++) {
            if (aTags[i].textContent == searchText) {
                found = aTags[i];
                found.innerHTML = " ";

            }

        }
    }, 450);
})();

function GetHealthTip() {
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: ROOT + "Dashboard/GetHealthTip",
        success: function (data) {
            if (data) {
                //alert(data);
                $('#tipHeader').addClass('animated infinite bounceIn');
                $.each(data.split(''), function (i, letter) {

                    //we add 100*i ms delay to each letter 
                    setTimeout(function () {

                        //we add the letter to the container
                        $('#healthTipCtrl').html($('#healthTipCtrl').html() + letter);

                    }, 50 * i);
                });
            }
        }
    });
}











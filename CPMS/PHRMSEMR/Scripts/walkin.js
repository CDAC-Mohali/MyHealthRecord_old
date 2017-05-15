/**
 * Created by TheWorkStation on 23-06-2016.
 */
/* $(document).ready(function(){
    $('input[type="radio"]').click(function(){
        if($(this).attr("value")=="new-patient"){
            $("#re-visit").hide();
            $("#ptable").hide();
            $("#new-patient").show();
        }
        if($(this).attr("value")=="re-visit"){
            $("#new-patient").hide();
            $("#re-visit").show();
            $("#ptable").show();
        }
    });
}); // */
 function getPatientListing(d,u,b){
//     $.ajax({
//         type: "POST",
//         contentType: "application/json; charset=utf-8",
//         data: JSON.stringify(d),
//         url: u+"api/Account/GetPatientsByDocId",
//         success: function (data){
//            //alert(JSON.stringify(data));
//              var c, str, arr;
//             str = JSON.stringify(data);
//             c = data.length;
//             arr = JSON.parse(str);
//             for (var k=0; k<c;k++){
//                 $("#pDataBody").append("<tr>"+
//                     "<td>"+(k+1)+"</td>"+
//                     "<td><a href=\"javascript: reVisit('"+arr[k].MobileNo+"','"+b+"','"+u+"','"+d+"');\" style='color: blue;'>"+arr[k].FirstName+"</a></td>"+
//                     "<td>"+arr[k].MobileNo+"</td>"+
//                     "<td>"+arr[k].Email+"</td>"
//                     +"</tr>");
//             }
//         }
//     });
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
function submitNewPatient(oForm,b,u,d){
    var fname = oForm.elements['fname'].value;
    var lname = oForm.elements['lname'].value;
    var address = oForm.elements['address'].value;
    var dob = oForm.elements['dob'].value;
    var sex = oForm.elements['sex'].value;
    var ph = oForm.elements['phone_number'].value;
    var email = oForm.elements['email'].value;
    var ano = oForm.elements['ano'].value;
    var pass = "password";
    console.log( '{"AadhaarNo":"' + ano + '","FirstName": "' + fname + '","LastName": "' + lname + '","Password":"' + pass + '","Email":"' + email + '","MobileNo":"' + ph + '","DOB":"' + dob + '","strGender":"' + sex + '","DocId":"'+d+'"}');
    if(email.length> 0 || ph.length> 0) {
        toastr.info("Sending Data to Server... Checking for Previous Records...", "Pushing Data & Validating");
        $.ajax({
            url: u+'api/account/Register',
            type: 'Post',
            data: '{"AadhaarNo":"' + ano + '","FirstName": "' + fname + '","LastName": "' + lname + '","Password":"' + pass + '","Email":"' + email + '","MobileNo":"' + ph + '","DOB":"' + dob + '","strGender":"' + sex + '","DocId":"'+d+'"}',
            datatype: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //alert(JSON.stringify(data));
                //alert(data.response);
                if(data.status == 1){
                    var cStr = getCounters(data.response);
                    window.location=b+"WalkIn/StartSession/type/old/email/"+email+"/phone/"+ph+"/uid/"+data.response+"/fname/"+fname+"/lname/"+lname+"/age/"+dob+"/sex/"+sex + cStr;
                }
                else{
                    toastr.error("User already exists! Unable to Register Patient.","Oops!");
                }
            },
            error: function (request, status, err) {
                console.log("R:"+request+" S:"+status+" E:"+err);
                toastr.error("Something Went Wrong!","Oops!");
            }
        });
    }
    else{
        if(ph.length === 0) {
            $("#helpBlock1").text("Please Enter Patient's Phone Number");
        }
        if(email.length === 0) {
            $("#helpBlock2").text("Please Enter Patient's Email");
        }
        if(dob.length === 0) {
            $("#helpBlock3").text("Please Enter Patient's Age");
        }
        if(sex.length === 0) {
            $("#helpBlock4").text("Please Enter Patient's Sex");
        }
    }
}

function submitPatientRequest(oForm,d,b,u){
    var data, flag = 0, date, Parser, tmp;
    var email = $("#Semail").val();
    var phone_number = $("#Sphone_number").val();
    if (phone_number !== "" || email !== "") {
        flag = 1;
    }
    else {
        $("#ModalTitle").text("Unable to Fetch User!");
        $("#ModalDetails").text("Kindly Enter Either The Email or Phone Number of the Patient!");
        $("#ModalBody").html("<center>*** No Patient Data Retrieved ***</center>");
        $("#myModal8").modal('show');        
    }
    console.log(flag);
    if(flag){
        //toastr.info("Requesting Server for Data ...", "Fetching Records ...");
        console.log('{"DocId":"'+d+'","PhoneNumber":"'+phone_number+'","EmailAddress":"'+email+'"}');
        $.ajax({
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: '{"DocId":"'+d+'","PhoneNumber":"'+phone_number+'","EmailAddress":"'+email+'"}',
            url: u+"api/Account/CheckPatientByDoc",
            async: false,
            success: function (data){
                //alert(JSON.stringify(data));
                if(data.status === 0){
                    $("#ModalTitle").text("Oops! Something went Wrong!");
                    $("#ModalDetails").text("Server Error 500");
                    $("#ModalBody").html("<center>*** No Patient Data Retrieved ***</center>");
                    $("#myModal8").modal('show');                
                }
                else if(data.status == 1 && data.response.DocPatientId !== 0){
                    tmp = ((data.response.Gender === null || data.response.Gender == "null") ? "U" : data.response.Gender);
                    $("#ModalTitle").text("Patient: "+((data.response.FirstName === null || data.response.FirstName == "null") ? "" : data.response.FirstName)+" "+((data.response.LastName === null || data.response.LastName == "null") ? "" : data.response.LastName));
                    date = data.response.DOB.split("T");
                    $("#ModalDetails").html("<b>DoB</b>: "+ ((date[0] === null || date[0] == "null") ? "-" : date[0])+" | <b>Gender</b>: "+tmp+" | <b>Phone</b>: "+((data.response.PhoneNumber === null || data.response.PhoneNumber == "null") ? "-" : data.response.PhoneNumber)+" | <b>Email</b>: "+((data.response.EmailAddress === null || data.response.EmailAddress == "null") ? "-" : data.response.EmailAddress));   
                    $("#fname").val(((data.response.FirstName === null || data.response.FirstName == "null") ? "-" : data.response.FirstName)) ;     
                    $("#lname").val(((data.response.LastName === null || data.response.LastName == "null") ? "-" : data.response.LastName)) ;   
                    $("#dob").val(((date[0] === null || date[0] == "null") ? "-" : date[0])) ;   
                    $("#phone_number").val(((data.response.PhoneNumber === null || data.response.PhoneNumber == "null") ? "-" : data.response.PhoneNumber)) ;   
                    $("#email").val(((data.response.EmailAddress === null || data.response.EmailAddress == "null") ? "-" : data.response.EmailAddress)) ;   
                    $('input[value="'+tmp+'"]').attr("checked","checked");
                    $("#address1").val(((data.response.Address1 === null || data.response.Address1 == "null") ? "-" : data.response.Address1)) ;   
                    $("#address2").val(((data.response.Address2 === null || data.response.Address2 == "null") ? "-" : data.response.Address2)) ;   
                    $("#ano").val(((data.response.AadhaarNumber === null || data.response.AadhaarNumber == "null") ? "-" : data.response.AadhaarNumber)) ;   
                    console.log(JSON.stringify(data));
                    $("#myModal8").modal('show');  
                    Parser = '{"DocPatientId":"'+data.response.DocPatientId+'","DocId":"'+d+'","CreatedDate":"'+data.response.CreatedDate+'","ModifiedDate":"'+data.response.ModifiedDate+'","FirstName":"'+data.response.FirstName+'","LastName":"'+data.response.LastName+'","DOB":"'+data.response.DOB+'","Gender":"'+tmp+'","PhoneNumber":"'+data.response.PhoneNumber+'","EmailAddress":"'+data.response.EmailAddress+'","AadhaarNumber":"'+data.response.AadhaarNumber+'","Address1":"'+data.response.Address1+'","Address2":"'+data.response.Address2+'","City_Vill_Town":"'+data.response.City_Vill_Town+'","District":"'+data.response.District+'","State":"'+data.response.State+'"}';
                    $("#RegPatient").append("<input type='hidden' value='"+Parser+"' name='Parser' id='Parser' />");
                    $("#PatientRecords").show();  

                }
                else if(data.status == 1 && data.response.DocPatientId == 0){
                    Parser = '{"DocPatientId":"'+data.response.DocPatientId+'","DocId":"'+d+'","CreatedDate":"'+data.response.CreatedDate+'","ModifiedDate":"'+data.response.ModifiedDate+'","FirstName":"'+data.response.FirstName+'","LastName":"'+data.response.LastName+'","DOB":"'+data.response.DOB+'","Gender":"'+data.response.Gender+'","PhoneNumber":"'+phone_number+'","EmailAddress":"'+data.response.EmailAddress+'","AadhaarNumber":"'+data.response.AadhaarNumber+'","Address1":"'+data.response.Address1+'","Address2":"'+data.response.Address2+'","City_Vill_Town":"'+data.response.City_Vill_Town+'","District":"'+data.response.District+'","State":"'+data.response.State+'"}';
                    $("#phone_number").val(phone_number).attr("readonly","readonly");
                    $("#RegPatient").append("<input type='hidden' value='"+Parser+"' name='Parser' id='Parser' />");
                    $("#PatientRecords").show();    
                    $("#myModal8").modal('show');                                        
                }
                else{
                    toastr.error("Something Went Wrong!","Ouch!");
                }
            },
            error: function(){
                    $("#ModalTitle").text("Oops! Something went Wrong!");
                    $("#ModalDetails").text("Server Error 500");
                    $("#ModalBody").html("<center>*** No Patient Data Retrieved ***</center>");
                    $("#myModal8").modal('show');      
            }
        });
    }
    else{

    }
}
function startSession(oForm,b,u,d){
    var strParser, oParser, cStr;
    var fname = $("#fname").val();
    var lname = $("#lname").val();
    var dob   = $("#dob").val();
    var add1  = $("#add1").val();
    var add2  = $("#add2").val();
    var ano  = $("#ano").val();
    var sex = $("#sex").val();
    console.log(sex);
    strParser = $("#Parser").val();
    console.log(strParser);
    oParser = jQuery.parseJSON(strParser);
    oParser.FirstName = fname;
    oParser.LastName  = lname;
    strParser = JSON.stringify(oParser);
    oParser.DOB       = dob;
    oParser.Address1  = add1;
    oParser.Address2  = add2;
    oParser.AadhaarNumber  = ano;
    console.log(oParser.Gender);
    strParser = JSON.stringify(oParser);
    console.log(strParser);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: strParser,
        url: u+"api/Account/CreateDocPatientMapping",
        success: function(data){
            //alert(JSON.stringify(data));
            if(data.Status){
                //Start Session
                cStr = getCounters(data.Id, u);                
                window.location=b+"WalkIn/StartSession/type/old/email/"+oParser.EmailAddress+"/phone/"+oParser.PhoneNumber+"/uid/"+data.Id+"/fname/"+oParser.FirstName+"/lname/"+oParser.LastName+"/age/"+oParser.DOB+"/sex/"+oParser.Gender + cStr;                
            }
            else{
                alert(JSON.stringify(data));
            }
        }
    });
}
function submitOldPatient(oForm,d,b,u){


    var email = oForm.elements['email'].value;
    var phone_number = oForm.elements['phone_number'].value;
    var data, flag;
    if(!(phone_number == null || phone_number == "")){
        data = phone_number;
        flag = 1;
    }
    else if(!(email == null || email == "")){
        data = email;
        flag = 1;
    }
    else {
        toastr.error("Kindly Enter Either The Email or Phone Number of the Patient!","Unable to Fetch User!");
        flag = 0;
    }
    if(flag){
        toastr.info("Requesting Server for Data ...", "Fetching Records ...");
        $.ajax({
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: '{"DocId":"'+d+'","MobileNo":"'+data+'"}',
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
    else{

    }
}

function paginate(pid){
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            // alert(xmlhttp.responseText);
            $("#pDataBody").html(xmlhttp.responseText);
        }
    };
    xmlhttp.open("GET", "paginate.php?pid=" + pid, true);
    xmlhttp.send();
}
//paginate(1);
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
$().ready(function(){
    $("#RegPatient").validate({
        rules: {
          fname: "required",
          lname: "required",
          dob : "required",
          sex : "required",
          phone_number: {
            required: true,
            digits: true,
            minlength: 10,
            maxlength: 10
          },
          email: {
            email: true
          }
        },
        messages: {
          fname: "Please Enter Your First Name",
          lname: "Please Enter Your Last Name",
          dob : "Please Enter Your DoB",
          sex : "Please Enter Sex",
          phone_number: {
            required: "Please Enter Your Phone Number",
            digits: "Invalid Phone Number",
            minlength: "Enter Valid Phone Number",
            maxlength: "Enter Valid Phone"
          },
          email: {
            email: "Invalid Email"
          }
        },
        errorPlacement: function(error, element) {
               $( element )
                       .closest( "form" )
                       .find( "label[for='" + element.attr( "id" ) + "']" )
                       .append( error );
           },
           errorElement: "span",

            submitHandler: function(form) {
                addDoc(form,'<?php echo $api_url; ?>');
            }
    });
});
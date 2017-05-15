/**
 * Created by TheWorkStation on 13/6/16.
 */
function addDoc(oForm){
    var name = oForm.elements['name'].value;
    var license = oForm.elements['license'].value;
    var dob = oForm.elements['dob'].value;
    var password = oForm.elements['password'].value;
    var phone_number = oForm.elements['phone_number'].value;
    var email = oForm.elements['email'].value;
    var certifications = oForm.elements['certifications'].value;
    alert('{"license_number":"'+license+'","name":"'+name+'","date_of_birth":"'+ dob +'","phone_number":"'+phone_number+'","email":"'+email+'","qualification_set":"'+certifications+'","password":"'+password+'"}');
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: '{"license_number":"'+license+'","name":"'+name+'","date_of_birth":"'+ dob +'","phone_number":"'+phone_number+'","email":"'+email+'","qualification_set":"'+certifications+'","password":"'+password+'"}',
        url: "http://10.228.13.36:8085/api/Doctor/RegisterDoctor",
        success: function (data) {
            alert(JSON.stringify(data));
            var reply = JSON.stringify(data);
            if(reply == "true"){
                alert("True");
                //window.location="application/assets/php/register.php?license="+license;
            }
            else {
                alert("False");
            }
        }
    });
}
function addPoP(license, oForm){
    var name = oForm.elements['name'];
    var pp = oForm.elements['pp'];
    var address = oForm.elements['address'];
    var city = oForm.elements['city'];
    var state = oForm.elements['state'];
    var pin = oForm.elements['pin'];
}
function getStateListing(){
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "http://10.228.13.36:8085/api/Doctor/GetStateList",
        success: function (data) {
            //alert(JSON.stringify(data));
            var str, arr;
            var c = data.length;
            str = JSON.stringify(data);
            arr = JSON.parse(str);
            var i;
            for (i=0; i<c;i++) {
                $("#state").append("<option value='" + data[i].Id + "'>" + data[i].Name +"</option>");
            }
        }
    });
}

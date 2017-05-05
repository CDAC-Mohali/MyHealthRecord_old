/**
 * Created by TheWorkStation on 13/6/16.
 */
var b64;
File.prototype.convertToBase64 = function(callback) {
    var reader = new FileReader();
    reader.onload = function(e) {
        callback(e.target.result)
    };
    reader.onerror = function(e) {
        callback(null);
    }; 

    reader.readAsDataURL(this);
};

$("#proof").on('change', function() {
    var selectedFile = this.files[0];
    selectedFile.convertToBase64(function(base64) {
        b64 = base64;
        //$("#test").append(b64);
    })
});

function addDoc(oForm, u) {
    if (!$("#addDoc").valid()) {
        return false;
    }
    toastr.info("Contacting Server To Push Register Data", "Saving Data ...");
    var name = oForm.elements['name'].value;
    var dob = oForm.elements['dob'].value;
    var sex = oForm.elements['sex'].value;
    var password = oForm.elements['password'].value;
    var phone_number = oForm.elements['phone_number'].value;
    var email = oForm.elements['email'].value;
    var certifications = oForm.elements['certifications'].value;
    console.log('{"name":"' + name + '","sex":"' + sex + '","date_of_birth":"' + dob + '","phone_number":"' + phone_number + '","email":"' + email + '","qualification_set":"' + certifications + '","password":"' + password + '"}');
    var data = ',"Doctor":{"name":"' + name + '","Gender":"' + sex + '","date_of_birth":"' + dob + '","phone_number":"' + phone_number + '","email":"' + email + '","qualification_set":"' + certifications + '","password":"' + password + '"}}';
    $("#addDoc").attr("action", act);
    $("#addDoc").append("<input type='hidden' value='" + data + "'name='Step1'/>");
    document.getElementById("addDoc").submit();
}

function addPoP(oForm, u) {
    var name = oForm.elements['name'].value;
    var license = oForm.elements['license'].value;
    var pp = oForm.elements['pp'].value;
    var address1 = oForm.elements['address1'].value;
    var address2 = oForm.elements['address2'].value;
    var city = oForm.elements['city'].value;
    var state = oForm.elements['state'].value;
    var pin = oForm.elements['pin'].value;
    var step1 = oForm.elements['data'].value;
    //console.log('{"docid":"' + uid + '","name":"' + name + '","private_practice":' + pp + ',"address":"' + address + '","city":"' + city + '","state":"' + state + '","pincode":"' + pin + '"}');
    var step2;
    step2 = '{"Place":{"licence_copy":"' + b64 + '","name":"' + name + '","private_practice":"' + pp + '","AddressLine1":"' + address1 + '","AddressLine2":"'+address2+'","city":"' + city + '","state":"' + state + '","pincode":"' + pin + '"}';
    //console.log(step2);
    console.log(step1);
    finalCall(u, step1, step2);
}

function finalCall(u, step1, step2) {
    console.log(step2+step1);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: step2+step1,
        url: u + "api/Doctor/RegisterDoctor",
        async: false,
        success: function(data) {
            toastr.success("Request Sent to Administrator for Apporval","Request Sent!");
        }
    });
}

function getStateListing(u) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: u + "api/Doctor/GetStateList",
        success: function(data) {
            //alert(JSON.stringify(data));
            var str, arr;
            var c = data.length;
            str = JSON.stringify(data);
            arr = JSON.parse(str);
            var i;
            for (i = 0; i < c; i++) {
                $("#state").append("<option value='" + data[i].Id + "'>" + data[i].Name + "</option>");
            }
        }
    });
}

function startRegTour() {
    // Instance the tour
    var tour = new Tour({
        backdrop: true,
        steps: [{
            element: "#DocForm",
            title: "Personal Details",
            content: "Enter Your Personal Details in the Fields Provided! The details will be Verified by Our Admin for approval. All Fields are Mendatory!"
        }, {
            element: "#Quals",
            title: "Qualifications",
            content: "Enter Your Qualifications Here as You Want Them to Appear on Generated Prescriptions"
        }]
    });

    // Initialize the tour
    tour.init();

    // Start the tour
    tour.start();
}

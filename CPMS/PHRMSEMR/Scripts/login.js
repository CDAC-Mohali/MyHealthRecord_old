/**
 * Created by TheWorkStation on 23-06-2016.
 */
function checkLogin(oForm, b, u){
    toastr.info("Contacting Server for Validation", "Authenticating ...");
    var arr, x=0;
    var user = oForm.elements['username'].value;
    var pass = oForm.elements['password'].value;
    console.log('{"UserName":"'+user+'","Password":"'+pass+'"}');
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: '{"UserName":"'+user+'","Password":"'+pass+'"}',
        url: u+"api/Doctor/CheckDoctorCredentials",
        success: function (data){
            //alert(JSON.stringify(data));
            var str = JSON.stringify(data);
            arr = JSON.parse(str);
            if(arr.status == true){
                //alert(arr.data.docid);
                window.location = b+"Login/setLoginSession/DocId/"+arr.data.docid+"/license_number/"+arr.data.license_number+"/phone_number/"+arr.data.phone_number+"/name/"+arr.data.name+"/qualification_set/"+arr.data.qualification_set+"/address/"+arr.pop.address+"/city/"+arr.pop.city+"/state/"+arr.pop.strState+"/pin/"+arr.pop.pincode+"/cname/"+arr.pop.name;
                toastr.success("Starting Session and Logging In!", "User Authenticated!");
            }
            else{
                toastr.error("Credentials Mismatched! Kindly Check Your Username & Password!", "Authentication Failed!");
            }
        },
        error: function (error){
            console.log(error);
            toastr.error("Something Went Wrong with the Server! Please try again Later!", "Oops!");
        }
    });
}

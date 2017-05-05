package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import org.json.JSONException;
import org.json.JSONObject;


public class ParseJson_DeviceRegistrationData
{
    public static String[] Id;

    // Userid is available in the prefrence
    public static String[] DeviceType;



    private JSONObject jsonData;

    public ParseJson_DeviceRegistrationData(JSONObject jsonData) {
        this.jsonData = jsonData;
    }

    public String parsePostResponseDeviceRegisteration() {
        String parse_response = "-1"; // Invalid response

        //JSONObject jsonObject=null;
        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    if (jsonData.getString("response").equals("0")) {
                        parse_response = "0"; // No data to change
                    } else {
                        parse_response = "-2"; // No data returned
                    }
                } else {
                    if (jsonData.getString("status").equals("1") && jsonData.getString("response").equals("1")) {
                        parse_response = "1"; // Data available Load List
                    }
                }
            } else {
                parse_response = "-3"; // Service doesn't sent any response
            }
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            //errormsg = e.getMessage();
            //e.printStackTrace();
            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }


}
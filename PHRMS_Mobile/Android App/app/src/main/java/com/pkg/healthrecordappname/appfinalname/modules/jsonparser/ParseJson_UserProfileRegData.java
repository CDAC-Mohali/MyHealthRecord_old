package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.json.JSONException;
import org.json.JSONObject;


public class ParseJson_UserProfileRegData {


    public static String REGGUID = "";

    private JSONObject jsonData;

    public ParseJson_UserProfileRegData(JSONObject jsonData) {
        this.jsonData = jsonData;
    }

    public String parsePostResponseUserProfileReg() {
        String parse_response = "-1"; // Invalid response


        try {
            if (jsonData != null) {
                if (!Functions.isNullOrEmpty(jsonData.getString("response"))) {
                    String st_post = jsonData.getString("response").toString();
                    switch (st_post) {
                        case "0":
                            // Server Exception
                            parse_response = jsonData.getString("response").toString();
                            break;
                        case "1":
                            // Success - TemRegistered
                            parse_response = jsonData.getString("response").toString();
                            REGGUID = jsonData.getString("Status").toString();
                            break;
                        case "2":
                            // Mobile - exist
                            parse_response = jsonData.getString("response").toString();
                            break;
                        case "3":
                            // Email - Exist
                            parse_response = jsonData.getString("response").toString();
                            break;
                        default:
                            parse_response = "-1"; // Invalid response from server
                            break;
                    }
                } else {
                    parse_response = "-2"; // server response empty
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

    public String parsePostResponseUserProfileRegOTPVerification() {

        String parse_response = "-11"; // Invalid response

        try {
            if (jsonData != null) {
                if (!Functions.isNullOrEmpty(jsonData.getString("response"))) {
                    if (jsonData.getString("response").equals("1")) {
                        if (!Functions.isNullOrEmpty(jsonData.getString("Status").toString())) {
                            parse_response = jsonData.getString("Status").toString();
                        }
                    } else {
                        parse_response = "0"; // Initial
                    }
                } else {
                    parse_response = "-12"; // no response
                }
            } else {
                parse_response = "-13"; // Service doesn't sent any response
            }
        } catch (JSONException e) {
            // TODO Auto-generated catch block

            parse_response = "-14"; // Json Parsing Error
        }

        return parse_response;
    }


    public String parsePostResponseUserProfileResendOTP() {
        String parse_response = "-1";
        try {
            if (jsonData != null) {
                if (!Functions.isNullOrEmpty(jsonData.getString("response"))) {
                    String st_post = jsonData.getString("response").toString();

                    switch (st_post) {
                        case "0":
                            // Resedn Failed
                            parse_response = jsonData.getString("response").toString();
                            break;
                        case "1":
                            // Resend Success -
                            parse_response = jsonData.getString("response").toString();
                            break;
                        default:
                            parse_response = "-1"; // Invalid response from server
                            break;
                    }
                } else {
                    parse_response = "-2"; // server response empty
                }
            } else {
                parse_response = "-3"; // Service doesn't sent any response
            }
        } catch (JSONException e) {
            // TODO Auto-generated catch block

            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }


}
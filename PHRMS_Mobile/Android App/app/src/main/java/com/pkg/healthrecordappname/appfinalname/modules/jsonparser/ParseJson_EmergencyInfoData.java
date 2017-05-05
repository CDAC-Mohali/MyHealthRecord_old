package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import org.json.JSONException;
import org.json.JSONObject;


public class ParseJson_EmergencyInfoData {
    public static String Id;

    public static String Primary_Emergency_Contact;
    public static String PC_Relationship;

    public static String PC_AddressLine1;
    public static String PC_AddressLine2;
    public static String PC_City_Vill_Town;
    public static String PC_District;
    public static String PC_State;

    public static String PC_Pin;
    public static String PC_Phone1;
    public static String PC_Phone2;



    private JSONObject jsonData;

    public ParseJson_EmergencyInfoData(JSONObject jsonData) {
        this.jsonData = jsonData;
    }

    public String parseJson() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                } else {
                    JSONObject jo = new JSONObject(jsonData.getString("response"));

                    if (jo.length() > 0) {
                        Id = jo.getString("Id");
                        Primary_Emergency_Contact = jo.getString("Primary_Emergency_Contact");
                        PC_Relationship = jo.getString("PC_Relationship");
                        //strPC_Relationship = jo.getString("strPC_Relationship");
                        PC_AddressLine1 = jo.getString("PC_AddressLine1");
                        PC_AddressLine2 = jo.getString("PC_AddressLine2");
                        PC_City_Vill_Town = jo.getString("PC_City_Vill_Town");
                        PC_District = jo.getString("PC_District");
                        PC_State = jo.getString("PC_State");
                        //strPC_State = jo.getString("strPC_State");
                        PC_Pin = jo.getString("PC_Pin");
                        PC_Phone1 = jo.getString("PC_Phone1");
                        PC_Phone2 = jo.getString("PC_Phone2");
                        parse_response = "1"; // Data available Load List
                    } else {
                        parse_response = "-2"; // No data available from array
                    }
                }

            } else {
                parse_response = "-3"; // Service doesn't sent any response
            }
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            //errormsg = e.getMessage();
            e.printStackTrace();
            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }


}
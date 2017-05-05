package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import org.json.JSONException;
import org.json.JSONObject;



public class ParseJson_InsuranceInfoData {
    public static String Id;

    public static String Insu_Org_Name;
    public static String Insu_Org_Phone;
    public static String Insu_Org_Grp_Num;
    public static String ValidTill;
    public static String strValidTill;




    private JSONObject jsonData;

    public ParseJson_InsuranceInfoData(JSONObject jsonData) {
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
                        Insu_Org_Name = jo.getString("Insu_Org_Name");
                        Insu_Org_Phone = jo.getString("Insu_Org_Phone");
                        Insu_Org_Grp_Num = jo.getString("Insu_Org_Grp_Num");
                        ValidTill = jo.getString("ValidTill");
                        strValidTill = jo.getString("strValidTill");

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

    public String parsePostResponseInsurance()
    {
        String parse_response = "-1"; // Invalid response


        try {
            if (jsonData != null)
            {
                if (jsonData.getString("status").equals("0"))
                {
                    if (jsonData.getString("response").equals("0"))
                    {
                        parse_response = "0"; // No data to change
                    }
                    else
                    {
                        parse_response = "-2"; // No data returned
                    }
                }
                else
                {
                    if (jsonData.getString("status").equals("1") && jsonData.getString("response").equals("1"))
                    {
                        parse_response = "1"; // Data available Load List
                    }
                }
            }
            else
            {
                parse_response = "-3"; // Service doesn't sent any response
            }
        }
        catch (JSONException e)
        {
            // TODO Auto-generated catch block

            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }

}
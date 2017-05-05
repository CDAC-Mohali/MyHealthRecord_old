package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import org.json.JSONException;
import org.json.JSONObject;



public class ParseJson_PrefrenceInfoData {
    public static String Id;

    public static String Pref_Hosp; // Preffered Hospital is changed to Hospital Name
    public static String Prim_Care_Prov; // Primary Care is changed to Hospital Address
    public static String Special_Needs;



    private JSONObject jsonData;

    public ParseJson_PrefrenceInfoData(JSONObject jsonData) {
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
                        Pref_Hosp = jo.getString("Pref_Hosp");
                        Prim_Care_Prov = jo.getString("Prim_Care_Prov");
                        Special_Needs = jo.getString("Special_Needs");


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


            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }

    public String parsePostResponsePrefrence()
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
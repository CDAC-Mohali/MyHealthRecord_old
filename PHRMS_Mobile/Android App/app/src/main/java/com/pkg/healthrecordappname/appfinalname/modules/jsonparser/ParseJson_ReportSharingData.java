package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;


import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.LinkedHashMap;



public class ParseJson_ReportSharingData {



    public static LinkedHashMap hmReportSharingDoctorContact = new LinkedHashMap<String, String>();

    


    private JSONObject jsonData;

    public ParseJson_ReportSharingData(JSONObject jsonData) {
        this.jsonData = jsonData;
    }




    public String parseJsonReportSharingDocList() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0"))
                {
                    parse_response = "-1"; // No data available
                }
                else
                {
                    JSONArray jsonarray = new JSONArray(jsonData.getString("response"));
                    if (jsonarray.length() > 0) {
                        for (int i = 0; i < jsonarray.length(); i++) {
                            JSONObject jo = jsonarray.getJSONObject(i);

                            // Value as - PrimaryNumber_EmailAddress
                            // Update for no data.
                            String ContactValue = jo.getString("PrimaryPhone") + "_" + jo.getString("EmailAddress");

                            // Fill Spinner With:: Value as (PrimaryNumber_EmailAddress) and Text as (ContactName)
                            hmReportSharingDoctorContact.put(ContactValue, jo.getString("ContactName"));
                        }
                        parse_response = "1"; // Data available Load List
                    } else {
                        parse_response = "-2"; // No data available from array
                    }
                }
            }
            else
            {
                parse_response = "-3"; // Service doesn't sent any response
            }
        } catch (JSONException e) {
            // TODO Auto-generated catch block

            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }

    // Save - response
    public String parsePostResponseReportSharing() {
        String parse_response = "-1"; // Invalid response


        try {
            if (jsonData != null) {
                String Status = jsonData.getString("status").toString();
                // 0 - error
                parse_response = Status;

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
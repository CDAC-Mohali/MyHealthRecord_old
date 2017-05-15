package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;


public class ParseJson_WellnessBPData {

    public static String[] sno;
    public static String[] Id;

    public static String[] ResSystolic;
    public static String[] ResDiastolic;
    public static String[] GoalSystolic;
    public static String[] GoalDiastolic;
    public static String[] strCollectionDate;
    public static String[] CollectionDate;
    public static String[] CreatedDate;
    public static String[] GoalPulse;
    public static String[] ResPulse;
    public static String[] Comments;
    public static String[] strCreatedDate;
    public static String[] strModifiedDate;
    public static String[] DeleteFlag;
    public static String[] SourceId;


    private JSONObject jsonData;

    public ParseJson_WellnessBPData(JSONObject jsonData) {
        this.jsonData = jsonData;
    }

    public String parseJson() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                } else {
                    JSONArray jsonarray;
                    if (Functions.emrData) {
                        // Shows PHR + EMR
                        jsonarray = new JSONArray(jsonData.getString("response"));
                    } else {
                        // Never Shows EMR DATA
                        jsonarray = new JSONArray();
                        JSONArray jsonTemp = new JSONArray(jsonData.getString("response"));


                        if (jsonTemp != null && jsonTemp.length() > 0) {

                            for (int i = 0; i < jsonTemp.length(); i++) {
                                JSONObject jo = jsonTemp.getJSONObject(i);
                                //Exclude Data from SourceId = id here // EMR Application Data
                                if (!jo.getString("SourceId").equals("2")) {
                                    jsonarray.put(jsonTemp.get(i));
                                }
                            }
                        }
                    }


                    if (jsonarray != null && jsonarray.length() > 0) {
                        sno = new String[jsonarray.length()];
                        Id = new String[jsonarray.length()];
                        ResSystolic = new String[jsonarray.length()];
                        ResDiastolic = new String[jsonarray.length()];
                        GoalSystolic = new String[jsonarray.length()];
                        GoalDiastolic = new String[jsonarray.length()];
                        strCollectionDate = new String[jsonarray.length()];
                        CollectionDate = new String[jsonarray.length()];
                        CreatedDate = new String[jsonarray.length()];
                        GoalPulse = new String[jsonarray.length()];
                        ResPulse = new String[jsonarray.length()];
                        Comments = new String[jsonarray.length()];
                        strCreatedDate = new String[jsonarray.length()];
                        strModifiedDate = new String[jsonarray.length()];
                        DeleteFlag = new String[jsonarray.length()];
                        SourceId = new String[jsonarray.length()];


                        for (int i = 0; i < jsonarray.length(); i++) {
                            JSONObject jo = jsonarray.getJSONObject(i);
                            sno[i] = jo.getString("sno");
                            Id[i] = jo.getString("Id");
                            ResSystolic[i] = jo.getString("ResSystolic");
                            ResDiastolic[i] = jo.getString("ResDiastolic");
                            GoalSystolic[i] = jo.getString("GoalSystolic");
                            GoalDiastolic[i] = jo.getString("GoalDiastolic");
                            strCollectionDate[i] = jo.getString("strCollectionDate");
                            CollectionDate[i] = jo.getString("CollectionDate");
                            CreatedDate[i] = jo.getString("CreatedDate");
                            GoalPulse[i] = jo.getString("GoalPulse");
                            ResPulse[i] = jo.getString("ResPulse");
                            Comments[i] = jo.getString("Comments");
                            strCreatedDate[i] = jo.getString("strCreatedDate");
                            strModifiedDate[i] = jo.getString("strModifiedDate");
                            DeleteFlag[i] = jo.getString("DeleteFlag");
                            SourceId[i] = jo.getString("SourceId");
                        }

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


    public String parsePostResponseWellnessBP() {
        String parse_response = "-1"; // Invalid response


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

            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }
}
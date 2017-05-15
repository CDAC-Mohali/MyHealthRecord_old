package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;




public class ParseJson_AllergyData {
    public static String[] Id;

    public static String[] AllergyType;
    public static String[] AllergyName;
    public static String[] Still_Have;
    public static String[] Severity;
    public static String[] strSeverity;
    public static String[] DurationId;

    public static String[] strDuration;
    public static String[] ModifiedDate;
    public static String[] CreatedDate;
    public static String[] Comments;
    public static String[] strCreatedDate;

    public static String[] strModifiedDate;
    public static String[] strStill_Have;
    public static String[] sno;
    public static String[] DeleteFlag;
    public static String[] SourceId;

    // Seekbarfrom
    public static String[] fromId;
    public static String[] fromDuration;

    // Seekbarfrom
    public static String[] severityId;
    public static String[] severityName;

    public static List<String> responseAllegryList;

    private JSONObject jsonData;

    public ParseJson_AllergyData(JSONObject jsonData) {
        this.jsonData = jsonData;
    }

    public String parseJson() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                }
                else
                {

                    JSONArray jsonarray;
                    if (Functions.emrData)
                    {
                        jsonarray =  new JSONArray(jsonData.getString("response"));
                    }
                    else
                    {
                        jsonarray = new JSONArray();
                        JSONArray jsonTemp = new JSONArray(jsonData.getString("response"));


                        if (jsonTemp != null && jsonTemp.length() > 0)
                        {

                            for (int i = 0; i < jsonTemp.length(); i++)
                            {
                                JSONObject jo = jsonTemp.getJSONObject(i);

                                if (!jo.getString("SourceId").equals("2"))
                                {
                                    jsonarray.put(jsonTemp.get(i));
                                }
                            }
                        }
                    }

                    if (jsonarray != null && jsonarray.length() > 0)
                    {
                        Id = new String[jsonarray.length()];
                        AllergyType = new String[jsonarray.length()];
                        AllergyName = new String[jsonarray.length()];
                        Still_Have = new String[jsonarray.length()];
                        Severity = new String[jsonarray.length()];
                        strSeverity = new String[jsonarray.length()];
                        DurationId = new String[jsonarray.length()];

                        strDuration = new String[jsonarray.length()];
                        ModifiedDate = new String[jsonarray.length()];
                        CreatedDate = new String[jsonarray.length()];
                        Comments = new String[jsonarray.length()];
                        strCreatedDate = new String[jsonarray.length()];
                        strModifiedDate = new String[jsonarray.length()];

                        strStill_Have = new String[jsonarray.length()];
                        sno = new String[jsonarray.length()];
                        DeleteFlag = new String[jsonarray.length()];
                        SourceId = new String[jsonarray.length()];

                        for (int i = 0; i < jsonarray.length(); i++) {
                            JSONObject jo = jsonarray.getJSONObject(i);


                            Id[i] = jo.getString("Id");
                            AllergyType[i] = jo.getString("AllergyType");
                            AllergyName[i] = jo.getString("AllergyName");
                            Still_Have[i] = jo.getString("Still_Have");
                            Severity[i] = jo.getString("Severity");
                            strSeverity[i] = jo.getString("strSeverity");
                            DurationId[i] = jo.getString("DurationId");

                            strDuration[i] = jo.getString("strDuration");
                            ModifiedDate[i] = jo.getString("ModifiedDate");
                            CreatedDate[i] = jo.getString("CreatedDate");
                            Comments[i] = jo.getString("Comments");
                            strCreatedDate[i] = jo.getString("strCreatedDate");
                            strModifiedDate[i] = jo.getString("strModifiedDate");

                            strStill_Have[i] = jo.getString("strStill_Have");
                            sno[i] = jo.getString("sno");
                            DeleteFlag[i] = jo.getString("DeleteFlag");
                            SourceId[i] = jo.getString("SourceId");
                        }
                        parse_response = "1"; // Data available Load List
                    }
                    else
                    {
                        parse_response = "-2"; // No data available from array
                    }
                }

            } else {
                parse_response = "-3"; // Service doesn't sent any response
            }
        } catch (JSONException e) {
            // TODO Auto-generated catch block

            e.printStackTrace();
            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }


    public String parseJsonFrom() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                } else {
                    JSONArray jsonarray = new JSONArray(jsonData.getString("response"));
                    if (jsonarray.length() > 0) {
                        fromId = new String[jsonarray.length()];
                        fromDuration = new String[jsonarray.length()];

                        for (int i = 0; i < jsonarray.length(); i++) {
                            JSONObject jo = jsonarray.getJSONObject(i);


                            fromId[i] = jo.getString("Id");
                            fromDuration[i] = jo.getString("Duration");


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

            e.printStackTrace();
            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }


    public String parseJsonSeverity() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                } else {
                    JSONArray jsonarray = new JSONArray(jsonData.getString("response"));
                    if (jsonarray.length() > 0) {
                        severityId = new String[jsonarray.length()];
                        severityName = new String[jsonarray.length()];

                        for (int i = 0; i < jsonarray.length(); i++) {
                            JSONObject jo = jsonarray.getJSONObject(i);


                            severityId[i] = jo.getString("Id");
                            severityName[i] = jo.getString("Severity");
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
            //errormsg = e.getMessage();
            e.printStackTrace();
            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }

    public String parsePostResponseAllergy()
    {
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
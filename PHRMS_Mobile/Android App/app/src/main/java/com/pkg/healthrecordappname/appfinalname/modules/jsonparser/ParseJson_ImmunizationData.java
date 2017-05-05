package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;


public class ParseJson_ImmunizationData
{
    public static String[] Id;

    public static String[] ImmunizationsTypeId;
    public static String[] ImmunizationName;
    public static String[] ImmunizationDate;
    public static String[] Comments;
    public static String[] strImmunizationDate;
    public static String[] DeleteFlag;
    
    public static String[] strCreatedDate;
    public static String[] strModifiedDate;
    public static String[] ModifiedDate;
    public static String[] CreatedDate;
    public static String[] SourceId;



    private JSONObject jsonData;

    public ParseJson_ImmunizationData(JSONObject jsonData)
    {
        this.jsonData = jsonData;
    }

    public String parseJson()
    {
        String parse_response = "-1";


        try
        {
            if (jsonData != null)
            {
                if (jsonData.getString("status").equals("0"))
                {
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
                                //Exclude Data from SourceId = id here // EMR Application Data
                                if (!jo.getString("SourceId").equals("2"))
                                {
                                    jsonarray.put(jsonTemp.get(i));
                                }
                            }
                        }
                    }


                    if (jsonarray !=null && jsonarray.length() > 0)
                    {
                        Id = new String[jsonarray.length()];
                        ImmunizationsTypeId = new String[jsonarray.length()];
                        ImmunizationName = new String[jsonarray.length()];
                        ImmunizationDate = new String[jsonarray.length()];
                        Comments = new String[jsonarray.length()];
                        strImmunizationDate = new String[jsonarray.length()];
                        DeleteFlag = new String[jsonarray.length()];
                        strCreatedDate = new String[jsonarray.length()];
                        strModifiedDate = new String[jsonarray.length()];
                        ModifiedDate = new String[jsonarray.length()];
                        CreatedDate = new String[jsonarray.length()];
                        SourceId = new String[jsonarray.length()];

                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);


                            Id[i] = jo.getString("Id");
                            ImmunizationsTypeId[i] = jo.getString("ImmunizationsTypeId");
                            ImmunizationName[i] = jo.getString("ImmunizationName");
                            ImmunizationDate[i] = jo.getString("ImmunizationDate");
                            Comments[i] = jo.getString("Comments");
                            strImmunizationDate[i] = jo.getString("strImmunizationDate");
                            DeleteFlag[i] = jo.getString("DeleteFlag");
                            strCreatedDate[i] = jo.getString("strCreatedDate");
                            strModifiedDate[i] = jo.getString("strModifiedDate");
                            ModifiedDate[i] = jo.getString("ModifiedDate");
                            CreatedDate[i] = jo.getString("CreatedDate");
                            SourceId[i] = jo.getString("SourceId");
                        }
                        parse_response = "1"; // Data available Load List
                    }
                    else
                    {
                        parse_response = "-2"; // No data available from array
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
            //errormsg = e.getMessage();
            e.printStackTrace();
            parse_response = "-4"; // Json Parsing Error
        }



        return parse_response;
    }


    public String parsePostResponseImmunization() {
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
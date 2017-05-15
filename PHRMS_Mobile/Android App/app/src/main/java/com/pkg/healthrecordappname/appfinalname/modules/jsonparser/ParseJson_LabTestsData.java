package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;




public class ParseJson_LabTestsData
{
    public static String[] Id;

    public static String[] sno;
    public static String[] TestId;
    public static String[] TestName;
    public static String[] PerformedDate;
    public static String[] strPerformedDate;
    public static String[] Result;
    public static String[] Unit;
    public static String[] Comments;
    public static String[] CreatedDate;
    public static String[] ModifiedDate;
    public static String[] strCreatedDate;
    public static String[] strModifiedDate;
    public static String[] DeleteFlag;
    public static String[] lstFiles;
    public static String[] arrImages;
    public static String[] SourceId;




    private JSONObject jsonData;

    public ParseJson_LabTestsData(JSONObject jsonData)
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
                        // Shows PHR + EMR
                        jsonarray =  new JSONArray(jsonData.getString("response"));
                    }
                    else
                    {
                        // Never Shows EMR DATA
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


                    if (jsonarray!=null && jsonarray.length() > 0)
                    {
                        Id = new String[jsonarray.length()];
                        sno = new String[jsonarray.length()];
                        TestId = new String[jsonarray.length()];
                        TestName = new String[jsonarray.length()];
                        PerformedDate = new String[jsonarray.length()];
                        strPerformedDate = new String[jsonarray.length()];
                        Result = new String[jsonarray.length()];
                        Unit = new String[jsonarray.length()];
                        Comments = new String[jsonarray.length()];
                        CreatedDate = new String[jsonarray.length()];
                        ModifiedDate = new String[jsonarray.length()];
                        strCreatedDate = new String[jsonarray.length()];
                        strModifiedDate = new String[jsonarray.length()];
                        DeleteFlag = new String[jsonarray.length()];
                        lstFiles = new String[jsonarray.length()];
                        arrImages = new String[jsonarray.length()];
                        SourceId = new String[jsonarray.length()];

                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);


                            Id[i] = jo.getString("Id");
                            sno[i] = jo.getString("sno");
                            TestId[i] = jo.getString("TestId");
                            TestName[i] = jo.getString("TestName");
                            PerformedDate[i] = jo.getString("PerformedDate");
                            strPerformedDate[i] = jo.getString("strPerformedDate");
                            Result[i] = jo.getString("Result");
                            Unit[i] = jo.getString("Unit");
                            Comments[i] = jo.getString("Comments");
                            CreatedDate[i] = jo.getString("CreatedDate");
                            ModifiedDate[i] = jo.getString("ModifiedDate");
                            strCreatedDate[i] = jo.getString("strCreatedDate");
                            strModifiedDate[i] = jo.getString("strModifiedDate");
                            DeleteFlag[i] = jo.getString("DeleteFlag");
                            lstFiles[i] = jo.getString("lstFiles");
                            arrImages[i] = jo.getString("arrImages");
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

            e.printStackTrace();
            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }

    // response with id for post with images
    public String parsePostResponseLabTests()
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
                }
                else
                {
                    if (jsonData.getString("status").equals("1") && !Functions.isNullOrEmpty(jsonData.getString("response")))
                    {
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
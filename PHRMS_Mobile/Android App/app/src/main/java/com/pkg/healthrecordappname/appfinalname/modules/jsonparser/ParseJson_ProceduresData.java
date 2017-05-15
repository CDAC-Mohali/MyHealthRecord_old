package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;


public class ParseJson_ProceduresData
{
    public static String[] Id;

    public static String[] ProcedureType;
    public static String[] ProcedureName;
    public static String[] StartDate;
    public static String[] strStartDate;
    public static String[] EndDate;
    public static String[] strEndDate;
    public static String[] Comments;
    public static String[] SurgeonName;
    public static String[] CreatedDate;
    public static String[] ModifiedDate;
    public static String[] strCreatedDate;
    public static String[] strModifiedDate;
    public static String[] sno;
    public static String[] DeleteFlag;
    public static String[] lstFiles;
    public static String[] arrImages;
    public static String[] SourceId;



    private JSONObject jsonData;

    public ParseJson_ProceduresData(JSONObject jsonData)
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
                        //int len = jsonA.length();

                        if (jsonTemp != null && jsonTemp.length() > 0)
                        {

                            for (int i = 0; i < jsonTemp.length(); i++)
                            {
                                JSONObject jo = jsonTemp.getJSONObject(i);
                                //Exclude Data from SourceId = ID here // EMR Application Data
                                if (!jo.getString("SourceId").equals("2"))
                                {
                                    jsonarray.put(jsonTemp.get(i));
                                }
                            }
                        }
                    }

                    //JSONArray jsonarray = new JSONArray(jsonData.getString("response"));
                    if (jsonarray != null && jsonarray.length() > 0)
                    {
                        Id = new String[jsonarray.length()];
                        ProcedureType = new String[jsonarray.length()];
                        ProcedureName = new String[jsonarray.length()];
                        StartDate = new String[jsonarray.length()];
                        strStartDate = new String[jsonarray.length()];
                        EndDate = new String[jsonarray.length()];
                        strEndDate = new String[jsonarray.length()];
                        Comments = new String[jsonarray.length()];
                        SurgeonName = new String[jsonarray.length()];
                        CreatedDate = new String[jsonarray.length()];
                        ModifiedDate = new String[jsonarray.length()];
                        strCreatedDate = new String[jsonarray.length()];
                        strModifiedDate = new String[jsonarray.length()];
                        sno = new String[jsonarray.length()];
                        DeleteFlag = new String[jsonarray.length()];
                        lstFiles = new String[jsonarray.length()];
                        arrImages = new String[jsonarray.length()];
                        SourceId = new String[jsonarray.length()];

                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);


                            Id[i] = jo.getString("Id");
                            ProcedureType[i] = jo.getString("ProcedureType");
                            ProcedureName[i] = jo.getString("ProcedureName");
                            StartDate[i] = jo.getString("StartDate");
                            strStartDate[i] = jo.getString("strStartDate");
                            EndDate[i] = jo.getString("EndDate");
                            strEndDate[i] = jo.getString("strEndDate");
                            Comments[i] = jo.getString("Comments");
                            SurgeonName[i] = jo.getString("SurgeonName");
                            CreatedDate[i] = jo.getString("CreatedDate");
                            ModifiedDate[i] = jo.getString("ModifiedDate");
                            strCreatedDate[i] = jo.getString("strCreatedDate");
                            strModifiedDate[i] = jo.getString("strModifiedDate");
                            sno[i] = jo.getString("sno");
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

            parse_response = "-4"; // Json Parsing Error
        }


        return parse_response;
    }


    // response with id for post with images
    public String parsePostResponseProcedures() {
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
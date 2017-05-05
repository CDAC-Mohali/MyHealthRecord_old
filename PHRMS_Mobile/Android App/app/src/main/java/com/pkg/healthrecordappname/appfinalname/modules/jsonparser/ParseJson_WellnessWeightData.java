package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;




public class ParseJson_WellnessWeightData {

    public static String[] sno;
    public static String[] Id;

    public static String[] Height;
    public static String[] Weight;
    public static String[] CollectionDate;
    public static String[] strCollectionDate;
    public static String[] Comments;

    public static String[] CreatedDate;
    public static String[] ModifiedDate;



    public static String[] DeleteFlag;
    public static String[] SourceId;



    private JSONObject jsonData;

    public ParseJson_WellnessWeightData(JSONObject jsonData) {
        this.jsonData = jsonData;
    }

    public String parseJson() {
        String parse_response = "-1";


        try {
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

                                if (!jo.getString("SourceId").equals("2"))
                                {
                                    jsonarray.put(jsonTemp.get(i));
                                }
                            }
                        }
                    }



                    if (jsonarray != null && jsonarray.length() > 0)
                    {
                        sno = new String[jsonarray.length()];
                        Id = new String[jsonarray.length()];
                        Height = new String[jsonarray.length()];
                        Weight = new String[jsonarray.length()];

                        CollectionDate = new String[jsonarray.length()];
                        strCollectionDate = new String[jsonarray.length()];
                        Comments = new String[jsonarray.length()];
                        CreatedDate = new String[jsonarray.length()];
                        ModifiedDate = new String[jsonarray.length()];

                        DeleteFlag = new String[jsonarray.length()];
                        SourceId = new String[jsonarray.length()];


                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);
                            sno[i] = jo.getString("sno");
                            Id[i] = jo.getString("Id");
                            Height[i] = jo.getString("Goal");    // Json data as Goal but used as Height
                            Weight[i] = jo.getString("Result"); // Json data as Goal but used as  Weight

                            CollectionDate[i] = jo.getString("CollectionDate");
                            strCollectionDate[i] = jo.getString("strCollectionDate");
                            Comments[i] = jo.getString("Comments");
                            CreatedDate[i] = jo.getString("CreatedDate");
                            ModifiedDate[i] = jo.getString("ModifiedDate");

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


            parse_response = "-4"; // Json Parsing Error
        }


        return parse_response;
    }

    public String parsePostResponseWellnessWeight() {
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
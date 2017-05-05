package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;




public class ParseJson_PrescriptionData
{
    public static String[] Id;

    public static String[] DocName;
    public static String[] ClinicName;
    public static String[] DocAddress;
    public static String[] DocPhone;
    public static String[] Prescription;
    public static String[] PresDate;
    public static String[] strPresDate;
    public static String[] FileName;
    public static String[] strDeleteFlag;
    public static String[] DeleteFlag;
    public static String[] CreatedDate;
    public static String[] strCreatedDate;
    public static String[] ModifiedDate;
    public static String[] strModifiedDate;
    public static String[] arrImages;
    public static String[] SourceId;



    private JSONObject jsonData;

    public ParseJson_PrescriptionData(JSONObject jsonData)
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
                    // No Check for EMR in Prescription SHow all Data

                    JSONArray jsonarray = new JSONArray(jsonData.getString("response"));
                    if (jsonarray.length() > 0)
                    {
                        Id = new String[jsonarray.length()];
                        DocName = new String[jsonarray.length()];
                        ClinicName = new String[jsonarray.length()];
                        DocAddress = new String[jsonarray.length()];
                        DocPhone = new String[jsonarray.length()];
                        Prescription = new String[jsonarray.length()];
                        PresDate = new String[jsonarray.length()];
                        strPresDate = new String[jsonarray.length()];
                        FileName = new String[jsonarray.length()];
                        strDeleteFlag = new String[jsonarray.length()];
                        DeleteFlag = new String[jsonarray.length()];
                        CreatedDate = new String[jsonarray.length()];
                        strCreatedDate = new String[jsonarray.length()];
                        ModifiedDate = new String[jsonarray.length()];
                        strModifiedDate = new String[jsonarray.length()];
                        arrImages = new String[jsonarray.length()];
                        SourceId = new String[jsonarray.length()];

                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);
                            Id[i] = jo.getString("Id");
                            DocName[i] = jo.getString("DocName");
                            ClinicName[i] = jo.getString("ClinicName");
                            DocAddress[i] = jo.getString("DocAddress");
                            DocPhone[i] = jo.getString("DocPhone");
                            Prescription[i] = jo.getString("Prescription");
                            PresDate[i] = jo.getString("PresDate");
                            strPresDate[i] = jo.getString("strPresDate");
                            FileName[i] = jo.getString("FileName");
                            strDeleteFlag[i] = jo.getString("strDeleteFlag");
                            DeleteFlag[i] = jo.getString("DeleteFlag");
                            CreatedDate[i] = jo.getString("CreatedDate");
                            strCreatedDate[i] = jo.getString("strCreatedDate");
                            ModifiedDate[i] = jo.getString("ModifiedDate");
                            strModifiedDate[i] = jo.getString("strModifiedDate");
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
    public String parsePostResponsePrescription() {
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
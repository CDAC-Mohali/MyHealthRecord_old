package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.LinkedHashMap;



public class ParseJson_MedicalContactData
{
    public static String[] Id;

    public static String[] sno;
    public static String[] ContactName;
    public static String[] MedContType;
    public static String[] strMedContType;
    public static String[] EmailAddress;
    public static String[] strCreatedDate;
    public static String[] PrimaryPhone;
    public static String[] DeleteFlag;
    public static String[] CreatedDate;
    public static String[] ModifiedDate;
    public static String[] Address1;
    public static String[] Address2;
    public static String[] CityVillage;
    public static String[] PIN;
    public static String[] District;
    public static String[] strState;
    public static String[] State;
    public static String[] ClinicName;
    public static String[] SourceId;


    public static LinkedHashMap hmMedicalContactType = new LinkedHashMap<String, String>();



    private JSONObject jsonData;

    public ParseJson_MedicalContactData(JSONObject jsonData) {
        this.jsonData = jsonData;
    }

    public String parseJson() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0"))
                {
                    parse_response = "-1"; // No data available
                }
                else
                {
                    JSONArray jsonarray;

                        // Shows PHR + EMR
                        jsonarray =  new JSONArray(jsonData.getString("response"));



                    if (jsonarray !=null && jsonarray.length() > 0)
                    {
                        Id = new String[jsonarray.length()];
                        sno = new String[jsonarray.length()];
                        ContactName = new String[jsonarray.length()];
                        MedContType = new String[jsonarray.length()];
                        strMedContType = new String[jsonarray.length()];
                        EmailAddress = new String[jsonarray.length()];
                        strCreatedDate = new String[jsonarray.length()];
                        PrimaryPhone = new String[jsonarray.length()];
                        DeleteFlag = new String[jsonarray.length()];
                        CreatedDate = new String[jsonarray.length()];
                        ModifiedDate = new String[jsonarray.length()];
                        Address1 = new String[jsonarray.length()];
                        Address2 = new String[jsonarray.length()];
                        CityVillage = new String[jsonarray.length()];
                        PIN = new String[jsonarray.length()];
                        District = new String[jsonarray.length()];
                        strState = new String[jsonarray.length()];
                        State = new String[jsonarray.length()];
                        ClinicName = new String[jsonarray.length()];


                        SourceId = new String[jsonarray.length()];

                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);
                            Id[i] = jo.getString("Id");
                            sno[i] = jo.getString("sno");
                            ContactName[i] = jo.getString("ContactName");
                            MedContType[i] = jo.getString("MedContType");
                            strMedContType[i] = jo.getString("strMedContType");
                            EmailAddress[i] = jo.getString("EmailAddress");
                            strCreatedDate[i] = jo.getString("strCreatedDate");
                            PrimaryPhone[i] = jo.getString("PrimaryPhone");
                            DeleteFlag[i] = jo.getString("DeleteFlag");
                            CreatedDate[i] = jo.getString("CreatedDate");
                            ModifiedDate[i] = jo.getString("ModifiedDate");
                            Address1[i] = jo.getString("Address1");
                            Address2[i] = jo.getString("Address2");
                            CityVillage[i] = jo.getString("CityVillage");
                            PIN[i] = jo.getString("PIN");
                            District[i] = jo.getString("District");
                            strState[i] = jo.getString("strState");
                            State[i] = jo.getString("State");
                            ClinicName[i] = jo.getString("ClinicName");
                            SourceId[i] = "4";

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



    public String parseJsonMedicalContactType()
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
                    JSONArray jsonarray = new JSONArray(jsonData.getString("response"));
                    if (jsonarray.length() > 0)
                    {
                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);

                            hmMedicalContactType.put(jo.getString("Value"),jo.getString("Text"));
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

    // Save - response with id for post with images
    public String parsePostResponseMedicalContacts()
    {
        String parse_response = "-1"; // Invalid response


        try {
            if (jsonData != null)
            {
                String Status = jsonData.getString("status").toString();




                parse_response = Status;

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

}
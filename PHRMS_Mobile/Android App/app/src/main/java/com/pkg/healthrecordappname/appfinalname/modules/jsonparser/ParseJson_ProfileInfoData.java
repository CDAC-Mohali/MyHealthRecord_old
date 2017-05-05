package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import org.json.JSONException;
import org.json.JSONObject;



public class ParseJson_ProfileInfoData {
    public static String Id;
    public static String FirstName;

    public static String LastName;
    public static String AddressLine1;
    public static String AddressLine2;
    public static String BloodType;

    public static String DiffAbled;
    public static String DOB;
    public static String strDOB;
    public static String Cell_Phone;
    public static String City_Vill_Town;
    public static String DAbilityType;

    public static String District;
    public static String Gender;

    public static String Pin;
    public static String State;

    public static String Uhid;
    public static String Email;
    public static String Home_Phone;



    private JSONObject jsonData;

    public ParseJson_ProfileInfoData(JSONObject jsonData) {
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

                    if (jo.length() > 0)
                    {
                        Id = jo.getString("Id");
                        FirstName = jo.getString("FirstName");
                        LastName = jo.getString("LastName");
                        AddressLine1 = jo.getString("AddressLine1");
                        AddressLine2 = jo.getString("AddressLine2");
                        BloodType = jo.getString("BloodType");

                        DiffAbled = jo.getString("DiffAbled");
                        DOB = jo.getString("DOB");
                        strDOB = jo.getString("strDOB");
                        Cell_Phone = jo.getString("Cell_Phone");
                        City_Vill_Town = jo.getString("City_Vill_Town");
                        DAbilityType = jo.getString("DAbilityType");

                        District = jo.getString("District");
                        Gender = jo.getString("Gender");

                        Pin = jo.getString("Pin");
                        State = jo.getString("State");

                        Uhid = jo.getString("Uhid");
                        Email = jo.getString("Email");
                        Home_Phone = jo.getString("Home_Phone");

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


    public String parsePostResponseProfile()
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
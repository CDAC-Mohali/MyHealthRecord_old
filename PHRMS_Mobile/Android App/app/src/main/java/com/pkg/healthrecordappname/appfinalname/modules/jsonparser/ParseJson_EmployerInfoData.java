package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import org.json.JSONException;
import org.json.JSONObject;



public class ParseJson_EmployerInfoData {
    public static String Id;

    public static String EmployerName;
    public static String EmpAddressLine1;
    public static String EmpAddressLine2;
    public static String EmpCity_Vill_Town;
    public static String EmpDistrict;
    public static String EmpState;

    public static String EmpPin;
    public static String EmployerPhone;
    public static String EmployerOccupation;
    public static String CUG;



    private JSONObject jsonData;

    public ParseJson_EmployerInfoData(JSONObject jsonData) {
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
                        EmployerName = jo.getString("EmployerName");
                        EmpAddressLine1 = jo.getString("EmpAddressLine1");
                        EmpAddressLine2 = jo.getString("EmpAddressLine2");
                        EmpCity_Vill_Town = jo.getString("EmpCity_Vill_Town");
                        EmpDistrict = jo.getString("EmpDistrict");
                        EmpState = jo.getString("EmpState");
                        //strState = jo.getString("strState");
                        EmpPin = jo.getString("EmpPin");
                        EmployerPhone = jo.getString("EmployerPhone");
                        EmployerOccupation = jo.getString("EmployerOccupation");
                        CUG = jo.getString("CUG");
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

    public String parsePostResponseEmployer()
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
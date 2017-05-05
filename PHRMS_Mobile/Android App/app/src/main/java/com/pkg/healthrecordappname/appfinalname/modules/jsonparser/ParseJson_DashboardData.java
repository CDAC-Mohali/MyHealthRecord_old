package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.LinkedHashMap;



public class ParseJson_DashboardData {
    public static String[] BG_Value;
    public static String[] BG_Date;

    public static String[] BP_SYS_Value;
    public static String[] BP_DIA_Value;
    public static String[] BP_PULSE_Value;
    public static String[] BP_Date;

    public static String[] BMI_Value;
    public static String[] BMI_Date;

    public static LinkedHashMap<String, Float> Activity_DATA = new LinkedHashMap<String, Float>();

    private JSONObject jsonData;

    public ParseJson_DashboardData(JSONObject jsonData) {
        this.jsonData = jsonData;
    }

    public String parseJsonBG()
    {


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
                    if (jsonData.getString("status").equals("1"))
                    {
                        JSONObject jo_response_BG = new JSONObject(jsonData.getString("response"));

                        if (jo_response_BG!= null && jo_response_BG.length() > 0)
                        {
                            JSONArray ja_BG;

                            // If EMR False - Use Temp Array to hide data from EMR
                            if (Functions.emrData)
                            {
                                ja_BG =  new JSONArray(jo_response_BG.getString("oBloodGluscoseViewModel"));
                            }
                            else
                            {
                                ja_BG = new JSONArray();
                                JSONArray jsonTempBG = new JSONArray(jo_response_BG.getString("oBloodGluscoseViewModel"));
                                //int len = jsonA.length();

                                if (jsonTempBG != null && jsonTempBG.length() > 0)
                                {
                                    //for (int i=0;i<len;i++)
                                    for (int i = 0; i < jsonTempBG.length(); i++)
                                    {
                                        JSONObject jo = jsonTempBG.getJSONObject(i);
                                        //Exclude Data from SourceId = 2 // EMR Application Data
                                        if (!jo.getString("SourceId").equals("2"))
                                        {
                                            ja_BG.put(jsonTempBG.get(i));
                                        }
                                    }
                                }
                            }


                            if (ja_BG != null && ja_BG.length() > 0)
                            {
                                BG_Value = new String[ja_BG.length()];
                                BG_Date = new String[ja_BG.length()];

                                for (int i = 0; i < ja_BG.length(); i++)
                                {
                                    JSONObject jo_BG = ja_BG.getJSONObject(i);
                                    BG_Value[i] = jo_BG.getString("Result");
                                    BG_Date[i] = jo_BG.getString("strCollectionDate");

                                }


                                parse_response = "1";

                            }
                            else
                            {
                                parse_response = "-2";
                            }
                        }
                        else
                        {
                            parse_response = "-2";// No data available from array
                        }
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
        } catch (JSONException e) {
            // TODO Auto-generated catch block

            e.printStackTrace();
            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }

    public String parseJsonBP() {

        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                }
                else
                {
                    if (jsonData.getString("status").equals("1"))
                    {
                        JSONObject jo_response_BP = new JSONObject(jsonData.getString("response"));

                        if (jo_response_BP!= null && jo_response_BP.length() > 0)
                        {

                            JSONArray ja_BP;

                            // If EMR False - Use Temp Array to hide data from EMR
                            if (Functions.emrData)
                            {
                                ja_BP =  new JSONArray(jo_response_BP.getString("oBloodPressureAndPulseViewModel"));
                            }
                            else
                            {
                                ja_BP = new JSONArray();
                                JSONArray jsonTempBP = new JSONArray(jo_response_BP.getString("oBloodPressureAndPulseViewModel"));
                                //int len = jsonA.length();

                                if (jsonTempBP != null && jsonTempBP.length() > 0)
                                {
                                    //for (int i=0;i<len;i++)
                                    for (int i = 0; i < jsonTempBP.length(); i++)
                                    {
                                        JSONObject jo = jsonTempBP.getJSONObject(i);
                                        //Exclude Data from SourceId = 2 // EMR Application Data
                                        if (!jo.getString("SourceId").equals("2"))
                                        {
                                            ja_BP.put(jsonTempBP.get(i));
                                        }
                                    }
                                }
                            }


                            if (ja_BP !=null && ja_BP.length() > 0)
                            {
                                BP_SYS_Value = new String[ja_BP.length()];
                                BP_DIA_Value = new String[ja_BP.length()];
                                BP_PULSE_Value = new String[ja_BP.length()];

                                BP_Date = new String[ja_BP.length()];

                                for (int i = 0; i < ja_BP.length(); i++) {
                                    JSONObject jo_BP = ja_BP.getJSONObject(i);

                                    BP_SYS_Value[i] = jo_BP.getString("ResSystolic");
                                    BP_DIA_Value[i] = jo_BP.getString("ResDiastolic");
                                    BP_PULSE_Value[i] = jo_BP.getString("ResPulse");

                                    BP_Date[i] = jo_BP.getString("strCollectionDate");
                                }



                                parse_response = "1";
                            }
                            else
                            {
                                parse_response = "-2";
                            }
                        }
                        else
                        {
                            parse_response = "-2";
                        }
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

    public String parseJsonAV() {

        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                } else {
                    if (jsonData.getString("status").equals("1"))
                    {
                        JSONObject jo_response_Activity = new JSONObject(jsonData.getString("response"));

                        if (jo_response_Activity != null && jo_response_Activity.length() > 0)
                        {

                            JSONArray js_Activity;

                            // If EMR False - Use Temp Array to hide data from EMR
                            if (Functions.emrData)
                            {
                                js_Activity =  new JSONArray(jo_response_Activity.getString("oActivityViewModel"));
                            }
                            else
                            {
                                js_Activity = new JSONArray();
                                JSONArray jsonTempActivity = new JSONArray(jo_response_Activity.getString("oActivityViewModel"));
                                //int len = jsonA.length();

                                if (jsonTempActivity != null && jsonTempActivity.length() > 0)
                                {
                                    //for (int i=0;i<len;i++)
                                    for (int i = 0; i < jsonTempActivity.length(); i++)
                                    {
                                        JSONObject jo = jsonTempActivity.getJSONObject(i);
                                        //Exclude Data from SourceId = 2 // EMR Application Data
                                        if (!jo.getString("SourceId").equals("2"))
                                        {
                                            js_Activity.put(jsonTempActivity.get(i));
                                        }
                                    }
                                }
                            }




                            if (js_Activity != null && js_Activity.length() > 0)
                            {


                                Float WL = 0f; // ST
                                Float RN = 0f;
                                Float CL = 0f;
                                Float SW = 0f;



                                for (int i = 0; i < js_Activity.length(); i++)
                                {
                                    JSONObject jo_AV = js_Activity.getJSONObject(i);
                                    //Walking + Steps
                                    if (!Functions.isNullOrEmpty(jo_AV.getString("ActivityName")) && jo_AV.getString("ActivityId").equals("1"))
                                    {
                                        WL += Float.parseFloat(jo_AV.getString("Distance"));

                                    }
                                    //Running
                                    if (!Functions.isNullOrEmpty(jo_AV.getString("ActivityName")) && jo_AV.getString("ActivityId").equals("2"))
                                    {
                                        RN += Float.parseFloat(jo_AV.getString("Distance"));

                                    }
                                    //Cycling
                                    if (!Functions.isNullOrEmpty(jo_AV.getString("ActivityName")) && jo_AV.getString("ActivityId").equals("3"))
                                    {
                                        CL += Float.parseFloat(jo_AV.getString("Distance"));

                                    }
                                    //Swiming
                                    if (!Functions.isNullOrEmpty(jo_AV.getString("ActivityName")) && jo_AV.getString("ActivityId").equals("4"))
                                    {
                                        SW += Float.parseFloat(jo_AV.getString("Distance"));

                                    }
                                }



                                Activity_DATA.put("Walking + Steps", WL);
                                Activity_DATA.put("Running", RN);
                                Activity_DATA.put("Cycling", CL);
                                Activity_DATA.put("Swimming", SW);


                                parse_response = "1";
                            }
                            else
                            {
                                parse_response = "-2";
                            }
                        }
                        else
                        {
                            parse_response = "-2";
                        }


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


    public String parseJsonBMIForDashboard() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0"))
                {
                    parse_response = "-1"; // No data available
                }
                else
                {

                    JSONArray jsonarrayBMI;
                    if (Functions.emrData)
                    {
                        jsonarrayBMI =  new JSONArray(jsonData.getString("response"));
                    }
                    else
                    {
                        jsonarrayBMI = new JSONArray();
                        JSONArray jsonTempBMI = new JSONArray(jsonData.getString("response"));


                        if (jsonTempBMI != null && jsonTempBMI.length() > 0)
                        {

                            for (int i = 0; i < jsonTempBMI.length(); i++)
                            {
                                JSONObject jo = jsonTempBMI.getJSONObject(i);
                                //Exclude Data from SourceId = id here // EMR Application Data
                                if (!jo.getString("SourceId").equals("2"))
                                {
                                    jsonarrayBMI.put(jsonTempBMI.get(i));
                                }
                            }
                        }
                    }


                    if (jsonarrayBMI != null && jsonarrayBMI.length() > 0)
                    {
                        BMI_Value = new String[jsonarrayBMI.length()];
                        BMI_Date = new String[jsonarrayBMI.length()];
                        for (int i = 0; i < jsonarrayBMI.length(); i++)
                        {
                            JSONObject jo = jsonarrayBMI.getJSONObject(i);
                            // Json data as Goal but used as Height

                            String finalbmicValue = "0";
                            if (!Functions.isNullOrEmpty(jo.getString("Result")) && !Functions.isNullOrEmpty(jo.getString("Goal"))) {
                                double bmic = Double.parseDouble(jo.getString("Result")) / Math.pow(Double.parseDouble(jo.getString("Goal")) / 100, 2.0);
                                finalbmicValue = String.valueOf(Math.round(bmic * 100.0) / 100.0);
                            }

                            BMI_Value[i] = finalbmicValue;
                            BMI_Date[i] = jo.getString("strCollectionDate");
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

}
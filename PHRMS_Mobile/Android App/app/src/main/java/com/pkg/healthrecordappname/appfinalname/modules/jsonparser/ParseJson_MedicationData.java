package com.pkg.healthrecordappname.appfinalname.modules.jsonparser;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.LinkedHashMap;


public class ParseJson_MedicationData {
    public static String[] Id;

    public static String[] sno;
    public static String[] MedicineType;
    public static String[] MedicineName;
    public static String[] TakingMedicine;
    public static String[] strTakingMedicine;
    public static String[] PrescribedDate;
    public static String[] strPrescribedDate;
    public static String[] DispensedDate;

    public static String[] strDispensedDate;
    public static String[] Provider;
    public static String[] Route;
    public static String[] strRoute;
    public static String[] Strength;
    public static String[] DosValue;
    public static String[] strDosValue;
    public static String[] DosUnit;
    public static String[] strDosUnit;
    public static String[] Frequency;
    public static String[] strFrequency;
    public static String[] LabelInstructions;
    public static String[] Notes;

    public static String[] DeleteFlag;
    public static String[] CreatedDate;
    public static String[] ModifiedDate;
    public static String[] strCreatedDate;
    public static String[] strModifiedDate;
    public static String[] lstFiles;
    public static String[] arrImages;
    public static String[] SourceId;
    public static String[] PrescriptionId;

    public static LinkedHashMap hmMedicationRoute = new LinkedHashMap<String, String>();
    public static LinkedHashMap hmMedicationDosageValue = new LinkedHashMap<String, String>();
    public static LinkedHashMap hmMedicationDosageUnit = new LinkedHashMap<String, String>();
    public static LinkedHashMap hmMedicationFrequency = new LinkedHashMap<String, String>();


    private JSONObject jsonData;

    public ParseJson_MedicationData(JSONObject jsonData) {
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
                        sno = new String[jsonarray.length()];
                        MedicineType = new String[jsonarray.length()];
                        MedicineName = new String[jsonarray.length()];
                        TakingMedicine = new String[jsonarray.length()];
                        strTakingMedicine = new String[jsonarray.length()];
                        PrescribedDate = new String[jsonarray.length()];
                        strPrescribedDate = new String[jsonarray.length()];
                        DispensedDate = new String[jsonarray.length()];
                        strDispensedDate = new String[jsonarray.length()];
                        Provider = new String[jsonarray.length()];
                        Route = new String[jsonarray.length()];
                        strRoute = new String[jsonarray.length()];
                        Strength = new String[jsonarray.length()];
                        DosValue = new String[jsonarray.length()];
                        strDosValue = new String[jsonarray.length()];
                        DosUnit = new String[jsonarray.length()];
                        strDosUnit = new String[jsonarray.length()];
                        Frequency = new String[jsonarray.length()];
                        strFrequency = new String[jsonarray.length()];
                        LabelInstructions = new String[jsonarray.length()];
                        Notes = new String[jsonarray.length()];
                        DeleteFlag = new String[jsonarray.length()];
                        CreatedDate = new String[jsonarray.length()];
                        ModifiedDate = new String[jsonarray.length()];
                        strCreatedDate = new String[jsonarray.length()];
                        strModifiedDate = new String[jsonarray.length()];
                        lstFiles = new String[jsonarray.length()];
                        arrImages = new String[jsonarray.length()];
                        SourceId = new String[jsonarray.length()];
                        PrescriptionId = new String[jsonarray.length()];

                        for (int i = 0; i < jsonarray.length(); i++) {
                            JSONObject jo = jsonarray.getJSONObject(i);
                            Id[i] = jo.getString("Id");
                            sno[i] = jo.getString("sno");
                            MedicineType[i] = jo.getString("MedicineType");
                            MedicineName[i] = jo.getString("MedicineName");
                            TakingMedicine[i] = jo.getString("TakingMedicine");
                            strTakingMedicine[i] = jo.getString("strTakingMedicine");
                            PrescribedDate[i] = jo.getString("PrescribedDate");
                            strPrescribedDate[i] = jo.getString("strPrescribedDate");
                            DispensedDate[i] = jo.getString("DispensedDate");
                            strDispensedDate[i] = jo.getString("strDispensedDate");
                            Provider[i] = jo.getString("Provider");
                            Route[i] = jo.getString("Route");
                            strRoute[i] = jo.getString("strRoute");
                            Strength[i] = jo.getString("Strength");
                            DosValue[i] = jo.getString("DosValue");
                            strDosValue[i] = jo.getString("strDosValue");
                            DosUnit[i] = jo.getString("DosUnit");
                            strDosUnit[i] = jo.getString("strDosUnit");
                            Frequency[i] = jo.getString("Frequency");
                            strFrequency[i] = jo.getString("strFrequency");
                            LabelInstructions[i] = jo.getString("LabelInstructions");
                            Notes[i] = jo.getString("Notes");
                            DeleteFlag[i] = jo.getString("DeleteFlag");
                            CreatedDate[i] = jo.getString("CreatedDate");
                            ModifiedDate[i] = jo.getString("ModifiedDate");
                            strCreatedDate[i] = jo.getString("strCreatedDate");
                            strModifiedDate[i] = jo.getString("strModifiedDate");
                            lstFiles[i] = jo.getString("lstFiles");
                            arrImages[i] = jo.getString("arrImages");
                            SourceId[i] = jo.getString("SourceId");
                            PrescriptionId[i] = jo.getString("PrescriptionId");
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



    public String parseJsonRoute() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                } else {
                    JSONArray jsonarray = new JSONArray(jsonData.getString("response"));
                    if (jsonarray.length() > 0)
                    {
                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);

                            hmMedicationRoute.put(jo.getString("Id"),jo.getString("Route"));
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

    public String parseJsonDosageValue() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                } else {
                    JSONArray jsonarray = new JSONArray(jsonData.getString("response"));
                    if (jsonarray.length() > 0)
                    {
                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);

                            hmMedicationDosageValue.put(jo.getString("Id"),jo.getString("DosValue"));
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

    public String parseJsonDosageUnit() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                } else {
                    JSONArray jsonarray = new JSONArray(jsonData.getString("response"));
                    if (jsonarray.length() > 0)
                    {
                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);

                            hmMedicationDosageUnit.put(jo.getString("Id"),jo.getString("DosUnit"));
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

    public String parseJsonFrequency() {
        String parse_response = "-1";


        try {
            if (jsonData != null) {
                if (jsonData.getString("status").equals("0")) {
                    parse_response = "-1"; // No data available
                } else {
                    JSONArray jsonarray = new JSONArray(jsonData.getString("response"));
                    if (jsonarray.length() > 0)
                    {
                        for (int i = 0; i < jsonarray.length(); i++)
                        {
                            JSONObject jo = jsonarray.getJSONObject(i);

                            hmMedicationFrequency.put(jo.getString("Id"),jo.getString("Frequency"));
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

    // Save - response with id for post with images
    public String parsePostResponseMedication() {
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
                    if (jsonData.getString("status").equals("1") && !Functions.isNullOrEmpty(jsonData.getString("response"))) {
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
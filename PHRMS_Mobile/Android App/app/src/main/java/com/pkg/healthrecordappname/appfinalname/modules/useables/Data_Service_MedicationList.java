package com.pkg.healthrecordappname.appfinalname.modules.useables;


import android.app.IntentService;
import android.content.Intent;
import android.os.Bundle;
import android.os.ResultReceiver;
import android.text.TextUtils;
import android.util.Log;

import com.pkg.healthrecordappname.appfinalname.modules.clinicaladapters.ClinicalTerms;
import com.pkg.healthrecordappname.appfinalname.modules.httpconnections.HttpUrlConnectionRequest;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;

public class Data_Service_MedicationList extends IntentService {

    public static final int STATUS_RUNNING = 0;
    public static final int STATUS_FINISHED = 1;
    public static final int STATUS_ERROR = 2;

    private static final String TAG = "Data Service Medication";


    public Data_Service_MedicationList() {
        super(Data_Service_MedicationList.class.getName());
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        Log.d(TAG, "Service Started!");

        final ResultReceiver receiver = intent.getParcelableExtra("receiver");
        String url = intent.getStringExtra("url");
        String searchTerm = intent.getStringExtra("searchTerm");

        Bundle bundle = new Bundle();

        if (!TextUtils.isEmpty(url)) {
            /* Update UI: Download Service is Running */
            receiver.send(STATUS_RUNNING, Bundle.EMPTY);

            try {

                ArrayList CT_Results = downloadCT(url, searchTerm);


                bundle.putStringArrayList("CT_RESULT", CT_Results);
                receiver.send(STATUS_FINISHED, bundle);

            } catch (Exception e) {
                /* Sending error message back to activity */
                bundle.putString(Intent.EXTRA_TEXT, e.toString());
                receiver.send(STATUS_ERROR, bundle);
            }
        }
        Log.d(TAG, "Service Stopping!");
        this.stopSelf();
    }


    private ArrayList downloadCT(String url, String params) {
        try {
            String jsonString = HttpUrlConnectionRequest.sendPost(url, params);
            Boolean emptyArray = (jsonString.equals("[]")) ? true : false;
            ArrayList ClinicalTermsList = new ArrayList<>();

            if (emptyArray == false) {
                JSONObject jobject = new JSONObject("{\"Result\"=" + jsonString + "}");

                if (jobject != null && jobject.length() > 0) {
                    JSONArray jsonArray = jobject.getJSONArray("Result");

                    // limit to 100 records per character
                    int count = 100;
                    if (jsonArray.length() <= 100) {
                        count = jsonArray.length();
                    }

                    for (int i = 0; i < count; i++) {
                        JSONObject jo = jsonArray.getJSONObject(i);
                        //store the CTerms name
                        ClinicalTerms CTerms = new ClinicalTerms();
                        CTerms.setName(jo.getString("MedicineName"));
                        CTerms.setId(jo.getString("Id"));

                        ClinicalTermsList.add(CTerms);
                    }
                }


            }

            return ClinicalTermsList;

        } catch (Exception e) {
            return null;
        }
    }


}
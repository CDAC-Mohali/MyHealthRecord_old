package com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues;

import android.app.DialogFragment;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONException;
import org.json.JSONObject;

public class PHRMS_MedicalContact_Dialogue extends DialogFragment {


    private TextView txtMedicalContactsName_value;
    private TextView txtMedicalContactsType_value;
    private TextView txtMedicalContactsClinicName_value;
    private TextView txtMedicalContactsAddress1_value;

    private TextView txtMedicalContactsAddress2_value;
    private TextView txtMedicalContactsCityVillage_value;

    private TextView txtMedicalContactsDistrict_value;
    private TextView txtMedicalContactsState_value;
    private TextView txtMedicalContactsPIN_value;

    private TextView txtMedicalContactsMobile_value;
    private TextView txtMedicalContactsEmailAddress_value;

    private ProgressBar dataMedicalContactsDialogue_progressbar;

    public PHRMS_MedicalContact_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.dialogue_medicalcontact, container, false);


        dataMedicalContactsDialogue_progressbar = (ProgressBar) rootView.findViewById(R.id.dataMedicalContactsDialogue_progressbar);

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid))
        {
            Functions.mainscreen(getActivity());
        }
        else
        {
            // Bundle to display and URL from fragment
            Bundle bundle = this.getArguments();
            int Display = bundle.getInt("Display");
            getDialog().setTitle("Medical Contacts Details");

            txtMedicalContactsName_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsName_value);
            txtMedicalContactsType_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsType_value);
            txtMedicalContactsClinicName_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsClinicName_value);
            txtMedicalContactsAddress1_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsAddress1_value);

            txtMedicalContactsAddress2_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsAddress2_value);
            txtMedicalContactsCityVillage_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsCityVillage_value);

            txtMedicalContactsDistrict_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsDistrict_value);
            txtMedicalContactsState_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsState_value);
            txtMedicalContactsPIN_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsPIN_value);

            txtMedicalContactsMobile_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsMobile_value);
            txtMedicalContactsEmailAddress_value = (TextView) rootView.findViewById(R.id.txtMedicalContactsEmailAddress_value);

            if (Display == 1)
            {
                if (Functions.isNetworkAvailable(getActivity()))
                {
                    if(!Functions.isNullOrEmpty(bundle.getString("URLDetails")) )
                    {
                        LoadMedicalContactsDetailsByID(bundle.getString("URLDetails"));
                    }
                    else
                    {
                        Functions.showSnackbar(getView(), "Url/Data Not Available !!", "Action");
                        getDialog().cancel();
                    }
                }
                else
                {
                    Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    getDialog().cancel();
                }
            }
            else
            {
                getDialog().cancel();
            }
        }

        return rootView;
    }

    public void LoadMedicalContactsDetailsByID(String url_for_details)
    {
        Functions.showProgress(true, dataMedicalContactsDialogue_progressbar);

        final JsonObjectRequest jsObjRequest_detailsbyID = new JsonObjectRequest(Request.Method.GET, url_for_details, null, new Response.Listener<JSONObject>()
        {
            @Override
            public void onResponse(JSONObject jsonData_Detailsby_ID)
            {
                try
                {
                    if (jsonData_Detailsby_ID != null)
                    {
                        if (jsonData_Detailsby_ID.getString("status").equals("0"))
                        {
                            Functions.showToast(getActivity(), "No Record Available");
                        }
                        else if (jsonData_Detailsby_ID.getString("status").equals("1") && !Functions.isNullOrEmpty(jsonData_Detailsby_ID.getString("response").toString()))
                        {
                            JSONObject jo = jsonData_Detailsby_ID.getJSONObject("response");
                            String Id = jo.getString("Id");
                            String sno = jo.getString("sno");
                            String ContactName = jo.getString("ContactName");
                            String MedContType = jo.getString("MedContType");
                            String strMedContType = jo.getString("strMedContType");
                            String EmailAddress = jo.getString("EmailAddress");
                            String strCreatedDate = jo.getString("strCreatedDate");
                            String PrimaryPhone = jo.getString("PrimaryPhone");
                            String DeleteFlag = jo.getString("DeleteFlag");
                            String CreatedDate = jo.getString("CreatedDate");
                            String ModifiedDate = jo.getString("ModifiedDate");
                            String Address1 = jo.getString("Address1");
                            String Address2 = jo.getString("Address2");
                            String CityVillage = jo.getString("CityVillage");
                            String PIN = jo.getString("PIN");
                            String District = jo.getString("District");
                            String strState = jo.getString("strState");
                            String State = jo.getString("State");
                            String ClinicName = jo.getString("ClinicName");
                            String SourceId = "4";

                            // Data as Bundle to display in Dialogue
                            Bundle bundleMCD = new Bundle();
                            bundleMCD.putInt("Display", 1);
                            bundleMCD.putString("Id", Id);
                            bundleMCD.putString("sno", sno);
                            bundleMCD.putString("ContactName", ContactName);
                            bundleMCD.putString("MedContType", MedContType);
                            bundleMCD.putString("strMedContType", strMedContType);
                            bundleMCD.putString("EmailAddress", EmailAddress);
                            bundleMCD.putString("strCreatedDate", strCreatedDate);
                            bundleMCD.putString("PrimaryPhone", PrimaryPhone);
                            bundleMCD.putString("DeleteFlag", DeleteFlag);
                            bundleMCD.putString("CreatedDate", CreatedDate);
                            bundleMCD.putString("ModifiedDate", ModifiedDate);
                            bundleMCD.putString("Address1", Address1);
                            bundleMCD.putString("Address2", Address2);
                            bundleMCD.putString("CityVillage", CityVillage);
                            bundleMCD.putString("PIN", PIN);
                            bundleMCD.putString("District", District);
                            bundleMCD.putString("strState", strState);
                            bundleMCD.putString("State", State);
                            bundleMCD.putString("ClinicName", ClinicName);
                            bundleMCD.putString("SourceId", SourceId);

                            // Display Dialogue with data
                            loadMedicalContactssData(bundleMCD);

                        }

                    }
                    else
                    {
                        Functions.showToast(getActivity(), "Service failed to fetch records");
                    }
                }
                catch (JSONException e)
                {
                    // TODO Auto-generated catch block
                    //errormsg = e.getMessage();
                    Functions.showToast(getActivity(), "unable to fetch records");
                }
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, dataMedicalContactsDialogue_progressbar);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("medicalcontacts Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest_detailsbyID);


    }

    private void loadMedicalContactssData(Bundle bundleMCDetails)
    {
        if (!bundleMCDetails.isEmpty())
        {
            Functions.showProgress(false, dataMedicalContactsDialogue_progressbar);

            txtMedicalContactsName_value.setText(bundleMCDetails.getString("ContactName"));
            txtMedicalContactsType_value.setText(bundleMCDetails.getString("strMedContType"));
            txtMedicalContactsClinicName_value.setText(bundleMCDetails.getString("ClinicName"));
            txtMedicalContactsAddress1_value.setText(bundleMCDetails.getString("Address1"));
            txtMedicalContactsAddress2_value.setText(bundleMCDetails.getString("Address2"));
            txtMedicalContactsCityVillage_value.setText(bundleMCDetails.getString("CityVillage"));
            txtMedicalContactsDistrict_value.setText(bundleMCDetails.getString("District"));
            txtMedicalContactsState_value.setText(bundleMCDetails.getString("strState"));
            txtMedicalContactsPIN_value.setText(bundleMCDetails.getString("PIN"));
            txtMedicalContactsMobile_value.setText("+91" + bundleMCDetails.getString("PrimaryPhone"));
            txtMedicalContactsEmailAddress_value.setText(bundleMCDetails.getString("EmailAddress"));
        }
    }

}
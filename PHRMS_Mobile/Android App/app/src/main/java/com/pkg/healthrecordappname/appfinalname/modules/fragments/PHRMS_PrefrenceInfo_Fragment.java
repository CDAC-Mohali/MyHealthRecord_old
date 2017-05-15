package com.pkg.healthrecordappname.appfinalname.modules.fragments;


import android.app.Fragment;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.v4.widget.SwipeRefreshLayout;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.Spinner;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.PHRMS_LoginActivity;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_PrefrenceInfoData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

/**
 * A placeholder fragment containing a simple view for frame_allergies.xml layout.
 */
public class PHRMS_PrefrenceInfo_Fragment extends Fragment {
    String url = null;
    private ProgressBar mProgressViewPref = null;



    private EditText edprefHosp_value;
    private EditText edPrefHospAddress_value;
    private EditText edprefSpclNeeds_value;

    private FloatingActionButton fab_edit;
    private FloatingActionButton fab_save;
    private FloatingActionButton fab_Cancel_prefrence;
    private String userid = "-1";
    private LinearLayout lv_prefrence;
    private RelativeLayout rl_fab_prefrence;
    private View rootViewPref;
    private SwipeRefreshLayout mSwipeRefreshLayout;


    public PHRMS_PrefrenceInfo_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        rootViewPref = inflater.inflate(R.layout.frame_prefinfo, container, false);

        mProgressViewPref = (ProgressBar) rootViewPref.findViewById(R.id.ProgressBarPreInfo);
        lv_prefrence = (LinearLayout) rootViewPref.findViewById(R.id.lv_prefrence);


        mSwipeRefreshLayout = (SwipeRefreshLayout) rootViewPref.findViewById(R.id.prefrence_swipe_refresh);

        edprefHosp_value = (EditText) rootViewPref.findViewById(R.id.edprefHosp_value);
        edPrefHospAddress_value = (EditText) rootViewPref.findViewById(R.id.edprefHospAddress_value);

        edprefSpclNeeds_value = (EditText) rootViewPref.findViewById(R.id.edprefSpclNeeds_value);


        rl_fab_prefrence = (RelativeLayout) rootViewPref.findViewById(R.id.rl_fab_prefrence);

        // Floating Action Button
        fab_edit = (FloatingActionButton) rootViewPref.findViewById(R.id.fab_Edit_prefrence);
        fab_save = (FloatingActionButton) rootViewPref.findViewById(R.id.fab_Save_prefrence);
        fab_Cancel_prefrence = (FloatingActionButton) rootViewPref.findViewById(R.id.fab_Cancel_prefrence);

        Functions.progressbarStyle(mProgressViewPref, getActivity());

        if (Functions.isNetworkAvailable(getActivity())) {
            userid = Functions.decrypt(rootViewPref.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {

                Functions.enableDisableView(lv_prefrence, false);


                // Binding using LinkedhashMapAdapter


                if (!userid.equals("-1")) {
                    url = getString(R.string.urlLogin) + getString(R.string.LoadPreferencesInfo) + userid;

                    if (url != null) {
                        // mSwipeRefreshLayout.setRefreshing(true);
                        Functions.showProgress(true, mProgressViewPref);
                        LoadprefrenceInfoData(url);
                    }
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {
                                LoadprefrenceInfoData(url);
                            } else {
                                Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                            }
                        }
                    }
                });



                fab_edit.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        Functions.enableDisableView(lv_prefrence, true);
                        fab_edit.setVisibility(View.GONE);
                        rl_fab_prefrence.setVisibility(View.VISIBLE);
                        Functions.showSnackbar(view, "Edit View - prefrence Info", "Action");
                    }
                });


                fab_save.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            // Post data with url
                            SendprefrenceInfoData(getString(R.string.urlLogin) + getString(R.string.UpdatePreferencesInfo));
                        } else {
                            Functions.showSnackbar(view, "Internet Not Available !!", "Action");
                        }
                    }
                });

                fab_Cancel_prefrence.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {

                        if (url != null)
                        {
                            if (Functions.isNetworkAvailable(getActivity()))
                            {
                                LoadprefrenceInfoData(url);
                            }
                        }

                        Functions.enableDisableView(lv_prefrence, false);
                        rl_fab_prefrence.setVisibility(View.GONE);
                        Functions.showSnackbar(view, "Update Request Canceled.", "Action");
                        fab_edit.setVisibility(View.VISIBLE);
                    }
                });
            }

        }
        else
        {
            Functions.enableDisableView(lv_prefrence, false);

            Functions.showSnackbar(rootViewPref, Functions.IE_NotAvailable, "Action");
        }

        return rootViewPref;
    }


    public void LoadprefrenceInfoData(String url) {
        //Functions.showProgress(true, mProgressViewPref);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressViewPref);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }


    private void LoadJSONData(JSONObject jsonData)
    {
        // Class to parse data and load in data arrays
        ParseJson_PrefrenceInfoData prefrenceInfo_pj = new ParseJson_PrefrenceInfoData(jsonData);
        String STATUS = prefrenceInfo_pj.parseJson();
        if (STATUS.equals("1"))
        {
            // By Deafult False;

            // Changes  - Preffered Hospital is changed to Hospital Name
            if (!Functions.isNullOrEmpty(ParseJson_PrefrenceInfoData.Pref_Hosp) && !ParseJson_PrefrenceInfoData.Pref_Hosp.equals("not available"))
            {
                //spPrefHospitalType_value.setSelection(getIndex(spPrefHospitalType_value, ParseJson_PrefrenceInfoData.Pref_Hosp));
                edprefHosp_value.setText(ParseJson_PrefrenceInfoData.Pref_Hosp);
            }

            // Changes - Primary Care is changed to Hospital Address
            if (!Functions.isNullOrEmpty(ParseJson_PrefrenceInfoData.Prim_Care_Prov) && !ParseJson_PrefrenceInfoData.Prim_Care_Prov.equals("not available"))
            {
                edPrefHospAddress_value.setText(ParseJson_PrefrenceInfoData.Prim_Care_Prov);
            }


            if (!Functions.isNullOrEmpty(ParseJson_PrefrenceInfoData.Special_Needs) && !ParseJson_PrefrenceInfoData.Special_Needs.equals("not available")) {
                edprefSpclNeeds_value.setText(ParseJson_PrefrenceInfoData.Special_Needs);
            }

            mSwipeRefreshLayout.setRefreshing(false);

        } else {
            mSwipeRefreshLayout.setRefreshing(false);
            Functions.enableDisableView(lv_prefrence, true);
        }

        Functions.showProgress(false, mProgressViewPref);
    }



    public void SendprefrenceInfoData(String url)
    {
        Functions.showProgress(true, mProgressViewPref);


        Map<String, String> jsonParams = new HashMap<String, String>();
        jsonParams.put("UserId", userid.toString());


        jsonParams.put("Pref_Hosp", edprefHosp_value.getText().toString());
        jsonParams.put("Prim_Care_Prov", edPrefHospAddress_value.getText().toString());
        jsonParams.put("Special_Needs", edprefSpclNeeds_value.getText().toString());



        JsonObjectRequest postRequestPrefrence = new JsonObjectRequest(Request.Method.POST, url,
                new JSONObject(jsonParams),
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        AfterPostPrefrence(response);
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        Functions.showProgress(false, mProgressViewPref);
                        Functions.ErrorHandling(getActivity(), error);
                    }
                }) {
            @Override
            public Map<String, String> getHeaders() {
                HashMap<String, String> headers = new HashMap<String, String>();
                headers.put("Content-Type", "application/json; charset=utf-8");
                headers.put("User-agent", System.getProperty("http.agent"));
                return headers;
            }
        };

        postRequestPrefrence.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(postRequestPrefrence);

    }

    private void AfterPostPrefrence(JSONObject response) {
        ParseJson_PrefrenceInfoData prefrenceInfo_pj = new ParseJson_PrefrenceInfoData(response);
        String STATUS_Post = prefrenceInfo_pj.parsePostResponsePrefrence();
        if (STATUS_Post.equals("1")) {
            //mSwipeRefreshLayout.setRefreshing(false);
            Functions.enableDisableView(lv_prefrence, false);
            rl_fab_prefrence.setVisibility(View.GONE);
            fab_edit.setVisibility(View.VISIBLE);
            Functions.showSnackbar(getView(), "Prefrence Info - Data Updated", "Action");
        } else if (STATUS_Post.equals("0")) {

            Functions.showSnackbar(getView(), "Prefrence Info - Nothing To Change", "Action");
        } else {

            Functions.showToast(getActivity(), STATUS_Post);
        }

        Functions.showProgress(false, mProgressViewPref);
    }

    private int getIndex(Spinner spinner, String myString) {
        int index = 0;
        for (int i = 0; i < spinner.getCount(); i++) {
            if (spinner.getItemAtPosition(i).equals(myString)) {
                index = i;
            }
        }
        return index;
    }


}
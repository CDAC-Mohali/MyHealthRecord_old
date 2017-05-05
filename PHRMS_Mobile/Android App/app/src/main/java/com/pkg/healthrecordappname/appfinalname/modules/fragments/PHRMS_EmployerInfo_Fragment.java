package com.pkg.healthrecordappname.appfinalname.modules.fragments;


import android.app.Fragment;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.TextInputLayout;
import android.support.v4.widget.SwipeRefreshLayout;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
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
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_EmployerInfoData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.LinkedHashMapAdapter;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;


public class PHRMS_EmployerInfo_Fragment extends Fragment {
    String url = null;
    private ProgressBar mProgressViewemp;
    private EditText edempfn_value;
    private EditText edempprimaryphne_value;
    private EditText edempdistrict_value;
    private EditText edempAdrline1_value;
    private EditText edempAdrline2_value;
    private EditText edempcity_value;
    private Spinner spempstate;
    private EditText edempdesg_value;
    private EditText edemppin_value;
    private EditText edempCUGcode_value;
    private FloatingActionButton fab_edit;
    private FloatingActionButton fab_save;
    private FloatingActionButton fab_Cancel_employer;
    private String userid = "-1";
    private LinkedHashMapAdapter<String, String> state_adapter;

    private LinearLayout lv_employer;
    private RelativeLayout rl_fab_employer;
    private View rootViewemp;
    private SwipeRefreshLayout mSwipeRefreshLayout;

    private TextInputLayout input_empprimaryphne_value;
    private TextInputLayout input_emppin_value;
    private TextInputLayout input_empCUGcode_value;


    public PHRMS_EmployerInfo_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        rootViewemp = inflater.inflate(R.layout.frame_employerinfo, container, false);

        mProgressViewemp = (ProgressBar) rootViewemp.findViewById(R.id.ProgressBarEMPRInfo);

        lv_employer = (LinearLayout) rootViewemp.findViewById(R.id.lv_employer);

        edempfn_value = (EditText) rootViewemp.findViewById(R.id.edempfn_value);

        edempprimaryphne_value = (EditText) rootViewemp.findViewById(R.id.edempprimaryphne_value);

        edempAdrline1_value = (EditText) rootViewemp.findViewById(R.id.edempAdrline1_value);
        edempAdrline2_value = (EditText) rootViewemp.findViewById(R.id.edempAdrline2_value);

        edempcity_value = (EditText) rootViewemp.findViewById(R.id.edempcity_value);
        edempdistrict_value = (EditText) rootViewemp.findViewById(R.id.edempdistrict_value);

        // Binding using LinkedhashMapAdapter
        spempstate = (Spinner) rootViewemp.findViewById(R.id.spempstate);

        edemppin_value = (EditText) rootViewemp.findViewById(R.id.edemppin_value);

        input_emppin_value = (TextInputLayout) rootViewemp.findViewById(R.id.input_emppin_value);

        edempdesg_value = (EditText) rootViewemp.findViewById(R.id.edempdesg_value);

        edempCUGcode_value = (EditText) rootViewemp.findViewById(R.id.edempCUGcode_value);
        input_empCUGcode_value = (TextInputLayout) rootViewemp.findViewById(R.id.input_empCUGcode_value);
        rl_fab_employer = (RelativeLayout) rootViewemp.findViewById(R.id.rl_fab_employer);

        mSwipeRefreshLayout = (SwipeRefreshLayout) rootViewemp.findViewById(R.id.employer_swipe_refresh);

        Functions.progressbarStyle(mProgressViewemp, getActivity());

        userid = Functions.decrypt(rootViewemp.getContext(), Functions.pref.getString(Functions.P_UsrID, null));


        if (Functions.isNetworkAvailable(getActivity())) {

            if (Functions.isNullOrEmpty(userid)) {
                Functions.mainscreen(getActivity());
            } else {


                Functions.enableDisableView(lv_employer, false);

                input_empprimaryphne_value = (TextInputLayout) rootViewemp.findViewById(R.id.input_empprimaryphne_value);
                edempprimaryphne_value.addTextChangedListener(new EditTextWatcher(edempprimaryphne_value));

                state_adapter = new LinkedHashMapAdapter<String, String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, Functions.StateData_LinkHasMap());
                state_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                spempstate.setAdapter(state_adapter);

                edemppin_value.addTextChangedListener(new EditTextWatcher(edemppin_value));

                edempCUGcode_value.addTextChangedListener(new EditTextWatcher(edempCUGcode_value));

                if (!userid.equals("-1")) {
                    url = getString(R.string.urlLogin) + getString(R.string.LoadEmployerInfo) + userid;

                    if (url != null) {
                        //mSwipeRefreshLayout.setRefreshing(true);
                        Functions.showProgress(true, mProgressViewemp);
                        LoademployerInfoData(url);
                    }
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {
                                LoademployerInfoData(url);
                            } else {
                                Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                            }
                        }
                    }
                });

                // Floating Action Button
                fab_edit = (FloatingActionButton) rootViewemp.findViewById(R.id.fab_Edit_employer);
                fab_save = (FloatingActionButton) rootViewemp.findViewById(R.id.fab_Save_employer);
                fab_Cancel_employer = (FloatingActionButton) rootViewemp.findViewById(R.id.fab_Cancel_employer);


                fab_edit.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        Functions.enableDisableView(lv_employer, true);
                        fab_edit.setVisibility(View.GONE);
                        rl_fab_employer.setVisibility(View.VISIBLE);

                        Functions.showSnackbar(view, "Edit View - employer Info", "Action");
                    }
                });


                fab_save.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            // Post data with url
                            SendemployerInfoData(getString(R.string.urlLogin) + getString(R.string.UpdateEmployerInfo));
                        } else {
                            Functions.showSnackbar(view, "Internet Not Available !!", "Action");
                        }

                    }
                });

                fab_Cancel_employer.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {

                        if (url != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {
                                LoademployerInfoData(url);
                            }
                        }

                        Functions.enableDisableView(lv_employer, false);
                        rl_fab_employer.setVisibility(View.GONE);


                        Functions.showSnackbar(view, "Update Request Canceled.", "Action");

                        fab_edit.setVisibility(View.VISIBLE);

                    }
                });
            }
        } else {
            Functions.enableDisableView(lv_employer, false);

            Functions.showSnackbar(rootViewemp, Functions.IE_NotAvailable, "Action");
        }

        return rootViewemp;
    }


    public void LoademployerInfoData(String url) {


        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressViewemp);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }


    private void LoadJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_EmployerInfoData employerInfo_pj = new ParseJson_EmployerInfoData(jsonData);
        String STATUS = employerInfo_pj.parseJson();
        if (STATUS.equals("1")) {

            if (!Functions.isNullOrEmpty(ParseJson_EmployerInfoData.EmployerName) && !ParseJson_EmployerInfoData.EmployerName.equals("not available")) {
                edempfn_value.setText(ParseJson_EmployerInfoData.EmployerName);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmployerInfoData.EmpAddressLine1) && !ParseJson_EmployerInfoData.EmpAddressLine1.equals("not available")) {
                edempAdrline1_value.setText(ParseJson_EmployerInfoData.EmpAddressLine1);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmployerInfoData.EmpAddressLine2) && !ParseJson_EmployerInfoData.EmpAddressLine2.equals("not available")) {
                edempAdrline2_value.setText(ParseJson_EmployerInfoData.EmpAddressLine2);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmployerInfoData.EmpCity_Vill_Town) && !ParseJson_EmployerInfoData.EmpCity_Vill_Town.equals("not available")) {
                edempcity_value.setText(ParseJson_EmployerInfoData.EmpCity_Vill_Town);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmployerInfoData.EmpDistrict) && !ParseJson_EmployerInfoData.EmpDistrict.equals("not available")) {
                edempdistrict_value.setText(ParseJson_EmployerInfoData.EmpDistrict);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmployerInfoData.EmpState) && !ParseJson_EmployerInfoData.EmpState.equals("not available")) {

                List<String> indexes = new ArrayList<String>(Functions.StateData_LinkHasMap().keySet());
                spempstate.setSelection(indexes.indexOf(ParseJson_EmployerInfoData.EmpState));
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmployerInfoData.EmpPin) && !ParseJson_EmployerInfoData.EmpPin.equals("not available")) {
                edemppin_value.setText(ParseJson_EmployerInfoData.EmpPin);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmployerInfoData.EmployerPhone) && !ParseJson_EmployerInfoData.EmployerPhone.equals("not available")) {
                edempprimaryphne_value.setText(ParseJson_EmployerInfoData.EmployerPhone);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmployerInfoData.EmployerOccupation) && !ParseJson_EmployerInfoData.EmployerOccupation.equals("not available")) {
                edempdesg_value.setText(ParseJson_EmployerInfoData.EmployerOccupation);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmployerInfoData.CUG) && !ParseJson_EmployerInfoData.CUG.equals("not available")) {
                edempCUGcode_value.setText(ParseJson_EmployerInfoData.CUG);
            }

            mSwipeRefreshLayout.setRefreshing(false);
        } else {
            Functions.enableDisableView(lv_employer, true);
            mSwipeRefreshLayout.setRefreshing(false);
        }

        Functions.showProgress(false, mProgressViewemp);
    }


    public void SendemployerInfoData(String url) {

        if (validatePhoneNumber() == true && validatePin() == true && validateCUGCODE() == true) {
            Functions.showProgress(true, mProgressViewemp);

            Map<String, String> jsonParams = new HashMap<String, String>();

            jsonParams.put("UserId", userid.toString());
            jsonParams.put("EmployerName", edempfn_value.getText().toString());
            jsonParams.put("EmpAddressLine1", edempAdrline1_value.getText().toString());
            jsonParams.put("EmpAddressLine2", edempAdrline2_value.getText().toString());
            jsonParams.put("EmpCity_Vill_Town", edempcity_value.getText().toString());
            jsonParams.put("EmpDistrict", edempdistrict_value.getText().toString());
            Map.Entry<String, String> spempstate_item = (Map.Entry<String, String>) spempstate.getSelectedItem();
            jsonParams.put("EmpState", spempstate_item.getKey().toString());//spempstate
            jsonParams.put("EmpPin", edemppin_value.getText().toString());
            jsonParams.put("EmployerPhone", edempprimaryphne_value.getText().toString());
            jsonParams.put("EmployerOccupation", edempdesg_value.getText().toString());
            jsonParams.put("CUG", edempCUGcode_value.getText().toString());


            JsonObjectRequest postRequestEmployer = new JsonObjectRequest(Request.Method.POST, url,
                    new JSONObject(jsonParams),
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            AfterPostEmployer(response);
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            Functions.showProgress(false, mProgressViewemp);
                            mSwipeRefreshLayout.setRefreshing(false);
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

            postRequestEmployer.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

            // Access the RequestQueue through your singleton class.
            MySingleton.getInstance(getActivity()).addToRequestQueue(postRequestEmployer);


        } else {

            return;
        }
    }


    private void AfterPostEmployer(JSONObject response) {
        ParseJson_EmployerInfoData empInfo_pj = new ParseJson_EmployerInfoData(response);
        String STATUS_Post = empInfo_pj.parsePostResponseEmployer();
        if (STATUS_Post.equals("1")) {

            Functions.enableDisableView(lv_employer, false);
            rl_fab_employer.setVisibility(View.GONE);
            fab_edit.setVisibility(View.VISIBLE);

            Functions.showSnackbar(getView(), "Employer Info - Data Updated", "Action"); // Snackbar.LENGTH_SHORT).setAction(, null).show();
        } else if (STATUS_Post.equals("0")) {

            Functions.showSnackbar(getView(), "Employer Info - Nothing To Change", "Action");
        } else {

            Functions.showToast(getActivity(), STATUS_Post);
        }

        Functions.showProgress(false, mProgressViewemp);
    }

    // Not Required Case
    protected boolean validatePhoneNumber() {
        boolean bool_Phone = true;
        if (!Functions.isNullOrEmpty(edempprimaryphne_value.getText().toString().trim())) {

            try {
                double d = Double.valueOf(edempprimaryphne_value.getText().toString().trim());
                if (d == (long) d) {
                    if (edempprimaryphne_value.getText().toString().trim().length() == 10) {

                        input_empprimaryphne_value.setError(null);
                        input_empprimaryphne_value.setErrorEnabled(false);
                        //return true;
                    } else {
                        input_empprimaryphne_value.setErrorEnabled(true);
                        input_empprimaryphne_value.setError(getString(R.string.errphonelenght));
                        requestFocus(edempprimaryphne_value);
                        bool_Phone = false;
                    }
                } else {

                    input_empprimaryphne_value.setErrorEnabled(true);
                    input_empprimaryphne_value.setError(getString(R.string.errphoneint));
                    requestFocus(edempprimaryphne_value);
                    bool_Phone = false;
                }
            } catch (Exception e) {

                input_empprimaryphne_value.setErrorEnabled(true);
                input_empprimaryphne_value.setError(getString(R.string.errphoneint));
                requestFocus(edempprimaryphne_value);
                bool_Phone = false;
            }
        } else {
            input_empprimaryphne_value.setError(null);
            input_empprimaryphne_value.setErrorEnabled(false);
            // return true;
        }

        return bool_Phone;
    }

    // Not Required Case
    protected boolean validatePin() {
        boolean bool_pin = true;
        if (!Functions.isNullOrEmpty(edemppin_value.getText().toString().trim())) {
            try {
                double d = Double.valueOf(edemppin_value.getText().toString().trim());
                if (d == (long) d) {
                    if (edemppin_value.getText().toString().trim().length() == 6) {

                        input_emppin_value.setError(null);
                        input_emppin_value.setErrorEnabled(false);

                    } else {
                        input_emppin_value.setErrorEnabled(true);
                        input_emppin_value.setError(getString(R.string.errpinlenght));
                        requestFocus(edemppin_value);
                        bool_pin = false;
                    }
                } else {

                    input_emppin_value.setErrorEnabled(true);
                    input_emppin_value.setError(getString(R.string.errpinint));
                    requestFocus(edemppin_value);
                    bool_pin = false;
                }
            } catch (Exception e) {

                input_emppin_value.setErrorEnabled(true);
                input_emppin_value.setError(getString(R.string.errpinint));
                requestFocus(edemppin_value);
                bool_pin = false;
            }
        } else {
            input_emppin_value.setError(null);
            input_emppin_value.setErrorEnabled(false);

        }

        return bool_pin;
    }

    // Not Required Case
    protected boolean validateCUGCODE() {
        boolean bool_cugcode = true;
        if (!Functions.isNullOrEmpty(edempCUGcode_value.getText().toString().trim())) {
            if (edempCUGcode_value.getText().toString().trim().length() == 6) {

                input_empCUGcode_value.setError(null);
                input_empCUGcode_value.setErrorEnabled(false);
                //return true;
            } else {
                input_empCUGcode_value.setErrorEnabled(true);
                input_empCUGcode_value.setError(getString(R.string.errempCUGCode));
                requestFocus(edempCUGcode_value);
                bool_cugcode = false;
            }
        } else {
            input_empCUGcode_value.setError(null);
            input_empCUGcode_value.setErrorEnabled(false);
            // return true;
        }

        return bool_cugcode;
    }

    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getActivity().getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    private class EditTextWatcher implements TextWatcher {

        private View view;

        private EditTextWatcher(View view) {
            this.view = view;
        }

        public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {
        }

        public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
        }

        public void afterTextChanged(Editable editable) {
            switch (view.getId()) {
                case R.id.edempprimaryphne_value:
                    // cHECK FOR VALID PHONE
                    validatePhoneNumber();
                    break;
                case R.id.edemppin_value:
                    // cHECK FOR VALID PIN
                    validatePin();
                    break;
                case R.id.edempCUGcode_value:
                    // cHECK FOR VALID PHONE
                    validateCUGCODE();
                    break;
            }
        }
    }

}

package com.pkg.healthrecordappname.appfinalname.modules.fragments;


import android.app.DialogFragment;
import android.app.Fragment;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.TextInputLayout;
import android.support.v4.widget.SwipeRefreshLayout;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DatePickerFragment_NoPastDate;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DateValidator;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_InsuranceInfoData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;


public class PHRMS_InsuranceInfo_Fragment extends Fragment {
    String url = null;
    private ProgressBar mProgressViewinsr;
    private EditText edinsrProvider_value;
    private EditText edinsrpolicynumber_value;
    private EditText edpolicyname_value;
    private EditText edinsrValidTill_value;
    private FloatingActionButton fab_edit;
    private FloatingActionButton fab_save;
    private FloatingActionButton fab_Cancel_insurance;
    private String userid = "-1";
    private LinearLayout lv_insurance;
    private RelativeLayout rl_fab_insurance;
    private View rootViewinsr;
    private SwipeRefreshLayout mSwipeRefreshLayout;
    TextInputLayout input_insrProvider_value;
    TextInputLayout input_insrpolicynumber_value;
    TextInputLayout input_policyname_value;
    TextInputLayout input_insrValidTill_value;


    public PHRMS_InsuranceInfo_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        rootViewinsr = inflater.inflate(R.layout.frame_insuranceinfo, container, false);

        mProgressViewinsr = (ProgressBar) rootViewinsr.findViewById(R.id.ProgressBarINSRInfo);
        lv_insurance = (LinearLayout) rootViewinsr.findViewById(R.id.lv_insurance);
        edinsrProvider_value = (EditText) rootViewinsr.findViewById(R.id.edinsrProvider_value);
        edinsrpolicynumber_value = (EditText) rootViewinsr.findViewById(R.id.edinsrpolicynumber_value);
        edpolicyname_value = (EditText) rootViewinsr.findViewById(R.id.edpolicyname_value);
        edinsrValidTill_value = (EditText) rootViewinsr.findViewById(R.id.edinsrValidTill_value);

        input_insrProvider_value = (TextInputLayout) rootViewinsr.findViewById(R.id.input_insrProvider_value);
        input_insrpolicynumber_value = (TextInputLayout) rootViewinsr.findViewById(R.id.input_insrpolicynumber_value);
        input_policyname_value = (TextInputLayout) rootViewinsr.findViewById(R.id.input_policyname_value);
        input_insrValidTill_value = (TextInputLayout) rootViewinsr.findViewById(R.id.input_insrValidTill_value);


        rl_fab_insurance = (RelativeLayout) rootViewinsr.findViewById(R.id.rl_fab_insurance);
        // Floating Action Button
        fab_edit = (FloatingActionButton) rootViewinsr.findViewById(R.id.fab_Edit_insurance);
        fab_save = (FloatingActionButton) rootViewinsr.findViewById(R.id.fab_Save_insurance);
        fab_Cancel_insurance = (FloatingActionButton) rootViewinsr.findViewById(R.id.fab_Cancel_insurance);


        mSwipeRefreshLayout = (SwipeRefreshLayout) rootViewinsr.findViewById(R.id.insurance_swipe_refresh);

        Functions.progressbarStyle(mProgressViewinsr, getActivity());

        userid = Functions.decrypt(rootViewinsr.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNetworkAvailable(getActivity())) {

            if (Functions.isNullOrEmpty(userid)) {
                Functions.mainscreen(getActivity());
            } else {

                Functions.enableDisableView(lv_insurance, false);


                edinsrProvider_value.addTextChangedListener(new EditTextWatcher(edinsrProvider_value));

                // Binding using LinkedhashMapAdapter

                edinsrpolicynumber_value.addTextChangedListener(new EditTextWatcher(edinsrpolicynumber_value));


                edpolicyname_value.addTextChangedListener(new EditTextWatcher(edpolicyname_value));


                edinsrValidTill_value.addTextChangedListener(new EditTextWatcher(edinsrValidTill_value));


                edinsrValidTill_value.setOnTouchListener(new View.OnTouchListener() {
                    public boolean onTouch(View v, MotionEvent event) {
                        DialogFragment datepicker = DatePickerFragment_NoPastDate.newInstance(edinsrValidTill_value);
                        if (datepicker != null) {
                            datepicker.show(getFragmentManager(), "DatePickerFragment");
                        }
                        return false;
                    }
                });


                if (!userid.equals("-1")) {
                    url = getString(R.string.urlLogin) + getString(R.string.LoadInsuranceInfo) + userid;

                    if (url != null) {

                        Functions.showProgress(true, mProgressViewinsr);

                        LoadinsuranceInfoData(url);
                    }
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {
                                LoadinsuranceInfoData(url);
                                //mSwipeRefreshLayout.setRefreshing(false);
                            } else {
                                Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                            }
                        }

                    }
                });



                fab_edit.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        Functions.enableDisableView(lv_insurance, true);
                        fab_edit.setVisibility(View.GONE);
                        rl_fab_insurance.setVisibility(View.VISIBLE);
                        Functions.showSnackbar(view, "Edit View - insurance Info", "Action");
                    }
                });


                fab_save.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            SendinsuranceInfoData(getString(R.string.urlLogin) + getString(R.string.UpdateInsuranceInfo));
                        } else {
                            Functions.showSnackbar(view, "Internet Not Available !!", "Action");
                        }
                    }
                });

                fab_Cancel_insurance.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {

                        if (url != null)
                        {
                            if (Functions.isNetworkAvailable(getActivity()))
                            {
                                LoadinsuranceInfoData(url);

                            }
                        }
                        Functions.enableDisableView(lv_insurance, false);
                        rl_fab_insurance.setVisibility(View.GONE);
                        Functions.showSnackbar(view, "Update Request Canceled.", "Action");
                        fab_edit.setVisibility(View.VISIBLE);

                    }
                });
            }
        }
        else
        {
            Functions.enableDisableView(lv_insurance, false);
            Functions.showSnackbar(rootViewinsr, Functions.IE_NotAvailable, "Action");
        }
        return rootViewinsr;
    }


    public void LoadinsuranceInfoData(String url) {


        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {

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

        ParseJson_InsuranceInfoData insuranceInfo_pj = new ParseJson_InsuranceInfoData(jsonData);
        String STATUS = insuranceInfo_pj.parseJson();
        if (STATUS.equals("1")) {

            if (!Functions.isNullOrEmpty(ParseJson_InsuranceInfoData.Insu_Org_Name) && !ParseJson_InsuranceInfoData.Insu_Org_Name.equals("not available")) {
                edinsrProvider_value.setText(ParseJson_InsuranceInfoData.Insu_Org_Name);
            }


            if (!Functions.isNullOrEmpty(ParseJson_InsuranceInfoData.strValidTill) && !ParseJson_InsuranceInfoData.strValidTill.equals("not available")) {
                edinsrValidTill_value.setText(ParseJson_InsuranceInfoData.strValidTill);
            }


            if (!Functions.isNullOrEmpty(ParseJson_InsuranceInfoData.Insu_Org_Grp_Num) && !ParseJson_InsuranceInfoData.Insu_Org_Grp_Num.equals("not available")) {
                edpolicyname_value.setText(ParseJson_InsuranceInfoData.Insu_Org_Grp_Num);
            }



            if (!Functions.isNullOrEmpty(ParseJson_InsuranceInfoData.Insu_Org_Phone) && !ParseJson_InsuranceInfoData.Insu_Org_Phone.equals("not available")) {
                edinsrpolicynumber_value.setText(ParseJson_InsuranceInfoData.Insu_Org_Phone);
            }
            mSwipeRefreshLayout.setRefreshing(false);

        } else {
            Functions.enableDisableView(lv_insurance, true);
            mSwipeRefreshLayout.setRefreshing(false);
        }

        Functions.showProgress(false, mProgressViewinsr);
    }


    public void SendinsuranceInfoData(String url) {
        if (validateProviderName() == true && validatePolicyNumber() == true && validatePolicyName() == true && validateValidTill() == true) {
            // if valid date conversion done then
            if (!Functions.DateToDateHH(edinsrValidTill_value.getText().toString()).equals("-1")) {
                Functions.showProgress(true, mProgressViewinsr);

                Map<String, String> jsonParams = new HashMap<String, String>();


                jsonParams.put("Id", ParseJson_InsuranceInfoData.Id.toString());
                jsonParams.put("UserId", userid.toString());
                jsonParams.put("Insu_Org_Name", edinsrProvider_value.getText().toString());
                jsonParams.put("Insu_Org_Phone", edinsrpolicynumber_value.getText().toString());
                jsonParams.put("Insu_Org_Grp_Num", edpolicyname_value.getText().toString());


                Functions.DateToDateHH(edinsrValidTill_value.getText().toString());

                jsonParams.put("ValidTill", Functions.DateToDateHH(edinsrValidTill_value.getText().toString()));

                JsonObjectRequest postRequestInsr = new JsonObjectRequest(Request.Method.POST, url,
                        new JSONObject(jsonParams),
                        new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                AfterPostINSR(response);
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Functions.showProgress(false, mProgressViewinsr);
                                //mSwipeRefreshLayout.setRefreshing(false);
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

                postRequestInsr.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

                // Access the RequestQueue through your singleton class.
                MySingleton.getInstance(getActivity()).addToRequestQueue(postRequestInsr);
            } else {
                Functions.showSnackbar(getView(), "Invalid Date", "Action");
            }

        }

    }

    private void AfterPostINSR(JSONObject response) {
        ParseJson_InsuranceInfoData insuranceInfo_pj = new ParseJson_InsuranceInfoData(response);
        String STATUS_Post = insuranceInfo_pj.parsePostResponseInsurance();
        if (STATUS_Post.equals("1")) {

            Functions.enableDisableView(lv_insurance, false);
            rl_fab_insurance.setVisibility(View.GONE);
            fab_edit.setVisibility(View.VISIBLE);
            Functions.showSnackbar(getView(), "Insurance Info - Data Updated", "Action");
        } else if (STATUS_Post.equals("0")) {

            Functions.showSnackbar(getView(), "Insurance Info - Nothing To Change", "Action");
        } else {

            Functions.showToast(getActivity(), STATUS_Post);
        }

        Functions.showProgress(false, mProgressViewinsr);
    }


    protected boolean validateProviderName() {
        Boolean bool_pname = true;
        if (edinsrProvider_value.getText().toString().trim().isEmpty()) {
            input_insrProvider_value.setErrorEnabled(true);
            input_insrProvider_value.setError(getString(R.string.errinsrProvider));
            requestFocus(edinsrProvider_value);
            bool_pname = false;
        } else {
            input_insrProvider_value.setError(null);
            input_insrProvider_value.setErrorEnabled(false);
        }

        return bool_pname;
    }

    protected boolean validatePolicyNumber() {

        boolean bool_pnumber = true;

        if (edinsrpolicynumber_value.getText().toString().trim().isEmpty()) {
            input_insrpolicynumber_value.setErrorEnabled(true);
            input_insrpolicynumber_value.setError(getString(R.string.errinsrpolicyNumber));
            requestFocus(edinsrpolicynumber_value);
            bool_pnumber = false;
        } else {
            try {
                double d = Double.valueOf(edinsrpolicynumber_value.getText().toString().trim());
                if (d == (long) d) {
                    if (edinsrpolicynumber_value.getText().toString().trim().length() == 11) {

                        input_insrpolicynumber_value.setError(null);
                        input_insrpolicynumber_value.setErrorEnabled(false);
                    } else {
                        input_insrpolicynumber_value.setErrorEnabled(true);
                        input_insrpolicynumber_value.setError(getString(R.string.errinsrpolicyNumberlength));
                        requestFocus(edinsrpolicynumber_value);
                        bool_pnumber = false;
                    }


                } else {

                    input_insrpolicynumber_value.setErrorEnabled(true);
                    input_insrpolicynumber_value.setError(getString(R.string.errinsrpolicyNumberIntegerOnly));
                    requestFocus(edinsrpolicynumber_value);
                    bool_pnumber = false;
                }
            } catch (Exception e) {

                input_insrpolicynumber_value.setErrorEnabled(true);
                input_insrpolicynumber_value.setError(getString(R.string.errinsrpolicyNumberIntegerOnly));
                requestFocus(edinsrpolicynumber_value);
                bool_pnumber = false;
            }
        }

        return bool_pnumber;
    }

    protected boolean validatePolicyName() {

        boolean bool_policyname = true;
        if (edpolicyname_value.getText().toString().trim().isEmpty()) {
            input_policyname_value.setErrorEnabled(true);
            input_policyname_value.setError(getString(R.string.errinsrPolicyName));
            requestFocus(edpolicyname_value);
            bool_policyname = false;
        } else {
            input_policyname_value.setError(null);
            input_policyname_value.setErrorEnabled(false);
        }

        return bool_policyname;
    }

    protected boolean validateValidTill() {
        boolean bool_validtTille = true;

        if (Functions.isNullOrEmpty(edinsrValidTill_value.getText().toString())) {
            input_insrValidTill_value.setErrorEnabled(true);
            input_insrValidTill_value.setError(getString(R.string.errinsrValidTillreq));
            requestFocus(edinsrValidTill_value);
            bool_validtTille = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edinsrValidTill_value.getText().toString(), "dd/MM/yyyy")) {
                input_insrValidTill_value.setError(null);
                input_insrValidTill_value.setErrorEnabled(false);

            } else {
                input_insrValidTill_value.setErrorEnabled(true);
                input_insrValidTill_value.setError(getString(R.string.errinsrValidTill));
                requestFocus(edinsrValidTill_value);
                bool_validtTille = false;
            }
        }

        return bool_validtTille;

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
                case R.id.edinsrProvider_value:
                    // Required check for empty also
                    validateProviderName();
                    break;
                case R.id.edinsrpolicynumber_value:
                    // Required check for empty also
                    validatePolicyNumber();
                    break;
                case R.id.edpolicyname_value:
                    // Required check for empty also
                    validatePolicyName();
                    break;
                case R.id.edinsrValidTill_value:
                    // Required check for empty also
                    validateValidTill();
                    break;
            }
        }
    }
}
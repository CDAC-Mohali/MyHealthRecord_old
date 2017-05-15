package com.pkg.healthrecordappname.appfinalname.modules.fragments;


import android.app.Fragment;
import android.content.Intent;
import android.database.Cursor;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.provider.ContactsContract;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.TextInputLayout;
import android.support.v4.content.ContextCompat;
import android.support.v4.widget.SwipeRefreshLayout;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.EditText;
import android.widget.ImageButton;
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
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_EmergencyInfoData;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_ProfileInfoData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.ContactHelper;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.LinkedHashMapAdapter;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static android.app.Activity.RESULT_OK;


public class PHRMS_EmergencyInfo_Fragment extends Fragment {
    String url = null;
    private ProgressBar mProgressViewEMR;
    private EditText edemrfn_value;
    private TextInputLayout input_emrfn_value;
    private Spinner spemrrel_value;
    private EditText edemrdistrict_value;
    private EditText edemrAdrline1_value;
    private EditText edemrAdrline2_value;
    private EditText edemrcity_value;
    private Spinner spemrstate;
    private EditText edsecondaryphne_value;
    private EditText edemrpin_value;
    private EditText edprimaryphne_value;
    private FloatingActionButton fab_edit;
    private FloatingActionButton fab_save;
    private FloatingActionButton fab_Cancel_emergency;
    private String userid = "-1";
    private LinkedHashMapAdapter<String, String> state_adapter;
    private LinkedHashMapAdapter<String, String> relation_adapter;
    private LinearLayout lv_emergency;
    private RelativeLayout rl_fab_emergency;
    private View rootViewEMR;
    private SwipeRefreshLayout mSwipeRefreshLayout;
    TextInputLayout inputTXTPin;
    TextInputLayout input_primaryphne_value;
    TextInputLayout input_secondaryphne_value;

    //Phone book

    private ImageButton imgButtonprimaryphnebook;

    private ImageButton imgButtonsecondaryphnebook;

    private static final int PERMISSION_REQUEST_CODE_CONTACT = 636;


    public PHRMS_EmergencyInfo_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        rootViewEMR = inflater.inflate(R.layout.frame_emergencyinfo, container, false);

        mProgressViewEMR = (ProgressBar) rootViewEMR.findViewById(R.id.ProgressBarEMRInfo);

        lv_emergency = (LinearLayout) rootViewEMR.findViewById(R.id.lv_emergency);
        edemrfn_value = (EditText) rootViewEMR.findViewById(R.id.edemrfn_value);


        // Binding using LinkedhashMapAdapter
        spemrrel_value = (Spinner) rootViewEMR.findViewById(R.id.spemrrel_value);

        edemrdistrict_value = (EditText) rootViewEMR.findViewById(R.id.edemrdistrict_value);
        edemrAdrline1_value = (EditText) rootViewEMR.findViewById(R.id.edemrAdrline1_value);
        edemrAdrline2_value = (EditText) rootViewEMR.findViewById(R.id.edemrAdrline2_value);
        edemrcity_value = (EditText) rootViewEMR.findViewById(R.id.edemrcity_value);
        edprimaryphne_value = (EditText) rootViewEMR.findViewById(R.id.edprimaryphne_value);
        edsecondaryphne_value = (EditText) rootViewEMR.findViewById(R.id.edsecondaryphne_value);
        edemrpin_value = (EditText) rootViewEMR.findViewById(R.id.edemrpin_value);

        rl_fab_emergency = (RelativeLayout) rootViewEMR.findViewById(R.id.rl_fab_emergency);

        inputTXTPin = (TextInputLayout) rootViewEMR.findViewById(R.id.input_emrpin_value);
        mSwipeRefreshLayout = (SwipeRefreshLayout) rootViewEMR.findViewById(R.id.emergency_swipe_refresh);

        edemrfn_value.addTextChangedListener(new EditTextWatcher(edemrfn_value));
        input_emrfn_value = (TextInputLayout) rootViewEMR.findViewById(R.id.input_emrfn_value);

        spemrstate = (Spinner) rootViewEMR.findViewById(R.id.spemrstate);

        input_primaryphne_value = (TextInputLayout) rootViewEMR.findViewById(R.id.input_primaryphne_value);
        input_secondaryphne_value = (TextInputLayout) rootViewEMR.findViewById(R.id.input_secondaryphne_value);


        // Floating Action Button
        fab_edit = (FloatingActionButton) rootViewEMR.findViewById(R.id.fab_Edit_emergency);
        fab_save = (FloatingActionButton) rootViewEMR.findViewById(R.id.fab_Save_emergency);
        fab_Cancel_emergency = (FloatingActionButton) rootViewEMR.findViewById(R.id.fab_Cancel_emergency);


        imgButtonprimaryphnebook = (ImageButton) rootViewEMR.findViewById(R.id.imgButtonprimaryphnebook);


        imgButtonsecondaryphnebook = (ImageButton) rootViewEMR.findViewById(R.id.imgButtonsecondaryphnebook);

        Functions.progressbarStyle(mProgressViewEMR, getActivity());
        userid = Functions.decrypt(rootViewEMR.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Functions.mainscreen(getActivity());
            } else {
                Functions.enableDisableView(lv_emergency, false);


                relation_adapter = new LinkedHashMapAdapter<String, String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, Functions.RelationShip_LinkHasMap());
                relation_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                spemrrel_value.setAdapter(relation_adapter);

                // Binding using LinkedhashMapAdapter

                state_adapter = new LinkedHashMapAdapter<String, String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, Functions.StateData_LinkHasMap());
                state_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                spemrstate.setAdapter(state_adapter);

                edprimaryphne_value.addTextChangedListener(new EditTextWatcher(edprimaryphne_value));

                edsecondaryphne_value.addTextChangedListener(new EditTextWatcher(edsecondaryphne_value));

                edemrpin_value.addTextChangedListener(new EditTextWatcher(edemrpin_value));


                if (!userid.equals("-1")) {
                    url = getString(R.string.urlLogin) + getString(R.string.LoadEmergencyInfo) + userid;

                    if (url != null) {

                        Functions.showProgress(true, mProgressViewEMR);
                        LoademergencyInfoData(url);
                    }
                }




                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {
                                LoademergencyInfoData(url);
                            } else {
                                Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                            }

                        }

                    }
                });



                fab_edit.setOnClickListener(
                        new View.OnClickListener() {
                            @Override
                            public void onClick(View view) {
                                Functions.enableDisableView(lv_emergency, true);
                                fab_edit.setVisibility(View.GONE);
                                rl_fab_emergency.setVisibility(View.VISIBLE);

                                imgButtonprimaryphnebook.setColorFilter(ContextCompat.getColor(getActivity(), R.color.colorPrimary));
                                imgButtonsecondaryphnebook.setColorFilter(ContextCompat.getColor(getActivity(), R.color.colorPrimary));

                                Functions.showSnackbar(view, "Edit View - Emergency Info", "Action");


                            }
                        });


                fab_save.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            // Post data with url
                            SendemergencyInfoData(getString(R.string.urlLogin) + getString(R.string.UpdateEmergencyInfo));
                        } else {
                            Functions.showSnackbar(view, "Internet Not Available !!", "Action");
                        }

                    }
                });

                fab_Cancel_emergency.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (url != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {
                                LoademergencyInfoData(url);
                            }
                        }

                        Functions.enableDisableView(lv_emergency, false);
                        rl_fab_emergency.setVisibility(View.GONE);
                        imgButtonprimaryphnebook.setColorFilter(ContextCompat.getColor(getActivity(), R.color.grey));
                        imgButtonsecondaryphnebook.setColorFilter(ContextCompat.getColor(getActivity(), R.color.grey));
                        Functions.showSnackbar(view, "Update Request Canceled.", "Action");
                        fab_edit.setVisibility(View.VISIBLE);
                    }
                });

                imgButtonprimaryphnebook.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Build.VERSION.SDK_INT >= 23) {
                            if ((Functions.hasContactsReadPermission(getActivity()) == true)) {
                                Log.e("testing", "Contacts Permission is granted");
                                readcontact(1); // 1 for primary contact
                            } else {
                                Functions.checkcontactspermissions(getActivity(), getActivity(), PERMISSION_REQUEST_CODE_CONTACT);
                            }
                        } else {
                            //permission is automatically granted on sdk<23 upon installation
                            Log.e("testing", "Contacts Permission is already granted");
                            readcontact(1); // 1 for primary contact
                        }
                    }
                });

                imgButtonsecondaryphnebook.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Build.VERSION.SDK_INT >= 23) {
                            if ((Functions.hasContactsReadPermission(getActivity()) == true)) {
                                Log.e("testing", "Contacts Permission is granted");
                                readcontact(2); // 2 for secondary contact
                            } else {

                                Functions.checkcontactspermissions(getActivity(), getActivity(), PERMISSION_REQUEST_CODE_CONTACT);
                            }
                        } else {
                            //permission is automatically granted on sdk<23 upon installation
                            Log.e("testing", "Contacts Permission is already granted");
                            readcontact(2); // 2 for secondary contact
                        }


                    }
                });


            }

        } else {
            Functions.enableDisableView(lv_emergency, false);
            Functions.showSnackbar(rootViewEMR, Functions.IE_NotAvailable, "Action");
        }

        return rootViewEMR;
    }


    public void LoademergencyInfoData(String url) {


        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressViewEMR);
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
        ParseJson_EmergencyInfoData emergencyInfo_pj = new ParseJson_EmergencyInfoData(jsonData);
        String STATUS = emergencyInfo_pj.parseJson();
        if (STATUS.equals("1")) {
            // By Deafult False;

            if (!Functions.isNullOrEmpty(ParseJson_EmergencyInfoData.Primary_Emergency_Contact) && !ParseJson_EmergencyInfoData.Primary_Emergency_Contact.equals("not available")) {
                edemrfn_value.setText(ParseJson_EmergencyInfoData.Primary_Emergency_Contact);
            }




            if (!Functions.isNullOrEmpty(ParseJson_EmergencyInfoData.PC_Relationship) && !ParseJson_EmergencyInfoData.PC_Relationship.equals("not available")) {
                List<String> indexes = new ArrayList<String>(Functions.RelationShip_LinkHasMap().keySet());
                spemrrel_value.setSelection(indexes.indexOf(ParseJson_EmergencyInfoData.PC_Relationship));
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmergencyInfoData.PC_AddressLine1) && !ParseJson_EmergencyInfoData.PC_AddressLine1.equals("not available")) {
                edemrAdrline1_value.setText(ParseJson_EmergencyInfoData.PC_AddressLine1);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmergencyInfoData.PC_AddressLine2) && !ParseJson_EmergencyInfoData.PC_AddressLine2.equals("not available")) {
                edemrAdrline2_value.setText(ParseJson_EmergencyInfoData.PC_AddressLine2);
            }



            if (!Functions.isNullOrEmpty(ParseJson_EmergencyInfoData.PC_City_Vill_Town) && !ParseJson_EmergencyInfoData.PC_City_Vill_Town.equals("not available")) {
                edemrcity_value.setText(ParseJson_EmergencyInfoData.PC_City_Vill_Town);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmergencyInfoData.PC_District) && !ParseJson_EmergencyInfoData.PC_District.equals("not available")) {
                edemrdistrict_value.setText(ParseJson_EmergencyInfoData.PC_District);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmergencyInfoData.PC_State) && !ParseJson_EmergencyInfoData.PC_State.equals("not available")) {

                List<String> indexes = new ArrayList<String>(Functions.StateData_LinkHasMap().keySet());
                spemrstate.setSelection(indexes.indexOf(ParseJson_EmergencyInfoData.PC_State));
            }

            if (!Functions.isNullOrEmpty(ParseJson_EmergencyInfoData.PC_Pin) && !ParseJson_EmergencyInfoData.PC_Pin.equals("not available")) {
                edemrpin_value.setText(ParseJson_EmergencyInfoData.PC_Pin);
            }


            if (!Functions.isNullOrEmpty(ParseJson_EmergencyInfoData.PC_Phone1) && !ParseJson_EmergencyInfoData.PC_Phone1.equals("not available")) {
                edprimaryphne_value.setText(ParseJson_EmergencyInfoData.PC_Phone1);
            }



            if (!Functions.isNullOrEmpty(ParseJson_EmergencyInfoData.PC_Phone2) && !ParseJson_EmergencyInfoData.PC_Phone2.equals("not available")) {
                edsecondaryphne_value.setText(ParseJson_EmergencyInfoData.PC_Phone2);
            }

            mSwipeRefreshLayout.setRefreshing(false);
        } else {
            Functions.enableDisableView(lv_emergency, true);
            mSwipeRefreshLayout.setRefreshing(false);
        }

        Functions.showProgress(false, mProgressViewEMR);
    }



    public void SendemergencyInfoData(String url) {
        if (validateFirstName() == true && validatePin() == true && validatePhoneNumberPrimary() == true && validatePhoneNumberSecondary() == true) {
            // No required field chec data on watcher only
            Functions.showProgress(true, mProgressViewEMR);

            Map<String, String> jsonParams = new HashMap<String, String>();
            jsonParams.put("UserId", userid.toString());
            jsonParams.put("Primary_Emergency_Contact", edemrfn_value.getText().toString());

            Map.Entry<String, String> spemrrel_item = (Map.Entry<String, String>) spemrrel_value.getSelectedItem();
            jsonParams.put("PC_Relationship", spemrrel_item.getKey().toString()); //spemrrel_value

            jsonParams.put("PC_AddressLine1", edemrAdrline1_value.getText().toString());
            jsonParams.put("PC_AddressLine2", edemrAdrline2_value.getText().toString());
            jsonParams.put("PC_City_Vill_Town", edemrcity_value.getText().toString());
            jsonParams.put("PC_District", edemrdistrict_value.getText().toString());

            Map.Entry<String, String> spemrstate_item = (Map.Entry<String, String>) spemrstate.getSelectedItem();
            jsonParams.put("PC_State", spemrstate_item.getKey().toString());//spemrstate
            jsonParams.put("PC_Pin", edemrpin_value.getText().toString().trim());
            jsonParams.put("PC_Phone1", edprimaryphne_value.getText().toString().trim());
            jsonParams.put("PC_Phone2", edsecondaryphne_value.getText().toString().trim());


            JsonObjectRequest postRequestEmergencyInfo = new JsonObjectRequest(Request.Method.POST, url,
                    new JSONObject(jsonParams),
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            AfterPostEmergency(response);
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            Functions.showProgress(false, mProgressViewEMR);
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
            postRequestEmergencyInfo.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
            // Access the RequestQueue through your singleton class.
            MySingleton.getInstance(getActivity()).addToRequestQueue(postRequestEmergencyInfo);
        } else {

            return;
        }
    }

    // Required
    protected boolean validateFirstName() {
        boolean bool_firstname = true;
        if (edemrfn_value.getText().toString().trim().isEmpty()) {
            input_emrfn_value.setErrorEnabled(true);
            input_emrfn_value.setError(getString(R.string.erremrName));
            requestFocus(edemrfn_value);
            bool_firstname = false;
        } else {
            input_emrfn_value.setError(null);
            input_emrfn_value.setErrorEnabled(false);
        }

        return bool_firstname;
    }

    // Not required
    protected boolean validatePin() {
        Boolean bool_pin = true;

        if (!Functions.isNullOrEmpty(edemrpin_value.getText().toString().trim())) {
            try {
                double d = Double.valueOf(edemrpin_value.getText().toString().trim());
                if (d == (long) d) {
                    if (edemrpin_value.getText().toString().trim().length() == 6) {
                        //System.out.println("integer"+(int)d);
                        inputTXTPin.setError(null);
                        inputTXTPin.setErrorEnabled(false);
                        bool_pin = true;

                    } else {
                        inputTXTPin.setErrorEnabled(true);
                        inputTXTPin.setError(getString(R.string.errpinlenght));
                        requestFocus(edemrpin_value);
                        bool_pin = false;
                    }
                } else {
                    //System.out.println("double"+d);
                    inputTXTPin.setErrorEnabled(true);
                    inputTXTPin.setError(getString(R.string.errpinint));
                    requestFocus(edemrpin_value);
                    bool_pin = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                inputTXTPin.setErrorEnabled(true);
                inputTXTPin.setError(getString(R.string.errpinint));
                requestFocus(edemrpin_value);
                bool_pin = false;
            }
        } else {
            inputTXTPin.setError(null);
            inputTXTPin.setErrorEnabled(false);
        }
        return bool_pin;
    }

    //// Required - phone integer check for primary phones
    protected boolean validatePhoneNumberPrimary() {

        Boolean bool_phprimary = false;

        if (edprimaryphne_value.getText().toString().trim().isEmpty()) {
            input_primaryphne_value.setErrorEnabled(true);
            input_primaryphne_value.setError(getString(R.string.errphone));
            requestFocus(edprimaryphne_value);
            //bool_phprimary = false;
        } else {

            try {
                double d = Double.valueOf(edprimaryphne_value.getText().toString().trim());
                if (d == (long) d) {
                    if (edprimaryphne_value.getText().toString().trim().length() == 10) {

                        input_primaryphne_value.setError(null);
                        input_primaryphne_value.setErrorEnabled(false);
                        bool_phprimary = true;
                    } else {
                        input_primaryphne_value.setErrorEnabled(true);
                        input_primaryphne_value.setError(getString(R.string.errphonelenght));
                        requestFocus(edprimaryphne_value);
                        //bool_phprimary = false;
                    }
                } else {
                    input_primaryphne_value.setErrorEnabled(true);
                    input_primaryphne_value.setError(getString(R.string.errphoneint));
                    requestFocus(edprimaryphne_value);
                    //bool_phprimary = false;
                }
            } catch (Exception e) {
                input_primaryphne_value.setErrorEnabled(true);
                input_primaryphne_value.setError(getString(R.string.errphoneint));
                requestFocus(edprimaryphne_value);
                //bool_phprimary = false;
            }
        }


        return bool_phprimary;

    }

    //// Not required Only phone integer check for primary phones
    protected boolean validatePhoneNumberSecondary() {
        Boolean bool_ph2 = true;


        if (!Functions.isNullOrEmpty(edsecondaryphne_value.getText().toString().trim())) {
            try {
                double d = Double.valueOf(edsecondaryphne_value.getText().toString().trim());
                if (d == (long) d) {
                    if (edsecondaryphne_value.getText().toString().trim().length() == 10) {
                        //System.out.println("integer"+(int)d);
                        input_secondaryphne_value.setError(null);
                        input_secondaryphne_value.setErrorEnabled(false);
                        bool_ph2 = true;
                    } else {
                        input_secondaryphne_value.setErrorEnabled(true);
                        input_secondaryphne_value.setError(getString(R.string.errphonelenght));
                        requestFocus(edsecondaryphne_value);
                        bool_ph2 = false;
                    }
                } else {
                    //System.out.println("double"+d);
                    input_secondaryphne_value.setErrorEnabled(true);
                    input_secondaryphne_value.setError(getString(R.string.errphoneint));
                    requestFocus(edsecondaryphne_value);
                    bool_ph2 = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_secondaryphne_value.setErrorEnabled(true);
                input_secondaryphne_value.setError(getString(R.string.errphoneint));
                requestFocus(edsecondaryphne_value);
                bool_ph2 = false;
            }
        } else {
            input_secondaryphne_value.setError(null);
            input_secondaryphne_value.setErrorEnabled(false);
        }

        return bool_ph2;

    }

    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getActivity().getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }


    private void AfterPostEmergency(JSONObject response) {
        ParseJson_ProfileInfoData profileInfo_pj = new ParseJson_ProfileInfoData(response);
        String STATUS_Post = profileInfo_pj.parsePostResponseProfile();
        if (STATUS_Post.equals("1")) {

            Functions.enableDisableView(lv_emergency, false);
            rl_fab_emergency.setVisibility(View.GONE);
            fab_edit.setVisibility(View.VISIBLE);

            imgButtonprimaryphnebook.setColorFilter(ContextCompat.getColor(getActivity(), R.color.grey));
            imgButtonsecondaryphnebook.setColorFilter(ContextCompat.getColor(getActivity(), R.color.grey));

            Functions.showSnackbar(getView(), "Emergency Info - Data Updated", "Action");

        } else if (STATUS_Post.equals("0")) {


            Functions.showSnackbar(getView(), "Emergency Info - Nothing To Change", "Action");

        } else {

            Functions.showToast(getActivity(), STATUS_Post);
        }

        Functions.showProgress(false, mProgressViewEMR);
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
                case R.id.edemrfn_value:
                    validateFirstName();
                    break;
                case R.id.edemrpin_value:
                    // Not Required check for empty and integer only
                    validatePin();
                    break;
                case R.id.edprimaryphne_value:
                    // Required check for empty and integer
                    validatePhoneNumberPrimary();
                    break;
                case R.id.edsecondaryphne_value:
                    // Not Required check for integer only
                    validatePhoneNumberSecondary();
                    break;
            }
        }
    }


    // ReadContacts
    public void readcontact(Integer PICK) {
        Intent intent = new Intent(Intent.ACTION_PICK, ContactsContract.Contacts.CONTENT_URI);
        startActivityForResult(intent, PICK);
    }


    @Override
    public void onActivityResult(int reqCode, int resultCode, Intent data) {
        //String phn_no_activity_result = "";

        if (resultCode == RESULT_OK) {
            switch (reqCode) {
                case 1:

                    Cursor cursorprimary = null;

                    try {
                        Uri result = data.getData();
                        // get the contact id from the Uri
                        String id = result.getLastPathSegment();

                        cursorprimary = ContactHelper.getSelectedContactDataCursor(getActivity().getContentResolver(), id);

                        if (cursorprimary != null) {
                            int nameId = cursorprimary.getColumnIndex(ContactsContract.Contacts.DISPLAY_NAME);
                            int PhoneIdx = cursorprimary.getColumnIndex(ContactsContract.CommonDataKinds.Phone.NUMBER);
                            if (cursorprimary.moveToFirst()) {
                                String namePrimary = cursorprimary.getString(nameId);
                                String numberPrimary = cursorprimary.getString(PhoneIdx);
                                Functions.showToast(getActivity(), "Selected Primary Contact :: " + namePrimary + "\n Phone Number :: " + numberPrimary);

                                if (!Functions.isNullOrEmpty(namePrimary)) {
                                    edemrfn_value.setText(namePrimary);
                                }

                                if (!Functions.isNullOrEmpty(numberPrimary)) {
                                    edprimaryphne_value.setText(ContactHelper.getNumber(numberPrimary, getActivity()));
                                }
                            } else {
                                edprimaryphne_value.setText("");
                                edemrfn_value.setText("");
                                Functions.showToast(getActivity(), "No Mobile number found");
                            }

                            if (cursorprimary != null) {
                                cursorprimary.close();
                            }
                        } else {
                            edprimaryphne_value.setText("");
                            edemrfn_value.setText("");
                            Functions.showToast(getActivity(), "Contact data not avialable.");
                        }
                    } catch (Exception e) {
                        edprimaryphne_value.setText("");
                        edemrfn_value.setText("");
                        Functions.showToast(getActivity(), "Unable to read primary contact data.");
                    }

                    break;
                case 2:

                    Cursor cursorsecondary = null;

                    try {
                        Uri resultsecondary = data.getData();
                        // get the contact id from the Uri
                        String idsecondary = resultsecondary.getLastPathSegment();
                        cursorsecondary = ContactHelper.getSelectedContactDataCursor(getActivity().getContentResolver(), idsecondary);

                        if (cursorsecondary != null) {
                            int nameId = cursorsecondary.getColumnIndex(ContactsContract.Contacts.DISPLAY_NAME);
                            int PhoneIdx = cursorsecondary.getColumnIndex(ContactsContract.CommonDataKinds.Phone.NUMBER);
                            if (cursorsecondary.moveToFirst()) {
                                String nameSecondary = cursorsecondary.getString(nameId);
                                String numberSecondary = cursorsecondary.getString(PhoneIdx);
                                Functions.showToast(getActivity(), "Selected Primary Contact :: " + nameSecondary + "\n Phone Number :: " + numberSecondary);

                                if (!Functions.isNullOrEmpty(numberSecondary)) {
                                    edsecondaryphne_value.setText(ContactHelper.getNumber(numberSecondary, getActivity()));
                                }
                            } else {
                                edsecondaryphne_value.setText("");
                                Functions.showToast(getActivity(), "No Mobile number found");
                            }

                            if (cursorsecondary != null) {
                                cursorsecondary.close();
                            }
                        } else {
                            edsecondaryphne_value.setText("");
                            Functions.showToast(getActivity(), "Contact data not avialable.");
                        }
                    } catch (Exception e) {
                        edsecondaryphne_value.setText("");
                        Functions.showToast(getActivity(), "Unable to read secondary contact data.");
                    }

                    break;
            }
        } else {
            Functions.showToast(getActivity(), "No Contact Selected");
        }
    }
}
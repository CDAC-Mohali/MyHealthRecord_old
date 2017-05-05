
package com.pkg.healthrecordappname.appfinalname.modules.addfragments;

import android.content.Intent;
import android.database.Cursor;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.provider.ContactsContract;
import android.support.design.widget.TextInputLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.WindowManager;
import android.widget.AdapterView;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ProgressBar;
import android.widget.ScrollView;
import android.widget.Spinner;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_MedicalContact_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_MedicalContactData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.ContactHelper;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.LinkedHashMapAdapter;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;


public class PHRMS_MedicalContact_Fragment_Add extends AppCompatActivity {

    private ProgressBar mProgressBarAddMedicalContacts;
    private EditText edMedicalContactsName_Value;

    private EditText edMedicalContactsClinicName_Value;
    private EditText edMedicalContactsAddress1_Value;
    private EditText edMedicalContactsDistrict_Value;
    private Spinner sp_MedicalContactsType_value;


    private Spinner sp_MedicalContactsState_value;

    private EditText edMedicalContactsPIN_value;
    private EditText edMedicalContactsMobile_value;
    private EditText edMedicalContactsEmailAddress_value;

    private EditText edMedicalContactsAddress2_Value;
    private EditText edMedicalContactsCityVillage_Value;

    private ScrollView svMedicalContactsAdd;
    private Boolean boolMenu = false;
    private View parentLayout;

    private Boolean MedicalContactsTypeHasValue = false;
    private Boolean validateState = false;

    TextInputLayout input_MedicalContactsEmailAddress_value;
    TextInputLayout input_MedicalContactsMobile_value;
    TextInputLayout input_MedicalContactsPIN_value;
    TextInputLayout input_MedicalContactsName_value;
    TextInputLayout input_MedicalContactsClinicName_value;

    private LinkedHashMapAdapter<String, String> speciality_adapter;
    private LinkedHashMapAdapter<String, String> state_adapter;

    //Phone book
    private ImageButton imgButtonMedicalContactsphnebook;
    private static final int PERMISSION_REQUEST_CODE_CONTACT = 637;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_medicalcontact_add);
        parentLayout = findViewById(R.id.svMedicalContactsAdd);

        //toolbar
        Toolbar mtoolbar_add_MedicalContacts = (Toolbar) findViewById(R.id.toolbar_addMedicalContacts);

        if (mtoolbar_add_MedicalContacts != null) {
            setSupportActionBar(mtoolbar_add_MedicalContacts);
        }
        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarAddMedicalContacts = (ProgressBar) findViewById(R.id.ProgressBarAddMedicalContacts);

        edMedicalContactsName_Value = (EditText) findViewById(R.id.edMedicalContactsName_Value);
        input_MedicalContactsName_value = (TextInputLayout) findViewById(R.id.input_MedicalContactsName_value);
        edMedicalContactsName_Value.addTextChangedListener(new EditTextWatcher(edMedicalContactsName_Value));

        sp_MedicalContactsType_value = (Spinner) findViewById(R.id.sp_MedicalContactsType_value);
        sp_MedicalContactsType_value.setEnabled(false);

        edMedicalContactsClinicName_Value = (EditText) findViewById(R.id.edMedicalContactsClinicName_Value);
        input_MedicalContactsClinicName_value = (TextInputLayout) findViewById(R.id.input_MedicalContactsClinicName_value);
        edMedicalContactsClinicName_Value.addTextChangedListener(new EditTextWatcher(edMedicalContactsClinicName_Value));

        edMedicalContactsAddress1_Value = (EditText) findViewById(R.id.edMedicalContactsAddress1_Value);

        edMedicalContactsAddress2_Value = (EditText) findViewById(R.id.edMedicalContactsAddress2_Value);

        edMedicalContactsCityVillage_Value = (EditText) findViewById(R.id.edMedicalContactsCityVillage_Value);

        edMedicalContactsDistrict_Value = (EditText) findViewById(R.id.edMedicalContactsDistrict_Value);


        imgButtonMedicalContactsphnebook = (ImageButton) findViewById(R.id.imgButtonMedicalContactsphnebook);

        sp_MedicalContactsState_value = (Spinner) findViewById(R.id.sp_MedicalContactsState_value);
        // Binding using LinkedhashMapAdapter
        state_adapter = new LinkedHashMapAdapter<String, String>(PHRMS_MedicalContact_Fragment_Add.this, android.R.layout.simple_spinner_dropdown_item, Functions.StateData_LinkHasMap());
        state_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        sp_MedicalContactsState_value.setAdapter(state_adapter);

        sp_MedicalContactsState_value.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                // TODO Auto-generated method stub
                // State Required.
                Map.Entry<String, String> spStateValue_item = (Map.Entry<String, String>) sp_MedicalContactsState_value.getSelectedItem();

                if (!Functions.isNullOrEmpty(spStateValue_item.getKey()) && !spStateValue_item.getKey().toString().equals("0")) {
                    validateState = true;
                } else {
                    validateState = false;
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                // TODO Auto-generated method stub
                validateState = false;
            }
        });


        sp_MedicalContactsType_value.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                // TODO Auto-generated method stub
                // Contact Type Required.
                Map.Entry<String, String> spmedicalcontactType_item = (Map.Entry<String, String>) sp_MedicalContactsType_value.getSelectedItem();

                if (!Functions.isNullOrEmpty(spmedicalcontactType_item.getKey()) && !spmedicalcontactType_item.getKey().toString().equals("0")) {
                    MedicalContactsTypeHasValue = true;
                } else {
                    MedicalContactsTypeHasValue = false;
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                // TODO Auto-generated method stub
                MedicalContactsTypeHasValue = false;
            }
        });

        edMedicalContactsPIN_value = (EditText) findViewById(R.id.edMedicalContactsPIN_value);
        input_MedicalContactsPIN_value = (TextInputLayout) findViewById(R.id.input_MedicalContactsPIN_value);
        edMedicalContactsPIN_value.addTextChangedListener(new EditTextWatcher(edMedicalContactsPIN_value));

        edMedicalContactsMobile_value = (EditText) findViewById(R.id.edMedicalContactsMobile_value);
        input_MedicalContactsMobile_value = (TextInputLayout) findViewById(R.id.input_MedicalContactsMobile_value);
        edMedicalContactsMobile_value.addTextChangedListener(new EditTextWatcher(edMedicalContactsMobile_value));

        edMedicalContactsEmailAddress_value = (EditText) findViewById(R.id.edMedicalContactsEmailAddress_value);
        input_MedicalContactsEmailAddress_value = (TextInputLayout) findViewById(R.id.input_MedicalContactsEmailAddress_value);
        edMedicalContactsEmailAddress_value.addTextChangedListener(new EditTextWatcher(edMedicalContactsEmailAddress_value));


        svMedicalContactsAdd = (ScrollView) findViewById(R.id.svMedicalContactsAdd);
        svMedicalContactsAdd.setVisibility(View.INVISIBLE);

        Functions.progressbarStyle(mProgressBarAddMedicalContacts, PHRMS_MedicalContact_Fragment_Add.this);

        if (Functions.isNetworkAvailable(PHRMS_MedicalContact_Fragment_Add.this)) {
            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                Functions.mainscreen(PHRMS_MedicalContact_Fragment_Add.this);
            } else {
                loadContactTypeSpinner();
            }
        } else {
            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
        }

        imgButtonMedicalContactsphnebook.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (Build.VERSION.SDK_INT >= 23) {
                    if ((Functions.hasContactsReadPermission(getApplicationContext()) == true)) {
                        Log.e("testing", "Contacts Permission is granted");
                        readcontact(1); // 1 for primary contact
                    } else {
                        Functions.checkcontactspermissions(getApplicationContext(), PHRMS_MedicalContact_Fragment_Add.this, PERMISSION_REQUEST_CODE_CONTACT);
                    }
                } else {

                    Log.e("testing", "Contacts Permission is already granted");
                    readcontact(1); // 1 for primary contact
                }
            }
        });


    }

    public void loadContactTypeSpinner() {
        Functions.showProgress(true, mProgressBarAddMedicalContacts);

        String url_MedicalContacts_ContactTypeList = getString(R.string.urlLogin) + getString(R.string.GetMedicalContactSpecialityList);

        final JsonObjectRequest jsObjRequestMedicalContactsTypeList = new JsonObjectRequest(Request.Method.GET, url_MedicalContacts_ContactTypeList, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONDataMedicalContactsTypeSpinner(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressBarAddMedicalContacts);
                Functions.ErrorHandling(PHRMS_MedicalContact_Fragment_Add.this, error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(PHRMS_MedicalContact_Fragment_Add.this).addToRequestQueue(jsObjRequestMedicalContactsTypeList);
    }

    public void LoadJSONDataMedicalContactsTypeSpinner(JSONObject MedicalContactsType) {
        ParseJson_MedicalContactData route_pj = new ParseJson_MedicalContactData(MedicalContactsType);
        String STATUS = route_pj.parseJsonMedicalContactType();

        if (STATUS.equals("1")) {
            // Binding using LinkedhashMapAdapter from parsed data
            speciality_adapter = new LinkedHashMapAdapter<String, String>(PHRMS_MedicalContact_Fragment_Add.this, android.R.layout.simple_spinner_dropdown_item, ParseJson_MedicalContactData.hmMedicalContactType);
            speciality_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            sp_MedicalContactsType_value.setAdapter(speciality_adapter);

            if (ParseJson_MedicalContactData.hmMedicalContactType.size() > 0) {
                sp_MedicalContactsType_value.setEnabled(true);
            }

            if (svMedicalContactsAdd.getVisibility() == View.INVISIBLE) {
                svMedicalContactsAdd.setVisibility(View.VISIBLE);
            }
        } else {
            Functions.showToast(PHRMS_MedicalContact_Fragment_Add.this, "Unable to load District Data");
        }

        Functions.showProgress(false, mProgressBarAddMedicalContacts);
    }


    public void onActivityResult(int requestCode, int resultCode, Intent data) {


        if (resultCode == RESULT_OK) {
            switch (requestCode) {
                case 1:
                    try
                    {
                        Uri result = data.getData();
                        // get the contact id from the Uri
                        String contactid = result.getLastPathSegment();

                        // To read Name and Number
                        loadMedicalContactNameAndNumber(contactid);
                        // To Read Name and Email
                        loadMedicalContactNameAndEmail(contactid);

                    }
                    catch (Exception e)
                    {
                        edMedicalContactsName_Value.setText("");
                        edMedicalContactsMobile_value.setText("");
                        edMedicalContactsEmailAddress_value.setText("");
                        Functions.showToast(getApplicationContext(), "Unable to read contact data.");
                    }

                    break;
            }
        } else {
            Functions.showToast(getApplicationContext(), "No Contact Selected");
        }

        super.onActivityResult(requestCode, resultCode, data);
    }

    public void loadMedicalContactNameAndNumber(String contactid)
    {
        Cursor cursorprimary = null;
        cursorprimary = ContactHelper.getSelectedContactDataCursor(getApplicationContext().getContentResolver(), contactid);
        //to get name and phone number of selected contact
        if (cursorprimary != null)
        {
            // name - phone number - email id
            int nameId = cursorprimary.getColumnIndex(ContactsContract.Contacts.DISPLAY_NAME);
            int PhoneIdx = cursorprimary.getColumnIndex(ContactsContract.CommonDataKinds.Phone.NUMBER);

            if (cursorprimary.moveToFirst()) {
                String namePrimary = cursorprimary.getString(nameId);
                String numberPrimary = cursorprimary.getString(PhoneIdx);

                Functions.showToast(getApplicationContext(), "Selected Primary Contact :: " + namePrimary + "\n Phone Number :: " + numberPrimary);

                if (!Functions.isNullOrEmpty(namePrimary)) {
                    edMedicalContactsName_Value.setText(namePrimary);
                }

                if (!Functions.isNullOrEmpty(numberPrimary)) {
                    edMedicalContactsMobile_value.setText(ContactHelper.getNumber(numberPrimary, PHRMS_MedicalContact_Fragment_Add.this));
                }
            }
            else
            {
                edMedicalContactsName_Value.setText("");
                edMedicalContactsMobile_value.setText("");
                Functions.showToast(getApplicationContext(), "No Mobile number found");
            }
        }
        else
        {
            edMedicalContactsName_Value.setText("");
            edMedicalContactsMobile_value.setText("");
            Functions.showToast(getApplicationContext(), "Contact data not avialable.");
        }

        if (cursorprimary != null)
        {
            cursorprimary.close();
        }
    }

    public void loadMedicalContactNameAndEmail(String contactid)
    {
        Cursor cursoremail = ContactHelper.getEmailDetails(getApplicationContext().getContentResolver(), contactid);

        if (cursoremail != null)
        {
            String nameMC = null;
            String emailMC = null;


            if (cursoremail.moveToFirst())
            {
                //to get the contact names
                nameMC = cursoremail.getString(cursoremail.getColumnIndex(ContactsContract.CommonDataKinds.Phone.DISPLAY_NAME));
                Log.e("Name :", nameMC);

                emailMC = cursoremail.getString(cursoremail.getColumnIndex(ContactsContract.CommonDataKinds.Email.DATA));
                Log.e("Email", emailMC);

                if (!Functions.isNullOrEmpty(nameMC))
                {
                    edMedicalContactsName_Value.setText(nameMC);
                }

                if (!Functions.isNullOrEmpty(emailMC))
                {
                    if (validate_email_from_Contact(emailMC))
                    {
                        edMedicalContactsEmailAddress_value.setText(emailMC);
                    }
                    else
                    {
                        edMedicalContactsEmailAddress_value.setText("");
                        Functions.showToast(getApplicationContext(), "Invalid Email ID");
                    }
                }
            }
            else
            {
                edMedicalContactsEmailAddress_value.setText("");
                Functions.showToast(getApplicationContext(), "No email id found");
            }
        }
        else
        {
            edMedicalContactsEmailAddress_value.setText("");
            Functions.showToast(getApplicationContext(), "Email data not avialable.");
        }

        if (cursoremail != null) {
            cursoremail.close();
        }
    }


    public void AddMedicalContactsData(String url) {
        if (MedicalContactsTypeHasValue) {
            if (validateMedicalContactName() == true && validateMedicalContactClinicName() == true && validateMedicalContactsMobileNumber() == true && validatePin() == true && validate_email() == true) {
                if (validateState) {
                    Functions.showProgress(true, mProgressBarAddMedicalContacts);

                    Map<String, String> jsonParams = new HashMap<String, String>();

                    jsonParams.put("UserId", Functions.ApplicationUserid);
                    jsonParams.put("ContactName", edMedicalContactsName_Value.getText().toString());

                    Map.Entry<String, String> spmedicalcontactType_item = (Map.Entry<String, String>) sp_MedicalContactsType_value.getSelectedItem();
                    jsonParams.put("MedContType", spmedicalcontactType_item.getKey().toString());

                    jsonParams.put("PrimaryPhone", edMedicalContactsMobile_value.getText().toString());

                    jsonParams.put("EmailAddress", edMedicalContactsEmailAddress_value.getText().toString());
                    jsonParams.put("Address1", edMedicalContactsAddress1_Value.getText().toString());
                    jsonParams.put("Address2", edMedicalContactsAddress2_Value.getText().toString());
                    jsonParams.put("CityVillage", edMedicalContactsCityVillage_Value.getText().toString());
                    jsonParams.put("PIN", edMedicalContactsPIN_value.getText().toString());
                    jsonParams.put("District", edMedicalContactsDistrict_Value.getText().toString());
                    jsonParams.put("ClinicName", edMedicalContactsClinicName_Value.getText().toString());

                    jsonParams.put("DeleteFlag", "false");
                    jsonParams.put("SourceId", Functions.SourceID);

                    Map.Entry<String, String> spStateValue_item = (Map.Entry<String, String>) sp_MedicalContactsState_value.getSelectedItem();
                    jsonParams.put("State", spStateValue_item.getKey().toString());

                    JsonObjectRequest postRequestMedicalContactsSave = new JsonObjectRequest(Request.Method.POST, url, new JSONObject(jsonParams), new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            AfterPostMedicalContacts(response);
                        }
                    },
                            new Response.ErrorListener() {
                                @Override
                                public void onErrorResponse(VolleyError error) {
                                    Functions.showProgress(false, mProgressBarAddMedicalContacts);
                                    Functions.ErrorHandling(PHRMS_MedicalContact_Fragment_Add.this, error);
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

                    // Pass values to volley to stop sending multiple requests reduced from 2500milisecond to 2000 - 20secs

                    postRequestMedicalContactsSave.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                    // Access the RequestQueue through your singleton class.
                    MySingleton.getInstance(PHRMS_MedicalContact_Fragment_Add.this).addToRequestQueue(postRequestMedicalContactsSave);

                } else {
                    Functions.showToast(PHRMS_MedicalContact_Fragment_Add.this, "Kindly Select Contact State");
                    return;
                }
            } else {
                return;
            }
        } else {
            Functions.showToast(PHRMS_MedicalContact_Fragment_Add.this, "Data not loaded. Kindly Add Again");
        }
    }


    // Required
    protected boolean validateMedicalContactName() {
        boolean bool_firstname = true;
        if (edMedicalContactsName_Value.getText().toString().trim().isEmpty()) {
            input_MedicalContactsName_value.setErrorEnabled(true);
            input_MedicalContactsName_value.setError(getString(R.string.errMedicalContactsName));
            requestFocus(edMedicalContactsName_Value);
            bool_firstname = false;
        } else {
            input_MedicalContactsName_value.setError(null);
            input_MedicalContactsName_value.setErrorEnabled(false);
        }

        return bool_firstname;
    }

    // Required
    protected boolean validateMedicalContactClinicName() {
        boolean bool_firstname = true;
        if (edMedicalContactsClinicName_Value.getText().toString().trim().isEmpty()) {
            input_MedicalContactsClinicName_value.setErrorEnabled(true);
            input_MedicalContactsClinicName_value.setError(getString(R.string.errMedicalContactsClinicName));
            requestFocus(edMedicalContactsClinicName_Value);
            bool_firstname = false;
        } else {
            input_MedicalContactsClinicName_value.setError(null);
            input_MedicalContactsClinicName_value.setErrorEnabled(false);
        }

        return bool_firstname;
    }

    // Required
    protected boolean validateMedicalContactsMobileNumber() {
        boolean bool_phone = true;
        if (!Functions.isNullOrEmpty(edMedicalContactsMobile_value.getText().toString().trim())) {
            try {
                double d = Double.valueOf(edMedicalContactsMobile_value.getText().toString().trim());

                if (d == (long) d) {
                    if (edMedicalContactsMobile_value.getText().toString().trim().length() == 10) {

                        input_MedicalContactsMobile_value.setError(null);
                        input_MedicalContactsMobile_value.setErrorEnabled(false);
                        //return true;
                    } else {
                        input_MedicalContactsMobile_value.setErrorEnabled(true);
                        input_MedicalContactsMobile_value.setError(getString(R.string.errMedicalContactsMobilelenght));
                        requestFocus(edMedicalContactsMobile_value);
                        bool_phone = false;
                    }
                } else {

                    input_MedicalContactsMobile_value.setErrorEnabled(true);
                    input_MedicalContactsMobile_value.setError(getString(R.string.errMedicalContactsMobileint));
                    requestFocus(edMedicalContactsMobile_value);
                    bool_phone = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_MedicalContactsMobile_value.setErrorEnabled(true);
                input_MedicalContactsMobile_value.setError(getString(R.string.errMedicalContactsMobileint));
                requestFocus(edMedicalContactsMobile_value);
                bool_phone = false;
            }
        } else {
            input_MedicalContactsMobile_value.setErrorEnabled(true);
            input_MedicalContactsMobile_value.setError(getString(R.string.errMedicalContactsMobileRequired));
            requestFocus(edMedicalContactsMobile_value);
            bool_phone = false;
            // return true;
        }

        return bool_phone;
    }

    // not Required Case
    protected boolean validatePin() {
        boolean bool_pin = true;
        if (!Functions.isNullOrEmpty(edMedicalContactsPIN_value.getText().toString().trim())) {
            try {
                double d = Double.valueOf(edMedicalContactsPIN_value.getText().toString().trim());
                if (d == (long) d) {
                    if (edMedicalContactsPIN_value.getText().toString().trim().length() == 6) {
                        //System.out.println("integer"+(int)d);
                        input_MedicalContactsPIN_value.setError(null);
                        input_MedicalContactsPIN_value.setErrorEnabled(false);
                        //bool_pin = true;
                    } else {
                        input_MedicalContactsPIN_value.setErrorEnabled(true);
                        input_MedicalContactsPIN_value.setError(getString(R.string.errMedicalContactsPinlenght));
                        requestFocus(edMedicalContactsPIN_value);
                        bool_pin = false;
                    }
                } else {

                    input_MedicalContactsPIN_value.setErrorEnabled(true);
                    input_MedicalContactsPIN_value.setError(getString(R.string.errMedicalContactsPinint));
                    requestFocus(edMedicalContactsPIN_value);
                    bool_pin = false;
                }
            } catch (Exception e) {

                input_MedicalContactsPIN_value.setErrorEnabled(true);
                input_MedicalContactsPIN_value.setError(getString(R.string.errMedicalContactsPinint));
                requestFocus(edMedicalContactsPIN_value);
                bool_pin = false;
            }
        } else {
            input_MedicalContactsPIN_value.setError(null);
            input_MedicalContactsPIN_value.setErrorEnabled(false);

        }

        return bool_pin;
    }

    private boolean validate_email() {
        boolean emailcancel = false;
        // Check for a valid email locaddr.
        if (TextUtils.isEmpty(edMedicalContactsEmailAddress_value.getText().toString().trim())) {
            input_MedicalContactsEmailAddress_value.setErrorEnabled(true);
            input_MedicalContactsEmailAddress_value.setError(getString(R.string.errMedicalContactsEmailAddressRequired));
            requestFocus(edMedicalContactsEmailAddress_value);
        } else if (!isEmailValid(edMedicalContactsEmailAddress_value.getText().toString().trim())) {
            input_MedicalContactsEmailAddress_value.setErrorEnabled(true);
            input_MedicalContactsEmailAddress_value.setError(getString(R.string.errMedicalContactsEmailAddressfromat));
            requestFocus(edMedicalContactsEmailAddress_value);

        } else {
            input_MedicalContactsEmailAddress_value.setError(null);
            input_MedicalContactsEmailAddress_value.setErrorEnabled(false);
            emailcancel = true;
        }

        return emailcancel;
    }

    private boolean validate_email_from_Contact(String emailidfromcontact) {
        boolean emailcancel = false;
        // Check for a valid email locaddr.
        if (TextUtils.isEmpty(emailidfromcontact.toString().trim())) {
            emailcancel = false;
        } else if (!isEmailValid(emailidfromcontact.toString().trim())) {
            emailcancel = true;
        } else {
            emailcancel = true;
        }

        return emailcancel;
    }

    private boolean isEmailValid(String email) {
        //TODO: Replace this with your own logic
        boolean rt_value = false;

        if (email.contains("@") && !TextUtils.isEmpty(email) && android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            rt_value = true;
        } else if (!TextUtils.isEmpty(email)) {
            try {
                double d = Double.valueOf(email.toString().trim());
                if (d == (long) d) {
                    if (email.toString().trim().length() == 10) {
                        rt_value = true;
                    }
                }
            } catch (Exception e) {
                rt_value = false;
            }
        }

        return rt_value;
    }

    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    // Check response with id, as data with immage.
    private void AfterPostMedicalContacts(JSONObject response) {
        ParseJson_MedicalContactData addMedicalContacts_pj = new ParseJson_MedicalContactData(response);
        String STATUS_Post = addMedicalContacts_pj.parsePostResponseMedicalContacts();

        switch (STATUS_Post) {
            case "0": // Error
                Functions.showToast(PHRMS_MedicalContact_Fragment_Add.this, "Medical Contact Not Saved");
                Functions.showSnackbar(parentLayout, "Medical Contact Not Saved", "Action");
                Functions.showProgress(false, mProgressBarAddMedicalContacts);
                break;
            case "1": // Success
                Functions.showProgress(false, mProgressBarAddMedicalContacts);
                Intent intMedicalContacts = new Intent(PHRMS_MedicalContact_Fragment_Add.this, PHRMS_MedicalContact_Fragment.class);
                intMedicalContacts.putExtra("MedicalContactsSaved", 1);
                setResult(RESULT_OK, intMedicalContacts);
                finish();
                break;
            case "2": // Phone Number Already Exist
                Functions.showToast(PHRMS_MedicalContact_Fragment_Add.this, "Phone Number Already Exist");
                Functions.showSnackbar(parentLayout, "Phone Number Already Exist", "Action");
                Functions.showProgress(false, mProgressBarAddMedicalContacts);
                break;
            case "3": // Email Already Exist
                Functions.showToast(PHRMS_MedicalContact_Fragment_Add.this, "Email Address Already Exist");
                Functions.showSnackbar(parentLayout, "Email Address Already Exist", "Action");
                Functions.showProgress(false, mProgressBarAddMedicalContacts);
                break;
            default:
                Functions.showToast(PHRMS_MedicalContact_Fragment_Add.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarAddMedicalContacts);
                break;
        }

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.savebuttonmenu, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                finish();
                return true;
            case R.id.savedata:
                // Post data with url
                if (Functions.isNetworkAvailable(PHRMS_MedicalContact_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_MedicalContact_Fragment_Add.this);
                    } else {
                        AddMedicalContactsData(getString(R.string.urlLogin) + getString(R.string.AddMedicalContact));
                    }
                } else {
                    Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
                }
                return true;
            default:
                return super.onOptionsItemSelected(item);
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
                case R.id.edMedicalContactsName_Value:
                    // Required check for empty also
                    validateMedicalContactName();
                    break;
                case R.id.edMedicalContactsClinicName_Value:
                    //Required check for empty also
                    validateMedicalContactClinicName();
                    break;
                case R.id.edMedicalContactsMobile_value:
                    //Required check for empty also
                    validateMedicalContactsMobileNumber();
                    break;
                case R.id.edMedicalContactsPIN_value:
                    //Required check for empty also
                    validatePin();
                    break;
                case R.id.edMedicalContactsEmailAddress_value:
                    //Required check for empty also
                    validate_email();
                    break;
            }
        }
    }


    // ReadContacts
    public void readcontact(Integer PICK) {
        Intent intent = new Intent(Intent.ACTION_PICK, ContactsContract.Contacts.CONTENT_URI);
        startActivityForResult(intent, PICK);
    }


}

package com.pkg.healthrecordappname.appfinalname.modules.addfragments;

import android.app.DialogFragment;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.TextInputLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.WindowManager;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DatePickerFragment;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DateValidator;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Immunization_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_ImmunizationData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;


public class PHRMS_Immunization_Fragment_Add extends AppCompatActivity {

    private ProgressBar mProgressBarAddImmunization;
    private TextView txtImmunizationNameValue;
    private EditText edImmunizationTakenon_Value;
    private TextInputLayout input_ImmunizationTakenon_value;
    private EditText edImmunizationComments_value;

    private Boolean boolMenu = false;

    private View parentLayout;
    private int ImmunizationID = -1;



    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_immunization_add);
        parentLayout = findViewById(R.id.svImmunizationAdd);

        //toolbar
        Toolbar mtoolbar_add_Immunization = (Toolbar) findViewById(R.id.toolbar_addImmunization);
        if (mtoolbar_add_Immunization != null) {
            setSupportActionBar(mtoolbar_add_Immunization);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);


        mProgressBarAddImmunization = (ProgressBar) findViewById(R.id.ProgressBarAddImmunization);

        txtImmunizationNameValue = (TextView) findViewById(R.id.txtImmunizationNameValue);

        edImmunizationTakenon_Value = (EditText) findViewById(R.id.edImmunizationTakenon_Value);

        input_ImmunizationTakenon_value = (TextInputLayout) findViewById(R.id.input_ImmunizationTakenon_value);

        edImmunizationTakenon_Value.addTextChangedListener(new EditTextWatcher(edImmunizationTakenon_Value));

        edImmunizationTakenon_Value.setOnTouchListener(new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                DialogFragment datepicker = DatePickerFragment.newInstance(edImmunizationTakenon_Value);
                if (datepicker != null) {
                    datepicker.show(getFragmentManager(), "DatePickerFragment");
                }
                return false;
            }
        });

        edImmunizationComments_value = (EditText) findViewById(R.id.edImmunizationComments_value);


        Functions.progressbarStyle(mProgressBarAddImmunization, PHRMS_Immunization_Fragment_Add.this);

        if (Functions.isNetworkAvailable(PHRMS_Immunization_Fragment_Add.this)) {
            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                Functions.mainscreen(PHRMS_Immunization_Fragment_Add.this);
            } else {

                txtImmunizationNameValue.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(PHRMS_Immunization_Fragment_Add.this)) {
                            Intent intImmunizationList = new Intent(PHRMS_Immunization_Fragment_Add.this, PHRMS_ImmunizationList_Fragment.class);
                            startActivityForResult(intImmunizationList, 102);
                        } else {
                            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
                        }

                    }
                });



            }


        } else {
            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
        }
    }


    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 102) {
            if (resultCode == RESULT_OK) {
                if (data.getIntExtra("Immunization", 0) == 1) {
                    txtImmunizationNameValue.setText(data.getStringExtra("ImmunizationName")); //+ data.getStringExtra("ImmunizationID"));
                    ImmunizationID = Integer.parseInt(data.getStringExtra("ImmunizationID"));

                    boolMenu = true;
                    invalidateOptionsMenu();
                }
            }
        }
    }


    public void AddImmunizationData(String url) {

        if (validateImmunizationID() == true && validateTakenOnDate() == true) {
            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            String currentDate = sdf.format(new Date());
            String Date_To_HH = Functions.DateToDateHH(currentDate);
            if (!Date_To_HH.equals("-1")) {
                Functions.showProgress(true, mProgressBarAddImmunization);

                Map<String, String> jsonParams = new HashMap<String, String>();
                jsonParams.put("ImmunizationsTypeId", String.valueOf(ImmunizationID));
                jsonParams.put("CreatedDate", Date_To_HH);
                jsonParams.put("DeleteFlag", "false");
                jsonParams.put("ImmunizationDate", Functions.DateToDateHH(edImmunizationTakenon_Value.getText().toString()));
                jsonParams.put("ModifiedDate", Date_To_HH);
                jsonParams.put("Comments", edImmunizationComments_value.getText().toString());
                jsonParams.put("SourceId", Functions.SourceID);
                jsonParams.put("UserId", Functions.ApplicationUserid);


                JsonObjectRequest postRequestImmunization = new JsonObjectRequest(Request.Method.POST, url,
                        new JSONObject(jsonParams),
                        new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                AfterPostImmunization(response);
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Functions.showProgress(false, mProgressBarAddImmunization);
                                Functions.ErrorHandling(PHRMS_Immunization_Fragment_Add.this, error);
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

                postRequestImmunization.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                // Access the RequestQueue through your singleton class.
                MySingleton.getInstance(PHRMS_Immunization_Fragment_Add.this).addToRequestQueue(postRequestImmunization);

            } else {
                Functions.showSnackbar(parentLayout, "Invalid DOB", "Action");
            }
        } else {

            return;
        }
    }


    // Required
    protected boolean validateImmunizationID() {
        //empty
        Boolean bool_Uhid = true;
        if (ImmunizationID == -1) {
            bool_Uhid = false;
            Functions.showToast(this, "No Immunization Selected");
        }
        return bool_Uhid;
    }


    // Required
    protected boolean validateTakenOnDate() {
        boolean bool_DD = true;
        if (Functions.isNullOrEmpty(edImmunizationTakenon_Value.getText().toString())) {
            input_ImmunizationTakenon_value.setErrorEnabled(true);
            input_ImmunizationTakenon_value.setError(getString(R.string.errTakenonreq));
            requestFocus(edImmunizationTakenon_Value);
            bool_DD = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edImmunizationTakenon_Value.getText().toString(), "dd/MM/yyyy")) {
                input_ImmunizationTakenon_value.setError(null);
                input_ImmunizationTakenon_value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_ImmunizationTakenon_value.setErrorEnabled(true);
                input_ImmunizationTakenon_value.setError(getString(R.string.errTakenon));
                requestFocus(edImmunizationTakenon_Value);
                bool_DD = false;
            }
        }
        return bool_DD;
    }


    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    private void AfterPostImmunization(JSONObject response) {
        ParseJson_ImmunizationData addImmunization_pj = new ParseJson_ImmunizationData(response);
        String STATUS_Post = addImmunization_pj.parsePostResponseImmunization();

        switch (STATUS_Post) {
            case "1":
                Functions.showProgress(false, mProgressBarAddImmunization);
                Intent intImmunization = new Intent(PHRMS_Immunization_Fragment_Add.this, PHRMS_Immunization_Fragment.class);
                intImmunization.putExtra("ImmunizationSaved", 1);
                setResult(RESULT_OK, intImmunization);
                finish();

                break;
            case "0":
                Functions.showSnackbar(parentLayout, "Immunization Info - Nothing To Change", "Action");
                Functions.showProgress(false, mProgressBarAddImmunization);
                //Snackbar.make(getView(), "Profile Info - Nothing To Change", Snackbar.LENGTH_SHORT).setAction("Action", null).show();
                break;
            default:
                Functions.showToast(PHRMS_Immunization_Fragment_Add.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarAddImmunization);
                break;
        }

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.savebuttonmenu, menu);

        // find save button and set false;
        MenuItem item = menu.findItem(R.id.savedata);
        //could be used with bool value
        item.setVisible(boolMenu);

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
                if (Functions.isNetworkAvailable(PHRMS_Immunization_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_Immunization_Fragment_Add.this);
                    } else {
                        AddImmunizationData(getString(R.string.urlLogin) + getString(R.string.AddImmunizationData));
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
                case R.id.edImmunizationTakenon_Value:
                    // Required check for empty also
                    validateTakenOnDate();
                    break;
            }
        }
    }


}
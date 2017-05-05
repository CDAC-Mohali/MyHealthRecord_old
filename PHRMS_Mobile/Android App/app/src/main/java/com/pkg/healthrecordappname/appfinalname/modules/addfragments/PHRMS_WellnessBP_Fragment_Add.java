
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

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DatePickerFragment;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DateValidator;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_WellnessBP_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_WellnessBPData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;


public class PHRMS_WellnessBP_Fragment_Add extends AppCompatActivity {

    private ProgressBar mProgressBarAddWellnessBP;


    private EditText edWellnessS_Value;
    private TextInputLayout input_WellnessS_Value;

    private EditText edWellnessD_Value;
    private TextInputLayout input_WellnessD_Value;


    private EditText edWellnessP_Value;
    private TextInputLayout input_WellnessP_Value;

    private EditText edWellnessBPDate_Value;
    private TextInputLayout input_WellnessBPDate_Value;

    private EditText edWellnessBPN_value;



    private View parentLayout;


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_wellnessbp_add);
        parentLayout = findViewById(R.id.svWellnessBPAdd);

        //toolbar
        Toolbar mtoolbar_add_WellnessBP = (Toolbar) findViewById(R.id.toolbar_addWellnessBP);
        if (mtoolbar_add_WellnessBP != null) {
            setSupportActionBar(mtoolbar_add_WellnessBP);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarAddWellnessBP = (ProgressBar) findViewById(R.id.ProgressBarAddWellnessBP);


        edWellnessS_Value = (EditText) findViewById(R.id.edWellnessS_Value);
        input_WellnessS_Value = (TextInputLayout) findViewById(R.id.input_WellnessS_Value);
        edWellnessS_Value.addTextChangedListener(new EditTextWatcher(edWellnessS_Value));

        edWellnessD_Value = (EditText) findViewById(R.id.edWellnessD_Value);
        input_WellnessD_Value = (TextInputLayout) findViewById(R.id.input_WellnessD_Value);
        edWellnessD_Value.addTextChangedListener(new EditTextWatcher(edWellnessD_Value));

        edWellnessP_Value = (EditText) findViewById(R.id.edWellnessP_Value);
        input_WellnessP_Value = (TextInputLayout) findViewById(R.id.input_WellnessP_Value);
        edWellnessP_Value.addTextChangedListener(new EditTextWatcher(edWellnessP_Value));

        edWellnessBPDate_Value = (EditText) findViewById(R.id.edWellnessBPDate_Value);
        input_WellnessBPDate_Value = (TextInputLayout) findViewById(R.id.input_WellnessBPDate_value);
        edWellnessBPDate_Value.addTextChangedListener(new EditTextWatcher(edWellnessBPDate_Value));
        edWellnessBPDate_Value.setOnTouchListener(new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                DialogFragment datepicker = DatePickerFragment.newInstance(edWellnessBPDate_Value);
                if (datepicker != null) {
                    datepicker.show(getFragmentManager(), "DatePickerFragment");
                }
                return false;
            }
        });

        edWellnessBPN_value = (EditText) findViewById(R.id.edWellnessBPN_value);

        Functions.progressbarStyle(mProgressBarAddWellnessBP, PHRMS_WellnessBP_Fragment_Add.this);


    }

    public void AddWellnessBPData(String url) {

        if (validateWellnessBPSYS() == true && validateWellnessBPDYS() == true && validateWellnessBPPulse() == true && validateWellnessBPDate() == true) {
            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            String currentDate = sdf.format(new Date());
            String Date_To_HH = Functions.DateToDateHH(currentDate);
            if (!Date_To_HH.equals("-1")) {
                Functions.showProgress(true, mProgressBarAddWellnessBP);

                Map<String, String> jsonParams = new HashMap<String, String>();

                jsonParams.put("ResSystolic", edWellnessS_Value.getText().toString());
                jsonParams.put("ResDiastolic", edWellnessD_Value.getText().toString());
                jsonParams.put("ResPulse", edWellnessP_Value.getText().toString());
                jsonParams.put("CollectionDate", Functions.DateToDateHH(edWellnessBPDate_Value.getText().toString()));
                jsonParams.put("CreatedDate", Date_To_HH);
                jsonParams.put("ModifiedDate", Date_To_HH);

                jsonParams.put("Comments", edWellnessBPN_value.getText().toString());

                jsonParams.put("DeleteFlag", "false");
                jsonParams.put("SourceId", Functions.SourceID);
                jsonParams.put("UserId", Functions.ApplicationUserid);



                JsonObjectRequest postRequestWellnessBP = new JsonObjectRequest(Request.Method.POST, url,
                        new JSONObject(jsonParams),
                        new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                AfterPostWellnessBP(response);
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Functions.showProgress(false, mProgressBarAddWellnessBP);
                                Functions.ErrorHandling(PHRMS_WellnessBP_Fragment_Add.this, error);
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

                postRequestWellnessBP.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                // Access the RequestQueue through your singleton class.
                MySingleton.getInstance(PHRMS_WellnessBP_Fragment_Add.this).addToRequestQueue(postRequestWellnessBP);

            } else {
                Functions.showSnackbar(parentLayout, "Invalid DOB", "Action");
            }
        } else {

            return;
        }
    }

    // Required
    protected boolean validateWellnessBPSYS() {
        //empty
        Boolean bool_WellnessBP_SYS = true;

        if (edWellnessS_Value.getText().toString().trim().isEmpty()) {
            input_WellnessS_Value.setErrorEnabled(true);
            input_WellnessS_Value.setError(getString(R.string.errBPSYSreq));
            requestFocus(edWellnessS_Value);
            bool_WellnessBP_SYS = false;
        } else {
            try {
                double d = Double.valueOf(edWellnessS_Value.getText().toString().trim());

                //valid integer
                if (d == (long) d) {
                    if (d <= 260) {
                        input_WellnessS_Value.setError(null);
                        input_WellnessS_Value.setErrorEnabled(false);
                    } else {
                        input_WellnessS_Value.setErrorEnabled(true);
                        input_WellnessS_Value.setError(getString(R.string.errBPSYSRange));
                        requestFocus(edWellnessS_Value);
                        bool_WellnessBP_SYS = false;
                    }
                } else {
                    input_WellnessS_Value.setErrorEnabled(true);
                    input_WellnessS_Value.setError(getString(R.string.errBPSYSformat));
                    requestFocus(edWellnessS_Value);
                    bool_WellnessBP_SYS = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_WellnessS_Value.setErrorEnabled(true);
                input_WellnessS_Value.setError(getString(R.string.errBPSYSformat));
                requestFocus(edWellnessS_Value);
                bool_WellnessBP_SYS = false;
            }
        }

        return bool_WellnessBP_SYS;

    }

    // Required Case
    // Required
    protected boolean validateWellnessBPDYS() {
        //empty
        Boolean bool_WellnessBP_DYS = true;

        if (edWellnessD_Value.getText().toString().trim().isEmpty()) {
            input_WellnessD_Value.setErrorEnabled(true);
            input_WellnessD_Value.setError(getString(R.string.errBPDIAreq));
            requestFocus(edWellnessD_Value);
            bool_WellnessBP_DYS = false;
        } else {
            try {
                double d = Double.valueOf(edWellnessD_Value.getText().toString().trim());

                //valid integer
                if (d == (long) d) {
                    if (d <= 260) {
                        input_WellnessD_Value.setError(null);
                        input_WellnessD_Value.setErrorEnabled(false);
                    } else {
                        input_WellnessD_Value.setErrorEnabled(true);
                        input_WellnessD_Value.setError(getString(R.string.errBPDIARange));
                        requestFocus(edWellnessD_Value);
                        bool_WellnessBP_DYS = false;
                    }
                } else {
                    input_WellnessD_Value.setErrorEnabled(true);
                    input_WellnessD_Value.setError(getString(R.string.errBPDIAformat));
                    requestFocus(edWellnessD_Value);
                    bool_WellnessBP_DYS = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_WellnessD_Value.setErrorEnabled(true);
                input_WellnessD_Value.setError(getString(R.string.errBPDIAformat));
                requestFocus(edWellnessD_Value);
                bool_WellnessBP_DYS = false;
            }
        }

        return bool_WellnessBP_DYS;
    }

    // Required Case
    // Required
    protected boolean validateWellnessBPPulse() {
        //empty
        Boolean bool_WellnessBP_Pulse = true;

        if (edWellnessP_Value.getText().toString().trim().isEmpty()) {
            input_WellnessP_Value.setErrorEnabled(true);
            input_WellnessP_Value.setError(getString(R.string.errBPPulsereq));
            requestFocus(edWellnessP_Value);
            bool_WellnessBP_Pulse = false;
        } else {
            try {
                double d = Double.valueOf(edWellnessP_Value.getText().toString().trim());

                //valid integer
                if (d == (long) d) {
                    if (d <= 200) {
                        input_WellnessP_Value.setError(null);
                        input_WellnessP_Value.setErrorEnabled(false);
                    } else {
                        input_WellnessP_Value.setErrorEnabled(true);
                        input_WellnessP_Value.setError(getString(R.string.errBPPulseRange));
                        requestFocus(edWellnessP_Value);
                        bool_WellnessBP_Pulse = false;
                    }
                } else {
                    input_WellnessP_Value.setErrorEnabled(true);
                    input_WellnessP_Value.setError(getString(R.string.errBPPulseformat));
                    requestFocus(edWellnessP_Value);
                    bool_WellnessBP_Pulse = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_WellnessP_Value.setErrorEnabled(true);
                input_WellnessP_Value.setError(getString(R.string.errBPPulseformat));
                requestFocus(edWellnessP_Value);
                bool_WellnessBP_Pulse = false;
            }
        }

        return bool_WellnessBP_Pulse;
    }

    // Required
    protected boolean validateWellnessBPDate() {
        boolean bool_DD = true;
        if (Functions.isNullOrEmpty(edWellnessBPDate_Value.getText().toString())) {
            input_WellnessBPDate_Value.setErrorEnabled(true);
            input_WellnessBPDate_Value.setError(getString(R.string.errBPDatereq));
            requestFocus(edWellnessBPDate_Value);
            bool_DD = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edWellnessBPDate_Value.getText().toString(), "dd/MM/yyyy")) {
                input_WellnessBPDate_Value.setError(null);
                input_WellnessBPDate_Value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_WellnessBPDate_Value.setErrorEnabled(true);
                input_WellnessBPDate_Value.setError(getString(R.string.errBPDateformat));
                requestFocus(edWellnessBPDate_Value);
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


    private void AfterPostWellnessBP(JSONObject response) {
        ParseJson_WellnessBPData addWellnessBP_pj = new ParseJson_WellnessBPData(response);
        String STATUS_Post = addWellnessBP_pj.parsePostResponseWellnessBP();

        switch (STATUS_Post) {
            case "1":
                Functions.showProgress(false, mProgressBarAddWellnessBP);
                Intent intWellnessBP = new Intent(PHRMS_WellnessBP_Fragment_Add.this, PHRMS_WellnessBP_Fragment.class);
                intWellnessBP.putExtra("WellnessBPSaved", 1);
                setResult(RESULT_OK, intWellnessBP);
                finish();
                break;
            case "0":
                Functions.showSnackbar(parentLayout, "WellnessBP Info - Nothing To Change", "Action");

                Functions.showProgress(false, mProgressBarAddWellnessBP);
                break;
            default:
                Functions.showToast(PHRMS_WellnessBP_Fragment_Add.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarAddWellnessBP);
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
                if (Functions.isNetworkAvailable(PHRMS_WellnessBP_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_WellnessBP_Fragment_Add.this);
                    } else {
                        AddWellnessBPData(getString(R.string.urlLogin) + getString(R.string.AddBPData));
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
                case R.id.edWellnessS_Value:
                    // Required check for empty also
                    validateWellnessBPSYS();
                    break;
                case R.id.edWellnessD_Value:
                    // Required check for empty also
                    validateWellnessBPDYS();
                    break;
                case R.id.edWellnessP_Value:
                    // Required check for empty also
                    validateWellnessBPPulse();
                    break;
                case R.id.edWellnessBPDate_Value:
                    // Required check for empty also
                    validateWellnessBPDate();
                    break;
            }
        }
    }



}
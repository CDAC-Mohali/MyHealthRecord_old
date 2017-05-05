
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
import android.widget.Spinner;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DatePickerFragment;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DateValidator;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_WellnessGlucose_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_WellnessGlucoseData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.LinkedHashMapAdapter;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;



public class PHRMS_WellnessGlucose_Fragment_Add extends AppCompatActivity {

    private ProgressBar mProgressBarAddWellnessBG;


    private EditText edWellnessBGResult_D_Value;
    private TextInputLayout input_WellnessBGResult_D_Value;

    private Spinner sp_WellnessBGVT_value;
    private LinkedHashMapAdapter<String, String> bgType_adapter;

    private EditText edWellnessBGDate_Value;
    private TextInputLayout input_WellnessBGDate_Value;

    private EditText edWellnessBGN_value;



    private View parentLayout;


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_wellnessglucose_add);
        parentLayout = findViewById(R.id.svWellnessBGAdd);

        //toolbar
        Toolbar mtoolbar_add_WellnessBG = (Toolbar) findViewById(R.id.toolbar_addWellnessBG);
        if (mtoolbar_add_WellnessBG != null) {
            setSupportActionBar(mtoolbar_add_WellnessBG);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarAddWellnessBG = (ProgressBar) findViewById(R.id.ProgressBarAddWellnessBG);


        edWellnessBGResult_D_Value = (EditText) findViewById(R.id.edWellnessBGResult_D_Value);
        input_WellnessBGResult_D_Value = (TextInputLayout) findViewById(R.id.input_WellnessBGResult_D_value);
        edWellnessBGResult_D_Value.addTextChangedListener(new EditTextWatcher(edWellnessBGResult_D_Value));

        sp_WellnessBGVT_value = (Spinner) findViewById(R.id.sp_WellnessBGVT_value);
        // Binding using LinkedhashMapAdapter
        bgType_adapter = new LinkedHashMapAdapter<String, String>(PHRMS_WellnessGlucose_Fragment_Add.this, android.R.layout.simple_spinner_dropdown_item, Functions.BG_ValueType_LinkHasMap());
        bgType_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        sp_WellnessBGVT_value.setAdapter(bgType_adapter);


        edWellnessBGDate_Value = (EditText) findViewById(R.id.edWellnessBGDate_Value);
        input_WellnessBGDate_Value = (TextInputLayout) findViewById(R.id.input_WellnessBGDate_value);
        edWellnessBGDate_Value.addTextChangedListener(new EditTextWatcher(edWellnessBGDate_Value));
        edWellnessBGDate_Value.setOnTouchListener(new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                DialogFragment datepicker = DatePickerFragment.newInstance(edWellnessBGDate_Value);
                if (datepicker != null) {
                    datepicker.show(getFragmentManager(), "DatePickerFragment");
                }
                return false;
            }
        });

        edWellnessBGN_value = (EditText) findViewById(R.id.edWellnessBGN_value);


        Functions.progressbarStyle(mProgressBarAddWellnessBG, PHRMS_WellnessGlucose_Fragment_Add.this);


    }

    public void AddWellnessBGData(String url) {

        if (validateWellnessBGResult() == true && validateWellnessBGDate() == true) {
            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            String currentDate = sdf.format(new Date());
            String Date_To_HH = Functions.DateToDateHH(currentDate);
            if (!Date_To_HH.equals("-1")) {
                Functions.showProgress(true, mProgressBarAddWellnessBG);

                Map<String, String> jsonParams = new HashMap<String, String>();

                jsonParams.put("Result", edWellnessBGResult_D_Value.getText().toString());

                Map.Entry<String, String> spValueType_item = (Map.Entry<String, String>) sp_WellnessBGVT_value.getSelectedItem();
                jsonParams.put("ValueType", spValueType_item.getKey().toString());

                jsonParams.put("CollectionDate", Functions.DateToDateHH(edWellnessBGDate_Value.getText().toString()));
                jsonParams.put("CreatedDate", Date_To_HH);
                jsonParams.put("ModifiedDate", Date_To_HH);
                jsonParams.put("Comments", edWellnessBGN_value.getText().toString());
                jsonParams.put("DeleteFlag", "false");
                jsonParams.put("SourceId", Functions.SourceID);
                jsonParams.put("UserId", Functions.ApplicationUserid);



                JsonObjectRequest postRequestWellnessBG = new JsonObjectRequest(Request.Method.POST, url,
                        new JSONObject(jsonParams),
                        new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                AfterPostWellnessBG(response);
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Functions.showProgress(false, mProgressBarAddWellnessBG);
                                Functions.ErrorHandling(PHRMS_WellnessGlucose_Fragment_Add.this, error);
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

                postRequestWellnessBG.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                // Access the RequestQueue through your singleton class.
                MySingleton.getInstance(PHRMS_WellnessGlucose_Fragment_Add.this).addToRequestQueue(postRequestWellnessBG);

            } else {
                Functions.showSnackbar(parentLayout, "Invalid DOB", "Action");
            }
        } else {

            return;
        }
    }

    // Required
    protected boolean validateWellnessBGResult() {
        //empty
        Boolean bool_WellnessBG_Result = true;

        if (edWellnessBGResult_D_Value.getText().toString().trim().isEmpty()) {
            input_WellnessBGResult_D_Value.setErrorEnabled(true);
            input_WellnessBGResult_D_Value.setError(getString(R.string.errBGResultreq));
            requestFocus(edWellnessBGResult_D_Value);
            bool_WellnessBG_Result = false;
        } else {
            try {
                double d = Double.valueOf(edWellnessBGResult_D_Value.getText().toString().trim());

                //valid integer
                if (d == (long) d) {
                    if (d <= 450) {
                        input_WellnessBGResult_D_Value.setError(null);
                        input_WellnessBGResult_D_Value.setErrorEnabled(false);
                    } else {
                        input_WellnessBGResult_D_Value.setErrorEnabled(true);
                        input_WellnessBGResult_D_Value.setError(getString(R.string.errBGResultRange));
                        requestFocus(edWellnessBGResult_D_Value);
                        bool_WellnessBG_Result = false;
                    }
                } else {
                    input_WellnessBGResult_D_Value.setErrorEnabled(true);
                    input_WellnessBGResult_D_Value.setError(getString(R.string.errBGResultformat));
                    requestFocus(edWellnessBGResult_D_Value);
                    bool_WellnessBG_Result = false;
                }
            } catch (Exception e) {
                //Resulttem.out.println("not number");
                input_WellnessBGResult_D_Value.setErrorEnabled(true);
                input_WellnessBGResult_D_Value.setError(getString(R.string.errBGResultformat));
                requestFocus(edWellnessBGResult_D_Value);
                bool_WellnessBG_Result = false;
            }
        }

        return bool_WellnessBG_Result;
    }


    // Required
    protected boolean validateWellnessBGDate() {
        boolean bool_DD = true;
        if (Functions.isNullOrEmpty(edWellnessBGDate_Value.getText().toString())) {
            input_WellnessBGDate_Value.setErrorEnabled(true);
            input_WellnessBGDate_Value.setError(getString(R.string.errBGDatereq));
            requestFocus(edWellnessBGDate_Value);
            bool_DD = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edWellnessBGDate_Value.getText().toString(), "dd/MM/yyyy")) {
                input_WellnessBGDate_Value.setError(null);
                input_WellnessBGDate_Value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_WellnessBGDate_Value.setErrorEnabled(true);
                input_WellnessBGDate_Value.setError(getString(R.string.errBGDateformat));
                requestFocus(edWellnessBGDate_Value);
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


    private void AfterPostWellnessBG(JSONObject response) {
        ParseJson_WellnessGlucoseData addWellnessBG_pj = new ParseJson_WellnessGlucoseData(response);
        String STATUS_Post = addWellnessBG_pj.parsePostResponseWellnessBG();

        switch (STATUS_Post) {
            case "1":
                Functions.showProgress(false, mProgressBarAddWellnessBG);
                Intent intWellnessBG = new Intent(PHRMS_WellnessGlucose_Fragment_Add.this, PHRMS_WellnessGlucose_Fragment.class);
                intWellnessBG.putExtra("WellnessBGSaved", 1);
                setResult(RESULT_OK, intWellnessBG);
                finish();
                break;
            case "0":
                Functions.showProgress(false, mProgressBarAddWellnessBG);
                Functions.showSnackbar(parentLayout, "WellnessBG Info - Nothing To Change", "Action");
                //Snackbar.make(getView(), "Profile Info - Nothing To Change", Snackbar.LENGTH_SHORT).setAction("Action", null).show();
                break;
            default:
                Functions.showProgress(false, mProgressBarAddWellnessBG);
                Functions.showToast(PHRMS_WellnessGlucose_Fragment_Add.this, STATUS_Post);
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
                if (Functions.isNetworkAvailable(PHRMS_WellnessGlucose_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_WellnessGlucose_Fragment_Add.this);
                    } else {
                        AddWellnessBGData(getString(R.string.urlLogin) + getString(R.string.AddBloodGlucoseData));
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
                case R.id.edWellnessBGResult_D_Value:
                    // Required check for empty also
                    validateWellnessBGResult();
                    break;
                case R.id.edWellnessBGDate_Value:
                    // Required check for empty also
                    validateWellnessBGDate();
                    break;
            }
        }
    }



}
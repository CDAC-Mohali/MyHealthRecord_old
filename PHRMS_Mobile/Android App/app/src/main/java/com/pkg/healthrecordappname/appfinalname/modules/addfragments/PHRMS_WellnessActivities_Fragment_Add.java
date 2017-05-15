
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
import android.widget.NumberPicker;
import android.widget.ProgressBar;
import android.widget.Spinner;
import android.widget.TextView;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DatePickerFragment;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DateValidator;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_WellnessActivities_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_WellnessActivitiesData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.LinkedHashMapAdapter;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;


public class PHRMS_WellnessActivities_Fragment_Add extends AppCompatActivity {
    //String url = null;
    private ProgressBar mProgressBarAddWellnessActivity;

    private Spinner sp_WellnessActivity_value;
    private LinkedHashMapAdapter<String, String> activity_adapter;

    private EditText edWellnessActivityPath_Value;
    private TextInputLayout input_WellnessActivityPath_value;

    private EditText edWellnessActivityDist_Value;
    private TextInputLayout input_WellnessActivityDist_value;

    private NumberPicker nP_WellnessActivityTTH_value;
    private NumberPicker nP_WellnessActivityTTM_value;

    private EditText edWellnessActivityDate_Value;
    private TextInputLayout input_WellnessActivityDate_Value;

    private EditText edWellnessActivitynotes_value;


    private View parentLayout;

    private TextView txtWellnessActivityTTHErrorMessage;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_wellnessactivities_add);
        parentLayout = findViewById(R.id.svWellnessActivityAdd);

        //toolbar
        Toolbar mtoolbar_add_WellnessActivity = (Toolbar) findViewById(R.id.toolbar_addWellnessActivity);
        if (mtoolbar_add_WellnessActivity != null) {
            setSupportActionBar(mtoolbar_add_WellnessActivity);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarAddWellnessActivity = (ProgressBar) findViewById(R.id.ProgressBarAddWellnessActivity);

        txtWellnessActivityTTHErrorMessage = (TextView)findViewById(R.id.txtWellnessActivityTTHErrorMessage);

        sp_WellnessActivity_value = (Spinner) findViewById(R.id.sp_WellnessActivity_value);
        // Binding using LinkedhashMapAdapter
        activity_adapter = new LinkedHashMapAdapter<String, String>(PHRMS_WellnessActivities_Fragment_Add.this, android.R.layout.simple_spinner_dropdown_item, Functions.Activity_LinkHasMap());
        activity_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        sp_WellnessActivity_value.setAdapter(activity_adapter);

        edWellnessActivityPath_Value = (EditText) findViewById(R.id.edWellnessActivityPath_Value);
        input_WellnessActivityPath_value = (TextInputLayout) findViewById(R.id.input_WellnessActivityPath_value);
        edWellnessActivityPath_Value.addTextChangedListener(new EditTextWatcher(edWellnessActivityPath_Value));

        edWellnessActivityDist_Value = (EditText) findViewById(R.id.edWellnessActivityDist_Value);
        input_WellnessActivityDist_value = (TextInputLayout) findViewById(R.id.input_WellnessActivityDist_value);
        edWellnessActivityDist_Value.addTextChangedListener(new EditTextWatcher(edWellnessActivityDist_Value));

        edWellnessActivityDate_Value = (EditText) findViewById(R.id.edWellnessActivityDate_Value);
        input_WellnessActivityDate_Value = (TextInputLayout) findViewById(R.id.input_WellnessActivityDate_value);
        edWellnessActivityDate_Value.addTextChangedListener(new EditTextWatcher(edWellnessActivityDate_Value));
        edWellnessActivityDate_Value.setOnTouchListener(new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                DialogFragment datepicker = DatePickerFragment.newInstance(edWellnessActivityDate_Value);
                if (datepicker != null) {
                    datepicker.show(getFragmentManager(), "DatePickerFragment");
                }
                return false;
            }
        });

        edWellnessActivitynotes_value = (EditText) findViewById(R.id.edWellnessActivityN_value);

        nP_WellnessActivityTTH_value = (NumberPicker) findViewById(R.id.nP_WellnessActivityTTH_value);

        //Populate NumberPicker values from minimum and maximum value range
        //Set the minimum value of NumberPicker
        nP_WellnessActivityTTH_value.setMinValue(0);
        //Specify the maximum value/number of NumberPicker
        nP_WellnessActivityTTH_value.setMaxValue(24);

        //Gets whether the selector wheel wraps when reaching the min/max value.
        nP_WellnessActivityTTH_value.setWrapSelectorWheel(true);

//        //Set a value change listener for NumberPicker
        nP_WellnessActivityTTH_value.setOnValueChangedListener(new NumberPicker.OnValueChangeListener() {
            @Override
            public void onValueChange(NumberPicker picker, int oldVal, int newVal)
            {
                //Display the newly selected number from picker


                if(newVal>0)
                {
                   txtWellnessActivityTTHErrorMessage.setVisibility(View.GONE);
                }
            }
        });

        nP_WellnessActivityTTM_value = (NumberPicker) findViewById(R.id.nP_WellnessActivityTTM_value);

        //Populate NumberPicker values from minimum and maximum value range
        //Set the minimum value of NumberPicker
        nP_WellnessActivityTTM_value.setMinValue(0);
        //Specify the maximum value/number of NumberPicker
        nP_WellnessActivityTTM_value.setMaxValue(59);

        //Gets whether the selector wheel wraps when reaching the min/max value.
        nP_WellnessActivityTTM_value.setWrapSelectorWheel(true);

        Functions.progressbarStyle(mProgressBarAddWellnessActivity, PHRMS_WellnessActivities_Fragment_Add.this);

        //Set a value change listener for NumberPicker
        nP_WellnessActivityTTM_value.setOnValueChangedListener(new NumberPicker.OnValueChangeListener() {
            @Override
            public void onValueChange(NumberPicker picker, int oldVal, int newVal) {
                //Display the newly selected number from picker

                if(newVal<=0)
                {
                    txtWellnessActivityTTHErrorMessage.setVisibility(View.VISIBLE);
                }
                else
                {
                    txtWellnessActivityTTHErrorMessage.setVisibility(View.GONE);
                }
            }
        });



    }

    public void AddWellnessActivityData(String url)
    {

        if (validateWellnessActivityPath() == true && validateWellnessActivityDistance() == true && validateWellnessActivityTimeCheck() ==true && validateWellnessActivityDate() == true)
        {
            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            String currentDate = sdf.format(new Date());
            String Date_To_HH = Functions.DateToDateHH(currentDate);
            if (!Date_To_HH.equals("-1"))
            {
                int hr_min = Math.round(nP_WellnessActivityTTH_value.getValue() * 60);
                int min = hr_min + nP_WellnessActivityTTM_value.getValue();

                if(min>0)
                {
                    txtWellnessActivityTTHErrorMessage.setVisibility(View.GONE);

                    Functions.showProgress(true, mProgressBarAddWellnessActivity);

                    Map<String, String> jsonParams = new HashMap<String, String>();

                    Map.Entry<String, String> spWellnessActivity_value_item = (Map.Entry<String, String>) sp_WellnessActivity_value.getSelectedItem();
                    jsonParams.put("ActivityId", spWellnessActivity_value_item.getKey().toString());

                    jsonParams.put("Distance", edWellnessActivityDist_Value.getText().toString());
                    jsonParams.put("CollectionDate", Functions.DateToDateHH(edWellnessActivityDate_Value.getText().toString()));


                    jsonParams.put("FinishTime", String.valueOf(min));

                    jsonParams.put("PathName", edWellnessActivityPath_Value.getText().toString()); // Path
                    jsonParams.put("Comments", edWellnessActivitynotes_value.getText().toString());
                    jsonParams.put("CreatedDate", Date_To_HH);
                    jsonParams.put("ModifiedDate", Date_To_HH);
                    jsonParams.put("DeleteFlag", "false");
                    jsonParams.put("SourceId", Functions.SourceID);
                    jsonParams.put("UserId", Functions.ApplicationUserid);



                    JsonObjectRequest postRequestWellnessActivity = new JsonObjectRequest(Request.Method.POST, url,
                            new JSONObject(jsonParams),
                            new Response.Listener<JSONObject>() {
                                @Override
                                public void onResponse(JSONObject response) {
                                    AfterPostWellnessActivity(response);
                                }
                            },
                            new Response.ErrorListener() {
                                @Override
                                public void onErrorResponse(VolleyError error) {
                                    Functions.showProgress(false, mProgressBarAddWellnessActivity);
                                    Functions.ErrorHandling(PHRMS_WellnessActivities_Fragment_Add.this, error);
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

                    postRequestWellnessActivity.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                    // Access the RequestQueue through your singleton class.
                    MySingleton.getInstance(PHRMS_WellnessActivities_Fragment_Add.this).addToRequestQueue(postRequestWellnessActivity);
                }
               else
                {
                    txtWellnessActivityTTHErrorMessage.setVisibility(View.VISIBLE);
                }
            }
            else
            {
                Functions.showSnackbar(parentLayout, "Invalid Date", "Action");
            }
        } else {

            return;
        }
    }

    // Required
    protected boolean validateWellnessActivityPath() {
        boolean bool_WellnessActivityPath = true;

        if (edWellnessActivityPath_Value.getText().toString().trim().isEmpty()) {
            input_WellnessActivityPath_value.setErrorEnabled(true);
            input_WellnessActivityPath_value.setError(getString(R.string.errActivityPathreq));
            requestFocus(edWellnessActivityPath_Value);
            bool_WellnessActivityPath = false;
        } else {
            input_WellnessActivityPath_value.setError(null);
            input_WellnessActivityPath_value.setErrorEnabled(false);
        }
        return bool_WellnessActivityPath;
    }

    // Required Case
    // Required
    protected boolean validateWellnessActivityDistance() {
        //empty
        Boolean bool_ActivityDistance = true;

        if (edWellnessActivityDist_Value.getText().toString().trim().isEmpty()) {
            input_WellnessActivityDist_value.setErrorEnabled(true);
            input_WellnessActivityDist_value.setError(getString(R.string.errActivityDistreq));
            requestFocus(edWellnessActivityDist_Value);
            bool_ActivityDistance = false;
        } else {
            try {
                double d = Double.valueOf(edWellnessActivityDist_Value.getText().toString().trim());

                //valid integer or double
                if ((d == (long) d) || (d == d))
                {
                    if(d>0)
                    {
                        input_WellnessActivityDist_value.setError(null);
                        input_WellnessActivityDist_value.setErrorEnabled(false);
                    }
                    else
                    {
                        input_WellnessActivityDist_value.setErrorEnabled(true);
                        input_WellnessActivityDist_value.setError(getString(R.string.errActivityDistValid));
                        requestFocus(edWellnessActivityDist_Value);
                        bool_ActivityDistance = false;
                    }
                }
                else
                {
                    input_WellnessActivityDist_value.setErrorEnabled(true);
                    input_WellnessActivityDist_value.setError(getString(R.string.errActivityDistformat));
                    requestFocus(edWellnessActivityDist_Value);
                    bool_ActivityDistance = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_WellnessActivityDist_value.setErrorEnabled(true);
                input_WellnessActivityDist_value.setError(getString(R.string.errActivityDistformat));
                requestFocus(edWellnessActivityDist_Value);
                bool_ActivityDistance = false;
            }
        }

        return bool_ActivityDistance;
    }

    // Required
    protected boolean validateWellnessActivityTimeCheck()
    {
        boolean bool_TimeCheck = true;

        int hr_min = Math.round(nP_WellnessActivityTTH_value.getValue() * 60);
        int min = hr_min + nP_WellnessActivityTTM_value.getValue();

        if(min>0)
        {
            txtWellnessActivityTTHErrorMessage.setVisibility(View.GONE);
        }
        else
        {
            txtWellnessActivityTTHErrorMessage.setVisibility(View.VISIBLE);
            bool_TimeCheck = false;
        }
        return bool_TimeCheck;
    }


    // Required
    protected boolean validateWellnessActivityDate() {
        boolean bool_DD = true;
        if (Functions.isNullOrEmpty(edWellnessActivityDate_Value.getText().toString())) {
            input_WellnessActivityDate_Value.setErrorEnabled(true);
            input_WellnessActivityDate_Value.setError(getString(R.string.errActivityDatereq));
            requestFocus(edWellnessActivityDate_Value);
            bool_DD = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edWellnessActivityDate_Value.getText().toString(), "dd/MM/yyyy")) {
                input_WellnessActivityDate_Value.setError(null);
                input_WellnessActivityDate_Value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_WellnessActivityDate_Value.setErrorEnabled(true);
                input_WellnessActivityDate_Value.setError(getString(R.string.errActivityDateformat));
                requestFocus(edWellnessActivityDate_Value);
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


    private void AfterPostWellnessActivity(JSONObject response) {
        ParseJson_WellnessActivitiesData addWellnessActivity_pj = new ParseJson_WellnessActivitiesData(response);
        String STATUS_Post = addWellnessActivity_pj.parsePostResponseWellnessActivities();

        switch (STATUS_Post) {
            case "1":
                Functions.showProgress(false, mProgressBarAddWellnessActivity);
                Intent intWellnessActivity = new Intent(PHRMS_WellnessActivities_Fragment_Add.this, PHRMS_WellnessActivities_Fragment.class);
                intWellnessActivity.putExtra("WellnessActivitySaved", 1);
                setResult(RESULT_OK, intWellnessActivity);
                finish();
                break;
            case "0":
                Functions.showSnackbar(parentLayout, "WellnessActivity Info - Nothing To Change", "Action");

                Functions.showProgress(false, mProgressBarAddWellnessActivity);
                break;
            default:
                Functions.showToast(PHRMS_WellnessActivities_Fragment_Add.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarAddWellnessActivity);
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
                if (Functions.isNetworkAvailable(PHRMS_WellnessActivities_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_WellnessActivities_Fragment_Add.this);
                    } else {
                        AddWellnessActivityData(getString(R.string.urlLogin) + getString(R.string.AddActivitiesData));
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
                case R.id.edWellnessActivityPath_Value:
                    // Required check for empty also
                    validateWellnessActivityPath();
                    break;
                case R.id.edWellnessActivityDist_Value:
                    // Required check for empty also
                    validateWellnessActivityDistance();
                    break;
                case R.id.edWellnessActivityDate_Value:
                    // Required check for empty also
                    validateWellnessActivityDate();
                    break;
            }
        }
    }



}
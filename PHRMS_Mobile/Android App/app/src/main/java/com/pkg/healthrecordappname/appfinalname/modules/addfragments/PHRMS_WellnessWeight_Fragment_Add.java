
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
import android.widget.ImageButton;
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
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_WellnessWeight_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_WellnessWeightData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;



public class PHRMS_WellnessWeight_Fragment_Add extends AppCompatActivity {

    private ProgressBar mProgressBarAddWellnessWeight;

    private EditText edWellnessWeightW_Value;
    private TextInputLayout input_WellnessWeightW_Value;

    private EditText edWellnessWeightH_value;
    private TextInputLayout input_WellnessWeightH_value;

    private EditText edWellnessWeightDate_Value;
    private TextInputLayout input_WellnessWeightDate_Value;

    private EditText edWellnessWeightN_value;

    private TextView txtWellnessWeightBMI_value;



    private ImageButton Save_WellnessWeight;

    private View parentLayout;
    private boolean weight = false;
    private boolean height = false;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_wellnessweight_add);
        parentLayout = findViewById(R.id.svWellnessWeightAdd);

        //toolbar
        Toolbar mtoolbar_add_WellnessWeight = (Toolbar) findViewById(R.id.toolbar_addWellnessWeight);
        if (mtoolbar_add_WellnessWeight != null) {
            setSupportActionBar(mtoolbar_add_WellnessWeight);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarAddWellnessWeight = (ProgressBar) findViewById(R.id.ProgressBarAddWellnessWeight);


        edWellnessWeightW_Value = (EditText) findViewById(R.id.edWellnessWeightW_Value);
        input_WellnessWeightW_Value = (TextInputLayout) findViewById(R.id.input_WellnessWeightW_value);
        edWellnessWeightW_Value.addTextChangedListener(new EditTextWatcher(edWellnessWeightW_Value));

        edWellnessWeightH_value = (EditText) findViewById(R.id.edWellnessWeightH_Value);
        input_WellnessWeightH_value = (TextInputLayout) findViewById(R.id.input_WellnessWeightH_value);
        edWellnessWeightH_value.addTextChangedListener(new EditTextWatcher(edWellnessWeightH_value));


        edWellnessWeightDate_Value = (EditText) findViewById(R.id.edWellnessWeightDate_Value);
        input_WellnessWeightDate_Value = (TextInputLayout) findViewById(R.id.input_WellnessWeightDate_value);
        edWellnessWeightDate_Value.addTextChangedListener(new EditTextWatcher(edWellnessWeightDate_Value));
        edWellnessWeightDate_Value.setOnTouchListener(new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                DialogFragment datepicker = DatePickerFragment.newInstance(edWellnessWeightDate_Value);
                if (datepicker != null) {
                    datepicker.show(getFragmentManager(), "DatePickerFragment");
                }
                return false;
            }
        });

        edWellnessWeightN_value = (EditText) findViewById(R.id.edWellnessWeightN_value);

        txtWellnessWeightBMI_value = (TextView) findViewById(R.id.txtWellnessWeightBMI_value);

        Functions.progressbarStyle(mProgressBarAddWellnessWeight, PHRMS_WellnessWeight_Fragment_Add.this);

    }

    public void AddWellnessWeightData(String url) {

        if (validateWellnessWeightData() == true && validateWellnessHeightData() == true && validateWellnessWeightDate()) {
            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            String currentDate = sdf.format(new Date());
            String Date_To_HH = Functions.DateToDateHH(currentDate);
            if (!Date_To_HH.equals("-1")) {
                Functions.showProgress(true, mProgressBarAddWellnessWeight);

                Map<String, String> jsonParams = new HashMap<String, String>();

                // Json data as Goal but used as Height
                jsonParams.put("Result", edWellnessWeightW_Value.getText().toString());

                // Json data as Goal but used as  Weight
                jsonParams.put("Goal", edWellnessWeightH_value.getText().toString());

                jsonParams.put("CollectionDate", Functions.DateToDateHH(edWellnessWeightDate_Value.getText().toString()));
                jsonParams.put("CreatedDate", Date_To_HH);
                jsonParams.put("ModifiedDate", Date_To_HH);
                jsonParams.put("Comments", edWellnessWeightN_value.getText().toString());
                jsonParams.put("DeleteFlag", "false");
                jsonParams.put("SourceId", Functions.SourceID);
                jsonParams.put("UserId", Functions.ApplicationUserid);


                JsonObjectRequest postRequestWellnessWeight = new JsonObjectRequest(Request.Method.POST, url,
                        new JSONObject(jsonParams),
                        new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                AfterPostWellnessWeight(response);
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Functions.showProgress(false, mProgressBarAddWellnessWeight);
                                Functions.ErrorHandling(PHRMS_WellnessWeight_Fragment_Add.this, error);
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

                postRequestWellnessWeight.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                // Access the RequestQueue through your singleton class.
                MySingleton.getInstance(PHRMS_WellnessWeight_Fragment_Add.this).addToRequestQueue(postRequestWellnessWeight);

            } else {
                Functions.showSnackbar(parentLayout, "Invalid DOB", "Action");
            }
        } else {

            return;
        }
    }

    // Required
    protected boolean validateWellnessWeightData() {
        //empty
        Boolean bool_WellnessWeight = true;

        if (edWellnessWeightW_Value.getText().toString().trim().isEmpty()) {
            input_WellnessWeightW_Value.setErrorEnabled(true);
            input_WellnessWeightW_Value.setError(getString(R.string.errWeightWreq));
            requestFocus(edWellnessWeightW_Value);
            bool_WellnessWeight = false;
        } else {
            try {
                double d = Double.valueOf(edWellnessWeightW_Value.getText().toString().trim());

                //valid integer
                if (d == d) {
                    if (d <= 600f) {
                        input_WellnessWeightW_Value.setError(null);
                        input_WellnessWeightW_Value.setErrorEnabled(false);
                    } else {
                        input_WellnessWeightW_Value.setErrorEnabled(true);
                        input_WellnessWeightW_Value.setError(getString(R.string.errWeightWRange));
                        requestFocus(edWellnessWeightW_Value);
                        bool_WellnessWeight = false;
                    }
                } else {
                    input_WellnessWeightW_Value.setErrorEnabled(true);
                    input_WellnessWeightW_Value.setError(getString(R.string.errWeightWformat));
                    requestFocus(edWellnessWeightW_Value);
                    bool_WellnessWeight = false;
                }
            } catch (Exception e) {
                //Resulttem.out.println("not number");
                input_WellnessWeightW_Value.setErrorEnabled(true);
                input_WellnessWeightW_Value.setError(getString(R.string.errWeightWformat));
                requestFocus(edWellnessWeightW_Value);
                bool_WellnessWeight = false;
            }
        }

        return bool_WellnessWeight;
    }

    // Required
    protected boolean validateWellnessHeightData() {
        //empty
        Boolean bool_WellnessHeight = true;

        if (edWellnessWeightH_value.getText().toString().trim().isEmpty()) {
            input_WellnessWeightH_value.setErrorEnabled(true);
            input_WellnessWeightH_value.setError(getString(R.string.errWeightHreq));
            requestFocus(edWellnessWeightH_value);
            bool_WellnessHeight = false;
        } else {
            try {
                double d = Double.valueOf(edWellnessWeightH_value.getText().toString().trim());

                //valid integer
                if (d == d) {
                    if (d <= 300f) {
                        input_WellnessWeightH_value.setError(null);
                        input_WellnessWeightH_value.setErrorEnabled(false);
                    } else {
                        input_WellnessWeightH_value.setErrorEnabled(true);
                        input_WellnessWeightH_value.setError(getString(R.string.errWeightHRange));
                        requestFocus(edWellnessWeightH_value);
                        bool_WellnessHeight = false;
                    }
                } else {
                    input_WellnessWeightH_value.setErrorEnabled(true);
                    input_WellnessWeightH_value.setError(getString(R.string.errWeightHformat));
                    requestFocus(edWellnessWeightH_value);
                    bool_WellnessHeight = false;
                }
            } catch (Exception e) {
                //Resulttem.out.println("not number");
                input_WellnessWeightH_value.setErrorEnabled(true);
                input_WellnessWeightH_value.setError(getString(R.string.errWeightHformat));
                requestFocus(edWellnessWeightH_value);
                bool_WellnessHeight = false;
            }
        }

        return bool_WellnessHeight;
    }

    // Required
    protected boolean validateWellnessWeightDate() {
        boolean bool_DD = true;
        if (Functions.isNullOrEmpty(edWellnessWeightDate_Value.getText().toString())) {
            input_WellnessWeightDate_Value.setErrorEnabled(true);
            input_WellnessWeightDate_Value.setError(getString(R.string.errWeightDatereq));
            requestFocus(edWellnessWeightDate_Value);
            bool_DD = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edWellnessWeightDate_Value.getText().toString(), "dd/MM/yyyy")) {
                input_WellnessWeightDate_Value.setError(null);
                input_WellnessWeightDate_Value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_WellnessWeightDate_Value.setErrorEnabled(true);
                input_WellnessWeightDate_Value.setError(getString(R.string.errWeightDateformat));
                requestFocus(edWellnessWeightDate_Value);
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


    private void AfterPostWellnessWeight(JSONObject response) {
        ParseJson_WellnessWeightData addWellnessWeight_pj = new ParseJson_WellnessWeightData(response);
        String STATUS_Post = addWellnessWeight_pj.parsePostResponseWellnessWeight();

        switch (STATUS_Post) {
            case "1":
                Functions.showProgress(false, mProgressBarAddWellnessWeight);
                Intent intWellnessWeight = new Intent(PHRMS_WellnessWeight_Fragment_Add.this, PHRMS_WellnessWeight_Fragment.class);
                intWellnessWeight.putExtra("WellnessWeightSaved", 1);
                setResult(RESULT_OK, intWellnessWeight);
                finish();
                break;
            case "0":
                Functions.showProgress(false, mProgressBarAddWellnessWeight);
                Functions.showSnackbar(parentLayout, "WellnessWeight Info - Nothing To Change", "Action");

                break;
            default:
                Functions.showProgress(false, mProgressBarAddWellnessWeight);
                Functions.showToast(PHRMS_WellnessWeight_Fragment_Add.this, STATUS_Post);
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
                if (Functions.isNetworkAvailable(PHRMS_WellnessWeight_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_WellnessWeight_Fragment_Add.this);
                    } else {
                        AddWellnessWeightData(getString(R.string.urlLogin) + getString(R.string.AddWeightData));
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
                case R.id.edWellnessWeightW_Value:
                    // Required check for empty also
                    weight = validateWellnessWeightData();
                    break;
                case R.id.edWellnessWeightH_Value:
                    // Required check for empty also
                    height = validateWellnessHeightData();
                    break;
                case R.id.edWellnessWeightDate_Value:
                    // Required check for empty also
                    validateWellnessWeightDate();
                    break;
            }

            if (weight == true && height == true) {
                if (!Functions.isNullOrEmpty(edWellnessWeightW_Value.getText().toString()) && !Functions.isNullOrEmpty(edWellnessWeightH_value.getText().toString())) {
                    double bmic = Double.parseDouble(edWellnessWeightW_Value.getText().toString()) / Math.pow(Double.parseDouble(edWellnessWeightH_value.getText().toString()) / 100, 2.0);
                    String finalbmicValue = String.valueOf(Math.round(bmic * 100.0) / 100.0);
                    txtWellnessWeightBMI_value.setText(finalbmicValue);
                } else {
                    txtWellnessWeightBMI_value.setText("0");
                }
            }
        }
    }



}
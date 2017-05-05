
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
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DatePickerFragment;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DateValidator;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Problems_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_ProblemsData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;


public class PHRMS_Problems_Fragment_Add extends AppCompatActivity {

    private ProgressBar mProgressBarAddProblems;

    private TextView txtProblemsNameValue;
    private RadioGroup rdgrpProblemsstill;
    private RadioButton rdProblemsstillYes;
    private RadioButton rdProblemsstillNo;
    private EditText edProblemsDD_Value;
    private TextInputLayout input_ProblemsDD_value;
    private EditText edProblemsDBY_Value;
    private TextInputLayout input_ProblemsDB_value;
    private EditText edProblemsnotes_value;


    private Boolean boolMenu = false;

    private View parentLayout;

    private int ProblemsID = -1;



    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_problems_add);
        parentLayout = findViewById(R.id.svProblemsAdd);

        //toolbar
        Toolbar mtoolbar_add_Problems = (Toolbar) findViewById(R.id.toolbar_addProblems);
        if (mtoolbar_add_Problems != null) {
            setSupportActionBar(mtoolbar_add_Problems);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarAddProblems = (ProgressBar) findViewById(R.id.ProgressBarAddProblems);

        txtProblemsNameValue = (TextView) findViewById(R.id.txtProblemsNameValue);
        rdgrpProblemsstill = (RadioGroup) findViewById(R.id.rdgrpProblemsstill);
        rdProblemsstillYes = (RadioButton) findViewById(R.id.rdProblemsstillYes);
        rdProblemsstillNo = (RadioButton) findViewById(R.id.rdProblemsstillNo);
        rdgrpProblemsstill.check(R.id.rdProblemsstillYes);

        edProblemsDD_Value = (EditText) findViewById(R.id.edProblemsDD_Value);

        input_ProblemsDD_value = (TextInputLayout) findViewById(R.id.input_ProblemsDD_value);

        edProblemsDD_Value.addTextChangedListener(new EditTextWatcher(edProblemsDD_Value));


        edProblemsDD_Value.setOnTouchListener(new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {


                DialogFragment datepicker = DatePickerFragment.newInstance(edProblemsDD_Value);
                if (datepicker != null) {
                    datepicker.show(getFragmentManager(), "DatePickerFragment");
                }
                return false;
            }
        });

        edProblemsDBY_Value = (EditText) findViewById(R.id.edProblemsDB_Value);
        input_ProblemsDB_value = (TextInputLayout) findViewById(R.id.input_ProblemsDB_value);
        edProblemsDBY_Value.addTextChangedListener(new EditTextWatcher(edProblemsDBY_Value));

        edProblemsnotes_value = (EditText) findViewById(R.id.edProblemsnotes_value);

        Functions.progressbarStyle(mProgressBarAddProblems, PHRMS_Problems_Fragment_Add.this);

        if (Functions.isNetworkAvailable(PHRMS_Problems_Fragment_Add.this)) {
            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                Functions.mainscreen(PHRMS_Problems_Fragment_Add.this);
            } else {

                txtProblemsNameValue.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(PHRMS_Problems_Fragment_Add.this)) {
                            Intent intProblemsList = new Intent(PHRMS_Problems_Fragment_Add.this, PHRMS_ProblemsList_Fragment.class);
                            startActivityForResult(intProblemsList, 105);
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
        if (requestCode == 105) {
            if (resultCode == RESULT_OK) {
                if (data.getIntExtra("Problems", 0) == 1) {
                    txtProblemsNameValue.setText(data.getStringExtra("ProblemsName"));// + data.getStringExtra("ProblemsID"));
                    ProblemsID = Integer.parseInt(data.getStringExtra("ProblemsID"));
                    boolMenu = true;
                    invalidateOptionsMenu();
                }
            }
        }
    }


    public void AddProblemsData(String url) {

        if (validateProblemsID() == true && validateDiagnosisDate() == true && validateDiagnosedBY() == true) {
            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            String currentDate = sdf.format(new Date());
            String Date_To_HH = Functions.DateToDateHH(currentDate);
            if (!Date_To_HH.equals("-1")) {
                Functions.showProgress(true, mProgressBarAddProblems);

                Map<String, String> jsonParams = new HashMap<String, String>();
                jsonParams.put("ConditionType", String.valueOf(ProblemsID));
                jsonParams.put("CreatedDate", Date_To_HH);
                jsonParams.put("DeleteFlag", "false");
                jsonParams.put("DiagnosisDate", Functions.DateToDateHH(edProblemsDD_Value.getText().toString()));
                jsonParams.put("ModifiedDate", Date_To_HH);
                jsonParams.put("Notes", edProblemsnotes_value.getText().toString());
                jsonParams.put("Provider", edProblemsDBY_Value.getText().toString());
                jsonParams.put("SourceId", Functions.SourceID);
                int selectedId = rdgrpProblemsstill.getCheckedRadioButtonId();
                if (selectedId == rdProblemsstillYes.getId()) {
                    jsonParams.put("StillHaveCondition", "true");
                } else {
                    jsonParams.put("StillHaveCondition", "false");
                }
                jsonParams.put("UserId", Functions.ApplicationUserid);


                JsonObjectRequest postRequestProblems = new JsonObjectRequest(Request.Method.POST, url,
                        new JSONObject(jsonParams),
                        new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                AfterPostProblems(response);
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Functions.showProgress(false, mProgressBarAddProblems);
                                Functions.ErrorHandling(PHRMS_Problems_Fragment_Add.this, error);
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

                postRequestProblems.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                // Access the RequestQueue through your singleton class.
                MySingleton.getInstance(PHRMS_Problems_Fragment_Add.this).addToRequestQueue(postRequestProblems);

            } else {
                Functions.showSnackbar(parentLayout, "Invalid DOB", "Action");
            }
        } else {

            return;
        }
    }


    // Required
    protected boolean validateProblemsID() {
        //empty
        Boolean bool_Uhid = true;
        if (ProblemsID == -1) {
            bool_Uhid = false;
            Functions.showToast(this, "No Problems Selected");
        }
        return bool_Uhid;
    }


    // Required
    protected boolean validateDiagnosisDate() {
        boolean bool_DD = true;
        if (Functions.isNullOrEmpty(edProblemsDD_Value.getText().toString())) {
            input_ProblemsDD_value.setErrorEnabled(true);
            input_ProblemsDD_value.setError(getString(R.string.errddreq));
            requestFocus(edProblemsDD_Value);
            bool_DD = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edProblemsDD_Value.getText().toString(), "dd/MM/yyyy")) {
                input_ProblemsDD_value.setError(null);
                input_ProblemsDD_value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_ProblemsDD_value.setErrorEnabled(true);
                input_ProblemsDD_value.setError(getString(R.string.errdd));
                requestFocus(edProblemsDD_Value);
                bool_DD = false;
            }
        }
        return bool_DD;
    }

    // Required
    protected boolean validateDiagnosedBY() {
        boolean bool_DiagnosedBY = true;
        if (Functions.isNullOrEmpty(edProblemsDBY_Value.getText().toString().trim())) {
            input_ProblemsDB_value.setErrorEnabled(true);
            input_ProblemsDB_value.setError(getString(R.string.errdby));
            requestFocus(edProblemsDBY_Value);
            bool_DiagnosedBY = false;
        } else {
            input_ProblemsDB_value.setError(null);
            input_ProblemsDB_value.setErrorEnabled(false);
        }

        return bool_DiagnosedBY;
    }

    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    private void AfterPostProblems(JSONObject response) {
        ParseJson_ProblemsData addProblems_pj = new ParseJson_ProblemsData(response);
        String STATUS_Post = addProblems_pj.parsePostResponseProblems();

        switch (STATUS_Post) {
            case "1":
                Intent intProblems = new Intent(PHRMS_Problems_Fragment_Add.this, PHRMS_Problems_Fragment.class);
                intProblems.putExtra("ProblemsSaved", 1);
                setResult(RESULT_OK, intProblems);
                finish();
                Functions.showProgress(false, mProgressBarAddProblems);
                break;
            case "0":
                Functions.showSnackbar(parentLayout, "Problems Info - Nothing To Change", "Action");
                //Snackbar.make(getView(), "Profile Info - Nothing To Change", Snackbar.LENGTH_SHORT).setAction("Action", null).show();
                Functions.showProgress(false, mProgressBarAddProblems);
                break;
            default:
                Functions.showToast(PHRMS_Problems_Fragment_Add.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarAddProblems);
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
                if (Functions.isNetworkAvailable(PHRMS_Problems_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_Problems_Fragment_Add.this);
                    } else {
                        AddProblemsData(getString(R.string.urlLogin) + getString(R.string.AddProblemsData));
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
                case R.id.edProblemsDD_Value:
                    // Required check for empty also
                    validateDiagnosisDate();
                    break;
                case R.id.edProblemsDB_Value:
                    // Required check for empty also
                    validateDiagnosedBY();
                    break;
            }
        }
    }


}
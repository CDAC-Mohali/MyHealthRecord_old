
package com.pkg.healthrecordappname.appfinalname.modules.addfragments;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.WindowManager;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.SeekBar;
import android.widget.TextView;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Allergies_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_AllergyData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;


/**
 * A placeholder fragment containing a simple view for frame_allergies.xml layout.
 */
public class PHRMS_Allergies_Fragment_Add extends AppCompatActivity {

    private ProgressBar mProgressBarAddAllergy;
    private ProgressBar mProgressBarAllergyFrom;
    private ProgressBar mProgressBarAllergySeverity;

    private TextView txtallergyNameValue;
    private RadioGroup rdgrpallergystill;
    private RadioButton rdallergystillYes;
    private RadioButton rdallergystillNo;
    private SeekBar seekbarallergyfrom;
    private Boolean allergyfromHasValue = false;

    private SeekBar seekbarallergySeverity;
    private Boolean allergySeverityHasValue = false;

    private EditText edallergynotes_value;


    private Boolean boolMenu = false;

    private View parentLayout;

    private int AllergyID = -1;


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_allergies_add);

        parentLayout = findViewById(R.id.svallergydata);

        //toolbar
        Toolbar mtoolbar_add_allergy = (Toolbar) findViewById(R.id.toolbar_addallergy);
        if (mtoolbar_add_allergy != null) {
            setSupportActionBar(mtoolbar_add_allergy);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);



        mProgressBarAddAllergy = (ProgressBar) findViewById(R.id.ProgressBarAddAllergy);

        mProgressBarAllergyFrom = (ProgressBar) findViewById(R.id.ProgressBarAllergyFrom);

        mProgressBarAllergySeverity = (ProgressBar) findViewById(R.id.ProgressBarAllergySeverity);

        txtallergyNameValue = (TextView) findViewById(R.id.txtallergyNameValue);
        rdgrpallergystill = (RadioGroup) findViewById(R.id.rdgrpallergystill);
        rdallergystillYes = (RadioButton) findViewById(R.id.rdallergystillYes);
        rdallergystillNo = (RadioButton) findViewById(R.id.rdallergystillNo);
        rdgrpallergystill.check(R.id.rdallergystillYes);

        seekbarallergyfrom = (SeekBar) findViewById(R.id.seekbarallergyfrom);
        seekbarallergySeverity = (SeekBar) findViewById(R.id.seekbarallergySeverity);
        edallergynotes_value = (EditText) findViewById(R.id.edallergynotes_value);



        Functions.progressbarStyle(mProgressBarAddAllergy, PHRMS_Allergies_Fragment_Add.this);
        Functions.progressbarStyle(mProgressBarAllergyFrom, PHRMS_Allergies_Fragment_Add.this);
        Functions.progressbarStyle(mProgressBarAllergySeverity, PHRMS_Allergies_Fragment_Add.this);

        if (Functions.isNetworkAvailable(PHRMS_Allergies_Fragment_Add.this)) {
            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                Functions.mainscreen(PHRMS_Allergies_Fragment_Add.this);
            } else {

                txtallergyNameValue.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(PHRMS_Allergies_Fragment_Add.this)) {


                            Intent intAllergyList = new Intent(PHRMS_Allergies_Fragment_Add.this, PHRMS_AllergiesList_Fragment.class);
                            startActivityForResult(intAllergyList, 101);
                        } else {
                            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
                        }

                    }
                });



                loadseekbarfrom();
                loadseekbarseverity();

            }


        } else {
            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
        }
    }

    public void loadseekbarfrom() {

        Functions.showProgress(true, mProgressBarAllergyFrom);

        String url_allergy_duration = getString(R.string.urlLogin) + getString(R.string.GetAllergyDuration);


        final JsonObjectRequest jsObjRequestSeekBarFrom = new JsonObjectRequest(Request.Method.GET, url_allergy_duration, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONDataToSeekBarFrom(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressBarAllergyFrom);
                Functions.ErrorHandling(PHRMS_Allergies_Fragment_Add.this, error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(PHRMS_Allergies_Fragment_Add.this).addToRequestQueue(jsObjRequestSeekBarFrom);
    }

    public void LoadJSONDataToSeekBarFrom(JSONObject js) {

        ParseJson_AllergyData from_pj = new ParseJson_AllergyData(js);
        String STATUS = from_pj.parseJsonFrom();
        if (STATUS.equals("1")) {

            seekbarallergyfrom.setProgress(0);
            seekbarallergyfrom.incrementProgressBy(1);
            seekbarallergyfrom.setMax(ParseJson_AllergyData.fromId.length - 1);

            if (ParseJson_AllergyData.fromId.length > 0) {
                allergyfromHasValue = true;
            }


            seekbarallergyfrom.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {

                @Override
                public void onProgressChanged(SeekBar seekBar, int progresValue, boolean fromUser) {
                    if (fromUser) {
                        if (progresValue >= 0 && progresValue <= seekBar.getMax()) {

                            seekBar.setSecondaryProgress(progresValue);
                        }
                    }
                }

                @Override
                public void onStartTrackingTouch(SeekBar seekBar) {

                }

                @Override
                public void onStopTrackingTouch(SeekBar seekBar) {

                }
            });
        } else {
            Functions.showToast(PHRMS_Allergies_Fragment_Add.this, "Unable to load From Data");
        }

        Functions.showProgress(false, mProgressBarAllergyFrom);
    }

    // Severity
    public void loadseekbarseverity() {

        Functions.showProgress(true, mProgressBarAllergySeverity);

        String url_allergy_severity = getString(R.string.urlLogin) + getString(R.string.GetAllergySeverity);


        final JsonObjectRequest jsObjRequestSeekBarSeverity = new JsonObjectRequest(Request.Method.GET, url_allergy_severity, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONDataToSeekBarSeverity(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressBarAllergySeverity);
                Functions.ErrorHandling(PHRMS_Allergies_Fragment_Add.this, error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(PHRMS_Allergies_Fragment_Add.this).addToRequestQueue(jsObjRequestSeekBarSeverity);
    }

    public void LoadJSONDataToSeekBarSeverity(JSONObject js) {

        ParseJson_AllergyData severity_pj = new ParseJson_AllergyData(js);
        String STATUS = severity_pj.parseJsonSeverity();
        if (STATUS.equals("1")) {


            seekbarallergySeverity.setProgress(1);
            seekbarallergySeverity.incrementProgressBy(1);
            seekbarallergySeverity.setMax(ParseJson_AllergyData.severityId.length - 1);

            if (ParseJson_AllergyData.severityId.length > 0) {
                allergySeverityHasValue = true;
            }

            seekbarallergyfrom.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {

                @Override
                public void onProgressChanged(SeekBar seekBar, int progresValue, boolean fromUser) {
                    if (fromUser) {
                        if (progresValue >= 0 && progresValue <= seekBar.getMax()) {

                            seekBar.setSecondaryProgress(progresValue);
                        }
                    }
                }

                @Override
                public void onStartTrackingTouch(SeekBar seekBar) {

                }

                @Override
                public void onStopTrackingTouch(SeekBar seekBar) {


                }
            });
        } else {
            Functions.showToast(PHRMS_Allergies_Fragment_Add.this, "Unable to load Severity Data");
        }

        Functions.showProgress(false, mProgressBarAllergySeverity);
    }


    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 101) {
            if (resultCode == RESULT_OK) {

                if (data.getIntExtra("Allergy", 0) == 1) {
                    txtallergyNameValue.setText(data.getStringExtra("AllergyName"));
                    AllergyID = Integer.parseInt(data.getStringExtra("AllergyID"));

                    boolMenu = true;
                    invalidateOptionsMenu();
                }
            }
        }
    }


    public void AddAllergyData(String url) {

        if (validateAllergyID() == true) {
            if (allergyfromHasValue == true && allergySeverityHasValue == true) {
                SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
                String currentDate = sdf.format(new Date());
                String Date_To_HH = Functions.DateToDateHH(currentDate);
                if (!Date_To_HH.equals("-1")) {
                    Functions.showProgress(true, mProgressBarAddAllergy);

                    Map<String, String> jsonParams = new HashMap<String, String>();

                    jsonParams.put("AllergyType", String.valueOf(AllergyID));
                    jsonParams.put("Comments", edallergynotes_value.getText().toString());
                    jsonParams.put("CreatedDate", Date_To_HH);
                    jsonParams.put("DeleteFlag", "false");
                    jsonParams.put("DurationId", ParseJson_AllergyData.fromId[seekbarallergyfrom.getProgress()]); //seekbarallergyfrom.getTag().toString()
                    jsonParams.put("ModifiedDate", Date_To_HH);
                    jsonParams.put("Severity", ParseJson_AllergyData.severityId[seekbarallergySeverity.getProgress()]);//seekbarallergySeverity.getTag().toString()
                    jsonParams.put("SourceId", Functions.SourceID);

                    int selectedId = rdgrpallergystill.getCheckedRadioButtonId();
                    if (selectedId == rdallergystillYes.getId()) {
                        jsonParams.put("Still_Have", "true");
                    } else {
                        jsonParams.put("Still_Have", "false");
                    }

                    jsonParams.put("UserId", Functions.ApplicationUserid);


                    JsonObjectRequest postRequestAllergy = new JsonObjectRequest(Request.Method.POST, url,
                            new JSONObject(jsonParams),
                            new Response.Listener<JSONObject>() {
                                @Override
                                public void onResponse(JSONObject response) {
                                    AfterPostAlergy(response);
                                }
                            },
                            new Response.ErrorListener() {
                                @Override
                                public void onErrorResponse(VolleyError error) {
                                    Functions.showProgress(false, mProgressBarAddAllergy);
                                    Functions.ErrorHandling(PHRMS_Allergies_Fragment_Add.this, error);
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

                    postRequestAllergy.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

                    // Access the RequestQueue through your singleton class.
                    MySingleton.getInstance(PHRMS_Allergies_Fragment_Add.this).addToRequestQueue(postRequestAllergy);

                } else {
                    Functions.showSnackbar(parentLayout, "Invalid DOB", "Action");
                }
            } else {
                Functions.showToast(PHRMS_Allergies_Fragment_Add.this, "Select Allergy from and Severity");
            }
        } else {
            Functions.showToast(PHRMS_Allergies_Fragment_Add.this, "No Allergy Selected");
        }
    }


    // Required
    protected boolean validateAllergyID() {
        //empty
        Boolean bool_Uhid = true;
        if (AllergyID == -1) {
            bool_Uhid = false;
        }
        return bool_Uhid;
    }

    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    private void AfterPostAlergy(JSONObject response) {
        ParseJson_AllergyData addAllergy_pj = new ParseJson_AllergyData(response);
        String STATUS_Post = addAllergy_pj.parsePostResponseAllergy();

        switch (STATUS_Post) {
            case "1":
                Intent intAllergy = new Intent(PHRMS_Allergies_Fragment_Add.this, PHRMS_Allergies_Fragment.class);
                intAllergy.putExtra("AllergySaved", 1);
                setResult(RESULT_OK, intAllergy);
                finish();
                Functions.showProgress(false, mProgressBarAddAllergy);
                break;
            case "0":
                Functions.showSnackbar(parentLayout, "Allergy Info - Nothing To Change", "Action");
                Functions.showProgress(false, mProgressBarAddAllergy);

                break;
            default:
                Functions.showToast(PHRMS_Allergies_Fragment_Add.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarAddAllergy);
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
                if (Functions.isNetworkAvailable(PHRMS_Allergies_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_Allergies_Fragment_Add.this);
                    } else {
                        AddAllergyData(getString(R.string.urlLogin) + getString(R.string.AddAllergyData));
                    }
                } else {
                    Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
                }
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }



}
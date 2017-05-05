
package com.pkg.healthrecordappname.appfinalname.modules.addfragments;

import android.Manifest;
import android.app.DialogFragment;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.os.Build;
import android.os.Bundle;
import android.support.design.widget.TextInputLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.WindowManager;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ProgressBar;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.ScrollView;
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
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Medication_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_MedicationData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.ImagePicker;
import com.pkg.healthrecordappname.appfinalname.modules.useables.LinkedHashMapAdapter;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.Locale;
import java.util.Map;


public class PHRMS_Medication_Fragment_Add extends AppCompatActivity {

    private ProgressBar mProgressBarAddMedication;

    private TextView txtMedicationNameValue;
    private RadioGroup rdgrpMedicationAreyou;
    private RadioButton rdMedicationAreyouYes;
    private RadioButton rdMedicationAreyouNo;

    private EditText edMedicationDateFP_Value;
    private TextInputLayout input_MedicationDateFP_Value;
    private EditText edMedicationStrength_Value;
    private TextInputLayout input_MedicationStrength_Value;
    private EditText edMedicationnotes_value;

    private Spinner sp_MedicationRoute_value;
    private Boolean MedicationRouteHasValue = false;

    private Spinner sp_MedicationDT_value;
    private Boolean MedicationDTHasValue = false;

    private Spinner sp_MedicationDT_Unit_value;
    private Boolean MedicationDT_UnitHasValue = false;

    private Spinner sp_MedicationFT_value;
    private Boolean MedicationFTHasValue = false;

    private EditText edMedicationLI_value;

    private ScrollView svMedicationAdd;

    //private ImageButton Save_Medication;
    private Boolean boolMenu = false;

    private View parentLayout;

    private int MedicationID = -1;


    private LinkedHashMapAdapter<String, String> route_adapter;
    private LinkedHashMapAdapter<String, String> dosageValue_adapter;
    private LinkedHashMapAdapter<String, String> dosageUnit_adapter;
    private LinkedHashMapAdapter<String, String> frequency_adapter;


    // Image Pickers
    private ImageButton imgbtnMedicationAttachments;

    private Boolean imageExists = false;
    private String imageBase64string = "-1";
    public static final int PICK_IMAGE_ID = 204;
    private static final int PERMISSION_REQUEST_CODE_MULTIPLE = 304;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_medication_add);
        parentLayout = findViewById(R.id.svMedicationAdd);

        //toolbar
        Toolbar mtoolbar_add_Medication = (Toolbar) findViewById(R.id.toolbar_addMedication);
        if (mtoolbar_add_Medication != null) {
            setSupportActionBar(mtoolbar_add_Medication);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarAddMedication = (ProgressBar) findViewById(R.id.ProgressBarAddMedication);

        txtMedicationNameValue = (TextView) findViewById(R.id.txtMedicationNameValue);

        rdgrpMedicationAreyou = (RadioGroup) findViewById(R.id.rdgrpMedicationAreyou);
        rdMedicationAreyouYes = (RadioButton) findViewById(R.id.rdMedicationAreyouYes);
        rdMedicationAreyouNo = (RadioButton) findViewById(R.id.rdMedicationAreyouNo);
        rdgrpMedicationAreyou.check(R.id.rdMedicationAreyouYes);

        edMedicationDateFP_Value = (EditText) findViewById(R.id.edMedicationDateFP_Value);

        input_MedicationDateFP_Value = (TextInputLayout) findViewById(R.id.input_MedicationDateFP_value);

        edMedicationDateFP_Value.addTextChangedListener(new EditTextWatcher(edMedicationDateFP_Value));

        edMedicationDateFP_Value.setOnTouchListener(new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                DialogFragment datepicker = DatePickerFragment.newInstance(edMedicationDateFP_Value);
                if (datepicker != null) {
                    datepicker.show(getFragmentManager(), "DatePickerFragment");
                }
                return false;
            }
        });

        sp_MedicationRoute_value = (Spinner) findViewById(R.id.sp_MedicationRoute_value);

        edMedicationStrength_Value = (EditText) findViewById(R.id.edMedicationStrength_Value);
        input_MedicationStrength_Value = (TextInputLayout) findViewById(R.id.input_MedicationStrength_value);
        edMedicationStrength_Value.addTextChangedListener(new EditTextWatcher(edMedicationStrength_Value));

        sp_MedicationDT_value = (Spinner) findViewById(R.id.sp_MedicationDT_value);
        sp_MedicationDT_Unit_value = (Spinner) findViewById(R.id.sp_MedicationDT_Unit_value);

        sp_MedicationFT_value = (Spinner) findViewById(R.id.sp_MedicationFT_value);
        edMedicationLI_value = (EditText) findViewById(R.id.edMedicationLI_value);

        edMedicationnotes_value = (EditText) findViewById(R.id.edMedicationN_value);


        svMedicationAdd = (ScrollView) findViewById(R.id.svMedicationAdd);
        svMedicationAdd.setVisibility(View.INVISIBLE);

        imgbtnMedicationAttachments = (ImageButton) findViewById(R.id.imgbtnMedicationAttachments);
        ImagePicker.setMinQuality(300, 300);

        Functions.progressbarStyle(mProgressBarAddMedication, PHRMS_Medication_Fragment_Add.this);

        if (Functions.isNetworkAvailable(PHRMS_Medication_Fragment_Add.this)) {
            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                Functions.mainscreen(PHRMS_Medication_Fragment_Add.this);
            } else {

                loadRouteSpinner();
                loadDosageValueSpinner();
                loadDosageUnitSpinner();
                loadFrequencySpinner();

                txtMedicationNameValue.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(PHRMS_Medication_Fragment_Add.this)) {
                            Intent intMedicationList = new Intent(PHRMS_Medication_Fragment_Add.this, PHRMS_MedicationList_Fragment.class);
                            startActivityForResult(intMedicationList, 104);
                        } else {
                            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
                        }
                    }
                });



                imgbtnMedicationAttachments.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Build.VERSION.SDK_INT >= 23) {
                            if ((Functions.hasCameraPermission(PHRMS_Medication_Fragment_Add.this) == true) && (Functions.hasExternalStoragePermission(PHRMS_Medication_Fragment_Add.this) == true)) {
                                Log.e("testing", "Permission is granted");
                                // Activity result code is integrated to ImagePicker - v2
                                ImagePicker.pickImage(PHRMS_Medication_Fragment_Add.this, "Select your Medication Image:", PICK_IMAGE_ID);
                            } else {

                                Functions.checkpermissions(getApplicationContext(), PHRMS_Medication_Fragment_Add.this, PERMISSION_REQUEST_CODE_MULTIPLE);
                            }
                        } else {
                            //permission is automatically granted on sdk<23 upon installation
                            Log.e("testing", "Permission is already granted");
                            ImagePicker.pickImage(PHRMS_Medication_Fragment_Add.this, "Select your Medication Image:", PICK_IMAGE_ID);
                        }
                    }
                });

            }
        } else {
            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
        }
    }

    public void loadRouteSpinner() {
        Functions.showProgress(true, mProgressBarAddMedication);

        String url_Medication_Route = getString(R.string.urlLogin) + getString(R.string.GetMedicationRoute);


        final JsonObjectRequest jsObjRequestMedicationRoute = new JsonObjectRequest(Request.Method.GET, url_Medication_Route, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONDataToRouteSpinner(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressBarAddMedication);
                Functions.ErrorHandling(PHRMS_Medication_Fragment_Add.this, error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(PHRMS_Medication_Fragment_Add.this).addToRequestQueue(jsObjRequestMedicationRoute);
    }

    public void LoadJSONDataToRouteSpinner(JSONObject jsroute) {
        ParseJson_MedicationData route_pj = new ParseJson_MedicationData(jsroute);
        String STATUS = route_pj.parseJsonRoute();
        if (STATUS.equals("1")) {
            // Binding using LinkedhashMapAdapter from parsed data
            route_adapter = new LinkedHashMapAdapter<String, String>(PHRMS_Medication_Fragment_Add.this, android.R.layout.simple_spinner_dropdown_item, ParseJson_MedicationData.hmMedicationRoute);
            route_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            sp_MedicationRoute_value.setAdapter(route_adapter);

            if (ParseJson_MedicationData.hmMedicationRoute.size() > 0) {
                MedicationRouteHasValue = true;
            }

            if (svMedicationAdd.getVisibility() == View.INVISIBLE) {
                svMedicationAdd.setVisibility(View.VISIBLE);
            }
        } else {
            Functions.showToast(PHRMS_Medication_Fragment_Add.this, "Unable to load Route Data");
        }

        Functions.showProgress(false, mProgressBarAddMedication);
    }

    public void loadDosageValueSpinner() {
        Functions.showProgress(true, mProgressBarAddMedication);

        String url_dosagevalue = getString(R.string.urlLogin) + getString(R.string.GetMedicationDosageValue);


        final JsonObjectRequest jsObjRequestMedicationRoute = new JsonObjectRequest(Request.Method.GET, url_dosagevalue, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONDataToDosageValueSpinner(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressBarAddMedication);
                Functions.ErrorHandling(PHRMS_Medication_Fragment_Add.this, error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(PHRMS_Medication_Fragment_Add.this).addToRequestQueue(jsObjRequestMedicationRoute);


    }

    public void LoadJSONDataToDosageValueSpinner(JSONObject jsDosageValue) {
        ParseJson_MedicationData DosageValue_pj = new ParseJson_MedicationData(jsDosageValue);
        String STATUS = DosageValue_pj.parseJsonDosageValue();
        if (STATUS.equals("1")) {
            // Binding using LinkedhashMapAdapter from parsed data
            dosageValue_adapter = new LinkedHashMapAdapter<String, String>(PHRMS_Medication_Fragment_Add.this, android.R.layout.simple_spinner_dropdown_item, ParseJson_MedicationData.hmMedicationDosageValue);
            dosageValue_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            sp_MedicationDT_value.setAdapter(dosageValue_adapter);

            if (ParseJson_MedicationData.hmMedicationDosageValue.size() > 0) {
                MedicationDTHasValue = true;
            }

            if (svMedicationAdd.getVisibility() == View.INVISIBLE) {
                svMedicationAdd.setVisibility(View.VISIBLE);
            }
        } else {
            Functions.showToast(PHRMS_Medication_Fragment_Add.this, "Unable to load Route Data");
        }

        Functions.showProgress(false, mProgressBarAddMedication);
    }


    public void loadDosageUnitSpinner() {
        Functions.showProgress(true, mProgressBarAddMedication);

        String url_dosageunit = getString(R.string.urlLogin) + getString(R.string.GetMedicationDosageUnit);


        final JsonObjectRequest jsObjRequestMedicationUnit = new JsonObjectRequest(Request.Method.GET, url_dosageunit, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONDataToDosageUnitSpinner(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressBarAddMedication);
                Functions.ErrorHandling(PHRMS_Medication_Fragment_Add.this, error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(PHRMS_Medication_Fragment_Add.this).addToRequestQueue(jsObjRequestMedicationUnit);
    }

    public void LoadJSONDataToDosageUnitSpinner(JSONObject jsDosageUnit) {
        ParseJson_MedicationData DosageUnit_pj = new ParseJson_MedicationData(jsDosageUnit);
        String STATUS = DosageUnit_pj.parseJsonDosageUnit();
        if (STATUS.equals("1")) {
            // Binding using LinkedhashMapAdapter from parsed data
            dosageUnit_adapter = new LinkedHashMapAdapter<String, String>(PHRMS_Medication_Fragment_Add.this, android.R.layout.simple_spinner_dropdown_item, ParseJson_MedicationData.hmMedicationDosageUnit);
            dosageUnit_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            sp_MedicationDT_Unit_value.setAdapter(dosageUnit_adapter);

            if (ParseJson_MedicationData.hmMedicationDosageUnit.size() > 0) {
                MedicationDT_UnitHasValue = true;
            }

        } else {
            Functions.showToast(PHRMS_Medication_Fragment_Add.this, "Unable to load Route Data");
        }

        Functions.showProgress(false, mProgressBarAddMedication);
    }

    public void loadFrequencySpinner() {
        Functions.showProgress(true, mProgressBarAddMedication);

        String url_Frequency = getString(R.string.urlLogin) + getString(R.string.GetMedicationFrequencyList);


        final JsonObjectRequest jsObjRequestMedicationFrequency = new JsonObjectRequest(Request.Method.GET, url_Frequency, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONDataToFrequencySpinner(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressBarAddMedication);
                Functions.ErrorHandling(PHRMS_Medication_Fragment_Add.this, error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(PHRMS_Medication_Fragment_Add.this).addToRequestQueue(jsObjRequestMedicationFrequency);
    }

    public void LoadJSONDataToFrequencySpinner(JSONObject jsFrequency) {
        ParseJson_MedicationData Frequency_pj = new ParseJson_MedicationData(jsFrequency);
        String STATUS = Frequency_pj.parseJsonFrequency();
        if (STATUS.equals("1")) {
            // Binding using LinkedhashMapAdapter from parsed data
            frequency_adapter = new LinkedHashMapAdapter<String, String>(PHRMS_Medication_Fragment_Add.this, android.R.layout.simple_spinner_dropdown_item, ParseJson_MedicationData.hmMedicationFrequency);
            frequency_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            sp_MedicationFT_value.setAdapter(frequency_adapter);

            if (ParseJson_MedicationData.hmMedicationFrequency.size() > 0) {
                MedicationFTHasValue = true;
            }

            if (svMedicationAdd.getVisibility() == View.INVISIBLE) {
                svMedicationAdd.setVisibility(View.VISIBLE);
            }
        } else {
            Functions.showToast(PHRMS_Medication_Fragment_Add.this, "Unable to load Route Data");
        }

        Functions.showProgress(false, mProgressBarAddMedication);
    }




    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        switch (requestCode) {
            case PERMISSION_REQUEST_CODE_MULTIPLE: {
                LinkedHashMap<String, Integer> perms = new LinkedHashMap<String, Integer>();
                // Initial
                perms.put(Manifest.permission.WRITE_EXTERNAL_STORAGE, PackageManager.PERMISSION_GRANTED);
                perms.put(Manifest.permission.CAMERA, PackageManager.PERMISSION_GRANTED);

                // Fill with results
                for (int i = 0; i < permissions.length; i++)
                    perms.put(permissions[i], grantResults[i]);


                if (perms.get(Manifest.permission.WRITE_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED && perms.get(Manifest.permission.CAMERA) == PackageManager.PERMISSION_GRANTED) {
                    ImagePicker.pickImage(PHRMS_Medication_Fragment_Add.this, "Select your Medication Image:", PICK_IMAGE_ID);
                } else {
                    // Permission Denied
                    Functions.showToast(PHRMS_Medication_Fragment_Add.this, "Some Permission Denied");
                }
            }
            break;
            default:
                super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }


    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        switch (requestCode) {
            case 104:
                if (resultCode == RESULT_OK) {
                    if (data.getIntExtra("Medication", 0) == 1) {
                        txtMedicationNameValue.setText(data.getStringExtra("MedicationName"));// + data.getStringExtra("MedicationID"));
                        MedicationID = Integer.parseInt(data.getStringExtra("MedicationID"));
                        //Save_Medication.setVisibility(View.VISIBLE);
                        boolMenu = true;
                        invalidateOptionsMenu();
                    }
                }
                break;
            case PICK_IMAGE_ID:
                // If ImagePicker Intent is Canceld
                // TODO use bitmap
                if (resultCode == RESULT_OK) {
                    Bitmap bitmap_labtest = ImagePicker.getImageFromResult(PHRMS_Medication_Fragment_Add.this, requestCode, resultCode, data);
                    if (bitmap_labtest != null) {
                        imgbtnMedicationAttachments.setImageBitmap(bitmap_labtest);
                        imageBase64string = Functions.bitmapToBase64(bitmap_labtest);
                        // Check converted string and set the value to true
                        if (!Functions.isNullOrEmpty(imageBase64string) && !imageBase64string.equals("-1")) {
                            imageExists = true;
                        }
                    }
                }
                break;
            default:
                super.onActivityResult(requestCode, resultCode, data);
                break;
        }
    }


    public void AddMedicationData(String url) {
        if (MedicationRouteHasValue == true && MedicationDTHasValue == true && MedicationDT_UnitHasValue == true && MedicationFTHasValue == true) {
            if (validateMedicationID() == true && validateDateFirstPrescribed() == true && validateMedicationStrength() == true) {
                SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
                String currentDate = sdf.format(new Date());
                String Date_To_HH = Functions.DateToDateHH(currentDate);
                if (!Date_To_HH.equals("-1")) {
                    Functions.showProgress(true, mProgressBarAddMedication);

                    Map<String, String> jsonParams = new HashMap<String, String>();

                    jsonParams.put("MedicineType", String.valueOf(MedicationID));
                    jsonParams.put("CreatedDate", Date_To_HH);
                    jsonParams.put("DeleteFlag", "false");
                    jsonParams.put("PrescribedDate", Functions.DateToDateHH(edMedicationDateFP_Value.getText().toString()));
                    jsonParams.put("DispensedDate", Functions.DateToDateHH(edMedicationDateFP_Value.getText().toString()));
                    jsonParams.put("ModifiedDate", Date_To_HH);

                    Map.Entry<String, String> sproute_item = (Map.Entry<String, String>) sp_MedicationRoute_value.getSelectedItem();
                    jsonParams.put("Route", sproute_item.getKey().toString());

                    Map.Entry<String, String> spDosValue_item = (Map.Entry<String, String>) sp_MedicationDT_value.getSelectedItem();
                    jsonParams.put("DosValue", spDosValue_item.getKey().toString());

                    Map.Entry<String, String> spDosUnit_item = (Map.Entry<String, String>) sp_MedicationDT_Unit_value.getSelectedItem();
                    jsonParams.put("DosUnit", spDosUnit_item.getKey().toString());

                    Map.Entry<String, String> spFrequency_item = (Map.Entry<String, String>) sp_MedicationFT_value.getSelectedItem();
                    jsonParams.put("Frequency", spFrequency_item.getKey().toString());

                    jsonParams.put("LabelInstructions", edMedicationLI_value.getText().toString());
                    jsonParams.put("Strength", edMedicationStrength_Value.getText().toString());
                    jsonParams.put("Notes", edMedicationnotes_value.getText().toString());
                    //jsonParams.put("arrImages", "");
                    if (imageExists == true && !Functions.isNullOrEmpty(imageBase64string) && !imageBase64string.equals("-1")) {
                        jsonParams.put("arrImages", imageBase64string);
                    } else {
                        jsonParams.put("arrImages", "");
                    }

                    jsonParams.put("SourceId", Functions.SourceID);
                    int selectedId = rdgrpMedicationAreyou.getCheckedRadioButtonId();
                    if (selectedId == rdMedicationAreyouYes.getId()) {
                        jsonParams.put("TakingMedicine", "true");
                    } else {
                        jsonParams.put("TakingMedicine", "false");
                    }
                    jsonParams.put("UserId", Functions.ApplicationUserid);



                    JsonObjectRequest postRequestMedication = new JsonObjectRequest(Request.Method.POST, url,
                            new JSONObject(jsonParams),
                            new Response.Listener<JSONObject>() {
                                @Override
                                public void onResponse(JSONObject response) {
                                    AfterPostMedication(response);
                                }
                            },
                            new Response.ErrorListener() {
                                @Override
                                public void onErrorResponse(VolleyError error) {
                                    Functions.showProgress(false, mProgressBarAddMedication);
                                    Functions.ErrorHandling(PHRMS_Medication_Fragment_Add.this, error);
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

                    postRequestMedication.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));


                    // Access the RequestQueue through your singleton class.
                    MySingleton.getInstance(PHRMS_Medication_Fragment_Add.this).addToRequestQueue(postRequestMedication);

                } else {
                    Functions.showSnackbar(parentLayout, "Invalid DOB", "Action");
                }
            } else {
                return;
            }
        } else {
            Functions.showToast(PHRMS_Medication_Fragment_Add.this, "Data not loaded. Kindly Refresh");
        }
    }


    // Required
    protected boolean validateMedicationID() {
        //empty
        Boolean bool_Uhid = true;
        if (MedicationID == -1) {
            bool_Uhid = false;
            Functions.showToast(this, "No Medication Selected");
        }
        return bool_Uhid;
    }


    // Required
    protected boolean validateDateFirstPrescribed() {
        boolean bool_DD = true;
        if (Functions.isNullOrEmpty(edMedicationDateFP_Value.getText().toString())) {
            input_MedicationDateFP_Value.setErrorEnabled(true);
            input_MedicationDateFP_Value.setError(getString(R.string.errMedicationDatereq));
            requestFocus(edMedicationDateFP_Value);
            bool_DD = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edMedicationDateFP_Value.getText().toString(), "dd/MM/yyyy")) {
                input_MedicationDateFP_Value.setError(null);
                input_MedicationDateFP_Value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_MedicationDateFP_Value.setErrorEnabled(true);
                input_MedicationDateFP_Value.setError(getString(R.string.errMedicationDateformat));
                requestFocus(edMedicationDateFP_Value);
                bool_DD = false;
            }
        }
        return bool_DD;
    }

    // Not Required Case
    protected boolean validateMedicationStrength() {
        boolean bool_mStrength = true;
        if (!Functions.isNullOrEmpty(edMedicationStrength_Value.getText().toString().trim())) {
            try {
                double d = Double.valueOf(edMedicationStrength_Value.getText().toString().trim());
                if (d == (long) d || d == d) {
                    input_MedicationStrength_Value.setError(null);
                    input_MedicationStrength_Value.setErrorEnabled(false);
                } else {
                    //System.out.println("double"+d);
                    input_MedicationStrength_Value.setErrorEnabled(true);
                    input_MedicationStrength_Value.setError(getString(R.string.errMedicationStrengthformat));
                    requestFocus(edMedicationStrength_Value);
                    bool_mStrength = false;
                }
            } catch (Exception e) {
                input_MedicationStrength_Value.setErrorEnabled(true);
                input_MedicationStrength_Value.setError(getString(R.string.errMedicationStrengthformat));
                requestFocus(edMedicationStrength_Value);
                bool_mStrength = false;
            }
        } else {
            input_MedicationStrength_Value.setError(null);
            input_MedicationStrength_Value.setErrorEnabled(false);

        }

        return bool_mStrength;
    }


    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    // Check response with id, as data with immage.
    private void AfterPostMedication(JSONObject response) {
        ParseJson_MedicationData addMedication_pj = new ParseJson_MedicationData(response);
        String STATUS_Post = addMedication_pj.parsePostResponseMedication();

        switch (STATUS_Post) {
            case "1":
                Functions.showProgress(false, mProgressBarAddMedication);
                Intent intMedication = new Intent(PHRMS_Medication_Fragment_Add.this, PHRMS_Medication_Fragment.class);
                intMedication.putExtra("MedicationSaved", 1);
                setResult(RESULT_OK, intMedication);
                finish();
                break;
            case "0":
                Functions.showSnackbar(parentLayout, "Medication Info - Nothing To Change", "Action");

                Functions.showProgress(false, mProgressBarAddMedication);
                break;
            default:
                Functions.showToast(PHRMS_Medication_Fragment_Add.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarAddMedication);
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
                if (Functions.isNetworkAvailable(PHRMS_Medication_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_Medication_Fragment_Add.this);
                    } else {
                        AddMedicationData(getString(R.string.urlLogin) + getString(R.string.AddMedicationData));
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
                case R.id.edMedicationDateFP_Value:
                    // Required check for empty also
                    validateDateFirstPrescribed();
                    break;
                case R.id.edMedicationStrength_Value:
                    // Required check for empty also
                    validateMedicationStrength();
                    break;
            }
        }
    }



}
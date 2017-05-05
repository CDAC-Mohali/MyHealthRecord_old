
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
import android.widget.TextView;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DatePickerFragment;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DateValidator;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_LabTests_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_LabTestsData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.ImagePicker;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.Locale;
import java.util.Map;


public class PHRMS_LabTests_Fragment_Add extends AppCompatActivity {

    private ProgressBar mProgressBarAddLabTests;

    private TextView txtLabTestsNameValue;
    private EditText edLabTestsTPO_Value;
    private TextInputLayout input_LabTestsTPO_value;
    private EditText edLabTestsResult_Value;
    private TextInputLayout input_LabTestsResult_value;

    private EditText edLabTestsUnit_Value;
    private TextInputLayout input_LabTestsUnit_value;

    private EditText edLabTestsnotes_value;


    private Boolean boolMenu = false;

    private View parentLayout;

    private int LabTestsID = -1;

    // Image Pickers
    private ImageButton imgbtnLabTestsAttachments;

    private Boolean imageExists = false;
    private String imageBase64string = "-1"; // no image
    private static final int PICK_IMAGE_ID = 203;


    private static final int PERMISSION_REQUEST_CODE_MULTIPLE = 303;


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_labtests_add);
        parentLayout = findViewById(R.id.svLabTestsAdd);

        //toolbar
        Toolbar mtoolbar_add_LabTests = (Toolbar) findViewById(R.id.toolbar_addLabTests);
        if (mtoolbar_add_LabTests != null) {
            setSupportActionBar(mtoolbar_add_LabTests);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        //invalidateOptionsMenu();

        mProgressBarAddLabTests = (ProgressBar) findViewById(R.id.ProgressBarAddLabTests);

        txtLabTestsNameValue = (TextView) findViewById(R.id.txtLabTestsNameValue);

        edLabTestsTPO_Value = (EditText) findViewById(R.id.edLabTestsTPO_Value);

        input_LabTestsTPO_value = (TextInputLayout) findViewById(R.id.input_LabTestsTPO_value);

        edLabTestsTPO_Value.addTextChangedListener(new EditTextWatcher(edLabTestsTPO_Value));

        edLabTestsTPO_Value.setOnTouchListener(new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                DialogFragment datepicker = DatePickerFragment.newInstance(edLabTestsTPO_Value);
                if (datepicker != null) {
                    datepicker.show(getFragmentManager(), "DatePickerFragment");
                }
                return false;
            }
        });

        edLabTestsResult_Value = (EditText) findViewById(R.id.edLabTestsResult_Value);
        input_LabTestsResult_value = (TextInputLayout) findViewById(R.id.input_LabTestsResult_value);
        edLabTestsResult_Value.addTextChangedListener(new EditTextWatcher(edLabTestsResult_Value));

        edLabTestsUnit_Value = (EditText) findViewById(R.id.edLabTestsUnit_Value);
        input_LabTestsUnit_value = (TextInputLayout) findViewById(R.id.input_LabTestsUnit_value);
        edLabTestsUnit_Value.addTextChangedListener(new EditTextWatcher(edLabTestsUnit_Value));

        edLabTestsnotes_value = (EditText) findViewById(R.id.edLabTestsnotes_value);

        imgbtnLabTestsAttachments = (ImageButton) findViewById(R.id.imgbtnLabTestsAttachments);
        ImagePicker.setMinQuality(300, 300);

        Functions.progressbarStyle(mProgressBarAddLabTests, PHRMS_LabTests_Fragment_Add.this);

        if (Functions.isNetworkAvailable(PHRMS_LabTests_Fragment_Add.this)) {
            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                Functions.mainscreen(PHRMS_LabTests_Fragment_Add.this);
            } else {
                //url = getString(R.string.urlLogin) + getString(R.string.GetLabTestsTypes);
                txtLabTestsNameValue.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(PHRMS_LabTests_Fragment_Add.this)) {
                            Intent intLabTestsList = new Intent(PHRMS_LabTests_Fragment_Add.this, PHRMS_LabTestsList_Fragment.class);
                            startActivityForResult(intLabTestsList, 103);
                        } else {
                            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
                        }

                    }
                });


                imgbtnLabTestsAttachments.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {

                        if (Build.VERSION.SDK_INT >= 23) {

                            if ((Functions.hasCameraPermission(PHRMS_LabTests_Fragment_Add.this) == true) && (Functions.hasExternalStoragePermission(PHRMS_LabTests_Fragment_Add.this) == true)) {
                                Log.e("testing", "Permission is granted");
                                ImagePicker.pickImage(PHRMS_LabTests_Fragment_Add.this, "Select your Lab Test Image:", PICK_IMAGE_ID);
                            } else {

                                Functions.checkpermissions(getApplicationContext(), PHRMS_LabTests_Fragment_Add.this, PERMISSION_REQUEST_CODE_MULTIPLE);
                            }
                        } else { //permission is automatically granted on sdk<23 upon installation
                            Log.e("testing", "Permission is already granted");

                            ImagePicker.pickImage(PHRMS_LabTests_Fragment_Add.this, "Select your Lab Test Image:", PICK_IMAGE_ID);
                        }


                    }
                });

            }


        } else {
            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
        }
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
                    // All Permissions Granted

                    ImagePicker.pickImage(PHRMS_LabTests_Fragment_Add.this, "Select your Lab Test Image:", PICK_IMAGE_ID);
                } else {
                    // Permission Denied
                    Functions.showToast(PHRMS_LabTests_Fragment_Add.this, "Some Permission Denied");
                }
            }
            break;
            default:
                super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public void onActivityResult(int requestCode, int resultCode, Intent data)
    {
        switch (requestCode)
        {
            case 103:
                if (resultCode == RESULT_OK)
                {
                    if (data.getIntExtra("LabTests", 0) == 1)
                    {
                        txtLabTestsNameValue.setText(data.getStringExtra("LabTestsName"));// + data.getStringExtra("LabTestsID"));
                        LabTestsID = Integer.parseInt(data.getStringExtra("LabTestsID"));

                        boolMenu = true;
                        invalidateOptionsMenu();
                    }
                }
                break;
            case PICK_IMAGE_ID:
                if (resultCode == RESULT_OK)
                {
                    Bitmap bitmap_labtest = ImagePicker.getImageFromResult(PHRMS_LabTests_Fragment_Add.this, requestCode, resultCode, data);

                    if (bitmap_labtest != null)
                    {
                        imgbtnLabTestsAttachments.setImageBitmap(bitmap_labtest);
                        imageBase64string = Functions.bitmapToBase64(bitmap_labtest);
                        // Check converted string and set the value to true
                        if (!Functions.isNullOrEmpty(imageBase64string) && !imageBase64string.equals("-1"))
                        {
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





    public void AddLabTestsData(String url) {

        if (validateLabTestsID() == true && validateTestPerformedOnDate() == true && validateResult() == true && validateUnit() == true) {
            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            String currentDate = sdf.format(new Date());
            String Date_To_HH = Functions.DateToDateHH(currentDate);
            if (!Date_To_HH.equals("-1")) {
                Functions.showProgress(true, mProgressBarAddLabTests);

                Map<String, String> jsonParams = new HashMap<String, String>();

                jsonParams.put("TestId", String.valueOf(LabTestsID));
                jsonParams.put("CreatedDate", Date_To_HH);
                jsonParams.put("DeleteFlag", "false");
                jsonParams.put("PerformedDate", Functions.DateToDateHH(edLabTestsTPO_Value.getText().toString()));
                jsonParams.put("ModifiedDate", Date_To_HH);
                jsonParams.put("Result", edLabTestsResult_Value.getText().toString());
                jsonParams.put("Unit", edLabTestsUnit_Value.getText().toString());
                jsonParams.put("Comments", edLabTestsnotes_value.getText().toString());

                if (imageExists == true && !Functions.isNullOrEmpty(imageBase64string) && !imageBase64string.equals("-1")) {
                    jsonParams.put("arrImages", imageBase64string);
                } else {
                    jsonParams.put("arrImages", "");
                }

                jsonParams.put("SourceId", Functions.SourceID);
                jsonParams.put("UserId", Functions.ApplicationUserid);



                JsonObjectRequest postRequestLabTests = new JsonObjectRequest(Request.Method.POST, url,
                        new JSONObject(jsonParams),
                        new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                AfterPostLabTests(response);
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Functions.showProgress(false, mProgressBarAddLabTests);
                                Functions.ErrorHandling(PHRMS_LabTests_Fragment_Add.this, error);
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

                postRequestLabTests.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                // Access the RequestQueue through your singleton class.
                MySingleton.getInstance(PHRMS_LabTests_Fragment_Add.this).addToRequestQueue(postRequestLabTests);

            } else {
                Functions.showSnackbar(parentLayout, "Invalid DOB", "Action");
            }
        } else {

            return;
        }
    }


    // Required
    protected boolean validateLabTestsID() {
        //empty
        Boolean bool_Uhid = true;
        if (LabTestsID == -1) {
            bool_Uhid = false;
            Functions.showToast(this, "No LabTests Selected");
        }
        return bool_Uhid;
    }


    // Required
    protected boolean validateTestPerformedOnDate() {
        boolean bool_DD = true;
        if (Functions.isNullOrEmpty(edLabTestsTPO_Value.getText().toString())) {
            input_LabTestsTPO_value.setErrorEnabled(true);
            input_LabTestsTPO_value.setError(getString(R.string.errLabTestsTPOreq));
            requestFocus(edLabTestsTPO_Value);
            bool_DD = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edLabTestsTPO_Value.getText().toString(), "dd/MM/yyyy")) {
                input_LabTestsTPO_value.setError(null);
                input_LabTestsTPO_value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_LabTestsTPO_value.setErrorEnabled(true);
                input_LabTestsTPO_value.setError(getString(R.string.errLabTestsTPO));
                requestFocus(edLabTestsTPO_Value);
                bool_DD = false;
            }
        }
        return bool_DD;
    }

    // Not Required
    protected boolean validateResult() {
        boolean bool_Result = true;
        if (!Functions.isNullOrEmpty(edLabTestsResult_Value.getText().toString().trim())) {
            try {
                double d = Double.valueOf(edLabTestsResult_Value.getText().toString().trim());
                if ((d == (long) d) || (d == d)) {
                    input_LabTestsResult_value.setError(null);
                    input_LabTestsResult_value.setErrorEnabled(false);
                } else {
                    input_LabTestsResult_value.setErrorEnabled(true);
                    input_LabTestsResult_value.setError(getString(R.string.errLabTestsNumeric));
                    requestFocus(edLabTestsResult_Value);
                    bool_Result = false;
                }
            } catch (Exception e) {
                input_LabTestsResult_value.setErrorEnabled(true);
                input_LabTestsResult_value.setError(getString(R.string.errLabTestsNumeric));
                requestFocus(edLabTestsResult_Value);
                bool_Result = false;
            }

        } else {
            input_LabTestsResult_value.setError(null);
            input_LabTestsResult_value.setErrorEnabled(false);
        }

        return bool_Result;
    }

    // Not Required - unit - alphanumeric text Check only and should not be numeric entry
    protected boolean validateUnit() {
        boolean bool_Unit = true;
        if (!Functions.isNullOrEmpty(edLabTestsUnit_Value.getText().toString().trim())) {
            try {
                double d = Double.valueOf(edLabTestsUnit_Value.getText().toString().trim());
                if ((d == (long) d) || (d == d)) {
                    input_LabTestsUnit_value.setErrorEnabled(true);
                    input_LabTestsUnit_value.setError(getString(R.string.errLabTestsAlphaNumeric));
                    requestFocus(edLabTestsUnit_Value);
                    bool_Unit = false;
                } else {
                    input_LabTestsUnit_value.setError(null);
                    input_LabTestsUnit_value.setErrorEnabled(false);
                }
            } catch (Exception e) {
                // exeption if unit is numeric lonng or double
                input_LabTestsUnit_value.setError(null);
                input_LabTestsUnit_value.setErrorEnabled(false);
            }
        } else {
            input_LabTestsUnit_value.setError(null);
            input_LabTestsUnit_value.setErrorEnabled(false);
        }

        return bool_Unit;
    }

    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    // Check response with id, as data with immage.
    private void AfterPostLabTests(JSONObject response) {
        ParseJson_LabTestsData addLabTests_pj = new ParseJson_LabTestsData(response);
        String STATUS_Post = addLabTests_pj.parsePostResponseLabTests();

        switch (STATUS_Post) {
            case "1":
                Functions.showProgress(false, mProgressBarAddLabTests);
                Intent intLabTests = new Intent(PHRMS_LabTests_Fragment_Add.this, PHRMS_LabTests_Fragment.class);
                intLabTests.putExtra("LabTestsSaved", 1);
                setResult(RESULT_OK, intLabTests);
                finish();
                break;
            case "0":
                Functions.showSnackbar(parentLayout, "LabTests Info - Nothing To Change", "Action");
                //Snackbar.make(getView(), "Profile Info - Nothing To Change", Snackbar.LENGTH_SHORT).setAction("Action", null).show();
                Functions.showProgress(false, mProgressBarAddLabTests);
                break;
            default:
                Functions.showToast(PHRMS_LabTests_Fragment_Add.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarAddLabTests);
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
                if (Functions.isNetworkAvailable(PHRMS_LabTests_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_LabTests_Fragment_Add.this);
                    } else {
                        AddLabTestsData(getString(R.string.urlLogin) + getString(R.string.AddLabData));
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
                case R.id.edLabTestsTPO_Value:
                    // Required check for empty also
                    validateTestPerformedOnDate();
                    break;
                case R.id.edLabTestsResult_Value:
                    // Required check for empty also
                    validateResult();
                    break;
                case R.id.edLabTestsUnit_Value:
                    // Required check for empty also
                    validateUnit();
                    break;
            }
        }
    }



}
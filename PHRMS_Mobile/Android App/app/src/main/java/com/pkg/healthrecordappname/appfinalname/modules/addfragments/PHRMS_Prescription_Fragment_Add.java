
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
import com.mobsandgeeks.saripaar.ValidationError;
import com.mobsandgeeks.saripaar.Validator;
import com.mobsandgeeks.saripaar.annotation.Length;
import com.mobsandgeeks.saripaar.annotation.NotEmpty;
import com.mobsandgeeks.saripaar.annotation.Order;
import com.mobsandgeeks.saripaar.annotation.Pattern;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DatePickerFragment;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DateValidator;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Prescription_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_PrescriptionData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.ImagePicker;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;


public class PHRMS_Prescription_Fragment_Add extends AppCompatActivity implements Validator.ValidationListener {

    private Validator validatePrescriptionAddform;
    // Order - for order of execution to check validation,
    // sequence - if multiple validation is set for single control then order of sequence will be used
    @NotEmpty(messageResId = R.string.PrescriptionDocName, sequence = 1)
    @Pattern(regex = Functions.OnlyAlphabetWithSpace_Format, message = "Should contain only alphabets and space", sequence = 2)
    @Length(min = 5, message = "Doctor name must be atleast 5 characters", sequence = 3)
    @Order(1) //- used in immediate mode
    private EditText edPrescriptionDocNameValue;

    @NotEmpty(message = "Hospital name is required", sequence = 1)
    @Pattern(regex = Functions.OnlyAlphabetWithSpace_Format, message = "Should contain only alphabets and space", sequence = 2)
    @Length(min = 6, message = "Hospital name must be atleast 6 characters", sequence = 3)
    @Order(2) //- used in immediate mode
    private EditText edPrescriptionHCNameValue;

    @Length(min = 10, max = 12, message = "Phone Number can be upto 12 Digit Integer", sequence = 1)
    @Order(3) //- used in immediate mode
    private EditText edPrescriptionPhoneValue;

    @NotEmpty(message = "Prescription Date is Required", sequence = 1)
    @Order(4) //- used in immediate mode
    private EditText edPrescriptionDateValue;

    @NotEmpty(message = "Remarks are required", sequence = 1)
    @Order(5) //- used in immediate mode
    private EditText edPrescriptionRemarksValue;

    private ProgressBar mProgressBarAddPrescription;
    private EditText edPrescriptionAddressValue;
    private TextInputLayout input_PrescriptionDocName_value;
    private TextInputLayout input_PrescriptionHCName_value;
    private TextInputLayout input_PrescriptionPhone_value;
    private TextInputLayout input_PrescriptionDate_value;
    private TextInputLayout input_PrescriptionRemarks_value;

    private View parentLayout;

    // Image Pickers
    private ImageButton imgbtnPrescriptionAttachments;

    private Boolean imageExists = false;
    private String imageBase64string = "-1";
    public static final int PICK_IMAGE_ID = 234;
    private static final int PERMISSION_REQUEST_CODE_MULTIPLE = 334;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_prescription_add);
        parentLayout = findViewById(R.id.svPrescriptionAdd);

        //toolbar
        Toolbar mtoolbar_add_Prescription = (Toolbar) findViewById(R.id.toolbar_addPrescription);
        if (mtoolbar_add_Prescription != null) {
            setSupportActionBar(mtoolbar_add_Prescription);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarAddPrescription = (ProgressBar) findViewById(R.id.ProgressBarAddPrescription);

        edPrescriptionDocNameValue = (EditText) findViewById(R.id.edPrescriptionDocNameValue);
        input_PrescriptionDocName_value = (TextInputLayout) findViewById(R.id.input_PrescriptionDocName_value);

        edPrescriptionHCNameValue = (EditText) findViewById(R.id.edPrescriptionHCNameValue);
        input_PrescriptionHCName_value = (TextInputLayout) findViewById(R.id.input_PrescriptionHCName_value);

        edPrescriptionAddressValue = (EditText) findViewById(R.id.edPrescriptionAddressValue);

        edPrescriptionPhoneValue = (EditText) findViewById(R.id.edPrescriptionPhoneValue);
        input_PrescriptionPhone_value = (TextInputLayout) findViewById(R.id.input_PrescriptionPhone_value);

        edPrescriptionDateValue = (EditText) findViewById(R.id.edPrescriptionDateValue);
        input_PrescriptionDate_value = (TextInputLayout) findViewById(R.id.input_PrescriptionDate_value);

        edPrescriptionRemarksValue = (EditText) findViewById(R.id.edPrescriptionRemarksValue);
        input_PrescriptionRemarks_value = (TextInputLayout) findViewById(R.id.input_PrescriptionRemarks_value);

        // Add S Validation Listner
        validatePrescriptionAddform = new Validator(this);
        validatePrescriptionAddform.setValidationListener(this);

        edPrescriptionDocNameValue.addTextChangedListener(new EditTextWatcher(edPrescriptionDocNameValue));
        edPrescriptionHCNameValue.addTextChangedListener(new EditTextWatcher(edPrescriptionHCNameValue));
        edPrescriptionPhoneValue.addTextChangedListener(new EditTextWatcher(edPrescriptionPhoneValue));
        edPrescriptionDateValue.addTextChangedListener(new EditTextWatcher(edPrescriptionDateValue));
        edPrescriptionRemarksValue.addTextChangedListener(new EditTextWatcher(edPrescriptionRemarksValue));

        imgbtnPrescriptionAttachments = (ImageButton) findViewById(R.id.imgbtnPrescriptionAttachments);
        ImagePicker.setMinQuality(300, 300);

        Functions.progressbarStyle(mProgressBarAddPrescription, PHRMS_Prescription_Fragment_Add.this);

        edPrescriptionDateValue.setOnTouchListener(new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                DialogFragment datepicker = DatePickerFragment.newInstance(edPrescriptionDateValue);
                if (datepicker != null) {
                    datepicker.show(getFragmentManager(), "DatePickerFragment");
                }
                return false;
            }
        });

        if (Functions.isNetworkAvailable(PHRMS_Prescription_Fragment_Add.this)) {
            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                Functions.mainscreen(PHRMS_Prescription_Fragment_Add.this);
            } else {

                imgbtnPrescriptionAttachments.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Build.VERSION.SDK_INT >= 23) {
                            if ((Functions.hasCameraPermission(PHRMS_Prescription_Fragment_Add.this) == true) && (Functions.hasExternalStoragePermission(PHRMS_Prescription_Fragment_Add.this) == true)) {
                                Log.e("testing", "Permission is granted");
                                // Activity result code is integrated to ImagePicker - v2
                                ImagePicker.pickImage(PHRMS_Prescription_Fragment_Add.this, "Select your Prescription Image:", PICK_IMAGE_ID);
                            } else {

                                Functions.checkpermissions(getApplicationContext(), PHRMS_Prescription_Fragment_Add.this, PERMISSION_REQUEST_CODE_MULTIPLE);
                            }
                        } else {
                            //permission is automatically granted on sdk<23 upon installation
                            Log.e("testing", "Permission is already granted");
                            // Activity result code is integrated to ImagePicker - v2
                            ImagePicker.pickImage(PHRMS_Prescription_Fragment_Add.this, "Select your Prescription Image:", PICK_IMAGE_ID);
                        }
                    }
                });
            }
        } else {
            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
        }
    }


    @Override
    public void onValidationSucceeded() {
        // Post data with url
        if (Functions.isNetworkAvailable(PHRMS_Prescription_Fragment_Add.this)) {
            AddPrescriptionData();
        } else {
            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
        }
    }

    @Override
    public void onValidationFailed(List<ValidationError> errors) {
        for (ValidationError error : errors) {
            View verror = error.getView();
            String message = error.getCollatedErrorMessage(this);
            if (verror instanceof EditText) {
                ((EditText) verror).setError(message);
                //verror.requestFocus();
            } else if (verror instanceof TextView) {
                ((TextView) verror).setError(message);
                //verror.requestFocus();
            } else {
                Functions.showToast(PHRMS_Prescription_Fragment_Add.this, message);
            }
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
                    // Activity result code is integrated to ImagePicker - v2
                    ImagePicker.pickImage(PHRMS_Prescription_Fragment_Add.this, "Select your Prescription Image:", PICK_IMAGE_ID);
                } else {
                    // Permission Denied
                    Functions.showToast(PHRMS_Prescription_Fragment_Add.this, "Some Permission Denied");
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
            case PICK_IMAGE_ID:
                 // If ImagePicker Intent is Canceld
                // TODO use bitmap
                if (resultCode == RESULT_OK)
                {
                    Bitmap bitmap_labtest = ImagePicker.getImageFromResult(PHRMS_Prescription_Fragment_Add.this, requestCode, resultCode, data);
                    if (bitmap_labtest != null)
                    {
                        imgbtnPrescriptionAttachments.setImageBitmap(bitmap_labtest);
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



    public void AddPrescriptionData() {
        if (validatePrescriptionDocName() == true && validatePrescriptionHCName() == true && validatePhoneNumber() == true && validatePrescriptionDate() == true && validatePrescriptionRemarks() == true) {
            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            String currentDate = sdf.format(new Date());
            String Date_To_HH = Functions.DateToDateHH(currentDate);
            if (!Date_To_HH.equals("-1")) {
                String urlAddPrescription = getString(R.string.urlLogin) + getString(R.string.AddPrescriptionData);

                Functions.showProgress(true, mProgressBarAddPrescription);

                Map<String, String> jsonParams = new HashMap<String, String>();

                jsonParams.put("DocName", edPrescriptionDocNameValue.getText().toString());
                jsonParams.put("ClinicName", edPrescriptionHCNameValue.getText().toString());
                jsonParams.put("DocAddress", edPrescriptionAddressValue.getText().toString());
                jsonParams.put("DocPhone", edPrescriptionPhoneValue.getText().toString());
                jsonParams.put("PresDate", Functions.DateToDateHH(edPrescriptionDateValue.getText().toString()));
                jsonParams.put("Prescription", edPrescriptionRemarksValue.getText().toString());
                jsonParams.put("CreatedDate", Date_To_HH);
                jsonParams.put("DeleteFlag", "false");
                jsonParams.put("ModifiedDate", Date_To_HH);
                jsonParams.put("FileName", "");

                if (imageExists == true && !Functions.isNullOrEmpty(imageBase64string) && !imageBase64string.equals("-1")) {
                    jsonParams.put("arrImages", imageBase64string);
                } else {
                    jsonParams.put("arrImages", "");
                }

                jsonParams.put("SourceId", Functions.SourceID);
                jsonParams.put("UserId", Functions.ApplicationUserid);


                JsonObjectRequest postRequestPrescription = new JsonObjectRequest(Request.Method.POST, urlAddPrescription,
                        new JSONObject(jsonParams),
                        new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                AfterPostPrescription(response);
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Functions.showProgress(false, mProgressBarAddPrescription);
                                Functions.ErrorHandling(PHRMS_Prescription_Fragment_Add.this, error);
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

                postRequestPrescription.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                // Access the RequestQueue through your singleton class.
                MySingleton.getInstance(PHRMS_Prescription_Fragment_Add.this).addToRequestQueue(postRequestPrescription);
            } else {
                Functions.showSnackbar(parentLayout, "Invalid DOB", "Action");
            }
        } else {

            return;
        }
    }

    // required
    protected boolean validatePrescriptionDocName() {
        boolean bool_DocName = true;
        if (Functions.isNullOrEmpty(edPrescriptionDocNameValue.getText().toString().trim())) {
            input_PrescriptionDocName_value.setErrorEnabled(true);
            input_PrescriptionDocName_value.setError(getString(R.string.errPrescriptionDocNamereq));
            requestFocus(edPrescriptionDocNameValue);
            bool_DocName = false;
        } else {
            input_PrescriptionDocName_value.setError(null);
            input_PrescriptionDocName_value.setErrorEnabled(false);
        }

        return bool_DocName;
    }

    //required
    protected boolean validatePrescriptionHCName() {
        boolean bool_PrescriptionHCName = true;
        if (Functions.isNullOrEmpty(edPrescriptionHCNameValue.getText().toString().trim())) {
            input_PrescriptionHCName_value.setErrorEnabled(true);
            input_PrescriptionHCName_value.setError(getString(R.string.errPrescriptionHCNamereq));
            requestFocus(edPrescriptionHCNameValue);
            bool_PrescriptionHCName = false;
        } else {
            input_PrescriptionHCName_value.setError(null);
            input_PrescriptionHCName_value.setErrorEnabled(false);
        }

        return bool_PrescriptionHCName;
    }


    //required
    protected boolean validatePrescriptionRemarks() {
        boolean bool_PrescriptionRemarks = true;
        if (Functions.isNullOrEmpty(edPrescriptionRemarksValue.getText().toString().trim())) {
            input_PrescriptionRemarks_value.setErrorEnabled(true);
            input_PrescriptionRemarks_value.setError(getString(R.string.errPrescriptionRemarksreq));
            requestFocus(edPrescriptionRemarksValue);
            bool_PrescriptionRemarks = false;
        } else {
            input_PrescriptionRemarks_value.setError(null);
            input_PrescriptionRemarks_value.setErrorEnabled(false);
        }

        return bool_PrescriptionRemarks;
    }

    // Required
    protected boolean validatePrescriptionDate() {
        boolean bool_DD = true;
        if (Functions.isNullOrEmpty(edPrescriptionDateValue.getText().toString())) {
            input_PrescriptionDate_value.setErrorEnabled(true);
            input_PrescriptionDate_value.setError(getString(R.string.errPrescriptionDatereq));
            requestFocus(edPrescriptionDateValue);
            bool_DD = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edPrescriptionDateValue.getText().toString(), "dd/MM/yyyy")) {
                input_PrescriptionDate_value.setError(null);
                input_PrescriptionDate_value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_PrescriptionDate_value.setErrorEnabled(true);
                input_PrescriptionDate_value.setError(getString(R.string.errPrescriptionDateformat));
                requestFocus(edPrescriptionDateValue);
                bool_DD = false;
            }
        }
        return bool_DD;
    }

    // Not Required Case
    protected boolean validatePhoneNumber() {
        boolean bool_phone = true;
        if (!Functions.isNullOrEmpty(edPrescriptionPhoneValue.getText().toString().trim())) {

            try {
                double d = Double.valueOf(edPrescriptionPhoneValue.getText().toString().trim());
                if (d == (long) d) {
                    if (edPrescriptionPhoneValue.getText().toString().trim().length() >= 10 && edPrescriptionPhoneValue.getText().toString().trim().length() <= 12) {
                        //System.out.println("integer"+(int)d);
                        input_PrescriptionPhone_value.setError(null);
                        input_PrescriptionPhone_value.setErrorEnabled(false);
                        //return true;
                    } else {
                        input_PrescriptionPhone_value.setErrorEnabled(true);
                        input_PrescriptionPhone_value.setError(getString(R.string.errPrescriptionphonelenght));
                        requestFocus(edPrescriptionPhoneValue);
                        bool_phone = false;
                    }
                } else {
                    //System.out.println("double"+d);
                    input_PrescriptionPhone_value.setErrorEnabled(true);
                    input_PrescriptionPhone_value.setError(getString(R.string.errPrescriptionphoneint));
                    requestFocus(edPrescriptionPhoneValue);
                    bool_phone = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_PrescriptionPhone_value.setErrorEnabled(true);
                input_PrescriptionPhone_value.setError(getString(R.string.errPrescriptionphoneint));
                requestFocus(edPrescriptionPhoneValue);
                bool_phone = false;
            }
        } else {
            input_PrescriptionPhone_value.setError(null);
            input_PrescriptionPhone_value.setErrorEnabled(false);
            // return true;
        }

        return bool_phone;
    }

    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    // Check response with id, as data with immage.
    private void AfterPostPrescription(JSONObject response) {
        ParseJson_PrescriptionData addPrescription_pj = new ParseJson_PrescriptionData(response);
        String STATUS_Post = addPrescription_pj.parsePostResponsePrescription();

        switch (STATUS_Post) {
            case "1":
                Functions.showProgress(false, mProgressBarAddPrescription);
                Intent intPrescription = new Intent(PHRMS_Prescription_Fragment_Add.this, PHRMS_Prescription_Fragment.class);
                intPrescription.putExtra("PrescriptionSaved", 1);
                setResult(RESULT_OK, intPrescription);
                finish();
                break;
            case "0":
                Functions.showSnackbar(parentLayout, "Prescription Info - Nothing To Change", "Action");
                //Snackbar.make(getView(), "Profile Info - Nothing To Change", Snackbar.LENGTH_SHORT).setAction("Action", null).show();
                Functions.showProgress(false, mProgressBarAddPrescription);
                break;
            default:
                Functions.showToast(PHRMS_Prescription_Fragment_Add.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarAddPrescription);
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
                if (Functions.isNetworkAvailable(PHRMS_Prescription_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_Prescription_Fragment_Add.this);
                    } else {

                        validatePrescriptionAddform.validate(true); // Async Call;
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
                case R.id.edPrescriptionDocNameValue:
                    // Required check for empty also
                    validatePrescriptionDocName();
                    break;
                case R.id.edPrescriptionHCNameValue:
                    // Required check for empty also
                    validatePrescriptionHCName();
                    break;
                case R.id.edPrescriptionPhoneValue:
                    // Not required type check
                    validatePhoneNumber();
                    break;
                case R.id.edPrescriptionDateValue:
                    // Required check for empty also
                    validatePrescriptionDate();
                    break;
                case R.id.edPrescriptionRemarksValue:
                    // Required check for empty also
                    validatePrescriptionRemarks();
                    break;
            }
        }
    }


}
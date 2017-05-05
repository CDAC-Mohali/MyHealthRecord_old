
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
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Procedures_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_ProceduresData;
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


/**
 * A placeholder fragment containing a simple view for frame_allergies.xml layout.
 */
public class PHRMS_Procedures_Fragment_Add extends AppCompatActivity {

    private ProgressBar mProgressBarAddProcedures;

    private TextView txtProceduresNameValue;
    private EditText edProceduresDOP_Value;
    private TextInputLayout input_ProceduresDOP_value;
    private EditText edProceduresDB_Value;
    private TextInputLayout input_ProceduresDB_value;
    private EditText edProceduresnotes_value;

    private Boolean boolMenu = false;

    private View parentLayout;

    private int ProceduresID = -1;

    // Image Pickers
    private ImageButton imgbtnProcedureDS;

    private Boolean imageExists = false;
    private String imageBase64string = "-1";
    public static final int PICK_IMAGE_ID = 206;
    private static final int PERMISSION_REQUEST_CODE_MULTIPLE = 306;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_procedures_add);
        parentLayout = findViewById(R.id.svProceduresAdd);

        //toolbar
        Toolbar mtoolbar_add_Procedures = (Toolbar) findViewById(R.id.toolbar_addProcedures);
        if (mtoolbar_add_Procedures != null) {
            setSupportActionBar(mtoolbar_add_Procedures);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarAddProcedures = (ProgressBar) findViewById(R.id.ProgressBarAddProcedures);

        txtProceduresNameValue = (TextView) findViewById(R.id.txtProceduresNameValue);

        edProceduresDOP_Value = (EditText) findViewById(R.id.edProceduresDOP_Value);

        input_ProceduresDOP_value = (TextInputLayout) findViewById(R.id.input_ProceduresDOP_value);

        edProceduresDOP_Value.addTextChangedListener(new EditTextWatcher(edProceduresDOP_Value));

        edProceduresDOP_Value.setOnTouchListener(new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                DialogFragment datepicker = DatePickerFragment.newInstance(edProceduresDOP_Value);
                if (datepicker != null) {
                    datepicker.show(getFragmentManager(), "DatePickerFragment");
                }
                return false;
            }
        });

        edProceduresDB_Value = (EditText) findViewById(R.id.edProceduresDB_Value);
        input_ProceduresDB_value = (TextInputLayout) findViewById(R.id.input_ProceduresDB_value);
        edProceduresDB_Value.addTextChangedListener(new EditTextWatcher(edProceduresDB_Value));

        edProceduresnotes_value = (EditText) findViewById(R.id.edProceduresnotes_value);

        imgbtnProcedureDS = (ImageButton) findViewById(R.id.imgbtnProcedureDS);
        ImagePicker.setMinQuality(300, 300);

        Functions.progressbarStyle(mProgressBarAddProcedures, PHRMS_Procedures_Fragment_Add.this);

        if (Functions.isNetworkAvailable(PHRMS_Procedures_Fragment_Add.this)) {
            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                Functions.mainscreen(PHRMS_Procedures_Fragment_Add.this);
            } else {

                txtProceduresNameValue.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(PHRMS_Procedures_Fragment_Add.this)) {


                            Intent intProceduresList = new Intent(PHRMS_Procedures_Fragment_Add.this, PHRMS_ProceduresList_Fragment.class);
                            startActivityForResult(intProceduresList, 106);
                        } else {
                            Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
                        }

                    }
                });


                imgbtnProcedureDS.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {


                        if (Build.VERSION.SDK_INT >= 23) {
                            if ((Functions.hasCameraPermission(PHRMS_Procedures_Fragment_Add.this) == true) && (Functions.hasExternalStoragePermission(PHRMS_Procedures_Fragment_Add.this) == true)) {
                                Log.e("testing", "Permission is granted");
                                // Activity result code is integrated to ImagePicker - v2
                                ImagePicker.pickImage(PHRMS_Procedures_Fragment_Add.this, "Select your Procedures Image:", PICK_IMAGE_ID);
                            } else {

                                Functions.checkpermissions(getApplicationContext(), PHRMS_Procedures_Fragment_Add.this, PERMISSION_REQUEST_CODE_MULTIPLE);
                            }
                        } else {
                            //permission is automatically granted on sdk<23 upon installation
                            Log.e("testing", "Permission is already granted");
                            // Activity result code is integrated to ImagePicker - v2
                            ImagePicker.pickImage(PHRMS_Procedures_Fragment_Add.this, "Select your Procedures Image:", PICK_IMAGE_ID);
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
                    // Activity result code is integrated to ImagePicker - v2
                    ImagePicker.pickImage(PHRMS_Procedures_Fragment_Add.this, "Select your Procedures Image:", PICK_IMAGE_ID);
                } else {
                    // Permission Denied
                    Functions.showToast(PHRMS_Procedures_Fragment_Add.this, "Some Permission Denied");
                }
            }
            break;
            default:
                super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        switch (requestCode) {
            case 106:
                if (resultCode == RESULT_OK) {
                    if (data.getIntExtra("Procedures", 0) == 1) {
                        txtProceduresNameValue.setText(data.getStringExtra("ProceduresName"));// + data.getStringExtra("ProceduresID"));
                        ProceduresID = Integer.parseInt(data.getStringExtra("ProceduresID"));
                        //Save_Procedures.setVisibility(View.VISIBLE);
                        boolMenu = true;
                        invalidateOptionsMenu();
                    }
                }
                break;
            case PICK_IMAGE_ID:
                // If ImagePicker Intent is Canceld
                // TODO use bitmap
                if (resultCode == RESULT_OK) {
                    Bitmap bitmap_labtest = ImagePicker.getImageFromResult(PHRMS_Procedures_Fragment_Add.this, requestCode, resultCode, data);
                    if (bitmap_labtest != null) {
                        imgbtnProcedureDS.setImageBitmap(bitmap_labtest);
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


    public void AddProceduresData(String url) {

        if (validateProceduresID() == true && validateDateOfProcedure() == true && validateDiagnosedBY() == true) {
            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            String currentDate = sdf.format(new Date());
            String Date_To_HH = Functions.DateToDateHH(currentDate);
            if (!Date_To_HH.equals("-1")) {
                Functions.showProgress(true, mProgressBarAddProcedures);

                Map<String, String> jsonParams = new HashMap<String, String>();

                jsonParams.put("ProcedureType", String.valueOf(ProceduresID));
                jsonParams.put("CreatedDate", Date_To_HH);
                jsonParams.put("DeleteFlag", "false");
                jsonParams.put("EndDate", Functions.DateToDateHH(edProceduresDOP_Value.getText().toString()));
                jsonParams.put("StartDate", Functions.DateToDateHH(edProceduresDOP_Value.getText().toString()));
                jsonParams.put("ModifiedDate", Date_To_HH);
                jsonParams.put("SurgeonName", edProceduresDB_Value.getText().toString());
                jsonParams.put("Comments", edProceduresnotes_value.getText().toString());
                if (imageExists == true && !Functions.isNullOrEmpty(imageBase64string) && !imageBase64string.equals("-1")) {
                    jsonParams.put("arrImages", imageBase64string);
                } else {
                    jsonParams.put("arrImages", "");
                }
                jsonParams.put("SourceId", Functions.SourceID);
                jsonParams.put("UserId", Functions.ApplicationUserid);


                JsonObjectRequest postRequestProcedures = new JsonObjectRequest(Request.Method.POST, url,
                        new JSONObject(jsonParams),
                        new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                AfterPostProcedures(response);
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                Functions.showProgress(false, mProgressBarAddProcedures);
                                Functions.ErrorHandling(PHRMS_Procedures_Fragment_Add.this, error);
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

                postRequestProcedures.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

                // Access the RequestQueue through your singleton class.
                MySingleton.getInstance(PHRMS_Procedures_Fragment_Add.this).addToRequestQueue(postRequestProcedures);

            } else {
                Functions.showSnackbar(parentLayout, "Invalid DOB", "Action");
            }
        } else {

            return;
        }
    }


    // Required
    protected boolean validateProceduresID() {
        //empty
        Boolean bool_Uhid = true;
        if (ProceduresID == -1) {
            bool_Uhid = false;
            Functions.showToast(this, "No Procedures Selected");
        }
        return bool_Uhid;
    }


    // Required
    protected boolean validateDateOfProcedure() {
        boolean bool_DD = true;
        if (Functions.isNullOrEmpty(edProceduresDOP_Value.getText().toString())) {
            input_ProceduresDOP_value.setErrorEnabled(true);
            input_ProceduresDOP_value.setError(getString(R.string.errProcedureDOPreq));
            requestFocus(edProceduresDOP_Value);
            bool_DD = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(edProceduresDOP_Value.getText().toString(), "dd/MM/yyyy")) {
                input_ProceduresDOP_value.setError(null);
                input_ProceduresDOP_value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_ProceduresDOP_value.setErrorEnabled(true);
                input_ProceduresDOP_value.setError(getString(R.string.errProcedureDOP));
                requestFocus(edProceduresDOP_Value);
                bool_DD = false;
            }
        }
        return bool_DD;
    }

    // Required
    protected boolean validateDiagnosedBY() {
        boolean bool_DiagnosedBY = true;
        if (Functions.isNullOrEmpty(edProceduresDB_Value.getText().toString().trim())) {
            input_ProceduresDB_value.setErrorEnabled(true);
            input_ProceduresDB_value.setError(getString(R.string.errProcedureDB));
            requestFocus(edProceduresDB_Value);
            bool_DiagnosedBY = false;
        } else {
            input_ProceduresDB_value.setError(null);
            input_ProceduresDB_value.setErrorEnabled(false);
        }

        return bool_DiagnosedBY;
    }

    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    // Check response with id, as data with immage.
    private void AfterPostProcedures(JSONObject response) {
        ParseJson_ProceduresData addProcedures_pj = new ParseJson_ProceduresData(response);
        String STATUS_Post = addProcedures_pj.parsePostResponseProcedures();

        switch (STATUS_Post) {
            case "1":
                Functions.showProgress(false, mProgressBarAddProcedures);
                Intent intProcedures = new Intent(PHRMS_Procedures_Fragment_Add.this, PHRMS_Procedures_Fragment.class);
                intProcedures.putExtra("ProceduresSaved", 1);
                setResult(RESULT_OK, intProcedures);
                finish();

                break;
            case "0":
                Functions.showSnackbar(parentLayout, "Procedures Info - Nothing To Change", "Action");
                Functions.showProgress(false, mProgressBarAddProcedures);

                break;
            default:
                Functions.showToast(PHRMS_Procedures_Fragment_Add.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarAddProcedures);
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
                if (Functions.isNetworkAvailable(PHRMS_Procedures_Fragment_Add.this)) {
                    if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                        Functions.mainscreen(PHRMS_Procedures_Fragment_Add.this);
                    } else {
                        AddProceduresData(getString(R.string.urlLogin) + getString(R.string.AddProceduresData));
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
                case R.id.edProceduresDOP_Value:
                    // Required check for empty also
                    validateDateOfProcedure();
                    break;
                case R.id.edProceduresDB_Value:
                    // Required check for empty also
                    validateDiagnosedBY();
                    break;
            }
        }
    }


}
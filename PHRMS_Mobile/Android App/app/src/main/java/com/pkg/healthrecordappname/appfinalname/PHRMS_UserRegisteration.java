package com.pkg.healthrecordappname.appfinalname;


import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.TextInputLayout;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.util.Log;
import android.util.Patterns;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Spinner;
import android.widget.TextView;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.google.android.gms.common.api.CommonStatusCodes;
import com.google.android.gms.vision.barcode.Barcode;
import com.pkg.healthrecordappname.appfinalname.modules.barcode.BarcodeCaptureActivity;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_UserProfileRegData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.AADhaar_Profile;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.LinkedHashMapAdapter;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;
import org.xmlpull.v1.XmlPullParser;
import org.xmlpull.v1.XmlPullParserException;
import org.xmlpull.v1.XmlPullParserFactory;

import java.io.IOException;
import java.io.StringReader;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;


public class PHRMS_UserRegisteration extends Functions {

    private ProgressBar mProgressViewuserprofile;

    private EditText edfn_valueuserprofile;
    private EditText edln_valueuserprofile;
    private EditText edemail_valueuserprofile;
    private EditText edaadhaar_valueuserprofile;


    private Spinner spstateuserprofile;
    private EditText edmob_valueuserprofile;

    private FloatingActionButton fab_aadhaar_Profileuserprofile;

    private String userid = "-1";

    private LinkedHashMapAdapter<String, String> state_adapter;
    private LinkedHashMapAdapter<String, String> gender_adapter;


    private LinearLayout lv_profileuserprofile;
    private LinearLayout lv_profileuserprofileRegForm;
    private LinearLayout lv_profileuserprofileOTPForm;

    TextInputLayout input_edfn_valueuserprofileuserprofile;
    TextInputLayout input_edln_valueuserprofileuserprofile;
    TextInputLayout input_aadhaar_valueuserprofile;

    TextInputLayout input_email_valueuserprofile;
    TextInputLayout input_mob_valueuserprofile;

    private EditText edpwd_valueuserprofile;
    TextInputLayout input_pwd_valueuserprofile;

    private EditText edcnfpwd_valueuserprofile;
    TextInputLayout input_cnfpwd_valueuserprofile;

    TextInputLayout input_ed_valueuserprofileOTP;
    private EditText ed_valueuserprofileOTP;
    TextView txt_valueuserprofileOTP;

    Button registerOTP_button;
    Button OTPResend_button;

    private static final int RC_BARCODE_CAPTURE_USERPROFILE = 9077;
    private static final String TAG = "adhrbrUserReg";

    private View parentLayout;

    private RadioGroup rdgrpgender_valueuserprofile;
    private RadioButton rdgrpgenderMale;
    private RadioButton rdgrpgenderFemale;
    private RadioButton rdgrpgenderOthers;

    Menu menu_save;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.register_userprofile);

        parentLayout = findViewById(R.id.viewSwipeuserprofile);

        //toolbar
        Toolbar mtoolbar_UserResgistration = (Toolbar) findViewById(R.id.toolbar_UserResgistration);
        if (mtoolbar_UserResgistration != null) {
            setSupportActionBar(mtoolbar_UserResgistration);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);


        lv_profileuserprofile = (LinearLayout) findViewById(R.id.lv_profileuserprofile);

        lv_profileuserprofileRegForm = (LinearLayout) findViewById(R.id.lv_profileuserprofileRegForm);
        lv_profileuserprofileRegForm.setVisibility(View.VISIBLE);

        lv_profileuserprofileOTPForm = (LinearLayout) findViewById(R.id.lv_profileuserprofileOTPForm);
        lv_profileuserprofileOTPForm.setVisibility(View.GONE);

        registerOTP_button = (Button) findViewById(R.id.registerOTP_button);

        OTPResend_button = (Button) findViewById(R.id.OTPResend_button);

        // Add input TextWatcher
        edfn_valueuserprofile = (EditText) findViewById(R.id.edfn_valueuserprofile);
        input_edfn_valueuserprofileuserprofile = (TextInputLayout) findViewById(R.id.input_edfn_valueuserprofile);
        edfn_valueuserprofile.addTextChangedListener(new EditTextWatcher(edfn_valueuserprofile));

        // Add input TextWatcher
        edln_valueuserprofile = (EditText) findViewById(R.id.edln_valueuserprofile);
        input_edln_valueuserprofileuserprofile = (TextInputLayout) findViewById(R.id.input_edln_valueuserprofile);
        edln_valueuserprofile.addTextChangedListener(new EditTextWatcher(edln_valueuserprofile));


        edmob_valueuserprofile = (EditText) findViewById(R.id.edmob_valueuserprofile);
        input_mob_valueuserprofile = (TextInputLayout) findViewById(R.id.input_mob_valueuserprofile);
        edmob_valueuserprofile.addTextChangedListener(new EditTextWatcher(edmob_valueuserprofile));

        edemail_valueuserprofile = (EditText) findViewById(R.id.edemail_valueuserprofile);
        input_email_valueuserprofile = (TextInputLayout) findViewById(R.id.input_email_valueuserprofile);
        edemail_valueuserprofile.addTextChangedListener(new EditTextWatcher(edemail_valueuserprofile));

        edaadhaar_valueuserprofile = (EditText) findViewById(R.id.edaadhaar_valueuserprofile);
        input_aadhaar_valueuserprofile = (TextInputLayout) findViewById(R.id.input_aadhaar_valueuserprofile);
        edaadhaar_valueuserprofile.addTextChangedListener(new EditTextWatcher(edaadhaar_valueuserprofile));

        edpwd_valueuserprofile = (EditText) findViewById(R.id.edpwd_valueuserprofile);
        input_pwd_valueuserprofile = (TextInputLayout) findViewById(R.id.input_pwd_valueuserprofile);
        edpwd_valueuserprofile.addTextChangedListener(new EditTextWatcher(edpwd_valueuserprofile));


        edcnfpwd_valueuserprofile = (EditText) findViewById(R.id.edcnfpwd_valueuserprofile);
        input_cnfpwd_valueuserprofile = (TextInputLayout) findViewById(R.id.input_cnfpwd_valueuserprofile);
        edcnfpwd_valueuserprofile.addTextChangedListener(new EditTextWatcher(edcnfpwd_valueuserprofile));

        // Floating Action Button
        fab_aadhaar_Profileuserprofile = (FloatingActionButton) findViewById(R.id.fab_aadhaar_Profileuserprofile);
        fab_aadhaar_Profileuserprofile.setVisibility(View.VISIBLE);

        mProgressViewuserprofile = (ProgressBar) findViewById(R.id.ProgressBarProfileInfouserprofile);

        rdgrpgender_valueuserprofile = (RadioGroup) findViewById(R.id.rdgrpgender_valueuserprofile);
        rdgrpgenderMale = (RadioButton) findViewById(R.id.rdgrpgenderMale);
        rdgrpgenderFemale = (RadioButton) findViewById(R.id.rdgrpgenderFemale);
        rdgrpgenderOthers = (RadioButton) findViewById(R.id.rdgrpgenderOthers);

        ed_valueuserprofileOTP = (EditText) findViewById(R.id.ed_valueuserprofileOTP);
        input_ed_valueuserprofileOTP = (TextInputLayout) findViewById(R.id.input_ed_valueuserprofileOTP);

        // Binding using LinkedhashMapAdapter
        spstateuserprofile = (Spinner) findViewById(R.id.spstateuserprofile);
        state_adapter = new LinkedHashMapAdapter<String, String>(PHRMS_UserRegisteration.this, android.R.layout.simple_spinner_dropdown_item, Functions.StateData_LinkHasMap());
        state_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spstateuserprofile.setAdapter(state_adapter);

        txt_valueuserprofileOTP = (TextView) findViewById(R.id.txt_valueuserprofileOTP);

        fab_aadhaar_Profileuserprofile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                AlertDialog.Builder builder = new AlertDialog.Builder(PHRMS_UserRegisteration.this);
                builder.setTitle("Scan Aadhaar Data");
                builder.setMessage(getString(R.string.aadhaar_beforeScanuserprofile)).setPositiveButton("Yes", dialogClickListenerForScan)
                        .setNegativeButton("No", dialogClickListenerForScan).show();
            }
        });

        registerOTP_button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (Functions.isNetworkAvailable(getApplicationContext())) {
                    if (!Functions.isNullOrEmpty(ParseJson_UserProfileRegData.REGGUID)) {


                        SendProfileInfoDataWithOTP();

                    } else {
                        Functions.showToast(getApplicationContext(), "User details not created");
                    }
                } else {
                    Functions.showToast(getApplicationContext(), Functions.IE_NotAvailable);
                }


            }
        });

        OTPResend_button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (Functions.isNetworkAvailable(getApplicationContext())) {
                    if (!Functions.isNullOrEmpty(ParseJson_UserProfileRegData.REGGUID)) {
                        ResendOTP();
                    } else {
                        Functions.showToast(getApplicationContext(), "User details not available");
                    }
                } else {
                    Functions.showToast(getApplicationContext(), Functions.IE_NotAvailable);
                }


            }
        });


    }

    DialogInterface.OnClickListener dialogClickListenerForScan = new DialogInterface.OnClickListener() {
        @Override
        public void onClick(DialogInterface dialog, int which) {
            switch (which) {
                case DialogInterface.BUTTON_POSITIVE:
                    //Yes button clicked
                    Intent intent = new Intent(PHRMS_UserRegisteration.this, BarcodeCaptureActivity.class);

                    intent.putExtra(BarcodeCaptureActivity.AutoFocus, true); // autoFocus.isChecked()
                    intent.putExtra(BarcodeCaptureActivity.UseFlash, false); // useFlash.isChecked()
                    startActivityForResult(intent, RC_BARCODE_CAPTURE_USERPROFILE);

                    break;

                case DialogInterface.BUTTON_NEGATIVE:
                    //No button clicked
                    Functions.showToast(PHRMS_UserRegisteration.this, "Aadhaar Scan Cancelled");
                    break;
            }
        }
    };


    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == RC_BARCODE_CAPTURE_USERPROFILE) {
            if (resultCode == CommonStatusCodes.SUCCESS) {
                if (data != null) {
                    Barcode barcode = data.getParcelableExtra(BarcodeCaptureActivity.BarcodeObject);

                    // aadhaar

                    try {
                        XmlPullParserFactory factory = XmlPullParserFactory.newInstance();
                        factory.setNamespaceAware(true);
                        XmlPullParser xpp = factory.newPullParser();
                        xpp.setFeature(XmlPullParser.FEATURE_PROCESS_NAMESPACES, false);

                        if (barcode.displayValue.contains("/?xml")) {
                            barcode.displayValue = barcode.displayValue.replace("/?xml", "?xml");
                        }

                        xpp.setInput(new StringReader(barcode.displayValue)); // String From barcode

                        final ArrayList<AADhaar_Profile> aaDhaar_profiles = parseAadhaarXML(xpp);

                        if (!aaDhaar_profiles.isEmpty() && aaDhaar_profiles.size() > 0) {
                            AlertDialog.Builder builder = new AlertDialog.Builder(PHRMS_UserRegisteration.this);
                            int aadhaar_message = -1;

                            for (AADhaar_Profile aadhaar : aaDhaar_profiles) {
                                String aadhaarname = aadhaar.getName();
                                int aadhaarnamesize = aadhaar.getName().split(" ").length;

                                String firstName = "";
                                if (aadhaarnamesize == 1) {
                                    firstName = aadhaar.getName().toString(); // to get the first name
                                } else {
                                    int start = aadhaarname.indexOf(' ');
                                    if (start >= 0) {
                                        firstName = aadhaarname.substring(0, start);
                                    }
                                }

                                if (!Functions.isNullOrEmpty(edaadhaar_valueuserprofile.getText().toString()) || !Functions.isNullOrEmpty(edfn_valueuserprofile.getText().toString())) {
                                    Boolean uidmatched = false;
                                    Boolean namematched = false;

                                    if (!Functions.isNullOrEmpty(aadhaar.getUID().toString()) && edaadhaar_valueuserprofile.getText().toString().equals(aadhaar.getUID().toString())) {
                                        uidmatched = true;
                                    }

                                    if (!Functions.isNullOrEmpty(firstName) && edfn_valueuserprofile.getText().toString().equals(firstName)) {
                                        namematched = true;
                                    }

                                    if (uidmatched == true && namematched == true) {
                                        aadhaar_message = 1; // both matched
                                    } else if (uidmatched == true && namematched == false) {
                                        aadhaar_message = 2; // uid only matched
                                    } else if (uidmatched == false && namematched == true) {
                                        aadhaar_message = 3; // name only matched
                                    } else {
                                        aadhaar_message = 4; // nothing matched
                                    }

                                } else {
                                    aadhaar_message = 5;
                                }

                            }


                            if (aadhaar_message != -1) {
                                builder.setTitle("Confirm Aadhaar Data");

                                switch (aadhaar_message) {
                                    case 1:
                                        // both matched
                                        builder.setMessage(getString(R.string.aadhaar_matched));
                                        break;
                                    case 2:
                                        // uid only matched
                                        builder.setMessage(getString(R.string.aadhaar_matchedUID));
                                        break;
                                    case 3:
                                        // name only matched
                                        builder.setMessage(getString(R.string.aadhaar_matchedName));
                                        break;
                                    case 4:
                                        // nothing matched
                                        builder.setMessage(getString(R.string.aadhaar_nothingmatched));
                                        break;
                                    case 5:
                                        // nothing matched
                                        builder.setMessage(getString(R.string.aadhaar_nothingexistuserprofile));
                                        break;
                                    default:
                                        builder.setMessage(getString(R.string.aadhaar_nothingmatched));
                                        break;
                                }


                                builder.setPositiveButton("YES", new DialogInterface.OnClickListener() {

                                    public void onClick(DialogInterface dialog, int which) {
                                        // Do nothing, but close the dialog
                                        dialog.dismiss();
                                        LoadAadhaarData(aaDhaar_profiles);
                                    }
                                });

                                builder.setNegativeButton("NO", new DialogInterface.OnClickListener() {

                                    @Override
                                    public void onClick(DialogInterface dialog, int which) {

                                        // Do nothing
                                        dialog.dismiss();
                                        Functions.showToast(PHRMS_UserRegisteration.this, "Aadhaar data cancelled.");
                                    }
                                });

                                AlertDialog alert = builder.create();
                                alert.show();


                            } else {
                                Functions.showToast(PHRMS_UserRegisteration.this, "Unable to Scan QR code");
                            }
                        } else {
                            Functions.showToast(PHRMS_UserRegisteration.this, "Scanned QR code is not related to aadhar.");
                        }
                    } catch (XmlPullParserException e) {
                        //e.printStackTrace();
                        Functions.showToast(PHRMS_UserRegisteration.this, "Scanned QR is Not valid for Aadhaar");
                    } catch (IOException e) {
                        // TODO Auto-generated catch block
                        //e.printStackTrace();
                        Functions.showToast(PHRMS_UserRegisteration.this, "Scanned QR Invalid for Aadhaar");
                    }

                } else {
                    //statusMessage.setText(R.string.barcode_failure);
                    Functions.showToast(PHRMS_UserRegisteration.this, getString(R.string.barcode_failure));
                    Log.d(TAG, "No barcode captured, intent data is null");
                }
            } else {
                Functions.showToast(PHRMS_UserRegisteration.this, String.format(getString(R.string.barcode_error), CommonStatusCodes.getStatusCodeString(resultCode)));
            }
        } else {
            super.onActivityResult(requestCode, resultCode, data);
        }
    }


    private ArrayList<AADhaar_Profile> parseAadhaarXML(XmlPullParser parser) throws XmlPullParserException, IOException {
        ArrayList<AADhaar_Profile> aadhaar_arList = null;
        int eventType = parser.getEventType();
        AADhaar_Profile aadhaar = null;

        while (eventType != XmlPullParser.END_DOCUMENT) {
            String name = parser.getName();
            switch (eventType) {
                case XmlPullParser.START_DOCUMENT:
                    aadhaar_arList = new ArrayList();
                    break;
                case XmlPullParser.START_TAG:
                    break;
                case XmlPullParser.END_TAG:
                    if (name.equals("PrintLetterBarcodeData")) {
                        aadhaar = new AADhaar_Profile();
                        aadhaar.uid = Long.parseLong(parser.getAttributeValue(null, "uid"));
                        aadhaar.name = parser.getAttributeValue(null, "name");
                        aadhaar.gender = parser.getAttributeValue(null, "gender").charAt(0); // Single character
                        aadhaar.yob = Integer.parseInt(parser.getAttributeValue(null, "yob")); // year of birth
                        aadhaar.co = parser.getAttributeValue(null, "co"); // Care off
                        aadhaar.house = parser.getAttributeValue(null, "house"); // House address
                        aadhaar.lm = parser.getAttributeValue(null, "lm");
                        aadhaar.street = parser.getAttributeValue(null, "street"); // Street
                        aadhaar.loc = parser.getAttributeValue(null, "loc"); // Location
                        aadhaar.vtc = parser.getAttributeValue(null, "vtc"); //
                        aadhaar.po = parser.getAttributeValue(null, "po"); // Post Office
                        aadhaar.dist = parser.getAttributeValue(null, "dist"); // District
                        aadhaar.state = parser.getAttributeValue(null, "state"); // State
                        aadhaar.pc = Long.parseLong(parser.getAttributeValue(null, "pc")); //Postal code - Pin
                        aadhaar.dob = parser.getAttributeValue(null, "dob");

                        aadhaar_arList.add(aadhaar);
                    }
                    break;
                case XmlPullParser.TEXT:
                    break;
            }
            eventType = parser.next();
        }

        return aadhaar_arList;
    }

    private void LoadAadhaarData(ArrayList<AADhaar_Profile> aaDhaar_profiles_data) {


        if (!aaDhaar_profiles_data.isEmpty() && aaDhaar_profiles_data.size() == 1) {
            for (AADhaar_Profile aadhar_data : aaDhaar_profiles_data) {


                if (!Functions.isNullOrEmpty(aadhar_data.getName())) {
                    String aadhaarname = aadhar_data.getName();

                    int aadhaarnamesize = aadhar_data.getName().split(" ").length;

                    if (aadhaarnamesize == 1) {
                        edfn_valueuserprofile.setText(Functions.isNullOrEmpty(aadhar_data.getName()) ? "" : aadhar_data.getName());
                    } else {
                        int start = aadhaarname.indexOf(' ');

                        // Last Index
                        int end = aadhaarname.lastIndexOf(' ');

                        String firstName = "";
                        String middleName = "";
                        String lastName = "";

                        if (start >= 0) {
                            firstName = aadhaarname.substring(0, start);
                            if (end > start)
                                middleName = aadhaarname.substring(start + 1, end);
                            lastName = aadhaarname.substring(end + 1, aadhaarname.length());
                        }

                        edfn_valueuserprofile.setText(Functions.isNullOrEmpty(firstName + middleName) ? "" : firstName + middleName);

                        edln_valueuserprofile.setText(Functions.isNullOrEmpty(lastName) ? "" : lastName);
                    }
                }

                //"Uhid": "",
                if (!Functions.isNullOrEmpty(aadhar_data.getUID().toString())) {
                    edaadhaar_valueuserprofile.setText(aadhar_data.getUID().toString());
                }

                //"Gender": "M",
                //"strGender": null,
                if (!Functions.isNullOrEmpty(aadhar_data.getGender().toString().toUpperCase())) {

                    if (aadhar_data.getGender().toString().toUpperCase().equals("M")) {
                        rdgrpgender_valueuserprofile.check(R.id.rdgrpgenderMale);
                    } else if (aadhar_data.getGender().toString().toUpperCase().equals("F")) {
                        rdgrpgender_valueuserprofile.check(R.id.rdgrpgenderFemale);
                    } else {
                        rdgrpgender_valueuserprofile.check(R.id.rdgrpgenderOthers);
                    }

                }

                //"District": ""
                if (!Functions.isNullOrEmpty(aadhar_data.getDistrict())) {

                }

                String address_Details = null;

                if (!Functions.isNullOrEmpty(aadhar_data.getCareoff())) {
                    address_Details = aadhar_data.getCareoff() + ",";
                }

                if (!Functions.isNullOrEmpty(aadhar_data.getHouse())) {
                    if (!Functions.isNullOrEmpty(address_Details)) {
                        address_Details += aadhar_data.getHouse() + ",";
                    } else {
                        address_Details = aadhar_data.getHouse();
                    }
                }

                if (!Functions.isNullOrEmpty(aadhar_data.getLoc())) {
                    if (!Functions.isNullOrEmpty(address_Details)) {
                        address_Details += aadhar_data.getLoc() + ",";
                    } else {
                        address_Details = aadhar_data.getLoc();
                    }
                }

                if (!Functions.isNullOrEmpty(aadhar_data.getStreet())) {
                    if (!Functions.isNullOrEmpty(address_Details)) {
                        address_Details += aadhar_data.getStreet() + ",";
                    } else {
                        address_Details = aadhar_data.getStreet();
                    }
                }

                if (!Functions.isNullOrEmpty(address_Details)) {
                    if (address_Details.endsWith(",")) {
                        address_Details = address_Details.substring(0, address_Details.length() - 1);
                    }

                }

                //"City_Vill_Town": "not available",
                if (!Functions.isNullOrEmpty(aadhar_data.getVTC())) {

                }

                //"State": 0,

                if (!Functions.isNullOrEmpty(aadhar_data.getState())) {

                    List<String> indexes_values = new ArrayList<String>(Functions.StateData_LinkHasMap().values());
                    spstateuserprofile.setSelection(indexes_values.indexOf(aadhar_data.getState().toString()));
                }

                //"Pin": "",
                if (!Functions.isNullOrEmpty(aadhar_data.getPin().toString())) {

                }
            }

            Functions.showToast(PHRMS_UserRegisteration.this, "Aaadhaar Data Mapped Successfully");
        } else {
            Functions.showToast(PHRMS_UserRegisteration.this, "Aaadhaar Data Not Mapped");
        }
    }

    public void SendProfileInfoDataUserProfile() {
        //&& validateUhid() == true

        if (validateFirstName() == true && validateLastName() == true && ValidateGender() == true && validatePhoneNumber() == true && Validate_email() == true && Validate_Password() == true && Validate_ConfirmPassword() == true && validateUhid() == true && ValidateState() == true) {


            Functions.showProgress(true, mProgressViewuserprofile);


            Map<String, String> jsonParams = new HashMap<String, String>();
            jsonParams.put("FirstName", edfn_valueuserprofile.getText().toString());
            jsonParams.put("LastName", edln_valueuserprofile.getText().toString());

            int selectedId = rdgrpgender_valueuserprofile.getCheckedRadioButtonId();
            if (selectedId == rdgrpgenderMale.getId()) {
                jsonParams.put("Gender", "M");
            } else if (selectedId == rdgrpgenderFemale.getId()) {
                jsonParams.put("Gender", "F");
            } else {
                jsonParams.put("Gender", "U");
            }

            jsonParams.put("MobileNo", edmob_valueuserprofile.getText().toString());

            jsonParams.put("Email", edemail_valueuserprofile.getText().toString());

            String secure_pass = get_SecurePassword(edpwd_valueuserprofile.getText().toString().trim());
            jsonParams.put("Password", secure_pass);


            jsonParams.put("AadhaarNo", edaadhaar_valueuserprofile.getText().toString());

            Map.Entry<String, String> spstateuserprofile_item = (Map.Entry<String, String>) spstateuserprofile.getSelectedItem();
            jsonParams.put("State", spstateuserprofile_item.getKey().toString());


            JsonObjectRequest postRequestProfile = new JsonObjectRequest(Request.Method.POST, getString(R.string.urlLogin) + getString(R.string.RegUserProfile),
                    new JSONObject(jsonParams),
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            AfterPostProfile(response);
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            Functions.showProgress(false, mProgressViewuserprofile);
                            Functions.ErrorHandling(getApplicationContext(), error);
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

            postRequestProfile.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

            // Access the RequestQueue through your singleton class.
            MySingleton.getInstance(PHRMS_UserRegisteration.this).addToRequestQueue(postRequestProfile);


        } else {
            return;
        }
    }


    public String get_SecurePassword(String passwordToHash) {

        return "";
    }

    private void AfterPostProfile(JSONObject response) {

        ParseJson_UserProfileRegData profileuserInfo_pj = new ParseJson_UserProfileRegData(response);
        String STATUS_Post = profileuserInfo_pj.parsePostResponseUserProfileReg();

        switch (STATUS_Post) {
            case "0":
                Functions.showToast(PHRMS_UserRegisteration.this, "Registeration not done");
                break;
            case "1": //Success
                //Show OTP Screen
                if (!Functions.isNullOrEmpty(profileuserInfo_pj.REGGUID.toString())) {

                    MenuItem item = menu_save.findItem(R.id.savedata);
                    item.setVisible(false);

                    fab_aadhaar_Profileuserprofile.setVisibility(View.GONE);
                    lv_profileuserprofileRegForm.setVisibility(View.GONE);
                    lv_profileuserprofileOTPForm.setVisibility(View.VISIBLE);
                    String mobilelast4digit = edmob_valueuserprofile.getText().toString().substring(edmob_valueuserprofile.length() - 4);
                    txt_valueuserprofileOTP.setText("Enter Verification code recieved on registered emailid and mobile XXXXXX" + mobilelast4digit);
                } else {
                    Functions.showToast(PHRMS_UserRegisteration.this, "User details not created");
                }
                break;
            case "2":
                Functions.showToast(PHRMS_UserRegisteration.this, "Mobile number already exist");
                break;
            case "3":
                Functions.showToast(PHRMS_UserRegisteration.this, "Email ID already exist");
                break;
            case "-1":
                Functions.showToast(PHRMS_UserRegisteration.this, "Invalid response");
                break;
            case "-2":
                Functions.showToast(PHRMS_UserRegisteration.this, "Server response empty");
                break;
            case "-3":
                Functions.showToast(PHRMS_UserRegisteration.this, "Service doesn't sent any response");
                break;
            case "-4":
                Functions.showToast(PHRMS_UserRegisteration.this, "Parsing Error");
                break;
            default:
                Functions.showToast(PHRMS_UserRegisteration.this, "No response");
                break;
        }

        Functions.showProgress(false, mProgressViewuserprofile);
    }

    // Required
    protected boolean validateFirstName() {
        boolean bool_firstname = true;

        if (edfn_valueuserprofile.getText().toString().trim().isEmpty()) {
            input_edfn_valueuserprofileuserprofile.setErrorEnabled(true);
            input_edfn_valueuserprofileuserprofile.setError(getString(R.string.errfirstname));
            requestFocus(edfn_valueuserprofile);
            bool_firstname = false;
        } else {
            input_edfn_valueuserprofileuserprofile.setError(null);
            input_edfn_valueuserprofileuserprofile.setErrorEnabled(false);
        }

        return bool_firstname;
    }

    // Required
    protected boolean validateLastName() {


        boolean bool_lastname = true;

        if (edln_valueuserprofile.getText().toString().trim().isEmpty()) {
            input_edln_valueuserprofileuserprofile.setErrorEnabled(true);
            input_edln_valueuserprofileuserprofile.setError(getString(R.string.errlastname));
            requestFocus(edln_valueuserprofile);
            bool_lastname = false;
        } else {

            input_edln_valueuserprofileuserprofile.setError(null);
            input_edln_valueuserprofileuserprofile.setErrorEnabled(false);
        }
        return bool_lastname;
    }

    protected boolean ValidateGender() {
        boolean genderbool = false;

        int selectedId = rdgrpgender_valueuserprofile.getCheckedRadioButtonId();

        if (selectedId == rdgrpgenderMale.getId()) {
            genderbool = true;
        } else if (selectedId == rdgrpgenderFemale.getId()) {
            genderbool = true;
        } else if (selectedId == rdgrpgenderOthers.getId()) {
            genderbool = true;
        } else {
            Functions.showToast(getApplicationContext(), "Gender Field is required, Please select");
        }

        return genderbool;
    }

    // Not Required - but if specified must be integer upto 12 characters
    protected boolean validateUhid() {

        //empty
        Boolean bool_Uhid = true;

        if (!edaadhaar_valueuserprofile.getText().toString().trim().isEmpty()) {
            try {
                double d = Double.valueOf(edaadhaar_valueuserprofile.getText().toString().trim());


                if (d == (long) d) {

                    if (edaadhaar_valueuserprofile.getText().toString().trim().length() == 12) {
                        input_aadhaar_valueuserprofile.setError(null);
                        input_aadhaar_valueuserprofile.setErrorEnabled(false);

                    } else {
                        input_aadhaar_valueuserprofile.setErrorEnabled(true);
                        input_aadhaar_valueuserprofile.setError(getString(R.string.erraadharlenght));
                        requestFocus(edaadhaar_valueuserprofile);
                        bool_Uhid = false;
                    }
                } else {

                    input_aadhaar_valueuserprofile.setErrorEnabled(true);
                    input_aadhaar_valueuserprofile.setError(getString(R.string.erraadharint));
                    requestFocus(edaadhaar_valueuserprofile);
                    bool_Uhid = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_aadhaar_valueuserprofile.setErrorEnabled(true);
                input_aadhaar_valueuserprofile.setError(getString(R.string.erraadharint));
                requestFocus(edaadhaar_valueuserprofile);
                bool_Uhid = false;
            }
        } else {
            input_aadhaar_valueuserprofile.setError(null);
            input_aadhaar_valueuserprofile.setErrorEnabled(false);
        }

        return bool_Uhid;
    }


    // Not Required Case
    protected boolean validatePhoneNumber() {


        boolean bool_phone = true;
        if (!Functions.isNullOrEmpty(edmob_valueuserprofile.getText().toString().trim())) {

            try {
                double d = Double.valueOf(edmob_valueuserprofile.getText().toString().trim());
                if (d == (long) d) {
                    if (edmob_valueuserprofile.getText().toString().trim().length() == 10) {
                        //System.out.println("integer"+(int)d);
                        input_mob_valueuserprofile.setError(null);
                        input_mob_valueuserprofile.setErrorEnabled(false);
                        //return true;
                    } else {
                        input_mob_valueuserprofile.setErrorEnabled(true);
                        input_mob_valueuserprofile.setError(getString(R.string.errphonelenghtuserprofile));
                        requestFocus(edmob_valueuserprofile);
                        bool_phone = false;
                    }
                } else {
                    //System.out.println("double"+d);
                    input_mob_valueuserprofile.setErrorEnabled(true);
                    input_mob_valueuserprofile.setError(getString(R.string.errphoneintuserprofile));
                    requestFocus(edmob_valueuserprofile);
                    bool_phone = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_mob_valueuserprofile.setErrorEnabled(true);
                input_mob_valueuserprofile.setError(getString(R.string.errphoneintuserprofile));
                requestFocus(edmob_valueuserprofile);
                bool_phone = false;
            }
        } else {
            input_mob_valueuserprofile.setErrorEnabled(true);
            input_mob_valueuserprofile.setError(getString(R.string.errphoneuserprofile));
            requestFocus(edmob_valueuserprofile);
            bool_phone = false;
        }

        return bool_phone;
    }

    private boolean Validate_email() {

        boolean emailcancel = true;
        // Check for a valid email locaddr.
        if (TextUtils.isEmpty(edemail_valueuserprofile.getText().toString().trim())) {
            input_email_valueuserprofile.setError(getString(R.string.error_email_field_requireduserprofile));
            emailcancel = false;
            requestFocus(edemail_valueuserprofile);
        } else if (!isEmailValid(edemail_valueuserprofile.getText().toString().trim())) {
            input_email_valueuserprofile.setError(getString(R.string.error_invalid_emailuserprofile));
            emailcancel = false;
            requestFocus(edemail_valueuserprofile);
        } else {
            input_email_valueuserprofile.setError(null);
            input_email_valueuserprofile.setErrorEnabled(false);
        }


        return emailcancel;
    }

    // will return true if no validation
    private boolean isEmailValid(String email) {
        //TODO: Replace this with your own logic
        boolean rt_value = false;

        if (email.contains("@") && !TextUtils.isEmpty(email) && Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            rt_value = true;
        } else if (!TextUtils.isEmpty(email)) {
            try {
                double d = Double.valueOf(email.toString().trim());
                if (d == (long) d) {
                    if (email.toString().trim().length() == 10) {
                        rt_value = true;
                    }
                }
            } catch (Exception e) {
                rt_value = false;
            }
        }

        return rt_value;
    }

    private boolean Validate_Password() {

        boolean epwdcancel = true;

        // Check for a valid email locaddr.
        if (TextUtils.isEmpty(edpwd_valueuserprofile.getText().toString().trim())) {
            input_pwd_valueuserprofile.setError(getString(R.string.error_pwdfield_requireduserprofile));
            epwdcancel = false;
            requestFocus(edpwd_valueuserprofile);
        } else if ((edpwd_valueuserprofile.getText().toString().trim().length() < 6)) {
            input_pwd_valueuserprofile.setError("Password must be 6 characters long");
            epwdcancel = false;
            requestFocus(edpwd_valueuserprofile);
        } else {
            input_pwd_valueuserprofile.setError(null);
            input_pwd_valueuserprofile.setErrorEnabled(false);
        }

        return epwdcancel;
    }

    private boolean Validate_ConfirmPassword() {
        boolean ecnfpwdcancel = true;

        // Check for a valid email locaddr.
        if (TextUtils.isEmpty(edcnfpwd_valueuserprofile.getText().toString().trim())) {
            input_cnfpwd_valueuserprofile.setError(getString(R.string.error_cnfpwdfield_requireduserprofile));
            ecnfpwdcancel = false;
            requestFocus(edcnfpwd_valueuserprofile);
        } else if ((edcnfpwd_valueuserprofile.getText().toString().trim().length() < 6)) {
            input_cnfpwd_valueuserprofile.setError("Confirm Password must be 6 characters long");
            ecnfpwdcancel = false;
            requestFocus(edcnfpwd_valueuserprofile);
        } else if (!edcnfpwd_valueuserprofile.getText().toString().equals(edpwd_valueuserprofile.getText().toString())) {
            input_cnfpwd_valueuserprofile.setError(getString(R.string.error_cnfpwdnotmatchfield_requireduserprofile));
            ecnfpwdcancel = false;
            requestFocus(edcnfpwd_valueuserprofile);
        } else {
            input_cnfpwd_valueuserprofile.setError(null);
            input_cnfpwd_valueuserprofile.setErrorEnabled(false);
        }

        return ecnfpwdcancel;
    }

    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    protected boolean ValidateState() {
        Boolean statebool = false;

        Map.Entry<String, String> spstate_item = (Map.Entry<String, String>) spstateuserprofile.getSelectedItem();
        String stateid = spstate_item.getKey(); //.toString(); // SelectedItemId();

        if (!TextUtils.isEmpty(stateid.trim())) {
            if (stateid.equals("0")) {
                Functions.showToast(getApplicationContext(), "State is required");
            } else {
                statebool = true;
            }
        } else {
            Functions.showToast(getApplicationContext(), "State is required");
        }

        return statebool;
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
                case R.id.edfn_valueuserprofile:
                    // Required check for empty also
                    validateFirstName();
                    break;
                case R.id.edln_valueuserprofile:
                    // Required check for empty also
                    validateLastName();
                    break;
                //Gender - radio
                case R.id.edmob_valueuserprofile:
                    validatePhoneNumber();
                    break;
                case R.id.edemail_valueuserprofile:
                    Validate_email();
                    break;
                case R.id.edpwd_valueuserprofile:
                    Validate_Password();
                    break;
                case R.id.edcnfpwd_valueuserprofile:
                    Validate_ConfirmPassword();
                    break;
                case R.id.edaadhaar_valueuserprofile:
                    // Not required - but if filled must be 12 character numeric
                    validateUhid();
                    break;
                //State - spinner
            }
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        getMenuInflater().inflate(R.menu.savebuttonmenu, menu);

        menu_save = menu;

        // find save button and set false;
        MenuItem item = menu.findItem(R.id.savedata);

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
                if (Functions.isNetworkAvailable(getApplicationContext())) {
                    SendProfileInfoDataUserProfile();
                } else {
                    Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
                }
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    // Required
    protected boolean ValidateOTP() {
        boolean bool_OTP = true;

        if (ed_valueuserprofileOTP.getText().toString().trim().isEmpty()) {

            input_ed_valueuserprofileOTP.setErrorEnabled(true);
            input_ed_valueuserprofileOTP.setError("OTP is Required");
            requestFocus(ed_valueuserprofileOTP);
            bool_OTP = false;
        } else {
            input_ed_valueuserprofileOTP.setError(null);
            input_ed_valueuserprofileOTP.setErrorEnabled(false);
        }
        return bool_OTP;
    }

    protected boolean ValidateREGGUID() {
        boolean bool_REGGUID = true;

        if (Functions.isNullOrEmpty(ParseJson_UserProfileRegData.REGGUID.toString())) {
            bool_REGGUID = false;
        }

        return bool_REGGUID;
    }


    public void SendProfileInfoDataWithOTP() {
        if (ValidateOTP() == true && ValidateREGGUID() == true) {
            Functions.showProgress(true, mProgressViewuserprofile);

            Map<String, String> jsonParams_OTP = new HashMap<String, String>();

            jsonParams_OTP.put("Id", ParseJson_UserProfileRegData.REGGUID);
            jsonParams_OTP.put("OTP", ed_valueuserprofileOTP.getText().toString());

            JsonObjectRequest postRequestProfile_OTP = new JsonObjectRequest(Request.Method.POST, getString(R.string.urlLogin) + getString(R.string.RegUserProfileVerifyOTP),
                    new JSONObject(jsonParams_OTP),
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            AfterPostProfileOTPVerification(response);
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            Functions.showProgress(false, mProgressViewuserprofile);
                            Functions.ErrorHandling(getApplicationContext(), error);
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

            postRequestProfile_OTP.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

            // Access the RequestQueue through your singleton class.
            MySingleton.getInstance(PHRMS_UserRegisteration.this).addToRequestQueue(postRequestProfile_OTP);
        } else {
            return;
        }

    }


    private void AfterPostProfileOTPVerification(JSONObject response) {


        ParseJson_UserProfileRegData profileInfo_pj = new ParseJson_UserProfileRegData(response);
        String STATUS_Post = profileInfo_pj.parsePostResponseUserProfileRegOTPVerification();

        switch (STATUS_Post) {
            case "0":
                Functions.showToast(getApplication(), "Initial");
                break;
            case "1":
                Functions.showToast(getApplication(), "OTP Verification");
                break;
            case "2": //Success
                Intent Login = new Intent(getApplicationContext(), PHRMS_LoginActivity.class);
                Login.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                Functions.showToast(getApplication(), "User Registered Successfully");
                finish();
                startActivity(Login);
                break;
            case "-1": //ProcessFailure
                Functions.showToast(getApplication(), "Process Failure");
                break;
            case "-2":
                Functions.showToast(getApplication(), "OTP Mismatch");
                break;
            case "-11":
                Functions.showToast(getApplication(), "Invalid response");
                break;
            case "-12":
                Functions.showToast(getApplication(), "No response");
                break;
            case "-13":
                Functions.showToast(getApplication(), "Service doesn't sent any response");
                break;
            case "-14":
                Functions.showToast(getApplication(), "Parsing Failed");
                break;
            default:
                Functions.showToast(getApplication(), "Invalid Response");
                break;
        }

        Functions.showProgress(false, mProgressViewuserprofile);
    }

    /////==========

    public void ResendOTP() {
        if (ValidateREGGUID() == true) {
            Functions.showProgress(true, mProgressViewuserprofile);

            Map<String, String> jsonParams_OTP_Resend = new HashMap<String, String>();

            jsonParams_OTP_Resend.put("Id", ParseJson_UserProfileRegData.REGGUID);

            JsonObjectRequest postRequestProfile_OTP_Resend = new JsonObjectRequest(Request.Method.POST, getString(R.string.urlLogin) + getString(R.string.RegUserOTPResend),
                    new JSONObject(jsonParams_OTP_Resend),
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            AfterPostOTPResendVerification(response);
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            Functions.showProgress(false, mProgressViewuserprofile);
                            Functions.ErrorHandling(getApplicationContext(), error);
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

            postRequestProfile_OTP_Resend.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

            // Access the RequestQueue through your singleton class.
            MySingleton.getInstance(PHRMS_UserRegisteration.this).addToRequestQueue(postRequestProfile_OTP_Resend);
        } else {
            return;
        }

    }


    private void AfterPostOTPResendVerification(JSONObject response) {

        ParseJson_UserProfileRegData profileInfo_pj = new ParseJson_UserProfileRegData(response);
        String STATUS_Post_Resend = profileInfo_pj.parsePostResponseUserProfileResendOTP();

        switch (STATUS_Post_Resend) {
            case "0":
                Functions.showToast(getApplication(), "Resend Failed");
                break;
            case "1":
                Functions.showToast(getApplication(), "OTP Resent Successfully");
                break;
            default:
                Functions.showToast(getApplication(), "Invalid Response");
                break;
        }

        Functions.showProgress(false, mProgressViewuserprofile);
    }


}
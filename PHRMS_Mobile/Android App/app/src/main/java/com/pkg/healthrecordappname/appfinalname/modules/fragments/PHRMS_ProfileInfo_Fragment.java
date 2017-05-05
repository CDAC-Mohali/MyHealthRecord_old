package com.pkg.healthrecordappname.appfinalname.modules.fragments;


import android.app.AlertDialog;
import android.app.DialogFragment;
import android.app.Fragment;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.TextInputLayout;
import android.support.v4.widget.SwipeRefreshLayout;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.RelativeLayout;
import android.widget.Spinner;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.google.android.gms.common.api.CommonStatusCodes;
import com.google.android.gms.vision.barcode.Barcode;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.barcode.BarcodeCaptureActivity;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DatePickerFragment;
import com.pkg.healthrecordappname.appfinalname.modules.datetimefragments.DateValidator;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_ProfileInfoData;
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
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;


public class PHRMS_ProfileInfo_Fragment extends Fragment {
    String url = null;
    private ProgressBar mProgressView;
    private SwipeRefreshLayout mSwipeRefreshLayout;
    private EditText edfn_value;
    private EditText edln_value;
    private EditText edemail_value;
    private EditText edaadhaar_value;
    private EditText eddob_value;
    private Spinner spgender_value;
    private Spinner spbg_value;
    private EditText eddistrict_value;
    private EditText edAdrline1_value;
    private EditText edAdrline2_value;
    private EditText edcity_value;
    private Spinner spstate;
    private EditText edphne_value;
    private EditText edpin_value;
    private RadioGroup rdDiff_Group;
    private RadioButton radioYes;
    private RadioButton radioNo;
    private Spinner spdiffableType;
    private EditText edmob_value;
    private FloatingActionButton fab_edit;
    private FloatingActionButton fab_save;
    private FloatingActionButton fab_Cancel_Prfoile;

    private FloatingActionButton fab_aadhaar_Profile;

    private String userid = "-1";

    private LinkedHashMapAdapter<String, String> state_adapter;
    private LinkedHashMapAdapter<String, String> bg_adapter;
    private LinkedHashMapAdapter<String, String> gender_adapter;
    private LinkedHashMapAdapter<String, String> diffableType_adapter;

    private LinearLayout lvdiffableType;
    private LinearLayout lv_profile;
    private RelativeLayout rl_fab_Profile;
    private View rootViewProfile;


    TextInputLayout input_edfn_value;


    TextInputLayout input_edln_value;


    TextInputLayout input_aadhaar_value;

    TextInputLayout input_dob_value;


    TextInputLayout input_email_value;


    TextInputLayout input_phne_value;

    TextInputLayout inputTXTPin;



    private static final int RC_BARCODE_CAPTURE = 9001;
    private static final String TAG = "AadharBarcodeMain";


    public PHRMS_ProfileInfo_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        rootViewProfile = inflater.inflate(R.layout.frame_profile, container, false);


        mProgressView = (ProgressBar) rootViewProfile.findViewById(R.id.ProgressBarProfileInfo);

        mSwipeRefreshLayout = (SwipeRefreshLayout) rootViewProfile.findViewById(R.id.profile_swipe_refresh);
        lv_profile = (LinearLayout) rootViewProfile.findViewById(R.id.lv_profile);
        // Add input TextWatcher
        edfn_value = (EditText) rootViewProfile.findViewById(R.id.edfn_value);
        input_edfn_value = (TextInputLayout) rootViewProfile.findViewById(R.id.input_edfn_value);
        // Add input TextWatcher
        edln_value = (EditText) rootViewProfile.findViewById(R.id.edln_value);
        input_edln_value = (TextInputLayout) rootViewProfile.findViewById(R.id.input_edln_value);

        edemail_value = (EditText) rootViewProfile.findViewById(R.id.edemail_value);
        input_email_value = (TextInputLayout) rootViewProfile.findViewById(R.id.input_email_value);

        edemail_value.setEnabled(false);



        edaadhaar_value = (EditText) rootViewProfile.findViewById(R.id.edaadhaar_value);
        input_aadhaar_value = (TextInputLayout) rootViewProfile.findViewById(R.id.input_aadhaar_value);


        eddob_value = (EditText) rootViewProfile.findViewById(R.id.eddob_value);
        input_dob_value = (TextInputLayout) rootViewProfile.findViewById(R.id.input_dob_value);

        spgender_value = (Spinner) rootViewProfile.findViewById(R.id.spgender_value);
        spbg_value = (Spinner) rootViewProfile.findViewById(R.id.spbg_value);

        eddistrict_value = (EditText) rootViewProfile.findViewById(R.id.eddistrict_value);
        edAdrline1_value = (EditText) rootViewProfile.findViewById(R.id.edAdrline1_value);
        edAdrline2_value = (EditText) rootViewProfile.findViewById(R.id.edAdrline2_value);
        edcity_value = (EditText) rootViewProfile.findViewById(R.id.edcity_value);

        edphne_value = (EditText) rootViewProfile.findViewById(R.id.edphne_value);
        input_phne_value = (TextInputLayout) rootViewProfile.findViewById(R.id.input_phne_value);

        edpin_value = (EditText) rootViewProfile.findViewById(R.id.edpin_value);
        inputTXTPin = (TextInputLayout) rootViewProfile.findViewById(R.id.input_pin_value);

        lvdiffableType = (LinearLayout) rootViewProfile.findViewById(R.id.lvdiffableType);
        rl_fab_Profile = (RelativeLayout) rootViewProfile.findViewById(R.id.rl_fab_Profile);

        // Find RadiogRoup
        rdDiff_Group = (RadioGroup) rootViewProfile.findViewById(R.id.rdDiff_Group);
        radioYes = (RadioButton) rootViewProfile.findViewById(R.id.rdYes);
        radioNo = (RadioButton) rootViewProfile.findViewById(R.id.rdNo);

        spdiffableType = (Spinner) rootViewProfile.findViewById(R.id.spdiffableType);

        edmob_value = (EditText) rootViewProfile.findViewById(R.id.edmob_value);
        edmob_value.setEnabled(false);

        // Floating Action Button
        fab_edit = (FloatingActionButton) rootViewProfile.findViewById(R.id.fab_Edit_Prfoile);
        fab_save = (FloatingActionButton) rootViewProfile.findViewById(R.id.fab_Save_Prfoile);
        fab_Cancel_Prfoile = (FloatingActionButton) rootViewProfile.findViewById(R.id.fab_Cancel_Prfoile);
        fab_aadhaar_Profile = (FloatingActionButton) rootViewProfile.findViewById(R.id.fab_aadhaar_Profile);
        fab_aadhaar_Profile.setVisibility(View.GONE);

        Functions.progressbarStyle(mProgressView, getActivity());




        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                Functions.mainscreen(getActivity());
            } else {
                Functions.enableDisableView(lv_profile, false);
                edemail_value.setEnabled(false);
                edmob_value.setEnabled(false);

                edfn_value.addTextChangedListener(new EditTextWatcher(edfn_value));
                edln_value.addTextChangedListener(new EditTextWatcher(edln_value));
                edaadhaar_value.addTextChangedListener(new EditTextWatcher(edaadhaar_value));

                eddob_value.addTextChangedListener(new EditTextWatcher(eddob_value));
                eddob_value.setOnTouchListener(new View.OnTouchListener() {
                    public boolean onTouch(View v, MotionEvent event) {
                        DialogFragment datepicker = DatePickerFragment.newInstance(eddob_value);
                        if (datepicker != null) {
                            datepicker.show(getFragmentManager(), "DatePickerFragment");
                        }
                        return false;
                    }
                });

                // Binding using LinkedhashMapAdapter
                gender_adapter = new LinkedHashMapAdapter<String, String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, Functions.Gender_LinkHasMap());
                gender_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                spgender_value.setAdapter(gender_adapter);

                // Binding using LinkedhashMapAdapter
                bg_adapter = new LinkedHashMapAdapter<String, String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, Functions.BloodGroup_LinkHasMap());
                bg_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                spbg_value.setAdapter(bg_adapter);

                // Binding using LinkedhashMapAdapter
                spstate = (Spinner) rootViewProfile.findViewById(R.id.spstate);
                state_adapter = new LinkedHashMapAdapter<String, String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, Functions.StateData_LinkHasMap());
                state_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                spstate.setAdapter(state_adapter);

                edphne_value.addTextChangedListener(new EditTextWatcher(edphne_value));

                edpin_value.addTextChangedListener(new EditTextWatcher(edpin_value));

                rdDiff_Group.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
                    // @Override
                    public void onCheckedChanged(RadioGroup group, int checkedId) {
                        // // find which radio button is selected
                        if (checkedId == R.id.rdYes) {
                            lvdiffableType.setVisibility(View.VISIBLE);
                        } else {
                            lvdiffableType.setVisibility(View.GONE);
                        }
                    }
                });


                diffableType_adapter = new LinkedHashMapAdapter<String, String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, Functions.DiffAbledType_LinkHasMap());
                diffableType_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                spdiffableType.setAdapter(diffableType_adapter);



                url = getString(R.string.urlLogin) + getString(R.string.LoadPersonalInfo) + Functions.ApplicationUserid;//+ userid;

                if (url != null) {

                    Functions.showProgress(true, mProgressView);
                    LoadProfileInfoData(url);
                }



                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {
                                LoadProfileInfoData(url);
                            } else {
                                Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                            }
                        }
                    }
                });



                fab_edit.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        Functions.enableDisableView(lv_profile, true);
                        edemail_value.setEnabled(false);
                        edmob_value.setEnabled(false);
                        fab_edit.setVisibility(View.GONE);
                        rl_fab_Profile.setVisibility(View.VISIBLE);


                        fab_aadhaar_Profile.setVisibility(View.VISIBLE);

                        Functions.showSnackbar(view, "Edit View - Profile Info", "Action");

                    }
                });

                fab_aadhaar_Profile.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
                        builder.setTitle("Scan Aadhaar Data");
                        builder.setMessage(getString(R.string.aadhaar_beforeScan)).setPositiveButton("Yes", dialogClickListenerForScan)
                                .setNegativeButton("No", dialogClickListenerForScan).show();
                    }
                });


                fab_save.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            // Post data with url
                            SendProfileInfoData(getString(R.string.urlLogin) + getString(R.string.UpdatePersonalInfo));
                        } else {
                            Functions.showSnackbar(view, "Internet Not Available !!", "Action");
                        }
                    }
                });

                fab_Cancel_Prfoile.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {

                        if (url != null)
                        {
                            if (Functions.isNetworkAvailable(getActivity()))
                            {
                                LoadProfileInfoData(url);
                            }
                        }

                        Functions.enableDisableView(lv_profile, false);
                        edemail_value.setEnabled(false);
                        edmob_value.setEnabled(false);
                        rl_fab_Profile.setVisibility(View.GONE);
                        Functions.showSnackbar(view, "Update Request Canceled.", "Action");
                        fab_edit.setVisibility(View.VISIBLE);
                        fab_aadhaar_Profile.setVisibility(View.GONE);
                    }
                });
            }
        } else {
            Functions.enableDisableView(lv_profile, false);
            edemail_value.setEnabled(false);
            edmob_value.setEnabled(false);
            Functions.showSnackbar(rootViewProfile, Functions.IE_NotAvailable, "Action");
        }

        return rootViewProfile;
    }


    DialogInterface.OnClickListener dialogClickListenerForScan = new DialogInterface.OnClickListener() {
        @Override
        public void onClick(DialogInterface dialog, int which) {
            switch (which) {
                case DialogInterface.BUTTON_POSITIVE:
                    //Yes button clicked
                    Intent intent = new Intent(getActivity(), BarcodeCaptureActivity.class);

                    intent.putExtra(BarcodeCaptureActivity.AutoFocus, true); // autoFocus.isChecked()
                    intent.putExtra(BarcodeCaptureActivity.UseFlash, false); // useFlash.isChecked()
                    startActivityForResult(intent, RC_BARCODE_CAPTURE);

                    break;

                case DialogInterface.BUTTON_NEGATIVE:
                    //No button clicked
                    Functions.showToast(getActivity(), "Aadhaar Scan Cancelled");
                    break;
            }
        }
    };


    //    /**
//     * Called when an activity you launched exits, giving you the requestCode
//     * you started it with, the resultCode it returned, and any additional
//     * data from it.  The <var>resultCode</var> will be
//     * {@link #RESULT_CANCELED} if the activity explicitly returned that,
//     * didn't return any result, or crashed during its operation.
//     * <p/>
//     * <p>You will receive this call immediately before onResume() when your
//     * activity is re-starting.
//     * <p/>
//     *
//     * @param requestCode The integer request code originally supplied to
//     *                    startActivityForResult(), allowing you to identify who this
//     *                    result came from.
//     * @param resultCode  The integer result code returned by the child activity
//     *                    through its setResult().
//     * @param data        An Intent, which can return result data to the caller
//     *                    (various data can be attached to Intent "extras").
//     * @see #startActivityForResult
//     * @see #createPendingResult
//     * @see #setResult(int)
//     */
    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data)
    {
        if (requestCode == RC_BARCODE_CAPTURE)
        {
            if (resultCode == CommonStatusCodes.SUCCESS)
            {
                if (data != null)
                {
                    Barcode barcode = data.getParcelableExtra(BarcodeCaptureActivity.BarcodeObject);

                    // aadhaar

                    try {
                        XmlPullParserFactory factory = XmlPullParserFactory.newInstance();
                        factory.setNamespaceAware(true);
                        XmlPullParser xpp = factory.newPullParser();
                        xpp.setFeature(XmlPullParser.FEATURE_PROCESS_NAMESPACES, false);

                        if(barcode.displayValue.contains("/?xml"))
                        {
                            barcode.displayValue = barcode.displayValue.replace("/?xml","?xml");
                        }

                        xpp.setInput(new StringReader(barcode.displayValue)); // String From barcode

                        final ArrayList<AADhaar_Profile> aaDhaar_profiles = parseAadhaarXML(xpp);

                        if (!aaDhaar_profiles.isEmpty() && aaDhaar_profiles.size() > 0)
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
                            int aadhaar_message = -1;

                            for (AADhaar_Profile aadhaar : aaDhaar_profiles)
                            {
                                String aadhaarname = aadhaar.getName();
                                int aadhaarnamesize = aadhaar.getName().split(" ").length;

                                String firstName = "";
                                if (aadhaarnamesize == 1)
                                {
                                    firstName = aadhaar.getName().toString(); // to get the first name
                                }
                                else
                                    {
                                    int start = aadhaarname.indexOf(' ');
                                    if (start >= 0) {
                                        firstName = aadhaarname.substring(0, start);
                                    }
                                }

                                Boolean uidmatched = false;
                                Boolean namematched = false;

                                if (!Functions.isNullOrEmpty(aadhaar.getUID().toString()) && edaadhaar_value.getText().toString().equals(aadhaar.getUID().toString())) {
                                    uidmatched = true;
                                }

                                if (!Functions.isNullOrEmpty(firstName) && edfn_value.getText().toString().equals(firstName)) {
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
                            }


                            if (aadhaar_message != -1) {
                                builder.setTitle("Confirm Aadhaar Data");

                                switch (aadhaar_message) {
                                    case 1:
                                        // both matched
                                        builder.setMessage( getString(R.string.aadhaar_matched));
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
                                        Functions.showToast(getActivity(), "Aadhaar data cancelled.");
                                    }
                                });

                                AlertDialog alert = builder.create();
                                alert.show();


                                //Functions.showToast(getActivity(), getString(R.string.barcode_success) + "Data:: " + barcode.displayValue);
                                //Log.d(TAG, "Barcode read: " + barcode.displayValue);
                            } else {
                                Functions.showToast(getActivity(), "Unable to Scan QR code");
                            }
                        } else {
                            Functions.showToast(getActivity(), "Scanned QR code is not related to aadhar.");
                        }
                    } catch (XmlPullParserException e) {
                        //e.printStackTrace();
                        Functions.showToast(getActivity(), "Scanned QR is Not valid for Aadhaar");
                        //Functions.showToast(getActivity(), "-1");
                    } catch (IOException e) {
                        // TODO Auto-generated catch block
                        //e.printStackTrace();
                        //Functions.showToast(getActivity(), "-2");
                        Functions.showToast(getActivity(), "Scanned QR Invalid for Aadhaar");
                    }

                } else {
                    //statusMessage.setText(R.string.barcode_failure);
                    Functions.showToast(getActivity(), getString(R.string.barcode_failure));
                    Log.d(TAG, "No barcode captured, intent data is null");
                }
            } else {
                //statusMessage.setText(String.format(getString(R.string.barcode_error),CommonStatusCodes.getStatusCodeString(resultCode)));
                Functions.showToast(getActivity(), String.format(getString(R.string.barcode_error), CommonStatusCodes.getStatusCodeString(resultCode)));
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
                        aadhaar.lm = parser.getAttributeValue(null,"lm");
                        aadhaar.street = parser.getAttributeValue(null, "street"); // Street
                        aadhaar.loc = parser.getAttributeValue(null, "loc"); // Location
                        aadhaar.vtc = parser.getAttributeValue(null, "vtc"); //
                        aadhaar.po = parser.getAttributeValue(null, "po"); // Post Office
                        aadhaar.dist = parser.getAttributeValue(null, "dist"); // District
                        aadhaar.state = parser.getAttributeValue(null, "state"); // State
                        aadhaar.pc = Long.parseLong(parser.getAttributeValue(null, "pc")); //Postal code - Pin
                        aadhaar.dob = parser.getAttributeValue(null,"dob");

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
                        edfn_value.setText(Functions.isNullOrEmpty(aadhar_data.getName()) ? "" : aadhar_data.getName());
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

                        edfn_value.setText(Functions.isNullOrEmpty(firstName + middleName) ? "" : firstName + middleName);

                        edln_value.setText(Functions.isNullOrEmpty(lastName) ? "" : lastName);
                    }
                }

                //"Uhid": "",
                if (!Functions.isNullOrEmpty(aadhar_data.getUID().toString())) {
                    edaadhaar_value.setText(aadhar_data.getUID().toString());
                }

                if (!Functions.isNullOrEmpty(aadhar_data.getDob()))
                {
                    String dd_MM_yyyy =  FromDateYYYY_MM_DDToDD_MM_YYYY(aadhar_data.getDob());

                    if(!Functions.isNullOrEmpty(dd_MM_yyyy) && !dd_MM_yyyy.equals("-1"))
                    {
                        eddob_value.setText(dd_MM_yyyy);
                    }
                }



                if (!Functions.isNullOrEmpty(aadhar_data.getGender().toString().toUpperCase())) {

                    List<String> indexes = new ArrayList<String>(Functions.Gender_LinkHasMap().keySet());
                    spgender_value.setSelection(indexes.indexOf(aadhar_data.getGender().toString().toUpperCase()));
                }


                if (!Functions.isNullOrEmpty(aadhar_data.getDistrict())) {
                    eddistrict_value.setText(aadhar_data.getDistrict());
                }

                String address_Details = null;

                if (!Functions.isNullOrEmpty(aadhar_data.getCareoff()))
                {
                    address_Details = aadhar_data.getCareoff() + ",";
                }

                if(!Functions.isNullOrEmpty(aadhar_data.getHouse()))
                {
                    if(!Functions.isNullOrEmpty(address_Details))
                    {
                        address_Details += aadhar_data.getHouse() + "," ;
                    }
                    else
                    {
                        address_Details = aadhar_data.getHouse();
                    }
                }

                if(!Functions.isNullOrEmpty(aadhar_data.getLoc()))
                {
                    if(!Functions.isNullOrEmpty(address_Details))
                    {
                        address_Details += aadhar_data.getLoc() + "," ;
                    }
                    else
                    {
                        address_Details = aadhar_data.getLoc();
                    }
                }

                if(!Functions.isNullOrEmpty(aadhar_data.getStreet()))
                {
                    if(!Functions.isNullOrEmpty(address_Details))
                    {
                      address_Details += aadhar_data.getStreet() + ",";
                    }
                    else
                    {
                        address_Details = aadhar_data.getStreet();
                    }
                }

                if(!Functions.isNullOrEmpty(address_Details))
                {
                    if (address_Details.endsWith(","))
                    {
                        address_Details = address_Details.substring(0, address_Details.length() - 1);
                    }
                    edAdrline1_value.setText(address_Details);
                }


                if (!Functions.isNullOrEmpty(aadhar_data.getVTC()))
                {
                    edcity_value.setText(aadhar_data.getVTC());
                }


                if (!Functions.isNullOrEmpty(aadhar_data.getState())) {

                    List<String> indexes_values = new ArrayList<String>(Functions.StateData_LinkHasMap().values());
                    spstate.setSelection(indexes_values.indexOf(aadhar_data.getState().toString()));
                }


                if (!Functions.isNullOrEmpty(aadhar_data.getPin().toString())) {
                    edpin_value.setText(aadhar_data.getPin().toString());
                }
            }

            Functions.showToast(getActivity(), "Aaadhaar Data Mapped Successfully");
        } else {
            Functions.showToast(getActivity(), "Aaadhaar Data Not Mapped");
        }
    }

    public static String FromDateYYYY_MM_DDToDD_MM_YYYY(String dateStr)
    {
        //from
        DateFormat fromFormat = new SimpleDateFormat("yyyy-MM-dd");
        fromFormat.setLenient(false);

        //to - dd/MM/yyyy
        DateFormat toFormat = new SimpleDateFormat("dd/MM/yyyy");
        toFormat.setLenient(false);

        Date date = null;
        String todateHH = "-1";
        try {
            date = fromFormat.parse(dateStr);

            todateHH = toFormat.format(date).toString();
        } catch (ParseException e) {
            //e.printStackTrace();
            todateHH = "-1";
        }

        return todateHH;
    }



    public void LoadProfileInfoData(String url) {


        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }


    private void LoadJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_ProfileInfoData ProfileInfo_pj = new ParseJson_ProfileInfoData(jsonData);
        String STATUS = ProfileInfo_pj.parseJson();
        if (STATUS.equals("1"))
        {
            // By Deafult False;
            //enableDisableView(lv_profile, false);

            //1. To update app details if user profile is changed
             SharedPreferences.Editor editor = Functions.pref.edit();

            // Required Fields - P_Email - MobileNo
            edfn_value.setText(Functions.isNullOrEmpty(ParseJson_ProfileInfoData.FirstName) ? "" : ParseJson_ProfileInfoData.FirstName);
            edln_value.setText(Functions.isNullOrEmpty(ParseJson_ProfileInfoData.LastName) ? "" : ParseJson_ProfileInfoData.LastName);

            //2. To update profile name shared prefrence
            String profile_name = edfn_value.getText().toString()+ " "+ edln_value.getText().toString();
            editor.putString(Functions.P_NAME, Functions.isNullOrEmpty(profile_name)?null:profile_name);

            edemail_value.setText(Functions.isNullOrEmpty(ParseJson_ProfileInfoData.Email) ? "" : ParseJson_ProfileInfoData.Email); // Cant be updated so disabled

            //"Uhid": "",
            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.Uhid) && !ParseJson_ProfileInfoData.Uhid.equals("not available"))
            {
                edaadhaar_value.setText(ParseJson_ProfileInfoData.Uhid);

                //3. To update aadhar shared prefrence
                editor.putString(Functions.P_AdhrN, Functions.isNullOrEmpty(edaadhaar_value.getText().toString()) ? null : Functions.encrypt(getActivity(), edaadhaar_value.getText().toString()));

            }


            //"DOB": "1982-07-13T00:00:00",
            //"strDOB": "13/07/1982",
            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.strDOB) && !ParseJson_ProfileInfoData.strDOB.equals("not available")) {
                eddob_value.setText(ParseJson_ProfileInfoData.strDOB);
            }

            //"Gender": "M",
            //"strGender": null,
            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.Gender) && !ParseJson_ProfileInfoData.Gender.equals("not available")) {
                // From Key get Value

                // To get data from key
                List<String> indexes = new ArrayList<String>(Functions.Gender_LinkHasMap().keySet());
                spgender_value.setSelection(indexes.indexOf(ParseJson_ProfileInfoData.Gender));
            }

            //"BloodType": 0,
            //"strBloodType": "Do Not Specify"
            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.BloodType) && !ParseJson_ProfileInfoData.BloodType.equals("not available")) {

                List<String> indexes = new ArrayList<String>(Functions.BloodGroup_LinkHasMap().keySet());
                spbg_value.setSelection(indexes.indexOf(ParseJson_ProfileInfoData.BloodType));


            }


            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.District) && !ParseJson_ProfileInfoData.District.equals("not available")) {
                eddistrict_value.setText(ParseJson_ProfileInfoData.District);
            }

            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.AddressLine1) && !ParseJson_ProfileInfoData.AddressLine1.equals("not available")) {
                edAdrline1_value.setText(ParseJson_ProfileInfoData.AddressLine1);
            }

            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.AddressLine2) && !ParseJson_ProfileInfoData.AddressLine2.equals("not available")) {
                edAdrline2_value.setText(ParseJson_ProfileInfoData.AddressLine2);
            }


            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.City_Vill_Town) && !ParseJson_ProfileInfoData.City_Vill_Town.equals("not available")) {
                edcity_value.setText(ParseJson_ProfileInfoData.City_Vill_Town);
            }


            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.State) && !ParseJson_ProfileInfoData.State.equals("not available")) {

                List<String> indexes = new ArrayList<String>(Functions.StateData_LinkHasMap().keySet());
                spstate.setSelection(indexes.indexOf(ParseJson_ProfileInfoData.State));
            }


            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.Home_Phone) && !ParseJson_ProfileInfoData.Home_Phone.equals("not available")) {
                edphne_value.setText(ParseJson_ProfileInfoData.Home_Phone);
            }



            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.Pin) && !ParseJson_ProfileInfoData.Pin.equals("not available")) {
                edpin_value.setText(ParseJson_ProfileInfoData.Pin);
            }


            if (ParseJson_ProfileInfoData.DiffAbled.equals("true")) {
                rdDiff_Group.check(R.id.rdYes);

                lvdiffableType.setVisibility(View.VISIBLE);


                if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.DAbilityType) && !ParseJson_ProfileInfoData.DAbilityType.equals("not available")) {

                    List<String> indexes = new ArrayList<String>(Functions.DiffAbledType_LinkHasMap().keySet());
                    spdiffableType.setSelection(indexes.indexOf(ParseJson_ProfileInfoData.DAbilityType));


                }
            } else {
                rdDiff_Group.check(R.id.rdNo);
                lvdiffableType.setVisibility(View.GONE);
            }


            if (!Functions.isNullOrEmpty(ParseJson_ProfileInfoData.Cell_Phone) && !ParseJson_ProfileInfoData.Cell_Phone.equals("not available")) {
                edmob_value.setText(ParseJson_ProfileInfoData.Cell_Phone);
            }

//

            //4. To commit shared prefrence changes for final save
            editor.apply();

            // Update Navigation User Name or Email - as changed
            Functions.LoadHeaderTextContent(getActivity());

            mSwipeRefreshLayout.setRefreshing(false);



        }
        else
            {

            mSwipeRefreshLayout.setRefreshing(false);
        }

        Functions.showProgress(false, mProgressView);
    }


    public void SendProfileInfoData(String url) {

        if (validateFirstName() == true && validateLastName() == true && validateUhid() == true && validateDOB() == true && validatePhoneNumber() == true && validatePin() == true) {
            String Date_To_HH = Functions.DateToDateHH(eddob_value.getText().toString().trim());
            if (!Date_To_HH.equals("-1")) {
                Functions.showProgress(true, mProgressView);


                Map<String, String> jsonParams = new HashMap<String, String>();

                jsonParams.put("UserId", Functions.ApplicationUserid);
                jsonParams.put("FirstName", edfn_value.getText().toString());
                jsonParams.put("LastName", edln_value.getText().toString());
                jsonParams.put("AddressLine1", edAdrline1_value.getText().toString());
                jsonParams.put("AddressLine2", edAdrline2_value.getText().toString());

                Map.Entry<String, String> spbg_item = (Map.Entry<String, String>) spbg_value.getSelectedItem();
                jsonParams.put("BloodType", spbg_item.getKey().toString()); // spbg_value.getSelectedItem().toString()




                int selectedId = rdDiff_Group.getCheckedRadioButtonId();

                if (selectedId == radioYes.getId()) {
                    jsonParams.put("DiffAbled", "true");
                    //spdiffableType
                    Map.Entry<String, String> spdiffableType_item = (Map.Entry<String, String>) spdiffableType.getSelectedItem();
                    jsonParams.put("DAbilityType", spdiffableType_item.getKey().toString());
                } else {
                    jsonParams.put("DiffAbled", "false");
                    //spdiffableType
                    jsonParams.put("DAbilityType", "0");
                }

                jsonParams.put("DOB", Date_To_HH.toString());



                jsonParams.put("City_Vill_Town", edcity_value.getText().toString());
                jsonParams.put("District", eddistrict_value.getText().toString());

                Map.Entry<String, String> spGender_item = (Map.Entry<String, String>) spgender_value.getSelectedItem();
                jsonParams.put("Gender", spGender_item.getKey().toString()); //spgender_value

                jsonParams.put("Pin", edpin_value.getText().toString());

                Map.Entry<String, String> spstate_item = (Map.Entry<String, String>) spstate.getSelectedItem();
                jsonParams.put("State", spstate_item.getKey().toString());//spstate

                jsonParams.put("Uhid", edaadhaar_value.getText().toString());
                jsonParams.put("Home_Phone", edphne_value.getText().toString());


                JsonObjectRequest postRequestProfile = new JsonObjectRequest(Request.Method.POST, url,
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
                                Functions.showProgress(false, mProgressView);
                                mSwipeRefreshLayout.setRefreshing(false);
                                Functions.ErrorHandling(getActivity(), error);
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
                MySingleton.getInstance(getActivity()).addToRequestQueue(postRequestProfile);



            } else {
                Functions.showSnackbar(getView(), "Invalid DOB", "Action");
                return;
            }
        } else {

            return;
        }

    }

    private void AfterPostProfile(JSONObject response) {
        ParseJson_ProfileInfoData profileInfo_pj = new ParseJson_ProfileInfoData(response);
        String STATUS_Post = profileInfo_pj.parsePostResponseProfile();
        if (STATUS_Post.equals("1")) {

            Functions.enableDisableView(lv_profile, false);
            edemail_value.setEnabled(false);
            edmob_value.setEnabled(false);

            rl_fab_Profile.setVisibility(View.GONE);
            fab_edit.setVisibility(View.VISIBLE);

            if (!Functions.isNullOrEmpty(edfn_value.getText().toString()) && !Functions.isNullOrEmpty(edln_value.getText().toString()))
            {
                //1. To update app details if user profile is changed
                SharedPreferences.Editor editor = Functions.pref.edit();
                //2. To update profile name shared prefrence
                String profile_name = edfn_value.getText().toString()+ " "+ edln_value.getText().toString();
                editor.putString(Functions.P_NAME, Functions.isNullOrEmpty(profile_name)?null:profile_name);
                //3. To update aadhar shared prefrence
                editor.putString(Functions.P_AdhrN, Functions.isNullOrEmpty(edaadhaar_value.getText().toString()) ? null : Functions.encrypt(getActivity(), edaadhaar_value.getText().toString()));
                editor.apply();

                // Update Navigation User Name or Email - as changed
                Functions.LoadHeaderTextContent(getActivity());
            }

            if (!Functions.isNullOrEmpty(edaadhaar_value.getText().toString().trim())) {
                SharedPreferences.Editor editor = Functions.pref.edit();
                editor.putString(Functions.P_AdhrN, Functions.isNullOrEmpty(edaadhaar_value.getText().toString().trim()) ? null : edaadhaar_value.getText().toString());
                editor.apply();
            }

            Functions.showSnackbar(getView(), "Profile Info - Data Updated", "Action");
            //LoadProfileInfoData(url);
        } else if (STATUS_Post.equals("0")) {
            // mSwipeRefreshLayout.setRefreshing(false);
            Functions.showSnackbar(getView(), "Profile Info - Nothing To Change", "Action");
        } else {
            //mSwipeRefreshLayout.setRefreshing(false);
            Functions.showToast(getActivity(), STATUS_Post);
        }

        fab_aadhaar_Profile.setVisibility(View.GONE);
        Functions.showProgress(false, mProgressView);
    }

    // Required
    protected boolean validateFirstName() {
        boolean bool_firstname = true;
        if (edfn_value.getText().toString().trim().isEmpty()) {
            input_edfn_value.setErrorEnabled(true);
            input_edfn_value.setError(getString(R.string.errfirstname));
            requestFocus(edfn_value);
            bool_firstname = false;
        } else {
            input_edfn_value.setError(null);
            input_edfn_value.setErrorEnabled(false);
        }

        return bool_firstname;
    }

    // Required
    protected boolean validateLastName() {


        boolean bool_lastname = true;

        if (edln_value.getText().toString().trim().isEmpty()) {
            input_edln_value.setErrorEnabled(true);
            input_edln_value.setError(getString(R.string.errlastname));
            requestFocus(edln_value);
            bool_lastname = false;
        } else {

            input_edln_value.setError(null);
            input_edln_value.setErrorEnabled(false);
        }
        return bool_lastname;
    }

    // Required
    protected boolean validateUhid() {

        //empty
        Boolean bool_Uhid = true;

        if (edaadhaar_value.getText().toString().trim().isEmpty()) {
            input_aadhaar_value.setErrorEnabled(true);
            input_aadhaar_value.setError(getString(R.string.erraadhar));
            requestFocus(edaadhaar_value);
            bool_Uhid = false;
        } else {
            try {
                double d = Double.valueOf(edaadhaar_value.getText().toString().trim());

                //valid integer
                if (d == (long) d) {
                    //valid lenght
                    if (edaadhaar_value.getText().toString().trim().length() == 12) {
                        input_aadhaar_value.setError(null);
                        input_aadhaar_value.setErrorEnabled(false);
                        //bool_Uhid = true;
                    } else {
                        input_aadhaar_value.setErrorEnabled(true);
                        input_aadhaar_value.setError(getString(R.string.erraadharlenght));
                        requestFocus(edaadhaar_value);
                        bool_Uhid = false;
                    }
                } else {
                    //System.out.println("double"+d);
                    input_aadhaar_value.setErrorEnabled(true);
                    input_aadhaar_value.setError(getString(R.string.erraadharint));
                    requestFocus(edaadhaar_value);
                    bool_Uhid = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_aadhaar_value.setErrorEnabled(true);
                input_aadhaar_value.setError(getString(R.string.erraadharint));
                requestFocus(edaadhaar_value);
                bool_Uhid = false;
            }
        }

        return bool_Uhid;
    }


    // Required
    protected boolean validateDOB() {


        boolean bool_DOB = true;
        if (Functions.isNullOrEmpty(eddob_value.getText().toString())) {
            input_dob_value.setErrorEnabled(true);
            input_dob_value.setError(getString(R.string.errdobreq));
            requestFocus(eddob_value);
            bool_DOB = false;
        } else {
            DateValidator d = new DateValidator();
            if (d.isThisDateValid(eddob_value.getText().toString(), "dd/MM/yyyy")) {
                input_dob_value.setError(null);
                input_dob_value.setErrorEnabled(false);
                //bool_DOB =  true;
            } else {
                input_dob_value.setErrorEnabled(true);
                input_dob_value.setError(getString(R.string.errdob));
                requestFocus(eddob_value);
                bool_DOB = false;
            }
        }

        return bool_DOB;
    }


    // Not Required Case
    protected boolean validatePhoneNumber() {


        boolean bool_phone = true;
        if (!Functions.isNullOrEmpty(edphne_value.getText().toString().trim())) {

            try {
                double d = Double.valueOf(edphne_value.getText().toString().trim());
                if (d == (long) d) {
                    if (edphne_value.getText().toString().trim().length() == 10) {
                        //System.out.println("integer"+(int)d);
                        input_phne_value.setError(null);
                        input_phne_value.setErrorEnabled(false);
                        //return true;
                    } else {
                        input_phne_value.setErrorEnabled(true);
                        input_phne_value.setError(getString(R.string.errphonelenght));
                        requestFocus(edphne_value);
                        bool_phone = false;
                    }
                } else {
                    //System.out.println("double"+d);
                    input_phne_value.setErrorEnabled(true);
                    input_phne_value.setError(getString(R.string.errphoneint));
                    requestFocus(edphne_value);
                    bool_phone = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                input_phne_value.setErrorEnabled(true);
                input_phne_value.setError(getString(R.string.errphoneint));
                requestFocus(edphne_value);
                bool_phone = false;
            }
        } else {
            input_phne_value.setError(null);
            input_phne_value.setErrorEnabled(false);
            // return true;
        }

        return bool_phone;
    }

    // Not Required Case
    protected boolean validatePin() {


        boolean bool_pin = true;
        if (!Functions.isNullOrEmpty(edpin_value.getText().toString().trim())) {
            try {
                double d = Double.valueOf(edpin_value.getText().toString().trim());
                if (d == (long) d) {
                    if (edpin_value.getText().toString().trim().length() == 6) {
                        //System.out.println("integer"+(int)d);
                        inputTXTPin.setError(null);
                        inputTXTPin.setErrorEnabled(false);
                        //bool_pin = true;
                    } else {
                        inputTXTPin.setErrorEnabled(true);
                        inputTXTPin.setError(getString(R.string.errpinlenght));
                        requestFocus(edpin_value);
                        bool_pin = false;
                    }
                } else {
                    //System.out.println("double"+d);
                    inputTXTPin.setErrorEnabled(true);
                    inputTXTPin.setError(getString(R.string.errpinint));
                    requestFocus(edpin_value);
                    bool_pin = false;
                }
            } catch (Exception e) {
                //System.out.println("not number");
                inputTXTPin.setErrorEnabled(true);
                inputTXTPin.setError(getString(R.string.errpinint));
                requestFocus(edpin_value);
                bool_pin = false;
            }
        } else {
            inputTXTPin.setError(null);
            inputTXTPin.setErrorEnabled(false);
            //bool_pin = true;
        }

        return bool_pin;
    }

    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getActivity().getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
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
                case R.id.edfn_value:
                    // Required check for empty also
                    validateFirstName();
                    break;
                case R.id.edln_value:
                    // Required check for empty also
                    validateLastName();
                    break;
                case R.id.edaadhaar_value:
                    // Required check for empty also
                    validateUhid();
                    break;
                case R.id.eddob_value:
                    // Required check for empty also
                    validateDOB();
                    break;
                case R.id.edpin_value:
                    // cHECK FOR VALID PIN
                    validatePin();
                    break;
                case R.id.edphne_value:
                    // cHECK FOR VALID PHONE
                    validatePhoneNumber();
                    break;
            }
        }
    }

}
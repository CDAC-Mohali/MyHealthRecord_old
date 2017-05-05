package com.pkg.healthrecordappname.appfinalname.modules.fragments;

import android.app.Fragment;
import android.app.FragmentManager;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.TextInputLayout;
import android.support.v4.widget.SwipeRefreshLayout;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.SeekBar;
import android.widget.Spinner;
import android.widget.TextView;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.PHRMS_LoginActivity;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_ReportSharingData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.LinkedHashMapAdapter;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

import static com.pkg.healthrecordappname.appfinalname.modules.useables.Functions.PHRMS_FRAGMENT;
import static com.pkg.healthrecordappname.appfinalname.modules.useables.Functions.mfragment;
import static com.pkg.healthrecordappname.appfinalname.modules.useables.Functions.mfragmentManager;


public class PHRMS_ReportSharing_Fragment extends Fragment {
    String url = null;

    private Spinner sp_ReportSharingDocList_value;

    private Boolean ReportSharingDocListeHasValue = false;

    private LinkedHashMapAdapter<String, String> doctorlist_adapter;

    private String report_List = "1"; // for Personal Information

    private Button btSelectAll;

    private Button btDeSelectAll;

    CheckBox cb_PersonalInformation;
    CheckBox cb_Allergies;
    CheckBox cb_Immunizations;
    CheckBox cb_Medications;
    CheckBox cb_Procedures;
    CheckBox cb_LabsTests;
    CheckBox cb_Problems;
    CheckBox cb_WellnessActivities;
    CheckBox cb_WellnessBloodPressure;
    CheckBox cb_WellnessBloodGlucose;
    CheckBox cb_WellnessBMI;
    Button btn_reportshare;


    EditText ed_reportsharingquery_value;
    TextInputLayout input_ed_reportsharingquery_value;
    private SeekBar seekbarRSValidUpto;

    ProgressBar mProgressBarReportSharing;

    FloatingActionButton fab_addMedicalContact_Sharing;

    private SwipeRefreshLayout mSwipeRefreshLayout_RS;

    TextView tv_ReportSharingDocList_value;

    public PHRMS_ReportSharing_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_reportsharing, container, false);

        mProgressBarReportSharing = (ProgressBar) rootView.findViewById(R.id.ProgressBarRS);

        btSelectAll = (Button) rootView.findViewById(R.id.btSelectAll);
        btDeSelectAll = (Button) rootView.findViewById(R.id.btDeSelectAll);
        btn_reportshare = (Button) rootView.findViewById(R.id.btn_reportshare);

        ed_reportsharingquery_value = (EditText) rootView.findViewById(R.id.ed_reportsharingquery_value);
        input_ed_reportsharingquery_value = (TextInputLayout) rootView.findViewById(R.id.input_ed_reportsharingquery_value);

        fab_addMedicalContact_Sharing = (FloatingActionButton) rootView.findViewById(R.id.fab_AddMedicalContactFrom_ReportSharing);
        mSwipeRefreshLayout_RS = (SwipeRefreshLayout) rootView.findViewById(R.id.swipe_refresh_ReportSharing);

        sp_ReportSharingDocList_value = (Spinner) rootView.findViewById(R.id.sp_ReportSharingDocList_value);
        sp_ReportSharingDocList_value.setEnabled(false);

        seekbarRSValidUpto = (SeekBar) rootView.findViewById(R.id.seekbarRSValidUpto);

        tv_ReportSharingDocList_value = (TextView) rootView.findViewById(R.id.tv_ReportSharingDocList_value);

        cb_PersonalInformation = (CheckBox) rootView.findViewById(R.id.checkbox_PersonalInformation);
        cb_Allergies = (CheckBox) rootView.findViewById(R.id.checkbox_Allergies);
        cb_Immunizations = (CheckBox) rootView.findViewById(R.id.checkbox_Immunizations);
        cb_Medications = (CheckBox) rootView.findViewById(R.id.checkbox_Medications);
        cb_Procedures = (CheckBox) rootView.findViewById(R.id.checkbox_Procedures);
        cb_LabsTests = (CheckBox) rootView.findViewById(R.id.checkbox_LabsTests);
        cb_Problems = (CheckBox) rootView.findViewById(R.id.checkbox_Problems);
        cb_WellnessActivities = (CheckBox) rootView.findViewById(R.id.checkbox_WellnessActivities);
        cb_WellnessBloodPressure = (CheckBox) rootView.findViewById(R.id.checkbox_WellnessBloodPressure);
        cb_WellnessBloodGlucose = (CheckBox) rootView.findViewById(R.id.checkbox_WellnessBloodGlucose);
        cb_WellnessBMI = (CheckBox) rootView.findViewById(R.id.checkbox_WellnessBMI);

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {
                url = getString(R.string.urlLogin) + getString(R.string.GetReportSharingDoctorsList) + userid;

                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                if (url != null) {
                    LoadDoctorListReportSharingData(url);
                }

                // Doctor List Spinner
                sp_ReportSharingDocList_value.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
                    @Override
                    public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                        // TODO Auto-generated method stub
                        // Contact Type Required.
                        Map.Entry<String, String> spReportSharingDocList_item = (Map.Entry<String, String>) sp_ReportSharingDocList_value.getSelectedItem();
                        String[] s = spReportSharingDocList_item.getKey().toString().split("_");


                        if (!Functions.isNullOrEmpty(spReportSharingDocList_item.getKey()) && s.length == 2) {
                            ReportSharingDocListeHasValue = true;
                        } else {
                            ReportSharingDocListeHasValue = false;
                        }
                    }

                    @Override
                    public void onNothingSelected(AdapterView<?> parent) {
                        // TODO Auto-generated method stub
                        ReportSharingDocListeHasValue = false;
                    }
                });


                btSelectAll.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        SelectDeselect(true);
                    }
                });

                btDeSelectAll.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        SelectDeselect(false);
                    }
                });


                btn_reportshare.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                                Functions.mainscreen(getActivity());
                            } else {
                                ReportSharingData();
                            }
                        } else {
                            Functions.showSnackbar(view, "Internet Not Available !!", "Action");
                        }
                    }
                });


                // Start Progress bar ValidUpto with 0 to 4
                seekbarRSValidUpto.setProgress(0);
                seekbarRSValidUpto.incrementProgressBy(1);
                seekbarRSValidUpto.setMax(4);
                seekbarRSValidUpto.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
                    @Override
                    public void onProgressChanged(SeekBar seekBar, int progresValue, boolean fromUser) {
                        if (fromUser) {
                            if (progresValue >= 0 && progresValue <= seekBar.getMax()) {
                                // As data is from 0 to 4
                                // Required is 1 to 5 so add each value by 1
                                seekBar.setSecondaryProgress(progresValue);
                                //Functions.showToast(getActivity(), "Stopped tracking seekbar:: " + progresValue);
                            }
                        }
                    }

                    @Override
                    public void onStartTrackingTouch(SeekBar seekBar) {
                        // Functions.showToast(getActivity(),"Started tracking seekbar");
                    }

                    @Override
                    public void onStopTrackingTouch(SeekBar seekBar) {
                        //textView.setText("Covered: " + progress + "/" + seekBar.getMax());
                        //Functions.showToast(getActivity(), "Stopped tracking seekbar:: " + seekBar.getMax());
                    }
                });

                mSwipeRefreshLayout_RS.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {
                                //mSwipeRefreshLayout.setRefreshing(true);
                                LoadDoctorListReportSharingData(url);

                            } else {
                                Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                            }
                        }
                    }
                });

                // Get Navigation View Control
                final Menu menuNav = Functions.navigationView.getMenu();

                //Activity Relative Layout Onlcicks to open coresponding fragments
                fab_addMedicalContact_Sharing.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            mfragment = new PHRMS_MedicalContact_Fragment();
                            if (mfragment != null) {
                                if (Functions.isNavDrawerOpen()) {
                                    Functions.closeNavDrawer();
                                }

                                // Custom Check Navigation View
                                MenuItem activitiesItem = menuNav.findItem(R.id.nav_addmedicalcontact);

                                Functions.navigationView.setCheckedItem(activitiesItem.getItemId());

                                mfragmentManager = getFragmentManager();

                                mfragmentManager.beginTransaction().replace(R.id.content_frame, mfragment, PHRMS_FRAGMENT).commit();

                                mfragmentManager
                                        .addOnBackStackChangedListener(new FragmentManager.OnBackStackChangedListener() {
                                            @Override
                                            public void onBackStackChanged() {
                                                if (getFragmentManager().getBackStackEntryCount() == 0) {
                                                    getActivity().finish();
                                                } else if (getFragmentManager().getBackStackEntryCount() > 0) {
                                                    mfragmentManager.popBackStack();
                                                }
                                            }
                                        });
                            } else {
                                // error in creating fragment
                                Log.e("MainActivity", "Error in creating fragment");
                            }
                        } else {

                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }
                    }
                });

            }

        } else {
            Functions.showSnackbar(rootView, Functions.IE_NotAvailable, "Action");
        }

        return rootView;
    }

    public void LoadDoctorListReportSharingData(String doc_list_url) {
        Functions.showProgress(true, mProgressBarReportSharing);

        final JsonObjectRequest jsObjRequestReportSharingsDocList = new JsonObjectRequest(Request.Method.GET, doc_list_url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONDataReportSharingsDocListSpinner(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressBarReportSharing);
                mSwipeRefreshLayout_RS.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequestReportSharingsDocList);
    }


    public void LoadJSONDataReportSharingsDocListSpinner(JSONObject ReportSharingsDocList) {
        ParseJson_ReportSharingData route_pj = new ParseJson_ReportSharingData(ReportSharingsDocList);
        String STATUS = route_pj.parseJsonReportSharingDocList();

        if (STATUS.equals("1")) {
            tv_ReportSharingDocList_value.setVisibility(View.GONE);

            sp_ReportSharingDocList_value.setVisibility(View.VISIBLE);

            // Binding using LinkedhashMapAdapter from parsed data
            doctorlist_adapter = new LinkedHashMapAdapter<String, String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, ParseJson_ReportSharingData.hmReportSharingDoctorContact);
            doctorlist_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            sp_ReportSharingDocList_value.setAdapter(doctorlist_adapter);

            if (ParseJson_ReportSharingData.hmReportSharingDoctorContact.size() > 0) {
                sp_ReportSharingDocList_value.setEnabled(true);
            }
        } else if (STATUS.equals("-1")) {
            tv_ReportSharingDocList_value.setVisibility(View.VISIBLE);
            sp_ReportSharingDocList_value.setVisibility(View.GONE);

            Functions.showToast(getActivity(), "Medical Contact Not Available - Add");
        } else {
            tv_ReportSharingDocList_value.setVisibility(View.VISIBLE);
            sp_ReportSharingDocList_value.setVisibility(View.GONE);

            Functions.showToast(getActivity(), "Unable to load Medical Contact");
        }

        mSwipeRefreshLayout_RS.setRefreshing(false);
        Functions.showProgress(false, mProgressBarReportSharing);
    }


    public void SelectDeselect(Boolean b_value) {
        if (b_value == true) {
            cb_Allergies.setChecked(b_value);
            cb_Immunizations.setChecked(b_value);
            cb_Medications.setChecked(b_value);
            cb_Procedures.setChecked(b_value);
            cb_LabsTests.setChecked(b_value);
            cb_Problems.setChecked(b_value);
            cb_WellnessActivities.setChecked(b_value);
            cb_WellnessBloodPressure.setChecked(b_value);
            cb_WellnessBloodGlucose.setChecked(b_value);
            cb_WellnessBMI.setChecked(b_value);
        } else {
            cb_Allergies.setChecked(b_value);
            cb_Immunizations.setChecked(b_value);
            cb_Medications.setChecked(b_value);
            cb_Procedures.setChecked(b_value);
            cb_LabsTests.setChecked(b_value);
            cb_Problems.setChecked(b_value);
            cb_WellnessActivities.setChecked(b_value);
            cb_WellnessBloodPressure.setChecked(b_value);
            cb_WellnessBloodGlucose.setChecked(b_value);
            cb_WellnessBMI.setChecked(b_value);
        }
    }

    public void ReportSharingData() {
        if (ReportSharingDocListeHasValue) // Checked doctor list with data both number and emailid
        {
            if (validateReportSharingQuery() == true) // Check if query is provided
            {
                Map.Entry<String, String> spReportSharingDocList_item = (Map.Entry<String, String>) sp_ReportSharingDocList_value.getSelectedItem();
                String[] report_sharing_data = spReportSharingDocList_item.getKey().toString().split("_");

                if (report_sharing_data.length == 2) {

                    Functions.showProgress(true, mProgressBarReportSharing);

                    Map<String, String> jsonParams = new HashMap<String, String>();


                    jsonParams.put("UserId", Functions.ApplicationUserid);
                    jsonParams.put("Query", ed_reportsharingquery_value.getText().toString());


                    if (cb_Allergies.isChecked()) {
                        report_List += "," + "7";
                    }

                    if (cb_Immunizations.isChecked()) {
                        report_List += "," + "8";
                    }

                    if (cb_Medications.isChecked()) {
                        report_List += "," + "9";
                    }

                    if (cb_Procedures.isChecked()) {
                        report_List += "," + "10";
                    }
                    if (cb_LabsTests.isChecked()) {
                        report_List += "," + "11";
                    }
                    if (cb_Problems.isChecked()) {
                        report_List += "," + "12";
                    }
                    if (cb_WellnessActivities.isChecked()) {
                        report_List += "," + "13";
                    }
                    if (cb_WellnessBloodPressure.isChecked()) {
                        report_List += "," + "14";
                    }
                    if (cb_WellnessBloodGlucose.isChecked()) {
                        report_List += "," + "15";
                    }
                    if (cb_WellnessBMI.isChecked()) {
                        report_List += "," + "16";
                    }


                    jsonParams.put("strChecks", report_List);


                    jsonParams.put("PhoneNumber", report_sharing_data[0].toString());
                    jsonParams.put("EmailAddress", report_sharing_data[1].toString());

                    // As data is from 0 to 4
                    // Required is 1 to 5 so add each value by 1
                    jsonParams.put("ValidUpto", String.valueOf(seekbarRSValidUpto.getProgress() + 1));

                    String share_url = getString(R.string.urlLogin) + getString(R.string.ShareHealthReport);


                    JsonObjectRequest postRequestReportSharingSave = new JsonObjectRequest(Request.Method.POST, share_url, new JSONObject(jsonParams), new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            AfterPostReportSharing(response);
                        }
                    },
                            new Response.ErrorListener() {
                                @Override
                                public void onErrorResponse(VolleyError error) {
                                    Functions.showProgress(false, mProgressBarReportSharing);
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

                    // Pass values to volley to stop sending multiple requests reduced from 2500milisecond to 2000 - 20secs

                    postRequestReportSharingSave.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                    // Access the RequestQueue through your singleton class.
                    MySingleton.getInstance(getActivity()).addToRequestQueue(postRequestReportSharingSave);

                    //Functions.showToast(getActivity(), "Post Successfull");
                } else {
                    Functions.showToast(getActivity(), "Invalid Phone/Email ID");
                }


            } else {
                return;
            }
        } else {
            Functions.showToast(getActivity(), "Doctor list not loaded.");
        }
    }


    // Required
    protected boolean validateReportSharingQuery() {
        boolean bool_query = true;

        if (ed_reportsharingquery_value.getText().toString().trim().isEmpty()) {
            input_ed_reportsharingquery_value.setErrorEnabled(true);
            input_ed_reportsharingquery_value.setError(getString(R.string.errReportSharingQuery));
            requestFocus(ed_reportsharingquery_value);
            bool_query = false;
        } else {
            input_ed_reportsharingquery_value.setError(null);
            input_ed_reportsharingquery_value.setErrorEnabled(false);
        }

        return bool_query;
    }


    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getActivity().getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    private void AfterPostReportSharing(JSONObject response) {
        ParseJson_ReportSharingData addReportSharing_pj = new ParseJson_ReportSharingData(response);
        String STATUS_Post = addReportSharing_pj.parsePostResponseReportSharing();

        switch (STATUS_Post) {
            case "0": // Error
                Functions.showToast(getActivity(), "Report Not Shared");
                //Functions.showSnackbar(parentLayout, "Report Not Saved", "Action");
                break;
            case "1": // Success
                SelectDeselect(false);
                ed_reportsharingquery_value.setText("");
                Functions.showToast(getActivity(), "Report Shared Successfully");
                break;
            case "2": // Phone Number Already Exist
                Functions.showToast(getActivity(), "Phone Number Already Exist");

                break;
            case "3": // Email Already Exist
                Functions.showToast(getActivity(), "Email Address Already Exist");

                break;
            default:
                Functions.showToast(getActivity(), "Report Not Shared:: " + STATUS_Post);
                break;
        }

        Functions.showProgress(false, mProgressBarReportSharing);
    }


    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
    }

    // Use Prepare Menu when user wants to hide something from menu
    @Override
    public void onPrepareOptionsMenu(Menu menu) {
        MenuItem item = menu.findItem(R.id.action_share);
        item.setVisible(false);
    }


}
package com.pkg.healthrecordappname.appfinalname.modules.fragments;

import android.app.AlertDialog;
import android.app.Fragment;
import android.app.FragmentManager;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.Typeface;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.v4.widget.SwipeRefreshLayout;
import android.text.SpannableString;
import android.text.style.ForegroundColorSpan;
import android.text.style.RelativeSizeSpan;
import android.text.style.StyleSpan;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.bumptech.glide.Glide;
import com.bumptech.glide.load.engine.DiskCacheStrategy;
import com.bumptech.glide.load.resource.drawable.GlideDrawable;
import com.bumptech.glide.load.resource.gif.GifDrawable;
import com.bumptech.glide.request.RequestListener;
import com.bumptech.glide.request.target.Target;
import com.github.mikephil.charting.animation.Easing;
import com.github.mikephil.charting.charts.CombinedChart;
import com.github.mikephil.charting.charts.PieChart;
import com.github.mikephil.charting.components.AxisBase;
import com.github.mikephil.charting.components.Legend;
import com.github.mikephil.charting.components.LimitLine;
import com.github.mikephil.charting.components.XAxis;
import com.github.mikephil.charting.components.YAxis;
import com.github.mikephil.charting.data.BarData;
import com.github.mikephil.charting.data.BarDataSet;
import com.github.mikephil.charting.data.BarEntry;
import com.github.mikephil.charting.data.CombinedData;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.LineDataSet;
import com.github.mikephil.charting.data.PieData;
import com.github.mikephil.charting.data.PieDataSet;
import com.github.mikephil.charting.data.PieEntry;
import com.github.mikephil.charting.formatter.AxisValueFormatter;
import com.github.mikephil.charting.formatter.PercentFormatter;
import com.github.mikephil.charting.highlight.Highlight;
import com.github.mikephil.charting.listener.OnChartValueSelectedListener;
import com.github.mikephil.charting.utils.ColorTemplate;
import com.pkg.healthrecordappname.appfinalname.PHRMS_LoginActivity;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.httpconnections.HttpUrlConnectionRequest;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_DashboardData;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_WellnessActivitiesData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.FontManager;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.samsung.android.sdk.healthdata.HealthConnectionErrorResult;
import com.samsung.android.sdk.healthdata.HealthConstants;
import com.samsung.android.sdk.healthdata.HealthDataService;
import com.samsung.android.sdk.healthdata.HealthDataStore;
import com.samsung.android.sdk.healthdata.HealthPermissionManager;
import com.samsung.android.sdk.healthdata.HealthPermissionManager.PermissionKey;
import com.samsung.android.sdk.healthdata.HealthPermissionManager.PermissionResult;
import com.samsung.android.sdk.healthdata.HealthResultHolder;

import org.apache.http.client.ClientProtocolException;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import de.hdodenhof.circleimageview.CircleImageView;

import static com.pkg.healthrecordappname.appfinalname.modules.useables.Functions.MENU_ITEM_PERMISSION_SETTING;
import static com.pkg.healthrecordappname.appfinalname.modules.useables.Functions.PHRMS_FRAGMENT;
import static com.pkg.healthrecordappname.appfinalname.modules.useables.Functions.mfragment;
import static com.pkg.healthrecordappname.appfinalname.modules.useables.Functions.mfragmentManager;
import static com.pkg.healthrecordappname.appfinalname.modules.useables.Functions.saveStartTimeasLastTimeSync;


public class PHRMS_Dashboard_Fragment extends Fragment {

    private TextView txtDashboardData;
    String url_dashboard = null;


    private CombinedChart mBP_CominedChart;
    private CombinedChart mBG_CominedChart;
    private PieChart mAV_PieChart;
    private CombinedChart mBMI_CominedChart;
    private SwipeRefreshLayout mSwipeRefreshLayout;

    private ProgressBar mProgressBarDashboardGraphs;

    private LinearLayout mlvChartActivity;
    private LinearLayout mlvChartBP;
    private LinearLayout mlvChartBG;
    private LinearLayout mlvChartBMI;

    private CircleImageView imgview;

    private ProgressBar pbrUser;

    private RelativeLayout rvactivityCall;
    private RelativeLayout rvBPCall;
    private RelativeLayout rvBGCall;
    private RelativeLayout rvBMICall;

    private HealthConnectionErrorResult mConnError;
    private Set<PermissionKey> mKeySet;
    private StepDataCountReporter mStepReporter;
    private ExerciseDataCountReporter mExerciseReporter;

    private static LinearLayout mlvshealth;

    public static TextView todaydate;
    public static TextView stepdatakm;
    public static TextView Runningdatakm;

    private static Button btn_SHealthSync;

    // ShealthErrorCode = -1 // means error // Shealth Not Supported
    // ShealthErrorCode = 0 // means Success Shealth
    public int ShealthErrorCode = 0;

    private static Context mContextToast;

    private static TextView textUsernameDashboard;

    private UserLoginLoadHeaderContentTask mAuthHeaderTask = null;

    public PHRMS_Dashboard_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        final View rootView = inflater.inflate(R.layout.frame_dashboard, container, false);



        mlvshealth = (LinearLayout) rootView.findViewById(R.id.lvshealth);

        todaydate = (TextView) rootView.findViewById(R.id.textshealthDate);
        stepdatakm = (TextView) rootView.findViewById(R.id.tvswvalue);
        Runningdatakm = (TextView) rootView.findViewById(R.id.tvrunningvalue);

        txtDashboardData = (TextView) rootView.findViewById(R.id.txtdashboard);
        mAV_PieChart = (PieChart) rootView.findViewById(R.id.avchart_bar);
        mBP_CominedChart = (CombinedChart) rootView.findViewById(R.id.bpchart_bar);
        mBG_CominedChart = (CombinedChart) rootView.findViewById(R.id.bgchart_bar);

        mBMI_CominedChart = (CombinedChart) rootView.findViewById(R.id.bmichart_bar);

        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.data_dashboard_swipe_refresh);

        mProgressBarDashboardGraphs = (ProgressBar) rootView.findViewById(R.id.ProgressBarDashboardGraphs);

        mlvChartActivity = (LinearLayout) rootView.findViewById(R.id.lvChartActivity);
        mlvChartBP = (LinearLayout) rootView.findViewById(R.id.lvChartBP);
        mlvChartBG = (LinearLayout) rootView.findViewById(R.id.lvChartBG);
        mlvChartBMI = (LinearLayout) rootView.findViewById(R.id.lvChartBMI);

        rvactivityCall = (RelativeLayout) rootView.findViewById(R.id.rvactivityCall);
        rvBPCall = (RelativeLayout) rootView.findViewById(R.id.rvBPCall);
        rvBGCall = (RelativeLayout) rootView.findViewById(R.id.rvBGCall);
        rvBMICall = (RelativeLayout) rootView.findViewById(R.id.rvBMICall);

        // declare fontawseome support to all containers in drawer
        Typeface iconFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvType), iconFont);


        imgview = (CircleImageView) rootView.findViewById(R.id.imageUserDashboard);

        pbrUser = (ProgressBar) rootView.findViewById(R.id.ProgressBarUserImageDashboard);

        // To sync SHealth Data
        btn_SHealthSync = (Button) rootView.findViewById(R.id.btn_SHealthSync);

        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Dash_Refresh);

        FloatingActionButton fab_Share = (FloatingActionButton) rootView.findViewById(R.id.fab_Dash_Share);



        textUsernameDashboard = (TextView) rootView.findViewById(R.id.textUsernameDashboard);
        textUsernameDashboard.setText(Functions.pref.getString(Functions.P_NAME, ""));



        LoadHeaderContentDashboard(getActivity());

        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(Functions.ApplicationUserid)) {
                Functions.mainscreen(getActivity());
            } else {

                url_dashboard = getString(R.string.urlLogin) + getString(R.string.LoadDashboardDataAsc) + Functions.ApplicationUserid;

                if (url_dashboard != null) {
                    LoadDashboard(url_dashboard);
                }

                // Floating Action Button
                fab.setVisibility(View.GONE);
                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (url_dashboard != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {
                                // Load Dashboard only on floating button click
                                LoadHeaderContentDashboard(getActivity());

                                mSwipeRefreshLayout.setRefreshing(true);
                                LoadDashboard(url_dashboard);
                            } else {
                                mSwipeRefreshLayout.setRefreshing(false);
                                Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                            }
                        }
                    }
                });

                fab_Share.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            btn_share();
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }
                    }
                });


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url_dashboard != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {
                                LoadHeaderContentDashboard(getActivity());
                                LoadDashboard(url_dashboard);
                            } else {
                                mSwipeRefreshLayout.setRefreshing(false);
                                Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                            }
                        }
                    }
                });


                // Get Navigation View Control
                final Menu menuNav = Functions.navigationView.getMenu();

                //Activity Relative Layout Onlcicks to open coresponding fragments
                rvactivityCall.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            mfragment = new PHRMS_WellnessActivities_Fragment();
                            if (mfragment != null) {
                                if (Functions.isNavDrawerOpen()) {
                                    Functions.closeNavDrawer();
                                }

                                // Custom Check Navigation View
                                MenuItem activitiesItem = menuNav.findItem(R.id.nav_activities);
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

                //BP Relative Layout Onlcicks to open coresponding fragments
                rvBPCall.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            mfragment = new PHRMS_WellnessBP_Fragment();
                            if (mfragment != null) {
                                if (Functions.isNavDrawerOpen()) {
                                    Functions.closeNavDrawer();
                                }

                                // Custom Check Navigation View
                                MenuItem bpItem = menuNav.findItem(R.id.nav_bloodpressure);
                                Functions.navigationView.setCheckedItem(bpItem.getItemId());

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

                //BG Relative Layout Onlcicks to open coresponding fragments
                rvBGCall.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            mfragment = new PHRMS_WellnessGlucose_Fragment();
                            if (mfragment != null) {
                                if (Functions.isNavDrawerOpen()) {
                                    Functions.closeNavDrawer();
                                }

                                // Custom Check Navigation View
                                MenuItem bgItem = menuNav.findItem(R.id.nav_bloodglucose);
                                Functions.navigationView.setCheckedItem(bgItem.getItemId());

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

                //BG Relative Layout Onlcicks to open coresponding fragments
                rvBMICall.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            mfragment = new PHRMS_WellnessWeight_Fragment();
                            if (mfragment != null) {
                                if (Functions.isNavDrawerOpen()) {
                                    Functions.closeNavDrawer();
                                }

                                // Custom Check Navigation View
                                MenuItem bmiItem = menuNav.findItem(R.id.nav_weight);
                                Functions.navigationView.setCheckedItem(bmiItem.getItemId());

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


                // ************* Shealth Integration ************
                // 1. Condition - To Check API Level 19 Andorid KitKat 4.4 for sHealth
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT) {
                    // 1. First Time ShealthDisplay == true , As to display shealth permissions settings
                    // 2. If User Has canceled the SHealth Display or not installed SHealth, ShealthDisplay == false
                    // 3. ShealthDisplay == false is defined in Permissions Cancelled Settings

                    // Check Shealth Display - check if user have what user have selected to display shealth
                    Boolean ShealthDisplay = Functions.pref.getBoolean(Functions.PREF_ShealthDisplay, true);
                    // If User Has installed and selected SHealth
                    if (ShealthDisplay) {
                        CallShealthSettingsandServices();
                    }

                } else {
                    ShealthErrorCode = -1;
                    mlvshealth.setVisibility(View.GONE);
                }
                // ************* Shealth End ************

                //Shealth Data Sync
                btn_SHealthSync.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            if (ShealthErrorCode == 0) {
                                // Neither empty nor 0.0 float Zero
                                if ((!Functions.isNullOrEmpty(stepdatakm.getText().toString()) && !Double.valueOf(stepdatakm.getText().toString()).equals(0.0)) || (!Functions.isNullOrEmpty(Runningdatakm.getText().toString()) && !Double.valueOf(Runningdatakm.getText().toString()).equals(0.0))) {
                                    AlertDialog.Builder builder_Sync = new AlertDialog.Builder(getActivity());
                                    builder_Sync.setTitle("SYNC SHealth APP Data");
                                    builder_Sync.setMessage(getString(R.string.sync_Shealth)).setPositiveButton("Yes", dialogClickListenerForActivitySync)
                                            .setNegativeButton("No", dialogClickListenerForActivitySync).show();
                                } else {
                                    Functions.showToast(getActivity(), "No Shealth Data to Sync !!");
                                }
                            } else {
                                Functions.showToast(getActivity(), "Shealth Not Available");
                            }
                        } else {
                            Functions.showToast(getActivity(), Functions.IE_NotAvailable);
                        }

                    }
                });

            }

        } else {
            Functions.showSnackbar(rootView, Functions.IE_NotAvailable, "Action");
            mProgressBarDashboardGraphs.setVisibility(View.GONE);
        }

        return rootView;
    }

    // ************* Shealth ************

    private void CallShealthSettingsandServices() {
        // 2. Condition - To Check sHealth 4.6 Above for Health Data 1.2 or above
        // Step 1.
        mKeySet = new HashSet<PermissionKey>();
        mKeySet.add(new PermissionKey(HealthConstants.StepCount.HEALTH_DATA_TYPE, HealthPermissionManager.PermissionType.READ));

        // Exercise PermissionKey
        mKeySet.add(new PermissionKey(HealthConstants.Exercise.HEALTH_DATA_TYPE, HealthPermissionManager.PermissionType.READ));

        HealthDataService healthDataService = new HealthDataService();
        try {
            healthDataService.initialize(getActivity());
        } catch (Exception e) {
            mlvshealth.setVisibility(View.GONE);
            Log.e(Functions.APP_TAG, e.getClass().getName() + " - " + e.getMessage());
            Log.e(Functions.APP_TAG, "HealthDataService Error");
            e.printStackTrace();
        }

        // Create a HealthDataStore instance and set its listener
        Functions.mStoreHealth = new HealthDataStore(getActivity(), mConnectionListener);


        // Request the connection to the health data store
        Functions.mStoreHealth.connectService();
    }


    DialogInterface.OnClickListener dialogClickListenerForActivitySync = new DialogInterface.OnClickListener() {
        @Override
        public void onClick(DialogInterface dialog, int which) {
            switch (which) {
                case DialogInterface.BUTTON_POSITIVE:
                    //Yes button clicked


                    if (!Functions.isNullOrEmpty(stepdatakm.getText().toString()) && !Double.valueOf(stepdatakm.getText().toString()).equals(0.0)) {

                        int S_W_Minutes = Functions.pref.getInt(Functions.PREF_SC_Minutes, 0) + Functions.pref.getInt(Functions.PREF_WC_Minutes, 0);
                        AddWellnessActivityData("Shealth APP", 1, Double.valueOf(stepdatakm.getText().toString()), S_W_Minutes);
                    }

                    if (!Functions.isNullOrEmpty(Runningdatakm.getText().toString()) && !Double.valueOf(Runningdatakm.getText().toString()).equals(0.0)) {

                        int R_Minutes = Functions.pref.getInt(Functions.PREF_RC_Minutes, 0);
                        AddWellnessActivityData("Shealth APP", 2, Double.valueOf(Runningdatakm.getText().toString()), R_Minutes);
                    }
                    break;
                case DialogInterface.BUTTON_NEGATIVE:
                    //No button clicked
                    Functions.showToast(getActivity(), "SHealth APP Data Sync Cancelled.");
                    break;
            }
        }
    };

    //On success of Step 1.
    // Step 2. Connect to HealthDataStore
    private final HealthDataStore.ConnectionListener mConnectionListener = new HealthDataStore.ConnectionListener() {
        @Override
        public void onConnected() {
            Log.d(Functions.APP_TAG, "Health data service is connected.");
            HealthPermissionManager pmsManager = new HealthPermissionManager(Functions.mStoreHealth);

            mStepReporter = new StepDataCountReporter(Functions.mStoreHealth);
            mExerciseReporter = new ExerciseDataCountReporter(Functions.mStoreHealth);

            try {
                // Check whether the permissions that this application needs are acquired
                Map<PermissionKey, Boolean> resultMap = pmsManager.isPermissionAcquired(mKeySet);

                // Step 3. Check for permissions
                if (resultMap.containsValue(Boolean.FALSE)) {
                    // Request the permission for reading step counts if it is not acquired
                    pmsManager.requestPermissions(mKeySet, getActivity()).setResultListener(mPermissionListener);
                } else {
                    SharedPreferences.Editor editor = Functions.pref.edit();
                    editor.putBoolean(Functions.PREF_ShealthDisplay, true);
                    editor.commit();

                    mlvshealth.setVisibility(View.VISIBLE);

                    // Get the current step count and display it
                    // Step 4. Get Count
                    mStepReporter.start();
                    mExerciseReporter.start();

                }
            } catch (Exception e) {
                mlvshealth.setVisibility(View.GONE);

                Log.e(Functions.APP_TAG, e.getClass().getName() + " - " + e.getMessage());
                Log.e(Functions.APP_TAG, "Permission setting fails.");
            }
        }

        @Override
        public void onConnectionFailed(HealthConnectionErrorResult error) {
            Log.d(Functions.APP_TAG, "Health data service is not available.");
            showConnectionFailureDialog(error);
            mlvshealth.setVisibility(View.GONE);
        }

        @Override
        public void onDisconnected() {
            Log.d(Functions.APP_TAG, "Health data service is disconnected.");

            mlvshealth.setVisibility(View.GONE);

        }
    };

    private final HealthResultHolder.ResultListener<PermissionResult> mPermissionListener = new HealthResultHolder.ResultListener<PermissionResult>() {
        @Override
        public void onResult(PermissionResult result) {
            Log.d(Functions.APP_TAG, "Permission callback is received.");
            Map<PermissionKey, Boolean> resultMap = result.getResultMap();

            if (resultMap.containsValue(Boolean.FALSE)) {
                mlvshealth.setVisibility(View.GONE);
                drawSHealthDataCount();
                showPermissionAlarmDialog();
            } else {
                SharedPreferences.Editor editor = Functions.pref.edit();
                editor.putBoolean(Functions.PREF_ShealthDisplay, true);
                editor.commit();

                mlvshealth.setVisibility(View.VISIBLE);
                // Get the current step count and display it
                mStepReporter.start();
                mExerciseReporter.start();
            }
        }
    };


    private void showPermissionAlarmDialog() {
        if (getActivity().isFinishing()) {
            return;
        }

        AlertDialog.Builder alert = new AlertDialog.Builder(getActivity());
        alert.setTitle("Notice");
        alert.setMessage("All permissions should be acquired");
        alert.setPositiveButton("OK", null);
        alert.show();
    }

    private void showConnectionFailureDialog(HealthConnectionErrorResult error) {
        AlertDialog.Builder alert = new AlertDialog.Builder(getActivity());
        mConnError = error;
        String message = "Connection with S Health is not available";

        if (mConnError.hasResolution()) {
            switch (error.getErrorCode()) {
                case HealthConnectionErrorResult.CONNECTION_FAILURE:
                    break;
                case HealthConnectionErrorResult.PLATFORM_NOT_INSTALLED:
                    message = "Please install S Health";
                    break;
                case HealthConnectionErrorResult.OLD_VERSION_PLATFORM:
                    message = "Please upgrade S Health";
                    break;
                case HealthConnectionErrorResult.PLATFORM_DISABLED:
                    message = "Please enable S Health";
                    break;
                case HealthConnectionErrorResult.USER_AGREEMENT_NEEDED:
                    message = "Please agree with S Health policy";
                    break;
                default:
                    message = "Please make S Health available";
                    break;
            }
        }

        alert.setMessage(message);

        alert.setPositiveButton("OK", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int id) {
                if (mConnError.hasResolution()) {
                    mConnError.resolve(getActivity());
                }
            }
        });

        if (error.hasResolution()) {


            alert.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int id) {
                    // First Time Ask for Shealth in that case if user cancels Shealth then
                    // Set Prefrence as false;

                    SharedPreferences.Editor editor = Functions.pref.edit();
                    editor.putBoolean(Functions.PREF_ShealthDisplay, false);
                    editor.commit();


                }
            });

        }

        alert.show();
    }

    public static void ShowToast() {
        Functions.showToast(mContextToast, "Kindly, Enable Developer Mode of S Health APP");
        mlvshealth.setVisibility(View.GONE);
    }

    public static void drawSHealthDataCount() {
        SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
        String currentDate = sdf.format(new Date());

        float SDistance = Functions.pref.getFloat(Functions.PREF_SC, 0.0f);
        float WDistance = Functions.pref.getFloat(Functions.PREF_WC, 0.0f);
        float RDistance = Functions.pref.getFloat(Functions.PREF_RC, 0.0f);

        todaydate.setText(currentDate);


        //Updated

        stepdatakm.setText(String.format("%.3f", (SDistance / 1000.0f) + (WDistance / 1000.0f)).toString());

        // Display the today Running Distance

        //Updated

        Runningdatakm.setText(String.format("%.3f", (RDistance / 1000.0f)).toString());

        if ((!Functions.isNullOrEmpty(stepdatakm.getText().toString()) && !Double.valueOf(stepdatakm.getText().toString()).equals(0.0)) || (!Functions.isNullOrEmpty(Runningdatakm.getText().toString()) && !Double.valueOf(Runningdatakm.getText().toString()).equals(0.0))) {

            btn_SHealthSync.setVisibility(View.VISIBLE);
        } else {

            btn_SHealthSync.setVisibility(View.GONE);
        }

    }

    // Add Activities Data

    public void AddWellnessActivityData(String ACTIVITY_DATA_TYPE, final int ActivityID, Double Distance, int min) {
        if (ValidateActivityID(ActivityID) == true && Validatemin(min) == true && ValidateDistance(Distance) == true) {
            mSwipeRefreshLayout.setRefreshing(true);

            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            String currentDate = sdf.format(new Date());
            String Date_To_HH = Functions.DateToDateHH(currentDate);

            if (!Date_To_HH.equals("-1")) {

                Map<String, String> jsonParams = new HashMap<String, String>();


                jsonParams.put("ActivityId", String.valueOf(ActivityID));
                jsonParams.put("Distance", String.valueOf(Distance));
                jsonParams.put("CollectionDate", Date_To_HH);

                jsonParams.put("FinishTime", String.valueOf(min));
                jsonParams.put("PathName", ACTIVITY_DATA_TYPE); // Shealth
                jsonParams.put("Comments", "SHealth App Activity");
                jsonParams.put("CreatedDate", Date_To_HH);
                jsonParams.put("ModifiedDate", Date_To_HH);
                jsonParams.put("DeleteFlag", "false");
                jsonParams.put("SourceId", Functions.SourceID);
                jsonParams.put("UserId", Functions.ApplicationUserid);


                String Addactivity_URL = getString(R.string.urlLogin) + getString(R.string.AddActivitiesData);

                JsonObjectRequest postRequestWellnessActivity = new JsonObjectRequest(Request.Method.POST, Addactivity_URL,
                        new JSONObject(jsonParams),
                        new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                AfterPostWellnessActivity(response, ActivityID);
                            }
                        },
                        new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                mSwipeRefreshLayout.setRefreshing(false);
                                mProgressBarDashboardGraphs.setVisibility(View.GONE);
                                Functions.ErrorHandling(getActivity(), error);
                                // TODO Auto-generated method stub
                                Log.e("Dashboard Frame Error", error.toString());
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

                postRequestWellnessActivity.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
                // Access the RequestQueue through your singleton class.
                MySingleton.getInstance(getActivity()).addToRequestQueue(postRequestWellnessActivity);

            } else {
                Functions.showToast(getActivity(), "Invalid Date");
            }
        } else {
            return;
        }
    }

    private Boolean ValidateActivityID(int ActivityID) {
        if (ActivityID != 0) {
            return true;
        } else {
            Functions.showToast(getActivity(), "Invalid ActivityID");
            return false;
        }
    }

    private Boolean Validatemin(int min) {
        if (min >= 0) {
            return true;
        } else {
            Functions.showToast(getActivity(), "Invalid min");
            return false;
        }
    }

    private Boolean ValidateDistance(Double Distance) {
        if (!Distance.equals(0.0)) {
            return true;
        } else {
            Functions.showToast(getActivity(), "Invalid Distance");
            return false;
        }
    }


    private void AfterPostWellnessActivity(JSONObject response, int ActivityID) {
        ParseJson_WellnessActivitiesData addWellnessActivity_pj = new ParseJson_WellnessActivitiesData(response);
        String STATUS_Post = addWellnessActivity_pj.parsePostResponseWellnessActivities();

        mSwipeRefreshLayout.setRefreshing(false);

        switch (STATUS_Post) {
            case "1":
                //sucess
                if (ActivityID == 1) {
                    // Set Step Count and Walking 0.0, with minutes 0
                    SharedPreferences.Editor editor = Functions.pref.edit();
                    editor.putFloat(Functions.PREF_SC, 0.0f);
                    editor.putInt(Functions.PREF_SC_Minutes, 0);

                    editor.putFloat(Functions.PREF_WC, 0.0f);
                    editor.putInt(Functions.PREF_WC_Minutes, 0);
                    editor.commit();

                    mSwipeRefreshLayout.setRefreshing(true);
                    LoadDashboard(url_dashboard);
                }
                if (ActivityID == 2) {
                    // Set Running Count 0.0, with minutes 0
                    SharedPreferences.Editor editor = Functions.pref.edit();
                    editor.putFloat(Functions.PREF_RC, 0.0f);
                    editor.putInt(Functions.PREF_RC_Minutes, 0);
                    editor.commit();

                    mSwipeRefreshLayout.setRefreshing(true);
                    LoadDashboard(url_dashboard);
                }

                Functions.showToast(getActivity(), "Shealth Data Synced !!");
                Functions.SHealthDataCount();
                saveStartTimeasLastTimeSync();

                break;
            case "0":

                break;
            default:

                break;
        }
    }

// ************* Shealth End ************

    public void LoadDashboard(String urldash) {


        JsonObjectRequest jsDashboardRequest = new JsonObjectRequest(Request.Method.GET, urldash, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {
                LoadJSONDataDashboard(response);

            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                mSwipeRefreshLayout.setRefreshing(false);

                mProgressBarDashboardGraphs.setVisibility(View.GONE);

                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Dashboard Frame Error", error.toString());

                txtDashboardData.setVisibility(View.VISIBLE);
            }
        });

        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsDashboardRequest);


    }

    private void LoadJSONDataDashboard(JSONObject jsonData) {
        ParseJson_DashboardData pj_dashboard = new ParseJson_DashboardData(jsonData);
        String ST_BG = pj_dashboard.parseJsonBG();
        String ST_BP = pj_dashboard.parseJsonBP();
        String ST_AV = pj_dashboard.parseJsonAV();

        mProgressBarDashboardGraphs.setVisibility(View.GONE);

        if (ST_BG.equals("1") || ST_BP.equals("1") || ST_AV.equals("1")) {

            mSwipeRefreshLayout.setRefreshing(false);
            txtDashboardData.setVisibility(View.GONE);

            if (ST_AV.equals("1")) {
                mlvChartActivity.setVisibility(View.VISIBLE);
            } else {
                mlvChartActivity.setVisibility(View.GONE);
            }
            if (ST_BG.equals("1")) {
                mlvChartBG.setVisibility(View.VISIBLE);
            } else {
                mlvChartBG.setVisibility(View.GONE);
            }
            if (ST_BP.equals("1")) {
                mlvChartBP.setVisibility(View.VISIBLE);
            } else {
                mlvChartBP.setVisibility(View.GONE);
            }

            if (ST_AV.equals("1")) {
                AVPieChart();
            }
            if (ST_BG.equals("1")) {
                BGCHART();
            }
            if (ST_BP.equals("1")) {
                BPCHART();
            }
        } else {
            txtDashboardData.setVisibility(View.VISIBLE);
            mSwipeRefreshLayout.setRefreshing(false);

            mlvChartActivity.setVisibility(View.GONE);
            mlvChartBG.setVisibility(View.GONE);
            mlvChartBP.setVisibility(View.GONE);
        }

        LoadwellnessWeightData();



    }

    //Activity PIE CHARt
    private void AVPieChart() {
        mAV_PieChart.setBackgroundColor(Color.WHITE);
        mAV_PieChart.setUsePercentValues(true);
        mAV_PieChart.setDescription("");
        mAV_PieChart.setExtraOffsets(5, 10, 5, 5);
        mAV_PieChart.setDragDecelerationFrictionCoef(0.95f);
        mAV_PieChart.setCenterTextTypeface(Functions.mTfLight);

        mAV_PieChart.setExtraOffsets(20.f, 0.f, 20.f, 0.f);
        mAV_PieChart.setDrawHoleEnabled(true);
        mAV_PieChart.setHoleColor(Color.WHITE);
        mAV_PieChart.setTransparentCircleColor(Color.WHITE);
        mAV_PieChart.setTransparentCircleAlpha(110);
        mAV_PieChart.setHoleRadius(48f); //58f
        mAV_PieChart.setTransparentCircleRadius(61f);
        mAV_PieChart.setDrawCenterText(true);
        mAV_PieChart.setRotationAngle(0);

        // enable rotation of the chart by touch
        mAV_PieChart.setRotationEnabled(false);
        mAV_PieChart.setHighlightPerTapEnabled(true);



        // set a chart value selected listener this
        mAV_PieChart.setOnChartValueSelectedListener(new OnChartValueSelectedListener() {
            @Override
            public void onValueSelected(Entry e, Highlight h) {

                if (e == null) {
                    return;
                } else {

                    mAV_PieChart.setCenterText(generateCenterSpannableText_WithValue(String.format("%.2f", e.getY()).toString()));
                }

            }

            @Override
            public void onNothingSelected() {

                mAV_PieChart.setCenterText(generateCenterSpannableText());
            }

        });


        setPieChartData(); // number of parts chart have 4, total pie 100

        mAV_PieChart.animateY(1400, Easing.EasingOption.EaseInOutQuad);


        Legend l = mAV_PieChart.getLegend();
        l.setWordWrapEnabled(true);
        l.setTypeface(Functions.mTfLight);
        l.setTextSize(15f);
        l.setFormSize(12f); // set the size of the legend forms/shapes
        l.setForm(Legend.LegendForm.CIRCLE); // set what type of form/shape should be used
        l.setOrientation(Legend.LegendOrientation.HORIZONTAL);
        l.setPosition(Legend.LegendPosition.ABOVE_CHART_CENTER);

        // Addons
        l.setEnabled(true);

        // entry label styling
        mAV_PieChart.setEntryLabelColor(Color.BLACK);
        mAV_PieChart.setEntryLabelTypeface(Functions.mTfRegular);
        mAV_PieChart.setEntryLabelTextSize(12f);
        mAV_PieChart.setCenterText(generateCenterSpannableText());
    }


    private void setPieChartData() {
        //int count number of items, float range = 100

        float range = 100f;

        ArrayList<PieEntry> entries = new ArrayList<PieEntry>();

        // NOTE: The order of the entries when being added to the entries array determines
        // their position around the center of
        // the chart.
        //for (int i = 0; i < count; i++)
        // {
        //entries.add(new PieEntry((float) (Math.random() * range) + range / 5, mParties[i % mParties.length]));
        //}


//        Activity_DATA.put("Walking + Steps", WL);
//        Activity_DATA.put("Running", RN);
//        Activity_DATA.put("Cycling", CL);
//        Activity_DATA.put("Swimming", SW);


        for (LinkedHashMap.Entry<String, Float> entry : ParseJson_DashboardData.Activity_DATA.entrySet()) {
            //entries.add(new PieEntry(entry.getValue() * range + range / 5, entry.getKey()));
            // if(entry.getValue()!=0f)
            //{
            //    entries.add(new PieEntry(entry.getValue(), entry.getKey()));
            //}

            entries.add(new PieEntry(entry.getValue(), entry.getKey()));
        }


        PieDataSet dataSet = new PieDataSet(entries, "Activities");
        dataSet.setSliceSpace(3f);
        dataSet.setSelectionShift(5f);

        // add a lot of colors

        ArrayList<Integer> colors = new ArrayList<Integer>();

        for (int c : ColorTemplate.VORDIPLOM_COLORS)
            colors.add(c);

        for (int c : ColorTemplate.JOYFUL_COLORS)
            colors.add(c);

        for (int c : ColorTemplate.COLORFUL_COLORS)
            colors.add(c);

        for (int c : ColorTemplate.LIBERTY_COLORS)
            colors.add(c);

        for (int c : ColorTemplate.PASTEL_COLORS)
            colors.add(c);

        colors.add(ColorTemplate.getHoloBlue());

        dataSet.setColors(colors);
        //dataSet.setSelectionShift(0f);


        dataSet.setValueLinePart1OffsetPercentage(80.f);
        dataSet.setValueLinePart1Length(0.2f);
        dataSet.setValueLinePart2Length(0.4f);
        // dataSet.setXValuePosition(PieDataSet.ValuePosition.OUTSIDE_SLICE);
        dataSet.setYValuePosition(PieDataSet.ValuePosition.OUTSIDE_SLICE);

        PieData data = new PieData(dataSet);
        data.setValueFormatter(new PercentFormatter());
        data.setValueTextSize(11f);
        data.setValueTextColor(Color.BLACK);
        data.setValueTypeface(Functions.mTfRegular);
        mAV_PieChart.setData(data);

        // undo all highlights
        mAV_PieChart.highlightValues(null);

        mAV_PieChart.invalidate();
    }


    private SpannableString generateCenterSpannableText() {
        Float D = 0f;

        for (Map.Entry<String, Float> entry : ParseJson_DashboardData.Activity_DATA.entrySet()) {
            D += entry.getValue();
        }

        //Activities"\n Total Distance\n"D.toString()"KM\n(All Slices)"
        SpannableString s = new SpannableString("Activities" + "\n Total Distance\n" + String.format("%.2f", D).toString() + " KM\n(All Slices)");

        s.setSpan(new RelativeSizeSpan(1.5f), 0, 26, 0);
        s.setSpan(new StyleSpan(Typeface.NORMAL), 0, 26, 0);
        s.setSpan(new ForegroundColorSpan(Color.GRAY), 0, 26, 0);

        s.setSpan(new RelativeSizeSpan(.85f), 26, s.length(), 0);
        s.setSpan(new StyleSpan(Typeface.ITALIC), 26, s.length(), 0);


        return s;
    }

    private SpannableString generateCenterSpannableText_WithValue(String current) {
        SpannableString s = new SpannableString("Total Distance\n" + current + " KM");
        s.setSpan(new RelativeSizeSpan(1.5f), 0, 15, 0);
        s.setSpan(new StyleSpan(Typeface.NORMAL), 0, 15, 0);
        s.setSpan(new ForegroundColorSpan(Color.GRAY), 0, 15, 0);


        return s;
    }


    // BP CHART Combined Chart
    private void BPCHART() {

        mBP_CominedChart.setDescription("");
        mBP_CominedChart.setBackgroundColor(Color.WHITE);
        mBP_CominedChart.setDrawGridBackground(false);
        mBP_CominedChart.setDrawBarShadow(false);
        mBP_CominedChart.setHighlightFullBarEnabled(true);
        mBG_CominedChart.setDragEnabled(true);
        mBG_CominedChart.setScaleEnabled(true);
        mBG_CominedChart.setPinchZoom(true);

        // draw bars behind lines
        mBP_CominedChart.setDrawOrder(new CombinedChart.DrawOrder[]
                {
                        CombinedChart.DrawOrder.BAR, CombinedChart.DrawOrder.BUBBLE, CombinedChart.DrawOrder.CANDLE, CombinedChart.DrawOrder.LINE, CombinedChart.DrawOrder.SCATTER
                });


        AxisValueFormatter xaxisformatterDateBP = new AxisValueFormatter() {
            @Override
            public String getFormattedValue(float value, AxisBase axis) {
                //String dt = ParseJson_DashboardData.BP_Date[(int) value % ParseJson_DashboardData.BP_Date.length];
                return ParseJson_DashboardData.BP_Date[(int) value % ParseJson_DashboardData.BP_Date.length];
            }

            // we don't draw numbers, so no decimal digits needed
            @Override
            public int getDecimalDigits() {
                return 0;
            }
        };

        XAxis xAxis = mBP_CominedChart.getXAxis();
        // x values top bottom

        xAxis.setPosition(XAxis.XAxisPosition.BOTTOM);
        xAxis.setTypeface(Functions.mTfLight);
        xAxis.setDrawGridLines(false);
        xAxis.setAxisMinValue(0f);
        xAxis.setGranularity(1f);
        xAxis.setValueFormatter(xaxisformatterDateBP);


        YAxis leftAxis = mBP_CominedChart.getAxisLeft();
        leftAxis.setDrawGridLines(false);
        leftAxis.setTypeface(Functions.mTfLight);
        leftAxis.setAxisMinValue(0f); // PHRMS_Dashboard_Fragment.this replaces setStartAtZero(true)


        Legend l = mBP_CominedChart.getLegend();
        l.setWordWrapEnabled(true);
        l.setTypeface(Functions.mTfLight);
        l.setTextSize(15f);
        l.setFormSize(12f); // set the size of the legend forms/shapes
        l.setOrientation(Legend.LegendOrientation.HORIZONTAL);
        l.setPosition(Legend.LegendPosition.ABOVE_CHART_CENTER);

        // Addons
        l.setEnabled(true);

        CombinedData data = new CombinedData();

        data.setData(generateBarData_BP());
        data.setData(generateLineData_BP());
        data.setValueTypeface(Functions.mTfLight);

        xAxis.setAxisMaxValue(data.getXMax() + 0.25f);


        mBP_CominedChart.setData(data);
        mBP_CominedChart.invalidate();

    }


    private LineData generateLineData_BP() {

        LineData d = new LineData();

        ArrayList<Entry> entries = new ArrayList<Entry>();

        for (int index = 0; index < ParseJson_DashboardData.BP_PULSE_Value.length; index++) {
            entries.add(new Entry(index + 0.5f, Float.parseFloat(ParseJson_DashboardData.BP_PULSE_Value[index])));
        }

        LineDataSet set = new LineDataSet(entries, "Pulse");
        set.setColor(Color.rgb(240, 238, 70));
        set.setLineWidth(2.5f);
        set.setCircleColor(Color.rgb(240, 238, 70));
        set.setCircleRadius(5f);
        set.setFillColor(Color.rgb(240, 238, 70));
        set.setMode(LineDataSet.Mode.LINEAR);
        set.setDrawValues(true);
        set.setValueTextSize(11f);
        set.setValueTextColor(Color.BLACK); //Color.rgb(240, 238, 70)

        set.setAxisDependency(YAxis.AxisDependency.LEFT);
        d.addDataSet(set);

        return d;
    }

    private BarData generateBarData_BP() {

        ArrayList<BarEntry> Sys_entries = new ArrayList<BarEntry>();
        ArrayList<BarEntry> Dia_entries = new ArrayList<BarEntry>();

        for (int index = 0; index < ParseJson_DashboardData.BP_SYS_Value.length; index++) {
            Sys_entries.add(new BarEntry(index, Float.parseFloat(ParseJson_DashboardData.BP_SYS_Value[index])));
            // stacked

            Dia_entries.add(new BarEntry(index, Float.parseFloat(ParseJson_DashboardData.BP_DIA_Value[index])));
        }

        // Systolic Blue - Color.rgb(0, 117, 194)
        // Diastolic Green - Color.rgb(26, 175, 93)
        BarDataSet set_sys = new BarDataSet(Sys_entries, "SYSTOLIC");
        set_sys.setColor(Color.rgb(0, 117, 194));
        set_sys.setValueTextColor(Color.rgb(0, 117, 194));
        set_sys.setValueTextSize(10f);
        set_sys.setAxisDependency(YAxis.AxisDependency.LEFT);

        BarDataSet set_dia = new BarDataSet(Dia_entries, "DIASTOLIC");
        //set2.setStackLabels(new String[]{"Stack 1", "Stack 2"});
        //set2.setColors(new int[]{Color.rgb(61, 165, 255), Color.rgb(23, 197, 255)});
        set_dia.setColor(Color.rgb(26, 175, 93));
        set_dia.setValueTextColor(Color.rgb(26, 175, 93));
        set_dia.setValueTextSize(10f);
        set_dia.setAxisDependency(YAxis.AxisDependency.LEFT);

        float groupSpace = 0.06f;
        float barSpace = 0.02f; // x2 dataset
        float barWidth = 0.45f; // x2 dataset
        // (0.45 + 0.02) * 2 + 0.06 = 1.00 -> interval per "group"

        BarData d = new BarData(set_sys, set_dia);
        d.setBarWidth(barWidth);

        // make this BarData object grouped
        d.groupBars(0, groupSpace, barSpace); // start at x = 0

        return d;
    }


    //Blood Glusoce Line Chart
    private void BGCHART() {

        mBG_CominedChart.setDescription("");
        mBG_CominedChart.setBackgroundColor(Color.WHITE);
        mBG_CominedChart.setDrawBarShadow(false);
        mBG_CominedChart.setHighlightFullBarEnabled(false);
        mBG_CominedChart.setHighlightPerDragEnabled(true);
        mBG_CominedChart.setDragEnabled(true);
        mBG_CominedChart.setScaleEnabled(true);
        mBG_CominedChart.setPinchZoom(true);
        mBG_CominedChart.setDrawBarShadow(false);


        // draw bars behind lines
        mBG_CominedChart.setDrawOrder(new CombinedChart.DrawOrder[]
                {
                        CombinedChart.DrawOrder.BAR, CombinedChart.DrawOrder.BUBBLE, CombinedChart.DrawOrder.CANDLE, CombinedChart.DrawOrder.LINE, CombinedChart.DrawOrder.SCATTER
                });

        // get the legend (only possible after setting data)
        Legend l = mBG_CominedChart.getLegend();
        l.setWordWrapEnabled(true);
        l.setTypeface(Functions.mTfLight);
        l.setTextSize(15f);
        l.setFormSize(12f); // set the size of the legend forms/shapes
        l.setForm(Legend.LegendForm.LINE); // set what type of form/shape should be used
        l.setOrientation(Legend.LegendOrientation.HORIZONTAL);
        l.setPosition(Legend.LegendPosition.ABOVE_CHART_CENTER);

        // Addons
        l.setEnabled(true);



        YAxis leftAxis = mBG_CominedChart.getAxisLeft();
        leftAxis.setDrawGridLines(false);
        leftAxis.setAxisMinValue(0f); // this replaces setStartAtZero(true)

        XAxis xAxis = mBG_CominedChart.getXAxis();

        xAxis.setPosition(XAxis.XAxisPosition.BOTTOM);
        xAxis.setAxisMinValue(0f);
        xAxis.setGranularity(1f);


        AxisValueFormatter formatter = new AxisValueFormatter() {

            @Override
            public String getFormattedValue(float value, AxisBase axis) {

                return ParseJson_DashboardData.BG_Date[(int) value % ParseJson_DashboardData.BG_Date.length];
            }

            // we don't draw numbers, so no decimal digits needed
            @Override
            public int getDecimalDigits() {
                return 0;
            }
        };
        xAxis.setValueFormatter(formatter);


        LimitLine ll2 = new LimitLine(80f, "Lower Limit");
        ll2.setLineWidth(4f);
        ll2.enableDashedLine(10f, 10f, 0f);
        ll2.setLabelPosition(LimitLine.LimitLabelPosition.RIGHT_BOTTOM);
        ll2.setTextSize(10f);
        ll2.setTypeface(Functions.mTfRegular);

        LimitLine ll1 = new LimitLine(150f, "Upper Limit");
        ll1.setLineWidth(4f);
        ll1.enableDashedLine(10f, 10f, 0f);
        ll1.setLabelPosition(LimitLine.LimitLabelPosition.RIGHT_TOP);
        ll1.setTextSize(10f);
        ll1.setTypeface(Functions.mTfRegular);


        leftAxis.removeAllLimitLines(); // reset all limit lines to avoid overlapping lines
        leftAxis.addLimitLine(ll1);
        leftAxis.addLimitLine(ll2);
        leftAxis.setDrawZeroLine(true);


        CombinedData data = new CombinedData();

        data.setData(generateLineData_BG());
        data.setValueTypeface(Functions.mTfLight);

        xAxis.setAxisMaxValue(data.getXMax() + 0.25f);

        mBG_CominedChart.setData(data);

        mBG_CominedChart.getAxisRight().setEnabled(false);
        mBG_CominedChart.invalidate();

    }


    private LineData generateLineData_BG() {

        LineData d = new LineData();

        ArrayList<Entry> entries = new ArrayList<Entry>();

        for (int index = 0; index < ParseJson_DashboardData.BG_Value.length; index++) {
            entries.add(new Entry(index, Float.parseFloat(ParseJson_DashboardData.BG_Value[index])));
        }

        LineDataSet set = new LineDataSet(entries, "Blood Glucose");
        //set.setColor(Color.rgb(240, 238, 70));
        //set.setLineWidth(2.5f);
        //set.setCircleColor(Color.rgb(240, 238, 70));
        //set.setCircleRadius(5f);
        //set.setFillColor(Color.rgb(240, 238, 70));
        set.setMode(LineDataSet.Mode.LINEAR); //HORIZONTAL_BEZIER
        set.setDrawValues(true);
        set.setValueTextSize(10f);
        set.setValueTextColor(Color.BLACK); //Color.rgb(240, 238, 70));
        set.setValueTextSize(11f);

        set.setAxisDependency(YAxis.AxisDependency.LEFT);
        set.setColor(ColorTemplate.getHoloBlue());
        set.setCircleColor(ColorTemplate.getHoloBlue());
        set.setLineWidth(2.5f);
        set.setCircleRadius(5f);
        set.setFillAlpha(65);
        set.setFillColor(ColorTemplate.getHoloBlue());
        set.setHighLightColor(Color.rgb(244, 117, 117));
        set.setDrawCircleHole(true);
        //set1.setFillFormatter(new MyFillFormatter(0f));
        set.setDrawHorizontalHighlightIndicator(false);
        //set.setVisible(false);

        //set.setAxisDependency(YAxis.AxisDependency.LEFT);
        d.addDataSet(set);
        //d.setValueTextColor(ColorTemplate.getHoloBlue());
        //d.setValueTextColor(Color.BLACK);
        //d.setValueTextSize(11f);


        return d;
    }


    // BMI CHART CODE
    ///Using Same bmi datalist and creating sperate async task

    public void LoadwellnessWeightData() {

        // To be changed with Ascending data as Web : Changes - LoadWeightDataAsc


        String urlBMI = getString(R.string.urlLogin) + getString(R.string.LoadWeightDataAsc) + Functions.ApplicationUserid;



        final JsonObjectRequest jsObjRequestBMI = new JsonObjectRequest(Request.Method.GET, urlBMI, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonDatabmi) {
                LoadwellnessWeightJSONData(jsonDatabmi);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {

                //Functions.showProgress(false, mProgressBarBMI);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Weight Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequestBMI);
    }

    private void LoadwellnessWeightJSONData(JSONObject jsonDatabmi) {
        // Class to parse data and load in data arrays
        ParseJson_DashboardData wellnessWeight_pj = new ParseJson_DashboardData(jsonDatabmi);
        String STATUS = wellnessWeight_pj.parseJsonBMIForDashboard();

        if (STATUS.equals("1")) {
            mSwipeRefreshLayout.setRefreshing(false);
            txtDashboardData.setVisibility(View.GONE);
            mlvChartBMI.setVisibility(View.VISIBLE);

            BMICHART();
        } else {
            mlvChartBMI.setVisibility(View.GONE);
        }
    }


    //Blood Glusoce Line Chart
    private void BMICHART() {

        mBMI_CominedChart.setDescription("");
        mBMI_CominedChart.setBackgroundColor(Color.WHITE);
        mBMI_CominedChart.setDrawBarShadow(false);
        mBMI_CominedChart.setHighlightFullBarEnabled(false);
        mBMI_CominedChart.setHighlightPerDragEnabled(true);
        mBMI_CominedChart.setDragEnabled(true);
        mBMI_CominedChart.setScaleEnabled(true);
        mBMI_CominedChart.setPinchZoom(true);
        mBMI_CominedChart.setDrawBarShadow(false);


        // draw bars behind lines
        mBMI_CominedChart.setDrawOrder(new CombinedChart.DrawOrder[]
                {
                        CombinedChart.DrawOrder.BAR, CombinedChart.DrawOrder.BUBBLE, CombinedChart.DrawOrder.CANDLE, CombinedChart.DrawOrder.LINE, CombinedChart.DrawOrder.SCATTER
                });

        // get the legend (only possible after setting data)
        Legend l = mBMI_CominedChart.getLegend();
        l.setWordWrapEnabled(true);
        l.setTypeface(Functions.mTfLight);
        l.setTextSize(15f);
        l.setFormSize(12f); // set the size of the legend forms/shapes
        l.setForm(Legend.LegendForm.LINE); // set what type of form/shape should be used
        l.setOrientation(Legend.LegendOrientation.HORIZONTAL);
        l.setPosition(Legend.LegendPosition.ABOVE_CHART_CENTER);

        // Addons
        l.setEnabled(true);

        YAxis leftAxis = mBMI_CominedChart.getAxisLeft();
        leftAxis.setDrawGridLines(false);
        leftAxis.setAxisMinValue(0f); // this replaces setStartAtZero(true)

        XAxis xAxis = mBMI_CominedChart.getXAxis();
        // x values top bottom
        //xAxis.setPosition(XAxis.XAxisPosition.BOTH_SIDED);
        xAxis.setPosition(XAxis.XAxisPosition.BOTTOM);
        xAxis.setAxisMinValue(0f);
        xAxis.setGranularity(1f);

        AxisValueFormatter formatter = new AxisValueFormatter() {

            @Override
            public String getFormattedValue(float value, AxisBase axis) {
                // return ParseJson_DashboardData.BMI_Date[(int) value];
                return ParseJson_DashboardData.BMI_Date[(int) value % ParseJson_DashboardData.BMI_Date.length];
            }

            // we don't draw numbers, so no decimal digits needed
            @Override
            public int getDecimalDigits() {
                return 0;
            }
        };
        xAxis.setValueFormatter(formatter);


        leftAxis.removeAllLimitLines(); // reset all limit lines to avoid overlapping lines
        leftAxis.setDrawZeroLine(true);

        CombinedData data = new CombinedData();

        data.setData(generateLineData_BMI());
        data.setValueTypeface(Functions.mTfLight);

        xAxis.setAxisMaxValue(data.getXMax() + 0.25f);

        mBMI_CominedChart.setData(data);

        mBMI_CominedChart.getAxisRight().setEnabled(false);
        mBMI_CominedChart.invalidate();

    }


    private LineData generateLineData_BMI() {

        LineData d = new LineData();

        ArrayList<Entry> entries = new ArrayList<Entry>();

        for (int index = 0; index < ParseJson_DashboardData.BMI_Value.length; index++) {
            entries.add(new Entry(index, Float.parseFloat(ParseJson_DashboardData.BMI_Value[index])));
        }

        LineDataSet set = new LineDataSet(entries, "BMI");
        set.setDrawValues(true);
        set.setValueTextSize(10f);
        set.setValueTextColor(Color.BLACK);//Color.rgb(240, 238, 70));
        set.setValueTextSize(11f);

        set.setAxisDependency(YAxis.AxisDependency.LEFT);
        set.setColor(Color.rgb(255, 208, 140));
        set.setCircleColor(Color.rgb(255, 208, 140));
        set.setLineWidth(2.5f);
        set.setCircleRadius(5f);
        set.setFillAlpha(65);
        set.setFillColor(ColorTemplate.getHoloBlue());
        set.setHighLightColor(Color.rgb(244, 117, 117));
        set.setDrawCircleHole(true);

        set.setDrawHorizontalHighlightIndicator(false);



        d.addDataSet(set);

        return d;
    }

    public void LoadHeaderContentDashboard(final Context cnt) {
        if (Functions.isNetworkAvailable(cnt)) {
            // Userid based image path is required to fetch new image without login
            // Currently only login based service will provide image path
            String user_GUID = Functions.decrypt(getActivity(), Functions.pref.getString(Functions.P_UsrID, null));
            String loggedout = Functions.pref.getString(Functions.P_Lgout, "yes"); // By default logged out - yes
            if (user_GUID != null && loggedout.equals("no")) {
                // Send hash u_pass from session
                mAuthHeaderTask = new UserLoginLoadHeaderContentTask(user_GUID);
                mAuthHeaderTask.execute((Void) null);
            } else {
                String ProfileImagePath = Functions.pref.getString(Functions.P_Img, null);
                if (!Functions.isNullOrEmpty(ProfileImagePath)) {
                    String url_image = cnt.getString(R.string.ImagePathProfile) + ProfileImagePath;
                    LoadImageFromGlideWithProgressBarDashboard(url_image, cnt, "Profile");
                }
            }
        } else {
            Functions.showToast(cnt, "Internet Not Available !!");
        }
    }

    /**
     * Represents an asynchronous login/registration task used to authenticate
     * the user.
     */
    public class UserLoginLoadHeaderContentTask extends AsyncTask<Void, Void, Integer> {

        private final String mUSER_GUID;


        UserLoginLoadHeaderContentTask(String user_guid) {
            mUSER_GUID = user_guid;

        }

        @Override
        protected void onPreExecute() {
            Functions.mLockScreenRotation(getActivity());
            mSwipeRefreshLayout.setRefreshing(true);
        }

        @Override
        protected Integer doInBackground(Void... params) {
            // TODO: attempt authentication against a network service.
            return userCheckLoad(mUSER_GUID);
        }

        @Override
        protected void onPostExecute(final Integer success) {
            mAuthHeaderTask = null;
            Functions.enable_orientation(getActivity());
            mSwipeRefreshLayout.setRefreshing(false);

            switch (success) {
                case 0:
                    // success
                    textUsernameDashboard.setText(Functions.pref.getString(Functions.P_NAME, ""));

                    String ProfileImagePath = Functions.pref.getString(Functions.P_Img, null);

                    if (!Functions.isNullOrEmpty(ProfileImagePath)) {
                        String url_image = getActivity().getString(R.string.ImagePathProfile) + ProfileImagePath;
                        LoadImageFromGlideWithProgressBarDashboard(url_image, getActivity(), "Profile");
                        // Update Navigation User Name or Email - as changed
                        Functions.LoadHeaderContent(getActivity());
                    }
                    break;
                default:
                    textUsernameDashboard.setText(Functions.pref.getString(Functions.P_NAME, ""));

                    String ProfileImage = Functions.pref.getString(Functions.P_Img, null);

                    if (!Functions.isNullOrEmpty(ProfileImage)) {
                        String url_image = getActivity().getString(R.string.ImagePathProfile) + ProfileImage;
                        LoadImageFromGlideWithProgressBarDashboard(url_image, getActivity(), "Profile");
                        // Update Navigation User Name or Email - as changed
                        Functions.LoadHeaderContent(getActivity());
                    }

                    break;
            }
        }

        @Override
        protected void onCancelled() {
            mAuthHeaderTask = null;
            //showProgress(false);
            mSwipeRefreshLayout.setRefreshing(false);
        }

        public int userCheckLoad(String user_guid)
        {
            int i = -99;
            // If Hash u_pass generated then call the web api -- if -7 the error occured while generating hashpassword
            if (!Functions.isNullOrEmpty(user_guid)) {
                try {
                    // to get latest name , image and aadhar of user if updated
                    JSONObject jSonData = HttpUrlConnectionRequest.SendJsonHttpUrlGetRequest(getString(R.string.urlLogin) + getString(R.string.ImageUserProfile) + user_guid);

                    if (jSonData != null) {


                        if (jSonData.getString("status").equals("0")) {
                            i = -1; // no data available
                        } else {
                            JSONObject Json_response = new JSONObject(jSonData.getString("response"));

                            if (Json_response != null) {
                                SharedPreferences.Editor editor = Functions.pref.edit();
                                String aadhaar_user = null;
                                if (!Functions.isNullOrEmpty(Json_response.getString("AadhaarNo"))) {
                                    aadhaar_user = Json_response.getString("AadhaarNo");
                                }
                                editor.putString(Functions.P_AdhrN, Functions.isNullOrEmpty(aadhaar_user.toString()) ? null : Functions.encrypt(getActivity(), aadhaar_user.toString()));


                                String profile_name = null;
                                if (!Functions.isNullOrEmpty(Json_response.getString("FirstName")) && !Functions.isNullOrEmpty(Json_response.getString("LastName"))) {
                                    profile_name = Json_response.getString("FirstName") + " " + Json_response.getString("LastName");
                                }
                                editor.putString(Functions.P_NAME, profile_name);


                                if (!Functions.isNullOrEmpty(Json_response.getString("ImgPath")) && Json_response.getString("ImgPath").startsWith("\\")) {
                                    editor.putString(Functions.P_Img, Functions.isNullOrEmpty(Json_response.getString("ImgPath")) ? null : Json_response.getString("ImgPath").replace("\\", "//"));
                                } else {
                                    //---- Actual File Path

                                    editor.putString(Functions.P_Img, Functions.isNullOrEmpty(Json_response.getString("ImgPath")) ? null : Json_response.getString("ImgPath"));
                                }

                                editor.apply();
                                //editor.commit();
                                i = 0;

                            } else {
                                i = -2; // User Profile data empty
                            }
                        }
                    } else {
                        i = -3; // Request result not available
                    }
                } catch (ClientProtocolException e) {
                    // TODO Auto-generated catch block
                    // dialog.dismiss();
                    e.printStackTrace();
                    i = -4;
                } catch (IOException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                    i = -5;
                } catch (JSONException e) {
                    // TODO Auto-generated catch block
                    //errormsg = e.getMessage();
                    e.printStackTrace();
                    i = -6;
                }
            } else {
                // Invalid hash u_pass generated
                i = -7;
            }

            return i;
        }

    }


    public void LoadImageFromGlideWithProgressBarDashboard(final String url, final Context cnt, final String imgType) {

        if (url.endsWith(".gif")) {

            Glide.with(cnt.getApplicationContext())
                    .load(url)
                    .asGif()
                    .listener(new RequestListener<String, GifDrawable>() {
                        @Override
                        public boolean onException(Exception e, String model, Target<GifDrawable> target, boolean isFirstResource) {
                            pbrUser.setVisibility(View.GONE);
                            return false;
                        }

                        @Override
                        public boolean onResourceReady(GifDrawable resource, String model, Target<GifDrawable> target, boolean isFromMemoryCache, boolean isFirstResource) {
                            pbrUser.setVisibility(View.GONE);
                            return false;
                        }
                    })
                    .error(R.drawable.user_light)

                    .diskCacheStrategy(DiskCacheStrategy.RESULT)
                    .skipMemoryCache(false)
                    .crossFade()

                    .into(imgview);
        } else {

            Glide.with(cnt.getApplicationContext())
                    .load(url)
                    .listener(new RequestListener<String, GlideDrawable>() {
                        @Override
                        public boolean onException(Exception e, String model, Target<GlideDrawable> target, boolean isFirstResource) {
                            pbrUser.setVisibility(View.GONE);
                            return false;
                        }

                        @Override
                        public boolean onResourceReady(GlideDrawable resource, String model, Target<GlideDrawable> target, boolean isFromMemoryCache, boolean isFirstResource) {
                            pbrUser.setVisibility(View.GONE);
                            return false;
                        }
                    })
                    .error(R.drawable.user_light)

                    .diskCacheStrategy(DiskCacheStrategy.RESULT)
                    .skipMemoryCache(false)
                    .crossFade()

                    .into(imgview);
        }



    }


    // Menu System for shealth also

    public void onCreate(Bundle savedInstanceState) {
        setHasOptionsMenu(true);
        // to show toast for Shealth Developer Mode
        mContextToast = getActivity();

        super.onCreate(savedInstanceState);
    }


    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        // Do something that differs the Activity's menu here
        super.onCreateOptionsMenu(menu, inflater);

        MenuItem item = menu.findItem(R.id.action_share);
        item.setVisible(false);

        // 1. Condition - To Check API Level 19 Andorid KitKat 4.6 for sHealth
        // ShealthErrorCode = -1 // means error // Shealth Not Supported
        // ShealthErrorCode = 0 // means Success Shealth

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT && ShealthErrorCode == 0) {
            menu.add(1, MENU_ITEM_PERMISSION_SETTING, 0, "Connect to S Health");
        }
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle item selection
        switch (item.getItemId()) {
            case R.id.action_logout:
                DialogueLogout();
                return true;
            case R.id.action_share:
                btn_share();
                return true;
            case MENU_ITEM_PERMISSION_SETTING:

                //1. Check Shealth Display - check if user have selected not to display shealth
                //2. Now all permissions will be inoked again.
                Boolean ShealthDisplay = Functions.pref.getBoolean(Functions.PREF_ShealthDisplay, true);
                if (!ShealthDisplay) {
                    CallShealthSettingsandServices();
                } else {
                    HealthPermissionManager pmsManager = new HealthPermissionManager(Functions.mStoreHealth);
                    try {
                        // Show user permission UI for allowing user to change options
                        pmsManager.requestPermissions(mKeySet, getActivity()).setResultListener(mPermissionListener);
                    } catch (Exception e) {
                        Log.e(Functions.APP_TAG, e.getClass().getName() + " - " + e.getMessage());
                        Log.e(Functions.APP_TAG, "Permission setting fails.");
                    }
                }
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    public void DialogueLogout() {
        android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(getActivity());
        builder.setTitle("MyHealthRecord Logout");
        builder.setMessage("Do you really want to logout?").setPositiveButton("Yes", dialogClickListenerForScan)
                .setNegativeButton("No", dialogClickListenerForScan).show();
    }

    DialogInterface.OnClickListener dialogClickListenerForScan = new DialogInterface.OnClickListener() {
        @Override
        public void onClick(DialogInterface dialog, int which) {
            switch (which) {
                case DialogInterface.BUTTON_POSITIVE:
                    //Yes button clicked
                    btn_logout();
                    break;
                case DialogInterface.BUTTON_NEGATIVE:
                    //No button clicked
                    break;
            }
        }
    };


    public void btn_logout() {
        // Send Value to main activity for logout
        SharedPreferences.Editor editor = Functions.pref.edit();
        editor.clear();
        editor.apply();
        //editor.commit();
        // Call to Login Screen
        Intent LoginScreen = new Intent(getActivity(), PHRMS_LoginActivity.class);
        LoginScreen.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        getActivity().finish();
        this.startActivity(LoginScreen);
    }

    public void btn_share() {
        // Get Navigation View Control
        final Menu menuNav = Functions.navigationView.getMenu();

        mfragment = new PHRMS_ReportSharing_Fragment();

        if (mfragment != null) {
            if (Functions.isNavDrawerOpen()) {
                Functions.closeNavDrawer();
            }

            // Custom Check Navigation View
            MenuItem activitiesItem = menuNav.findItem(R.id.nav_reportsharing);

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
    }

}


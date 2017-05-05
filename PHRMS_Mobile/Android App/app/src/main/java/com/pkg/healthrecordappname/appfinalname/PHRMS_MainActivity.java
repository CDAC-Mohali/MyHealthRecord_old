package com.pkg.healthrecordappname.appfinalname;

import android.app.FragmentManager;
import android.content.Context;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.support.design.widget.NavigationView;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.RelativeLayout;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.google.android.gms.appindexing.Action;
import com.google.android.gms.appindexing.AppIndex;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.firebase.iid.FirebaseInstanceId;
import com.google.firebase.messaging.FirebaseMessaging;
import com.pkg.healthrecordappname.appfinalname.modules.appconfig.Config;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Dashboard_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_ProfileInfo_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_DeviceRegistrationData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;

import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;


public class PHRMS_MainActivity extends Functions {

    private GoogleApiClient client;

    // FCM
    private static final String TAG = PHRMS_LoginActivity.class.getSimpleName();


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_phrms_main);

        // Drawer
        mdrawer = (DrawerLayout) findViewById(R.id.drawer_layout);


        //toolbar
        mtoolbar = (Toolbar) findViewById(R.id.toolbar_allfragments);
        setSupportActionBar(mtoolbar);

        // Declared in Functions and // Initialize NavigationView Here
        navigationView = (NavigationView) findViewById(R.id.nav_view);

        // Initialize setNavigationItemSelected Listener for NavigationView
        navigationView.setNavigationItemSelectedListener(this);
        navigationView.setItemIconTintList(null);
        //// Declared in Functions Defined here Update Headerlayout
        headerLayout = navigationView.getHeaderView(0);


        mActionBarDrawerToggle = new ActionBarDrawerToggle(this, mdrawer, mtoolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close) {
            @Override
            public void onDrawerOpened(View drawerView) {
                super.onDrawerOpened(drawerView);
                // code here will execute once the drawer is opened( As I dont want anything happened whe drawer is
                // open I am not going to put anything here)
            }

            @Override
            public void onDrawerClosed(View drawerView) {
                super.onDrawerClosed(drawerView);
                // Code here will execute once drawer is closed
            }

            @Override
            public void onDrawerStateChanged(int newState) {

            }
        }; // Drawer Toggle Object Made

        assert mdrawer != null;
        if (mdrawer != null) {
            mdrawer.setDrawerListener(mActionBarDrawerToggle);
        }
        mActionBarDrawerToggle.syncState();



        Functions.LoadHeaderContent(getApplicationContext());

        final Menu menuNav = navigationView.getMenu();

        // When fragment is called for the first time
        if (savedInstanceState == null) {

            // Deafult View is frame_dashboard
            mfragment = new PHRMS_Dashboard_Fragment();
            if (mfragment != null) {
                // Custom Check Navigation View
                MenuItem homeItem = menuNav.findItem(R.id.nav_dashboard);
                navigationView.setCheckedItem(homeItem.getItemId());


                mfragmentManager = getFragmentManager();
                mfragmentManager.beginTransaction().replace(R.id.content_frame, mfragment, PHRMS_FRAGMENT).commit();
                //navigationView.getMenu().getItem(0).setChecked(true);
                mfragmentManager
                        .addOnBackStackChangedListener(new FragmentManager.OnBackStackChangedListener() {
                            @Override
                            public void onBackStackChanged() {
                                if (getFragmentManager().getBackStackEntryCount() == 0) {
                                    // Back button from main activity will finish the application context
                                    finish();
                                } else if (getFragmentManager().getBackStackEntryCount() > 0) {
                                    mfragmentManager.popBackStack();
                                }
                            }
                        });
            } else {
                // error in creating fragment
                Log.e("MainDashboard Activity", "Error in creating fragment");
            }
        } else {
            // do nothing - fragment is recreated automatically - When fragment is recreated on orientation changes or something
            // The Activity IS being re-created so we don't need to instantiate the Fragment or add it,
            // but if we need a reference to it, we can use the tag we passed to .replace
            mfragment = mfragmentManager.findFragmentByTag(PHRMS_FRAGMENT);
        }

        // Load Profile When clicked Navigation View - User's Profile Image Area
        RelativeLayout rvUserProfileCall = (RelativeLayout) headerLayout.findViewById(R.id.rvUserProfileCall);
        rvUserProfileCall.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                mfragment = new PHRMS_ProfileInfo_Fragment();

                if (mfragment != null) {
                    if (isNavDrawerOpen()) {
                        closeNavDrawer();
                    }

                    // Custom Check Navigation View
                    MenuItem profileItem = menuNav.findItem(R.id.nav_profile);
                    navigationView.setCheckedItem(profileItem.getItemId());

                    mfragmentManager = getFragmentManager();

                    mfragmentManager.beginTransaction().replace(R.id.content_frame, mfragment, PHRMS_FRAGMENT)
                            .commit();

                    mfragmentManager
                            .addOnBackStackChangedListener(new FragmentManager.OnBackStackChangedListener() {
                                @Override
                                public void onBackStackChanged() {
                                    if (getFragmentManager().getBackStackEntryCount() == 0) {
                                        finish();
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
        });

        sendRegistrationToServer();

        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        client = new GoogleApiClient.Builder(this).addApi(AppIndex.API).build();
    }


    public void sendRegistrationToServer() {
        SharedPreferences pref_token = getApplicationContext().getSharedPreferences(Config.SHARED_PREF, 0);
        Boolean Token_Sent_Status = pref_token.getBoolean("tksntservre", false);

        // If Token Sent Status is False - send data to server
        if (!Token_Sent_Status) {
            String tkfcmid = pref_token.getString("tkfcmid", null);

            if (Functions.isNullOrEmpty(tkfcmid)) {

                tkfcmid = FirebaseInstanceId.getInstance().getToken();

                storeRegIdInPref(getApplicationContext(), tkfcmid);
            }

            if (!Functions.isNullOrEmpty(ApplicationUserid) && !Functions.isNullOrEmpty(tkfcmid)) {
                if (Functions.isNetworkAvailable(getApplicationContext()))
                {
                    DeviceRegisterationToAppServer(getApplicationContext(), tkfcmid);
                } else {
                    Functions.showToast(getApplicationContext(), "Internet not available !!");
                }
            } else {
                Log.d(TAGFB, "No UserId/RegID Available: ");
                return;
            }
        } else {
            Log.d(TAGFB, "Token Already Sent");
            return;
        }
    }


    private void DeviceRegisterationToAppServer(Context cnt, String tkfcmid) {

        Map<String, String> jsonParams = new HashMap<String, String>();
        jsonParams.put("tokenID", tkfcmid);
        jsonParams.put("userID", ApplicationUserid);
        jsonParams.put("SourceID", SourceID);

        String DeviceRegURL = getString(R.string.urlLogin) + getString(R.string.DeviceRegisterURL);

        JsonObjectRequest postRequestDeviceRegisteration = new JsonObjectRequest(Request.Method.POST, DeviceRegURL,
                new JSONObject(jsonParams),
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        AfterPostDeviceRegisteration(response);
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
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

        postRequestDeviceRegisteration.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getApplicationContext()).addToRequestQueue(postRequestDeviceRegisteration);
    }

    private void AfterPostDeviceRegisteration(JSONObject response) {
        ParseJson_DeviceRegistrationData _pj = new ParseJson_DeviceRegistrationData(response);
        String STATUS_Post = _pj.parsePostResponseDeviceRegisteration();

        switch (STATUS_Post) {
            case "1":

                SharedPreferences pref_token = getApplicationContext().getSharedPreferences(Config.SHARED_PREF, 0);
                //Now Send Data to Server and update prefrences
                SharedPreferences.Editor editor = pref_token.edit();
                editor.putBoolean("tksntservre", true);
                editor.commit();

                FirebaseMessaging.getInstance().subscribeToTopic(Config.TOPIC_GLOBAL);
                Functions.displayFirebaseRegId(getApplicationContext());

                Log.d(TAGFB, "tksntservre:  " + pref_token.getString("tkfcmid", null));
                break;
            default:
                Functions.showToast(getApplicationContext(), "Device Registration not done on MyHealthRecord App Server");
                break;
        }
    }




    @Override
    public void onStart() {
        super.onStart();

        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        client.connect();
        Action viewAction = Action.newAction(
                Action.TYPE_VIEW, // TODO: choose an action type.
                "PHRMS_Main Page", // TODO: Define a title for the content shown.
                // TODO: If you have web page content that matches this app activity's content,
                // make sure this auto-generated web page URL is correct.
                // Otherwise, set the URL to null.
                Uri.parse("http://host/path"),
                // TODO: Make sure this auto-generated app URL is correct.
                Uri.parse("android-app://com.pkg.healthrecordappname.appfinalname/http/host/path")
        );
        AppIndex.AppIndexApi.start(client, viewAction);
    }

    @Override
    public void onStop() {
        super.onStop();

        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        Action viewAction = Action.newAction(
                Action.TYPE_VIEW, // TODO: choose an action type.
                "PHRMS_Main Page", // TODO: Define a title for the content shown.
                // TODO: If you have web page content that matches this app activity's content,
                // make sure this auto-generated web page URL is correct.
                // Otherwise, set the URL to null.
                Uri.parse("http://host/path"),
                // TODO: Make sure this auto-generated app URL is correct.
                Uri.parse("android-app://com.pkg.healthrecordappname.appfinalname/http/host/path")
        );
        AppIndex.AppIndexApi.end(client, viewAction);
        client.disconnect();
    }


}


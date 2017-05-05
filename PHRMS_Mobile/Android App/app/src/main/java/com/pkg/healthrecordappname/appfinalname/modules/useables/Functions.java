package com.pkg.healthrecordappname.appfinalname.modules.useables;

import android.Manifest;
import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.app.Activity;
import android.app.Dialog;
import android.app.Fragment;
import android.app.FragmentManager;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.content.pm.ActivityInfo;
import android.content.pm.PackageManager;
import android.content.res.Configuration;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.PorterDuff;
import android.graphics.Typeface;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.provider.Settings;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.NavigationView;
import android.support.design.widget.Snackbar;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.LocalBroadcastManager;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.text.TextUtils;
import android.util.Base64;
import android.util.Log;
import android.view.Gravity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewGroup.LayoutParams;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.view.animation.Transformation;
import android.view.inputmethod.InputMethodManager;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.AuthFailureError;
import com.android.volley.DefaultRetryPolicy;
import com.android.volley.NetworkError;
import com.android.volley.NetworkResponse;
import com.android.volley.NoConnectionError;
import com.android.volley.ParseError;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.ServerError;
import com.android.volley.TimeoutError;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.ImageLoader;
import com.android.volley.toolbox.JsonObjectRequest;
import com.bumptech.glide.Glide;
import com.bumptech.glide.load.engine.DiskCacheStrategy;
import com.bumptech.glide.load.resource.drawable.GlideDrawable;
import com.bumptech.glide.load.resource.gif.GifDrawable;
import com.bumptech.glide.request.RequestListener;
import com.bumptech.glide.request.target.Target;
import com.google.android.gms.appindexing.Action;
import com.google.android.gms.appindexing.AppIndex;
import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.GooglePlayServicesUtil;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.firebase.messaging.FirebaseMessaging;
import com.pkg.healthrecordappname.appfinalname.PHRMS_LoginActivity;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.appconfig.Config;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_About_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Allergies_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Dashboard_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_EmergencyInfo_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_EmployerInfo_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Immunization_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_InsuranceInfo_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_LabTests_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_MedicalContact_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Medication_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_PrefrenceInfo_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Prescription_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Problems_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_Procedures_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_ProfileInfo_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_ReportSharing_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_WellnessActivities_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_WellnessBP_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_WellnessGlucose_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.fragments.PHRMS_WellnessWeight_Fragment;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_DeviceRegistrationData;
import com.pkg.healthrecordappname.appfinalname.modules.util.NotificationUtils;
import com.pkg.healthrecordappname.appfinalname.modules.util.WakeLocker;
import com.samsung.android.sdk.healthdata.HealthDataStore;

import org.json.JSONObject;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.text.DateFormat;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.spec.PBEParameterSpec;

import de.hdodenhof.circleimageview.CircleImageView;

public class Functions extends AppCompatActivity implements NavigationView.OnNavigationItemSelectedListener {
    protected static DrawerLayout mdrawer;
    protected static ActionBarDrawerToggle mActionBarDrawerToggle;
    protected static Toolbar mtoolbar;

    public static NavigationView navigationView;
    protected static View headerLayout;

    public static String PHRMS_FRAGMENT = "PHRMS_Fragment";
    public static FragmentManager mfragmentManager;
    public static Fragment mfragment = null;
    protected static ImageLoader mImageLoader;

    Context context;

    public static Typeface mTfRegular;
    public static Typeface mTfLight;
    /**
     * ATTENTION: This was auto-generated to implement the App Indexing API.
     * See https://g.co/AppIndexing/AndroidStudio for more information.
     */
    protected GoogleApiClient client;
    public static SharedPreferences pref;

    public static final String PREFS_NAME = "PHRMS_PrefsFile";
    // private static final String TAG = "network settings";

    // Encryted
    public static final String P_UNAME = "uname";
    public static final String P_PASS = "pwd";
    public static final String P_UsrID = "UserID";

    //=====
    public static final String P_NAME = "profile_name";
    public static final String P_Email = "profile_email";

    // Encryted
    public static final String P_AdhrN = "AdhrN";

    //===
    public static final String P_Mobile = "MobileNo";
    public static final String P_RoleID = "role_id";
    public static final String P_Img = "ImgPath";
    public static final String P_Lgout = "logout";

    protected static final String UTF8 = "utf-8";
    private static final char[] SEKRIT = {'a', 'b'};
    public static final String SourceID = "4";
    public static String ApplicationUserid = "";
    public static String IE_NotAvailable = "Internet Not Available!! Please Try Again.";


    // Regx for form validations
    public static final String OnlyAlphabetWithSpace_Format = "^[a-zA-Z\\s]+$";


    private static final String EXTERNAL_STORAGE_PERMISSION = "android.permission.WRITE_EXTERNAL_STORAGE";

    private static final String CAMERA_PERMISSION = "android.permission.CAMERA";

    private static final String READ_CONTACTS_PERMISSION = "android.permission.READ_CONTACTS";


    public static final int DEFAULT_TIMEOUT_MS = 20000;

    public static final int DEFAULT_MAX_RETRIES = 0;

    // To Hide EMR Application Data from app set value to false
    public static final boolean emrData = false;

    // Addition of Menus
    //Shealth Integration
    public static final String PREF_SC = "stepcount";
    public static final String PREF_SC_Minutes = "stepcount_minutes";

    public static final String PREF_WC = "walking";
    public static final String PREF_WC_Minutes = "walking_minutes";

    public static final String PREF_RC = "running";
    public static final String PREF_RC_Minutes = "running_minutes";

    public static final String APP_TAG = "SimpleHealth";
    public static final int MENU_ITEM_PERMISSION_SETTING = 1;
    public static final String PREF_LastTimeSync = "LastSyncTime";

    public static HealthDataStore mStoreHealth;

    public static final String PREF_ShealthDisplay = "DisplaySHealth";

    public static final int MENU_ITEM_MEDICAL_CONTACTS = 2;
    public static final int MENU_ITEM_REPORT_SHARING = 3;

    //Tag for firebase
    public static final String TAGFB = "MyHealthRecord FB:";


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        // ATTENTION: This was auto-generated to implement the App Indexing API.

        client = new GoogleApiClient.Builder(this).addApi(AppIndex.API).build();
        ApplicationUserid = Functions.decrypt(getApplicationContext(), Functions.pref.getString(Functions.P_UsrID, null));

        mTfRegular = Typeface.createFromAsset(getAssets(), "fonts/OpenSans-Regular.ttf");
        mTfLight = Typeface.createFromAsset(getAssets(), "fonts/OpenSans-Light.ttf");

    }

    public static void mainscreen(Context cnt) {
        Activity activity = (Activity) cnt;
        Intent intent = new Intent(cnt, PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        activity.finish();
        cnt.startActivity(intent);
    }

    public static void LoadHeaderContent(final Context cnt) {
        // only create fragment if activity is started for the first time
        String userid = Functions.decrypt(cnt.getApplicationContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (!Functions.isNullOrEmpty(userid)) {
            ((TextView) headerLayout.findViewById(R.id.textUsername)).setText(Functions.pref.getString(Functions.P_NAME, ""));


            String ProfileImagePath = Functions.pref.getString(Functions.P_Img, null);



            if (!Functions.isNullOrEmpty(ProfileImagePath)) {

                String url = cnt.getString(R.string.ImagePathProfile) + ProfileImagePath;
                LoadImageFromGlideWithProgressBar(url, cnt, "Profile");
            }



        }
    }

    public static void LoadHeaderTextContent(final Context cnt) {
        // only create fragment if activity is started for the first time
        String userid = Functions.decrypt(cnt.getApplicationContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (!Functions.isNullOrEmpty(userid)) {
            ((TextView) headerLayout.findViewById(R.id.textUsername)).setText(Functions.pref.getString(Functions.P_NAME, ""));

        }
    }


    public static void LoadImageFromGlideWithProgressBar(final String url, final Context cnt, final String imgType) {


        final CircleImageView imgview = (CircleImageView) headerLayout.findViewById(R.id.imageUser);

        final ProgressBar pbrUser = (ProgressBar) headerLayout.findViewById(R.id.ProgressBarUserImage);


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
                    //.listener(new LoggingListener<String, GlideDrawable>())
                    .diskCacheStrategy(DiskCacheStrategy.NONE)
                    .skipMemoryCache(true)
                    .crossFade()
                    // No transformation for gif
                    .into(imgview);
        } else {
            //imgview.setScaleType(ImageView.ScaleType.CENTER_CROP);
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
                    //.listener(new LoggingListener<String, GlideDrawable>())
                    .diskCacheStrategy(DiskCacheStrategy.NONE)
                    .skipMemoryCache(true)
                    .crossFade()
                    //.bitmapTransform(new CropCircleTransformation(cnt.getApplicationContext()))
                    .into(imgview);
        }


        imgview.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (Functions.isNetworkAvailable(cnt)) {
                    Intent intImageview = new Intent(cnt.getApplicationContext(), ImageFullScreenProfile.class).addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                    intImageview.putExtra("imageurl", url);
                    intImageview.putExtra("imageType", imgType);
                    cnt.startActivity(intImageview);
                } else {
                    Functions.showToast(cnt, "Internet Not Available !!");
                }
            }
        });

    }


    public static void DispImageFromGlideWithProgressBar(final String url, final Context cnt, ImageView imgview, final ProgressBar prb, final String imgType) {
        if (url.endsWith(".gif")) {
            //imgview.setScaleType(ImageView.ScaleType.CENTER_CROP);
            Glide.with(cnt.getApplicationContext())
                    .load(url)
                    .asGif()
                    .thumbnail(0.5f)
                    //.placeholder(R.drawable.ic_image_black_48dp) // replace with spinner
                    .listener(new RequestListener<String, GifDrawable>() {
                        @Override
                        public boolean onException(Exception e, String model, Target<GifDrawable> target, boolean isFirstResource) {
                            prb.setVisibility(View.GONE);
                            return false;
                        }

                        @Override
                        public boolean onResourceReady(GifDrawable resource, String model, Target<GifDrawable> target, boolean isFromMemoryCache, boolean isFirstResource) {
                            prb.setVisibility(View.GONE);
                            return false;
                        }
                    })
                    .error(R.drawable.ic_image_black_48dp)
                    //.listener(new LoggingListener<String, GlideDrawable>())
                    .diskCacheStrategy(DiskCacheStrategy.NONE)
                    .skipMemoryCache(true)
                    .crossFade()
                    .into(imgview);

        } else {

            Glide.with(cnt.getApplicationContext())
                    .load(url)
                    .thumbnail(0.5f)
                    .listener(new RequestListener<String, GlideDrawable>() {
                        @Override
                        public boolean onException(Exception e, String model, Target<GlideDrawable> target, boolean isFirstResource) {
                            prb.setVisibility(View.GONE);
                            return false;
                        }

                        @Override
                        public boolean onResourceReady(GlideDrawable resource, String model, Target<GlideDrawable> target, boolean isFromMemoryCache, boolean isFirstResource) {
                            prb.setVisibility(View.GONE);
                            return false;
                        }
                    })
                    .error(R.drawable.ic_image_black_48dp)
                    .diskCacheStrategy(DiskCacheStrategy.NONE)
                    .skipMemoryCache(true)
                    .crossFade()
                    .into(imgview);
        }

        imgview.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                if (Functions.isNetworkAvailable(cnt)) {
                    Intent intImageview = new Intent(cnt.getApplicationContext(), ImageFullScreen.class).addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                    intImageview.putExtra("imageurl", url);
                    intImageview.putExtra("imageType", imgType);
                    cnt.startActivity(intImageview);
                } else {
                    Functions.showToast(cnt, "Internet Not Available !!");
                }
            }
        });
    }


    public static void ErrorHandling(Context cnt, VolleyError error) {
        NetworkResponse response = error.networkResponse;

        if (response != null && response.data != null) {
            switch (response.statusCode) {
                case 400:
                    showToast(cnt.getApplicationContext(), response.data.toString());
                    break;
                case 404:
                    showToast(cnt.getApplicationContext(), "Content not available on Web Server");
                    break;
            }
        }

        //Additional cases
        if (error instanceof TimeoutError || error instanceof NoConnectionError)
        {
            showToast(cnt.getApplicationContext(), "Internet not available, Request Timeout. Check your internet and try again");
        }
        else if (error instanceof AuthFailureError)
        {
            showToast(cnt.getApplicationContext(), "Auth");
            //TODO
        }
        else if (error instanceof ServerError)
        {
            showToast(cnt.getApplicationContext(), "ServerErr");
            //TODO
        }
        else if (error instanceof NetworkError)
        {
            showToast(cnt.getApplicationContext(), "NetworkErr");
            //TODO
        }
        else if (error instanceof ParseError)
        {
            showToast(cnt.getApplicationContext(), "ParseErr");
            //TODO
        }
    }

    public static void showProgress(final boolean show, final ProgressBar mProgressView) {
        // On Honeycomb MR2 we have the ViewPropertyAnimator APIs, which allow
        // for very easy animations. If available, use these APIs to fade-in
        // the progress spinner.
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB_MR2) {
            int shortAnimTime = 200; //getResources().getInteger(android.R.integer.config_shortAnimTime);

            mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);

            mProgressView.animate().setDuration(shortAnimTime).alpha(
                    show ? 1 : 0).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
                }
            });
        } else {
            // The ViewPropertyAnimator APIs are not available, so simply show
            // and hide the relevant UI components.
            mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
        }
    }

    public static void ControlVisibility(final boolean show, final View mView) {
        mView.setVisibility(show ? View.VISIBLE : View.GONE);
    }

    public static void mLockScreenRotation(Context context) {
        // Stop the screen orientation changing during an event
        switch (context.getResources().getConfiguration().orientation) {
            case Configuration.ORIENTATION_PORTRAIT:
                ((Activity) context).setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);
                break;
            case Configuration.ORIENTATION_LANDSCAPE:
                ((Activity) context).setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
                break;
        }
    }

    public static void enable_orientation(Context context) {
        ((Activity) context).setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED);
    }

    public static String DateToDateHH(String dateStr) {
        //String formattedDate = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(Calendar.getInstance().getTime());

        DateFormat fromFormat = new SimpleDateFormat("dd/MM/yyyy");
        fromFormat.setLenient(false);

        DateFormat toFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
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

    // menu For Navigation Bar

    public boolean onNavigationItemSelected(MenuItem item) {

        // set the toolbar title
        if (getSupportActionBar() != null) {
        }

        //Closing drawer on item click
        if (mdrawer != null) {
            mdrawer.closeDrawer(GravityCompat.START);
        }


        //Check to see which item was being clicked and perform appropriate action

        switch (item.getItemId()) {
            case android.R.id.home:
                Toast.makeText(getApplicationContext(), "Back button clicked", Toast.LENGTH_SHORT).show();
                mfragment = new PHRMS_Dashboard_Fragment();
                break;
            case R.id.nav_dashboard:
                mfragment = new PHRMS_Dashboard_Fragment();
                break;
            case R.id.nav_profile:
                mfragment = new PHRMS_ProfileInfo_Fragment();
                break;
            case R.id.nav_emergencyinfo:
                mfragment = new PHRMS_EmergencyInfo_Fragment();
                break;
            case R.id.nav_empinfo:
                mfragment = new PHRMS_EmployerInfo_Fragment();
                break;
            case R.id.nav_insuranceinfo:
                mfragment = new PHRMS_InsuranceInfo_Fragment();
                break;
            case R.id.nav_prefinfo:
                mfragment = new PHRMS_PrefrenceInfo_Fragment();
                break;
            case R.id.nav_allergies:
                mfragment = new PHRMS_Allergies_Fragment();
                break;
            case R.id.nav_problems:
                mfragment = new PHRMS_Problems_Fragment();
                break;
            case R.id.nav_Immunization:
                mfragment = new PHRMS_Immunization_Fragment();
                break;
            case R.id.nav_procedures:
                mfragment = new PHRMS_Procedures_Fragment();
                break;
            case R.id.nav_labtests:
                mfragment = new PHRMS_LabTests_Fragment();
                break;
            case R.id.nav_prescription:
                mfragment = new PHRMS_Prescription_Fragment();
                break;
            case R.id.nav_medication:
                mfragment = new PHRMS_Medication_Fragment();
                break;
            case R.id.nav_activities:
                mfragment = new PHRMS_WellnessActivities_Fragment();
                break;
            case R.id.nav_bloodpressure:
                mfragment = new PHRMS_WellnessBP_Fragment();
                break;
            case R.id.nav_bloodglucose:
                mfragment = new PHRMS_WellnessGlucose_Fragment();
                break;
            case R.id.nav_weight:
                mfragment = new PHRMS_WellnessWeight_Fragment();
                break;
            case R.id.nav_addmedicalcontact:
                mfragment = new PHRMS_MedicalContact_Fragment();
                break;
            case R.id.nav_reportsharing:
                mfragment = new PHRMS_ReportSharing_Fragment();
                break;
            case R.id.nav_about:
                mfragment = new PHRMS_About_Fragment();
                break;
            case R.id.nav_logout:
                DialogueLogout();
                break;
            default:
        }

        // Insert the fragment by replacing any existing fragment

        if (mfragment != null) {

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


        return true;


    }



    // Menus for action bar common
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.phrms_actionbar_main, menu);
        return true;
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
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    @Override
    public void onBackPressed() {
        if (isNavDrawerOpen()) {
            closeNavDrawer();
        } else {
            super.onBackPressed();
        }
    }

    public static boolean isNavDrawerOpen() {
        return mdrawer != null && mdrawer.isDrawerOpen(GravityCompat.START);
    }

    public static void closeNavDrawer() {
        if (mdrawer != null) {
            mdrawer.closeDrawer(GravityCompat.START);
        }
    }

    public void DialogueLogout() {
        android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(this);
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

    public static void hideKeyboard(Activity activity) {
        InputMethodManager imm = (InputMethodManager) activity.getSystemService(Activity.INPUT_METHOD_SERVICE);
        //Find the currently focused view, so we can grab the correct window token from it.
        View view = activity.getCurrentFocus();
        //If no view currently has focus, create a new one, just so we can grab a window token from it
        if (view == null) {
            view = new View(activity);
        }
        imm.hideSoftInputFromWindow(view.getWindowToken(), 0);
    }


    // Code for Button Logout Updated for FCM deregister from server
    public void btn_logout()
    {
        // Send Value to main activity for logout
        context = this;

        SharedPreferences pref = context.getSharedPreferences(Config.SHARED_PREF, Context.MODE_PRIVATE);
        String tkfcmid = pref.getString("tkfcmid", null);

        if (!isNullOrEmpty(tkfcmid))
        {
            Map<String, String> jsonParams = new HashMap<String, String>();
            jsonParams.put("tokenID", tkfcmid);
            jsonParams.put("userID", ApplicationUserid);
            jsonParams.put("SourceID", SourceID);

            String deviceUnRegisterURL = getString(R.string.urlLogin) + getString(R.string.DeviceUnRegisterURL);

            JsonObjectRequest postRequestDeviceUnRegisteration = new JsonObjectRequest(Request.Method.POST, deviceUnRegisterURL,
                    new JSONObject(jsonParams),
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            // Result from Server :: After deleting FCM Details
                            AfterPostUnregisterDevice(response);
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error)
                        {
                            Functions.ErrorHandling(getApplicationContext(), error);
                            DeleteAllPrefrences();
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

            postRequestDeviceUnRegisteration.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

            // Access the RequestQueue through your singleton class.
            MySingleton.getInstance(getApplicationContext()).addToRequestQueue(postRequestDeviceUnRegisteration);
        } else {
            DeleteAllPrefrences();
        }
    }

    private void DeleteAllPrefrences()
    {
        context = this;

        // Delete Application Prefrence
        SharedPreferences sharedpreferences = getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedpreferences.edit();
        editor.clear();
        editor.apply();

        FirebaseMessaging.getInstance().unsubscribeFromTopic(Config.TOPIC_GLOBAL);

        SharedPreferences fcmpreferences = getSharedPreferences(Config.SHARED_PREF, Context.MODE_PRIVATE);
        SharedPreferences.Editor editorfcm = fcmpreferences.edit();
        editorfcm.clear();
        editorfcm.apply();
        Log.d("Topic Unsubscribed", Config.TOPIC_GLOBAL);


        // Call to Login Screen
        Intent LoginScreen = new Intent(this, PHRMS_LoginActivity.class);
        LoginScreen.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        finish();
        this.startActivity(LoginScreen);
    }


    private void AfterPostUnregisterDevice(JSONObject response) {
        ParseJson_DeviceRegistrationData _pj = new ParseJson_DeviceRegistrationData(response);
        String STATUS_Post = _pj.parsePostResponseDeviceRegisteration();

        switch (STATUS_Post) {
            case "1":
                DeleteAllPrefrences();
                break;
            default:
                DeleteAllPrefrences();
                break;
        }
    }


    public void btn_share() {
        final Menu menuNav = navigationView.getMenu();
        mfragment = new PHRMS_ReportSharing_Fragment();
        if (mfragment != null) {
            if (isNavDrawerOpen()) {
                closeNavDrawer();
            }

            // Custom Check Navigation View
            MenuItem activitiesItem = menuNav.findItem(R.id.nav_reportsharing);

            navigationView.setCheckedItem(activitiesItem.getItemId());

            mfragmentManager = getFragmentManager();

            mfragmentManager.beginTransaction().replace(R.id.content_frame, mfragment, PHRMS_FRAGMENT).commit();

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



    public static String bitmapToBase64(Bitmap bm) {
        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        bm.compress(Bitmap.CompressFormat.JPEG, 100, baos);
        byte[] byteArrayImage = baos.toByteArray();

        return Base64.encodeToString(byteArrayImage, Base64.DEFAULT); //encodedImage;
    }

    // Base64 String to Image
    public static Bitmap base64ToBitmap(String encodedImage) {
        byte[] decodedString = Base64.decode(encodedImage, Base64.DEFAULT);

        return BitmapFactory.decodeByteArray(decodedString, 0, decodedString.length); //decodedByte;
    }

    // Any File to Base64
    public static void ANyFile() {
        File dir = Environment.getExternalStorageDirectory();
        File yourFile = new File(dir, "path/to/the/file/inside/the/sdcard.ext");
        try {
            String encodeFileToBase64Binary = encodeFileToBase64Binary(yourFile);
        } catch (IOException e) {
            // e.printStackTrace();
            Log.e("File conversion", e.toString());
        }
    }

    //
    private static String encodeFileToBase64Binary(File fileName) throws IOException {

        return "-1"; //encodedString;
    }

    public static boolean isNullOrEmpty(String s) {
        boolean result = s == null || s.trim().isEmpty() || s.equals("null");
        return result;
    }

    private boolean isValidMail(String email) {
        return android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches();
    }

    private boolean isValidMobile(String phone2) {
        boolean check = false;
        if (!Pattern.matches("[a-zA-Z]+", phone2)) {
            check = !(phone2.length() < 6 || phone2.length() > 13);
        } else {
            check = false;
        }
        return check;
    }

    public static void showToast(Context cnt, CharSequence msg) {
        Toast toast = Toast.makeText(cnt, msg, Toast.LENGTH_LONG);
        toast.setGravity(Gravity.CENTER, 0, 0);
        toast.show();
    }

    public static void showSnackbar(View cl, CharSequence msg, CharSequence Act) {
        Snackbar sk = Snackbar.make(cl, msg, Snackbar.LENGTH_SHORT);
        if (Act.equals("UNDO")) {
            sk.setAction("UNDO", new View.OnClickListener() {
                @Override
                public void onClick(View view) {

                }
            });
        }
        if (Act.equals("RETRY")) {
            sk.setAction("RETRY", new View.OnClickListener() {
                @Override
                public void onClick(View view) {

                }
            });
        }
        if (Act.equals("Action")) {
            sk.setAction(Act, null);
        }
        sk.show();
    }


    public static String encrypt(Context cnt, String value) {
        try
        {
            return "your encryption code";
        }
        catch (Exception e)
        {
            return "-1";
        }
    }

    public static String decrypt(Context cnt, String value) {
        try {

            return "your decryption code";

        } catch (Exception e) {
            //throw new RuntimeException(e);
            return "-1";
        }
    }

    public static void expand(final View v) {
        v.measure(LayoutParams.FILL_PARENT, LayoutParams.WRAP_CONTENT);
        final int targtetHeight = v.getMeasuredHeight();

        v.getLayoutParams().height = 0;
        v.setVisibility(View.VISIBLE);
        Animation a = new Animation() {
            @Override
            protected void applyTransformation(float interpolatedTime, Transformation t) {
                v.getLayoutParams().height = interpolatedTime == 1 ? LayoutParams.WRAP_CONTENT : (int) (targtetHeight * interpolatedTime);
                v.requestLayout();
            }

            @Override
            public boolean willChangeBounds() {
                return true;
            }
        };

        // 1dp/ms
        a.setDuration((int) (targtetHeight / v.getContext().getResources().getDisplayMetrics().density));
        v.startAnimation(a);
    }

    public static void collapse(final View v) {
        final int initialHeight = v.getMeasuredHeight();

        Animation a = new Animation() {
            @Override
            protected void applyTransformation(float interpolatedTime,
                                               Transformation t) {
                if (interpolatedTime == 1) {
                    v.setVisibility(View.GONE);
                } else {
                    v.getLayoutParams().height = initialHeight
                            - (int) (initialHeight * interpolatedTime);
                    v.requestLayout();
                }
            }

            @Override
            public boolean willChangeBounds() {
                return true;
            }
        };

        // 1dp/ms
        a.setDuration((int) (initialHeight / v.getContext().getResources()
                .getDisplayMetrics().density));
        v.startAnimation(a);
    }

    public static boolean isNetworkAvailable(Context cnt) {


        ConnectivityManager connectivity = (ConnectivityManager) cnt.getSystemService(Context.CONNECTIVITY_SERVICE);
        if (connectivity != null) {
            NetworkInfo activeNetwork = connectivity.getActiveNetworkInfo();
            if (activeNetwork != null && activeNetwork.isConnected()) {   // connected to the internet
                if (activeNetwork.getType() == ConnectivityManager.TYPE_WIFI) {
                    // connected to wifi

                } else if (activeNetwork.getType() == ConnectivityManager.TYPE_MOBILE) {
                    // connected to the mobile provider's data plan

                }
                return true;
            } else {
                // not connected to the internet
                showToast(cnt, "Internet Not Available !!");
                return false;
            }
        } else {
            showToast(cnt, "Network Not Available !!");
            return false;
        }
    }

    // If build less than lolipop change progressbar color
    public static void progressbarStyle(ProgressBar mPBR, Context context) {
        if (Build.VERSION.SDK_INT < 21) {
            mPBR.getIndeterminateDrawable().setColorFilter(Functions.getColor(context, R.color.colorAccent), PorterDuff.Mode.MULTIPLY);
        }
    }

    // if build less than marshmallow get color from contextcompat
    public static final int getColor(Context context, int id) {
        final int version = Build.VERSION.SDK_INT;
        if (version >= 23) {
            return ContextCompat.getColor(context, id);
        } else {
            return context.getResources().getColor(id);
        }
    }

    //Floating Button Transitions

    public static void FloatTransitions(Activity act, RecyclerView mRecyclerView, final FloatingActionButton fab) {

        final Animation Fab_fadOut = AnimationUtils.loadAnimation(act, android.R.anim.fade_out);
        final Animation Fab_fadIn = AnimationUtils.loadAnimation(act, android.R.anim.fade_in);

        mRecyclerView.addOnScrollListener(new RecyclerView.OnScrollListener() {
            @Override
            public void onScrolled(RecyclerView recyclerView, int dx, int dy) {
                super.onScrolled(recyclerView, dx, dy);
                if (dy > 0 && fab.isShown()) {
                    fab.startAnimation(Fab_fadIn);

                }
                if (dy < 0 && !fab.isShown()) {
                    fab.startAnimation(Fab_fadOut);

                }
            }
        });

    }

    // ================================
    // Code for granting RunTime Permissions on android 23(Marshmallow) Devices
    //===================================

    public static boolean hasExternalStoragePermission(Context context) {
        int extperm = context.checkCallingOrSelfPermission(EXTERNAL_STORAGE_PERMISSION);
        return extperm == PackageManager.PERMISSION_GRANTED;
    }

    public static boolean hasCameraPermission(Context context) {
        int camperm = context.checkCallingOrSelfPermission(CAMERA_PERMISSION);
        return camperm == PackageManager.PERMISSION_GRANTED;
    }

    public static boolean hasContactsReadPermission(Context context) {
        int contactperm = context.checkCallingOrSelfPermission(READ_CONTACTS_PERMISSION);
        return contactperm == PackageManager.PERMISSION_GRANTED;
    }

    // Permission check Camera , External Storage
    public static void checkpermissions(Context context, final Activity activity, final int PERMISSION_REQUEST_CODE_MULTIPLE) {
        List<String> permissionsNeeded = new ArrayList<String>();

        final List<String> permissionsList = new ArrayList<String>();

        if (!addPermission(context, activity, permissionsList, Manifest.permission.WRITE_EXTERNAL_STORAGE))
            permissionsNeeded.add("EXTERNAL STORAGE");
        if (!addPermission(context, activity, permissionsList, Manifest.permission.CAMERA))
            permissionsNeeded.add("CAMERA");


        if (permissionsList.size() > 0) {
            if (permissionsNeeded.size() > 0) {
                // Need Rationale
                String message = "You need to grant access to " + permissionsNeeded.get(0);
                for (int i = 1; i < permissionsNeeded.size(); i++)
                    message = message + ", " + permissionsNeeded.get(i);
                showMessageOKCancel(message, activity,
                        new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                ActivityCompat.requestPermissions(activity, permissionsList.toArray(new String[permissionsList.size()]), PERMISSION_REQUEST_CODE_MULTIPLE);
                            }
                        });
                return;
            }


            ActivityCompat.requestPermissions(activity, permissionsList.toArray(new String[permissionsList.size()]), PERMISSION_REQUEST_CODE_MULTIPLE);

            return;
        }


    }

//Persmission request external storage only
    public static void checkExternalpermissions(Context context, final Activity activity, final int PERMISSION_REQUEST_CODE_MULTIPLE) {
        List<String> permissionsNeeded = new ArrayList<String>();

        final List<String> permissionsList = new ArrayList<String>();

        if (!addPermission(context, activity, permissionsList, Manifest.permission.WRITE_EXTERNAL_STORAGE))
            permissionsNeeded.add("EXTERNAL STORAGE");


        if (permissionsList.size() > 0) {
            if (permissionsNeeded.size() > 0) {
                // Need Rationale
                String message = "You need to grant access to " + permissionsNeeded.get(0);
                for (int i = 1; i < permissionsNeeded.size(); i++)
                    message = message + ", " + permissionsNeeded.get(i);
                showMessageOKCancel(message, activity,
                        new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                ActivityCompat.requestPermissions(activity, permissionsList.toArray(new String[permissionsList.size()]), PERMISSION_REQUEST_CODE_MULTIPLE);
                            }
                        });
                return;
            }


            ActivityCompat.requestPermissions(activity, permissionsList.toArray(new String[permissionsList.size()]), PERMISSION_REQUEST_CODE_MULTIPLE);

            return;
        }


    }

    // Contacts Permission
    public static void checkcontactspermissions(Context context, final Activity activity, final int PERMISSION_REQUEST_CODE_CONTACT)
    {
        List<String> permissionsNeeded = new ArrayList<String>();

        final List<String> permissionsList = new ArrayList<String>();

        if (!addPermission(context, activity, permissionsList, Manifest.permission.READ_CONTACTS))
            permissionsNeeded.add("Contacts");


        if (permissionsList.size() > 0) {
            if (permissionsNeeded.size() > 0) {
                // Need Rationale
                String message = "You need to grant access to " + permissionsNeeded.get(0);
                for (int i = 1; i < permissionsNeeded.size(); i++)
                    message = message + ", " + permissionsNeeded.get(i);
                showMessageOKCancel(message, activity,
                        new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                ActivityCompat.requestPermissions(activity, permissionsList.toArray(new String[permissionsList.size()]), PERMISSION_REQUEST_CODE_CONTACT);
                            }
                        });
                return;
            }


            ActivityCompat.requestPermissions(activity, permissionsList.toArray(new String[permissionsList.size()]), PERMISSION_REQUEST_CODE_CONTACT);

            return;
        }
    }

    public static boolean addPermission(Context cnt, Activity activity, List<String> permissionsList, String permission) {
        if (Build.VERSION.SDK_INT >= 23) {
            if (cnt.checkCallingOrSelfPermission(permission) != PackageManager.PERMISSION_GRANTED) {
                permissionsList.add(permission);
                // Check for Rationale Option
                if (!activity.shouldShowRequestPermissionRationale(permission))
                    return false;
            }
        }
        return true;
    }

    public static void showMessageOKCancel(String message, Activity activity, DialogInterface.OnClickListener okListener) {
        new AlertDialog.Builder(activity)
                .setMessage(message)
                .setPositiveButton("OK", okListener)
                .setNegativeButton("Cancel", null)
                .create()
                .show();
    }

    // ================================
    // END - Code for granting RunTime Permissions on android 23(Marshmallow) Devices
    //===================================

    public static LinkedHashMap<String, String> BG_ValueType_LinkHasMap() {
        LinkedHashMap hmBG = new LinkedHashMap<String, String>();
        hmBG.put("Fasting", "Fasting");
        hmBG.put("Pre Breakfast", "Pre Breakfast");
        hmBG.put("After Breakfast", "After Breakfast");
        hmBG.put("Pre Noon Meal", "Pre Noon Meal");
        hmBG.put("After Noon Meal", "After Noon Meal");
        hmBG.put("Pre Dinner", "Pre Dinner");
        hmBG.put("After Dinner", "After Dinner");
        hmBG.put("Different Food", "Different Food");
        hmBG.put("Bed Time", "Bed Time");
        hmBG.put("During Night", "During Night");
        hmBG.put("Pre Exercise", "Pre Exercise");
        hmBG.put("After Exercise", "After Exercise");

        return hmBG;
    }

    public static LinkedHashMap<String, String> Activity_LinkHasMap() {
        LinkedHashMap hmActivity = new LinkedHashMap<String, String>();
        hmActivity.put("1", "Walking + Steps");
        hmActivity.put("2", "Running");
        hmActivity.put("3", "Cycling");
        hmActivity.put("4", "Swimming");

        return hmActivity;
    }

    public static LinkedHashMap<String, String> RelationShip_LinkHasMap() {
        LinkedHashMap<String, String> hmRelationship = new LinkedHashMap<String, String>();

        hmRelationship.put("0", "---Select Relation---");
        hmRelationship.put("11", "Aunt");
        hmRelationship.put("6", "Brother");
        hmRelationship.put("17", "Brother-in-law");
        hmRelationship.put("5", "Daughter");
        hmRelationship.put("20", "Daughter-in-law");
        hmRelationship.put("2", "Father");
        hmRelationship.put("16", "Father-in-law");
        hmRelationship.put("23", "Granddaughter");
        hmRelationship.put("8", "Grandfather");
        hmRelationship.put("9", "Grandmother");
        hmRelationship.put("22", "Grandson");
        hmRelationship.put("12", "Husband");
        hmRelationship.put("3", "Mother");
        hmRelationship.put("18", "Mother-in-law");
        hmRelationship.put("14", "Nephew");
        hmRelationship.put("15", "Niece");
        hmRelationship.put("1", "Self");
        hmRelationship.put("7", "Sister");
        hmRelationship.put("19", "Sister-in-law");
        hmRelationship.put("4", "Son");
        hmRelationship.put("21", "Son-in-law");
        hmRelationship.put("10", "Uncle");
        hmRelationship.put("13", "Wife");


        return hmRelationship;
    }


    public static LinkedHashMap<String, String> DiffAbledType_LinkHasMap() {
        LinkedHashMap<String, String> hmDiffAbledType = new LinkedHashMap<String, String>();

        hmDiffAbledType.put("0", "Do Not Specify");
        hmDiffAbledType.put("5", "Not Specified");
        hmDiffAbledType.put("4", "Others");
        hmDiffAbledType.put("1", "Physical Disability");
        hmDiffAbledType.put("2", "Speech and Language Disorder");
        hmDiffAbledType.put("3", "Vision Loss and blindness");

        return hmDiffAbledType;
    }

    public static LinkedHashMap<String, String> Gender_LinkHasMap() {
        LinkedHashMap<String, String> hmGender = new LinkedHashMap<String, String>();
        hmGender.put("U", "Do Not Specify");
        hmGender.put("M", "Male");
        hmGender.put("F", "Female");

        return hmGender;
    }



    public static LinkedHashMap<String, String> BloodGroup_LinkHasMap() {
        LinkedHashMap hmBG = new LinkedHashMap<String, String>();
        hmBG.put("0", "Do Not Specify");
        hmBG.put("2", "A Negative");
        hmBG.put("1", "A Positive");
        hmBG.put("6", "AB Negative");
        hmBG.put("5", "AB Positive");
        hmBG.put("4", "B Negative");
        hmBG.put("3", "B Positive");
        hmBG.put("8", "O Negative");
        hmBG.put("7", "O Positive");

        return hmBG;
    }



    public static LinkedHashMap<String, String> StateData_LinkHasMap() {
        LinkedHashMap hmState = new LinkedHashMap<String, String>();
        hmState.put("0", "---Select State---");
        hmState.put("30", "Andaman and Nicobar Islands");
        hmState.put("1", "Andhra Pradesh");
        hmState.put("2", "Arunachal Pradesh");
        hmState.put("3", "Assam");
        hmState.put("4", "Bihar");
        hmState.put("31", "Chandigarh");
        hmState.put("5", "Chhattisgarh");
        hmState.put("32", "Dadra and Nagar Haveli");
        hmState.put("33", "Daman and Diu");
        hmState.put("6", "Goa");
        hmState.put("7", "Gujarat");
        hmState.put("8", "Haryana");
        hmState.put("9", "Himachal Pradesh");
        hmState.put("10", "Jammu and Kashmir");
        hmState.put("11", "Jharkhand");
        hmState.put("12", "Karnataka");
        hmState.put("13", "Kerala");
        hmState.put("34", "Lakshadweep");
        hmState.put("14", "Madhya Pradesh");
        hmState.put("15", "Maharashtra");
        hmState.put("16", "Manipur");
        hmState.put("17", "Meghalaya");
        hmState.put("18", "Mizoram");
        hmState.put("19", "Nagaland");
        hmState.put("35", "National Capital Territory of Delhi");
        hmState.put("20", "Odisha(Orissa)");
        hmState.put("36", "Puducherry");
        hmState.put("21", "Punjab");
        hmState.put("22", "Rajasthan");
        hmState.put("23", "Sikkim");
        hmState.put("24", "Tamil Nadu");
        hmState.put("29", "Telangana");
        hmState.put("25", "Tripura");
        hmState.put("26", "Uttar Pradesh");
        hmState.put("27", "Uttarakhand");
        hmState.put("28", "West Bengal");

        return hmState;
    }



    // Shealth
    // Original Code
    public static long getStartTimeOfToday() {
        // Current Date Only
        Calendar today = Calendar.getInstance();
        today.set(Calendar.HOUR_OF_DAY, 0);
        today.set(Calendar.MINUTE, 0);
        today.set(Calendar.SECOND, 0);
        today.set(Calendar.MILLISECOND, 0);

        return today.getTimeInMillis();
    }

    // To be used when Sync Button is Clicked
    public static void saveStartTimeasLastTimeSync() {

        SharedPreferences.Editor editor = pref.edit();
        editor.putLong(PREF_LastTimeSync, isNullOrEmpty(String.valueOf(System.currentTimeMillis())) ? Long.valueOf(-1) : System.currentTimeMillis());
        editor.apply();
    }

    public static double roundTwoDecimals(double d) {
        DecimalFormat twoDForm = new DecimalFormat("#.##");
        return Double.valueOf(twoDForm.format(d));
    }

    public static void SHealthDataCount() {
        PHRMS_Dashboard_Fragment.drawSHealthDataCount();
    }

    public static void SHealthShowToast() {
        PHRMS_Dashboard_Fragment.ShowToast();
    }

    // shealth

    public static void enableDisableView(View view, boolean enabled) {
        view.setEnabled(enabled);

        if (view instanceof ViewGroup) {
            ViewGroup group = (ViewGroup) view;

            for (int idx = 0; idx < group.getChildCount(); idx++) {
                enableDisableView(group.getChildAt(idx), enabled);
            }
        }
    }

    public static boolean isValidEmail(String email) {
        return !TextUtils.isEmpty(email) && android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches();
    }



    public BroadcastReceiver mRegistrationBroadcastReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            // Waking up mobile if it is sleeping
            WakeLocker.acquire(getApplicationContext());
            // Releasing wake lock
            WakeLocker.release();

            // checking for type intent filter
            if (intent.getAction().equals(Config.REGISTRATION_COMPLETE)) {
                //FirebaseMessaging.getInstance().subscribeToTopic(Config.TOPIC_GLOBAL);
                Functions.displayFirebaseRegId(getApplicationContext());
            } else if (intent.getAction().equals(Config.PUSH_NOTIFICATION)) {
                // 1. In case only Notification == Show Mesaage Only
                // 2. If Notification is with Data == Show Data and Messages.
                if (intent.getExtras() != null) {
                    for (String key : intent.getExtras().keySet()) {
                        Object value = intent.getExtras().get(key);
                        Log.d(TAGFB, "Key: " + key + " Value: " + value);
                    }
                }

                // new push notification is received
                String message = intent.getStringExtra("message");
                Functions.showToast(getApplicationContext(), message);
            }
        }
    };



    public static void displayFirebaseRegId(Context cnt) {
        SharedPreferences pref = cnt.getSharedPreferences(Config.SHARED_PREF, Context.MODE_PRIVATE);
        String tkfcmid = pref.getString("tkfcmid", null);

        Log.d(TAGFB, "Firebase reg id: " + tkfcmid);

        if (!TextUtils.isEmpty(tkfcmid))
            Functions.showToast(cnt, "Device Registered to MyHealthRecord App Server");
        else
            Functions.showToast(cnt, "Device Not Registered to MyHealthRecord App Server");
    }

 

    public static void storeRegIdInPref(Context cnt, String token) {
        // Store Gcm token to to shared preferences MYHealthRecord
        SharedPreferences pref_token = cnt.getApplicationContext().getSharedPreferences(Config.SHARED_PREF, 0);
        SharedPreferences.Editor editor = pref_token.edit();
        editor.putString("tkfcmid", token);
        // When Token Refreshed - Set Boolean that refreshed token is not sent to server yet: False
        editor.putBoolean("tksntservre", false);
        editor.commit();
    }

    /**
     * Check whether Google Play Services are available.
     * <p>
     * If not, then display dialog allowing user to update Google Play Services
     *
     * @return true if available, or false if not
     */
    private boolean checkGooglePlayServicesAvailable() {
        final int status = GooglePlayServicesUtil.isGooglePlayServicesAvailable(getApplicationContext());
        if (status == ConnectionResult.SUCCESS) {
            return true;
        }

        Log.e(TAGFB, "Google Play Services not available: " + GooglePlayServicesUtil.getErrorString(status));
        if (GooglePlayServicesUtil.isUserRecoverableError(status)) {
            final Dialog errorDialog = GooglePlayServicesUtil.getErrorDialog(status, this, 1);
            if (errorDialog != null) {
                errorDialog.show();
            }
        }
        return false;
    }






    // For FCM
    @Override
    protected void onResume() {
        super.onResume();
        // register GCM registration complete receiver
        LocalBroadcastManager.getInstance(this).registerReceiver(mRegistrationBroadcastReceiver, new IntentFilter(Config.REGISTRATION_COMPLETE));

        LocalBroadcastManager.getInstance(this).registerReceiver(mRegistrationBroadcastReceiver, new IntentFilter(Config.PUSH_NOTIFICATION));

        // clear the notification area when the app is opened
        NotificationUtils.clearNotifications(getApplicationContext());
    }

    @Override
    protected void onPause() {
        LocalBroadcastManager.getInstance(this).unregisterReceiver(mRegistrationBroadcastReceiver);
        super.onPause();
    }

    @Override
    protected void onDestroy() {
     
        if (mStoreHealth != null) {
            try {
                mStoreHealth.disconnectService();
            } catch (Exception e) {
                Log.d(Functions.APP_TAG, "mStoreHealth Disconnect Service: " + e.getMessage());
            }
        }

        super.onDestroy();
    }

    @Override
    public void onStart() {
        super.onStart();

        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        client.connect();
        Action viewAction = Action.newAction(
                Action.TYPE_VIEW, // TODO: choose an action type.
                "Functions Page", // TODO: Define a title for the content shown.
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
                "Functions Page", // TODO: Define a title for the content shown.
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

package com.pkg.healthrecordappname.appfinalname;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.util.Patterns;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.view.inputmethod.EditorInfo;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.google.android.gms.appindexing.Action;
import com.google.android.gms.appindexing.AppIndex;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.firebase.appindexing.FirebaseUserActions;
import com.google.firebase.appindexing.builders.Actions;
import com.pkg.healthrecordappname.appfinalname.modules.httpconnections.HttpUrlConnectionRequest;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

import org.apache.http.client.ClientProtocolException;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;


/**
 * A login screen that offers login via email/u_pass.
 */
public class PHRMS_LoginActivity extends AppCompatActivity {

    /**
     * Keep track of the login task to ensure we can cancel it if requested.
     */
    private UserLoginTask mAuthTask = null;

    // UI references.
    private AutoCompleteTextView mEmailView;
    private EditText mPasswordView;
    private ProgressBar mProgressView;
    private View mCardView_LoginForm;

    String errormsg = "";
    String u_name = null;
    String u_pass = null;
    String user_GUID = null;
    String loggedout;


    private GoogleApiClient client;

    TextView txtf = null;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.login);

        // Set up the login form.
        mEmailView = (AutoCompleteTextView) findViewById(R.id.email);
        mEmailView.addTextChangedListener(new EditTextWatcher(mEmailView));
        mCardView_LoginForm = findViewById(R.id.card_login_form);
        mProgressView = (ProgressBar) findViewById(R.id.login_progress);
        txtf = (TextView) findViewById(R.id.txtfail);
        mPasswordView = (EditText) findViewById(R.id.password);
        mPasswordView.addTextChangedListener(new EditTextWatcher(mPasswordView));

        Button mUserRegisterationButton = (Button) findViewById(R.id.register_button);

        Functions.hideKeyboard(PHRMS_LoginActivity.this);

        Functions.progressbarStyle(mProgressView, PHRMS_LoginActivity.this);

        // after password filling - enable enter
        mPasswordView.setOnEditorActionListener(new TextView.OnEditorActionListener() {
            @Override
            public boolean onEditorAction(TextView textView, int id, KeyEvent keyEvent) {
                if (id == R.id.login || id == EditorInfo.IME_NULL || id == EditorInfo.IME_ACTION_DONE) {
                    attemptLogin();
                    return true;
                }
                return false;
            }
        });

        Button mEmailSignInButton = (Button) findViewById(R.id.email_sign_in_button);
        if (mEmailSignInButton != null) {
            mEmailSignInButton.setOnClickListener(new OnClickListener() {
                @Override
                public void onClick(View view) {
                    attemptLogin();
                }
            });
        }

        if (mUserRegisterationButton != null)
        {
            mUserRegisterationButton.setOnClickListener(new OnClickListener() {
                @Override
                public void onClick(View view)
                {
                    Intent UserRegisteration = new Intent(PHRMS_LoginActivity.this, PHRMS_UserRegisteration.class);
                    UserRegisteration.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);

                    startActivity(UserRegisteration);
                }
            });
        }

        Functions.pref = getSharedPreferences(Functions.PREFS_NAME, Context.MODE_PRIVATE);

        u_name = Functions.pref.getString(Functions.P_UNAME, null);
        u_pass = Functions.pref.getString(Functions.P_PASS, null);
        loggedout = Functions.pref.getString(Functions.P_Lgout, "yes"); // By default logged out - yes
        user_GUID = Functions.pref.getString(Functions.P_UsrID, null);

        if (u_name != null && u_pass != null && user_GUID != null && loggedout.equals("no")) {
            if (Functions.isNetworkAvailable(this)) {
                Functions.hideKeyboard(PHRMS_LoginActivity.this);
                showProgress(true);
                // Send hash u_pass from session
                mAuthTask = new UserLoginTask(Functions.decrypt(getApplicationContext(), u_name), Functions.decrypt(getApplicationContext(), u_pass));
                mAuthTask.execute((Void) null);
            } else {
                Functions.showToast(this, "Internet Not Available !!");
            }
        }

        // ATTENTION: This was auto-generated to implement the App Indexing API.

        client = new GoogleApiClient.Builder(this).addApi(AppIndex.API).build();
    }

    @Override
    public void onRestart() {
        super.onRestart();
        Intent LoginScreen = new Intent(this, PHRMS_LoginActivity.class);
        LoginScreen.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        this.startActivity(LoginScreen);
        // showToast("onRestart Called !!");
    }

    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
        if (hasFocus) {
            getWindow().getDecorView().setSystemUiVisibility(
                    View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                            | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                            | View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                            | View.SYSTEM_UI_FLAG_HIDE_NAVIGATION
                            | View.SYSTEM_UI_FLAG_FULLSCREEN
                            | View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY);
        }
    }

    /**
     * Attempts to  sign in or register the account specified by the login form.
     * If there are form errors (invalid email, missing fields, etc.), the
     * errors are presented and no actual login attempt is made.
     */


    private void attemptLogin() {
        Functions.hideKeyboard(PHRMS_LoginActivity.this);

        if (mAuthTask != null) {
            return;
        }
        // Reset errors.
        mEmailView.setError(null);
        mPasswordView.setError(null);
        // Store values at the time of the login attempt.
        txtf.setVisibility(View.INVISIBLE);

        // if no validation error returns false;
        boolean emailcancel = check_email();
        boolean passwordcancel = check_pass();



        // If no validation error
        if (emailcancel == false && passwordcancel == false) {
            // Show a progress spinner, and kick off a background task to
            // perform the user login attempt.
            // Check integrent and send request
            if (Functions.isNetworkAvailable(this)) {
                showProgress(true);
                // SHA512 Hashing
                String  secure_pass = get_SecurePassword(mPasswordView.getText().toString().trim());
                // Send hash u_pass
                mAuthTask = new UserLoginTask(mEmailView.getText().toString().trim(),  secure_pass);
                mAuthTask.execute((Void) null);
            } else {
                Functions.showToast(this, "Internet Not Available !!");
            }
        } else {
            return;
        }
    }

    private boolean check_email() {

        boolean emailcancel = false;
        // Check for a valid email locaddr.
        if (TextUtils.isEmpty(mEmailView.getText().toString().trim())) {
            mEmailView.setError(getString(R.string.error_field_required));
            //focusView = mEmailView;
            emailcancel = true;
            requestFocus(mEmailView);
        } else if (!isEmailValid(mEmailView.getText().toString().trim())) {
            mEmailView.setError(getString(R.string.error_invalid_emailmobile));
            //focusView = mEmailView;
            emailcancel = true;
            requestFocus(mEmailView);
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

    private boolean check_pass() {
        // Check for a valid u_pass, if the user entered one.
        boolean passwordcancel = false;

        if (TextUtils.isEmpty(mPasswordView.getText().toString().trim())) {
            mPasswordView.setError(getString(R.string.error_field_required));
            //focusView = mPasswordView;
            passwordcancel = true;
            requestFocus(mPasswordView);
        } else if (!isPasswordValid(mPasswordView.getText().toString().trim())) {
            mPasswordView.setError(getString(R.string.error_invalid_password));
            //focusView = mPasswordView;
            passwordcancel = true;
            requestFocus(mPasswordView);
        }

        return passwordcancel;
    }

    private boolean isPasswordValid(String password) {
        //TODO: Replace this with your own logic
        return password.length() >= 6;
    }

    public String get_SecurePassword(String passwordToHash) {

        return "Encrypt your passowrd";
    }

    /**
     * Shows the progress UI and hides the login form.
     */
    @TargetApi(Build.VERSION_CODES.HONEYCOMB_MR2)
    private void showProgress(final boolean show) {
        // On Honeycomb MR2 we have the ViewPropertyAnimator APIs, which allow
        // for very easy animations. If available, use these APIs to fade-in
        // the progress spinner.
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB_MR2) {
            int shortAnimTime = getResources().getInteger(android.R.integer.config_shortAnimTime);

            mCardView_LoginForm.setVisibility(show ? View.GONE : View.VISIBLE);
            mCardView_LoginForm.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    mCardView_LoginForm.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });

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
            mCardView_LoginForm.setVisibility(show ? View.GONE : View.VISIBLE);
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
                "PHRMS_Login Page", // TODO: Define a title for the content shown.
                // TODO: If you have web page content that matches this app activity's content,
                // make sure this auto-generated web page URL is correct.
                // Otherwise, set the URL to null.
                Uri.parse("http://host/path"),
                // TODO: Make sure this auto-generated app URL is correct.
                Uri.parse("android-app://com.pkg.healthrecordappname.appfinalname/http/host/path")
        );
        AppIndex.AppIndexApi.start(client, viewAction);
        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        FirebaseUserActions.getInstance().start(getIndexApiAction());
    }

    @Override
    public void onStop() {
        super.onStop();// ATTENTION: This was auto-generated to implement the App Indexing API.
// See https://g.co/AppIndexing/AndroidStudio for more information.
        FirebaseUserActions.getInstance().end(getIndexApiAction());

        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        Action viewAction = Action.newAction(
                Action.TYPE_VIEW, // TODO: choose an action type.
                "PHRMS_Login Page", // TODO: Define a title for the content shown.
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

    /**
     * ATTENTION: This was auto-generated to implement the App Indexing API.
     * See https://g.co/AppIndexing/AndroidStudio for more information.
     */
    public com.google.firebase.appindexing.Action getIndexApiAction() {
        return Actions.newView("PHRMS_Login", "http://[ENTER-YOUR-URL-HERE]");
    }

    /**
     * Represents an asynchronous login/registration task used to authenticate
     * the user.
     */
    public class UserLoginTask extends AsyncTask<Void, Void, Integer> {

        private final String mEmail;
        private final String mPassword;


        UserLoginTask(String email, String password) {
            mEmail = email;
            mPassword = password;
        }

        @Override
        protected void onPreExecute() {
            Functions.mLockScreenRotation(PHRMS_LoginActivity.this);

        }

        @Override
        protected Integer doInBackground(Void... params) {
            // TODO: attempt authentication against a network service.
            return userCheck(mEmail, mPassword);
        }



        @Override
        protected void onPostExecute(final Integer success) {
            mAuthTask = null;

            if (success == 0) {
                mProgressView.setVisibility(View.VISIBLE);
                mCardView_LoginForm.setVisibility(View.GONE);
            } else {
                //mProgressView.setVisibility(View.GONE);
                //mCardView_LoginForm.setVisibility(View.VISIBLE);
                showProgress(false);
            }

            Functions.enable_orientation(PHRMS_LoginActivity.this);

            switch (success) {
                case 0:
                    // When Success

                    Intent MainScreen = new Intent(PHRMS_LoginActivity.this, PHRMS_MainActivity.class);
                    MainScreen.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                    finish();
                    startActivity(MainScreen);


                    break;
                case -1:
                    txtf.setVisibility(View.VISIBLE);
                    txtf.setText("Login Failed. Wrong username or password");
                    break;
                case -2:
                    txtf.setVisibility(View.VISIBLE);
                    txtf.setText("User data is not accesible");
                    break;
                case -3:
                    txtf.setVisibility(View.VISIBLE);
                    txtf.setText("Request result not available!! No value returned");
                    break;
                case -4:
                    txtf.setVisibility(View.VISIBLE);
                    txtf.setText("ClientProtocolException");
                    break;
                case -5:
                    Functions.showToast(PHRMS_LoginActivity.this,
                            "No response from server. Please check your network connection.");
                    break;
                case -6:
                    txtf.setVisibility(View.VISIBLE);
                    txtf.setText("Json error");
                    break;
                case -7:
                    txtf.setVisibility(View.VISIBLE);
                    txtf.setText("SHA Failed");
                    break;
                default:
                    txtf.setVisibility(View.VISIBLE);
                    txtf.setText("Default case Error");
                    break;
            }
        }

        @Override
        protected void onCancelled() {
            mAuthTask = null;
            showProgress(false);
        }

        public int userCheck(String t_usr, String t_hashpwd) {
            int i = -99;

            if (!t_hashpwd.equals("-7")) {

                try {


                    JSONObject jSonData = HttpUrlConnectionRequest.SendJsonHttpUrlGetRequest(getString(R.string.urlLogin) + getString(R.string.AccountLogin) + t_usr + "/" + t_hashpwd);

                    if (jSonData != null) {

                        if (jSonData.getString("status").equals("0")) {
                            i = -1; // Invalid u_name / Password
                        } else {
                            JSONObject Json_response = new JSONObject(jSonData.getString("response"));

                            if (Json_response != null) {
                                // UsersViewModel - Data to fetch users login details
                                JSONObject json_user_l = new JSONObject(Json_response.getString("usersViewModel"));

                                // userPersonalProfileViewModel - Data to fetch users personal profile
                                JSONObject json_user_Profile = new JSONObject(Json_response.getString("userPersonalProfileViewModel"));

                                if (json_user_l != null && json_user_Profile!=null)
                                {
                                    SharedPreferences.Editor editor = Functions.pref.edit();

                                    editor.putString(Functions.P_UNAME, Functions.isNullOrEmpty(t_usr) ? null : Functions.encrypt(getApplicationContext(), t_usr));
                                    editor.putString(Functions.P_PASS, Functions.isNullOrEmpty(t_hashpwd) ? null : Functions.encrypt(getApplicationContext(), t_hashpwd));

                                    editor.putString(Functions.P_UsrID, Functions.isNullOrEmpty(json_user_l.getString("UserId")) ? null : Functions.encrypt(getApplicationContext(), json_user_l.getString("UserId")));


                                    String aadhaar_user = null;
                                    if (!Functions.isNullOrEmpty(json_user_l.getString("AadhaarNo")))
                                    {
                                        aadhaar_user = json_user_l.getString("AadhaarNo");
                                    }

                                    String aadhaar_userPersonalProfileViewModel=null;
                                    if (!Functions.isNullOrEmpty(json_user_Profile.getString("Uhid")) )
                                    {
                                        aadhaar_userPersonalProfileViewModel = json_user_Profile.getString("Uhid");
                                    }

                                    if(!aadhaar_user.equals(aadhaar_userPersonalProfileViewModel) && !Functions.isNullOrEmpty(aadhaar_userPersonalProfileViewModel))
                                    {
                                        aadhaar_user = aadhaar_userPersonalProfileViewModel;
                                    }

                                    editor.putString(Functions.P_AdhrN, Functions.isNullOrEmpty(aadhaar_user.toString()) ? null : Functions.encrypt(getApplicationContext(), aadhaar_user.toString()));


                                    String profile_name = null;
                                    if (!Functions.isNullOrEmpty(json_user_l.getString("FirstName")) && !Functions.isNullOrEmpty(json_user_l.getString("LastName")))
                                    {
                                        profile_name = json_user_l.getString("FirstName") + " " + json_user_l.getString("LastName");
                                    }

                                    // Condition when personal profile name differs - if it is updated by the user in perosnl profile details
                                    // Step 1.
                                    String personal_profilename = null;
                                    if (!Functions.isNullOrEmpty(json_user_Profile.getString("FirstName")) && !Functions.isNullOrEmpty(json_user_Profile.getString("LastName")))
                                    {
                                        personal_profilename = json_user_Profile.getString("FirstName") + " " + json_user_Profile.getString("LastName");
                                    }
                                    // Check if data form personal profile is null or not same as userview model
                                    // if not same then update name data from userPersonalProfileViewModel details
                                    if(!profile_name.equals(personal_profilename) && !Functions.isNullOrEmpty(personal_profilename))
                                    {
                                        profile_name = personal_profilename;
                                    }
                                    editor.putString(Functions.P_NAME, profile_name);

                                    editor.putString(Functions.P_RoleID, Functions.isNullOrEmpty(json_user_l.getString("RoleId")) ? null : json_user_l.getString("RoleId"));
                                    editor.putString(Functions.P_Email, Functions.isNullOrEmpty(json_user_l.getString("Email")) ? null : json_user_l.getString("Email"));
                                    editor.putLong(Functions.P_Mobile, Functions.isNullOrEmpty(json_user_l.getString("MobileNo")) ? -1 : Long.parseLong(json_user_l.getString("MobileNo")));


                                    // Convert Wrong Image Paths for Older Images
                                    if (!Functions.isNullOrEmpty(json_user_l.getString("ImgPath")) && json_user_l.getString("ImgPath").startsWith("\\")) {
                                        editor.putString(Functions.P_Img, Functions.isNullOrEmpty(json_user_l.getString("ImgPath")) ? null : json_user_l.getString("ImgPath").replace("\\", "//"));
                                    } else {
                                        //---- Actual File Path

                                        editor.putString(Functions.P_Img, Functions.isNullOrEmpty(json_user_l.getString("ImgPath")) ? null : json_user_l.getString("ImgPath"));
                                    }
                                    editor.putString(Functions.P_Lgout, "no");
                                    editor.apply();
                                    //editor.commit();
                                    i = 0;
                                } else {
                                    i = -2; // User Profile data empty
                                }
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
                    errormsg = e.getMessage();
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
                case R.id.email:
                    // Required check for empty also
                    check_email();
                    break;
                case R.id.password:
                    // Required check for empty also
                    check_pass();
                    break;
            }
        }
    }


    protected void requestFocus(View view) {
        if (view.requestFocus()) {
            getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_VISIBLE);
        }
    }

    // For FCM
    @Override
    protected void onResume() {
        super.onResume();
    }

    @Override
    protected void onPause() {
        super.onPause();
    }

}


package com.pkg.healthrecordappname.appfinalname.modules.useables;

import android.Manifest;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.os.Build;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageButton;
import android.widget.ProgressBar;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.R;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.Map;


public class ProfileImageChange extends AppCompatActivity {
    private ProgressBar mProgressBarFullScreenProfileImage;
    private ImageButton mIBSave_ProfileImage;
    private ImageButton mimagebtn_profile;

    private Boolean imageExists = false;
    private String imageBase64string = "-1";
    public static final int PICK_IMAGE_ID = 776;
    private static final int PERMISSION_REQUEST_CODE_MULTIPLE = 777;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.imageviewprofile);

        //toolbar
        Toolbar mtoolbar_toolbar_ImageFullScreen = (Toolbar) findViewById(R.id.toolbar_ProfileImageFullScreen);

        mIBSave_ProfileImage = (ImageButton) findViewById(R.id.IBSave_ProfileImage);
        mIBSave_ProfileImage.setVisibility(View.INVISIBLE);

        mimagebtn_profile = (ImageButton) findViewById(R.id.imagebtn_profile);

        if (mtoolbar_toolbar_ImageFullScreen != null) {
            setSupportActionBar(mtoolbar_toolbar_ImageFullScreen);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarFullScreenProfileImage = (ProgressBar) findViewById(R.id.ProgressBarFullScreenProfileImage);
        ImagePicker.setMinQuality(300, 300);

        Functions.progressbarStyle(mProgressBarFullScreenProfileImage, ProfileImageChange.this);

        mimagebtn_profile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (Build.VERSION.SDK_INT >= 23) {
                    if ((Functions.hasCameraPermission(ProfileImageChange.this) == true) && (Functions.hasExternalStoragePermission(ProfileImageChange.this) == true)) {
                        Log.e("testing", "Permission is granted");
                        // Activity result code is integrated to ImagePicker - v2
                        ImagePicker.pickImage(ProfileImageChange.this, "Select your Profile Image:", PICK_IMAGE_ID);
                    } else {
                        //checkpermissions(getApplicationContext(), ProfileImageChange.this);
                        Functions.checkpermissions(getApplicationContext(), ProfileImageChange.this, PERMISSION_REQUEST_CODE_MULTIPLE);
                    }
                } else {
                    //permission is automatically granted on sdk<23 upon installation
                    Log.e("testing", "Permission is already granted");
                    // Activity result code is integrated to ImagePicker - v2
                    ImagePicker.pickImage(ProfileImageChange.this, "Select your Profile Image:", PICK_IMAGE_ID);
                }
            }
        });


        mIBSave_ProfileImage.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                // Post data with url
                if (Functions.isNetworkAvailable(ProfileImageChange.this)) {
                    saveProfileImage();
                } else {
                    Functions.showToast(ProfileImageChange.this, "Internet Not Available!!");
                }
            }
        });

    }

    private void saveProfileImage() {

        if (imageExists == true && !Functions.isNullOrEmpty(imageBase64string) && !imageBase64string.equals("-1")) {
            Functions.showProgress(true, mProgressBarFullScreenProfileImage);

            //Get Upload Profile Image URL…
            final String url_ProfileImage = getString(R.string.urlLogin) + getString(R.string.UpdateUserProfileImage);

            Map<String, String> jsonParamsProfile = new HashMap<String, String>();

            jsonParamsProfile.put("Text", imageBase64string);
            jsonParamsProfile.put("userID", Functions.ApplicationUserid);

            JsonObjectRequest postRequestProfileImage = new JsonObjectRequest(Request.Method.POST, url_ProfileImage,
                    new JSONObject(jsonParamsProfile),
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {
                            AfterPostProfileImage(response);
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            Functions.showProgress(false, mProgressBarFullScreenProfileImage);
                            Functions.ErrorHandling(ProfileImageChange.this, error);
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

            postRequestProfileImage.setRetryPolicy(new DefaultRetryPolicy(Functions.DEFAULT_TIMEOUT_MS, Functions.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

            // Access the RequestQueue through your singleton class.
            MySingleton.getInstance(ProfileImageChange.this).addToRequestQueue(postRequestProfileImage);

        } else {
            Functions.showToast(ProfileImageChange.this, "Image not available!!");
        }
    }

    private void AfterPostProfileImage(JSONObject response) {
        String STATUS_Post = parseAfterPostJson(response);

        switch (STATUS_Post) {
            case "1":
                Functions.showProgress(false, mProgressBarFullScreenProfileImage);
                Functions.showToast(ProfileImageChange.this, "Profile Image - Updated.");
                Functions.LoadHeaderContent(getApplicationContext());
                break;
            case "-1":
                Functions.showToast(ProfileImageChange.this, "Profile Image - not available.");
                Functions.showProgress(false, mProgressBarFullScreenProfileImage);
                break;
            case "-2":
                Functions.showToast(ProfileImageChange.this, "Profile Image - unable to save image.");
                Functions.showProgress(false, mProgressBarFullScreenProfileImage);
                break;
            case "-3":
                Functions.showToast(ProfileImageChange.this, "Profile Image - No Response from server.");
                Functions.showProgress(false, mProgressBarFullScreenProfileImage);
                break;
            case "-4":
                Functions.showToast(ProfileImageChange.this, "Profile Image - Json Parser.");
                Functions.showProgress(false, mProgressBarFullScreenProfileImage);
                break;
            default:
                Functions.showToast(ProfileImageChange.this, STATUS_Post);
                Functions.showProgress(false, mProgressBarFullScreenProfileImage);
                break;
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
                    ImagePicker.pickImage(ProfileImageChange.this, "Select your Profile Image:", PICK_IMAGE_ID);
                } else {
                    // Permission Denied
                    Functions.showToast(ProfileImageChange.this, "Some Permission Denied");
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
                if (resultCode == RESULT_OK) {
                    Bitmap bitmap_labtest = ImagePicker.getImageFromResult(ProfileImageChange.this, requestCode, resultCode, data);

                    if (bitmap_labtest != null) {
                        mimagebtn_profile.setImageBitmap(bitmap_labtest);
                        imageBase64string = Functions.bitmapToBase64(bitmap_labtest);

                        // Check converted string and set the value to true
                        if (!Functions.isNullOrEmpty(imageBase64string) && !imageBase64string.equals("-1")) {
                            imageExists = true;
                            mIBSave_ProfileImage.setVisibility(View.VISIBLE);
                        }
                    }
                }
                break;
            default:
                super.onActivityResult(requestCode, resultCode, data);
                break;
        }
    }




    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                finish();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    public String parseAfterPostJson(JSONObject jsonData) {
        String parse_response = "-1";  // "Profile Image - not available." No image data recieved by server - file not saved

        try {
            if (jsonData != null) {
                if (jsonData.getString("Status").equals("1") && !Functions.isNullOrEmpty(jsonData.getString("Response")) && !jsonData.getString("Response").equals("-1")) {
                    parse_response = "1"; // Image Saved update image shared data url

                    // Update Shared Prefrences for Image Path
                    SharedPreferences.Editor editor = Functions.pref.edit();
                    String imagePathProfileCustom = jsonData.getString("Response");
                    editor.putString(Functions.P_Img, Functions.isNullOrEmpty(imagePathProfileCustom) ? null : imagePathProfileCustom);
                    editor.apply();

                } else {
                    if (jsonData.getString("Status").equals("-1") || jsonData.getString("Response").equals("-1")) {
                        parse_response = "-1"; // "Profile Image - not available." No image data recieved by server - file not saved
                    } else {
                        parse_response = "-2"; // "Profile Image - unable to save image." Exception while file Saving File at server
                    }
                }
            } else {
                parse_response = "-3"; // Service doesn't sent any response
            }
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            //errormsg = e.getMessage();
            //e.printStackTrace();
            parse_response = "-4"; // Json Parsing Error
        }

        return parse_response;
    }
}

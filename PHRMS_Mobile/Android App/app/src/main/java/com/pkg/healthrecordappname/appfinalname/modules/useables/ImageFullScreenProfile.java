package com.pkg.healthrecordappname.appfinalname.modules.useables;



import android.Manifest;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.ProgressBar;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.engine.DiskCacheStrategy;
import com.bumptech.glide.load.resource.drawable.GlideDrawable;
import com.bumptech.glide.load.resource.gif.GifDrawable;
import com.bumptech.glide.request.RequestListener;
import com.bumptech.glide.request.animation.GlideAnimation;
import com.bumptech.glide.request.target.SimpleTarget;
import com.bumptech.glide.request.target.Target;
import com.pkg.healthrecordappname.appfinalname.R;

import java.io.BufferedOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.LinkedHashMap;


public class ImageFullScreenProfile extends AppCompatActivity {
    private ProgressBar mProgressBarFullScreenImage;
    private ImageButton mIBSave_Image;

    private ImageView imageViewPreview;
    private ImageButton mIBEDIT_Image;

    private static final int PERMISSION_REQUEST_CODE_EXTERNAL = 501;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.imageviewfullscreenprofile);

        //toolbar
        Toolbar mtoolbar_toolbar_ImageFullScreen = (Toolbar) findViewById(R.id.toolbar_ImageFullScreenProfile);
        mIBSave_Image = (ImageButton) findViewById(R.id.IBProfileSave_Image);
        mIBSave_Image.setVisibility(View.INVISIBLE);

        // Added Edit Functionality on View
        mIBEDIT_Image = (ImageButton) findViewById(R.id.IBProfileEDIT_Image);

        imageViewPreview = (ImageView) findViewById(R.id.image_previewProfile);

        if (mtoolbar_toolbar_ImageFullScreen != null) {
            setSupportActionBar(mtoolbar_toolbar_ImageFullScreen);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarFullScreenImage = (ProgressBar) findViewById(R.id.ProgressBarFullScreenImageProfile);
        mProgressBarFullScreenImage.setVisibility(View.VISIBLE);


        Functions.progressbarStyle(mProgressBarFullScreenImage, ImageFullScreenProfile.this);

        Bundle bundle = getIntent().getExtras();

        //Extract the data…
        final String url = bundle.getString("imageurl");
        final String imageType = bundle.getString("imageType");

        if (!Functions.isNullOrEmpty(url)) {
            if (Functions.isNetworkAvailable(ImageFullScreenProfile.this)) {
                loadFullScreen(url);
            } else {
                Functions.showToast(ImageFullScreenProfile.this, "Internet Not Available!!");
            }
        } else {
            Functions.showToast(ImageFullScreenProfile.this, "Image Not Available!!");
        }

        mIBSave_Image.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (!Functions.isNullOrEmpty(url)) {
                    // Post data with url
                    if (Functions.isNetworkAvailable(ImageFullScreenProfile.this)) {
                        if (!Functions.isNullOrEmpty(url)) {
                            saveFile(url, imageType);
                        } else {
                            Functions.showToast(ImageFullScreenProfile.this, "Image not available!!");
                        }
                    } else {
                        Functions.showToast(ImageFullScreenProfile.this, "Internet Not Available!!");
                    }
                } else {
                    Functions.showToast(ImageFullScreenProfile.this, "Image Not Available!!");
                }
            }
        });


        mIBEDIT_Image.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                Intent intProfileImage = new Intent(getApplicationContext(), ProfileImageChange.class).addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                finish();
                startActivity(intProfileImage);
            }
        });
    }

    private void saveFile(String url, String imageType) {
        if (Build.VERSION.SDK_INT >= 23) {
            if (Functions.hasExternalStoragePermission(ImageFullScreenProfile.this)) {
                Log.e("testing", "Permission is granted");
                saveImageData(url, imageType);
            } else {
                Functions.checkExternalpermissions(getApplicationContext(), ImageFullScreenProfile.this, PERMISSION_REQUEST_CODE_EXTERNAL);
            }
        } else {
            //permission is automatically granted on sdk<23 upon installation
            Log.e("testing", "Permission is already granted");
            saveImageData(url, imageType);
        }
    }

    private void saveImageData(String url, final String imageType) {


        Glide.with(ImageFullScreenProfile.this)
                .load(url)
                .asBitmap()
                .toBytes(Bitmap.CompressFormat.JPEG, 80)
                .into(new SimpleTarget<byte[]>() {
                    @Override
                    public void onResourceReady(final byte[] resource, GlideAnimation<? super byte[]> glideAnimation) {
                        new AsyncTask<Void, Void, Boolean>() {
                            @Override
                            protected Boolean doInBackground(Void... params) {
                                Boolean fileSave = false;
                                File sdcard = Environment.getExternalStorageDirectory();
                                String formattedDate = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(Calendar.getInstance().getTime());



                                File file = new File(sdcard + "/PHRMS/" + imageType + "_" + formattedDate + ".jpg");
                                File dir = file.getParentFile();
                                try {
                                    if (!dir.mkdirs() && (!dir.exists() || !dir.isDirectory())) {
                                        throw new IOException("Cannot ensure parent directory for file " + file);
                                    } else {
                                        BufferedOutputStream s = new BufferedOutputStream(new FileOutputStream(file));
                                        s.write(resource);
                                        s.flush();
                                        s.close();

                                        fileSave = true;
                                    }
                                } catch (IOException e) {

                                }
                                return fileSave;
                            }

                            @Override
                            protected void onPostExecute(Boolean fileSave) {
                                if (fileSave) {
                                    Functions.showToast(ImageFullScreenProfile.this, "Image Saved");
                                } else {
                                    Functions.showToast(ImageFullScreenProfile.this, "Cannot ensure parent directory for file.");
                                }

                            }


                        }.execute();
                    }

                    //@Override
                    public boolean onException(Exception e, String model, Target<GlideDrawable> target, boolean isFirstResource) {
                        Functions.showToast(ImageFullScreenProfile.this, "Cannot ensure parent directory for file.");
                        return false;
                    }
                });
        //.with(context);
    }

    private void loadFullScreen(String url) {
        if (url.endsWith(".gif")) {
            Glide.with(ImageFullScreenProfile.this)
                    .load(url)
                    .asGif()
                    .thumbnail(0.5f)
                    //.placeholder(R.drawable.ic_image_black_48dp) // replace with spinner
                    .listener(new RequestListener<String, GifDrawable>() {
                        @Override
                        public boolean onException(Exception e, String model, Target<GifDrawable> target, boolean isFirstResource) {
                            mIBSave_Image.setVisibility(View.INVISIBLE);
                            mProgressBarFullScreenImage.setVisibility(View.GONE);
                            return false;
                        }

                        @Override
                        public boolean onResourceReady(GifDrawable resource, String model, Target<GifDrawable> target, boolean isFromMemoryCache, boolean isFirstResource) {
                            mIBSave_Image.setVisibility(View.VISIBLE);
                            mProgressBarFullScreenImage.setVisibility(View.GONE);
                            return false;
                        }
                    })
                    .error(R.drawable.ic_image_white_48dp)
                    .diskCacheStrategy(DiskCacheStrategy.NONE)
                    .skipMemoryCache(true)
                    .crossFade()
                    .into(imageViewPreview);

        } else {
            Glide.with(ImageFullScreenProfile.this)
                    .load(url)
                    .thumbnail(0.5f)
                    //.crossFade()
                    //.dontAnimate()
                    //.placeholder(R.drawable.ic_image_black_48dp)  // replace with spinner
                    .listener(new RequestListener<String, GlideDrawable>() {
                        @Override
                        public boolean onException(Exception e, String model, Target<GlideDrawable> target, boolean isFirstResource) {
                            mIBSave_Image.setVisibility(View.INVISIBLE);
                            mProgressBarFullScreenImage.setVisibility(View.GONE);
                            return false;
                        }

                        @Override
                        public boolean onResourceReady(GlideDrawable resource, String model, Target<GlideDrawable> target, boolean isFromMemoryCache, boolean isFirstResource) {
                            mIBSave_Image.setVisibility(View.VISIBLE);
                            mProgressBarFullScreenImage.setVisibility(View.GONE);
                            return false;
                        }
                    })
                    .error(R.drawable.ic_image_white_48dp)
                    .diskCacheStrategy(DiskCacheStrategy.NONE)
                    .skipMemoryCache(true)
                    .crossFade()
                    .into(imageViewPreview);
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        switch (requestCode) {
            case PERMISSION_REQUEST_CODE_EXTERNAL: {
                LinkedHashMap<String, Integer> perms = new LinkedHashMap<String, Integer>();
                // Initial
                perms.put(Manifest.permission.CAMERA, PackageManager.PERMISSION_GRANTED);

                // Fill with results
                for (int i = 0; i < permissions.length; i++)
                    perms.put(permissions[i], grantResults[i]);

                if (perms.get(Manifest.permission.WRITE_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED) {
                    Functions.showToast(ImageFullScreenProfile.this, "Permission Granted");
                } else {
                    // Permission Denied
                    Functions.showToast(ImageFullScreenProfile.this, "Some Permission Denied");
                }
            }
            break;
            default:
                super.onRequestPermissionsResult(requestCode, permissions, grantResults);
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
}

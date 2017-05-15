package com.pkg.healthrecordappname.appfinalname.modules.service;

import android.util.Log;

import com.google.firebase.iid.FirebaseInstanceId;
import com.google.firebase.iid.FirebaseInstanceIdService;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;


public class MyHealthRecordFirebaseInstanceIDService extends FirebaseInstanceIdService {
    private static final String TAG = MyHealthRecordFirebaseInstanceIDService.class.getSimpleName();

    @Override
    public void onTokenRefresh() {
        super.onTokenRefresh();

        // Get updated InstanceID token.
        String refreshedToken = FirebaseInstanceId.getInstance().getToken();

        Log.d(TAG, "Refreshed token: " + refreshedToken);

        // Saving reg id to shared preferences
        Functions.storeRegIdInPref(this, refreshedToken);

    }


}


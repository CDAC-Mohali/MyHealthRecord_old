package com.pkg.healthrecordappname.appfinalname.modules.service;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;

/**
 * Created by eSanj on 2/21/2017.
 */

public class CancelNotificationReceiver extends BroadcastReceiver
{
    @Override
    public void onReceive(Context context, Intent intent)
    {
        SharedPreferences sharedPreferences = context.getSharedPreferences("NotificationData", 0);
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.clear();
        editor.apply();
    }
}
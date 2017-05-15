package com.pkg.healthrecordappname.appfinalname.modules.service;

import android.content.Context;
import android.content.Intent;
import android.support.v4.content.LocalBroadcastManager;
import android.text.TextUtils;
import android.util.Log;

import com.google.firebase.messaging.FirebaseMessagingService;
import com.google.firebase.messaging.RemoteMessage;
import com.pkg.healthrecordappname.appfinalname.PHRMS_LoginActivity;
import com.pkg.healthrecordappname.appfinalname.modules.appconfig.Config;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.util.NotificationUtils;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Date;
import java.util.Map;

public class MyHealthRecordFirebaseMessagingService extends FirebaseMessagingService {

    private static final String TAG = MyHealthRecordFirebaseMessagingService.class.getSimpleName();

    private NotificationUtils notificationUtils;

    @Override
    public void onMessageReceived(RemoteMessage remoteMessage)
    {

        super.onMessageReceived(remoteMessage);

        Log.e(TAG, "From: " + remoteMessage.getFrom());

        if (remoteMessage == null)
            return;


        // Check if message contains a data payload.

        if (remoteMessage.getData().size() > 0) {
            Log.e(TAG, "Data Payload: " + remoteMessage.getData().toString());

            try {
                Map<String, String> params = remoteMessage.getData();
                JSONObject json = new JSONObject(params);//remoteMessage.getData().toString());
                handleDataMessage(json);
            } catch (Exception e) {
                Log.e(TAG, "Exception: " + e.getMessage());
            }
        }

        // Check if message only contains a notification payload.
        if (remoteMessage.getNotification() != null) {
            Log.e(TAG, "Notification Body: " + remoteMessage.getNotification().getBody());
            handleNotification(remoteMessage.getNotification().getTitle(), remoteMessage.getNotification().getBody(), String.valueOf((new Date()).getTime()));
        }
    }

    private void handleNotification(String title, String message, String timeStamp) {
        if (!NotificationUtils.isAppIsInBackground(getApplicationContext())) {

            Intent pushNotification = new Intent(Config.PUSH_NOTIFICATION);
            pushNotification.putExtra("message", message);
            LocalBroadcastManager.getInstance(this).sendBroadcast(pushNotification);
            showNotificationMessage(getApplicationContext(),title, message, timeStamp, pushNotification);
        }
        else
        {

        }
    }

    private void handleDataMessage(JSONObject json) {
        Log.e(TAG, "push json: " + json.toString());

        try {
            String title = json.getString("title");
            String message = json.getString("message");
            boolean isBackground = json.getBoolean("is_background");
            String imageUrl = json.getString("image");
            String timestamp = json.getString("timestamp");

            JSONObject payloadjson = new JSONObject(json.getString("payload"));
            JSONObject payload_data = new JSONObject(payloadjson.getString("data"));
            String dt = payload_data.getString("key");

            Log.e(TAG, "title: " + title);
            Log.e(TAG, "message: " + message);
            Log.e(TAG, "isBackground: " + isBackground);
            Log.e(TAG, "imageUrl: " + imageUrl);
            Log.e(TAG, "timestamp: " + timestamp);

            Log.e(TAG, "payload: " + payloadjson.toString());
            Log.e(TAG, "payload data Key: " + dt.toString());


            if (!NotificationUtils.isAppIsInBackground(getApplicationContext())) {
                // app is in foreground, broadcast the push message
                Intent pushNotification = new Intent(Config.PUSH_NOTIFICATION);
                pushNotification.putExtra("title", title);
                pushNotification.putExtra("message", message);
                pushNotification.putExtra("is_background", isBackground);
                pushNotification.putExtra("imageUrl", imageUrl);
                pushNotification.putExtra("timestamp", timestamp);
                pushNotification.putExtra("payload", dt.toString());
                LocalBroadcastManager.getInstance(this).sendBroadcast(pushNotification);

                showNotificationMessage(getApplicationContext(),title, message, timestamp, pushNotification);


            }
            else
            {
                // This section will be handled when only there is Data in Notification
                // app is in background, show the notification in notification tray
                Intent resultIntent = new Intent(getApplicationContext(), PHRMS_LoginActivity.class);



                //resultIntent.putExtra("message", message);
                resultIntent.putExtra("title", title);
                resultIntent.putExtra("message", message);
                resultIntent.putExtra("is_background", isBackground);
                resultIntent.putExtra("imageUrl", imageUrl);
                resultIntent.putExtra("timestamp", timestamp);
                resultIntent.putExtra("payload", dt.toString());


                // check for image attachment
                if (TextUtils.isEmpty(imageUrl))
                {
                    showNotificationMessage(getApplicationContext(), title, message, timestamp, resultIntent);
                }
                else
                {
                    // image is present, show notification with image
                    showNotificationMessageWithBigImage(getApplicationContext(), title, message, timestamp, resultIntent, imageUrl);
                }
            }
        } catch (JSONException e) {
            Log.e(TAG, "Json Exception: " + e.getMessage());
        } catch (Exception e) {
            Log.e(TAG, "Exception: " + e.getMessage());
        }
    }

    /**
     * Showing notification with text only
     */
    private void showNotificationMessage(Context context, String title, String message, String timeStamp, Intent intent) {
        notificationUtils = new NotificationUtils(context);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        notificationUtils.showNotificationMessage(title, message, timeStamp, intent);
    }

    /**
     * Showing notification with text and image
     */
    private void showNotificationMessageWithBigImage(Context context, String title, String message, String timeStamp, Intent intent, String imageUrl) {
        notificationUtils = new NotificationUtils(context);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        notificationUtils.showNotificationMessage(title, message, timeStamp, intent, imageUrl);
    }

}

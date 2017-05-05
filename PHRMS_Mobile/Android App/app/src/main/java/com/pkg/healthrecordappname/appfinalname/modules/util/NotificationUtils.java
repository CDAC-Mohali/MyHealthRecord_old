package com.pkg.healthrecordappname.appfinalname.modules.util;

import android.app.ActivityManager;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.media.Ringtone;
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.Build;
import android.support.v4.app.NotificationCompat;
import android.support.v4.content.ContextCompat;
import android.text.Html;
import android.text.TextUtils;
import android.util.Patterns;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.appconfig.Config;
import com.pkg.healthrecordappname.appfinalname.modules.service.CancelNotificationReceiver;

import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Random;

public class NotificationUtils {

    private static String TAG = NotificationUtils.class.getSimpleName();

    private Context mContext;

    public NotificationUtils(Context mContext) {
        this.mContext = mContext;
    }

    public void showNotificationMessage(String title, String message, String timeStamp, Intent intent) {
        showNotificationMessage(title, message, timeStamp, intent, null);
    }

    // Main Message for notifications
    public void showNotificationMessage(final String title, final String message, final String timeStamp, Intent intent, String imageUrl)
    {
        // Check for empty push message
        if (TextUtils.isEmpty(message))
            return;


        final int largeicon = R.mipmap.ic_launcher;

        int color = ContextCompat.getColor(mContext, R.color.button_color_selected);

        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);

        Intent onCancelNotificationReceiver = new Intent(mContext, CancelNotificationReceiver.class);
        PendingIntent onCancelNotificationReceiverPendingIntent = PendingIntent.getBroadcast(mContext, 0, onCancelNotificationReceiver, 0);

        final PendingIntent resultPendingIntent =
                PendingIntent.getActivity(
                        mContext,
                        0,
                        intent,
                        PendingIntent.FLAG_CANCEL_CURRENT
                );

        final NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(mContext);

        final Uri alarmSound = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);

        if (!TextUtils.isEmpty(imageUrl))
        {
            if (imageUrl != null && imageUrl.length() > 4 && Patterns.WEB_URL.matcher(imageUrl).matches())
            {
                Bitmap bitmap = getBitmapFromURL(imageUrl);

                if (bitmap != null) {
                    showBigNotification(bitmap, mBuilder, largeicon, title, message, timeStamp, resultPendingIntent, alarmSound, color, onCancelNotificationReceiverPendingIntent);
                } else {
                    showSmallNotification(mBuilder, largeicon, title, message, timeStamp, resultPendingIntent, alarmSound, color, onCancelNotificationReceiverPendingIntent);
                }
                playNotificationSound();
            }
        }
        else
        {
            showSmallNotification(mBuilder, largeicon, title, message, timeStamp, resultPendingIntent, alarmSound, color, onCancelNotificationReceiverPendingIntent);
            playNotificationSound();
        }
    }


    private void showSmallNotification(NotificationCompat.Builder mBuilder, int largeicon, String title, String message, String timeStamp, PendingIntent resultPendingIntent, Uri alarmSound, int color, PendingIntent onCancelNotificationReceiverPendingIntent)
    {
        String actual_message = message;

        if (Build.VERSION.SDK_INT >= 24)
        {
            actual_message = Html.fromHtml(message, 0).toString(); // for 24 api and more
        }
        else
        {
            actual_message = Html.fromHtml(message).toString(); // or for older api
        }

        NotificationCompat.BigTextStyle bigText = new NotificationCompat.BigTextStyle();
        bigText.bigText(actual_message);
        bigText.setBigContentTitle(title);
        bigText.setSummaryText("From: " + title);



        NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(mContext)

                .setTicker(title).setWhen(0)
                .setAutoCancel(true)
                .setContentTitle(title)
                .setContentIntent(resultPendingIntent)
                .setDeleteIntent(onCancelNotificationReceiverPendingIntent)
                .setSound(alarmSound)
                .setStyle(bigText) //bigPictureStyle
                .setWhen(getTimeMilliSec(timeStamp))
                .setSmallIcon(getNotificationIcon())
                .setColor(color)
                .setLargeIcon(BitmapFactory.decodeResource(mContext.getResources(), largeicon))
                .setGroup(Config.GROUP_KEY)
                .setContentText(message);


        NotificationManager notificationManager = (NotificationManager) mContext.getSystemService(Context.NOTIFICATION_SERVICE);

        SharedPreferences sharedPreferences = mContext.getSharedPreferences("NotificationData", 0);
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.putString(String.valueOf(new Random(Config.NOTIFICATION_ID)), actual_message);
        editor.apply();

        notificationManager.notify(Config.NOTIFICATION_ID, notificationBuilder.build());
    }

    private void showBigNotification(Bitmap bitmap, NotificationCompat.Builder mBuilder, int largeicon, String title, String message, String timeStamp, PendingIntent resultPendingIntent, Uri alarmSound, int color, PendingIntent onCancelNotificationReceiverPendingIntent)
    {
        String actual_message = message;

        if (Build.VERSION.SDK_INT >= 24)
        {
            actual_message = Html.fromHtml(message, 0).toString(); // for 24 api and more
        }
        else
        {
            actual_message = Html.fromHtml(message).toString(); // or for older api
        }

        NotificationCompat.BigTextStyle bigText = new NotificationCompat.BigTextStyle();
        bigText.bigText(actual_message);
        bigText.setBigContentTitle(title);
        bigText.setSummaryText("From: " + title);




        NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(mContext)
                .setTicker(title).setWhen(0)
                .setAutoCancel(true)
                .setContentTitle(title)
                .setContentIntent(resultPendingIntent)
                .setDeleteIntent(onCancelNotificationReceiverPendingIntent)
                .setSound(alarmSound)
                .setStyle(bigText) //bigPictureStyle
                .setWhen(getTimeMilliSec(timeStamp))
                .setSmallIcon(getNotificationIcon())
                .setColor(color)
                .setLargeIcon(BitmapFactory.decodeResource(mContext.getResources(), largeicon))
                .setGroup(Config.GROUP_KEY)
                .setContentText(message);


        NotificationManager notificationManager = (NotificationManager) mContext.getSystemService(Context.NOTIFICATION_SERVICE);

        SharedPreferences sharedPreferences = mContext.getSharedPreferences("NotificationData", 0);
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.putString(String.valueOf(new Random(Config.NOTIFICATION_ID)), actual_message);
        editor.apply();

        notificationManager.notify(Config.NOTIFICATION_ID_BIG_IMAGE, notificationBuilder.build());
    }

    /**
     * Downloading push notification image before displaying it in
     * the notification tray
     */
    public Bitmap getBitmapFromURL(String strURL) {
        try {
            URL url = new URL(strURL);
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setDoInput(true);
            connection.connect();
            InputStream input = connection.getInputStream();
            Bitmap myBitmap = BitmapFactory.decodeStream(input);
            return myBitmap;
        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }

    // Playing notification sound
    public void playNotificationSound() {
        try {

            Uri alarmSound = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);

            Ringtone r = RingtoneManager.getRingtone(mContext, alarmSound);
            r.play();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    /**
     * Method checks if the app is in background or not
     */
    public static boolean isAppIsInBackground(Context context) {
        boolean isInBackground = true;
        ActivityManager am = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        if (Build.VERSION.SDK_INT > Build.VERSION_CODES.KITKAT_WATCH) {
            List<ActivityManager.RunningAppProcessInfo> runningProcesses = am.getRunningAppProcesses();
            for (ActivityManager.RunningAppProcessInfo processInfo : runningProcesses) {
                if (processInfo.importance == ActivityManager.RunningAppProcessInfo.IMPORTANCE_FOREGROUND) {
                    for (String activeProcess : processInfo.pkgList) {
                        if (activeProcess.equals(context.getPackageName())) {
                            isInBackground = false;
                        }
                    }
                }
            }
        } else {
            List<ActivityManager.RunningTaskInfo> taskInfo = am.getRunningTasks(1);
            ComponentName componentInfo = taskInfo.get(0).topActivity;
            if (componentInfo.getPackageName().equals(context.getPackageName())) {
                isInBackground = false;
            }
        }

        return isInBackground;
    }

    // Clears notification tray messages
    public static void clearNotifications(Context context) {
        NotificationManager notificationManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.cancelAll();
    }

    public static long getTimeMilliSec(String timeStamp) {
        SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        try {
            Date date = format.parse(timeStamp);
            return date.getTime();
        } catch (ParseException e) {
            e.printStackTrace();
        }
        return 0;
    }

    // for FCM icon must be White or Transparent only
    private int getNotificationIcon() {
        boolean useWhiteIcon = (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.LOLLIPOP);
        return useWhiteIcon ? R.mipmap.appicon : R.mipmap.ic_launcher;
    }

}

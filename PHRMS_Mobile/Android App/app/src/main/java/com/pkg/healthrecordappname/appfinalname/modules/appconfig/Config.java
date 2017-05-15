package com.pkg.healthrecordappname.appfinalname.modules.appconfig;



public class Config {

    // global topic to receive app wide push notifications
    public static final String TOPIC_GLOBAL = "appname";

    // broadcast receiver intent filters
    public static final String REGISTRATION_COMPLETE = "com.pkg.healthrecordappname.appfinalname.registrationComplete";
    public static final String PUSH_NOTIFICATION = "com.pkg.healthrecordappname.appfinalname.pushNotification";

    // id to handle the notification in the notification tray
    //public static final int NOTIFICATION_ID = 100;

    public static final int NOTIFICATION_ID = 1050;

    public static final String GROUP_KEY = "GROUP_KEY_RANDOM_NAME";

    public static final int NOTIFICATION_ID_BIG_IMAGE = 101;

    public static final String SHARED_PREF = "appname_firebase";


}

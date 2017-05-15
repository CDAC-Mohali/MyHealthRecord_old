package com.pkg.healthrecordappname.appfinalname.modules.useables;

import android.app.Activity;
import android.content.ContentProviderOperation;
import android.content.ContentResolver;
import android.content.OperationApplicationException;
import android.database.Cursor;
import android.net.Uri;
import android.os.RemoteException;
import android.provider.ContactsContract;
import android.util.Log;

import java.util.ArrayList;
import java.util.regex.Pattern;


public class ContactHelper {

    //Specific Contact Search and Search with null
    public static Cursor getAllContactCursor(ContentResolver contactHelper, String startsWith) {

        String[] projection = {ContactsContract.CommonDataKinds.Phone._ID, ContactsContract.CommonDataKinds.Phone.DISPLAY_NAME, ContactsContract.CommonDataKinds.Phone.NUMBER};
        Cursor cur = null;

        try {
            if (startsWith != null && !startsWith.equals("")) {
                cur = contactHelper.query(ContactsContract.CommonDataKinds.Phone.CONTENT_URI, projection, ContactsContract.CommonDataKinds.Phone.DISPLAY_NAME + " like \"" + startsWith + "%\"", null, ContactsContract.CommonDataKinds.Phone.DISPLAY_NAME + " ASC");
            } else {
                cur = contactHelper.query(ContactsContract.CommonDataKinds.Phone.CONTENT_URI, projection, null, null, ContactsContract.CommonDataKinds.Phone.DISPLAY_NAME + " ASC");
            }
            cur.moveToFirst();
        } catch (Exception e) {
            e.printStackTrace();
        }

        return cur;
    }

    //Specific Contact Search
    public static Cursor getSelectedContactDataCursor(ContentResolver contactHelper, String id) {
        Cursor cur = null;

        try {

            // query for everything email
            cur = contactHelper.query(ContactsContract.CommonDataKinds.Phone.CONTENT_URI, null, ContactsContract.CommonDataKinds.Phone.CONTACT_ID + "=?", new String[]{id}, null);
        } catch (Exception e) {
            e.printStackTrace();
        }

        return cur;
    }

    //Insert Contact
    public static boolean insertContact(ContentResolver contactAdder, String firstName, String mobileNumber) {
        ArrayList<ContentProviderOperation> ops = new ArrayList<ContentProviderOperation>();
        ops.add(ContentProviderOperation.newInsert(ContactsContract.RawContacts.CONTENT_URI).withValue(ContactsContract.RawContacts.ACCOUNT_TYPE, null).withValue(ContactsContract.RawContacts.ACCOUNT_NAME, null).build());
        ops.add(ContentProviderOperation.newInsert(ContactsContract.Data.CONTENT_URI).withValueBackReference(ContactsContract.Data.RAW_CONTACT_ID, 0).withValue(ContactsContract.Data.MIMETYPE, ContactsContract.CommonDataKinds.StructuredName.CONTENT_ITEM_TYPE).withValue(ContactsContract.CommonDataKinds.StructuredName.GIVEN_NAME, firstName).build());
        ops.add(ContentProviderOperation.newInsert(ContactsContract.Data.CONTENT_URI).withValueBackReference(ContactsContract.Data.RAW_CONTACT_ID, 0).withValue(ContactsContract.Data.MIMETYPE, ContactsContract.CommonDataKinds.Phone.CONTENT_ITEM_TYPE).withValue(ContactsContract.CommonDataKinds.Phone.NUMBER, mobileNumber).withValue(ContactsContract.CommonDataKinds.Phone.TYPE, ContactsContract.CommonDataKinds.Phone.TYPE_MOBILE).build());
        try {
            contactAdder.applyBatch(ContactsContract.AUTHORITY, ops);
        } catch (Exception e) {
            return false;
        }
        return true;
    }

    //Delete Conatct
    public static void deleteContact(ContentResolver contactHelper, String number) {
        ArrayList<ContentProviderOperation> ops = new ArrayList<ContentProviderOperation>();
        String[] args = new String[]{String.valueOf(getContactID(contactHelper, number))};
        ops.add(ContentProviderOperation.newDelete(ContactsContract.RawContacts.CONTENT_URI).withSelection(ContactsContract.RawContacts.CONTACT_ID + "=?", args).build());
        try {
            contactHelper.applyBatch(ContactsContract.AUTHORITY, ops);
        } catch (RemoteException e) {
            e.printStackTrace();
        } catch (OperationApplicationException e) {
            e.printStackTrace();
        }
    }

    //Get ID
    private static long getContactID(ContentResolver contactHelper, String number) {
        Uri contactUri = Uri.withAppendedPath(ContactsContract.PhoneLookup.CONTENT_FILTER_URI, Uri.encode(number));
        String[] projection = {ContactsContract.PhoneLookup._ID};
        Cursor cursor = null;
        try {
            cursor = contactHelper.query(contactUri, projection, null, null, null);
            if (cursor.moveToFirst()) {
                int personID = cursor.getColumnIndex(ContactsContract.PhoneLookup._ID);
                return cursor.getLong(personID);
            }
            return -1;
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            if (cursor != null) {
                cursor.close();
                cursor = null;
            }
        }
        return -1;
    }

    // Get Normal number to store in MyhealthRecord DB
    public static String getNumber(String moNumber, Activity act) {
        Pattern special = Pattern.compile("[!@#$%&*()_+=|<>?{}\\[\\]~-]");

        if (moNumber.isEmpty() || moNumber.length() < 10) {
            Functions.showToast(act, "Please input valid Number");
            return null;
        } else if (moNumber.length() > 10 && !special.matcher(moNumber).find()) {
            String[] number = moNumber.split("");
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = moNumber.length(); i > moNumber.length() - 10; i--) {
                stringBuilder.append(number[i]);
            }
            String reverse = new StringBuffer(stringBuilder).reverse().toString();
            return reverse;
        } else if (moNumber.length() > 10 && special.matcher(moNumber).find()) {
            String numberOnly = moNumber.replaceAll("[^0-9]", "");
            String[] number = numberOnly.split("");
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = numberOnly.length(); i > numberOnly.length() - 10; i--) {
                stringBuilder.append(number[i]);
            }
            String reverse = new StringBuffer(stringBuilder).reverse().toString();
            Log.d("mobilenumberspecial", reverse);
            return reverse;
        } else {
            return moNumber;
        }
    }


    public static Cursor getEmailDetails(ContentResolver cr, String id) {
        Cursor cur = cr.query(ContactsContract.Contacts.CONTENT_URI, null, null, null, null);
        Cursor cur_email = null;

        if (cur.getCount() > 0) {
            cur_email = cr.query(ContactsContract.CommonDataKinds.Email.CONTENT_URI, null, ContactsContract.CommonDataKinds.Email.CONTACT_ID + " = ?", new String[]{id}, null);
        }
        return cur_email;
    }


}

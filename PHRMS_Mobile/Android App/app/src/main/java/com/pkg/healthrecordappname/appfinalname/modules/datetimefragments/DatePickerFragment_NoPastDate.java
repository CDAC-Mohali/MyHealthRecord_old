package com.pkg.healthrecordappname.appfinalname.modules.datetimefragments;

import android.app.DatePickerDialog;
import android.app.Dialog;
import android.app.DialogFragment;
import android.content.DialogInterface;
import android.os.Bundle;
import android.widget.DatePicker;
import android.widget.EditText;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Locale;

public class DatePickerFragment_NoPastDate extends DialogFragment implements
        DatePickerDialog.OnDateSetListener, DatePickerDialog.OnDismissListener {

    static EditText _etDate;
    public static Boolean ready = true;

    public static DatePickerFragment_NoPastDate newInstance(EditText etDate) {
        if (ready) {
            DatePickerFragment_NoPastDate frag = new DatePickerFragment_NoPastDate();
            _etDate = etDate;
            ready = false;
            return frag;
        } else {
            return null;
        }
    }

    @Override
    public Dialog onCreateDialog(Bundle savedInstanceState) {

        int year = 0, month = 0, day = 0;
        String[] dateParts = _etDate.getText().toString().trim().split("/");
        if (dateParts.length == 3) {
            try {

                // converted to dd/MM/yyyy
                day = Integer.parseInt(dateParts[0]);
                month = Integer.parseInt(dateParts[1]) - 1;
                year = Integer.parseInt(dateParts[2]);
            } catch (NumberFormatException ex) {

            }
        } else {
            Calendar c = Calendar.getInstance();
            year = c.get(Calendar.YEAR);
            month = c.get(Calendar.MONTH);
            day = c.get(Calendar.DAY_OF_MONTH);
        }


        // No past Dates

        DatePickerDialog datePickerDialog = new DatePickerDialog(getActivity(),this, year, month, day);
        datePickerDialog.getDatePicker().setMinDate(System.currentTimeMillis() - 1000);

        return  datePickerDialog;
    }

    public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
        Calendar c = Calendar.getInstance();
        c.set(year, monthOfYear, dayOfMonth);


        SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
        _etDate.setText(sdf.format(c.getTime()));
        ready = true;
    }

    public void onDismiss(DialogInterface dialog) {
        ready = true;
        super.onDismiss(dialog);
    }
}
package com.pkg.healthrecordappname.appfinalname.modules.datetimefragments;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

public class DateValidator {

    public boolean isThisDateValid(String dateToValidate, String dateFromat){

        if(dateToValidate == null || dateToValidate.isEmpty())
        {
            return false;
        }

        SimpleDateFormat sdf = new SimpleDateFormat(dateFromat);
        sdf.setLenient(false);

        try
        {
            //if not valid, it will throw ParseException
            Date date = sdf.parse(dateToValidate);


        }
        catch (ParseException e)
        {

            e.printStackTrace();
            return false;
        }

        return true;
    }


}

/**
 * Copyright (C) 2014 Samsung Electronics Co., Ltd. All rights reserved.
 * <p>
 * Mobile Communication Division,
 * Digital Media & Communications Business, Samsung Electronics Co., Ltd.
 * <p>
 * This software and its documentation are confidential and proprietary
 * information of Samsung Electronics Co., Ltd.  No part of the software and
 * documents may be copied, reproduced, transmitted, translated, or reduced to
 * any electronic medium or machine-readable form without the prior written
 * consent of Samsung Electronics.
 * <p>
 * Samsung Electronics makes no representations with respect to the contents,
 * and assumes no responsibility for any errors that might appear in the
 * software and documents. This publication and the contents hereof are subject
 * to change without notice.
 */

package com.pkg.healthrecordappname.appfinalname.modules.fragments;

import android.content.SharedPreferences;
import android.database.Cursor;
import android.util.Log;

import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.samsung.android.sdk.healthdata.HealthConstants;
import com.samsung.android.sdk.healthdata.HealthDataObserver;
import com.samsung.android.sdk.healthdata.HealthDataResolver;
import com.samsung.android.sdk.healthdata.HealthDataResolver.Filter;
import com.samsung.android.sdk.healthdata.HealthDataResolver.ReadRequest;
import com.samsung.android.sdk.healthdata.HealthDataResolver.ReadResult;
import com.samsung.android.sdk.healthdata.HealthDataStore;
import com.samsung.android.sdk.healthdata.HealthResultHolder;

import java.util.concurrent.TimeUnit;

public class ExerciseDataCountReporter {
    private final HealthDataStore mStore;


    int WalkingType = 1001;
    int RunningType = 1002;
    int Cycling = 11007;
    int Hiking = 13001;
    int Swimming = 14001;



    public ExerciseDataCountReporter(HealthDataStore store) {
        mStore = store;
    }

    public void start() {
        // Register an observer to listen changes of Excersie count and get today Excersie count
        HealthDataObserver.addObserver(mStore, HealthConstants.Exercise.HEALTH_DATA_TYPE, mObserver);
        readExerciseData();
    }


    // Read the today's Walking count on demand
    private void readExerciseData() {
        HealthDataResolver resolver = new HealthDataResolver(mStore, null);

        // Set time range from start time of today to the current time
        // Shared Prefrence for PREF_LastTimeSync Updated at Button Sync Pressed
        Long ST = Functions.pref.getLong(Functions.PREF_LastTimeSync, -1);
        long startTime = (ST!=-1)? ST : Functions.getStartTimeOfToday();


        long endTime = System.currentTimeMillis();

        Filter filter = Filter.and(Filter.greaterThanEquals(HealthConstants.Exercise.START_TIME, startTime),
                Filter.lessThanEquals(HealthConstants.Exercise.START_TIME, endTime));

        HealthDataResolver.ReadRequest request = new ReadRequest.Builder()
                .setDataType(HealthConstants.Exercise.HEALTH_DATA_TYPE)
                .setProperties(new String[]{HealthConstants.Exercise.EXERCISE_TYPE, HealthConstants.Exercise.COUNT, HealthConstants.Exercise.DISTANCE,HealthConstants.Exercise.DURATION})
                .setFilter(filter)
                .build();

        try
        {
            resolver.read(request).setResultListener(mListener);
        }
        catch (Exception e)
        {
            Log.e(Functions.APP_TAG, e.getClass().getName() + " - " + e.getMessage());
            Log.e(Functions.APP_TAG, "Getting Exercise data fails. - Enable Developer Mode for Shealth");
            Log.e(Functions.APP_TAG, "Enable Developer Mode for Shealth");

            Functions.SHealthShowToast();
        }
    }

    private final HealthResultHolder.ResultListener<ReadResult> mListener = new HealthResultHolder.ResultListener<ReadResult>() {
        @Override
        public void onResult(ReadResult result)
        {


            int Wcount = 0;
            float WDistanceExr = 0.0f;
            int WMinutes = 0;

            int Rcount = 0;
            float RDistanceExr = 0.0f;
            int RMinutes = 0;

            Cursor c = null;

            try {
                c = result.getResultCursor();
                if (c != null) {
                    while (c.moveToNext()) {
                        // Walking - EXERCISE_TYPE
                        if (WalkingType == c.getInt(c.getColumnIndex(HealthConstants.Exercise.EXERCISE_TYPE)))
                        {
                            Wcount += c.getInt(c.getColumnIndex(HealthConstants.Exercise.COUNT));
                            // Distance in Metre - Convert to KM
                            WDistanceExr += c.getFloat(c.getColumnIndex(HealthConstants.Exercise.DISTANCE));

                            String wmin = String.format("%d", TimeUnit.MILLISECONDS.toMinutes(c.getInt(c.getColumnIndex(HealthConstants.Exercise.DURATION))));

                            WMinutes += Integer.parseInt(wmin);
                        }

                        // Running - EXERCISE_TYPE
                        if (RunningType == c.getInt(c.getColumnIndex(HealthConstants.Exercise.EXERCISE_TYPE)))
                        {
                            Rcount += c.getInt(c.getColumnIndex(HealthConstants.Exercise.COUNT));
                            // Distance in Metre - Convert to KM
                            RDistanceExr += c.getFloat(c.getColumnIndex(HealthConstants.Exercise.DISTANCE));

                            String rmin = String.format("%d", TimeUnit.MILLISECONDS.toMinutes(c.getInt(c.getColumnIndex(HealthConstants.Exercise.DURATION))));
                            // 120000 Miliseconds = 2 Minutes
                            RMinutes += Integer.parseInt(rmin);
                        }

                        // Swing - Swim
                    }
                }
            } finally {
                if (c != null) {
                    c.close();
                }
            }

            SharedPreferences.Editor editor = Functions.pref.edit();
            editor.putFloat(Functions.PREF_WC, WDistanceExr);
            editor.putInt(Functions.PREF_WC_Minutes, WMinutes);

            editor.putFloat(Functions.PREF_RC, RDistanceExr);
            editor.putInt(Functions.PREF_RC_Minutes, RMinutes);
            editor.commit();

            Functions.SHealthDataCount();




        }
    };

    private final HealthDataObserver mObserver = new HealthDataObserver(null) {
        // Update the step count when a change event is received
        @Override
        public void onChange(String dataTypeName) {
            Log.d(Functions.APP_TAG, "Observer receives a data changed event");
            readExerciseData();
        }
    };

}

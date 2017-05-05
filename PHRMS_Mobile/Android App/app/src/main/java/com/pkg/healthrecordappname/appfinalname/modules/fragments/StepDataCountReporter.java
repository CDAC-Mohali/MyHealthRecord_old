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

public class StepDataCountReporter {
    private final HealthDataStore mStore;

    public StepDataCountReporter(HealthDataStore store) {
        mStore = store;
    }

    public void start() {
        // Register an observer to listen changes of step count and get today step count
        HealthDataObserver.addObserver(mStore, HealthConstants.StepCount.HEALTH_DATA_TYPE, mObserver);
        readTodayStepCount();
    }

    // Read the today's step count on demand
    private void readTodayStepCount() {
        HealthDataResolver resolver = new HealthDataResolver(mStore, null);

        // Set time range from start time of today to the current time
        // Shared Prefrence for PREF_LastTimeSync Updated at Button Sync Pressed

        Long ST = Functions.pref.getLong(Functions.PREF_LastTimeSync, -1);
        long startTime = (ST != -1) ? ST : Functions.getStartTimeOfToday();


        long endTime = System.currentTimeMillis();

        Filter filter = Filter.and(Filter.greaterThanEquals(HealthConstants.StepCount.START_TIME, startTime),
                Filter.lessThanEquals(HealthConstants.StepCount.START_TIME, endTime));

        HealthDataResolver.ReadRequest request = new ReadRequest.Builder()
                .setDataType(HealthConstants.StepCount.HEALTH_DATA_TYPE)
                .setProperties(new String[]{HealthConstants.StepCount.DEVICE_UUID, HealthConstants.StepCount.UUID, HealthConstants.StepCount.COUNT, HealthConstants.StepCount.DISTANCE, HealthConstants.StepCount.START_TIME, HealthConstants.StepCount.END_TIME})
                .setFilter(filter)
                .build();

        try {
            resolver.read(request).setResultListener(mListener);
        } catch (Exception e) {
            // This
            Log.e(Functions.APP_TAG, e.getClass().getName() + " - " + e.getMessage());
            Log.e(Functions.APP_TAG, "Getting step count fails. - Enable Developer Mode for Shealth");
            Log.e(Functions.APP_TAG, "Enable Developer Mode for Shealth");

            Functions.SHealthShowToast();
        }
    }

    private final HealthResultHolder.ResultListener<ReadResult> mListener = new HealthResultHolder.ResultListener<ReadResult>() {
        @Override
        public void onResult(ReadResult result) {
            int count = 0;
            float DistanceSC = 0.0f;
            Cursor c = null;
            int SCMinutes = 0;

            try {
                // Data DEVICE_UUID,UUID - Datauuid, COUNT,DISTANCE,START_TIME,END_TIME
                c = result.getResultCursor();

                if (c != null) {
                    while (c.moveToNext()) {

                        count += c.getInt(c.getColumnIndex(HealthConstants.StepCount.COUNT));

                        // Distance in Metre - Convert to KM

                        DistanceSC += c.getFloat(c.getColumnIndex(HealthConstants.StepCount.DISTANCE));

                        // Start Time in Milliseconds Convert to Minutes as required to store in activities
                        String ST = c.getString(c.getColumnIndex(HealthConstants.StepCount.START_TIME));
                        // Start Time in Milliseconds Convert to Minutes as required to store in activities
                        String ET = c.getString(c.getColumnIndex(HealthConstants.StepCount.END_TIME));


                        // Start Time and End Time in Milliseconds Convert to Minutes as required to store in activities
                        long DurationTime = Long.parseLong(c.getString(c.getColumnIndex(HealthConstants.StepCount.END_TIME))) - Long.parseLong(c.getString(c.getColumnIndex(HealthConstants.StepCount.START_TIME)));


                        SCMinutes += Integer.parseInt(String.format("%d", TimeUnit.MILLISECONDS.toMinutes(DurationTime)));
                    }
                }
            } finally {
                if (c != null) {
                    c.close();
                }
            }

            SharedPreferences.Editor editor = Functions.pref.edit();
            editor.putFloat(Functions.PREF_SC, DistanceSC);
            editor.putInt(Functions.PREF_SC_Minutes, SCMinutes);
            editor.commit();

            Functions.SHealthDataCount();


        }
    };

    private final HealthDataObserver mObserver = new HealthDataObserver(null) {
        // Update the step count when a change event is received
        @Override
        public void onChange(String dataTypeName) {
            Log.d(Functions.APP_TAG, "Observer receives a data changed event");
            readTodayStepCount();
        }
    };

}

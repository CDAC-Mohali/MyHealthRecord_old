package com.pkg.healthrecordappname.appfinalname.modules.fragments;

import android.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.pkg.healthrecordappname.appfinalname.R;

public class PHRMS_About_Fragment extends Fragment {


    public PHRMS_About_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.aboutus, container, false);


        return rootView;
    }

}
package com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues;

import android.app.DialogFragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

public class PHRMS_WellnessBP_Dialogue extends DialogFragment {

    private TextView txtWellnessS_Value;
    private TextView txtWellnessD_Value;
    private TextView txtWellnessP_Value;
    private TextView txtBPDate_value;
    private TextView txtBPN_value;

    public PHRMS_WellnessBP_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.dialogue_wellnessbp, container, false);

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(getActivity());
        } else {


            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");
            getDialog().setTitle("Blood Pressure Details");

            txtWellnessS_Value = (TextView) rootView.findViewById(R.id.txtWellnessS_Value);
            txtWellnessD_Value = (TextView) rootView.findViewById(R.id.txtWellnessD_Value);
            txtWellnessP_Value = (TextView) rootView.findViewById(R.id.txtWellnessP_Value);
            txtBPDate_value = (TextView) rootView.findViewById(R.id.txtBPDate_value);

            txtBPN_value = (TextView) rootView.findViewById(R.id.txtBPN_value);


            if (Display == 1) {
                loadBPData(bundle);
            } else {
                getDialog().cancel();
            }
        }

        return rootView;
    }


    private void loadBPData(Bundle bundle) {
        if (!bundle.isEmpty()) {
            txtWellnessS_Value.setText(bundle.getString("ResSystolic"));
            txtWellnessD_Value.setText(bundle.getString("ResDiastolic"));
            txtWellnessP_Value.setText(bundle.getString("ResPulse"));
            txtBPDate_value.setText(bundle.getString("strCollectionDate"));
            txtBPN_value.setText(bundle.getString("Comments"));
        }
    }

}
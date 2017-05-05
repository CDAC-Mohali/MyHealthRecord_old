package com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues;

import android.app.DialogFragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;


public class PHRMS_WellnessGlucose_Dialogue extends DialogFragment {

    private TextView txtBGResult_value;
    private TextView txtBGVT_value;
    private TextView txtBGDate_value;
    private TextView txtBGN_value;

    public PHRMS_WellnessGlucose_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.dialogue_wellnessglucose, container, false);


        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(getActivity());
        } else {


            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");

            txtBGResult_value = (TextView) rootView.findViewById(R.id.txtBGResult_value);
            txtBGVT_value = (TextView) rootView.findViewById(R.id.txtBGVT_value);
            txtBGDate_value = (TextView) rootView.findViewById(R.id.txtBGDate_value);
            txtBGN_value = (TextView) rootView.findViewById(R.id.txtBGN_value);

            getDialog().setTitle("Blood Glucose Details");

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
            txtBGResult_value.setText(bundle.getString("Result"));
            txtBGVT_value.setText(bundle.getString("ValueType"));
            txtBGDate_value.setText(bundle.getString("strCollectionDate"));
            txtBGN_value.setText(bundle.getString("Comments"));
        }
    }

}
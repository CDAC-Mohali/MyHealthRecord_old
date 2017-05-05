package com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues;

import android.app.DialogFragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

public class PHRMS_Immunization_Dialogue extends DialogFragment {

    private TextView txtimmunizationname_value;
    private TextView txtimmunizationTakenon_value;
    private TextView txtimmunizationComments_value;

    public PHRMS_Immunization_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.dialogue_immunization, container, false);

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(getActivity());
        } else {

            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");
            getDialog().setTitle("Immunization Details");
            txtimmunizationname_value = (TextView) rootView.findViewById(R.id.txtimmunizationname_value);
            txtimmunizationTakenon_value = (TextView) rootView.findViewById(R.id.txtimmunizationTakenon_value);
            txtimmunizationComments_value = (TextView) rootView.findViewById(R.id.txtimmunizationComments_value);

            if (Display == 1) {
                loadImmunizationData(bundle);
            } else {
                getDialog().cancel();
            }
        }

        return rootView;
    }


    private void loadImmunizationData(Bundle bundle) {
        if (!bundle.isEmpty()) {

            txtimmunizationname_value.setText(bundle.getString("ImmunizationName"));

            txtimmunizationTakenon_value.setText(bundle.getString("strImmunizationDate"));

            txtimmunizationComments_value.setText(bundle.getString("Comments"));


        }
    }

}
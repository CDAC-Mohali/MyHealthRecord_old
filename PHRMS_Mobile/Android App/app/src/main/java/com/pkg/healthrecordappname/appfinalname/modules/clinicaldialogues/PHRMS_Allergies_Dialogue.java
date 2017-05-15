package com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues;

import android.app.DialogFragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

public class PHRMS_Allergies_Dialogue extends DialogFragment
{


    private TextView txtallergyname_value;
    private TextView txtallergystill_value;
    private TextView txtallergyfrom_value;
    private TextView txtallergySeverity_value;
    private TextView txtallergynotess_value;

    public PHRMS_Allergies_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.dialogue_allergies, container, false);

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid))
        {
            Functions.mainscreen(getActivity());
        }
        else
        {


            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");
            getDialog().setTitle("Allergy Details");
            txtallergyname_value = (TextView) rootView.findViewById(R.id.txtallergyname_value);
            txtallergystill_value = (TextView) rootView.findViewById(R.id.txtallergystill_value);
            txtallergyfrom_value = (TextView) rootView.findViewById(R.id.txtallergyfrom_value);
            txtallergySeverity_value = (TextView) rootView.findViewById(R.id.txtallergySeverity_value);
            txtallergynotess_value = (TextView) rootView.findViewById(R.id.txtallergynotess_value);

            if (Display == 1) {
                loadAllergyData(bundle);
            } else {
                getDialog().cancel();
            }
        }

        return rootView;
    }


    private void loadAllergyData(Bundle bundle) {
        if (!bundle.isEmpty()) {

            txtallergyname_value.setText(bundle.getString("AllergyName"));


            if (bundle.getString("Still_Have").equals("true")) {
                txtallergystill_value.setText("Yes");
            } else {
                txtallergystill_value.setText("No");
            }

            txtallergyfrom_value.setText(bundle.getString("strDuration"));

            txtallergySeverity_value.setText(bundle.getString("strSeverity"));
            txtallergynotess_value.setText(bundle.getString("Comments"));

        }
    }

}
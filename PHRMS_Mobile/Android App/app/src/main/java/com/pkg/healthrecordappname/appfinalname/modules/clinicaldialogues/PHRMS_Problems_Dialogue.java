package com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues;

import android.app.DialogFragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

public class PHRMS_Problems_Dialogue extends DialogFragment {

    private TextView txtProblemsname_value;
    private TextView txtProblemsstill_value;
    private TextView txtProblemsDD_value;
    private TextView txtProblemsDB_value;
    private TextView txtProblemsnotess_value;

    public PHRMS_Problems_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.dialogue_problems, container, false);

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(getActivity());
        } else {


            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");
            getDialog().setTitle("Problem Details");
            txtProblemsname_value = (TextView) rootView.findViewById(R.id.txtProblemsname_value);
            txtProblemsstill_value = (TextView) rootView.findViewById(R.id.txtProblemsstill_value);
            txtProblemsDD_value = (TextView) rootView.findViewById(R.id.txtProblemsDD_value);
            txtProblemsDB_value = (TextView) rootView.findViewById(R.id.txtProblemsDB_value);
            txtProblemsnotess_value = (TextView) rootView.findViewById(R.id.txtProblemsnotess_value);

            if (Display == 1) {
                loadProblemsData(bundle);
            } else {
                getDialog().cancel();
            }
        }

        return rootView;
    }


    private void loadProblemsData(Bundle bundle) {
        if (!bundle.isEmpty()) {



            txtProblemsname_value.setText(bundle.getString("HealthCondition"));


            if (bundle.getString("StillHaveCondition").equals("true")) {
                txtProblemsstill_value.setText("Yes");
            } else {
                txtProblemsstill_value.setText("No");
            }



            txtProblemsDD_value.setText(bundle.getString("strDiagnosisDate"));

            txtProblemsDB_value.setText(bundle.getString("Provider"));
            txtProblemsnotess_value.setText(bundle.getString("Notes"));

        }
    }

}
package com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues;

import android.app.DialogFragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;


public class PHRMS_WellnessWeight_Dialogue extends DialogFragment {

    private TextView txtWeight_value;
    private TextView txtWeightH_value;
    private TextView txtWeightBMI_value;
    private TextView txtWeightDate_value;
    private TextView txtWeightNotes_value;

    public PHRMS_WellnessWeight_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.dialogue_weight, container, false);

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(getActivity());
        } else {


            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");
            getDialog().setTitle("BMI Details");
            txtWeight_value = (TextView) rootView.findViewById(R.id.txtWeight_value);
            txtWeightH_value = (TextView) rootView.findViewById(R.id.txtWeightH_value);
            txtWeightBMI_value = (TextView) rootView.findViewById(R.id.txtWeightBMI_value);
            txtWeightDate_value = (TextView) rootView.findViewById(R.id.txtWeightDate_value);

            txtWeightNotes_value = (TextView) rootView.findViewById(R.id.txtWeightNotes_value);

            if (Display == 1) {
                loadMedicationsData(bundle);
            } else {
                getDialog().cancel();
            }
        }

        return rootView;
    }


    private void loadMedicationsData(Bundle bundle) {
        if (!bundle.isEmpty()) {
            txtWeight_value.setText(bundle.getString("Weight"));
            txtWeightH_value.setText(bundle.getString("Height"));
            txtWeightBMI_value.setText(bundle.getString("BMI"));
            txtWeightDate_value.setText(bundle.getString("strCollectionDate"));
            txtWeightNotes_value.setText(bundle.getString("Comments"));
        }
    }

}
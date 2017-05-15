package com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues;

import android.app.DialogFragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;


public class PHRMS_WellnessActivities_Dialogue extends DialogFragment {

    private TextView txtActivityName_value;
    private TextView txtActivityPath_value;
    private TextView txtActivityDist_value;
    private TextView txtActivityTT_value;
    private TextView txtActivityDate_value;
    private TextView txtActivityN_value;

    public PHRMS_WellnessActivities_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.dialogue_wellnessactivities, container, false);

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(getActivity());
        } else {


            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");
            getDialog().setTitle("Activity Details");
            txtActivityName_value = (TextView) rootView.findViewById(R.id.txtActivityName_value);
            txtActivityPath_value = (TextView) rootView.findViewById(R.id.txtActivityPath_value);
            txtActivityDist_value = (TextView) rootView.findViewById(R.id.txtActivityDist_value);
            txtActivityTT_value = (TextView) rootView.findViewById(R.id.txtActivityTT_value);

            txtActivityDate_value = (TextView) rootView.findViewById(R.id.txtActivityDate_value);
            txtActivityN_value = (TextView) rootView.findViewById(R.id.txtActivityN_value);


            if (Display == 1) {
                loadActivitysData(bundle);
            } else {
                getDialog().cancel();
            }
        }

        return rootView;
    }


    private void loadActivitysData(Bundle bundle) {
        if (!bundle.isEmpty()) {


            txtActivityName_value.setText(bundle.getString("ActivityName"));
            txtActivityPath_value.setText(bundle.getString("PathName"));
            txtActivityDist_value.setText(bundle.getString("Distance"));
            txtActivityTT_value.setText(bundle.getString("FinishTime"));
            txtActivityDate_value.setText(bundle.getString("strCollectionDate"));
            txtActivityN_value.setText(bundle.getString("Comments"));

        }
    }

}
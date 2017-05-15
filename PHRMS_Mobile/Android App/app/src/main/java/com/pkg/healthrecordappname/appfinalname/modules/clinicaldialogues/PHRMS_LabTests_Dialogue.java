package com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues;

import android.app.DialogFragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

public class PHRMS_LabTests_Dialogue extends DialogFragment {

    private ProgressBar mProgressBarLabTestImage;
    private TextView txtlabtestname_value;
    private TextView txtlabtestResult_value;
    private TextView txtlabtestUnit_value;
    private TextView txtlabtestNotes_value;
    private ImageView imglabtestAttachments_value;

    public PHRMS_LabTests_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.dialogue_labtests, container, false);

        mProgressBarLabTestImage = (ProgressBar) rootView.findViewById(R.id.ProgressBarLabTestImage);

        Functions.progressbarStyle(mProgressBarLabTestImage, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(getActivity());
        }
        else
        {
            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");
            getDialog().setTitle("Test Result Details");
            txtlabtestname_value = (TextView) rootView.findViewById(R.id.txtlabtestname_value);
            txtlabtestResult_value = (TextView) rootView.findViewById(R.id.txtlabtestResult_value);
            txtlabtestUnit_value = (TextView) rootView.findViewById(R.id.txtlabtestUnit_value);
            txtlabtestNotes_value = (TextView) rootView.findViewById(R.id.txtlabtestNotes_value);
            imglabtestAttachments_value = (ImageView) rootView.findViewById(R.id.imglabtestAttachments_value);

            if (Display == 1) {
                loadlabtestsData(bundle);
            } else {
                getDialog().cancel();
            }
        }

        return rootView;
    }


    private void loadlabtestsData(Bundle bundle) {
        if (!bundle.isEmpty()) {

            txtlabtestname_value.setText(bundle.getString("TestName"));

            txtlabtestResult_value.setText(bundle.getString("Result"));

            txtlabtestUnit_value.setText(bundle.getString("Unit"));

            txtlabtestNotes_value.setText(bundle.getString("Comments"));


            String labtestImagePath = bundle.getString("arrImages");

            if (!Functions.isNullOrEmpty(labtestImagePath))
            {
                //Image URL - This can point to any image file supported by Android
                String url = getActivity().getString(R.string.ImagePath) + getActivity().getString(R.string.ImagePathLabTests) + labtestImagePath;

                if (!Functions.isNullOrEmpty(url))
                {

                    mProgressBarLabTestImage.setVisibility(View.VISIBLE);
                    Functions.DispImageFromGlideWithProgressBar(url, getActivity(), imglabtestAttachments_value,mProgressBarLabTestImage,"LabTest");
                }
            }

        }
    }

}
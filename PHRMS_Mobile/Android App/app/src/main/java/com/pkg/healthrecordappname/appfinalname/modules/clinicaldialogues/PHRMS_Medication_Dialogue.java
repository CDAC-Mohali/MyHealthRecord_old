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

public class PHRMS_Medication_Dialogue extends DialogFragment
{
    private ProgressBar mProgressMedicationImage;
    private TextView txtMedicationName_value;
    private TextView txtMedicationAreyou_value;
    private TextView txtMedicationDateFP_value;
    private TextView txtMedicationRoute_value;
    private ImageView imgMedicationAttachments_value;

    private TextView txtMedicationStrength_value;
    private TextView txtMedicationDT_value;
    private TextView txtMedicationFT_value;
    private TextView txtMedicationLI_value;
    private TextView txtMedicationNotes_value;

    public PHRMS_Medication_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {

        final View rootView = inflater.inflate(R.layout.dialogue_medication, container, false);

        mProgressMedicationImage = (ProgressBar) rootView.findViewById(R.id.ProgressBarMedicationImage);

        Functions.progressbarStyle(mProgressMedicationImage, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid))
        {
            Functions.mainscreen(getActivity());
        }
        else
        {
            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");
            getDialog().setTitle("Medication Details");
            txtMedicationName_value = (TextView) rootView.findViewById(R.id.txtMedicationName_value);
            txtMedicationAreyou_value = (TextView) rootView.findViewById(R.id.txtMedicationAreyou_value);
            txtMedicationDateFP_value = (TextView) rootView.findViewById(R.id.txtMedicationDateFP_value);
            txtMedicationRoute_value = (TextView) rootView.findViewById(R.id.txtMedicationRoute_value);

            txtMedicationStrength_value = (TextView) rootView.findViewById(R.id.txtMedicationStrength_value);
            txtMedicationDT_value = (TextView) rootView.findViewById(R.id.txtMedicationDT_value);

            txtMedicationFT_value = (TextView) rootView.findViewById(R.id.txtMedicationFT_value);
            txtMedicationLI_value = (TextView) rootView.findViewById(R.id.txtMedicationLI_value);
            txtMedicationNotes_value = (TextView) rootView.findViewById(R.id.txtMedicationNotes_value);


            imgMedicationAttachments_value = (ImageView) rootView.findViewById(R.id.imgMedicationAttachments_value);

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

            txtMedicationName_value.setText(bundle.getString("MedicineName"));
            txtMedicationAreyou_value.setText(bundle.getString("strTakingMedicine"));
            txtMedicationDateFP_value.setText(bundle.getString("strPrescribedDate"));
            txtMedicationRoute_value.setText(bundle.getString("strRoute"));
            txtMedicationStrength_value.setText(bundle.getString("Strength"));
            txtMedicationDT_value.setText(bundle.getString("strDosValue") + " " + bundle.getString("strDosUnit"));
            txtMedicationFT_value.setText(bundle.getString("strFrequency"));
            txtMedicationLI_value.setText(bundle.getString("LabelInstructions"));
            txtMedicationNotes_value.setText(bundle.getString("Notes"));

            String MedicationImagePath = bundle.getString("arrImages");

            if (!Functions.isNullOrEmpty(MedicationImagePath))
            {
                //Image URL - This can point to any image file supported by Android
                String url = getActivity().getString(R.string.ImagePath) + getActivity().getString(R.string.ImagePathMedication) + MedicationImagePath;

                if (!Functions.isNullOrEmpty(url))
                {
                    mProgressMedicationImage.setVisibility(View.VISIBLE);
                    Functions.DispImageFromGlideWithProgressBar (url, getActivity(), imgMedicationAttachments_value,mProgressMedicationImage,"Medication");
                }
            }
        }
    }

}
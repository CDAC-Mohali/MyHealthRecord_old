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


public class PHRMS_Procedures_Dialogue extends DialogFragment
{
    private ProgressBar mProgressBarProcedureImage;
    private TextView txtprocedurename_value;
    private TextView txtprocedureDOP_value;
    private TextView txtprocedureDB_value;
    private TextView txtprocedureNotes_value;
    private ImageView imgprocedureDS_value;

    public PHRMS_Procedures_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.dialogue_procedure, container, false);


        mProgressBarProcedureImage = (ProgressBar) rootView.findViewById(R.id.ProgressBarProcedureImage);


        Functions.progressbarStyle(mProgressBarProcedureImage, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNullOrEmpty(userid))
        {
            Functions.mainscreen(getActivity());
        }
        else
        {


            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");
            getDialog().setTitle("Procedure Details");
            txtprocedurename_value = (TextView) rootView.findViewById(R.id.txtprocedurename_value);
            txtprocedureDOP_value = (TextView) rootView.findViewById(R.id.txtprocedureDOP_value);
            txtprocedureDB_value = (TextView) rootView.findViewById(R.id.txtprocedureDB_value);
            txtprocedureNotes_value = (TextView) rootView.findViewById(R.id.txtprocedureNotes_value);
            imgprocedureDS_value = (ImageView) rootView.findViewById(R.id.imgprocedureDS_value);

            if (Display == 1) {
                loadproceduresData(bundle);
            } else {
                getDialog().cancel();
            }
        }

        return rootView;
    }


    private void loadproceduresData(Bundle bundle) {
        if (!bundle.isEmpty()) {



            txtprocedurename_value.setText(bundle.getString("ProcedureName"));

            txtprocedureDOP_value.setText(bundle.getString("strEndDate"));

            txtprocedureDB_value.setText(bundle.getString("SurgeonName"));

            txtprocedureNotes_value.setText(bundle.getString("Comments"));


            String ProcedureImagePath = bundle.getString("arrImages");

            if (!Functions.isNullOrEmpty(ProcedureImagePath))
            {
                //Image URL - This can point to any image file supported by Android
                String url = getActivity().getString(R.string.ImagePath) + getActivity().getString(R.string.ImagePathProcedures) + ProcedureImagePath;

                if (!Functions.isNullOrEmpty(url))
                {
                    //Functions.DispImageFromGlide(url, getActivity(), imgprocedureDS_value);
                    mProgressBarProcedureImage.setVisibility(View.VISIBLE);
                    Functions.DispImageFromGlideWithProgressBar (url, getActivity(), imgprocedureDS_value,mProgressBarProcedureImage,"Procedures");
                }
            }
        }
    }

}
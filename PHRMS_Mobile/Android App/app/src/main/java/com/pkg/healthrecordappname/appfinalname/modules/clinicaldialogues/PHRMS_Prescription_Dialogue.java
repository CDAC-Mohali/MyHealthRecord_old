package com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues;

import android.app.DialogFragment;
import android.content.ActivityNotFoundException;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;

public class PHRMS_Prescription_Dialogue extends DialogFragment {
    private ProgressBar mProgressPrescriptionImage;

    private TextView txtPrescriptionDocName_value;
    private TextView txtPrescriptionHCName_value;
    private TextView txtPrescriptionAddress_value;
    private TextView txtPrescriptionPhone_value;
    private ImageView imgPrescriptionAttachments_value;

    private TextView txtPrescriptionD_value;
    private TextView txtPrescriptionDate_value;

    // EMR Link
    private Button mIB_PrescriptionReportEMR;

    // Update Attachment text
    private TextView txtPrescriptionAttachment_value;

    // Report URL
    private String dataUrl = null;

    public PHRMS_Prescription_Dialogue() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {

        final View rootView = inflater.inflate(R.layout.dialogue_prescription, container, false);


        mProgressPrescriptionImage = (ProgressBar) rootView.findViewById(R.id.ProgressBarPrescriptionImage);

        mIB_PrescriptionReportEMR = (Button) rootView.findViewById(R.id.IB_PrescriptionReportEMR);

        Functions.progressbarStyle(mProgressPrescriptionImage, getActivity());


        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNullOrEmpty(userid))
        {
            Functions.mainscreen(getActivity());
        }
        else
        {


            Bundle bundle = this.getArguments();

            int Display = bundle.getInt("Display");
            getDialog().setTitle("Prescription Details");
            txtPrescriptionDocName_value = (TextView) rootView.findViewById(R.id.txtPrescriptionDocName_value);
            txtPrescriptionHCName_value = (TextView) rootView.findViewById(R.id.txtPrescriptionHCName_value);
            txtPrescriptionAddress_value = (TextView) rootView.findViewById(R.id.txtPrescriptionAddress_value);
            txtPrescriptionPhone_value = (TextView) rootView.findViewById(R.id.txtPrescriptionPhone_value);

            txtPrescriptionD_value = (TextView) rootView.findViewById(R.id.txtPrescriptionD_value);
            txtPrescriptionDate_value = (TextView) rootView.findViewById(R.id.txtPrescriptionDate_value);

            imgPrescriptionAttachments_value = (ImageView) rootView.findViewById(R.id.imgPrescriptionAttachments_value);

            txtPrescriptionAttachment_value = (TextView) rootView.findViewById(R.id.txtPrescriptionAttachment);

            if (Display == 1) {
                loadPrescriptionsData(bundle);
            } else {
                getDialog().cancel();
            }
        }

        return rootView;
    }


    private void loadPrescriptionsData(final Bundle bundle) {
        if (!bundle.isEmpty()) {



            txtPrescriptionDocName_value.setText(bundle.getString("DocName"));

            txtPrescriptionHCName_value.setText(bundle.getString("ClinicName"));

            txtPrescriptionAddress_value.setText(bundle.getString("DocAddress"));

            txtPrescriptionPhone_value.setText(bundle.getString("DocPhone"));

            txtPrescriptionD_value.setText(bundle.getString("Prescription"));
            txtPrescriptionDate_value.setText(bundle.getString("strPresDate"));

            final String Sourceid = bundle.getString("SourceId");

            // Show EMR Data in all Cases
            if (!Functions.isNullOrEmpty(Sourceid))
            {

                switch (Sourceid)
                {
                    case "2":
                        //Hide Image Attachment Links
                        // Change Links and Text to EMR Data
                        imgPrescriptionAttachments_value.setVisibility(View.GONE);
                        mProgressPrescriptionImage.setVisibility(View.GONE);
                        //else show EMR Data Link
                        mIB_PrescriptionReportEMR.setVisibility(View.VISIBLE);
                        mIB_PrescriptionReportEMR.setText(getString(R.string.PrescriptionEMRLinkdisp));
                        txtPrescriptionAttachment_value.setText(getString(R.string.PrescriptionEMRAttachmentsdisp));
                        // EMR Report URL
                        dataUrl = getActivity().getString(R.string.ImagePath) + getActivity().getString(R.string.urlEMRDATA) + bundle.getString("Id");
                        break;
                    case "5":
                        //Hide Image Attachment Links
                        // Change Links and Text to Share Data
                        imgPrescriptionAttachments_value.setVisibility(View.GONE);
                        mProgressPrescriptionImage.setVisibility(View.GONE);
                        //else show Share Data Link
                        mIB_PrescriptionReportEMR.setVisibility(View.VISIBLE);
                        mIB_PrescriptionReportEMR.setText(getString(R.string.PrescriptionShareLinkdisp));
                        txtPrescriptionAttachment_value.setText(getString(R.string.PrescriptionShareAttachmentsdisp));
                        // Shared Report URL
                        dataUrl = getActivity().getString(R.string.ImagePath) + getActivity().getString(R.string.urlShareReport) + bundle.getString("Id");
                        break;
                    default:
                        mIB_PrescriptionReportEMR.setVisibility(View.GONE);
                        String PrescriptionImagePath = bundle.getString("arrImages");
                        if (!Functions.isNullOrEmpty(PrescriptionImagePath)) {
                            //Image URL - This can point to any image file supported by Android
                            String url = getActivity().getString(R.string.ImagePath) + getActivity().getString(R.string.ImagePathPrescription) + PrescriptionImagePath;
                            if (!Functions.isNullOrEmpty(url)) {
                                //Functions.DispImageFromGlide(url, getActivity(), imgPrescriptionAttachments_value);
                                imgPrescriptionAttachments_value.setVisibility(View.VISIBLE);
                                mProgressPrescriptionImage.setVisibility(View.VISIBLE);
                                Functions.DispImageFromGlideWithProgressBar(url, getActivity(), imgPrescriptionAttachments_value, mProgressPrescriptionImage, "Prescription");
                            }
                        }
                        break;
                }

                // Set URl as per source id 4 EMR and 5 Share
                mIB_PrescriptionReportEMR.setOnClickListener(new View.OnClickListener()
                {
                    @Override
                    public void onClick(View view)
                    {
                        if (Functions.isNetworkAvailable(getActivity()))
                        {
                            if (!Functions.isNullOrEmpty(dataUrl))
                            {
                                try
                                {
                                    Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(dataUrl));
                                    startActivity(browserIntent);
                                }
                                catch (ActivityNotFoundException e)
                                {
                                    Functions.showToast(getActivity(), "No application can handle this request. Please install a webbrowser");
                                    //e.printStackTrace();
                                }
                            }
                        }
                        else
                        {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }
                    }
                });
            }





        }
    }

}
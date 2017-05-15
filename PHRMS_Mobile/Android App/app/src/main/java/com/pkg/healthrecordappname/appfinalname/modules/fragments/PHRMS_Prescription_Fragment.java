package com.pkg.healthrecordappname.appfinalname.modules.fragments;

import android.app.Activity;
import android.app.Fragment;
import android.content.Context;
import android.content.Intent;
import android.graphics.Typeface;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.GestureDetector;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.pkg.healthrecordappname.appfinalname.PHRMS_LoginActivity;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_Prescription_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_Prescription_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_PrescriptionData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.FontManager;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;

public class PHRMS_Prescription_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_prescription = null;
    private RecyclerView.LayoutManager mLayoutManager_prescription;
    private TextView txtprescriptionData;

    private ProgressBar mProgressView;
    private SwipeRefreshLayout mSwipeRefreshLayout;

    public PHRMS_Prescription_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_prescription, container, false);

        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);
        txtprescriptionData = (TextView) rootView.findViewById(R.id.txtprescriptionData);
        mRecyclerView_prescription = (RecyclerView) rootView.findViewById(R.id.lstdata_prescription_recycler);
        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_prescription_swipe_refresh);

        // Icon UsingFontAwesome
        Typeface iconFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvUsersPRE), iconFont);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvUsersPREShared), iconFont);

        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_prescription);

        Functions.progressbarStyle(mProgressView, getActivity());
        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNetworkAvailable(getActivity())) {

            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {

                url = getString(R.string.urlLogin) + getString(R.string.LoadPrescriptionData) + userid;


                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_prescription.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_prescription = new LinearLayoutManager(getActivity());
                mRecyclerView_prescription.setLayoutManager(mLayoutManager_prescription);
                mRecyclerView_prescription.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_prescription.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadprescriptionData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadprescriptionData(url);

                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_prescription.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_prescription, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {
                        PHRMS_Prescription_Dialogue dialogue_Prescription = new PHRMS_Prescription_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("DocName", ParseJson_PrescriptionData.DocName[position]);
                        bundle.putString("ClinicName", ParseJson_PrescriptionData.ClinicName[position]);
                        bundle.putString("DocAddress", ParseJson_PrescriptionData.DocAddress[position]);
                        bundle.putString("DocPhone", ParseJson_PrescriptionData.DocPhone[position]);
                        bundle.putString("Prescription", ParseJson_PrescriptionData.Prescription[position]);
                        bundle.putString("strPresDate", ParseJson_PrescriptionData.strPresDate[position]);
                        bundle.putString("arrImages", ParseJson_PrescriptionData.arrImages[position]);
                        //Start For EMR - 20/10/2016
                        bundle.putString("SourceId", ParseJson_PrescriptionData.SourceId[position]);
                        bundle.putString("Id", ParseJson_PrescriptionData.Id[position]);
                        // End For EMR - 20/10/2016
                        dialogue_Prescription.setArguments(bundle);
                        dialogue_Prescription.show(getFragmentManager(), "Prescription Details");


                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            Intent intPrescriptionList = new Intent(getActivity(), PHRMS_Prescription_Fragment_Add.class);
                            startActivityForResult(intPrescriptionList, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }

                    }
                });

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_prescription) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_prescription, fab);
                }
            }

        } else {
            Functions.showSnackbar(rootView, Functions.IE_NotAvailable, "Action");
        }

        return rootView;
    }

    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 1) {
            if (resultCode == -1) {
                if (data.getIntExtra("PrescriptionSaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout.setRefreshing(true);
                        LoadprescriptionData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadprescriptionData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadprescriptionJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("prescription Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadprescriptionJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_PrescriptionData prescription_pj = new ParseJson_PrescriptionData(jsonData);
        String STATUS = prescription_pj.parseJson();
        if (STATUS.equals("1")) {
            txtprescriptionData.setVisibility(View.GONE);

            prescriptionDataList m_prescriptionDataList = new prescriptionDataList(getActivity(), ParseJson_PrescriptionData.ClinicName, ParseJson_PrescriptionData.DocName, ParseJson_PrescriptionData.strCreatedDate, ParseJson_PrescriptionData.SourceId);
            // specify an adapter (see also next example)
            mRecyclerView_prescription.setAdapter(m_prescriptionDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        } else {
            txtprescriptionData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        }

    }

    //extends Recyler adapter prescriptionDataList and ViewHolder to hold data
    public class prescriptionDataList extends RecyclerView.Adapter<prescriptionDataList.ViewHolder> {
        private String[] clinicName;
        private String[] DocName;
        private String[] strCreatedDate;
        private String[] SourceId;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_clinicName, mTextView_DocName, mTextView_strCreatedDate, mTextView_tvfontUser, mTextView_tvfontDoctor, mTextView_tvfontMedicalShared;

            //Load Recyler adapter prescriptionDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_clinicName = (TextView) v.findViewById(R.id.txtpreclinicName);
                mTextView_DocName = (TextView) v.findViewById(R.id.txtpreDocNameprescription_Value);
                mTextView_strCreatedDate = (TextView) v.findViewById(R.id.txtprestrCreatedDate);
                mTextView_tvfontUser = (TextView) v.findViewById(R.id.tvfontUser);
                mTextView_tvfontDoctor = (TextView) v.findViewById(R.id.tvfontDoctor);
                mTextView_tvfontMedicalShared = (TextView) v.findViewById(R.id.tvfontMedicalShared);

                Typeface iconFontLVUserType = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
                FontManager.markAsIconContainer(v.findViewById(R.id.listUsersType), iconFontLVUserType);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public prescriptionDataList(Activity context, String[] clinicName, String[] DocName, String[] strCreatedDate, String[] SourceId) {
            this.context = context;
            this.clinicName = clinicName;
            this.DocName = DocName;
            this.strCreatedDate = strCreatedDate;
            this.SourceId = SourceId;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public prescriptionDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listprescription, parent, false);
            ViewHolder viewHolder = new ViewHolder(view);
            return viewHolder;
        }

        // Replace the contents of a view (invoked by the layout manager)
        @Override
        public void onBindViewHolder(ViewHolder holder, int position) {
            // - get element from your dataset at this position
            // - replace the contents of the view with that element

            // To show data entered by - user [or] - Doctor( from EMR)
            // Data Entered by Doctor for source id - id here  or id here
            if (SourceId[position].equals("2")) {
                // Doctor - EMR
                holder.mTextView_tvfontDoctor.setVisibility(View.VISIBLE);

                holder.mTextView_tvfontUser.setVisibility(View.GONE);
                holder.mTextView_tvfontMedicalShared.setVisibility(View.GONE);
            } else if (SourceId[position].equals("5")) {
                // Shared - Medical contact
                holder.mTextView_tvfontMedicalShared.setVisibility(View.VISIBLE);

                holder.mTextView_tvfontDoctor.setVisibility(View.GONE);
                holder.mTextView_tvfontUser.setVisibility(View.GONE);
            } else {
                // User entered
                holder.mTextView_tvfontUser.setVisibility(View.VISIBLE);

                // Data Entered by User
                holder.mTextView_tvfontDoctor.setVisibility(View.GONE);
                holder.mTextView_tvfontMedicalShared.setVisibility(View.GONE);
            }

            holder.mTextView_clinicName.setText(clinicName[position]);
            holder.mTextView_DocName.setText(DocName[position]);
            holder.mTextView_strCreatedDate.setText(strCreatedDate[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {

            return (null != clinicName ? clinicName.length : 0);
        }

        public void addItem(ViewHolder holder, int position) {

        }

        public void deleteItem(ViewHolder holder, int position) {

        }
    }

    // recycleview TouchListener
    public interface ClickListener {
        void onClick(View view, int position);

        void onLongClick(View view, int position);
    }

    public static class RecyclerTouchListener implements RecyclerView.OnItemTouchListener {

        private GestureDetector gestureDetector;
        private PHRMS_Prescription_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_Prescription_Fragment.ClickListener clickListener) {
            this.clickListener = clickListener;
            gestureDetector = new GestureDetector(context, new GestureDetector.SimpleOnGestureListener() {
                @Override
                public boolean onSingleTapUp(MotionEvent e) {
                    return true;
                }

                @Override
                public void onLongPress(MotionEvent e) {
                    View child = recyclerView.findChildViewUnder(e.getX(), e.getY());
                    if (child != null && clickListener != null) {
                        clickListener.onLongClick(child, recyclerView.getChildPosition(child));
                    }
                }
            });
        }

        @Override
        public boolean onInterceptTouchEvent(RecyclerView rv, MotionEvent e) {

            View child = rv.findChildViewUnder(e.getX(), e.getY());
            if (child != null && clickListener != null && gestureDetector.onTouchEvent(e)) {
                clickListener.onClick(child, rv.getChildPosition(child));
            }
            return false;
        }

        @Override
        public void onTouchEvent(RecyclerView rv, MotionEvent e) {
        }

        @Override
        public void onRequestDisallowInterceptTouchEvent(boolean disallowIntercept) {

        }
    }

}
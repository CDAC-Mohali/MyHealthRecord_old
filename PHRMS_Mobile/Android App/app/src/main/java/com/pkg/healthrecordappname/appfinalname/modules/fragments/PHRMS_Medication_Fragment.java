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
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_Medication_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_Medication_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_MedicationData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.FontManager;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;


public class PHRMS_Medication_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_medication = null;
    private RecyclerView.LayoutManager mLayoutManager_medication;
    private TextView txtmedicationData;

    private ProgressBar mProgressView;
    private SwipeRefreshLayout mSwipeRefreshLayout;

    public PHRMS_Medication_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_medication, container, false);

        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);
        txtmedicationData = (TextView) rootView.findViewById(R.id.txtmedicationData);
        mRecyclerView_medication = (RecyclerView) rootView.findViewById(R.id.lstdata_medication_recycler);
        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_medication_swipe_refresh);

        // Icon UsingFontAwesome
        Typeface iconFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvUsersMedi), iconFont);
        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_medication);

        Functions.progressbarStyle(mProgressView, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {

                url = getString(R.string.urlLogin) + getString(R.string.LoadMedicationData) + userid;


                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_medication.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_medication = new LinearLayoutManager(getActivity());
                mRecyclerView_medication.setLayoutManager(mLayoutManager_medication);
                mRecyclerView_medication.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_medication.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadmedicationData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadmedicationData(url);
                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_medication.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_medication, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {
                        PHRMS_Medication_Dialogue dialogue_Medication = new PHRMS_Medication_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("MedicineName", ParseJson_MedicationData.MedicineName[position]);
                        bundle.putString("strTakingMedicine", ParseJson_MedicationData.strTakingMedicine[position]);
                        bundle.putString("strPrescribedDate", ParseJson_MedicationData.strPrescribedDate[position]);
                        bundle.putString("strRoute", ParseJson_MedicationData.strRoute[position]);
                        bundle.putString("Strength", ParseJson_MedicationData.Strength[position]);
                        bundle.putString("strDosValue", ParseJson_MedicationData.strDosValue[position]);
                        bundle.putString("strDosUnit", ParseJson_MedicationData.strDosUnit[position]);
                        bundle.putString("strFrequency", ParseJson_MedicationData.strFrequency[position]);
                        bundle.putString("LabelInstructions", ParseJson_MedicationData.LabelInstructions[position]);
                        bundle.putString("Notes", ParseJson_MedicationData.Notes[position]);
                        bundle.putString("arrImages", ParseJson_MedicationData.arrImages[position]);
                        dialogue_Medication.setArguments(bundle);
                        dialogue_Medication.show(getFragmentManager(), "Medication Details");

                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            Intent intMedicationList = new Intent(getActivity(), PHRMS_Medication_Fragment_Add.class);
                            startActivityForResult(intMedicationList, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }
                    }
                });

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_medication) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_medication, fab);
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
                if (data.getIntExtra("MedicationSaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout.setRefreshing(true);
                        LoadmedicationData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadmedicationData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadmedicationJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("medication Error", error.toString());
                mSwipeRefreshLayout.setRefreshing(false);
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadmedicationJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_MedicationData medication_pj = new ParseJson_MedicationData(jsonData);
        String STATUS = medication_pj.parseJson();
        if (STATUS.equals("1")) {
            txtmedicationData.setVisibility(View.GONE);

            medicationDataList m_medicationDataList = new medicationDataList(getActivity(), ParseJson_MedicationData.MedicineName, ParseJson_MedicationData.strTakingMedicine, ParseJson_MedicationData.strCreatedDate, ParseJson_MedicationData.SourceId);



            mRecyclerView_medication.setAdapter(m_medicationDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        } else {
            txtmedicationData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        }

    }

    //extends Recyler adapter medicationDataList and ViewHolder to hold data
    public class medicationDataList extends RecyclerView.Adapter<medicationDataList.ViewHolder> {
        private String[] medicationName;
        private String[] StillTakingMedication;
        private String[] created_prescribedDate;
        private String[] SourceId;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_medicationName, mTextView_StillTakingMedication, mTextView_created_prescribedDate, mTextView_tvfontUserMedi, mTextView_tvfontDoctorMedi;

            //Load Recyler adapter medicationDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_medicationName = (TextView) v.findViewById(R.id.txtmedmedicationName);
                mTextView_StillTakingMedication = (TextView) v.findViewById(R.id.txtmedStillTakingMedication_Value);
                mTextView_created_prescribedDate = (TextView) v.findViewById(R.id.txtmedprescribedDate);

                mTextView_tvfontUserMedi = (TextView) v.findViewById(R.id.tvfontUserMedi);
                mTextView_tvfontDoctorMedi = (TextView) v.findViewById(R.id.tvfontDoctorMedi);

                Typeface iconFontLVUserType = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
                FontManager.markAsIconContainer(v.findViewById(R.id.listUsersTypeMedi), iconFontLVUserType);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public medicationDataList(Activity context, String[] medicationName, String[] StillTakingMedication, String[] create_prescribedDate, String[] SourceId) {
            this.context = context;
            this.medicationName = medicationName;
            this.StillTakingMedication = StillTakingMedication;
            this.created_prescribedDate = create_prescribedDate;

            this.SourceId = SourceId;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public medicationDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listmedication, parent, false);
            ViewHolder viewHolder = new ViewHolder(view);
            return viewHolder;
        }

        // Replace the contents of a view (invoked by the layout manager)
        @Override
        public void onBindViewHolder(ViewHolder holder, int position) {
            // - get element from your dataset at this position
            // - replace the contents of the view with that element

            // To show data entered by - user [or] - Doctor( from EMR)
            // Data Entered by Doctor for source id - id here or id here
            if (SourceId[position].equals("2") || SourceId[position].equals("5"))
            {
                holder.mTextView_tvfontDoctorMedi.setVisibility(View.VISIBLE);
                holder.mTextView_tvfontUserMedi.setVisibility(View.GONE);
            } else {
                // Data Entered by User
                holder.mTextView_tvfontDoctorMedi.setVisibility(View.GONE);
                holder.mTextView_tvfontUserMedi.setVisibility(View.VISIBLE);
            }

            holder.mTextView_medicationName.setText(medicationName[position]);
            holder.mTextView_StillTakingMedication.setText(StillTakingMedication[position]);
            holder.mTextView_created_prescribedDate.setText(created_prescribedDate[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != medicationName ? medicationName.length : 0);
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
        private PHRMS_Medication_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_Medication_Fragment.ClickListener clickListener) {
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
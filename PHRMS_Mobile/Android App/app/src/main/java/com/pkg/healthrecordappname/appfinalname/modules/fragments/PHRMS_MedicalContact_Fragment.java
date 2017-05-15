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
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_MedicalContact_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_MedicalContact_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_MedicalContactData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.FontManager;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;

public class PHRMS_MedicalContact_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_medicalcontacts = null;
    private RecyclerView.LayoutManager mLayoutManager_medicalcontacts;
    private TextView txtmedicalcontactsData;

    private ProgressBar mProgressView;
    private SwipeRefreshLayout mSwipeRefreshLayout_MedicalContacts;

    public PHRMS_MedicalContact_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_medicalcontact, container, false);


        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);
        txtmedicalcontactsData = (TextView) rootView.findViewById(R.id.txtMedicalContactsData);
        mRecyclerView_medicalcontacts = (RecyclerView) rootView.findViewById(R.id.lstdata_MedicalContacts_recycler);
        mSwipeRefreshLayout_MedicalContacts = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_MedicalContacts_swipe_refresh);

        // Icon UsingFontAwesome
        // Declare fontawseome support to all containers in drawer
        Typeface iconFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvMCType), iconFont);

        Typeface iconuserFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvUsersMedicalContacts), iconuserFont);

        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_MedicalContacts);

        Functions.progressbarStyle(mProgressView, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {
                url = getString(R.string.urlLogin) + getString(R.string.GetMedicalContactsList) + userid;

                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_medicalcontacts.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_medicalcontacts = new LinearLayoutManager(getActivity());
                mRecyclerView_medicalcontacts.setLayoutManager(mLayoutManager_medicalcontacts);
                mRecyclerView_medicalcontacts.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_medicalcontacts.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadmedicalcontactsData(url);
                }


                mSwipeRefreshLayout_MedicalContacts.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadmedicalcontactsData(url);
                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_medicalcontacts.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_medicalcontacts, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {
                        // To get Details by Contacts - ITEM ID
                        String url_for_detail_byID = getString(R.string.urlLogin) + getString(R.string.GetMedicalContactDetailByID) + ParseJson_MedicalContactData.Id[position].toString();
                        LoadMedicalContactsDetailsByID(url_for_detail_byID);
                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            Intent intMedicalContactsList = new Intent(getActivity(), PHRMS_MedicalContact_Fragment_Add.class);
                            startActivityForResult(intMedicalContactsList, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }
                    }
                });

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_medicalcontacts) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_medicalcontacts, fab);
                }
            }

        } else {
            Functions.showSnackbar(rootView, Functions.IE_NotAvailable, "Action");
        }

        return rootView;
    }

    public void LoadMedicalContactsDetailsByID(String url_for_details)
    {
        if (Functions.isNetworkAvailable(getActivity()))
        {
            PHRMS_MedicalContact_Dialogue dialogue_MedicalContacts = new PHRMS_MedicalContact_Dialogue();
            Bundle bundle = new Bundle();
            bundle.putInt("Display", 1);
            bundle.putString("URLDetails", url_for_details);
            dialogue_MedicalContacts.setArguments(bundle);
            dialogue_MedicalContacts.show(getFragmentManager(), "MedicalContacts Details");
        }
        else
        {
            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
        }
    }


    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 1) {
            if (resultCode == -1) {
                if (data.getIntExtra("MedicalContactsSaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout_MedicalContacts.setRefreshing(true);
                        LoadmedicalcontactsData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadmedicalcontactsData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadmedicalcontactsJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("medicalcontacts Error", error.toString());
                mSwipeRefreshLayout_MedicalContacts.setRefreshing(false);
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadmedicalcontactsJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_MedicalContactData medicalcontacts_pj = new ParseJson_MedicalContactData(jsonData);
        String STATUS = medicalcontacts_pj.parseJson();

        if (STATUS.equals("1")) {
            txtmedicalcontactsData.setVisibility(View.GONE);

            medicalcontactsDataList m_medicalcontactsDataList = new medicalcontactsDataList(getActivity(), ParseJson_MedicalContactData.ContactName, ParseJson_MedicalContactData.strMedContType, ParseJson_MedicalContactData.strCreatedDate, ParseJson_MedicalContactData.SourceId);

            // specify an adapter (see also next example)
            mRecyclerView_medicalcontacts.setAdapter(m_medicalcontactsDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout_MedicalContacts.setRefreshing(false);
        } else {
            txtmedicalcontactsData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout_MedicalContacts.setRefreshing(false);
        }

    }

    //extends Recyler adapter medicalcontactsDataList and ViewHolder to hold data
    public class medicalcontactsDataList extends RecyclerView.Adapter<medicalcontactsDataList.ViewHolder> {
        private String[] medicalcontactsName;
        private String[] strMedContType;
        private String[] strCreatedDate;
        private String[] SourceId;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_medicalcontactsName, mTextView_strMedContType, mTextView_strCreatedDate, mTextView_tvfontUserMedicalContacts, mTextView_tvfontDoctorMedicalContacts;

            //Load Recyler adapter medicalcontactsDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_strCreatedDate = (TextView) v.findViewById(R.id.txtmedicalcontactsCreatedDate);
                mTextView_medicalcontactsName = (TextView) v.findViewById(R.id.txtlistMedicalContactsName_value);
                mTextView_strMedContType = (TextView) v.findViewById(R.id.txtmedicalcontactsSpeciality_Value);

                mTextView_tvfontUserMedicalContacts = (TextView) v.findViewById(R.id.tvfontUserMedicalContacts);
                mTextView_tvfontDoctorMedicalContacts = (TextView) v.findViewById(R.id.tvfontDoctorMedicalContacts);

                Typeface iconFontLVUserType = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
                FontManager.markAsIconContainer(v.findViewById(R.id.listUsersTypeMedicalContacts), iconFontLVUserType);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public medicalcontactsDataList(Activity context, String[] medicalcontactsName, String[] MedContType, String[] CreatedDate, String[] SourceId) {
            this.context = context;
            this.strCreatedDate = CreatedDate;
            this.medicalcontactsName = medicalcontactsName;
            this.strMedContType = MedContType;
            this.SourceId = SourceId;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            //LayoutInflater inflater = context.getLayoutInflater();
            //View listViewItem = inflater.inflate(R.layout.medicalcontactslist, null, true);
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listmedicalcontact, parent, false);
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
            if (SourceId[position].equals("2") || SourceId[position].equals("5")) {
                holder.mTextView_tvfontDoctorMedicalContacts.setVisibility(View.VISIBLE);
                holder.mTextView_tvfontUserMedicalContacts.setVisibility(View.GONE);
            } else {
                // Data Entered by User
                holder.mTextView_tvfontDoctorMedicalContacts.setVisibility(View.GONE);
                holder.mTextView_tvfontUserMedicalContacts.setVisibility(View.VISIBLE);
            }

            holder.mTextView_medicalcontactsName.setText(medicalcontactsName[position]);
            holder.mTextView_strMedContType.setText(strMedContType[position]);
            holder.mTextView_strCreatedDate.setText(strCreatedDate[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != medicalcontactsName ? medicalcontactsName.length : 0);
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
        private ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final ClickListener clickListener) {
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
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
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_Immunization_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_Immunization_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_ImmunizationData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.FontManager;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;


public class PHRMS_Immunization_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_Immunization = null;
    private RecyclerView.LayoutManager mLayoutManager_Immunization;
    private TextView txtImmunizationData;
    private ProgressBar mProgressView;
    private SwipeRefreshLayout mSwipeRefreshLayout;

    public PHRMS_Immunization_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_immunization, container, false);


        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);
        txtImmunizationData = (TextView) rootView.findViewById(R.id.txtimmunizationData);
        mRecyclerView_Immunization = (RecyclerView) rootView.findViewById(R.id.lstdata_immunization_recycler);
        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_immunization_swipe_refresh);

        // Icon UsingFontAwesome
        Typeface iconFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvUsersImu ), iconFont);

        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_immunization);

        Functions.progressbarStyle(mProgressView, getActivity());


        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {
                url = getString(R.string.urlLogin) + getString(R.string.LoadImmunizationData) + userid;

                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_Immunization.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_Immunization = new LinearLayoutManager(getActivity());
                mRecyclerView_Immunization.setLayoutManager(mLayoutManager_Immunization);
                mRecyclerView_Immunization.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_Immunization.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadImmunizationData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadImmunizationData(url);

                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_Immunization.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_Immunization, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {

                        PHRMS_Immunization_Dialogue dialogue_immunization = new PHRMS_Immunization_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("ImmunizationName", ParseJson_ImmunizationData.ImmunizationName[position]);
                        bundle.putString("strImmunizationDate", ParseJson_ImmunizationData.strImmunizationDate[position]);
                        bundle.putString("Comments", ParseJson_ImmunizationData.Comments[position]);
                        dialogue_immunization.setArguments(bundle);


                        dialogue_immunization.show(getFragmentManager(), "Immunization Details");
                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {

                        if (Functions.isNetworkAvailable(getActivity())) {

                            Intent intImmunizationList = new Intent(getActivity(), PHRMS_Immunization_Fragment_Add.class);
                            startActivityForResult(intImmunizationList, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }
                    }
                });

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_Immunization) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_Immunization, fab);
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
                if (data.getIntExtra("ImmunizationSaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout.setRefreshing(true);

                        LoadImmunizationData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadImmunizationData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadImmunizationJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Immunize Frame Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadImmunizationJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_ImmunizationData Immunization_pj = new ParseJson_ImmunizationData(jsonData);
        String STATUS = Immunization_pj.parseJson();
        if (STATUS.equals("1")) {
            txtImmunizationData.setVisibility(View.GONE);

            ImmunizationDataList m_ImmunizationDataList = new ImmunizationDataList(getActivity(), ParseJson_ImmunizationData.ImmunizationName, ParseJson_ImmunizationData.Comments, ParseJson_ImmunizationData.strCreatedDate, ParseJson_ImmunizationData.SourceId);


            // specify an adapter (see also next example)
            mRecyclerView_Immunization.setAdapter(m_ImmunizationDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        } else {

            txtImmunizationData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        }

    }

    //extends Recyler adapter ImmunizationDataList and ViewHolder to hold data
    public class ImmunizationDataList extends RecyclerView.Adapter<ImmunizationDataList.ViewHolder> {
        private String[] ImmunizationName;
        private String[] immunizationcoments;
        private String[] strCreatedDate;
        private String[] SourceId;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_ImmunizationName, mTextView_immunizationcoments, mTextView_strCreatedDate, mTextView_tvfontUserImu, mTextView_tvfontDoctorImu;

            //Load Recyler adapter ImmunizationDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_ImmunizationName = (TextView) v.findViewById(R.id.txtimmimmunizationname);
                mTextView_immunizationcoments = (TextView) v.findViewById(R.id.txtimmimmunizationcoments_Value);
                mTextView_strCreatedDate = (TextView) v.findViewById(R.id.txtimmstrCreatedDate);
                mTextView_tvfontUserImu = (TextView) v.findViewById(R.id.tvfontUserImu);
                mTextView_tvfontDoctorImu = (TextView) v.findViewById(R.id.tvfontDoctorImu);

                Typeface iconFontLVUserType = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
                FontManager.markAsIconContainer(v.findViewById(R.id.listUsersTypeImu), iconFontLVUserType);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ImmunizationDataList(Activity context, String[] ImmunizationName, String[] immunizationcoments, String[] strCreatedDate, String[] SourceId) {
            this.context = context;
            this.ImmunizationName = ImmunizationName;
            this.immunizationcoments = immunizationcoments;
            this.strCreatedDate = strCreatedDate;
            this.SourceId = SourceId;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public ImmunizationDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listimmunization, parent, false);
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
            if (SourceId[position].equals("2") || SourceId[position].equals("5"))
            {
                holder.mTextView_tvfontDoctorImu.setVisibility(View.VISIBLE);
                holder.mTextView_tvfontUserImu.setVisibility(View.GONE);
            } else {
                // Data Entered by User
                holder.mTextView_tvfontDoctorImu.setVisibility(View.GONE);
                holder.mTextView_tvfontUserImu.setVisibility(View.VISIBLE);
            }

            holder.mTextView_ImmunizationName.setText(ImmunizationName[position]);
            holder.mTextView_immunizationcoments.setText(immunizationcoments[position]);
            holder.mTextView_strCreatedDate.setText(strCreatedDate[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != ImmunizationName ? ImmunizationName.length : 0);
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
        private PHRMS_Immunization_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_Immunization_Fragment.ClickListener clickListener) {
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
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
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_LabTests_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_LabTests_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_LabTestsData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.FontManager;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;


public class PHRMS_LabTests_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_labtests = null;
    private RecyclerView.LayoutManager mLayoutManager_labtests;
    private TextView txtlabtestsData = null;

    private ProgressBar mProgressView;
    private SwipeRefreshLayout mSwipeRefreshLayout;

    public PHRMS_LabTests_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_labtests, container, false);

        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);
        txtlabtestsData = (TextView) rootView.findViewById(R.id.txtlabtestsData);
        mRecyclerView_labtests = (RecyclerView) rootView.findViewById(R.id.lstdata_labtests_recycler);
        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_labtests_swipe_refresh);

        // Icon UsingFontAwesome
        Typeface iconFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvUserslabtest), iconFont);

        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_labtests);
        Functions.progressbarStyle(mProgressView, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {

                url = getString(R.string.urlLogin) + getString(R.string.LoadLabData) + userid;

                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_labtests.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_labtests = new LinearLayoutManager(getActivity());
                mRecyclerView_labtests.setLayoutManager(mLayoutManager_labtests);
                mRecyclerView_labtests.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_labtests.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadlabtestsData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadlabtestsData(url);
                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_labtests.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_labtests, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {

                        PHRMS_LabTests_Dialogue dialogue_LabTests = new PHRMS_LabTests_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("TestName", ParseJson_LabTestsData.TestName[position]);
                        bundle.putString("Result", ParseJson_LabTestsData.Result[position]);
                        bundle.putString("Unit", ParseJson_LabTestsData.Unit[position]);
                        bundle.putString("Comments", ParseJson_LabTestsData.Comments[position]);
                        bundle.putString("arrImages", ParseJson_LabTestsData.arrImages[position]);
                        dialogue_LabTests.setArguments(bundle);

                        dialogue_LabTests.show(getFragmentManager(), "Lab Result Details");

                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            Intent intLabTestsList = new Intent(getActivity(), PHRMS_LabTests_Fragment_Add.class);
                            startActivityForResult(intLabTestsList, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }

                    }
                });

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_labtests) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_labtests, fab);
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
                if (data.getIntExtra("LabTestsSaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout.setRefreshing(true);
                        LoadlabtestsData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }


    public void LoadlabtestsData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadlabtestsJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("labtests Frame Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadlabtestsJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_LabTestsData labtests_pj = new ParseJson_LabTestsData(jsonData);
        String STATUS = labtests_pj.parseJson();
        if (STATUS.equals("1")) {
            txtlabtestsData.setVisibility(View.GONE);

            labtestsDataList m_labtestsDataList = new labtestsDataList(getActivity(), ParseJson_LabTestsData.TestName, ParseJson_LabTestsData.Result, ParseJson_LabTestsData.Unit, ParseJson_LabTestsData.strCreatedDate, ParseJson_LabTestsData.SourceId);


            mRecyclerView_labtests.setAdapter(m_labtestsDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        } else {
            txtlabtestsData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        }

    }

    //extends Recyler adapter labtestsDataList and ViewHolder to hold data
    public class labtestsDataList extends RecyclerView.Adapter<labtestsDataList.ViewHolder> {
        private String[] labtestsName;
        private String[] strResult;
        private String[] strUnit;
        private String[] strCreatedDate;
        private String[] SourceId;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_labtestsName, mTextView_strResult, mTextView_strUnit, mTextView_strCreatedDate, mTextView_tvfontUserlabtest, mTextView_tvfontDoctorlabtest;

            //Load Recyler adapter labtestsDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_labtestsName = (TextView) v.findViewById(R.id.txtltlabtestsname);
                mTextView_strResult = (TextView) v.findViewById(R.id.txtltlabtestsResults_Value);
                mTextView_strUnit = (TextView) v.findViewById(R.id.txtltlabtestsUnit_Value);
                mTextView_strCreatedDate = (TextView) v.findViewById(R.id.txtltstrCreatedDate);

                mTextView_tvfontUserlabtest = (TextView) v.findViewById(R.id.tvfontUserlabtest);
                mTextView_tvfontDoctorlabtest = (TextView) v.findViewById(R.id.tvfontDoctorlabtest);

                Typeface iconFontLVUserType = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
                FontManager.markAsIconContainer(v.findViewById(R.id.listUsersTypelabtest), iconFontLVUserType);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public labtestsDataList(Activity context, String[] labtestsName, String[] strResult, String[] strUnit, String[] strCreatedDate, String[] SourceId) {
            this.context = context;
            this.labtestsName = labtestsName;
            this.strResult = strResult;
            this.strUnit = strUnit;
            this.strCreatedDate = strCreatedDate;
            this.SourceId = SourceId;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public labtestsDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listlabtests, parent, false);
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
                holder.mTextView_tvfontDoctorlabtest.setVisibility(View.VISIBLE);
                holder.mTextView_tvfontUserlabtest.setVisibility(View.GONE);
            } else {
                // Data Entered by User
                holder.mTextView_tvfontDoctorlabtest.setVisibility(View.GONE);
                holder.mTextView_tvfontUserlabtest.setVisibility(View.VISIBLE);
            }

            holder.mTextView_labtestsName.setText(labtestsName[position]);
            holder.mTextView_strResult.setText(strResult[position]);
            holder.mTextView_strUnit.setText(strUnit[position]);
            holder.mTextView_strCreatedDate.setText(strCreatedDate[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != labtestsName ? labtestsName.length : 0);
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
        private PHRMS_LabTests_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_LabTests_Fragment.ClickListener clickListener) {
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
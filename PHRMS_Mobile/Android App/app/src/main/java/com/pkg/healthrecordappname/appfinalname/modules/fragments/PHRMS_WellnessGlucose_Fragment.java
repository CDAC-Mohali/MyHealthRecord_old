package com.pkg.healthrecordappname.appfinalname.modules.fragments;

import android.app.Activity;
import android.app.Fragment;
import android.content.Context;
import android.content.Intent;
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
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_WellnessGlucose_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_WellnessGlucose_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_WellnessGlucoseData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;


public class PHRMS_WellnessGlucose_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_wellnessGlucose = null;
    private RecyclerView.LayoutManager mLayoutManager_wellnessGlucose;
    private TextView txtwellnessGlucoseData;

    private ProgressBar mProgressView;

    private SwipeRefreshLayout mSwipeRefreshLayout;

    public PHRMS_WellnessGlucose_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_wellnessglucose, container, false);

        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);

        txtwellnessGlucoseData = (TextView) rootView.findViewById(R.id.txtwellnessGlucoseData);
        mRecyclerView_wellnessGlucose = (RecyclerView) rootView.findViewById(R.id.lstdata_wellnessGlucose_recycler);

        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_wellnessGlucose_swipe_refresh);

        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_wellnessGlucose);

        Functions.progressbarStyle(mProgressView, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {
                url = getString(R.string.urlLogin) + getString(R.string.LoadBoodGlucoseData) + userid;


                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_wellnessGlucose.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_wellnessGlucose = new LinearLayoutManager(getActivity());
                mRecyclerView_wellnessGlucose.setLayoutManager(mLayoutManager_wellnessGlucose);
                mRecyclerView_wellnessGlucose.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_wellnessGlucose.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadwellnessGlucoseData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadwellnessGlucoseData(url);

                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_wellnessGlucose.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_wellnessGlucose, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {

                        PHRMS_WellnessGlucose_Dialogue dialogue_WellnessGlucose = new PHRMS_WellnessGlucose_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("Result", ParseJson_WellnessGlucoseData.Result[position]);
                        bundle.putString("ValueType", ParseJson_WellnessGlucoseData.ValueType[position]);
                        bundle.putString("strCollectionDate", ParseJson_WellnessGlucoseData.strCollectionDate[position]);
                        bundle.putString("Comments", ParseJson_WellnessGlucoseData.Comments[position]);
                        dialogue_WellnessGlucose.setArguments(bundle);
                        dialogue_WellnessGlucose.show(getFragmentManager(), "Blood Glucose Details");
                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {

                        if (Functions.isNetworkAvailable(getActivity())) {
                            Intent intWellnessBGAdd = new Intent(getActivity(), PHRMS_WellnessGlucose_Fragment_Add.class);
                            startActivityForResult(intWellnessBGAdd, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }

                    }
                });

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_wellnessGlucose) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_wellnessGlucose, fab);
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
            if (resultCode == Activity.RESULT_OK) {
                if (data.getIntExtra("WellnessBGSaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout.setRefreshing(true);
                        LoadwellnessGlucoseData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadwellnessGlucoseData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadwellnessGlucoseJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {

                Functions.showProgress(false, mProgressView);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Glucose Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadwellnessGlucoseJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_WellnessGlucoseData wellnessGlucose_pj = new ParseJson_WellnessGlucoseData(jsonData);
        String STATUS = wellnessGlucose_pj.parseJson();
        if (STATUS.equals("1")) {
            txtwellnessGlucoseData.setVisibility(View.GONE);

            wellnessGlucoseDataList m_wellnessGlucoseDataList = new wellnessGlucoseDataList(getActivity(), ParseJson_WellnessGlucoseData.ValueType, ParseJson_WellnessGlucoseData.Result, ParseJson_WellnessGlucoseData.strCreatedDate);

            // specify an adapter (see also next example)
            mRecyclerView_wellnessGlucose.setAdapter(m_wellnessGlucoseDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        } else {
            txtwellnessGlucoseData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        }


    }

    //extends Recyler adapter wellnessGlucoseDataList and ViewHolder to hold data
    public class wellnessGlucoseDataList extends RecyclerView.Adapter<wellnessGlucoseDataList.ViewHolder> {
        private String[] wellnessGlucoseValueType;
        private String[] wellnessGlucoseResult;
        private String[] createdDate;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_wellnessGlucoseValueType, mTextView_wellnessGlucoseResult, mTextView_createdDate;

            //Load Recyler adapter wellnessGlucoseDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_wellnessGlucoseValueType = (TextView) v.findViewById(R.id.txtactbgwellnessGlucoseValue);
                mTextView_wellnessGlucoseResult = (TextView) v.findViewById(R.id.txtactbgwellnessGlucoseResultValue);
                mTextView_createdDate = (TextView) v.findViewById(R.id.txtactbgcollectionDate);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public wellnessGlucoseDataList(Activity context, String[] wellnessGlucoseValueType, String[] wellnessGlucoseResult, String[] createdDate) {
            this.context = context;
            this.wellnessGlucoseValueType = wellnessGlucoseValueType;
            this.wellnessGlucoseResult = wellnessGlucoseResult;
            this.createdDate = createdDate;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public wellnessGlucoseDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {


            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listwellnessglucose, parent, false);

            ViewHolder viewHolder = new ViewHolder(view);
            return viewHolder;
        }

        // Replace the contents of a view (invoked by the layout manager)
        @Override
        public void onBindViewHolder(ViewHolder holder, int position) {
            // - get element from your dataset at this position
            // - replace the contents of the view with that element
            holder.mTextView_wellnessGlucoseValueType.setText(wellnessGlucoseValueType[position]);
            holder.mTextView_wellnessGlucoseResult.setText(wellnessGlucoseResult[position]);
            holder.mTextView_createdDate.setText(createdDate[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != wellnessGlucoseValueType ? wellnessGlucoseValueType.length : 0);
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
        private PHRMS_WellnessGlucose_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_WellnessGlucose_Fragment.ClickListener clickListener) {
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
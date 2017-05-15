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
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_WellnessBP_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_WellnessBP_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_WellnessBPData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;


public class PHRMS_WellnessBP_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_wellnessBP = null;
    private RecyclerView.LayoutManager mLayoutManager_wellnessBP;
    private TextView txtwellnessBPData;
    private ProgressBar mProgressView;
    private SwipeRefreshLayout mSwipeRefreshLayout;

    public PHRMS_WellnessBP_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_wellnessbp, container, false);

        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);
        txtwellnessBPData = (TextView) rootView.findViewById(R.id.txtwellnessbpData);
        mRecyclerView_wellnessBP = (RecyclerView) rootView.findViewById(R.id.lstdata_wellnessbp_recycler);
        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_wellnessbp_swipe_refresh);

        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_wellnessbp);

        Functions.progressbarStyle(mProgressView, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {
                url = getString(R.string.urlLogin) + getString(R.string.LoadBPData) + userid;


                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_wellnessBP.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_wellnessBP = new LinearLayoutManager(getActivity());
                mRecyclerView_wellnessBP.setLayoutManager(mLayoutManager_wellnessBP);
                mRecyclerView_wellnessBP.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_wellnessBP.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadwellnessBPData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadwellnessBPData(url);

                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_wellnessBP.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_wellnessBP, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {

                        PHRMS_WellnessBP_Dialogue dialogue_WellnessBP = new PHRMS_WellnessBP_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("ResSystolic", ParseJson_WellnessBPData.ResSystolic[position]);
                        bundle.putString("ResDiastolic", ParseJson_WellnessBPData.ResDiastolic[position]);
                        bundle.putString("ResPulse", ParseJson_WellnessBPData.ResPulse[position]);
                        bundle.putString("strCollectionDate", ParseJson_WellnessBPData.strCollectionDate[position]);
                        bundle.putString("Comments", ParseJson_WellnessBPData.Comments[position]);
                        dialogue_WellnessBP.setArguments(bundle);
                        dialogue_WellnessBP.show(getFragmentManager(), "Blood Pressure Details");


                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            Intent intWellnessBPList = new Intent(getActivity(), PHRMS_WellnessBP_Fragment_Add.class);
                            startActivityForResult(intWellnessBPList, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }

                    }
                });

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_wellnessBP) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_wellnessBP, fab);
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
                if (data.getIntExtra("WellnessBPSaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout.setRefreshing(true);
                        LoadwellnessBPData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadwellnessBPData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadwellnessBPJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("BP Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadwellnessBPJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_WellnessBPData wellnessBP_pj = new ParseJson_WellnessBPData(jsonData);
        String STATUS = wellnessBP_pj.parseJson();
        if (STATUS.equals("1")) {
            txtwellnessBPData.setVisibility(View.GONE);

            wellnessBPDataList m_wellnessBPDataList = new wellnessBPDataList(getActivity(), ParseJson_WellnessBPData.ResSystolic, ParseJson_WellnessBPData.ResDiastolic, ParseJson_WellnessBPData.ResPulse, ParseJson_WellnessBPData.strCreatedDate);
            // specify an adapter (see also next example)
            mRecyclerView_wellnessBP.setAdapter(m_wellnessBPDataList);
            mSwipeRefreshLayout.setRefreshing(false);
            Functions.showProgress(false, mProgressView);
        } else {
            txtwellnessBPData.setVisibility(View.VISIBLE);
            mSwipeRefreshLayout.setRefreshing(false);
            Functions.showProgress(false, mProgressView);
        }

    }

    //extends Recyler adapter wellnessBPDataList and ViewHolder to hold data
    public class wellnessBPDataList extends RecyclerView.Adapter<wellnessBPDataList.ViewHolder> {
        private String[] wellnessBPSYS;
        private String[] wellnessBPDYS;
        private String[] wellnessBPPulseValue;
        private String[] createdDate;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_wellnessBPSYS, mTextView_wellnessBPDYS, mTextView_wellnessBPPulseValue, mTextView_createdDate;

            //Load Recyler adapter wellnessBPDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_wellnessBPSYS = (TextView) v.findViewById(R.id.txtactbpwellnessSystolic);
                mTextView_wellnessBPDYS = (TextView) v.findViewById(R.id.txtactbpwellnessDystolic);
                mTextView_wellnessBPPulseValue = (TextView) v.findViewById(R.id.txtactbpwellnessPulseValue);
                mTextView_createdDate = (TextView) v.findViewById(R.id.txtactbpcollectionDate);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public wellnessBPDataList(Activity context, String[] wellnessBPSYS, String[] wellnessBPDYS, String[] wellnessBPPulseValue, String[] createdDate) {
            this.context = context;
            this.wellnessBPSYS = wellnessBPSYS;
            this.wellnessBPDYS = wellnessBPDYS;
            this.wellnessBPPulseValue = wellnessBPPulseValue;
            this.createdDate = createdDate;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public wellnessBPDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {


            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listwellnessbp, parent, false);

            ViewHolder viewHolder = new ViewHolder(view);
            return viewHolder;
        }

        // Replace the contents of a view (invoked by the layout manager)
        @Override
        public void onBindViewHolder(ViewHolder holder, int position) {
            // - get element from your dataset at this position
            // - replace the contents of the view with that element
            holder.mTextView_wellnessBPSYS.setText(wellnessBPSYS[position]);
            holder.mTextView_wellnessBPDYS.setText(wellnessBPDYS[position]);
            holder.mTextView_wellnessBPPulseValue.setText(wellnessBPPulseValue[position]);
            holder.mTextView_createdDate.setText(createdDate[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {

            return (null != wellnessBPSYS ? wellnessBPSYS.length : 0);
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
        private PHRMS_WellnessBP_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_WellnessBP_Fragment.ClickListener clickListener) {
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
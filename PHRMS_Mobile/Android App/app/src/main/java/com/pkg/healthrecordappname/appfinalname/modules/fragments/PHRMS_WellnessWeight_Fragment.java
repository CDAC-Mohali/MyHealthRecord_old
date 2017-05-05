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
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_WellnessWeight_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_WellnessWeight_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_WellnessWeightData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;


public class PHRMS_WellnessWeight_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_wellnessWeight = null;
    private RecyclerView.LayoutManager mLayoutManager_wellnessWeight;
    private TextView txtwellnessWeightData;

    private ProgressBar mProgressView;

    private SwipeRefreshLayout mSwipeRefreshLayout;

    public PHRMS_WellnessWeight_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_wellnessweight, container, false);


        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);

        txtwellnessWeightData = (TextView) rootView.findViewById(R.id.txtwellnessWeightData);
        mRecyclerView_wellnessWeight = (RecyclerView) rootView.findViewById(R.id.lstdata_wellnessWeight_recycler);

        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_wellnessWeight_swipe_refresh);

        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_wellnessWeight);

        Functions.progressbarStyle(mProgressView, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {
                url = getString(R.string.urlLogin) + getString(R.string.LoadWeightData) + userid;


                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_wellnessWeight.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_wellnessWeight = new LinearLayoutManager(getActivity());
                mRecyclerView_wellnessWeight.setLayoutManager(mLayoutManager_wellnessWeight);
                mRecyclerView_wellnessWeight.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_wellnessWeight.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadwellnessWeightData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadwellnessWeightData(url);

                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_wellnessWeight.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_wellnessWeight, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {
                        PHRMS_WellnessWeight_Dialogue dialogue_WellnessWeight = new PHRMS_WellnessWeight_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("Weight", ParseJson_WellnessWeightData.Weight[position]);
                        bundle.putString("Height", ParseJson_WellnessWeightData.Height[position]);

                        String finalbmicValue = "-";
                        if (!Functions.isNullOrEmpty(ParseJson_WellnessWeightData.Weight[position]) && !Functions.isNullOrEmpty(ParseJson_WellnessWeightData.Height[position])) {
                            double bmic = Double.parseDouble(ParseJson_WellnessWeightData.Weight[position].toString()) / Math.pow(Double.parseDouble(ParseJson_WellnessWeightData.Height[position].toString()) / 100, 2.0);
                            finalbmicValue = String.valueOf(Math.round(bmic * 100.0) / 100.0);
                        }

                        bundle.putString("BMI", finalbmicValue);
                        bundle.putString("strCollectionDate", ParseJson_WellnessWeightData.strCollectionDate[position]);
                        bundle.putString("Comments", ParseJson_WellnessWeightData.Comments[position]);
                        dialogue_WellnessWeight.setArguments(bundle);
                        dialogue_WellnessWeight.show(getFragmentManager(), "Weight Details");

                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            Intent intWellnessWeightAdd = new Intent(getActivity(), PHRMS_WellnessWeight_Fragment_Add.class);
                            startActivityForResult(intWellnessWeightAdd, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }
                        //Snackbar.make(view, "Replace with your wellnessWeight action", Snackbar.LENGTH_LONG).setAction("Action", null).show();
                    }
                });

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_wellnessWeight) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_wellnessWeight, fab);
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
                if (data.getIntExtra("WellnessWeightSaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {

                        mSwipeRefreshLayout.setRefreshing(true);
                        LoadwellnessWeightData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadwellnessWeightData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadwellnessWeightJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {

                Functions.showProgress(false, mProgressView);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Weight Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadwellnessWeightJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_WellnessWeightData wellnessWeight_pj = new ParseJson_WellnessWeightData(jsonData);
        String STATUS = wellnessWeight_pj.parseJson();
        if (STATUS.equals("1")) {
            txtwellnessWeightData.setVisibility(View.GONE);

            wellnessWeightDataList m_wellnessWeightDataList = new wellnessWeightDataList(getActivity(), ParseJson_WellnessWeightData.Weight, ParseJson_WellnessWeightData.Height, ParseJson_WellnessWeightData.strCollectionDate, ParseJson_WellnessWeightData.SourceId);


            // specify an adapter (see also next example)
            mRecyclerView_wellnessWeight.setAdapter(m_wellnessWeightDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        } else {
            txtwellnessWeightData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        }

    }

    //extends Recyler adapter wellnessWeightDataList and ViewHolder to hold data
    public class wellnessWeightDataList extends RecyclerView.Adapter<wellnessWeightDataList.ViewHolder> {
        private String[] wellnessWeightValue;
        private String[] wellnessHeightValue;
        private String[] collectionDate;
        //Added to check if data is from EMR
        private String[] SourceId;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_wellnessWeightValue, mTextView_wellnessHeightValue, mTextView_collectionDate, mTextView_wellnessBMIValue;

            //Load Recyler adapter wellnessWeightDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_wellnessWeightValue = (TextView) v.findViewById(R.id.txtactWTwellnessWeightValue);
                mTextView_wellnessHeightValue = (TextView) v.findViewById(R.id.txtactWTwellnessHeightValue);
                mTextView_collectionDate = (TextView) v.findViewById(R.id.txtactWTcollectionDate);
                mTextView_wellnessBMIValue = (TextView) v.findViewById(R.id.txtactWTwellnessBMIValue);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public wellnessWeightDataList(Activity context, String[] wellnessWeightValue_DS, String[] wellnessHeightValue_DS, String[] collectionDate_DS, String[] SourceIdValue_DS) {
            this.context = context;
            this.wellnessWeightValue = wellnessWeightValue_DS;
            this.wellnessHeightValue = wellnessHeightValue_DS;
            this.collectionDate = collectionDate_DS;
            this.SourceId = SourceIdValue_DS;

        }

        // Create new views (invoked by the layout manager)
        @Override
        public wellnessWeightDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listwellnessweight, parent, false);

            ViewHolder viewHolder = new ViewHolder(view);
            return viewHolder;
        }

        // Replace the contents of a view (invoked by the layout manager)
        @Override
        public void onBindViewHolder(ViewHolder holder, int position) {

            // - get element from your dataset at this position
            // - replace the contents of the view with that element
            holder.mTextView_wellnessWeightValue.setText(wellnessWeightValue[position]);
            holder.mTextView_wellnessHeightValue.setText(wellnessHeightValue[position]);

            String finalbmicValue = "-";
            if (!Functions.isNullOrEmpty(wellnessWeightValue[position]) && !Functions.isNullOrEmpty(wellnessHeightValue[position])) {
                double bmic = Double.parseDouble(wellnessWeightValue[position].toString()) / Math.pow(Double.parseDouble(wellnessHeightValue[position].toString()) / 100, 2.0);
                finalbmicValue = String.valueOf(Math.round(bmic * 100.0) / 100.0);
            }

            holder.mTextView_wellnessBMIValue.setText(finalbmicValue);
            holder.mTextView_collectionDate.setText(collectionDate[position]);

        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != wellnessWeightValue ? wellnessWeightValue.length : 0);
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
        private PHRMS_WellnessWeight_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_WellnessWeight_Fragment.ClickListener clickListener) {
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
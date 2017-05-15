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
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_WellnessActivities_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_WellnessActivities_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_WellnessActivitiesData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.FontManager;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;


public class PHRMS_WellnessActivities_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_wellnessActivities = null;
    private RecyclerView.LayoutManager mLayoutManager_wellnessActivities;
    private TextView txtwellnessActivitiesData;

    private ProgressBar mProgressView;
    private SwipeRefreshLayout mSwipeRefreshLayout;


    public PHRMS_WellnessActivities_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_wellnessactivities, container, false);

        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);
        txtwellnessActivitiesData = (TextView) rootView.findViewById(R.id.txtwellnessActivitiesData);
        mRecyclerView_wellnessActivities = (RecyclerView) rootView.findViewById(R.id.lstdata_wellnessActivities_recycler);
        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_wellnessActivities_swipe_refresh);

        Typeface iconFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvWlActvityByUsers), iconFont);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvWlActvityByApp), iconFont);

        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_wellnessActivities);

        Functions.progressbarStyle(mProgressView, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {
                url = getString(R.string.urlLogin) + getString(R.string.LoadActivitiesData) + userid;


                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_wellnessActivities.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_wellnessActivities = new LinearLayoutManager(getActivity());
                mRecyclerView_wellnessActivities.setLayoutManager(mLayoutManager_wellnessActivities);
                mRecyclerView_wellnessActivities.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_wellnessActivities.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadwellnessActivitiesData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadwellnessActivitiesData(url);

                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_wellnessActivities.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_wellnessActivities, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {

                        PHRMS_WellnessActivities_Dialogue dialogue_WellnessActivities = new PHRMS_WellnessActivities_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("ActivityName", ParseJson_WellnessActivitiesData.ActivityName[position]);
                        bundle.putString("PathName", ParseJson_WellnessActivitiesData.PathName[position]);
                        bundle.putString("Distance", ParseJson_WellnessActivitiesData.Distance[position]);
                        bundle.putString("FinishTime", ParseJson_WellnessActivitiesData.FinishTime[position]);
                        bundle.putString("strCollectionDate", ParseJson_WellnessActivitiesData.strCollectionDate[position]);
                        bundle.putString("Comments", ParseJson_WellnessActivitiesData.Comments[position]);
                        dialogue_WellnessActivities.setArguments(bundle);
                        dialogue_WellnessActivities.show(getFragmentManager(), "Activities Details");

                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            Intent intWellnessActivityList = new Intent(getActivity(), PHRMS_WellnessActivities_Fragment_Add.class);
                            startActivityForResult(intWellnessActivityList, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }

                    }
                });

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_wellnessActivities) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_wellnessActivities, fab);
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
                if (data.getIntExtra("WellnessActivitySaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout.setRefreshing(true);
                        LoadwellnessActivitiesData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadwellnessActivitiesData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadwellnessActivitiesJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Activities Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadwellnessActivitiesJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_WellnessActivitiesData wellnessActivities_pj = new ParseJson_WellnessActivitiesData(jsonData);
        String STATUS = wellnessActivities_pj.parseJson();
        if (STATUS.equals("1")) {
            txtwellnessActivitiesData.setVisibility(View.GONE);

            wellnessActivitiesDataList m_wellnessActivitiesDataList = new wellnessActivitiesDataList(getActivity(), ParseJson_WellnessActivitiesData.ActivityName, ParseJson_WellnessActivitiesData.PathName, ParseJson_WellnessActivitiesData.Distance, ParseJson_WellnessActivitiesData.strCreatedDate, ParseJson_WellnessActivitiesData.SourceId);
            // specify an adapter (see also next example)
            mRecyclerView_wellnessActivities.setAdapter(m_wellnessActivitiesDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        } else {
            txtwellnessActivitiesData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        }

    }

    //extends Recyler adapter wellnessActivitiesDataList and ViewHolder to hold data
    public class wellnessActivitiesDataList extends RecyclerView.Adapter<wellnessActivitiesDataList.ViewHolder> {
        private String[] wellnessActivitiesName;
        private String[] wellnessActivityPathArea_Value;
        private String[] wellnessActivityDistance_value;
        private String[] createdDate;
        private String[] wellnessActivitySourceID_value;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_wellnessActivitiesName, mTextView_wellnessActivityPathArea_Value, mTextView_wellnessActivityDistance_value, mTextView_createdDate, mTextView_tvfontWellnessActivityUser, mTextView_tvfontWellnessActivityDoctor, mTextView_tvfontWellnessActivityios, mTextView_tvfontWellnessActivityandroid;

            //Load Recyler adapter wellnessActivitiesDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_wellnessActivitiesName = (TextView) v.findViewById(R.id.txtactwellnessActivityName);
                mTextView_wellnessActivityPathArea_Value = (TextView) v.findViewById(R.id.txtactwellnessActivityPathArea_Value);
                mTextView_wellnessActivityDistance_value = (TextView) v.findViewById(R.id.txtactwellnessActivityDistance_value);
                mTextView_createdDate = (TextView) v.findViewById(R.id.txtactcreatedDate);
                mTextView_tvfontWellnessActivityUser = (TextView) v.findViewById(R.id.tvfontWellnessActivityUser);
                mTextView_tvfontWellnessActivityDoctor = (TextView) v.findViewById(R.id.tvfontWellnessActivityDoctor);
                mTextView_tvfontWellnessActivityios = (TextView) v.findViewById(R.id.tvfontWellnessActivityios);
                mTextView_tvfontWellnessActivityandroid = (TextView) v.findViewById(R.id.tvfontWellnessActivityandroid);

                Typeface iconFontLVUserType = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
                FontManager.markAsIconContainer(v.findViewById(R.id.listUsersTypeActivity), iconFontLVUserType);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public wellnessActivitiesDataList(Activity context, String[] wellnessActivitiesName, String[] wellnessActivityPathArea_Value, String[] wellnessActivityDistance_value, String[] createdDate, String[] wellnessActivitySourceID_value) {
            this.context = context;
            this.wellnessActivitiesName = wellnessActivitiesName;
            this.wellnessActivityPathArea_Value = wellnessActivityPathArea_Value;
            this.wellnessActivityDistance_value = wellnessActivityDistance_value;
            this.createdDate = createdDate;
            this.wellnessActivitySourceID_value = wellnessActivitySourceID_value;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public wellnessActivitiesDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listwellnessactivities, parent, false);
            ViewHolder viewHolder = new ViewHolder(view);
            return viewHolder;
        }

        // Replace the contents of a view (invoked by the layout manager)
        @Override
        public void onBindViewHolder(ViewHolder holder, int position) {
            // - get element from your dataset at this position
            // - replace the contents of the view with that element


            if (wellnessActivitySourceID_value[position].equals("1")) {
                //user entered
                holder.mTextView_tvfontWellnessActivityUser.setVisibility(View.VISIBLE);

                holder.mTextView_tvfontWellnessActivityDoctor.setVisibility(View.GONE);
                holder.mTextView_tvfontWellnessActivityios.setVisibility(View.GONE);
                holder.mTextView_tvfontWellnessActivityandroid.setVisibility(View.GONE);
            } else if (wellnessActivitySourceID_value[position].equals("3")) {
                //ios app entered
                holder.mTextView_tvfontWellnessActivityios.setVisibility(View.VISIBLE);

                holder.mTextView_tvfontWellnessActivityDoctor.setVisibility(View.GONE);
                holder.mTextView_tvfontWellnessActivityUser.setVisibility(View.GONE);
                holder.mTextView_tvfontWellnessActivityandroid.setVisibility(View.GONE);
            } else if (wellnessActivitySourceID_value[position].equals("4")) {
                //android app entered
                holder.mTextView_tvfontWellnessActivityandroid.setVisibility(View.VISIBLE);

                holder.mTextView_tvfontWellnessActivityDoctor.setVisibility(View.GONE);
                holder.mTextView_tvfontWellnessActivityUser.setVisibility(View.GONE);
                holder.mTextView_tvfontWellnessActivityios.setVisibility(View.GONE);
            }


            holder.mTextView_wellnessActivitiesName.setText(wellnessActivitiesName[position]);
            holder.mTextView_wellnessActivityPathArea_Value.setText(wellnessActivityPathArea_Value[position]);
            holder.mTextView_wellnessActivityDistance_value.setText(wellnessActivityDistance_value[position]);
            holder.mTextView_createdDate.setText(createdDate[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != wellnessActivitiesName ? wellnessActivitiesName.length : 0);
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
        private PHRMS_WellnessActivities_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_WellnessActivities_Fragment.ClickListener clickListener) {
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
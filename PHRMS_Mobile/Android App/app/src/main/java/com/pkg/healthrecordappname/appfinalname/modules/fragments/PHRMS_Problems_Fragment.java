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
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_Problems_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_Problems_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_ProblemsData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.FontManager;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;

public class PHRMS_Problems_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_Problems = null;
    private RecyclerView.LayoutManager mLayoutManager_Problems;
    private TextView txtProblemsData = null;

    private ProgressBar mProgressView = null;
    private SwipeRefreshLayout mSwipeRefreshLayout;

    public PHRMS_Problems_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_problems, container, false);

        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);
        txtProblemsData = (TextView) rootView.findViewById(R.id.txtProblemsData);
        mRecyclerView_Problems = (RecyclerView) rootView.findViewById(R.id.lstdata_problems_recycler);
        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_problems_swipe_refresh);

        // Icon UsingFontAwesome
        Typeface iconFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvUsersPrb), iconFont);

        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_Problems);

        Functions.progressbarStyle(mProgressView, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {

                url = getString(R.string.urlLogin) + getString(R.string.LoadProblemsData) + userid;

                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_Problems.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_Problems = new LinearLayoutManager(getActivity());
                mRecyclerView_Problems.setLayoutManager(mLayoutManager_Problems);
                mRecyclerView_Problems.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_Problems.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadProblemsData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadProblemsData(url);

                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_Problems.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_Problems, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {

                        PHRMS_Problems_Dialogue dialogue_problems = new PHRMS_Problems_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("HealthCondition", ParseJson_ProblemsData.HealthCondition[position]);
                        bundle.putString("strDiagnosisDate", ParseJson_ProblemsData.strDiagnosisDate[position]);
                        bundle.putString("Provider", ParseJson_ProblemsData.Provider[position]);
                        bundle.putString("Notes", ParseJson_ProblemsData.Notes[position]);
                        bundle.putString("StillHaveCondition", ParseJson_ProblemsData.StillHaveCondition[position]);
                        dialogue_problems.setArguments(bundle);

                        //  Window window = dialogue_problems.getWindow();
                        // window.setLayout(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT);
                        dialogue_problems.show(getFragmentManager(), "Problems Details");


                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_Problems) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_Problems, fab);
                }

                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            //http://stackoverflow.com/questions/14292398/how-to-pass-data-from-2nd-activity-to-1st-activity-when-pressed-back-android
                            Intent intProblemsList = new Intent(getActivity(), PHRMS_Problems_Fragment_Add.class);
                            startActivityForResult(intProblemsList, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }
                    }
                });
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
                if (data.getIntExtra("ProblemsSaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout.setRefreshing(true);
                        LoadProblemsData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadProblemsData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadProblemsJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("problems Frame Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadProblemsJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_ProblemsData Problems_pj = new ParseJson_ProblemsData(jsonData);
        String STATUS = Problems_pj.parseJson();
        if (STATUS.equals("1")) {
            txtProblemsData.setVisibility(View.GONE);
            //CustomList cl = new CustomList(this, ParseJSON.ids,ParseJSON.names,ParseJSON.emails);
            //listView.setAdapter(cl);
            ProblemsDataList m_ProblemsDataList = new ProblemsDataList(getActivity(), ParseJson_ProblemsData.HealthCondition, ParseJson_ProblemsData.strStillHaveCondition, ParseJson_ProblemsData.strCreatedDate, ParseJson_ProblemsData.SourceId);
            //Problems_listView.setAdapter(m_ProblemsDataList);

            // specify an adapter (see also next example)
            mRecyclerView_Problems.setAdapter(m_ProblemsDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        } else {
            txtProblemsData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        }

    }

    //extends Recyler adapter ProblemsDataList and ViewHolder to hold data
    public class ProblemsDataList extends RecyclerView.Adapter<ProblemsDataList.ViewHolder> {
        private String[] ProblemsName;
        private String[] strStill_Have;
        private String[] strCreatedDate;
        private String[] SourceId;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_ProblemsName, mTextView_strStill_Have, mTextView_strCreatedDate, mTextView_tvfontUserPrb, mTextView_tvfontDoctorPrb;

            //Load Recyler adapter ProblemsDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_ProblemsName = (TextView) v.findViewById(R.id.txtprblmname);
                mTextView_strStill_Have = (TextView) v.findViewById(R.id.txtprblmstillhaveproblems_Value);
                mTextView_strCreatedDate = (TextView) v.findViewById(R.id.txtprblmstrCreatedDate);
                mTextView_tvfontUserPrb = (TextView) v.findViewById(R.id.tvfontUserPrb);
                mTextView_tvfontDoctorPrb = (TextView) v.findViewById(R.id.tvfontDoctorPrb);

                Typeface iconFontLVUserType = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
                FontManager.markAsIconContainer(v.findViewById(R.id.listUsersTypePrb), iconFontLVUserType);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ProblemsDataList(Activity context, String[] ProblemsName, String[] strStill_Have, String[] strCreatedDate, String[] SourceId) {
            this.context = context;
            this.ProblemsName = ProblemsName;
            this.strStill_Have = strStill_Have;
            this.strCreatedDate = strCreatedDate;
            this.SourceId = SourceId;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public ProblemsDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listproblems, parent, false);
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
                holder.mTextView_tvfontDoctorPrb.setVisibility(View.VISIBLE);
                holder.mTextView_tvfontUserPrb.setVisibility(View.GONE);
            } else {
                // Data Entered by User
                holder.mTextView_tvfontDoctorPrb.setVisibility(View.GONE);
                holder.mTextView_tvfontUserPrb.setVisibility(View.VISIBLE);
            }

            holder.mTextView_ProblemsName.setText(ProblemsName[position]);
            holder.mTextView_strStill_Have.setText(strStill_Have[position]);
            holder.mTextView_strCreatedDate.setText(strCreatedDate[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != ProblemsName ? ProblemsName.length : 0);
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
        private PHRMS_Problems_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_Problems_Fragment.ClickListener clickListener) {
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
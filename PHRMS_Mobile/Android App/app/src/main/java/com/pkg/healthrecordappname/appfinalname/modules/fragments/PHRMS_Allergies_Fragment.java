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
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_Allergies_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_Allergies_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_AllergyData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.FontManager;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;


public class PHRMS_Allergies_Fragment extends Fragment {
    String url = null;

    private TextView txtAllergyData = null;
    private RecyclerView mRecyclerView = null;
    private RecyclerView.LayoutManager mLayoutManager;
    private ProgressBar mProgressView = null;

    private SwipeRefreshLayout mSwipeRefreshLayout;

    public PHRMS_Allergies_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_allergies, container, false);
        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);
        txtAllergyData = (TextView) rootView.findViewById(R.id.txtAllerydata);
        mRecyclerView = (RecyclerView) rootView.findViewById(R.id.lstdata_allergies_recycler);
        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_allergies_swipe_refresh);

        // Icon UsingFontAwesome
        Typeface iconFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvUsersAlr), iconFont);
        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_Allegies);

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        Functions.progressbarStyle(mProgressView, getActivity());

        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Functions.mainscreen(getActivity());
            } else {
                url = getString(R.string.urlLogin) + getString(R.string.LoadAllergyData) + userid;

                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager = new LinearLayoutManager(getActivity());
                mRecyclerView.setLayoutManager(mLayoutManager);
                mRecyclerView.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadAllergyData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            if (Functions.isNetworkAvailable(getActivity())) {

                                LoadAllergyData(url);

                            } else {
                                Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                            }
                        }
                    }
                });


                // Added Custom RecyclerTouchListener
                mRecyclerView.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {
                        PHRMS_Allergies_Dialogue dialogue_allergy = new PHRMS_Allergies_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("AllergyName", ParseJson_AllergyData.AllergyName[position]);
                        bundle.putString("Still_Have", ParseJson_AllergyData.Still_Have[position]);
                        bundle.putString("strDuration", ParseJson_AllergyData.strDuration[position]);
                        bundle.putString("strSeverity", ParseJson_AllergyData.strSeverity[position]);
                        bundle.putString("Comments", ParseJson_AllergyData.Comments[position]);
                        dialogue_allergy.setArguments(bundle);

                        dialogue_allergy.show(getFragmentManager(), "Allergy Details");
                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView, fab);
                }

                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {

                            Intent intAllergyList = new Intent(getActivity(), PHRMS_Allergies_Fragment_Add.class);
                            startActivityForResult(intAllergyList, 1);
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
                if (data.getIntExtra("AllergySaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout.setRefreshing(true);
                        LoadAllergyData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadAllergyData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);

                Functions.ErrorHandling(getActivity(), error);

                txtAllergyData.setVisibility(View.VISIBLE);
                mSwipeRefreshLayout.setRefreshing(false);
                // TODO Auto-generated method stub
                Log.e("Allergies Frame Error", error.toString());
            }
        });


        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_AllergyData Allergy_pj = new ParseJson_AllergyData(jsonData);
        String STATUS = Allergy_pj.parseJson();
        if (STATUS.equals("1")) {
            txtAllergyData.setVisibility(View.GONE);

            AlleryDataList m_alleryDataList = new AlleryDataList(getActivity(), ParseJson_AllergyData.AllergyName, ParseJson_AllergyData.strStill_Have, ParseJson_AllergyData.strCreatedDate, ParseJson_AllergyData.strDuration, ParseJson_AllergyData.SourceId);

            // specify an adapter (see also next example)
            mRecyclerView.setAdapter(m_alleryDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        } else {
            txtAllergyData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        }


    }

    //extends Recyler adapter AlleryDataList and ViewHolder to hold data
    public class AlleryDataList extends RecyclerView.Adapter<AlleryDataList.ViewHolder> {
        private String[] AllergyName;
        private String[] strStill_Have;
        private String[] strCreatedDate;
        private String[] strDuration;
        private String[] SourceId;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_AllergyName, mTextView_strStill_Have, mTextView_strCreatedDate, mTextView_strDuration, mTextView_tvfontUserAlr, mTextView_tvfontDoctorAlr;

            //Load Recyler adapter AlleryDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_AllergyName = (TextView) v.findViewById(R.id.txtallrgname);
                mTextView_strStill_Have = (TextView) v.findViewById(R.id.txtallrgstillhaveallergy_Value);
                mTextView_strCreatedDate = (TextView) v.findViewById(R.id.txtallrgstrCreatedDate);
                mTextView_strDuration = (TextView) v.findViewById(R.id.txtallrgfromvalue);
                mTextView_tvfontUserAlr = (TextView) v.findViewById(R.id.tvfontUserAlr);
                mTextView_tvfontDoctorAlr = (TextView) v.findViewById(R.id.tvfontDoctorAlr);

                Typeface iconFontLVUserType = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
                FontManager.markAsIconContainer(v.findViewById(R.id.listUsersTypeAlr), iconFontLVUserType);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public AlleryDataList(Activity context, String[] AllergyName, String[] strStill_Have, String[] strCreatedDate, String[] strDuration, String[] SourceId) {
            this.context = context;
            this.AllergyName = AllergyName;
            this.strStill_Have = strStill_Have;
            this.strCreatedDate = strCreatedDate;
            this.strDuration = strDuration;
            this.SourceId = SourceId;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public AlleryDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listallergy, parent, false);
            ViewHolder viewHolder = new ViewHolder(view);

            return viewHolder;
        }

        // Replace the contents of a view (invoked by the layout manager)
        @Override
        public void onBindViewHolder(ViewHolder holder, int position) {
            // - get element from your dataset at this position
            // - replace the contents of the view with that element

            // To show data entered by - user [or] - Doctor( from EMR)
            // Data Entered by Doctor for source id - 2 or 5
            if (SourceId[position].equals("2") || SourceId[position].equals("5")) {
                holder.mTextView_tvfontDoctorAlr.setVisibility(View.VISIBLE);
                holder.mTextView_tvfontUserAlr.setVisibility(View.GONE);
            } else {
                // Data Entered by User
                holder.mTextView_tvfontDoctorAlr.setVisibility(View.GONE);
                holder.mTextView_tvfontUserAlr.setVisibility(View.VISIBLE);
            }

            holder.mTextView_AllergyName.setText(AllergyName[position]);
            holder.mTextView_strStill_Have.setText(strStill_Have[position]);
            holder.mTextView_strCreatedDate.setText(strCreatedDate[position]);
            holder.mTextView_strDuration.setText(strDuration[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != AllergyName ? AllergyName.length : 0);
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
        private PHRMS_Allergies_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_Allergies_Fragment.ClickListener clickListener) {
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
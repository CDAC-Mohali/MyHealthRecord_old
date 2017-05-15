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
import com.pkg.healthrecordappname.appfinalname.modules.addfragments.PHRMS_Procedures_Fragment_Add;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaldialogues.PHRMS_Procedures_Dialogue;
import com.pkg.healthrecordappname.appfinalname.modules.jsonparser.ParseJson_ProceduresData;
import com.pkg.healthrecordappname.appfinalname.modules.useables.FontManager;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.MySingleton;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONObject;


public class PHRMS_Procedures_Fragment extends Fragment {
    String url = null;

    private RecyclerView mRecyclerView_Procedures = null;
    private RecyclerView.LayoutManager mLayoutManager_Procedures;
    private TextView txtProceduresData = null;
    private ProgressBar mProgressView = null;
    private SwipeRefreshLayout mSwipeRefreshLayout;

    public PHRMS_Procedures_Fragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        final View rootView = inflater.inflate(R.layout.frame_procedures, container, false);

        mProgressView = (ProgressBar) getActivity().findViewById(R.id.data_progressbar);
        txtProceduresData = (TextView) rootView.findViewById(R.id.txtproceduresData);
        mRecyclerView_Procedures = (RecyclerView) rootView.findViewById(R.id.lstdata_procedures_recycler);
        mSwipeRefreshLayout = (SwipeRefreshLayout) rootView.findViewById(R.id.lstdata_procedures_swipe_refresh);

        // Icon UsingFontAwesome
        Typeface iconFont = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
        FontManager.markAsIconContainer(rootView.findViewById(R.id.lvUsersProc), iconFont);

        // Floating Action Button
        FloatingActionButton fab = (FloatingActionButton) rootView.findViewById(R.id.fab_Add_procedures);

        Functions.progressbarStyle(mProgressView, getActivity());

        String userid = Functions.decrypt(rootView.getContext(), Functions.pref.getString(Functions.P_UsrID, null));

        if (Functions.isNetworkAvailable(getActivity())) {
            if (Functions.isNullOrEmpty(userid)) {
                Intent intent = new Intent(getActivity(), PHRMS_LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
                getActivity().finish();
            } else {
                url = getString(R.string.urlLogin) + getString(R.string.LoadProceduresData) + userid;

                // use this setting to improve performance if you know that changes
                // in content do not change the layout size of the RecyclerView
                mRecyclerView_Procedures.setHasFixedSize(true);
                // use a linear layout manager
                mLayoutManager_Procedures = new LinearLayoutManager(getActivity());
                mRecyclerView_Procedures.setLayoutManager(mLayoutManager_Procedures);
                mRecyclerView_Procedures.setItemAnimator(new DefaultItemAnimator());
                mRecyclerView_Procedures.addItemDecoration(new RecycleView_DividerItemDecoration(getActivity(), LinearLayoutManager.VERTICAL));

                if (url != null) {
                    LoadProceduresData(url);
                }


                mSwipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
                    @Override
                    public void onRefresh() {
                        if (url != null) {
                            LoadProceduresData(url);

                        }
                    }
                });

                // Added Custom RecyclerTouchListener
                mRecyclerView_Procedures.addOnItemTouchListener(new RecyclerTouchListener(getActivity(), mRecyclerView_Procedures, new ClickListener() {
                    @Override
                    public void onClick(View view, int position) {

                        PHRMS_Procedures_Dialogue dialogue_procedure = new PHRMS_Procedures_Dialogue();
                        Bundle bundle = new Bundle();
                        bundle.putInt("Display", 1);
                        bundle.putString("ProcedureName", ParseJson_ProceduresData.ProcedureName[position]);
                        bundle.putString("strEndDate", ParseJson_ProceduresData.strEndDate[position]);
                        bundle.putString("SurgeonName", ParseJson_ProceduresData.SurgeonName[position]);
                        bundle.putString("Comments", ParseJson_ProceduresData.Comments[position]);
                        bundle.putString("arrImages", ParseJson_ProceduresData.arrImages[position]);
                        dialogue_procedure.setArguments(bundle);
                        dialogue_procedure.show(getFragmentManager(), "Procedures Details");

                    }

                    @Override
                    public void onLongClick(View view, int position) {

                    }
                }));


                fab.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        if (Functions.isNetworkAvailable(getActivity())) {
                            Intent intProceduresList = new Intent(getActivity(), PHRMS_Procedures_Fragment_Add.class);
                            startActivityForResult(intProceduresList, 1);
                        } else {
                            Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                        }

                    }
                });

                // To show and hide floating buttons
                if (null != getActivity() && null != mRecyclerView_Procedures) {
                    Functions.FloatTransitions(getActivity(), mRecyclerView_Procedures, fab);
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
                if (data.getIntExtra("ProceduresSaved", 0) == 1) {
                    if (Functions.isNetworkAvailable(getActivity())) {
                        mSwipeRefreshLayout.setRefreshing(true);
                        LoadProceduresData(url);
                    } else {
                        Functions.showSnackbar(getView(), "Internet Not Available !!", "Action");
                    }
                }
            }
        }
    }

    public void LoadProceduresData(String url) {
        Functions.showProgress(true, mProgressView);

        final JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject jsonData) {
                LoadProceduresJSONData(jsonData);
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Functions.showProgress(false, mProgressView);
                mSwipeRefreshLayout.setRefreshing(false);
                Functions.ErrorHandling(getActivity(), error);
                // TODO Auto-generated method stub
                Log.e("Procedures Frame Error", error.toString());
            }
        });
        // Access the RequestQueue through your singleton class.
        MySingleton.getInstance(getActivity()).addToRequestQueue(jsObjRequest);
    }

    private void LoadProceduresJSONData(JSONObject jsonData) {
        // Class to parse data and load in data arrays
        ParseJson_ProceduresData Procedures_pj = new ParseJson_ProceduresData(jsonData);
        String STATUS = Procedures_pj.parseJson();
        if (STATUS.equals("1")) {
            txtProceduresData.setVisibility(View.GONE);

            ProceduresDataList m_ProceduresDataList = new ProceduresDataList(getActivity(), ParseJson_ProceduresData.ProcedureName, ParseJson_ProceduresData.SurgeonName, ParseJson_ProceduresData.strCreatedDate, ParseJson_ProceduresData.SourceId);


            // specify an adapter (see also next example)
            mRecyclerView_Procedures.setAdapter(m_ProceduresDataList);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);

        } else {
            txtProceduresData.setVisibility(View.VISIBLE);
            Functions.showProgress(false, mProgressView);
            mSwipeRefreshLayout.setRefreshing(false);
        }

    }

    //extends Recyler adapter ProceduresDataList and ViewHolder to hold data
    public class ProceduresDataList extends RecyclerView.Adapter<ProceduresDataList.ViewHolder> {
        private String[] ProceduresName;
        private String[] SurgeonName;
        private String[] strCreatedDate;
        private String[] SourceId;
        private Activity context;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_ProceduresName, mTextView_SurgeonName, mTextView_strCreatedDate, mTextView_tvfontUserProc, mTextView_tvfontDoctorProc;

            //Load Recyler adapter ProceduresDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_ProceduresName = (TextView) v.findViewById(R.id.txtprocproceduresname);
                mTextView_SurgeonName = (TextView) v.findViewById(R.id.txtprocsurgeonname_Value);
                mTextView_strCreatedDate = (TextView) v.findViewById(R.id.txtprocstrCreatedDate);

                mTextView_tvfontUserProc = (TextView) v.findViewById(R.id.tvfontUserProc);
                mTextView_tvfontDoctorProc = (TextView) v.findViewById(R.id.tvfontDoctorProc);

                Typeface iconFontLVUserType = FontManager.getTypeface(getActivity().getApplicationContext(), FontManager.FONTAWESOME);
                FontManager.markAsIconContainer(v.findViewById(R.id.listUsersTypeProc), iconFontLVUserType);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ProceduresDataList(Activity context, String[] ProceduresName, String[] SurgeonName, String[] strCreatedDate, String[] SourceId) {
            this.context = context;
            this.ProceduresName = ProceduresName;
            this.SurgeonName = SurgeonName;
            this.strCreatedDate = strCreatedDate;
            this.SourceId = SourceId;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public ProceduresDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listprocedures, parent, false);
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
                holder.mTextView_tvfontDoctorProc.setVisibility(View.VISIBLE);
                holder.mTextView_tvfontUserProc.setVisibility(View.GONE);
            } else {
                // Data Entered by User
                holder.mTextView_tvfontDoctorProc.setVisibility(View.GONE);
                holder.mTextView_tvfontUserProc.setVisibility(View.VISIBLE);
            }

            holder.mTextView_ProceduresName.setText(ProceduresName[position]);

            if (Functions.isNullOrEmpty(SurgeonName[position])) {
                holder.mTextView_SurgeonName.setText("Not Available");
            } else {
                holder.mTextView_SurgeonName.setText(SurgeonName[position]);
            }

            holder.mTextView_strCreatedDate.setText(strCreatedDate[position]);
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != ProceduresName ? ProceduresName.length : 0);
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
        private PHRMS_Procedures_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_Procedures_Fragment.ClickListener clickListener) {
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
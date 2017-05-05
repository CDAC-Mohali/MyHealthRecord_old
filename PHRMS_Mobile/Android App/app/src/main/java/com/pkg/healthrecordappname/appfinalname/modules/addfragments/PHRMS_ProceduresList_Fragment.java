
package com.pkg.healthrecordappname.appfinalname.modules.addfragments;

import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.design.widget.TextInputLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.GestureDetector;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaladapters.ClinicalTerms;
import com.pkg.healthrecordappname.appfinalname.modules.httpconnections.HttpUrlConnectionRequest;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;



public class PHRMS_ProceduresList_Fragment extends AppCompatActivity {
    String url = null;
    private ArrayList mProceduresList;
    private DownloadProceduresTypes mProceduresListTask = null;
    private RecyclerView mRecyclerViewProceduresSearchList = null;
    private RecyclerView.LayoutManager mLayoutManagerProceduresList;

    private ProgressBar mProgressBarProceduresSearch;
    private TextInputLayout input_ProceduresSearch_value;
    private EditText edProceduresSearch_value;
    private View parentLayout;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_procedures_list);

        parentLayout = findViewById(R.id.lv_Search_Procedures);

        //toolbar
        Toolbar mtoolbar_add_Procedures = (Toolbar) findViewById(R.id.toolbar_searchProcedures);
        if (mtoolbar_add_Procedures != null) {
            setSupportActionBar(mtoolbar_add_Procedures);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarProceduresSearch = (ProgressBar) findViewById(R.id.ProgressBarProceduresSearch);


        input_ProceduresSearch_value = (TextInputLayout) findViewById(R.id.input_ProceduresSearch_value);
        edProceduresSearch_value = (EditText) findViewById(R.id.edProceduresSearch_value);


        mRecyclerViewProceduresSearchList = (RecyclerView) findViewById(R.id.rv_ProceduresSearch);
        // use this setting to improve performance if you know that changes
        // in content do not change the layout size of the RecyclerView
        mRecyclerViewProceduresSearchList.setHasFixedSize(true);
        // use a linear layout manager
        mLayoutManagerProceduresList = new LinearLayoutManager(PHRMS_ProceduresList_Fragment.this);
        mRecyclerViewProceduresSearchList.setLayoutManager(mLayoutManagerProceduresList);
        mRecyclerViewProceduresSearchList.setItemAnimator(new DefaultItemAnimator());
        mRecyclerViewProceduresSearchList.addItemDecoration(new RecycleView_DividerItemDecoration(PHRMS_ProceduresList_Fragment.this, LinearLayoutManager.VERTICAL));

        Functions.progressbarStyle(mProgressBarProceduresSearch, PHRMS_ProceduresList_Fragment.this);

        String userid = Functions.decrypt(PHRMS_ProceduresList_Fragment.this, Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(PHRMS_ProceduresList_Fragment.this);
        } else {

            url = getString(R.string.urlLogin) + getString(R.string.GetProceduresTypes);

            edProceduresSearch_value.addTextChangedListener(new TextWatcher() {
                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {
                    // TODO Auto-generated method stub
                    if (url != null) {

                        if (s.toString().trim().length() < 3)
                        {
                            mRecyclerViewProceduresSearchList.setVisibility(View.GONE);
                            Functions.showToast(PHRMS_ProceduresList_Fragment.this, "Search Procedures with atleast 3 or more characters");
                        } else {
                            if (Functions.isNetworkAvailable(PHRMS_ProceduresList_Fragment.this)) {
                                mProceduresListTask = new DownloadProceduresTypes();
                                mProceduresListTask.execute(s.toString());
                            } else {
                                Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
                            }

                        }
                    }
                }

                @Override
                public void beforeTextChanged(CharSequence s, int start, int count,
                                              int after) {
                    // TODO Auto-generated method stub

                }

                @Override
                public void afterTextChanged(Editable editautocomplete) {
                    // TODO Auto-generated method stub

                }
            });


            // Added Custom RecyclerTouchListener
            mRecyclerViewProceduresSearchList.addOnItemTouchListener(new RecyclerTouchListener(PHRMS_ProceduresList_Fragment.this, mRecyclerViewProceduresSearchList, new ClickListener() {
                @Override
                public void onClick(View view, int position) {
                    if (mProceduresList.size() > 0) {
                        ClinicalTerms ct = (ClinicalTerms) mProceduresList.get(position);

                        Intent intAddProcedures = new Intent(PHRMS_ProceduresList_Fragment.this, PHRMS_Procedures_Fragment_Add.class);
                        intAddProcedures.putExtra("Procedures", 1);
                        intAddProcedures.putExtra("ProceduresID", ct.getId());
                        intAddProcedures.putExtra("ProceduresName", ct.getName());
                        setResult(RESULT_OK, intAddProcedures);
                        finish();


                    }
                }

                @Override
                public void onLongClick(View view, int position) {
                }
            }));
        }

    }

    protected class DownloadProceduresTypes extends AsyncTask<String, String, ArrayList> {

        @Override
        protected void onPreExecute() {
            mProgressBarProceduresSearch.setVisibility(View.VISIBLE);
        }

        @Override
        protected ArrayList doInBackground(String... params) {
            try {

                String jsonString = HttpUrlConnectionRequest.sendPost(url, params[0]);
                Boolean emptyArray = (jsonString.equals("[]")) ? true : false;

                ArrayList ClinicalTermsList = new ArrayList<>();

                if (!Functions.isNullOrEmpty(jsonString) && jsonString != null && !jsonString.isEmpty() && emptyArray == false)
                {
                    //JSONObject
                    JSONArray jsonArray = new JSONArray(jsonString);

                    if (jsonArray != null && jsonArray.length() > 0)
                    {
                        for (int i = 0; i < jsonArray.length(); i++)
                        {
                            JSONObject jo = jsonArray.getJSONObject(i);
                            //store the ClinicalTerms name
                            ClinicalTerms ClinicalTerms = new ClinicalTerms();
                            ClinicalTerms.setName(jo.getString("ProcedureName"));
                            ClinicalTerms.setId(jo.getString("Id"));
                            ClinicalTermsList.add(ClinicalTerms);
                        }
                    }
                }

                //return the ClinicalTermsList
                return ClinicalTermsList;

            } catch (Exception e) {
                Log.d("HUS", "EXCEPTION " + e.toString());
                return null;
            }
        }

        @Override
        protected void onPostExecute(ArrayList resultlist)
        {
            if (resultlist.size() > 0)
            {
                mRecyclerViewProceduresSearchList.setVisibility(View.VISIBLE);
                // Load result arraylist in recycleview
                mProceduresList = resultlist;
                ProceduresDataList m_ProceduresDataList = new ProceduresDataList(mProceduresList);
                mRecyclerViewProceduresSearchList.setAdapter(m_ProceduresDataList);

            }
            else
            {
                mRecyclerViewProceduresSearchList.setVisibility(View.GONE);
                Functions.showToast(PHRMS_ProceduresList_Fragment.this, "No result found");
            }


            mProgressBarProceduresSearch.setVisibility(View.GONE);
        }

    }


    //extends Recyler adapter ProceduresDataList and ViewHolder to hold data
    public class ProceduresDataList extends RecyclerView.Adapter<ProceduresDataList.ViewHolder> {

        private ArrayList al_Procedures;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_ProceduresName, mTextView_ProceduresId;

            //Load Recyler adapter ProceduresDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_ProceduresName = (TextView) v.findViewById(R.id.txttype);
                mTextView_ProceduresId = (TextView) v.findViewById(R.id.txtid);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ProceduresDataList(ArrayList ar) {
            this.al_Procedures = ar;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public ProceduresDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.autocomplete, parent, false);
            ViewHolder viewHolder = new ViewHolder(view);

            return viewHolder;
        }

        // Replace the contents of a view (invoked by the layout manager)
        @Override
        public void onBindViewHolder(ViewHolder holder, int position) {
            // - get element from your dataset at this position
            // - replace the contents of the view with that element
            //get ClinicalTerms
            ClinicalTerms ct = (ClinicalTerms) al_Procedures.get(position);
            holder.mTextView_ProceduresId.setText(ct.getId());
            holder.mTextView_ProceduresName.setText(ct.getName());
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != al_Procedures ? al_Procedures.size() : 0);
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
        private PHRMS_ProceduresList_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_ProceduresList_Fragment.ClickListener clickListener) {
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


    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                finish();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }



}

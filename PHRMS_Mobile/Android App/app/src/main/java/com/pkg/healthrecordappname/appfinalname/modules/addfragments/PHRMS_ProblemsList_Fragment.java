
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


public class PHRMS_ProblemsList_Fragment extends AppCompatActivity {
    String url = null;
    private ArrayList mProblemsList;
    private DownloadProblemsTypes mProblemsListTask = null;
    private RecyclerView mRecyclerViewProblemsSearchList = null;
    private RecyclerView.LayoutManager mLayoutManagerProblemsList;

    private ProgressBar mProgressBarProblemsSearch;
    private TextInputLayout input_ProblemsSearch_value;
    private EditText edProblemsSearch_value;
    private View parentLayout;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_problems_list);

        parentLayout = findViewById(R.id.lv_Search_problems);

        //toolbar
        Toolbar mtoolbar_add_Problems = (Toolbar) findViewById(R.id.toolbar_searchproblems);
        if (mtoolbar_add_Problems != null) {
            setSupportActionBar(mtoolbar_add_Problems);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarProblemsSearch = (ProgressBar) findViewById(R.id.ProgressBarproblemsSearch);


        input_ProblemsSearch_value = (TextInputLayout) findViewById(R.id.input_problemsSearch_value);
        edProblemsSearch_value = (EditText) findViewById(R.id.edproblemsSearch_value);


        mRecyclerViewProblemsSearchList = (RecyclerView) findViewById(R.id.rv_problemsSearch);
        // use this setting to improve performance if you know that changes
        // in content do not change the layout size of the RecyclerView
        mRecyclerViewProblemsSearchList.setHasFixedSize(true);
        // use a linear layout manager
        mLayoutManagerProblemsList = new LinearLayoutManager(PHRMS_ProblemsList_Fragment.this);
        mRecyclerViewProblemsSearchList.setLayoutManager(mLayoutManagerProblemsList);
        mRecyclerViewProblemsSearchList.setItemAnimator(new DefaultItemAnimator());
        mRecyclerViewProblemsSearchList.addItemDecoration(new RecycleView_DividerItemDecoration(PHRMS_ProblemsList_Fragment.this, LinearLayoutManager.VERTICAL));

        Functions.progressbarStyle(mProgressBarProblemsSearch, PHRMS_ProblemsList_Fragment.this);

        String userid = Functions.decrypt(PHRMS_ProblemsList_Fragment.this, Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(PHRMS_ProblemsList_Fragment.this);
        } else {

            url = getString(R.string.urlLogin) + getString(R.string.GetProblemsTypes);

            edProblemsSearch_value.addTextChangedListener(new TextWatcher() {
                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {
                    // TODO Auto-generated method stub
                    if (url != null) {

                        if (s.toString().trim().length() < 1)
                        {
                            mRecyclerViewProblemsSearchList.setVisibility(View.GONE);
                            Functions.showToast(PHRMS_ProblemsList_Fragment.this, "Search Problems with one or more characters");
                        }
                        else
                            {
                            if (Functions.isNetworkAvailable(PHRMS_ProblemsList_Fragment.this)) {
                                mProblemsListTask = new DownloadProblemsTypes();
                                mProblemsListTask.execute(s.toString());
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
            mRecyclerViewProblemsSearchList.addOnItemTouchListener(new RecyclerTouchListener(PHRMS_ProblemsList_Fragment.this, mRecyclerViewProblemsSearchList, new ClickListener() {
                @Override
                public void onClick(View view, int position) {
                    if (mProblemsList.size() > 0) {
                        ClinicalTerms ct = (ClinicalTerms) mProblemsList.get(position);

                        Intent intAddProblems = new Intent(PHRMS_ProblemsList_Fragment.this, PHRMS_Allergies_Fragment_Add.class);
                        intAddProblems.putExtra("Problems", 1);
                        intAddProblems.putExtra("ProblemsID", ct.getId());
                        intAddProblems.putExtra("ProblemsName", ct.getName());
                        setResult(RESULT_OK, intAddProblems);
                        finish();


                    }
                }

                @Override
                public void onLongClick(View view, int position) {
                }
            }));
        }

    }

    protected class DownloadProblemsTypes extends AsyncTask<String, String, ArrayList> {

        @Override
        protected void onPreExecute() {
            mProgressBarProblemsSearch.setVisibility(View.VISIBLE);
        }

        @Override
        protected ArrayList doInBackground(String... params)
        {
            try
            {

                String jsonString = HttpUrlConnectionRequest.sendPost(url, params[0]);
                Boolean emptyArray = (jsonString.equals("[]")) ? true : false;

                ArrayList ClinicalTermsList = new ArrayList<>();

                if (!Functions.isNullOrEmpty(jsonString) && jsonString != null && !jsonString.isEmpty() && emptyArray == false)
                {
                    //JSONArray
                    JSONArray jsonArray = new JSONArray(jsonString);

                    if (jsonArray != null && jsonArray.length() > 0)
                    {
                        for (int i = 0; i < jsonArray.length(); i++)
                        {
                            JSONObject jo = jsonArray.getJSONObject(i);
                            //store the ClinicalTerms name
                            ClinicalTerms ClinicalTerms = new ClinicalTerms();
                            ClinicalTerms.setName(jo.getString("HealthCondition"));
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
                mRecyclerViewProblemsSearchList.setVisibility(View.VISIBLE);
                // Load result arraylist in recycleview
                mProblemsList = resultlist;
                ProblemsDataList m_ProblemsDataList = new ProblemsDataList(mProblemsList);
                mRecyclerViewProblemsSearchList.setAdapter(m_ProblemsDataList);

            }
            else
            {
                mRecyclerViewProblemsSearchList.setVisibility(View.GONE);
                Functions.showToast(PHRMS_ProblemsList_Fragment.this, "No result found");
            }
            mProgressBarProblemsSearch.setVisibility(View.GONE);
        }

    }


    //extends Recyler adapter ProblemsDataList and ViewHolder to hold data
    public class ProblemsDataList extends RecyclerView.Adapter<ProblemsDataList.ViewHolder> {

        private ArrayList al_Problems;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_ProblemsName, mTextView_ProblemsId;

            //Load Recyler adapter ProblemsDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_ProblemsName = (TextView) v.findViewById(R.id.txttype);
                mTextView_ProblemsId = (TextView) v.findViewById(R.id.txtid);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ProblemsDataList(ArrayList ar) {
            this.al_Problems = ar;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public ProblemsDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.autocomplete, parent, false);
            ViewHolder viewHolder = new ViewHolder(view);

            return viewHolder;
        }

        // Replace the contents of a view (invoked by the layout manager)
        @Override
        public void onBindViewHolder(ViewHolder holder, int position) {
            // - get element from your dataset at this position
            // - replace the contents of the view with that element

            ClinicalTerms ct = (ClinicalTerms) al_Problems.get(position);
            holder.mTextView_ProblemsId.setText(ct.getId());
            holder.mTextView_ProblemsName.setText(ct.getName());
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements

            return (null != al_Problems ? al_Problems.size() : 0);
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
        private PHRMS_ProblemsList_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_ProblemsList_Fragment.ClickListener clickListener) {
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

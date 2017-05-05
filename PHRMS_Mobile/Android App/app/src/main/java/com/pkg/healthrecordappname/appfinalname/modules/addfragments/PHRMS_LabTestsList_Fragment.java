
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


/**
 * A placeholder fragment containing a simple view for frame_LabTests.xml layout.
 */
public class PHRMS_LabTestsList_Fragment extends AppCompatActivity {
    String url = null;
    private ArrayList mLabTestsList;
    private DownloadLabTestsTypes mLabTestsListTask = null;
    private RecyclerView mRecyclerViewLabTestsSearchList = null;
    private RecyclerView.LayoutManager mLayoutManagerLabTestsList;

    private ProgressBar mProgressBarLabTestsSearch;
    private TextInputLayout input_LabTestsSearch_value;
    private EditText edLabTestsSearch_value;
    private View parentLayout;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_labtests_list);

        parentLayout = findViewById(R.id.lv_Search_LabTests);

        //toolbar
        Toolbar mtoolbar_add_LabTests = (Toolbar) findViewById(R.id.toolbar_searchLabTests);
        if (mtoolbar_add_LabTests != null) {
            setSupportActionBar(mtoolbar_add_LabTests);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarLabTestsSearch = (ProgressBar) findViewById(R.id.ProgressBarLabTestsSearch);


        input_LabTestsSearch_value = (TextInputLayout) findViewById(R.id.input_LabTestsSearch_value);
        edLabTestsSearch_value = (EditText) findViewById(R.id.edLabTestsSearch_value);


        mRecyclerViewLabTestsSearchList = (RecyclerView) findViewById(R.id.rv_LabTestsSearch);
        // use this setting to improve performance if you know that changes
        // in content do not change the layout size of the RecyclerView
        mRecyclerViewLabTestsSearchList.setHasFixedSize(true);
        // use a linear layout manager
        mLayoutManagerLabTestsList = new LinearLayoutManager(PHRMS_LabTestsList_Fragment.this);
        mRecyclerViewLabTestsSearchList.setLayoutManager(mLayoutManagerLabTestsList);
        mRecyclerViewLabTestsSearchList.setItemAnimator(new DefaultItemAnimator());
        mRecyclerViewLabTestsSearchList.addItemDecoration(new RecycleView_DividerItemDecoration(PHRMS_LabTestsList_Fragment.this, LinearLayoutManager.VERTICAL));

        Functions.progressbarStyle(mProgressBarLabTestsSearch, PHRMS_LabTestsList_Fragment.this);

        String userid = Functions.decrypt(PHRMS_LabTestsList_Fragment.this, Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(PHRMS_LabTestsList_Fragment.this);
        } else {
            //http://phrms.cloudapp.net:8085/API/LabTests/GetLabTestsTypes
            url = getString(R.string.urlLogin) + getString(R.string.GetLabTypes);

            edLabTestsSearch_value.addTextChangedListener(new TextWatcher() {
                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {
                    // TODO Auto-generated method stub
                    if (url != null) {
                        //if (s.toString().trim().isEmpty() || s.toString().length() <= 1)
                        if (s.toString().trim().length() < 1)
                        {
                            mRecyclerViewLabTestsSearchList.setVisibility(View.GONE);
                            Functions.showToast(PHRMS_LabTestsList_Fragment.this, "Search LabTests with one or more characters");
                        } else {
                            if (Functions.isNetworkAvailable(PHRMS_LabTestsList_Fragment.this)) {
                                mLabTestsListTask = new DownloadLabTestsTypes();
                                mLabTestsListTask.execute(s.toString());
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
            mRecyclerViewLabTestsSearchList.addOnItemTouchListener(new RecyclerTouchListener(PHRMS_LabTestsList_Fragment.this, mRecyclerViewLabTestsSearchList, new ClickListener() {
                @Override
                public void onClick(View view, int position) {
                    if (mLabTestsList.size() > 0) {
                        ClinicalTerms ct = (ClinicalTerms) mLabTestsList.get(position);
                        //edLabTestsSearch_value.setText(ct.getName());
                        //Functions.showToast(PHRMS_LabTestsList_Fragment.this, "ID:" + ct.getId().toString() + "  LabTests Name: " + ct.getName().toString());

                        Intent intAddLabTests = new Intent(PHRMS_LabTestsList_Fragment.this, PHRMS_LabTests_Fragment_Add.class);
                        intAddLabTests.putExtra("LabTests", 1);
                        intAddLabTests.putExtra("LabTestsID", ct.getId());
                        intAddLabTests.putExtra("LabTestsName", ct.getName());
                        setResult(RESULT_OK, intAddLabTests);
                        finish();


                    }
                }

                @Override
                public void onLongClick(View view, int position) {
                }
            }));
        }

    }

    protected class DownloadLabTestsTypes extends AsyncTask<String, String, ArrayList> {

        @Override
        protected void onPreExecute() {
            mProgressBarLabTestsSearch.setVisibility(View.VISIBLE);
        }

        @Override
        protected ArrayList doInBackground(String... params) {
            try {
                //HttpUrlConnectionRequest htt = new HttpUrlConnectionRequest();
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
                            ClinicalTerms.setName(jo.getString("TestName"));
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
                mRecyclerViewLabTestsSearchList.setVisibility(View.VISIBLE);
                // Load result arraylist in recycleview
                mLabTestsList = resultlist;
                LabTestsDataList m_LabTestsDataList = new LabTestsDataList(mLabTestsList);
                mRecyclerViewLabTestsSearchList.setAdapter(m_LabTestsDataList);

            }
            else
            {
                mRecyclerViewLabTestsSearchList.setVisibility(View.GONE);
                Functions.showToast(PHRMS_LabTestsList_Fragment.this, "No result found");
            }

            mProgressBarLabTestsSearch.setVisibility(View.GONE);
        }

    }


    //extends Recyler adapter LabTestsDataList and ViewHolder to hold data
    public class LabTestsDataList extends RecyclerView.Adapter<LabTestsDataList.ViewHolder> {

        private ArrayList al_LabTests;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_LabTestsName, mTextView_LabTestsId;

            //Load Recyler adapter LabTestsDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_LabTestsName = (TextView) v.findViewById(R.id.txttype);
                mTextView_LabTestsId = (TextView) v.findViewById(R.id.txtid);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public LabTestsDataList(ArrayList ar) {
            this.al_LabTests = ar;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public LabTestsDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
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
            ClinicalTerms ct = (ClinicalTerms) al_LabTests.get(position);
            holder.mTextView_LabTestsId.setText(ct.getId());
            holder.mTextView_LabTestsName.setText(ct.getName());
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            // Return no. of values elements
            //return LabTestsName.length;
            return (null != al_LabTests ? al_LabTests.size() : 0);
        }

        public void addItem(ViewHolder holder, int position) {
            //mDataset.add(index, dataObj);
            //notifyItemInserted(index);
        }

        public void deleteItem(ViewHolder holder, int position) {
            //mDataset.remove(index);
            //notifyItemRemoved(index);
        }
    }

    // recycleview TouchListener
    public interface ClickListener {
        void onClick(View view, int position);

        void onLongClick(View view, int position);
    }

    public static class RecyclerTouchListener implements RecyclerView.OnItemTouchListener {

        private GestureDetector gestureDetector;
        private PHRMS_LabTestsList_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_LabTestsList_Fragment.ClickListener clickListener) {
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

    /*
    // Here you will enable Multidex
    @Override
    protected void attachBaseContext(Context base) {
        super.attachBaseContext(base);
        MultiDex.install(getBaseContext());
    }
    */

}

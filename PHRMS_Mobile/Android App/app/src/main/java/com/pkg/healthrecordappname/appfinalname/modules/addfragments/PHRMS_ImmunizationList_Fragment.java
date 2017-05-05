
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


public class PHRMS_ImmunizationList_Fragment extends AppCompatActivity {
    String url = null;
    private ArrayList mImmunizationList;
    private DownloadImmunizationTypes mImmunizationListTask = null;
    private RecyclerView mRecyclerViewImmunizationSearchList = null;
    private RecyclerView.LayoutManager mLayoutManagerImmunizationList;

    private ProgressBar mProgressBarImmunizationSearch;
    private TextInputLayout input_ImmunizationSearch_value;
    private EditText edImmunizationSearch_value;
    private View parentLayout;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_immunization_list);

        parentLayout = findViewById(R.id.lv_Search_Immunization);

        //toolbar
        Toolbar mtoolbar_add_Immunization = (Toolbar) findViewById(R.id.toolbar_searchImmunization);
        if (mtoolbar_add_Immunization != null) {
            setSupportActionBar(mtoolbar_add_Immunization);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarImmunizationSearch = (ProgressBar) findViewById(R.id.ProgressBarImmunizationSearch);


        input_ImmunizationSearch_value = (TextInputLayout) findViewById(R.id.input_ImmunizationSearch_value);
        edImmunizationSearch_value = (EditText) findViewById(R.id.edImmunizationSearch_value);


        mRecyclerViewImmunizationSearchList = (RecyclerView) findViewById(R.id.rv_ImmunizationSearch);
        // use this setting to improve performance if you know that changes
        // in content do not change the layout size of the RecyclerView
        mRecyclerViewImmunizationSearchList.setHasFixedSize(true);
        // use a linear layout manager
        mLayoutManagerImmunizationList = new LinearLayoutManager(PHRMS_ImmunizationList_Fragment.this);
        mRecyclerViewImmunizationSearchList.setLayoutManager(mLayoutManagerImmunizationList);
        mRecyclerViewImmunizationSearchList.setItemAnimator(new DefaultItemAnimator());
        mRecyclerViewImmunizationSearchList.addItemDecoration(new RecycleView_DividerItemDecoration(PHRMS_ImmunizationList_Fragment.this, LinearLayoutManager.VERTICAL));

        Functions.progressbarStyle(mProgressBarImmunizationSearch, PHRMS_ImmunizationList_Fragment.this);

        String userid = Functions.decrypt(PHRMS_ImmunizationList_Fragment.this, Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(PHRMS_ImmunizationList_Fragment.this);
        } else {

            url = getString(R.string.urlLogin) + getString(R.string.GetImmunizationTypes);

            edImmunizationSearch_value.addTextChangedListener(new TextWatcher() {
                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {
                    // TODO Auto-generated method stub
                    if (url != null) {

                        if (s.toString().trim().length() < 1) {
                            mRecyclerViewImmunizationSearchList.setVisibility(View.GONE);
                            Functions.showToast(PHRMS_ImmunizationList_Fragment.this, "Search Immunization with one or more characters");
                        } else {
                            if (Functions.isNetworkAvailable(PHRMS_ImmunizationList_Fragment.this)) {
                                mImmunizationListTask = new DownloadImmunizationTypes();
                                mImmunizationListTask.execute(s.toString());
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
            mRecyclerViewImmunizationSearchList.addOnItemTouchListener(new RecyclerTouchListener(PHRMS_ImmunizationList_Fragment.this, mRecyclerViewImmunizationSearchList, new ClickListener() {
                @Override
                public void onClick(View view, int position) {
                    if (mImmunizationList.size() > 0) {
                        ClinicalTerms ct = (ClinicalTerms) mImmunizationList.get(position);


                        Intent intAddImmunization = new Intent(PHRMS_ImmunizationList_Fragment.this, PHRMS_Allergies_Fragment_Add.class);
                        intAddImmunization.putExtra("Immunization", 1);
                        intAddImmunization.putExtra("ImmunizationID", ct.getId());
                        intAddImmunization.putExtra("ImmunizationName", ct.getName());
                        setResult(RESULT_OK, intAddImmunization);
                        finish();


                    }
                }

                @Override
                public void onLongClick(View view, int position) {
                }
            }));
        }

    }

    protected class DownloadImmunizationTypes extends AsyncTask<String, String, ArrayList> {

        @Override
        protected void onPreExecute() {
            mProgressBarImmunizationSearch.setVisibility(View.VISIBLE);
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
                            ClinicalTerms.setName(jo.getString("ImmunizationName"));
                            ClinicalTerms.setId(jo.getString("ImmunizationsTypeId"));
                            ClinicalTermsList.add(ClinicalTerms);
                        }
                    }
                }


                //return the ClinicalTermsList
                return ClinicalTermsList;

            }
            catch (Exception e)
            {
                Log.d("HUS", "EXCEPTION " + e.toString());
                return null;
            }
        }

        @Override
        protected void onPostExecute(ArrayList resultlist) {
            if (resultlist.size() > 0) {
                mRecyclerViewImmunizationSearchList.setVisibility(View.VISIBLE);
                // Load result arraylist in recycleview
                mImmunizationList = resultlist;
                ImmunizationDataList m_ImmunizationDataList = new ImmunizationDataList(mImmunizationList);
                mRecyclerViewImmunizationSearchList.setAdapter(m_ImmunizationDataList);

            } else {

                mRecyclerViewImmunizationSearchList.setVisibility(View.GONE);
                Functions.showToast(PHRMS_ImmunizationList_Fragment.this, "No result found");
            }

            mProgressBarImmunizationSearch.setVisibility(View.GONE);
        }

    }


    //extends Recyler adapter ImmunizationDataList and ViewHolder to hold data
    public class ImmunizationDataList extends RecyclerView.Adapter<ImmunizationDataList.ViewHolder> {

        private ArrayList al_Immunization;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_ImmunizationName, mTextView_ImmunizationId;

            //Load Recyler adapter ImmunizationDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_ImmunizationName = (TextView) v.findViewById(R.id.txttype);
                mTextView_ImmunizationId = (TextView) v.findViewById(R.id.txtid);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ImmunizationDataList(ArrayList ar) {
            this.al_Immunization = ar;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public ImmunizationDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.autocomplete, parent, false);
            ViewHolder viewHolder = new ViewHolder(view);

            return viewHolder;
        }

        // Replace the contents of a view (invoked by the layout manager)
        @Override
        public void onBindViewHolder(ViewHolder holder, int position) {
            //get ClinicalTerms
            ClinicalTerms ct = (ClinicalTerms) al_Immunization.get(position);
            holder.mTextView_ImmunizationId.setText(ct.getId());
            holder.mTextView_ImmunizationName.setText(ct.getName());
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {

            return (null != al_Immunization ? al_Immunization.size() : 0);
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
        private PHRMS_ImmunizationList_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_ImmunizationList_Fragment.ClickListener clickListener) {
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

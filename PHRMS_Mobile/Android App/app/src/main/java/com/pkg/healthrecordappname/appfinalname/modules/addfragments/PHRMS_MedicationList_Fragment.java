
package com.pkg.healthrecordappname.appfinalname.modules.addfragments;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.support.design.widget.TextInputLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
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
import com.pkg.healthrecordappname.appfinalname.modules.useables.Data_Service_MedicationList;
import com.pkg.healthrecordappname.appfinalname.modules.useables.DownloadResultReceiver;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import java.util.ArrayList;


public class PHRMS_MedicationList_Fragment extends AppCompatActivity implements DownloadResultReceiver.Receiver {
    String url = null;


    private ArrayList ClinicalTermsList = null;



    private RecyclerView mRecyclerViewMedicationSearchList = null;
    private RecyclerView.LayoutManager mLayoutManagerMedicationList;

    private ProgressBar mProgressBarMedicationSearch;
    private TextInputLayout input_MedicationSearch_value;
    private EditText edMedicationSearch_value;
    private View parentLayout;



    private DownloadResultReceiver mReceiver;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_medication_list);

        parentLayout = findViewById(R.id.lv_Search_Medication);

        //toolbar
        Toolbar mtoolbar_add_Medication = (Toolbar) findViewById(R.id.toolbar_searchMedication);
        if (mtoolbar_add_Medication != null) {
            setSupportActionBar(mtoolbar_add_Medication);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarMedicationSearch = (ProgressBar) findViewById(R.id.ProgressBarMedicationSearch);



        input_MedicationSearch_value = (TextInputLayout) findViewById(R.id.input_MedicationSearch_value);
        edMedicationSearch_value = (EditText) findViewById(R.id.edMedicationSearch_value);


        mRecyclerViewMedicationSearchList = (RecyclerView) findViewById(R.id.rv_MedicationSearch);
        // use this setting to improve performance if you know that changes
        // in content do not change the layout size of the RecyclerView
        mRecyclerViewMedicationSearchList.setHasFixedSize(true);
        // use a linear layout manager
        mLayoutManagerMedicationList = new LinearLayoutManager(PHRMS_MedicationList_Fragment.this);
        mRecyclerViewMedicationSearchList.setLayoutManager(mLayoutManagerMedicationList);
        mRecyclerViewMedicationSearchList.setItemAnimator(new DefaultItemAnimator());
        mRecyclerViewMedicationSearchList.addItemDecoration(new RecycleView_DividerItemDecoration(PHRMS_MedicationList_Fragment.this, LinearLayoutManager.VERTICAL));

        Functions.progressbarStyle(mProgressBarMedicationSearch, PHRMS_MedicationList_Fragment.this);

        String userid = Functions.decrypt(PHRMS_MedicationList_Fragment.this, Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(PHRMS_MedicationList_Fragment.this);
        } else {

            url = getString(R.string.urlLogin) + getString(R.string.GetMedicationTypes);

            edMedicationSearch_value.addTextChangedListener(new TextWatcher() {
                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {
                    // TODO Auto-generated method stub

                }

                @Override
                public void beforeTextChanged(CharSequence s, int start, int count,
                                              int after) {
                    // TODO Auto-generated method stub

                }

                @Override
                public void afterTextChanged(Editable editautocomplete) {
                    // TODO Auto-generated method stub

                    if (url != null) {

                        if (editautocomplete.toString().trim().length() < 3)
                        {
                            mProgressBarMedicationSearch.setVisibility(View.GONE);
                            mRecyclerViewMedicationSearchList.setVisibility(View.GONE);
                            Functions.showToast(PHRMS_MedicationList_Fragment.this, "Search Medication with atleast 3 or more characters");
                        }
                        else
                         {
                            if (Functions.isNetworkAvailable(PHRMS_MedicationList_Fragment.this)) {
                                /* Starting Download Service */
                                mReceiver = new DownloadResultReceiver(new Handler());
                                mReceiver.setReceiver(PHRMS_MedicationList_Fragment.this);
                                Intent intent = new Intent(Intent.ACTION_SYNC, null, PHRMS_MedicationList_Fragment.this, Data_Service_MedicationList.class);

                                /* Send optional extras to Download IntentService */
                                intent.putExtra("url", url);
                                intent.putExtra("searchTerm", editautocomplete.toString());
                                intent.putExtra("receiver", mReceiver);
                                intent.putExtra("requestId", 101);

                                startService(intent);
                            } else {
                                Functions.showSnackbar(parentLayout, "Internet Not Available !!", "Action");
                            }
                        }
                    }

                }
            });


            // Added Custom RecyclerTouchListener
            mRecyclerViewMedicationSearchList.addOnItemTouchListener(new RecyclerTouchListener(PHRMS_MedicationList_Fragment.this, mRecyclerViewMedicationSearchList, new ClickListener() {
                @Override
                public void onClick(View view, int position) {
                    //if (mMedicationList.size() > 0)
                    if (ClinicalTermsList != null && ClinicalTermsList.size() > 0) {
                        //ClinicalTerms ct = (ClinicalTerms) mMedicationList.get(position);
                        ClinicalTerms ct = (ClinicalTerms) ClinicalTermsList.get(position);

                        Intent intAddMedication = new Intent(PHRMS_MedicationList_Fragment.this, PHRMS_Medication_Fragment_Add.class);
                        intAddMedication.putExtra("Medication", 1);
                        intAddMedication.putExtra("MedicationID", ct.getId());
                        intAddMedication.putExtra("MedicationName", ct.getName());
                        setResult(RESULT_OK, intAddMedication);
                        finish();
                    }
                }

                @Override
                public void onLongClick(View view, int position) {
                }
            }));
        }

    }


    @Override
    public void onReceiveResult(int resultCode, Bundle resultData) {
        switch (resultCode) {
            case Data_Service_MedicationList.STATUS_RUNNING:
                mProgressBarMedicationSearch.setVisibility(View.VISIBLE);

                break;
            case Data_Service_MedicationList.STATUS_FINISHED:


                mProgressBarMedicationSearch.setVisibility(View.GONE);

                ClinicalTermsList = resultData.getStringArrayList("CT_RESULT");

                if (null != ClinicalTermsList && ClinicalTermsList.size() > 0) {

                    mRecyclerViewMedicationSearchList.setVisibility(View.VISIBLE);

                    MedicationDataList m_MedicationDataList = new MedicationDataList();
                    mRecyclerViewMedicationSearchList.setAdapter(m_MedicationDataList);

                } else {
                    mRecyclerViewMedicationSearchList.setVisibility(View.GONE);
                    Functions.showToast(PHRMS_MedicationList_Fragment.this, "No result found");

                }

                break;
            case Data_Service_MedicationList.STATUS_ERROR:
                /* Handle the error */
                String error = resultData.getString(Intent.EXTRA_TEXT);

                mRecyclerViewMedicationSearchList.setVisibility(View.GONE);
                mProgressBarMedicationSearch.setVisibility(View.GONE);

                Functions.showToast(PHRMS_MedicationList_Fragment.this, error); //, Toast.LENGTH_LONG).show();
                break;
        }
    }


    //extends Recyler adapter MedicationDataList and ViewHolder to hold data
    public class MedicationDataList extends RecyclerView.Adapter<MedicationDataList.ViewHolder> {



        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_MedicationName, mTextView_MedicationId;

            //Load Recyler adapter MedicationDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_MedicationName = (TextView) v.findViewById(R.id.txttype);
                mTextView_MedicationId = (TextView) v.findViewById(R.id.txtid);
            }
        }


        public MedicationDataList() {
        }

        // Create new views (invoked by the layout manager)
        @Override
        public MedicationDataList.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
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

            ClinicalTerms ct = (ClinicalTerms) ClinicalTermsList.get(position);
            holder.mTextView_MedicationId.setText(ct.getId());
            holder.mTextView_MedicationName.setText(ct.getName());
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {

            return (null != ClinicalTermsList ? ClinicalTermsList.size() : 0);

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
        private PHRMS_MedicationList_Fragment.ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final PHRMS_MedicationList_Fragment.ClickListener clickListener) {
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

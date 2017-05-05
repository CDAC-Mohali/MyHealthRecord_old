
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

import com.google.firebase.appindexing.Action;
import com.google.firebase.appindexing.FirebaseUserActions;
import com.google.firebase.appindexing.builders.Actions;
import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.clinicaladapters.ClinicalTerms;
import com.pkg.healthrecordappname.appfinalname.modules.httpconnections.HttpUrlConnectionRequest;
import com.pkg.healthrecordappname.appfinalname.modules.useables.Functions;
import com.pkg.healthrecordappname.appfinalname.modules.useables.RecycleView_DividerItemDecoration;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;


public class PHRMS_AllergiesList_Fragment extends AppCompatActivity {
    String url = null;
    private ArrayList mAllergyList;
    private DownloadAllergyTypes mAllergyListTask = null;
    private RecyclerView mRecyclerViewAllergySearchList = null;
    private RecyclerView.LayoutManager mLayoutManagerAllergyList;

    private ProgressBar mProgressBarAllergySearch;
    private TextInputLayout input_allergySearch_value;
    private EditText edallergySearch_value;
    private View parentLayout;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.frame_allergies_list);

        parentLayout = findViewById(R.id.lv_Search);

        //toolbar
        Toolbar mtoolbar_add_allergy = (Toolbar) findViewById(R.id.toolbar_searchallergy);
        if (mtoolbar_add_allergy != null) {
            setSupportActionBar(mtoolbar_add_allergy);
        }

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        mProgressBarAllergySearch = (ProgressBar) findViewById(R.id.ProgressBarAllergySearch);


        input_allergySearch_value = (TextInputLayout) findViewById(R.id.input_allergySearch_value);
        edallergySearch_value = (EditText) findViewById(R.id.edallergySearch_value);


        mRecyclerViewAllergySearchList = (RecyclerView) findViewById(R.id.rv_allergySearch);
        mRecyclerViewAllergySearchList.setHasFixedSize(true);
        // use a linear layout manager
        mLayoutManagerAllergyList = new LinearLayoutManager(PHRMS_AllergiesList_Fragment.this);
        mRecyclerViewAllergySearchList.setLayoutManager(mLayoutManagerAllergyList);
        mRecyclerViewAllergySearchList.setItemAnimator(new DefaultItemAnimator());
        mRecyclerViewAllergySearchList.addItemDecoration(new RecycleView_DividerItemDecoration(PHRMS_AllergiesList_Fragment.this, LinearLayoutManager.VERTICAL));


        Functions.progressbarStyle(mProgressBarAllergySearch, PHRMS_AllergiesList_Fragment.this);

        String userid = Functions.decrypt(PHRMS_AllergiesList_Fragment.this, Functions.pref.getString(Functions.P_UsrID, null));
        if (Functions.isNullOrEmpty(userid)) {
            Functions.mainscreen(PHRMS_AllergiesList_Fragment.this);
        } else {
            url = getString(R.string.urlLogin) + getString(R.string.GetAllergyTypes);

            edallergySearch_value.addTextChangedListener(new TextWatcher() {
                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {
                    // TODO Auto-generated method stub
                    if (url != null) {
                        if (s.toString().trim().length() < 1)
                        {
                            mRecyclerViewAllergySearchList.setVisibility(View.GONE);
                            Functions.showToast(PHRMS_AllergiesList_Fragment.this, "Search Allergy with one or more characters");
                        }
                        else
                            {
                            if (Functions.isNetworkAvailable(PHRMS_AllergiesList_Fragment.this))
                            {
                                mAllergyListTask = new DownloadAllergyTypes();
                                mAllergyListTask.execute(s.toString());
                            }
                            else
                             {
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
            mRecyclerViewAllergySearchList.addOnItemTouchListener(new RecyclerTouchListener(PHRMS_AllergiesList_Fragment.this, mRecyclerViewAllergySearchList, new ClickListener() {
                @Override
                public void onClick(View view, int position) {
                    if (mAllergyList.size() > 0) {
                        ClinicalTerms ct = (ClinicalTerms) mAllergyList.get(position);

                        Intent intAddAllergy = new Intent(PHRMS_AllergiesList_Fragment.this, PHRMS_Allergies_Fragment_Add.class);
                        intAddAllergy.putExtra("Allergy", 1);
                        intAddAllergy.putExtra("AllergyID", ct.getId());
                        intAddAllergy.putExtra("AllergyName", ct.getName());
                        setResult(RESULT_OK, intAddAllergy);
                        finish();


                    }
                }

                @Override
                public void onLongClick(View view, int position) {
                }
            }));
        }

    }

    /**
     * ATTENTION: This was auto-generated to implement the App Indexing API.
     * See https://g.co/AppIndexing/AndroidStudio for more information.
     */
    public Action getIndexApiAction() {
        return Actions.newView("PHRMS_AllergiesList_Fragment", "http://[ENTER-YOUR-URL-HERE]");
    }

    @Override
    public void onStart() {
        super.onStart();


        FirebaseUserActions.getInstance().start(getIndexApiAction());
    }

    @Override
    public void onStop() {


        FirebaseUserActions.getInstance().end(getIndexApiAction());
        super.onStop();
    }

    protected class DownloadAllergyTypes extends AsyncTask<String, String, ArrayList> {

        @Override
        protected void onPreExecute() {
            mProgressBarAllergySearch.setVisibility(View.VISIBLE);
        }

        @Override
        protected ArrayList doInBackground(String... params) {
            try {
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
                            ClinicalTerms.setName(jo.getString("AllergyName"));
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
                mRecyclerViewAllergySearchList.setVisibility(View.VISIBLE);
                // Load result arraylist in recycleview
                mAllergyList = resultlist;
                AlleryDataList m_alleryDataList = new AlleryDataList(mAllergyList);
                mRecyclerViewAllergySearchList.setAdapter(m_alleryDataList);

            }
            else
             {
                mRecyclerViewAllergySearchList.setVisibility(View.GONE);
                Functions.showToast(PHRMS_AllergiesList_Fragment.this, "No result found");
            }

            mProgressBarAllergySearch.setVisibility(View.GONE);
        }

    }


    //extends Recyler adapter AlleryDataList and ViewHolder to hold data
    public class AlleryDataList extends RecyclerView.Adapter<AlleryDataList.ViewHolder> {

        private ArrayList al_allergy;

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ViewHolder extends RecyclerView.ViewHolder {
            // each data item is just a string in this case
            public TextView mTextView_AllergyName, mTextView_AllergyId;

            //Load Recyler adapter AlleryDataList and ViewHolder to hold data
            public ViewHolder(View v) {
                super(v);
                mTextView_AllergyName = (TextView) v.findViewById(R.id.txttype);
                mTextView_AllergyId = (TextView) v.findViewById(R.id.txtid);
            }
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public AlleryDataList(ArrayList ar) {
            this.al_allergy = ar;
        }

        // Create new views (invoked by the layout manager)
        @Override
        public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
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
            ClinicalTerms ct = (ClinicalTerms) al_allergy.get(position);
            holder.mTextView_AllergyId.setText(ct.getId());
            holder.mTextView_AllergyName.setText(ct.getName());
        }

        // Return the size of your dataset (invoked by the layout manager)
        @Override
        public int getItemCount() {
            return (null != al_allergy ? al_allergy.size() : 0);
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
        private ClickListener clickListener;

        public RecyclerTouchListener(Context context, final RecyclerView recyclerView, final ClickListener clickListener) {
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

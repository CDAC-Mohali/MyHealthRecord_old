package com.pkg.healthrecordappname.appfinalname.modules.clinicaladapters;


import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Filter;
import android.widget.Filterable;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.pkg.healthrecordappname.appfinalname.R;
import com.pkg.healthrecordappname.appfinalname.modules.httpconnections.HttpUrlConnectionRequest;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;


public class AutocompleteAdapterAllergy extends ArrayAdapter implements Filterable {
    private ArrayList mAllergyList;
    private String ClinicalTerms_URL;
    private ProgressBar pbar;


    public AutocompleteAdapterAllergy(Context context, int resource, String url, ProgressBar progressBar) {
        super(context, resource);
        mAllergyList = new ArrayList<>();
        ClinicalTerms_URL = url;
        pbar = progressBar;
    }

    @Override
    public int getCount() {
        return mAllergyList.size();
    }

    @Override
    public ClinicalTerms getItem(int position) {
        return (ClinicalTerms) mAllergyList.get(position);
    }

    @Override
    public Filter getFilter() {
        Filter myFilter = new Filter() {
            @Override
            protected FilterResults performFiltering(CharSequence constraint) {
                FilterResults filterResults = new FilterResults();
                if (constraint != null) {
                    try {
                        //get data from the web
                        String term = constraint.toString();
                        pbar.setVisibility(View.VISIBLE);
                        mAllergyList = new DownloadAllergyTypes().execute(term).get();
                    } catch (Exception e) {
                        Log.d("HUS", "EXCEPTION " + e);
                    }
                    filterResults.values = mAllergyList;
                    filterResults.count = mAllergyList.size();
                }
                return filterResults;
            }

            @Override
            protected void publishResults(CharSequence constraint, FilterResults results) {
                if (results != null && results.count > 0) {
                    notifyDataSetChanged();
                } else {
                    notifyDataSetInvalidated();
                }
            }
        };

        return myFilter;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        LayoutInflater inflater = LayoutInflater.from(getContext());
        View view = inflater.inflate(R.layout.autocomplete, parent, false);

        //get ClinicalTerms
        ClinicalTerms ct = (ClinicalTerms) mAllergyList.get(position);

        TextView ClinicalTermsID = (TextView) view.findViewById(R.id.txtid);

        TextView ClinicalTermsName = (TextView) view.findViewById(R.id.txttype);

        ClinicalTermsID.setText(ct.getId());
        ClinicalTermsName.setText(ct.getName());

        return view;
    }


    protected class DownloadAllergyTypes extends AsyncTask<String, String, ArrayList> {

        @Override
        protected void onPreExecute() {
            pbar.setVisibility(View.VISIBLE);
        }

        @Override
        protected ArrayList doInBackground(String... params) {
            try {

                String jsonString = HttpUrlConnectionRequest.sendPost(ClinicalTerms_URL, params[0].toString());

                ArrayList ClinicalTermsList = new ArrayList<>();

                if (jsonString != null) {
                    //JSONObject
                    JSONArray jsonArray = new JSONArray(jsonString);
                    for (int i = 0; i < jsonArray.length(); i++) {
                        JSONObject jo = jsonArray.getJSONObject(i);
                        //store the ClinicalTerms name
                        ClinicalTerms ClinicalTerms = new ClinicalTerms();
                        ClinicalTerms.setId(jo.getString("Id"));
                        ClinicalTerms.setName(jo.getString("AllergyName"));
                        ClinicalTermsList.add(ClinicalTerms);
                    }
                }

                //return the ClinicalTermsList
                return ClinicalTermsList;

            } catch (Exception e) {
                Log.d("HUS", "EXCEPTION " + e);
                return null;
            }
        }

        @Override
        protected void onPostExecute(ArrayList resultlist) {
            if (resultlist.size() > 0) {
                pbar.setVisibility(View.INVISIBLE);
            }
        }

    }
}

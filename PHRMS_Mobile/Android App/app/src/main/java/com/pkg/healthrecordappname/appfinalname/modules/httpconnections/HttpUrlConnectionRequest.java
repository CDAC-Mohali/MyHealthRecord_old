package com.pkg.healthrecordappname.appfinalname.modules.httpconnections;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.URL;

import javax.net.ssl.HttpsURLConnection;


public class HttpUrlConnectionRequest {


    public static JSONObject SendJsonHttpUrlGetRequest(String uri) throws IOException, JSONException {

        URL url = new URL(uri);
        HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();

        /* optional request header */
        urlConnection.setRequestProperty("Content-Type", "application/json");

        /* optional request header */
        urlConnection.setRequestProperty("Accept", "application/json");

        /* for Get request */
        urlConnection.setRequestMethod("GET");
        //urlConnection.setDoOutput(true);
        urlConnection.connect();

        int statusCode = urlConnection.getResponseCode();

        String response = null;
        /* 200 represents HTTP OK */
        if (statusCode == 200) {


            StringBuilder sb = new StringBuilder();

            InputStream is = urlConnection.getInputStream();
            BufferedReader br = new BufferedReader(new InputStreamReader(is));

            String line = null;
            if (is != null) {
                while ((line = br.readLine()) != null) {
                    sb.append(line);
                }

                response = sb.toString();
            }
            is.close();
            urlConnection.disconnect();
        } else {
            //result = 0; //"Failed to fetch data!";
        }

        return response != null ? new JSONObject(response.toString()) : null;
    }

    // HTTP POST request
    public static String sendPost(String url, String SearchTerm) throws Exception {
        URL url_obj = new URL(url);


        HttpURLConnection con = (HttpURLConnection) url_obj.openConnection();

        con.setReadTimeout(15000 /* milliseconds */);
        con.setConnectTimeout(15000 /* milliseconds */);

        //add request header
        con.setRequestMethod("POST");
        con.setRequestProperty("Accept", "application/json");
        con.setRequestProperty("Content-Type", "application/json");

        // Send post request
        con.setDoInput(true);
        con.setDoOutput(true);
        con.connect();


        OutputStream os = con.getOutputStream();
        BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(os, "UTF-8"));
        String sendstr = '\"' + SearchTerm + '\"';
        writer.write(sendstr); //getPostDataString(postDataParams)
        writer.flush();
        writer.close();
        os.close();
        os.flush();


        String response = null;

        int responseCode = con.getResponseCode();

        if (responseCode == HttpsURLConnection.HTTP_OK) {
            StringBuilder sb = new StringBuilder();

            InputStream is = con.getInputStream();
            BufferedReader br = new BufferedReader(new InputStreamReader(is));

            String line = null;
            if (is != null) {
                while ((line = br.readLine()) != null) {
                    sb.append(line);
                }
                response = sb.toString();
            }
            is.close();
            con.disconnect();

        }


        return response;
    }


}
package com.merida.diego.project1;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;

import com.thomas.soap.SurveyBoomServiceSoap;
import com.thomas.soap.SurveyTransport;

import java.util.ArrayList;

public class Takeit extends Activity {

    private EditText editTxt;
    private Button btn;
    private ListView list;
    private ArrayAdapter<String> adapter;
    private ArrayList<String> arrayList;



    private class GetSurveyTask extends AsyncTask<String, Integer, Boolean> {

        Takeit parent_;

        GetSurveyTask(Takeit parent)
        {
            parent_ = parent;
        }

        @Override
        protected Boolean doInBackground(String... params) {

            SurveyBoomServiceSoap sm = new SurveyBoomServiceSoap();

            SurveyTransport st = null;
            try
            {
                st = sm.GetSurvey(Integer.parseInt(params[0]));

            }
            catch (Exception e)
            {

            }

            System.out.println(st.Description);

            return true;
        }


        protected void onPostExecute(Boolean result) {

            if(result) {

                //startActivity(new Intent(this, Soap.class));
                startActivity(new Intent(parent_, MainPanel.class));
            }
        }
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_takeit);

        //GetSurvey ID from previous activity
        Intent intent = getIntent();
        String survey_id = intent.getStringExtra("survey_id");

        GetSurveyTask tsk = new GetSurveyTask(this);
        tsk.execute(survey_id);

        editTxt = (EditText) findViewById(R.id.editText);
        btn = (Button) findViewById(R.id.button);
        list = (ListView) findViewById(R.id.listView);
        arrayList = new ArrayList<String>();

        // Adapter: You need three parameters 'the context, id of the layout (it will be where the data is shown),
        // and the array that contains the data
        adapter = new ArrayAdapter<String>(getApplicationContext(), android.R.layout.simple_spinner_item, arrayList);

        // Here, you set the data in your ListView
        list.setAdapter(adapter);

        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                // this line adds the data of your EditText and puts in your array
                arrayList.add(editTxt.getText().toString());
                // next thing you have to do is check if your adapter has changed
                adapter.notifyDataSetChanged();
            }
        });
    }
}
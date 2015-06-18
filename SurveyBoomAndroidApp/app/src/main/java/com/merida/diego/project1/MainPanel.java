package com.merida.diego.project1;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

public class MainPanel extends  Activity implements View.OnClickListener {

    Button btn_TakeSurvey;
    Button btn_ViewSurvey;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main_panel);

        btn_TakeSurvey = (Button) findViewById(R.id.btn_TakeSurvey);
        btn_ViewSurvey = (Button) findViewById(R.id.btn_ViewSurvey);

        btn_TakeSurvey.setOnClickListener(this);
        btn_ViewSurvey.setOnClickListener(this);
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main_panel, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        if (id == R.id.action_settings) {
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    public void takeSurvey(View view)
    {

        Intent intent = new Intent(this, Takeit.class);
        EditText editText = (EditText)findViewById(R.id.et_TakeSurvey);
        String message = editText.getText().toString();

        if(message.isEmpty())
            return;

        intent.putExtra("survey_id", message);
        startActivity(intent);
    }

    @Override
    public void onClick(View v) {

        switch (v.getId()) {


            case R.id.btn_TakeSurvey:
                Intent intent = new Intent(this, Takeit.class);
                EditText editText = (EditText)findViewById(R.id.et_TakeSurvey);
                String message = editText.getText().toString();

                if(message.isEmpty())
                    return;

                intent.putExtra("survey_id", message);
                startActivity(intent);

                break;


            case R.id.btn_ViewSurvey:





        }
    }
}
package com.merida.diego.project1;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;


public class Choose extends Activity implements View.OnClickListener {

    Button button1;
    Button button2;
    Button button3;
    Button button4;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_choose);
        button1 = (Button) findViewById(R.id.button1);
        button1.setOnClickListener(this);
        button2 = (Button) findViewById(R.id.button2);
        button2.setOnClickListener(this);
        button3 = (Button) findViewById(R.id.button3);
        button3.setOnClickListener(this);
        button4 = (Button) findViewById(R.id.button4);
        button4.setOnClickListener(this);

    }



    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onClick(View v) {

        switch(v.getId()){
            case R.id.button1:
                startActivity(new Intent(this,True_False.class));
                break;
        }
        switch(v.getId()){
            case R.id.button2:
                startActivity(new Intent(this,MultipleChoice.class));
                break;
        }
        switch(v.getId()){
            case R.id.button3:
                startActivity(new Intent(this,Text_Entry.class));
                break;
        }
        switch(v.getId()){
            case R.id.button4:
               // startActivity(new Intent(this,Question_Type.class));
                break;
        }


    }
}

package com.merida.diego.project1;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;


public class MyActivity extends Activity implements View.OnClickListener {




    Button bLogout;
    EditText etName,etAge, etUserName ;
    //UserLocalStore userLocalStore;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_my);

        etName =(EditText) findViewById(R.id.etName);
        etAge =(EditText) findViewById(R.id.etAge);
        etUserName =(EditText) findViewById(R.id.etUsername);

        bLogout =(Button) findViewById(R.id.bLogout);
        bLogout.setOnClickListener(this);

        //userLocalStore = new UserLocalStore(this);


    }

    @Override
    public void onClick(View v) {

        switch(v.getId()){

            case R.id.bLogout:

                //userLocalStore.clearUserData();
                //userLocalStore.serUserLoggedIn(false);


                startActivity(new Intent(this,Login.class));

                break;


        }




    }
}
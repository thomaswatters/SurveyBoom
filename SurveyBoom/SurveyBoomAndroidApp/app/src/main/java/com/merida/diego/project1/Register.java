package com.merida.diego.project1;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

public class Register extends Activity implements View.OnClickListener{



    Button bRegister;
    EditText etName,etAge, etUserName, etPassword, etConfirmPassword;
    TextView lblNotification;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);


        etName =(EditText) findViewById(R.id.etName);
        etAge =(EditText) findViewById(R.id.etAge);
        etUserName =(EditText) findViewById(R.id.etUsername);
        etPassword =(EditText) findViewById(R.id.etPassword);
        etConfirmPassword = (EditText) findViewById(R.id.etConfirmPassword);
        lblNotification = (TextView)findViewById(R.id.lblNotification);
        bRegister =(Button) findViewById(R.id.bRegister);
        bRegister.setOnClickListener(this);


    }

    @Override
    public void onClick(View v) {


        switch(v.getId()){


            case R.id.bRegister:
                if(!etConfirmPassword.getText().toString().equals( etPassword.getText().toString())) {
                    lblNotification.setText("Passwords do not match");
                }
                else {
                    CreateUserTask lt = new CreateUserTask(this);
                    lt.execute(etUserName.getText().toString(), etPassword.getText().toString());
                }
                break;

        }


    }


    private class CreateUserTask extends AsyncTask<String, Integer, Boolean> {

        Register parent_;

        CreateUserTask(Register parent)
        {
            parent_ = parent;
        }

        @Override
        protected Boolean doInBackground(String... params) {
            ServiceManager sm = new ServiceManager();

            return sm.CreateUser(params[0], params[1]);
        }


        protected void onPostExecute(Boolean result)
        {
            if(result)
            {
                startActivity(new Intent(parent_, Login.class));
            }
            else
            {
                lblNotification.setText("User already exists");
                try {
                    wait(5000);
                }
                catch(Exception e)
                {

                }
                startActivity(new Intent(parent_, Login.class));
            }

        }
    }
}


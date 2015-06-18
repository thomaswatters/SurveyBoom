package com.merida.diego.project1;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.thomas.soap.SurveyBoomServiceSoap;


public class Login extends Activity implements View.OnClickListener {


    Button bLogin;
    EditText etUsername, etPassword;
    TextView tvRegisterLink;
    TextView lblInvalidLogin;

    private class UserLoginTask extends AsyncTask<String, Integer, Boolean> {

        Login parent_;

        SurveyBoomServiceSoap service;
        String username;
        UserLoginTask(Login parent)
        {
            parent_ = parent;
            service = new SurveyBoomServiceSoap();
        }

        @Override
        protected Boolean doInBackground(String... params) {

            username = params[0];

            Boolean res = false;

            try
            {
                res = service.UserLogin(params[0], params[1]);
            }
            catch (Exception e)
            {

            }
            return res;
        }


        protected void onPostExecute(Boolean result) {

            if(result) {
//
//                try {
//                }
//                catch (Exception e)
//                {
//
//                }
                //startActivity(new Intent(this, Soap.class));
                startActivity(new Intent(parent_, MainPanel.class));
            }
            else {
                lblInvalidLogin.setText("Invalid Login Information");

            }


        }
    }


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        //look for  the id in  content view, find the view that the id, will cast it and then set it
        //to the variable


        etUsername =(EditText) findViewById(R.id.etUsername);
        etPassword =(EditText) findViewById(R.id.etPassword);
        lblInvalidLogin = (TextView) findViewById(R.id.lblInvalidLogin);
        tvRegisterLink =(TextView) findViewById(R.id.tvRegisterLink);
        bLogin =(Button) findViewById(R.id.bLogin);



        bLogin.setOnClickListener(this);
        tvRegisterLink.setOnClickListener(this);







    }

    @Override
    public void onClick(View v) {


        switch(v.getId()){


            case R.id.bLogin:
                UserLoginTask lt = new UserLoginTask(this);
                lt.execute(etUsername.getText().toString(), etPassword.getText().toString());
                break;



            case R.id.tvRegisterLink:
                startActivity(new Intent(this, Register.class));


        }




    }
}

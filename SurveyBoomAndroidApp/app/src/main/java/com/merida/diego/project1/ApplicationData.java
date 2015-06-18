package com.merida.diego.project1;

import android.app.Application;

/**
 * Created by Thomas on 6/17/2015.
 */
public class ApplicationData extends Application {
    private String username;

    private int UserID;

    public int getUserID() {
        return UserID;
    }

    public void setUserID(int userID) {
        UserID = userID;
    }



    public String getUsername(){return  username;}
    public void setUsername(String name){this.username = name;}
}

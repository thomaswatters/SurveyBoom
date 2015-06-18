package com.thomas.soap;

//----------------------------------------------------
//
// Generated by www.easywsdl.com
// Version: 4.1.7.0
//
// Created by Quasar Development at 18-06-2015
//
//---------------------------------------------------


import java.util.Hashtable;
import org.ksoap2.serialization.*;

public class SubmitSurveyResponse extends AttributeContainer implements KvmSerializable,java.io.Serializable
{
    
    public SurveyTransport st;
    
    public Integer survey_id=0;

    public SubmitSurveyResponse ()
    {
    }

    public SubmitSurveyResponse (Object paramObj,ExtendedSoapSerializationEnvelope __envelope)
    {
	    
	    if (paramObj == null)
            return;
        AttributeContainer inObj=(AttributeContainer)paramObj;


        SoapObject soapObject=(SoapObject)inObj;  
        if (soapObject.hasProperty("st"))
        {	
	        Object j = soapObject.getProperty("st");
	        this.st = (SurveyTransport)__envelope.get(j,SurveyTransport.class);
        }
        if (soapObject.hasProperty("survey_id"))
        {	
	        Object obj = soapObject.getProperty("survey_id");
            if (obj != null && obj.getClass().equals(SoapPrimitive.class))
            {
                SoapPrimitive j =(SoapPrimitive) obj;
                if(j.toString()!=null)
                {
                    this.survey_id = Integer.parseInt(j.toString());
                }
	        }
	        else if (obj!= null && obj instanceof Integer){
                this.survey_id = (Integer)obj;
            }    
        }


    }

    @Override
    public Object getProperty(int propertyIndex) {
        //!!!!! If you have a compilation error here then you are using old version of ksoap2 library. Please upgrade to the latest version.
        //!!!!! You can find a correct version in Lib folder from generated zip file!!!!!
        if(propertyIndex==0)
        {
            return this.st!=null?this.st:SoapPrimitive.NullSkip;
        }
        if(propertyIndex==1)
        {
            return survey_id;
        }
        return null;
    }


    @Override
    public int getPropertyCount() {
        return 2;
    }

    @Override
    public void getPropertyInfo(int propertyIndex, @SuppressWarnings("rawtypes") Hashtable arg1, PropertyInfo info)
    {
        if(propertyIndex==0)
        {
            info.type = SurveyTransport.class;
            info.name = "st";
            info.namespace= "http://surveyboomservice.azurewebsites.net/";
        }
        if(propertyIndex==1)
        {
            info.type = PropertyInfo.INTEGER_CLASS;
            info.name = "survey_id";
            info.namespace= "http://surveyboomservice.azurewebsites.net/";
        }
    }
    
    @Override
    public void setProperty(int arg0, Object arg1)
    {
    }

    @Override
    public String getInnerText() {
        return null;
    }

    @Override
    public void setInnerText(String s) {

    }
}
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

public class GetAllResponsesResponse extends AttributeContainer implements KvmSerializable,java.io.Serializable
{
    
    public ArrayOfQuestionTransport GetAllResponsesResult=new ArrayOfQuestionTransport();

    public GetAllResponsesResponse ()
    {
    }

    public GetAllResponsesResponse (Object paramObj,ExtendedSoapSerializationEnvelope __envelope)
    {
	    
	    if (paramObj == null)
            return;
        AttributeContainer inObj=(AttributeContainer)paramObj;


        SoapObject soapObject=(SoapObject)inObj;  
        if (soapObject.hasProperty("GetAllResponsesResult"))
        {	
	        Object j = soapObject.getProperty("GetAllResponsesResult");
	        this.GetAllResponsesResult = new ArrayOfQuestionTransport(j,__envelope);
        }


    }

    @Override
    public Object getProperty(int propertyIndex) {
        //!!!!! If you have a compilation error here then you are using old version of ksoap2 library. Please upgrade to the latest version.
        //!!!!! You can find a correct version in Lib folder from generated zip file!!!!!
        if(propertyIndex==0)
        {
            return this.GetAllResponsesResult!=null?this.GetAllResponsesResult:SoapPrimitive.NullSkip;
        }
        return null;
    }


    @Override
    public int getPropertyCount() {
        return 1;
    }

    @Override
    public void getPropertyInfo(int propertyIndex, @SuppressWarnings("rawtypes") Hashtable arg1, PropertyInfo info)
    {
        if(propertyIndex==0)
        {
            info.type = PropertyInfo.VECTOR_CLASS;
            info.name = "GetAllResponsesResult";
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

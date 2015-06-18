package com.merida.diego.project1;

import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;

import java.io.IOException;
import java.net.Proxy;
import java.net.SocketTimeoutException;




public class ServiceManager
{


    private static final String NAMESPACE = "http://surveyboomservice.azurewebsites.net/";
    private static final String MAIN_REQUEST_URL = "http://surveyboomservice.azurewebsites.net/SurveyBoomService.asmx";
    private static String SOAP_ACTION;
//    private final String METHOD_NAME = "UserLogin";



    private final SoapSerializationEnvelope getSoapSerializationEnvelope(SoapObject request) {
        SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
        envelope.dotNet = true;
//        envelope.implicitTypes = true;
//        envelope.setAddAdornments(false);
        envelope.setOutputSoapObject(request);


        return envelope;
    }

    private final HttpTransportSE getHttpTransportSE() {
        HttpTransportSE ht = new HttpTransportSE(Proxy.NO_PROXY,MAIN_REQUEST_URL,60000);
        ht.debug = true;
        ht.setXmlVersionTag("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        return ht;
    }

    public boolean UserLogin(String username, String password) {

        String data = null;

        String methodName = "UserLogin";

        SoapObject request = new SoapObject(NAMESPACE, methodName);
        request.addProperty("username", username);
        request.addProperty("password", password);

        System.out.println("SOAP oBJECT created ");

        SoapSerializationEnvelope envelope = getSoapSerializationEnvelope(request);


        HttpTransportSE ht = getHttpTransportSE();


        try

        {
            SOAP_ACTION = NAMESPACE + methodName;
            ht.call(SOAP_ACTION, envelope);
            System.out.println("test");

            data = envelope.getResponse().toString();

        }

        catch (SocketTimeoutException t) {
            t.printStackTrace();
        } catch (IOException i) {
            i.printStackTrace();
        } catch (Exception q) {
            q.printStackTrace();
        }

        return Boolean.parseBoolean(data);

    }

    public boolean CreateUser(String username, String password) {

        String data = null;

        String methodName = "CreateUser";

        SoapObject request = new SoapObject(NAMESPACE, methodName);
        request.addProperty("username", username);
        request.addProperty("password", password);

        System.out.println("SOAP oBJECT created ");

        SoapSerializationEnvelope envelope = getSoapSerializationEnvelope(request);


        HttpTransportSE ht = getHttpTransportSE();


        try

        {
            SOAP_ACTION = NAMESPACE + methodName;
            ht.call(SOAP_ACTION, envelope);
        }

        catch (Exception e)
        {
            return false;
        }

        return true;

    }


}

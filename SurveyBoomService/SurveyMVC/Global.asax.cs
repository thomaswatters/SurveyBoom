using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SurveyMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //Error logging omitted

            var httpException = exception as HttpException;
            var routeData = new RouteData();
            IController errorController = new Controllers.ErrorController();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("area", "");
            routeData.Values.Add("ex", exception);

            if (httpException != null)
            {
                //this is a basic exampe of how you can choose to handle your errors based on http status codes.
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        Response.Clear();

                        // page not found
                        routeData.Values.Add("action", "PageNotFound");

                        Server.ClearError();
                        // Call the controller with the route
                        errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));

                        break;
                    case 500:
                        // server error
                        routeData.Values.Add("action", "ServerError");

                        Server.ClearError();
                        // Call the controller with the route
                        errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                        break;
                    case 403:
                        // server error
                        routeData.Values.Add("action", "UnauthorisedRequest");

                        Server.ClearError();
                        // Call the controller with the route
                        errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                        break;
                    //add cases for other http errors you want to handle, otherwise HTTP500 will be returned as the default.
                    default:
                        // server error
                        routeData.Values.Add("action", "ServerError");

                        Server.ClearError();
                        // Call the controller with the route
                        errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                        break;
                }
            }
            //All other exceptions should result in a 500 error as they are issues with unhandled exceptions in the code
            else
            {
                routeData.Values.Add("action", "ServerError");
                Server.ClearError();
                // Call the controller with the route
                errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            }
        }
    }
}

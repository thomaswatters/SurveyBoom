using System.Web;
using System.Web.Mvc;

namespace SurveyMVC.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View("Error");
        }

        public ActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View("Error");
        }

        public ActionResult UnauthorisedRequest()
        {
            Response.StatusCode = 403;
            return View("Error");
        }

        //Any other errors you want to specifically handle here.

        public ActionResult CatchAllUrls()
        {
            //throwing an exception here pushes the error through the Application_Error method for centralised handling/logging
            throw new HttpException(404, "The requested url was not found");
        }
    }
}
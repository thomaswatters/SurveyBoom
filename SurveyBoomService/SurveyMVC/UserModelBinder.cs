//using System;
//using System.Web;
//using System.Web.Mvc;
//using SurveyMVC.Models;
//
//namespace SurveyMVC
//{
//    public class UserModelBinder : DefaultModelBinder
//    {
//        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//        {
//            // fetch the job id from the request
//            var userId = controllerContext.RouteData.Values["id"].ToString();
//
//            // fetch the currently connected username
//            var user = controllerContext.HttpContext.User.Identity.Name;
//
//            // Remark: You might need an additional step here
//            // to query AD and fetch the email
//
//            // Given the job id and the currently connected user, try 
//            // to fetch the corresponding job
//            UserModel current;
//
//            var service = new net.azurewebsites.surveyboom.SurveyBoomService();
//
//            bool exists = service.HasAccount(short.Parse(userId));
//
//
//            if (!exists)
//            {
//                // We didn't find any job that corresponds to
//                // the currently connected user
//                // => we throw
//                throw new HttpException(403, "Forbidden");
//            }
//            return userId;//this is a lie
//        }
//
//
//    }
//}
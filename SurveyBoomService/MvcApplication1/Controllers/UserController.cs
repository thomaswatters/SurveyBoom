using System;
using System.Net;
using System.Web.Mvc;
using MvcApplication1.net.azurewebsites.surveyboomservice;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        // GET/POST: User/Details/5
        public ActionResult Details(/*[ModelBinder(typeof(UserModelBinder))]*/ int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //user service here
            var user = new UserModel
            {
                Key = id.GetValueOrDefault(),
                Username = User.Identity.Name
            };

            //TODO: Return a lists of all the surveys related to this user     

            return View(user);
        }

//        [Authorize]
//        // GET: User/CreateSurvey/5
//        public ActionResult CreateSurvey(/*[ModelBinder(typeof(UserModelBinder))]*/ int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var service = new SurveyBoomService();
//
//            //user service here
//            var user = new UserModel();
//
//            user.Key = id.GetValueOrDefault();
//            user.Username = "Some User";
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return View(user);
//        }

        [HttpPost]
        public ActionResult Index(string title)
        {
            var a = title;
            Console.WriteLine(a);

            //   ViewBag.Data = string.Join(",", title ?? new string[] { });
            return View();
        }

        // GET: User/Create
        [Authorize(Roles = "canEdit")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
        public ActionResult Create([Bind(Include = "Key,Username,Password")] UserModel user)
        {
            if (ModelState.IsValid)
            {
                var service = new SurveyBoomService();

                service.CreateUser(user.Username, user.Password);

                return RedirectToAction("Index");
            }

            return View(user);
        }

//        // GET: User/Edit/5
//        [Authorize(Roles = "canEdit")]
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            user user = _db.users.Find(id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return View(user);
//        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "id,username,password_hashed,email,admin,invite_datetime,last_login_datetime,inviter_user_id")] user user)
//        {
//            if (ModelState.IsValid)
//            {
//                _db.Entry(user).State = EntityState.Modified;
//                _db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return System.Web.UI.WebControls.View(user);
//        }

//        // GET: User/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            user user = _db.users.Find(id);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return System.Web.UI.WebControls.View(user);
//        }

//        // POST: User/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            user user = _db.users.Find(id);
//            _db.users.Remove(user);
//            _db.SaveChanges();
//            return RedirectToAction("Index");
//        }
//
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                _db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace SurveyBoomService
{
    /// <summary>
    /// Summary description for SurveyBoomService
    /// </summary>
    [WebService(Namespace = "http://surveyboom.azurewebsites.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SurveyBoomService : System.Web.Services.WebService
    {

        private string EncodePassword(string password)
        {
            Byte[] original_bytes;
            Byte[] encoded_bytes;
            MD5 md5 = new MD5CryptoServiceProvider();

            original_bytes = ASCIIEncoding.Default.GetBytes(password);
            encoded_bytes = md5.ComputeHash(original_bytes);

            return BitConverter.ToString(encoded_bytes);
        }

        private string EncodePassword(string username, string password)
        {
            return EncodePassword(username + password);

        }

        [WebMethod]
        public int GetUserID(string username)
        {
            using (var db = new SurveyBoomEntities())
            {
                var user = db.users.FirstOrDefault(i => i.username == username );

                if (user != null)
                    return user.id;

                throw new SoapException("No such user", SoapException.ClientFaultCode);
                    
            }
        }


        [WebMethod]
        public bool UserLogin(string username, string password)
        {

            using(var db = new SurveyBoomEntities())
            {
                password = EncodePassword(username, password);
                var user = db.users.FirstOrDefault(i => i.username == username && i.password == password);

                return user != null;
            }
        }

        [WebMethod]
        public bool HasAccount(int id)
        {
            using (var db = new SurveyBoomEntities())
            {
                var user = db.users.FirstOrDefault(i => i.id == id);

                return user != null;
            }
        }

        [WebMethod]
        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            using (var db = new SurveyBoomEntities())
            {
                string password = EncodePassword(username, oldPassword);
                var user = db.users.FirstOrDefault(i => i.username == username && i.password == password);

                if(user != null)
                {
                    password = EncodePassword(username, newPassword);
                    user.password = password;
                    if(db.SaveChanges() != 0)
                        return true;
                }

                return false;
            }
        }


        [WebMethod]
        public void CreateUser(string username, string password)
        {
            using (var db = new SurveyBoomEntities())
            {
                var user = db.users.FirstOrDefault(i => i.username == username);

                if (user != null)
                    throw new SoapException("User already exists", SoapException.ClientFaultCode);

                user newUser = new user();

                newUser.username = username;

                password = EncodePassword(username, password);
                newUser.password = password;

                db.users.Add(newUser);
                db.SaveChanges();
            }
        }

    }
}

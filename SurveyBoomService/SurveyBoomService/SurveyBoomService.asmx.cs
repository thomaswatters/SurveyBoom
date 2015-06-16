﻿using System;
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
    [WebService(Namespace = "http://surveyboomservice.azurewebsites.net/")]
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
        public bool CreateUser(string username, string password)
        {
            using (var db = new SurveyBoomEntities())
            {
                var user = db.users.FirstOrDefault(i => i.username == username);

                if (user != null)
                    return false;

                user newUser = new user();

                newUser.username = username;

                password = EncodePassword(username, password);
                newUser.password = password;

                db.users.Add(newUser);
                db.SaveChanges();

                return true;
            }
        }

        [WebMethod]
        public int CreateSurvey(SurveyTransport submittedSurvey)
        {
            using( var db =  new SurveyBoomEntities())
            {
                survey newSurvey = new survey();

                //Look for user to add database reference to
                user thisUser = db.users.FirstOrDefault(i => i.id == submittedSurvey.UserID);

                if (thisUser == null)
                    return -1;

                // Create the survey
                newSurvey.title = submittedSurvey.Title;
                newSurvey.user_id = submittedSurvey.UserID;
                newSurvey.description = submittedSurvey.Description;

                //Add the survey to the database
                db.surveys.Add(newSurvey);
                db.SaveChanges();


                //Get the ID generated by the database for the current survey
                int surveyID = newSurvey.id;


                // Add questions to the database
                foreach (var quest in submittedSurvey.Questions)
                {
                    question newQuestion = new question();
                    newQuestion.question_text = quest.QuestionText;
                    newQuestion.survey_id = surveyID;

                    switch(quest.Type)
                    {
                        case(QuestionTransport.QuestionType.TrueOrFalse):
                            newQuestion.input_type_id = db.input_type.FirstOrDefault(i => i.name == "TrueOrFalse").id;
                            break;
                        case(QuestionTransport.QuestionType.ShortAnswer):
                            newQuestion.input_type_id = db.input_type.FirstOrDefault(i => i.name == "ShortAnswer").id;
                            break;
                        case(QuestionTransport.QuestionType.MultipleChoice):
                            newQuestion.input_type_id = db.input_type.FirstOrDefault(i => i.name == "MultipleChoice").id;
                            break;

                    }

                    db.questions.Add(newQuestion);
                    db.SaveChanges();

                    int questionID = newQuestion.id;

                    foreach (var op in quest.Options)
                    {
                        option newOption = new option();
                        newOption.question_id = questionID;
                        newOption.text = op;

                        db.options.Add(newOption);
                        db.SaveChanges();

                    }
                }

                return surveyID;

            }

        }


        [WebMethod]
        public SurveyTransport GetSurvey(int key)
        {
            using ( var db = new SurveyBoomEntities())
            {
                var survey = db.surveys.FirstOrDefault(i => i.id == key);

                if (survey == null)
                    return null;

                SurveyTransport st = new SurveyTransport();
                st.SurveyID = survey.id;
                st.Title = survey.title;
                st.Description = survey.description;
                st.Questions = new List<QuestionTransport>();
                foreach (var item in survey.questions)
                {
                    QuestionTransport qt = new QuestionTransport();
                    qt.QuestionText = item.question_text;
                    qt.Type = (QuestionTransport.QuestionType)Enum.Parse(typeof(QuestionTransport.QuestionType), item.input_type.name);
                    qt.Options = new List<string>();
                    if (qt.Type != QuestionTransport.QuestionType.ShortAnswer)
                    {
                        foreach (var op in item.options)
                        {
                            qt.Options.Add(op.text);
                        }
                    }

                    st.Questions.Add(qt);
                }

                return st;
            }
        }


        [WebMethod]
        public bool SubmitSurveyResponse(SurveyTransport st, int survey_id)
        {
            using(var db = new SurveyBoomEntities())
            {
                var survey = db.surveys.FirstOrDefault(i => i.id == survey_id);

                if (survey == null)
                    return false;


                int user_id = st.UserID;
                foreach (var item in st.Questions)
                {
                    answer ans = new answer();
                    ans.user_id = user_id;
                    ans.question_id = survey.questions.FirstOrDefault(i => i.question_text == item.QuestionText).id;
                    switch(item.Type)
                    {
                        case QuestionTransport.QuestionType.MultipleChoice:
                        case QuestionTransport.QuestionType.TrueOrFalse:
                            ans.option_id = survey.questions.FirstOrDefault(i => i.id == ans.question_id).options.FirstOrDefault(i => i.text == item.ResponseString).id;
                            break;
                        case QuestionTransport.QuestionType.ShortAnswer:
                            ans.answer_text_field = item.ResponseString;
                            break;
                    }
                    db.answers.Add(ans);
                    
                }

                if (db.SaveChanges() > 0)
                    return true;

                return false;
            }

        }


        [WebMethod]
        public List<QuestionTransport> GetAllResponses(int survey_id)
        {
            using (var db = new SurveyBoomEntities())
            {

                List<QuestionTransport> qtList = new List<QuestionTransport>();
                foreach (var item in db.surveys.FirstOrDefault(i => i.id==survey_id).questions)
                {
                    QuestionTransport.QuestionType type = (QuestionTransport.QuestionType)Enum.Parse(typeof(QuestionTransport.QuestionType), item.input_type.name);

                    List<string> options = new List<string>();
                    if (type != QuestionTransport.QuestionType.ShortAnswer)
                    {
                        foreach (var op in item.options)
                        {
                            options.Add(op.text);
                        }
                    }


                    foreach (var ans in item.answers)
                    {
                        QuestionTransport qt = new QuestionTransport();
                        
                        qt.QuestionText = item.question_text;
                        qt.Type = type;
                        switch(type)
                        {
                            case QuestionTransport.QuestionType.TrueOrFalse:
                            case QuestionTransport.QuestionType.MultipleChoice:
                                qt.ResponseString = ans.option.text;
                                break;
                            case QuestionTransport.QuestionType.ShortAnswer:
                                qt.ResponseString = ans.answer_text_field;
                                break;

                        }
                        
                        qt.Options = options;
                        qtList.Add(qt);
                    }
                }

                return qtList;
            }


        }

        [WebMethod]
        public List<SurveyTransport> GetSurveysByUserID(int user_id)
        {
            using(var db = new SurveyBoomEntities())
            {
                List<SurveyTransport> surveyList = new List<SurveyTransport>();
                foreach (var item in db.surveys.Where(i => i.user_id == user_id))
                {
                    surveyList.Add(GetSurvey(item.id));
                }

                return surveyList;
            }
        }

    }

}

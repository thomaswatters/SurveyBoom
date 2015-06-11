using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using Microsoft.AspNet.Identity;
using SurveyMVC.Models;
using SurveyMVC.net.azurewebsites.surveyboomservice;
namespace SurveyMVC.Controllers
{
    public class SurveyController : Controller
    {
        readonly SurveyBoomService _service = new SurveyBoomService();
        // GET: Survey
//        public ActionResult Index()
//        {
//            return View();
//        }
//
        // GET: Survey/Analytics/8
        public ActionResult Analytics(int? key)
        {
            var takenSurvey = new TakenSurveyModel();

            if (Session["takenSurvey"] != null)
                takenSurvey = (TakenSurveyModel) Session["takenSurvey"];


            return View(takenSurvey);
        }

        public ActionResult Chart()
        {
            var takenSurvey = new TakenSurveyModel();

            if (Session["takenSurvey"] != null)
                takenSurvey = (TakenSurveyModel)Session["takenSurvey"];



            var chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());

            chart.Series.Add(new Series("d"));
            chart.Series["d"].ChartType = SeriesChartType.Pie;
            chart.Series["d"]["PieLabelStyle"] = "Outside";
            chart.Series["d"]["PieLineColor"] = "Black";
            chart.Series["d"].Points.DataBindXY(new[] { "a", "b" }, new[] { 8, 7 });
            var ms = new MemoryStream();
            chart.SaveImage(ms, ChartImageFormat.Png);
            return File(ms.ToArray(), "image/png");
        }
        // GET: Survey/Display
        public ActionResult Display(int? key)
        {
            int id = GetCurrentUserId();

            if(id == -1 || key == null)//Check key with the service
                return RedirectToAction("Login", "Account");

            SurveyTransport model = _service.GetSurvey(key.Value);

            if(model == null) throw new Exception();

            Session["CurrentDisplayedSurvey"] = key.Value;

            return View(model);
        }
        [HttpPost]
        // POST: Survey/Display
        public ActionResult Display(string[] textAnswer, string[] longTextAnswer, string[] multipleChoiceQuestion, string[] multipleChoiceAnswer)
        {
            int surveyId;
            if (Session["CurrentDisplayedSurvey"] != null)
                surveyId = (int)Session["CurrentDisplayedSurvey"];
            else 
                throw new Exception();

            var textQuestionsAndAnswer = new List<Tuple<string, string>>();
            var paragraphQuestionAndAnswer = new List<Tuple<string, string>>();
            var multipleChoiceQuestionAndAnswer = new List<Tuple<string, string>>();

            SurveyModel model = null; 

            if (model != null)
            {
                if(textAnswer != null)
                    for (var i = 0; i < textAnswer.Length; i++)
                    {
                        var currentQuestion = model.TextQuestions[i];
                        var currentAnswer = textAnswer[i];
                        var tuple = new Tuple<string, string>(currentQuestion, currentAnswer);
                        textQuestionsAndAnswer.Add(tuple);
                    }

                if(longTextAnswer != null)
                    for (var i = 0; i < longTextAnswer.Length; i++)
                    {
                        var currentQuestion = model.ParagraphQuestions[i];
                        var currentAnswer = longTextAnswer[i];
                        var tuple = new Tuple<string, string>(currentQuestion, currentAnswer);
                        paragraphQuestionAndAnswer.Add(tuple);
                    }

                if (multipleChoiceQuestion != null && multipleChoiceAnswer != null)
                    for (var i = 0; i < multipleChoiceQuestion.Length; i++)
                    {
                        var currentQuestion = multipleChoiceQuestion[i];
                        var currentAnswer = multipleChoiceAnswer[i];
                        var tuple = new Tuple<string, string>(currentQuestion, currentAnswer);
                        multipleChoiceQuestionAndAnswer.Add(tuple);
                    }
            }

            if (Session["CurrentDisplayedSurvey"] != null)
                surveyId = (int) Session["CurrentDisplayedSurvey"];

            var takenSurvey = new TakenSurveyModel
            {
                SurveyId = surveyId,
                TextQuestionsAndAnswers = textQuestionsAndAnswer,
                ParagraphQuestionsAndAnswers = paragraphQuestionAndAnswer,
                MultipleChoiceQuestionsAndAnswers = multipleChoiceQuestionAndAnswer
            };

            Session["takenSurvey"] = takenSurvey;

            return RedirectToAction(string.Format("Analytics/{0}", surveyId), "Survey"); //Here should redirect to a thank you page or statistix page
        }

        // GET: Survey/Create
        public ActionResult Create(string title, string subTitle, string[] textQuestionTitle, string[] paragraphQuestionTitle, string[] multipleChoiceQuestionTitle, string[] multipleChoiceOptions, string[] numberOfOptions)
        {
            int id = GetCurrentUserId();

            if (id == -1)
                return RedirectToAction("Login", "Account");

            var textQuestions = FixArray(textQuestionTitle);
            var paragraphQuestions = FixArray(paragraphQuestionTitle);
            var multipleChoiceQuestionsAndOptions = GetMCQuestions(multipleChoiceQuestionTitle, multipleChoiceOptions, numberOfOptions);

            if(textQuestions.Length == 0 && paragraphQuestions.Length == 0 && multipleChoiceQuestionsAndOptions.Count == 0) return View();
            
            List<QuestionTransport> listOfQuestions = new List<QuestionTransport>();

            foreach (var question in textQuestions)
            {
                QuestionTransport qt = new QuestionTransport
                {
                    Type = QuestionType.ShortAnswer,
                    QuestionText = question,
                    Options = null
                };

                listOfQuestions.Add(qt);
            }

            foreach (var question in paragraphQuestions)
            {
                QuestionTransport qt = new QuestionTransport
                {
                    Type = QuestionType.ShortAnswer,
                    QuestionText = question,
                    Options = null
                };

                listOfQuestions.Add(qt);
            }

            foreach (var question in multipleChoiceQuestionsAndOptions)
            {
                QuestionTransport qt = new QuestionTransport
                {
                    Type = QuestionType.MultipleChoice,
                    QuestionText = question.Key,
                    Options = question.Value.ToArray()
                };

                listOfQuestions.Add(qt);
            }
            SurveyTransport model = new SurveyTransport
            {
                Title = title,
                Description = subTitle,
                UserID = id,
                Questions = listOfQuestions.ToArray()
            };
                       
            int surveyKey = _service.CreateSurvey(model);
            
            return RedirectToAction("Display", "Survey", new { key = surveyKey });//I should go back to the user page and display all the surveys
        }

        #region Methods
        private int GetCurrentUserId()
        {
            int id;
            if (Session["currentUser"] != null)
            {
                id = (int) Session["currentUser"];
            }
            else
            {
                try
                {
                    id = _service.GetUserID(User.Identity.GetUserName());
                }
                catch (Exception e)
                {
                    id = -1; //Not found
                }
                
            }
            return id;
        }

        private List<KeyValuePair<string, List<string>>> GetMCQuestions(string[] multipleChoiceQuestionTitle, string[] multipleChoiceOptions, string[] numberOfOptions)
        {
            var multipleDictionary = new List<KeyValuePair<string, List<string>>>();

            if (multipleChoiceOptions == null) return multipleDictionary;
            
            /*FixArray() removes the first element of the array which is an empty string*/
            var questions = FixArray(multipleChoiceQuestionTitle);
            var options = FixArray(multipleChoiceOptions);
            var numOfOptions = FixArray(numberOfOptions);

            var posSum = 0;

            for (var i = 0; i < questions.Length; i++)
            {
                /*Get Current Question*/
                string current = questions[i];
                /*Get Number of Options for current*/
                short currentNumOfOptions = short.Parse(numOfOptions[i]);
                /*Add the number of options to a counter*/
                posSum += currentNumOfOptions;
                /*calculate the new position in the array to the next options*/
                int position = posSum - currentNumOfOptions;
                /*Get the options with the calculated position*/
                List<string> currentOptions = GetCurrentOptions(position, currentNumOfOptions, options);
                /*Add current question with its options to a dictionary*/
                multipleDictionary.Add(new KeyValuePair<string, List<string>>(current, currentOptions));
            }

            return multipleDictionary;
        }

        private static List<string> GetCurrentOptions(int position, short currentNumOfOptions, string[] options)
        {
            var newPos = position;
            var list = new List<string>();

            for (var i = 0; i < currentNumOfOptions; i++ , newPos++)
            {
                list.Add(options[newPos]);
            }
            return list;
        }

        private static string[] FixArray(string[] oldArray)
        {
            if (oldArray == null) return new string[0];
            var list = oldArray.ToList();
            list.RemoveAt(0);
            return list.ToArray();
        }
        #endregion

        //        // POST: Survey/Create
//        [HttpPost]
//        public ActionResult Create(FormCollection collection)
//        {
//            try
//            {
//                // TODO: Add insert logic here
//
//           //     return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }

        // GET: Survey/Edit/5
//        public ActionResult Edit(int id)
//        {
//            return View();
//        }
//
//        // POST: Survey/Edit/5
//        [HttpPost]
//        public ActionResult Edit(int id, FormCollection collection)
//        {
//            try
//            {
//                // TODO: Add update logic here
//
//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }

        // GET: Survey/Delete/5
//        public ActionResult Delete(int id)
//        {
//            return View();
//        }
//
//        // POST: Survey/Delete/5
//        [HttpPost]
//        public ActionResult Delete(int id, FormCollection collection)
//        {
//            try
//            {
//                // TODO: Add delete logic here
//
//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }
    }
}

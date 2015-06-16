using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyBoomSiteMVC.net.azurewebsites.surveyboom;
using SurveyBoomSiteMVC.Models;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
namespace SurveyBoomSiteMVC.Controllers
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
        public ActionResult Analytics(int key)
        {
            var listOfQuestions = _service.GetAllResponses(key).ToArray();

            var shortAnswers = GetAnswers(listOfQuestions, QuestionType.ShortAnswer);
            var multipleChoiceAnswers = GetAnswers(listOfQuestions, QuestionType.MultipleChoice);
            var multipleChoiceQuestions = GetMultipleChoiceQuestions(listOfQuestions, QuestionType.MultipleChoice);
            var options = GetOptions(listOfQuestions);

            var model = new AnalyticsModel
            {
                ShortAnswers = shortAnswers,
                MultipleChoiceQuestions = multipleChoiceQuestions,
                MultipleChoiceAnswers = multipleChoiceAnswers,
                MultipleChoiceOptions = options
            };

            return View(model);
        }




        public ActionResult Chart(string options, string answers)
        {
            var optionsArray = options.Split(',');
            var answersArray = answers.Split(',');

            var dictionary = new Dictionary<string, int>();

            foreach (var option in optionsArray)
            {
                var option1 = option;
                var result = answersArray.Count(i => i == option1);
                dictionary.Add(option1, result);
            }

            var chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());

            chart.Series.Add(new Series("d"));
            chart.Series["d"].ChartType = SeriesChartType.Pie;
            chart.Series["d"]["PieLabelStyle"] = "Outside";
            chart.Series["d"]["PieLineColor"] = "Black";
            chart.Series["d"].Points.DataBindXY(dictionary.Keys, dictionary.Values);
            var ms = new MemoryStream();
            chart.SaveImage(ms, ChartImageFormat.Png);
            return File(ms.ToArray(), "image/png");
        }
        // GET: Survey/Display
        public ActionResult Display(int? key)
        {
            var id = GetCurrentUserId();

            if (id == -1 || key == null)
                return RedirectToAction("Login", "Account");

            var model = _service.GetSurvey(key.Value);

            if (model == null) throw new Exception();

            Session["CurrentDisplayedSurvey"] = key.Value;

            return View(model);
        }
        [HttpPost]
        // POST: Survey/Display
        public ActionResult Display(string[] textAnswer, string[] multipleChoiceQuestion, string[] multipleChoiceAnswer)
        {
            if (textAnswer == null) textAnswer = new string[0];

            int surveyId;
            if (Session["CurrentDisplayedSurvey"] != null)
                surveyId = (int)Session["CurrentDisplayedSurvey"];
            else
                throw new Exception("");

            int userId = GetCurrentUserId();

            SurveyTransport model = _service.GetSurvey(surveyId);

            if (model != null)
            {
                model.UserID = userId;

                for (var i = 0; i < textAnswer.Length; i++)
                {
                    var question = model.Questions.Where(j => j.Type == QuestionType.ShortAnswer).ElementAt(i);
                    question.ResponseString = textAnswer[i];
                }

                if (multipleChoiceQuestion != null && multipleChoiceAnswer != null)
                    for (var i = 0; i < multipleChoiceQuestion.Length; i++)
                    {
                        var question = model.Questions.Where(j => j.Type == QuestionType.MultipleChoice).ElementAt(i);
                        question.ResponseString = multipleChoiceAnswer[i];
                    }
            }

            var surveyWasSubmitted = _service.SubmitSurveyResponse(model);

            if (!surveyWasSubmitted)
                throw new Exception("meh");

            return RedirectToAction("Analytics", "Survey", new { key = surveyId }); //Here should redirect to a thank you page or statistix page
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

            if (textQuestions.Length == 0 && paragraphQuestions.Length == 0 && multipleChoiceQuestionsAndOptions.Count == 0) return View();

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
                id = (int)Session["currentUser"];
            }
            else
            {
                try
                {
                    id = _service.GetUserID(User.Identity.Name);
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

            for (var i = 0; i < currentNumOfOptions; i++, newPos++)
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

        private static Dictionary<string, List<string>> GetAnswers(QuestionTransport[] questions, QuestionType qt)
        {
            var answers = new Dictionary<string, List<string>>();

            foreach (var question in questions.Where(i => i.Type == qt))
            {
                var question1 = question;
                var result = questions.Where(i => i.QuestionText == question1.QuestionText && i.Type == qt).ToList();
                var textList = result.Select(i => i.ResponseString.ToString()).ToList();

                if (!answers.ContainsKey(question.QuestionText))
                    answers.Add(question.QuestionText, textList);
            }
            return answers;
        }

        private static HashSet<string> GetMultipleChoiceQuestions(QuestionTransport[] listOfQuestions, QuestionType multipleChoice)
        {
            var questions = new HashSet<string>();

            foreach (var question in listOfQuestions.Where(i => i.Type == QuestionType.MultipleChoice))
            {
                questions.Add(question.QuestionText);
            }
            return questions;
        }

        private static Dictionary<string, List<string>> GetOptions(QuestionTransport[] questions)
        {
            var answers = new Dictionary<string, List<string>>();

            foreach (var question in questions)
            {
                var question1 = question;
                var result = questions.Where(i => i.QuestionText == question1.QuestionText && i.Type == QuestionType.MultipleChoice).ToList();

                var textQuestion = question.QuestionText;

                var first = new string[] { };
                foreach (var i in result)
                {
                    first = i.Options;
                    break;
                }
                var textOptions = first.ToList();

                if (!answers.ContainsKey(textQuestion))
                    answers.Add(textQuestion, textOptions);
            }
            return answers;
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

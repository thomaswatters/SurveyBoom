using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyBoomService
{
    public class SurveyTransport
    {
        public string Title { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public List<QuestionTransport> Questions { get; set; }
    }



    public class QuestionTransport
    {

        public enum QuestionType
        {
            MultipleChoice,
            TrueOrFalse,
            ShortAnswer
        };

        public QuestionType Type { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
    }
}
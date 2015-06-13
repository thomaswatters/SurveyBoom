using System;
using System.Collections.Generic;

namespace SurveyMVC.Models
{
    public class TakenSurveyModel
    {
        public int SurveyId { get; set; }
        public List<Tuple<string, string>> TextQuestionsAndAnswers;
        public List<Tuple<string, string>> ParagraphQuestionsAndAnswers;
        public List<Tuple<string, string>> MultipleChoiceQuestionsAndAnswers;
    }
}
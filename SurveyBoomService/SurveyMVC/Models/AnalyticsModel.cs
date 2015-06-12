using System.Collections.Generic;

namespace SurveyMVC.Models
{
    public class AnalyticsModel
    {
        public Dictionary<string, List<string>> ShortAnswers { get; set; }
        public HashSet<string> MultipleChoiceQuestions { get; set; } 
        public Dictionary<string, List<string>> MultipleChoiceAnswers { get; set; }
        public Dictionary<string, List<string>> MultipleChoiceOptions { get; set; }
    }
}
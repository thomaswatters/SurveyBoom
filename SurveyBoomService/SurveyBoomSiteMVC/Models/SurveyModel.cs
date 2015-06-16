using System.Collections.Generic;

namespace SurveyBoomSiteMVC.Models
{
    public class SurveyModel
    {
        public int Key { get; set; }
        public string Title { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public string[] TextQuestions { get; set; }
        public string[] ParagraphQuestions { get; set; }
        public List<KeyValuePair<string, List<string>>> MultipleChoiceQuestions { get; set; }
    }
}
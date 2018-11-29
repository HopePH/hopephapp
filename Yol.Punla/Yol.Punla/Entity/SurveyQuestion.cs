using System.Collections.Generic;

namespace Yol.Punla.Entity
{
    public class SurveyQuestion
    {
        public bool IsChoices { get; set; }
        public string Question { get; set; }
        public IEnumerable<QuestionChoices> QuestionChoices { get; set; } = new List<QuestionChoices>();
    }
}

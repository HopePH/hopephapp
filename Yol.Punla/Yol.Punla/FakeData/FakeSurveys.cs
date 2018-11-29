using System.Collections.Generic;

namespace Yol.Punla.FakeData
{
    public class FakeSurveys
    {
        private static IEnumerable<Entity.SurveyQuestion> SurveyFields;
        public static IEnumerable<Entity.SurveyQuestion> Surveys { get => SurveyFields; set => SurveyFields = value; }

        public static void Init()
        {
            SurveyFields = new List<Entity.SurveyQuestion>
            {
                new Entity.SurveyQuestion
                {
                    IsChoices = false,
                    Question = "Age"
                },
                new Entity.SurveyQuestion
                {
                    IsChoices = true,
                    Question = "Gender",
                    QuestionChoices = new List<Entity.QuestionChoices> {
                         new Entity.QuestionChoices{ Text = "Male", Value = "Male" },
                         new Entity.QuestionChoices{ Text = "Female", Value = "Female" }
                    }
                }
            };
        }
    }
}

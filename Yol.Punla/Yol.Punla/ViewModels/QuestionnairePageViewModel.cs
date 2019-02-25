using PropertyChanged;
using System.Collections.ObjectModel;
using Yol.Punla.AttributeBase;
using Yol.Punla.Entity;
using Yol.Punla.Managers;
using System.Linq;
using System.Collections.Generic;
using System;
using Yol.Punla.Barrack;
using Prism.Services;
using Yol.Punla.Utility;
using Unity;
using Prism.Navigation;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class QuestionnairePageViewModel : ViewModelBase
    {
        private readonly IContactManager _contactManager;

        public bool IsChoices => SurveyQuestion.IsChoices;
        public string Question => SurveyQuestion.Question;
        public IEnumerable<QuestionChoices> QuestionChoices => SurveyQuestion.QuestionChoices;
        public SurveyQuestion SurveyQuestion { get; set; } = new SurveyQuestion();
        public ObservableCollection<SurveyQuestion> SurveyQuestions { get; set; }

        public QuestionnairePageViewModel(INavigationService navigationService,
             IContactManager contactManager) : base(navigationService)
            => _contactManager = contactManager;

        public override async void PreparingPageBindings()
        {
            try
            {
                var surveyResults = await _contactManager.GetSurveyQuestions();

                if (surveyResults != null && surveyResults.Count() > 0)
                {
                    SurveyQuestions = new ObservableCollection<SurveyQuestion>(surveyResults);
                    SurveyQuestion = SurveyQuestions.First();
                }
            }
            catch (Exception ex)
            {
                AppUnityContainer.Instance.Resolve<IDependencyService>().Get<ILogger>().Log(ex);
            }
        }
    }
}

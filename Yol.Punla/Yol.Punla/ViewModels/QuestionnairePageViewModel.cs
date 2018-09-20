using PropertyChanged;
using System.Collections.ObjectModel;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Entity;
using Yol.Punla.Mapper;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class QuestionnairePageViewModel : ViewModelBase
    {
        public bool IsChoices { get; set; }
        public string Question { get; set; } = "Are you single?";
        public ObservableCollection<QuestionChoices> QuestionChoices { get; set; }

        public QuestionnairePageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser) : base(serviceMapper, appUser)
        {
        }

        public override void PreparingPageBindings()
        {
            IsChoices = false;
            QuestionChoices = new ObservableCollection<QuestionChoices>
            {
                new QuestionChoices{ Text = "Yes", Value = "Yes" },
                new QuestionChoices{ Text = "No", Value = "No" }
            };
        }
    }
}

using PropertyChanged;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Mapper;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class QuestionnairePageViewModel : ViewModelBase
    {
        public QuestionnairePageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser) : base(serviceMapper, appUser)
        {
        }

        public override void PreparingPageBindings()
        {
           
        }
    }
}

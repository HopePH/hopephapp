using Prism.Navigation;
using PropertyChanged;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class NavPageViewModel : ViewModelBase
    {
        public NavPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void PreparingPageBindings() => IsBusy = false;
    }
}

using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Localized;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class WikiDetailsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;

        public ICommand NavigateBackCommand => new DelegateCommand(GoBack);
        public string ItemDetails { get; set; }

        public WikiDetailsPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser, 
            INavigationService navigationService, 
            INavigationStackService navigationStackService) : base(serviceMapper, appUser)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            Title = AppStrings.TitleReadContents;
        }

        public override void PreparingPageBindings()
        {
            if (PassingParameters != null && PassingParameters.ContainsKey("ItemSelected"))
                ItemDetails = ((Entity.Wiki)PassingParameters["ItemSelected"]).Content;

            IsBusy = false;
        }

        private void GoBack() => NavigateBackHelper(_navigationStackService, _navigationService);
    }
}

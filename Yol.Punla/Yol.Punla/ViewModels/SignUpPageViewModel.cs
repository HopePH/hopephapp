using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class SignUpPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;

        private string _keyHashString;
        public string KeyHashString { get => _keyHashString; }

        public ICommand FacebookSignUpCommand => new DelegateCommand(FacebookSignup);
        public ICommand ViewKeyHashStringCommand => new DelegateCommand(DisplayHashString);
        public ICommand VerifyEmailCommand => new DelegateCommand(VerifyEmail);

        public SignUpPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationService navigationService,
            INavigationStackService navigationStackService) : base(serviceMapper, appUser)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
        }

        public override void PreparingPageBindings()
        {
            _keyHashString = _keyValueCacheUtility.GetUserDefaultsKeyValue("KeyHashString");
            IsBusy = false;
        }

        private void FacebookSignup()
        {
            if (ProcessInternetConnection(true))
            {
                PassingParameters.Add("ComingFromLogin", false);
                NavigateToPageHelper(nameof(ViewNames.NativeFacebookPage), _navigationStackService, _navigationService, PassingParameters);
            }
        }

        private void DisplayHashString()
        {
            ProcessAnalyticReportingForHockeyApp(string.Format("KeyHash:{0}", KeyHashString ?? ""));
            Acr.UserDialogs.UserDialogs.Instance.Alert(KeyHashString, "Your Key hash", "OK");
        }

        private void VerifyEmail()
        {
            NavigateToPageHelper(nameof(ViewNames.EmailVerificationPage), _navigationStackService, _navigationService, PassingParameters);
        }
    }
}

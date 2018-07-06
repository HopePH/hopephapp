using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Entity;
using Yol.Punla.Managers;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class LogonPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;
        private readonly IContactManager _userManager;

        public ICommand FacebookLogonCommand => new DelegateCommand(FacebookLogin);
        public ICommand GoToSignUpCommand => new DelegateCommand(Signup);
        public ICommand GoToSigninWithAliasCommand => new DelegateCommand(SigninWithAlias);


        public Contact CurrentContact { get; set; }

        public LogonPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationService navigationService,
            IContactManager userManager,
            INavigationStackService navigationStackService) : base(serviceMapper, appUser)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _userManager = userManager;
        }

        public override void PreparingPageBindings()
        {
            IsBusy = false;
        }

        private void Signup() => NavigateToPageHelper(nameof(ViewNames.SignUpPage), _navigationStackService, _navigationService, PassingParameters);

        private void FacebookLogin()
        {
            if (ProcessInternetConnection(true))
            {
                PassingParameters.Add("ComingFromLogin", true);
                NavigateToPageHelper(nameof(ViewNames.NativeFacebookPage), _navigationStackService, _navigationService, PassingParameters); 
            }
        }

        private void SigninWithAlias()
        {
            NavigateToPageHelper(nameof(ViewNames.RequestSigninVerificationCodePage), _navigationStackService, _navigationService, PassingParameters);
        }
    }
}

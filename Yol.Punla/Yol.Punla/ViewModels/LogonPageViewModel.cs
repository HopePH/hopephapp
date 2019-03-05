using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System.Threading.Tasks;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;
using Yol.Punla.Managers;
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

        public ICommand GoToSignUpCommand => new DelegateCommand(async () => await Signup());
        public ICommand GoToSigninWithAliasCommand => new DelegateCommand(async () =>  await SigninWithAlias());
        public Contact CurrentContact { get; set; }
        public bool IsOpen { get; set; }

        public LogonPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationService navigationService,
            IContactManager userManager,
            INavigationStackService navigationStackService) : base(navigationService)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _userManager = userManager;
        }

        public override void PreparingPageBindings() => IsBusy = false;

        private async Task Signup() => await NavigateToPageHelper(nameof(ViewNames.EmailVerificationPage), PassingParameters);

        private async Task SigninWithAlias() => await NavigateToPageHelper(nameof(ViewNames.RequestSigninVerificationCodePage), PassingParameters);
    }
}

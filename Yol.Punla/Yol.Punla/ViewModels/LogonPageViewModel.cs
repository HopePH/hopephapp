using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System.Threading.Tasks;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Managers;
using Yol.Punla.NavigationHeap;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class LogonPageViewModel : ViewModelBase
    {
        public ICommand GoToSignUpCommand => new DelegateCommand(async () => await Signup());
        public ICommand GoToSigninWithAliasCommand => new DelegateCommand(async () =>  await SigninWithAlias());

        public LogonPageViewModel(IServiceMapper serviceMapper,
            IAppUser appUser,
            INavigationService navigationService,
            IContactManager userManager,
            INavigationStackService navigationStackService) : base(navigationService) { }

        public override void PreparingPageBindings() 
            => IsBusy = false;

        private async Task Signup() 
            => await NavigateToPageHelper(nameof(ViewNames.EmailVerificationPage), PassingParameters);

        private async Task SigninWithAlias()
            => await NavigateToPageHelper(nameof(ViewNames.RequestSigninVerificationCodePage), PassingParameters);
    }
}

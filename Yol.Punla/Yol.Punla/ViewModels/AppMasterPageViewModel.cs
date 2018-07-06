using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.Localized;
using Yol.Punla.Mapper;
using Yol.Punla.Messages;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class AppMasterPageViewModel : ViewModelBase
    {
        private readonly INavigationStackService _navigationStackService;
        private readonly INavigationService _navigationService;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set {
                SetProperty(ref _isOpen, value);
                IsShowOnlyWhenLogon = AppUser.SignUpCompleted && AppUser.IsAuthenticated;
            }
        }

        public bool IsShowOnlyWhenLogon { get; set; }        
        public string GenericMessage { get; set; }
        public bool GenericMessageWasDisplayed { get; set; }        
        public bool IsShowCrashLiveEnabler { get; set; }       
        public ICommand MentalCareFacilitiesPageCommand => new DelegateCommand(RedirectToMentalCarePage);
        public ICommand InfoPageCommand => new DelegateCommand(RedirectToInfoPage);
        public ICommand CrisisCommand => new DelegateCommand(async () => await RedirectToCrisis());
        public ICommand WriteDownCommand => new DelegateCommand(async() => await WriteYourFeelings());
        public ICommand CrashHandledCommand => new DelegateCommand(PushHandledCrash);
        public ICommand CrashUnHandledCommand => new DelegateCommand(PushUnHandledCrash);
        public ICommand SettingsPageCommand => new DelegateCommand(RedirectToSettingsPage);
        public ICommand NotificationsCommand => new DelegateCommand(async () => await GoToNotifications());

        public AppMasterPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationStackService navigationStackService,
            INavigationService navigationService) : base(serviceMapper, appUser)
        {
            _navigationStackService = navigationStackService;
            _navigationService = navigationService;
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();

            //chito.notes. this should not be in onappearing as side menu dont appear.
            MessagingCenter.Subscribe<AppMasterPageMessage>(this, "MasterDetailPageToggleMessage", message =>
            {
                IsOpen = message.IsOpen;
            });
        }

        public override void PreparingPageBindings()
        {
            IsShowCrashLiveEnabler = bool.Parse(AppSettingsProvider.Instance.GetValue("IsShowLiveCrash"));
            IsOpen = false;
            IsBusy = false;
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AppMasterPageMessage>(this, "MasterDetailPageToggleMessage");
        }

        private void RedirectToMentalCarePage() => ChangeRootAndNavigateToPageHelper(nameof(ViewNames.HomePage), _navigationStackService, _navigationService);

        private void RedirectToInfoPage() => NavigateToPageHelper(nameof(ViewNames.WikiPage), _navigationStackService, _navigationService);

        private async Task RedirectToCrisis()
        {
            if (!AppUser.SignUpCompleted || !AppUser.IsAuthenticated)
            {
                GenericMessage = AppStrings.SignUpMessage;
                await UserDialogs.Instance.AlertAsync(GenericMessage, AppStrings.RequiresSignUp, AppStrings.Ok);
                _keyValueCacheUtility.GetUserDefaultsKeyValue("NewPage", nameof(ViewNames.CrisisHotlineListPage));
                NavigateToPageHelper(nameof(ViewNames.LogonPage), _navigationStackService, _navigationService, null, true);
            }
            else
                NavigateToPageHelper(nameof(ViewNames.CrisisHotlineListPage), _navigationStackService, _navigationService);
        }

        private async Task WriteYourFeelings()
        {
            //chito. write down https://www.wikihow.com/Tell-Your-Best-Friend-You-Are-Depressed
            if (!AppUser.SignUpCompleted || !AppUser.IsAuthenticated)
            {
                GenericMessage = AppStrings.SignUpMessage;
                await UserDialogs.Instance.AlertAsync(GenericMessage, AppStrings.RequiresSignUp, AppStrings.Ok);
                _keyValueCacheUtility.GetUserDefaultsKeyValue("NewPage", nameof(ViewNames.PostFeedPage));
                NavigateToPageHelper(nameof(ViewNames.LogonPage), _navigationStackService, _navigationService, null, true);
            }
            else
                NavigateToPageHelper(nameof(ViewNames.PostFeedPage), _navigationStackService, _navigationService);
        }

        private void PushHandledCrash()
        {
            if (IsShowCrashLiveEnabler)
            {
                try
                {
                    throw new ArgumentException("Handle Live Crash Pushed.");
                }
                catch (Exception ex)
                {
                    ProcessErrorReportingForHockeyApp(ex, true);
                }
            }
        }

        private void PushUnHandledCrash()
        {
            if (IsShowCrashLiveEnabler)
            {
                throw new ArgumentException("NOT Handle Live Crash Pushed.");
            }
        }

        private void RedirectToSettingsPage() => NavigateToPageHelper(nameof(ViewNames.SettingsPage), _navigationStackService, _navigationService);

        private async Task GoToNotifications()
        {
            if (!AppUser.SignUpCompleted || !AppUser.IsAuthenticated)
            {
                GenericMessage = AppStrings.SignUpMessage;
                await UserDialogs.Instance.AlertAsync(GenericMessage, AppStrings.RequiresSignUp, AppStrings.Ok);
                _keyValueCacheUtility.GetUserDefaultsKeyValue("NewPage", nameof(ViewNames.NotificationsPage));
                NavigateToPageHelper(nameof(ViewNames.LogonPage), _navigationStackService, _navigationService, null, true);
            }
            else
                NavigateToPageHelper(nameof(ViewNames.NotificationsPage), _navigationStackService, _navigationService);
        }
    }
}

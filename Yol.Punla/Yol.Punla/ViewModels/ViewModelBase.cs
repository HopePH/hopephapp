using Acr.UserDialogs;
using FluentValidation.Results;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Localized;
using Yol.Punla.Messages;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [AddINotifyPropertyChangedInterface]
    public abstract class ViewModelBase : BindableBase, INavigationAware, IViewEventsListener
    {
        #region "ViewModel Wide Specifics"

        private readonly INavigationStackService _navigationStackService;
        private readonly INavigationService _navigationService;
        private const string VALIDATIONERRMSG = "Please correct the following: \r\n\r\n{0}";
        private const string NOINTERNETMSG = "Your internet connection seems to have dropped out. You are now offline mode.";
        private const string TASKCANCELLEDMSG = "The fetching of data is running slow. Please check the speed of your internet connection then try again.";
        private const int CANCELTASKINSECS = 10;
        private const string FAKEAPIURI = @"44.665.77.888";
        private int taskHandlerCount = 0;
        private bool isForcedToStopTaskHandlerTimer = false;

        protected bool IsInternetConnected => AppCrossConnectivity.Instance.IsConnected;

        private readonly IServiceMapper _serviceMapper;
        public IServiceMapper ServiceMapper { get => _serviceMapper; }

        private readonly IAppUser _appUser;
        public IAppUser AppUser { get => _appUser; }

        private readonly IUserDialogs _userDialogs;
        public IUserDialogs UserDialog { get => _userDialogs; }

        private bool _isCustomNavBarVisible = true;
        public bool IsCustomNavBarVisible
        {
            get => _isCustomNavBarVisible;
            set => _isCustomNavBarVisible = value;
        }

        private bool _isBusy = true;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;

                    if (!_isBusy)
                        isForcedToStopTaskHandlerTimer = true;
                }
            }
        }

        public IDisposable AlertResponse { get; set; }
        public ICommand TapNavigationBackCommand => new DelegateCommand(RemoveCurrentPage);
        public INavigationParameters ParametersBeforePageNavigate { get; set; } = new NavigationParameters();
        public INavigationParameters PassingParameters { get; set; } = new NavigationParameters();
        protected TokenHandler TokenHandler { get; set; }
        public StringBuilder PageErrors { get; set; }       
        public string Title { get; set; }
        public bool IsShowBackArrow { get; set; } = true;

        protected ViewModelBase(INavigationService navigationService)
        {
            Debug.WriteLine(string.Format("HOPEPH Loading constructor {0}.", this.GetType().Name));
            _navigationService = navigationService;
            _serviceMapper = AppUnityContainer.Instance.Resolve<IServiceMapper>();
            _appUser = AppUnityContainer.Instance.Resolve<IAppUser>();
            _navigationStackService = AppUnityContainer.Instance.Resolve<INavigationStackService>();
            _userDialogs = AppUnityContainer.Instance.Resolve<IUserDialogs>();
            AppStrings.Culture = new CultureInfo(AppSettingsProvider.Instance.GetValue("AppCulture"));
        }

        //before we navigate
        //https://medium.com/@hminaya/prism-101-navigation-8433eb30f578
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
            => ParametersBeforePageNavigate = parameters;

        //after the viewmodel has been pushed to stack
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            PassingParameters = new NavigationParameters();
            ParametersBeforePageNavigate = new NavigationParameters();
            AppUnityContainer.Instance.Resolve<INavigationStackService>().IsDisableNavPagePop = false;
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
            PassingParameters = parameters;
            PreparingPageBindings();
        }

        public virtual void OnAppearing() { }
      
        public virtual void OnDisappearing() { }

        public abstract void PreparingPageBindings();

        protected bool ProcessValidationErrors(ValidationResult validationResult, bool isShowErrorDialog = true)
        {
            IsBusy = true;

            if (!validationResult.IsValid)
            {
                PageErrors = new StringBuilder();
                int count = 1;

                foreach (var failure in validationResult.Errors)
                {
                    string appendString = (count < validationResult.Errors.Count) ? "\r\n" : "";
                    PageErrors.Append(failure.ErrorMessage + appendString);
                    ++count;
                }

                if (isShowErrorDialog)
                    _userDialogs.Alert(string.Format(VALIDATIONERRMSG, PageErrors.ToString()), "Error", "OK");

                IsBusy = false;
                return false;
            }

            return true;
        }

        protected bool ProcessInternetConnection(bool isShowErrorDialog = true)
        {
            IsBusy = true;

            if (!IsInternetConnected)
            {
                if (isShowErrorDialog) AlertResponse = _userDialogs.Alert(NOINTERNETMSG, "Offline", "Ok");
                IsBusy = false;
                return IsInternetConnected;
            }

            return IsInternetConnected;
        }

        protected void ProcessCustomPageErrors(string customError, bool isShowErrorDialog = true)
        {
            PageErrors = new StringBuilder();
            PageErrors.Append(customError);
            if (isShowErrorDialog) _userDialogs.Alert(PageErrors.ToString(), "Error", "OK");
            IsBusy = false;
        }

        protected void ProcessErrorReportingForRaygun(Exception ex, bool isShowErrorDialog = true)
        {
            IsBusy = false;

#if !FAKE
            AppUnityContainer.Instance.Resolve<IDependencyService>().Get<ILogger>().Log(ex);
#endif
            if (isShowErrorDialog)
            {
                var maskedServerAPIURI = ex.Message.Replace(AppSettingsProvider.Instance.GetValue("MaskingAPI"), FAKEAPIURI);
                _userDialogs.Alert(maskedServerAPIURI);
            }
        }

        protected void ProcessErrorReportingForHockeyApp(Exception ex, bool isShowErrorDialog = false)
        {
            IsBusy = false;

#if FAKE
            //chito. do not register to the hockeyapp when unittesting
#else
            ////chito. HEA this just means Handled Exception, just make it shorter. Also, there's no need to put if this is Android or IOS since they have unique hockeyid per platform        
            //HockeyApp.MetricsManager.TrackEvent(string.Format("HE.{0}", ex.Message ?? ""));
#endif

            //chito.22/12/2017. make sure that the real api is masked to fake one...
            if (isShowErrorDialog && ex.Message != "A task was canceled.")
            {
                var maskedServerAPIURI = ex.Message.Replace(AppSettingsProvider.Instance.GetValue("MaskingAPI"), FAKEAPIURI);
                UserDialogs.Instance.Alert(maskedServerAPIURI);
            }
        }

        protected void ProcessAnalyticReportingForHockeyApp(string message)
        {
            IsBusy = false;

#if FAKE
            //chito. do not register to the hockeyapp when unittesting
#else
            ////chito. AR this just means Analytics Reporting, just make it shorter. Also, there's no need to put if this is Android or IOS since they have unique hockeyid per platform        
            //HockeyApp.MetricsManager.TrackEvent(string.Format("AR.{0}", message ?? ""));
#endif
        }

        protected async Task NavigateToPageHelper(string page, INavigationParameters parameters = null, bool? useModalNavigation = null, bool animated = true)
        {
            _navigationStackService.UpdateStackState(CleanedPageName(page));
            await _navigationService.NavigateAsync(page, parameters, useModalNavigation, animated);
        }

        protected async Task ChangeRootAndNavigateToPageHelper(string page, INavigationParameters parameters = null, bool? useModalNavigation = null, bool animated = true)
        {
            var rootPage = AppSettingsProvider.Instance.GetValue("AppRootURI") + $"{ViewNames.AppMasterPage}/{nameof(ViewNames.NavPage)}/{page}";
            _navigationStackService.ResetStackStateTo(CleanedPageName(page));
            await _navigationService.NavigateAsync(rootPage, parameters, useModalNavigation, animated);
        }

        protected async Task NavigateBackHelper(INavigationParameters parameters = null, bool? useModalNavigation = null, bool animated = true)
        {
            AppUnityContainer.Instance.Resolve<INavigationStackService>().IsDisableNavPagePop = true;
            await _navigationService.GoBackAsync(parameters, useModalNavigation, animated);
            RemoveCurrentPage();
        }

        private void RemoveCurrentPage()
        {
            var toBeRemovedPage = _navigationStackService.CurrentStack;
            _navigationStackService.RemovePageFromNavigationStack(toBeRemovedPage);
        }

        private string CleanedPageName(string pageName)
        {
            string newPageName = pageName;
            int qIdx = pageName.IndexOf('/');
            if (qIdx >= 0) newPageName = pageName.Substring(qIdx + 1);
            return newPageName;
        }

        protected CancellationTokenSource CreateNewHandledTokenSource(string taskName, int cancelTaskInSecs = CANCELTASKINSECS)
        {
            TokenHandler = AppUnityContainer.Instance.Resolve<TokenHandler>();

            if (TokenHandler.IsTaskHandlerTimerRunning)
                return TokenHandler.TokenSource;
            else
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(cancelTaskInSecs));
                TokenHandler.Init(taskName, cts);

                taskHandlerCount = 0;
                isForcedToStopTaskHandlerTimer = false;
                CancelTokenInSecs(cancelTaskInSecs);
                return cts;
            }
        }

        //chito. the timer PCL https://xamarinhelp.com/xamarin-forms-timer/
        private void CancelTokenInSecs(int cancelTaskInSecs)
        {
            Device.StartTimer(TimeSpan.FromSeconds(0.25), () =>
            {
                if (isForcedToStopTaskHandlerTimer)
                    return TokenHandler.Destroy();

                try
                {
                    ++taskHandlerCount;

                    if (TokenHandler.Token != CancellationToken.None && TokenHandler.Token.IsCancellationRequested)
                        TokenHandler.Token.ThrowIfCancellationRequested();

                    if (taskHandlerCount > (cancelTaskInSecs * 4))
                        return TokenHandler.Destroy();
                }
                catch (Exception)
                {
                    ProcessTaskErrors(true);
                    return TokenHandler.Destroy();
                }

                return TokenHandler.IsTaskHandlerTimerRunning = true;
            });
        }

        private void ProcessTaskErrors(bool isShowErrorDialog = false)
        {
            PageErrors = new StringBuilder();
            PageErrors.Append(TASKCANCELLEDMSG);

            if (isShowErrorDialog)
                UserDialogs.Instance.Alert(PageErrors.ToString(), "Error", "OK");
        }

        #endregion


        #region Extended

        public int NoOfNotifications
        {
            get
            {
                var keyValueCacheUtility = AppUnityContainer.Instance.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>();
                var qtyString = keyValueCacheUtility.GetUserDefaultsKeyValue("NoOfNotifications");
                int qtyInt = 1;
                int.TryParse(qtyString, out qtyInt);

                if (qtyInt == 0)
                    ++qtyInt;

                return qtyInt;
            }
        }

        public ICommand ShowSideBarCommand => new DelegateCommand(DisplaySideBar);

        private void DisplaySideBar()
        {
            AppMasterPageMessage log = new AppMasterPageMessage() { IsOpen = true };
            MessagingCenter.Send<AppMasterPageMessage>(log, "MasterDetailPageToggleMessage");
        }

        #endregion
    }
}

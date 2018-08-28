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
using System.Windows.Input;
using Unity;
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
    [AddINotifyPropertyChangedInterface]
    public abstract class ViewModelBase : BindableBase, INavigationAware, IViewEventsListener
    {
        #region "ViewModel Wide Specifics"
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

        public NavigationParameters PassingParameters { get; set; }
        protected TokenHandler TokenHandler { get; set; }
        public StringBuilder PageErrors { get; set; }
        public IDisposable AlertResponseNoInternet { get; set; }
        public string Title { get; set; }
        public bool IsShowBackArrow { get; set; } = true;

        protected ViewModelBase(IServiceMapper serviceMapper, IAppUser appUser)
        {
            Debug.WriteLine(string.Format("HOPEPH Loading constructor {0}.", this.GetType().Name));
            _serviceMapper = serviceMapper;
            _appUser = appUser;

            AppStrings.Culture = new CultureInfo(AppSettingsProvider.Instance.GetValue("AppCulture"));
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters) => PassingParameters = parameters;

        public virtual void OnNavigatedTo(NavigationParameters parameters) => PassingParameters = parameters;

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
            PassingParameters = parameters;
            PreparingPageBindings();
            PassingParameters = new NavigationParameters();
        }

        public virtual void OnAppearing() { }
      
        public virtual void OnDisappearing() { }

        public abstract void PreparingPageBindings();

        protected bool ProcessValidationErrors(ValidationResult validationResult, bool isShowErrorDialog = false)
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
                    UserDialogs.Instance.Alert(string.Format(VALIDATIONERRMSG, PageErrors.ToString()), "Error", "OK");
                
                IsBusy = false;
                return false;
            }

            return true;
        }

        protected bool ProcessInternetConnection(bool isShowErrorDialog = false)
        {
            if (!IsInternetConnected)
            {
                if (isShowErrorDialog)
                    AlertResponseNoInternet = UserDialogs.Instance.Alert(NOINTERNETMSG, "Offline", "Ok");

                IsBusy = false;
                return IsInternetConnected;
            }

            return IsInternetConnected;
        }

        protected void ProcessCustomPageErrors(string customError, bool isShowErrorDialog = false)
        {
            PageErrors = new StringBuilder();
            PageErrors.Append(customError);

            if (isShowErrorDialog)
                UserDialogs.Instance.Alert(PageErrors.ToString(), "Error", "OK");

            IsBusy = false;
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

        protected void NavigateToPageHelper(string page, INavigationStackService navigationStackService, INavigationService navigationService, NavigationParameters parameters = null
            ,bool? useModalNavigation = null, bool animated = true)
        {
            navigationStackService.UpdateStackState(page);
            navigationService.NavigateAsync(page, parameters, useModalNavigation, animated);
        }

        protected void ChangeRootAndNavigateToPageHelper(string page, INavigationStackService navigationStackService, INavigationService navigationService, NavigationParameters parameters = null
           , bool? useModalNavigation = null, bool animated = true)
        {
            var rootPage = AppSettingsProvider.Instance.GetValue("AppRootURI") + $"{nameof(ViewNames.AppMasterPage)}/{nameof(NavigationPage)}/{page}";
            navigationStackService.ResetStackStateTo(page);
            navigationService.NavigateAsync(rootPage, parameters, useModalNavigation, animated);

            ////chito. bug in ios requires twice call
            ////navigationService.NavigateAsync(rootPage, parameters, useModalNavigation, animated);            
            //NavigationBug.SetAbsoluteURI(navigationService, rootPage, parameters, useModalNavigation, animated);
        }

        protected void NavigateBackHelper(INavigationStackService navigationStackService, INavigationService navigationService, NavigationParameters parameters = null
          , bool? useModalNavigation = null, bool animated = true)
        {
            var toBeRemovedPage = navigationStackService.CurrentStack;
            navigationStackService.RemovePageFromNavigationStack(toBeRemovedPage);

            var currentStack = navigationStackService.CurrentStack;

            if (currentStack == ViewNames.HomePage.ToString())
            {
                ChangeRootAndNavigateToPageHelper(ViewNames.HomePage.ToString(), navigationStackService, navigationService);
                return;
            }

            if (navigationStackService.NavigationStack.Count > 1)
                navigationService.GoBackAsync(parameters, useModalNavigation, animated);
            else
                ChangeRootAndNavigateToPageHelper(currentStack, navigationStackService, navigationService);
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

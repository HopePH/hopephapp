using Acr.UserDialogs;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Plugin.Notifications;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Services;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;
using Xamarin.Forms;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.GatewayAccess;
using Yol.Punla.Managers;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Repository;
using Yol.Punla.UnitTest.Mocks;
using Yol.Punla.Utility;
using Yol.Punla.Views;

namespace Yol.Punla.UnitTest.Barrack
{
    public class UnitTestApp : PrismApplication, IAppConfigurations
    {
        public bool Expired { get; set; } = true;
        public string UserName { get; set; }
        public string Password { get; set; }
        public Entity.Contact CurrentContact { get; set; }
        public IEnumerable<Entity.PostFeed> UnSupportedPostFeeds { get; set; }

        public void ConfigureAppWideSettings()
        {
            Debug.WriteLine("HOPEPH Getting device locale.");
            var unityContainer = Container.GetContainer();
            var dependencyService = unityContainer.Resolve<IDependencyService>();
            var localeLanguageUtility = dependencyService.Get<ILocalLanguageUtility>();
            string localeLanguage = localeLanguageUtility.GetLanguageLocale();
            string supportedLanguage = "fil"; //en-US or comment this for English | fil for Filipino | ja for Japenese.

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    switch (localeLanguage)
                    {
                        //bahasa indonesia language
                        case "id_US":
                            supportedLanguage = "id";
                            break;

                        //chito. default to Philippines Fil, Eng, and others that are not defined
                        default:
                            supportedLanguage = "fil";
                            break;
                    }
                    break;
                case Device.Android:
                    switch (localeLanguage)
                    {
                        //bahasa indonesia language
                        case "Bahasa Melayu":
                            supportedLanguage = "id";
                            break;

                        //chito. default to Philippines Fil, Eng, and others that are not defined
                        default:
                            supportedLanguage = "fil";
                            break;
                    }
                    break;
                case Device.UWP:
                default:
                    break;
            }

            //note: AppSettingsProvider cannot user LocalizationService as it will go in cycle. LocalizationService depends on AppSettigsProvider
            AppSettingsProvider.Instance.SetAppSettings(() =>
            {
                Dictionary<String, String> listSettings = new Dictionary<string, string>();
                Debug.WriteLine("HOPEPH Adding configurations.");
                string genericAPI = @"tobemovedtoblockchain"; 
                listSettings.Add("RestBaseApi", string.Format("{0}/api/", genericAPI));
                listSettings.Add("PostFeedAPI", genericAPI);
                listSettings.Add("MaskingAPI", genericAPI.Replace(@"https://", "").Replace(@"http://", ""));
                listSettings.Add("AppCulture", supportedLanguage);
                listSettings.Add("SQLiteName", "alfonRBF.db3");
                listSettings.Add("AppRootURI", "http://hopeph.com/");
                listSettings.Add("IsShowLiveCrash", "false");

                Debug.WriteLine("HOPEPH Setting caching of local tables in Hours.");
                listSettings.Add("CacheDBMentalHealthFacility", "48");     // Reynz: Equivalent to 2 days
                listSettings.Add("CacheDBPostFeed", "2");    // Reynz: Equivalent to 2 hours
                listSettings.Add("ImagesCacheDurationInDays", "7");
                listSettings.Add("CacheDBWiki", "16"); // Reynz : Equivalent to 16 hours
                return listSettings;
            });
        }

        public void ConfigureDatabaseInitilization()
        {
            Debug.WriteLine("HOPEPH Create the table here once only.");
            var unityContainer = Container.GetContainer();
            var mockRepository = unityContainer.Resolve<IMockRepository>();
            mockRepository.CreateTablesOnce();
        }

        //chito. this code is just put to public here to be unit tested
        public async Task OnResumePublic()
        {
            try
            {
                AppCrossConnectivity.Init(CrossConnectivity.Current);
                UserDialogs.Instance.ShowLoading("Reconnecting, please wait...");

                if (AppCrossConnectivity.Instance.IsConnected)
                {
                    var keyValueCacheUtility = Container.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>();
                    UserName = keyValueCacheUtility.GetUserDefaultsKeyValue("Username");
                    Password = keyValueCacheUtility.GetUserDefaultsKeyValue("Password");

                    if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))  
                        await PostFeedSpokeStatic.LogonToHubFake(UserName, Password);

                    await ShowLocalNotifications();
                }
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        protected override void OnInitialized()
        {
            try
            {
                Xamarin.Forms.Mocks.MockForms.Init();
                CreateMocks();
                ConfigureAppWideSettings();
                ConfigureDatabaseInitilization();               

                var unityContainer = Container.GetContainer();
                unityContainer.RegisterInstance<INavigationService>(NavigationService, new ContainerControlledLifetimeManager());
                AppUnityContainer.Init(unityContainer);
                AppCrossConnectivity.Init(unityContainer.Resolve<IConnectivity>());

                if (WasSignedUpAndLogon())
                    NavigateToRootPage(nameof(MainTabbedPage) + AddPagesInTab(), unityContainer.Resolve<INavigationStackService>(), NavigationService);
                else
                    NavigateToModalRootPage(nameof(LogonPage), unityContainer.Resolve<INavigationStackService>(), NavigationService);

                AllowAppPermissions();
            }
            catch (Exception ex)
            {
                AppUnityContainer.Instance.Resolve<IDependencyService>().Get<ILogger>().Log(ex);
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Bootstrapper.InitContainer(containerRegistry);
            Bootstrapper.AutoTypesRegistration<Yol.Punla.App>();
            ManualTypeRegistration(containerRegistry);
        }

        protected async override void OnResume()
        {
            await OnResumePublic();
        }

        protected override void OnSleep()
        {
            try
            {
                PostFeedSpokeStatic.StopConnection();
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex);
            }
        }

        private void CreateMocks()
        {
            UserDialogs.Instance = new UserDialogMock();
            CrossNotifications.Current = new NotificationsMock();
            Container.GetContainer().RegisterInstance<IConnectivity>(new CrossConnectivityMock(), new ContainerControlledLifetimeManager());
            Xamarin.Forms.DependencyService.Register<IHelperUtility, HelperUtilityMock>();
            Xamarin.Forms.DependencyService.Register<IKeyValueCacheUtility, KeyValueCacheMock>();
            Xamarin.Forms.DependencyService.Register<IKeyboardHelper, KeyboardHelperMock>();
            Xamarin.Forms.DependencyService.Register<ILocalLanguageUtility, LocaleLanguageUtilityMock>();

            //chito. always reset the contacts to what is from the fake data, otherwise anything changes from one scenario will be persisted the other scenarios
            FakeData.FakeUsers.Init();
            FakeData.FakeMentalFacility.Init();
            FakeData.FakePostFeeds.Init();
            FakeData.FakeWikis.Init();
            FakeData.FakeSurveys.Init();

            //chito.by default assume that these were presumptions
            var isLoaded = AppInitsHolder.IsWelcomeInstructionsLoaded;
            var userName = AppInitsHolder.UserName;
            var password = AppInitsHolder.Password;
            var notificationsPushedDate = AppInitsHolder.NotificationsPushedDateSaved;
            var wasLogon = AppInitsHolder.WasLogin;
            var wasSignUpCompleted = AppInitsHolder.WasSignUpCompleted;

            ((KeyValueCacheMock)Container.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>()).SetWelcomeInstructionLoadedManually(isLoaded);           
            ((KeyValueCacheMock)Container.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>()).SetUserAndPasswordManually(userName, password);
            ((KeyValueCacheMock)Container.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>()).SetNotificationsPushedDateManually(notificationsPushedDate);
            ((KeyValueCacheMock)Container.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>()).SetAuthenticationManually(wasLogon);
            ((KeyValueCacheMock)Container.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>()).SetSignUpCompletionManually(wasSignUpCompleted);
        }

        private void NavigateToRootPage(string page, INavigationStackService navigationStackService, INavigationService navigationService)
        {
            var rootPage = AppSettingsProvider.Instance.GetValue("AppRootURI") + $"{nameof(NavigationPage)}/{page}";
            navigationStackService.UpdateStackState(page);
            navigationService.NavigateAsync(rootPage);
        }

        private void NavigateToModalRootPage(string page, INavigationStackService navigationStackService, INavigationService navigationService)
        {
            var rootPage = AppSettingsProvider.Instance.GetValue("AppRootURI") + $"{page}";
            navigationStackService.UpdateStackState(page);
            navigationService.NavigateAsync(rootPage);
        }

        private void ManualTypeRegistration(IContainerRegistry containerRegistry)
        {
            Debug.WriteLine("HOPEPH Make all VMs as singleton here in UnitTest.");
            containerRegistry.RegisterForNavigation<NavigationPage>();
        }

        private async void AllowAppPermissions()
        {
            var helperUtility = Container.Resolve<IDependencyService>().Get<IHelperUtility>();
            await helperUtility.CheckIfPermissionToLocationGranted();

            if (AppCrossConnectivity.Instance.IsConnected)
            {
                await CrossNotifications.Current.RequestPermission();
                await ShowLocalNotifications();
            }
        }

        private void ProcessErrorReportingForHockeyApp(Exception ex)
        {
#if FAKE
            //chito. do not register to the hockeyapp when unittesting
#else
            //chito. HEA this just means Handled Exception, just make it shorter. Also, there's no need to put if this is Android or IOS since they have unique hockeyid per platform        
            HockeyApp.MetricsManager.TrackEvent(string.Format("HE.{0}", ex.Message ?? ""));
#endif
        }

        private bool WasWelcomeInstructionsLoaded()
        {
            var _keyValueCacheUtility = Container.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>();
            var cachedValue = _keyValueCacheUtility.GetUserDefaultsKeyValue("WasWelcomeInstructionLoaded");
            return string.IsNullOrEmpty(cachedValue) ? false : bool.Parse(cachedValue);
        }

        private async Task ShowLocalNotifications()
        {
            CurrentContact = Container.GetContainer().Resolve<IContactManager>().GetCurrentContactFromLocal();

            if (CurrentContact != null && IsShowingNotificationHasExpired())
            {
                UnSupportedPostFeeds = await Container.GetContainer().Resolve<IPostFeedService>().GetPostFeedNotifications(CurrentContact.RemoteId);

                if (UnSupportedPostFeeds != null && UnSupportedPostFeeds.Count() > 0)
                {
                    await CrossNotifications.Current.RequestPermission();

                    foreach (var item in UnSupportedPostFeeds)
                    {
                        var title = (item.ContentText.Length > 17) ? item.ContentText.Substring(0, 17) + "..." : item.ContentText;
                        var message = $"Please show support or care to {item.AliasName} by clicking support or by writing friendly advises.";

                        if (title.Length > 20)
                            title = title.Substring(0, 20);

                        await CrossNotifications.Current.Send(new Notification
                        {
                            Title = title,
                            Message = message,
                            Vibrate = true,
                            When = TimeSpan.FromSeconds(3)
                        });
                    }

                    var keyValueCacheUtility = Container.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>();
                    keyValueCacheUtility.GetUserDefaultsKeyValue("NotificationsPushedDate", DateTime.Now.ToString());
                    keyValueCacheUtility.GetUserDefaultsKeyValue("NoOfNotifications", UnSupportedPostFeeds.Count().ToString());
                }
            }
        }

        private bool IsShowingNotificationHasExpired()
        {
            try
            {
                var keyValueCacheUtility = Container.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>();
                var notificationPushedDate = keyValueCacheUtility.GetUserDefaultsKeyValue("NotificationsPushedDate");
                TimeSpan timeDiff = TimeSpan.MinValue;
                int hoursToExpire = 2;
                TimeSpan expirationSpan = new TimeSpan(hoursToExpire, 0, 10);

                if (!string.IsNullOrEmpty(notificationPushedDate))
                    notificationPushedDate = notificationPushedDate.Replace("|", ":");

                if (!string.IsNullOrEmpty(notificationPushedDate))
                    timeDiff = DateTime.Now.Subtract(DateTime.Parse(notificationPushedDate));

                if (timeDiff == TimeSpan.MinValue || (!string.IsNullOrEmpty(notificationPushedDate) && timeDiff >= expirationSpan))
                    Expired = true;
                else
                    Expired = false;

                return Expired;
            }
            catch (SQLite.SQLiteException)
            {
                return Expired;
            }
        }

        private string AddPagesInTab()
        {
            string path = "";
            var children = new List<string>();
            children.Add("addTab=WikiPage");
            path += "?" + string.Join("&", children);
            return path;
        }

        private bool WasSignedUpAndLogon()
        {
            var unityContainer = Container.GetContainer();
            var result = unityContainer.Resolve<IAppUser>().IsAuthenticated && unityContainer.Resolve<IAppUser>().SignUpCompleted;
            return result;
        }
    }
}

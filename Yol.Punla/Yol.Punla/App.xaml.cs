using Prism.Unity;
using Xamarin.Forms;
using Yol.Punla.Barrack;
using System;
using System.Diagnostics;
using Yol.Punla.Utility;
using System.Collections.Generic;
using Yol.Punla.Repository;
using Prism.Navigation;
using Yol.Punla.Views;
using Yol.Punla.NavigationHeap;
using Prism;
using Unity.Lifetime;
using Prism.Ioc;
using Unity;
using Unity.Injection;
using Prism.Services;
using Plugin.Connectivity;
using Plugin.Notifications;
using System.Linq;
using Yol.Punla.GatewayAccess;
using Yol.Punla.Managers;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace Yol.Punla
{
    public partial class App : PrismApplication, IAppConfigurations
    {
        public bool Expired { get; set; } = true;
        public string UserName { get; set; }
        public string Password { get; set; }
        public Entity.Contact CurrentContact { get; set; }
        public IEnumerable<Entity.PostFeed> UnSupportedPostFeeds { get; set; }

        public App(IPlatformInitializer initializer = null) : base(initializer) { }

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
                case Device.WinPhone:
                default:
                    break;
            }

            //note: AppSettingsProvider cannot user LocalizationService as it will go in cycle. LocalizationService depends on AppSettigsProvider
            AppSettingsProvider.Instance.SetAppSettings(() =>
            {
                Dictionary<String, String> listSettings = new Dictionary<string, string>();
                Debug.WriteLine("HOPEPH Adding configurations.");
                string genericAPI = "tobemovedtoblockchain";
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

        protected override void OnInitialized()
        {
            InitializeComponent();
            ConfigureAppWideSettings();
            ConfigureDatabaseInitilization();

            var unityContainer = Container.GetContainer();
            unityContainer.RegisterInstance<INavigationService>(NavigationService, new ContainerControlledLifetimeManager());
            AppUnityContainer.Init(unityContainer);
            AppCrossConnectivity.Init(CrossConnectivity.Current);

            if (WasWelcomeInstructionsLoaded())
                NavigateToRootPage(nameof(WikiPage), Container.Resolve<INavigationStackService>(), NavigationService);
            else
                NavigateToRootPage(nameof(WelcomeInstructionsPage), Container.Resolve<INavigationStackService>(), NavigationService);

            AllowAppPermissions();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Bootstrapper.InitContainer(containerRegistry);
            Bootstrapper.AutoTypesRegistration<App>();
            ManualTypeRegistration(containerRegistry);
        }

        protected async override void OnResume()
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

                Container.Resolve<IDependencyService>().Get<IHelperUtility>().ResetPackageVersionNo();
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

        private void NavigateToRootPage(string page, INavigationStackService navigationStackService, INavigationService navigationService)
        {
            var rootPage = AppSettingsProvider.Instance.GetValue("AppRootURI") + $"{nameof(AppMasterPage)}/{nameof(NavigationPage)}/{page}";
            navigationStackService.UpdateStackState(page);
            navigationService.NavigateAsync(rootPage);
        }

        private void ManualTypeRegistration(IContainerRegistry containerRegistry)
        {
            Debug.WriteLine("HOPEPH Get the onplatform db folder repository.");
            var unityContainer = containerRegistry.GetContainer();
            string dbName = "alfonRBF.db3";
            var dependencyService = unityContainer.Resolve<IDependencyService>();
            var localDBUtility = dependencyService.Get<ILocalDBUtility>();
            var dbPath = localDBUtility.GetDBPath(dbName);

            Debug.WriteLine("HOPEPH Registering repository manually.");
            unityContainer.RegisterType<IMockRepository, MockRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(dbPath, true));
            unityContainer.RegisterType<INavigationStackRepository, NavigationStackRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(dbPath, true));
            unityContainer.RegisterType<IFacilitiesRepository, FacilitiesRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(dbPath, true));
            unityContainer.RegisterType<IContactRepository, ContactRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(dbPath, true));
            unityContainer.RegisterType<ILocalTableTrackingRepository, LocalTableTrackingRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(dbPath, true));
            unityContainer.RegisterType<IPostFeedRepository, PostFeedRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(dbPath, true));
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
                    int count = 0;

                    foreach (var item in UnSupportedPostFeeds)
                    {
                        if (count > 2)
                            break;

                        var title = (item.ContentText.Length > 17) ? item.ContentText.Substring(0,17) + "..." : item.ContentText;
                        var message = $"Please show support or care to {item.AliasName} by clicking support or by writing friendly advises.";

                        if (title.Length > 20)
                            title = title.Substring(0, 20);

                        await CrossNotifications.Current.Send(new Notification
                        {
                            Title = title,
                            Message = message,
                            Vibrate = true,
                            When = TimeSpan.FromSeconds(2)
                        });

                        ++count;
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
    }
}

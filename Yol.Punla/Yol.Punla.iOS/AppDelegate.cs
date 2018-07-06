using Foundation;
using Prism;
using Prism.Ioc;
using UIKit;
using Yol.Punla.EntryPoint;
using FFImageLoading.Forms.Touch;
using HockeyApp.iOS;
using Unity;
using Prism.Unity;
using Xamarin.Forms;
using Yol.Punla.iOS.CustomRenderers;
using Yol.Punla.iOS.Services;
using Yol.Punla.iOS.Utility;
using Yol.Punla.Mapper;
using Yol.Punla.Messages;
using Yol.Punla.Utility;
using FacebookKit = Facebook.CoreKit;

namespace Yol.Punla.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private const string HOCKEYAPPID = "bd2dd53633384e308da32f813bc9438c";
        private PostFeedService postFeedService;
        private readonly ServiceMapper serviceMapper = new ServiceMapper();
        private readonly KeyValueCacheUtility _keyValueCacheUtility = new KeyValueCacheUtility();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            CachedImageRenderer.Init();
            InitFacebookIntegration();
            IosBackgroundService();
            InitHockeyApp();
            SaveCurrentAppVersionToCache();
            LoadApplication(new App(new iOSInitializer()));
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
            app.StatusBarStyle = UIStatusBarStyle.LightContent;
            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return FacebookKit.ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
        }

        public override void WillTerminate(UIApplication uiApplication)
        {
            KillRunningBackgroundService();
            UnsubscribeToMessagingCenter();

            base.WillTerminate(uiApplication);
        }

        private void InitFacebookIntegration()
        {
            FacebookKit.Profile.EnableUpdatesOnAccessTokenChange(true);
            FacebookKit.Settings.AppID = FBSettings.AppId;
            FacebookKit.Settings.DisplayName = FBSettings.DisplayName;
        }

        private void InitHockeyApp()
        {
            var manager = BITHockeyManager.SharedHockeyManager;
            manager.LogLevel = BITLogLevel.Error;
            manager.Configure(HOCKEYAPPID);
            manager.StartManager();
            manager.CrashManager.CrashManagerStatus = BITCrashManagerStatus.AutoSend;
            manager.Authenticator.AuthenticateInstallation();
            manager.DisableUpdateManager = true;
        }

        private void IosBackgroundService()
        {
            MessagingCenter.Subscribe<PostFeedMessage>(this, "LogonPostFeedToHub", async message =>
            {
                postFeedService = new PostFeedService();
                var currentPost = serviceMapper.Instance.Map<Contract.PostFeedK>(message.CurrentPost);
                var currentUser = serviceMapper.Instance.Map<Contract.ContactK>(message.CurrentUser);
                await postFeedService.Start(currentPost, currentUser, "LogonToHub");
            });

            MessagingCenter.Subscribe<PostFeedMessage>(this, "AddUpdatePostFeedToHub", async message =>
            {
                postFeedService = new PostFeedService();
                var currentPost = serviceMapper.Instance.Map<Contract.PostFeedK>(message.CurrentPost);
                var currentUser = serviceMapper.Instance.Map<Contract.ContactK>(message.CurrentUser);
                await postFeedService.Start(currentPost, currentUser, "AddUpdatePost");
            });

            MessagingCenter.Subscribe<PostFeedMessage>(this, "LikeOrUnlikePostFeedToHub", async message =>
            {
                postFeedService = new PostFeedService();
                var currentPost = serviceMapper.Instance.Map<Contract.PostFeedK>(message.CurrentPost);
                var currentUser = serviceMapper.Instance.Map<Contract.ContactK>(message.CurrentUser);
                await postFeedService.Start(currentPost, currentUser, "LikeOrUnlikePost");
            });

            MessagingCenter.Subscribe<PostFeedMessage>(this, "DeletePostFeedToHub", async message =>
            {
                postFeedService = new PostFeedService();
                var currentPost = serviceMapper.Instance.Map<Contract.PostFeedK>(message.CurrentPost);
                var currentUser = serviceMapper.Instance.Map<Contract.ContactK>(message.CurrentUser);
                await postFeedService.Start(currentPost, currentUser, "DeletePostFeed");
            });
        }

        private void UnsubscribeToMessagingCenter()
        {
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "LogonPostFeedToHub");
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "AddUpdatePostFeedToHub");
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "LikeOrUnlikePostFeedToHub");
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "DeletePostFeedToHub");
        }

        private void KillRunningBackgroundService()
        {
            if (postFeedService != null)
                postFeedService.Stop();
        }

        private void SaveCurrentAppVersionToCache()
        {
            var currentAppVersionFromInfoPList = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();

            if (!string.IsNullOrEmpty(currentAppVersionFromInfoPList))
                _keyValueCacheUtility.GetUserDefaultsKeyValue("AppCurrentVersion", currentAppVersionFromInfoPList);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            container.GetContainer().RegisterType<IDeviceScreenSizeService, DeviceScreenSizeService>();
        }
    }
}

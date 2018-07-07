using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using Android.Content;
using Android.Widget;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using Java.Security;
using Unity;
using Newtonsoft.Json;
using Prism.Unity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Yol.Punla.Barrack;
using Yol.Punla.Droid.CustomRenderers;
using Yol.Punla.Droid.Services;
using Yol.Punla.Droid.Utility;
using Yol.Punla.Localized;
using Yol.Punla.Messages;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;
using Yol.Punla.Views;
using Unity.Lifetime;
using FFImageLoading.Forms.Droid;

namespace Yol.Punla.Droid
{
    [Activity(Theme = "@style/MyTheme.Base", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
      , ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private PrismApplication app;
        private const string PACKAGENAME = "com.haiyangrpdev.HopePH";
        private const string HOCKEYAPPID = "b544024f438f40a482972fa96280e89e"; //chito. when debugging change not to record to the live correct one b544024f438f40a482972fa96280e89e
        private KeyValueCacheUtility keyValueCacheUtility = new KeyValueCacheUtility();
        private string[] masterMenuPages = new string[] { nameof(WikiPage), nameof(HomePage), nameof(PostFeedPage), nameof(CrisisHotlineListPage), nameof(SettingsPage), nameof(NotificationsPage) };

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            CachedImageRenderer.Init(enableFastRenderer: true);
            UserDialogs.Init(this);
            InitHockeyApp();
            GetPackageInfoAndHashKey();
            BackgroundedMessagingCenter();
            app = new App(new AndroidInitializer());
            LoadApplication(app);
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        protected override void OnResume()
        {
            base.OnResume();
            InitHockeyApp2();
        }

        protected override void OnPause()
        {
            base.OnPause();
            HockeyAppUnregister();
        }

        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            RunOnUiThread(() => {

                if (resultCode == Android.App.Result.Ok)
                {
                    var imagePhoto = FindViewById<ImageView>(Resource.Id.imageViewPhoto);

                    if (imagePhoto != null)
                        imagePhoto.SetImageURI(data.Data);

                    NativeFacebookPageRenderer.callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
                }
            });
        }

        protected override void OnDestroy()
        {
            HockeyAppUnregister();
            UnsubscribeToMessagingCenter();

            base.OnDestroy();
        }

        public override void OnBackPressed()
        {
            if (IsHitDeviceBackButton())
                base.OnBackPressed();
        }

        private void InitHockeyApp()
        {
            UpdateManager.Register(this, HOCKEYAPPID);
            MetricsManager.Register(Application, HOCKEYAPPID);
        }

        private void InitHockeyApp2()
        {
            CrashManager.Register(this, HOCKEYAPPID, new CrashListener());
        }

        private void HockeyAppUnregister()
        {
            UpdateManager.Unregister();
        }

        private void GetPackageInfoAndHashKey()
        {
            PackageInfo info = this.PackageManager.GetPackageInfo(PACKAGENAME, PackageInfoFlags.Signatures);

            foreach (Android.Content.PM.Signature signature in info.Signatures)
            {
                MessageDigest md = MessageDigest.GetInstance("SHA");
                md.Update(signature.ToByteArray());

                string keyhash = Convert.ToBase64String(md.Digest());
                keyValueCacheUtility.GetUserDefaultsKeyValue("KeyHashString", keyhash);

                Console.WriteLine("KeyHash: {0}", keyhash);
            }

            if (!string.IsNullOrEmpty(info.VersionName))
                keyValueCacheUtility.GetUserDefaultsKeyValue("AppCurrentVersion", info.VersionName);
        }

        private void BackgroundedMessagingCenter()
        {
            MessagingCenter.Subscribe<PostFeedMessage>(this, "LogonPostFeedToHub", message =>
            {
                ExtendedDataHolder.Instance.Clear();
                ExtendedDataHolder.Instance.PutExtra("CurrentUser", JsonConvert.SerializeObject(message.CurrentUser));
                ExtendedDataHolder.Instance.PutExtra("Operation", "LogonToHub");
                var intent = new Intent(this, typeof(PostFeedService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<PostFeedMessage>(this, "AddUpdatePostFeedToHub", message =>
            {
                ExtendedDataHolder.Instance.Clear();
                ExtendedDataHolder.Instance.PutExtra("CurrentUser", JsonConvert.SerializeObject(message.CurrentUser));
                ExtendedDataHolder.Instance.PutExtra("CurrentPost", JsonConvert.SerializeObject(message.CurrentPost));
                ExtendedDataHolder.Instance.PutExtra("Operation", "AddUpdatePost");
                var intent = new Intent(this, typeof(PostFeedService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<PostFeedMessage>(this, "LikeOrUnlikePostFeedToHub", message =>
            {
                ExtendedDataHolder.Instance.Clear();
                ExtendedDataHolder.Instance.PutExtra("CurrentUser", JsonConvert.SerializeObject(message.CurrentUser));
                ExtendedDataHolder.Instance.PutExtra("CurrentPost", JsonConvert.SerializeObject(message.CurrentPost));
                ExtendedDataHolder.Instance.PutExtra("Operation", "LikeOrUnlikePost");
                var intent = new Intent(this, typeof(PostFeedService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<PostFeedMessage>(this, "DeletePostFeedToHub", message =>
            {
                ExtendedDataHolder.Instance.Clear();
                ExtendedDataHolder.Instance.PutExtra("CurrentUser", JsonConvert.SerializeObject(message.CurrentUser));
                ExtendedDataHolder.Instance.PutExtra("CurrentPost", JsonConvert.SerializeObject(message.CurrentPost));
                ExtendedDataHolder.Instance.PutExtra("Operation", "DeletePostFeed");
                var intent = new Intent(this, typeof(PostFeedService));
                StartService(intent);
            });
        }

        private void UnsubscribeToMessagingCenter()
        {
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "LogonPostFeedToHub");
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "AddUpdatePostFeedToHub");
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "LikeOrUnlikePostFeedToHub");
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "DeletePostFeedToHub");
        }

        private bool IsHitDeviceBackButton()
        {
            var navigationStackService = AppUnityContainer.Instance.Resolve<INavigationStackService>();
            bool hitTheButton = true;

            foreach (var item in masterMenuPages)
            {
                if (navigationStackService.CurrentStack.Equals(item))
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetMessage(AppStrings.HamburgerMenuClicked);
                    alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                    hitTheButton = false;
                }
            }

            if (hitTheButton)
            {
                var toBeRemovedPage = navigationStackService.CurrentStack;
                navigationStackService.RemovePageFromNavigationStack(toBeRemovedPage);
            }

            return hitTheButton;
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            container.GetContainer().RegisterType<IDeviceScreenSizeService, DeviceScreenSizeService>(new ContainerControlledLifetimeManager());
        }
    }
}


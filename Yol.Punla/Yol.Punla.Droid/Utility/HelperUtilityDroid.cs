using Android.Content.PM;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Yol.Punla.Utility;
using DroidApp = Android.App;

[assembly: Dependency(typeof(Yol.Punla.Droid.Utility.HelperUtilityDroid))]
namespace Yol.Punla.Droid.Utility
{
    public class HelperUtilityDroid : IHelperUtility
    {
        private const string PACKAGENAME = "com.haiyangrpdev.HopePH";
        private KeyValueCacheUtility keyValueCacheUtility = new KeyValueCacheUtility();

        public async Task<bool> CheckIfPermissionToLocationGranted()
        {
            var permissionUtility = new PermissionUtility(DroidApp.Application.Context);
            bool allowedToKnowLocation = await permissionUtility.CheckIfHasLocationPermission();

            if (!allowedToKnowLocation)
                allowedToKnowLocation = await permissionUtility.RequestLocationPermissionAsync();

            return allowedToKnowLocation;
        }

        public bool ResetPackageVersionNo()
        {
            try
            {
                PackageInfo info = DroidApp.Application.Context.PackageManager.GetPackageInfo(PACKAGENAME, PackageInfoFlags.Signatures);

                if (!string.IsNullOrEmpty(info.VersionName))
                {
                    keyValueCacheUtility.RemoveKeyObject("AppCurrentVersion");
                    keyValueCacheUtility.GetUserDefaultsKeyValue("AppCurrentVersion", info.VersionName);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //do nothing
            }

            return false;
        }
    }
}
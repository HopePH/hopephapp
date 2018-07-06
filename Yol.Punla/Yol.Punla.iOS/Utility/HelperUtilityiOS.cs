using Foundation;
using System.Threading.Tasks;
using Xamarin.Forms;
using Yol.Punla.Utility;

[assembly: Dependency(typeof(Yol.Punla.iOS.Utility.HelperUtilityiOS))]
namespace Yol.Punla.iOS.Utility
{
    public class HelperUtilityiOS : IHelperUtility
    {
        private readonly KeyValueCacheUtility _keyValueCacheUtility = new KeyValueCacheUtility();

        public Task<bool> CheckIfPermissionToLocationGranted() => Task.FromResult(true);

        public bool ResetPackageVersionNo()
        {
            var currentAppVersionFromInfoPList = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();

            if (!string.IsNullOrEmpty(currentAppVersionFromInfoPList))
            {
                _keyValueCacheUtility.GetUserDefaultsKeyValue("AppCurrentVersion", currentAppVersionFromInfoPList);
                return true;
            }

            return false;
        }
    }
}
using Foundation;
using Xamarin.Forms;
using Yol.Punla.Utility;

[assembly: Dependency(typeof(Yol.Punla.iOS.Utility.KeyValueCacheUtility))]
namespace Yol.Punla.iOS.Utility
{
    public class KeyValueCacheUtility : IKeyValueCacheUtility
    {
        public string GetUserDefaultsKeyValue(string key, string setInitialValueWhenNull = "")
        {
            string value = NSUserDefaults.StandardUserDefaults.StringForKey(key);

            if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(setInitialValueWhenNull))
            {
                setInitialValueWhenNull = setInitialValueWhenNull.Replace(":", "|");
                NSUserDefaults.StandardUserDefaults.SetString(setInitialValueWhenNull, key);
                NSUserDefaults.StandardUserDefaults.Synchronize();
                return "";
            }
            else
                return value;
        }

        public void RemoveKeyObject(string key)
        {
            NSUserDefaults.StandardUserDefaults.RemoveObject(key);
        }
    }
}

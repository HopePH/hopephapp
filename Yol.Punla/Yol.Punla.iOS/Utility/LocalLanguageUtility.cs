using Foundation;
using System.Diagnostics;
using Xamarin.Forms;
using Yol.Punla.Utility;

[assembly: Dependency(typeof(Yol.Punla.iOS.Utility.LocalLanguageUtility))]
namespace Yol.Punla.iOS.Utility
{
    public class LocalLanguageUtility : ILocalLanguageUtility
    {
        public string GetLanguageLocale()
        {
            Debug.WriteLine("HOPEPH Getting local language from IOS.");
            return NSLocale.CurrentLocale.LocaleIdentifier;
        }
    }
}
using UIKit;
using Xamarin.Forms;
using Yol.Punla.Utility;

[assembly: Dependency(typeof(Yol.Punla.iOS.Utility.KeyboardHelperiOS))]
namespace Yol.Punla.iOS.Utility
{
    public class KeyboardHelperiOS : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }
}
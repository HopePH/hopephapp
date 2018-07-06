using Android.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.Localized;

[assembly: ExportEffect(typeof(Yol.Punla.Droid.Effects.EditorTransparentBottomlineEffectDroid), "EditorTransparentBottomlineEffect")]
namespace Yol.Punla.Droid.Effects
{
    public class EditorTransparentBottomlineEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                if (Control == null) return;

                var nativeControl = (EditText)Control;
                nativeControl.SetBackgroundResource(Resource.Drawable.TransparentBottomLine);
                nativeControl.Hint = AppStrings.SharePostText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            
        }
    }
}
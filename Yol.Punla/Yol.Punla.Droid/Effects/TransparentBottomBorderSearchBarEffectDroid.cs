using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.Droid.Effects;

[assembly: ExportEffect(typeof(TransparentBottomBorderSearchBarEffectDroid), "TransparentBottomBorderSearchBarEffect")]
namespace Yol.Punla.Droid.Effects
{
    public class TransparentBottomBorderSearchBarEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                if (Control is null) return;

                Control.SetBackgroundResource(Resource.Drawable.SearchBarNoBottomLine);

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
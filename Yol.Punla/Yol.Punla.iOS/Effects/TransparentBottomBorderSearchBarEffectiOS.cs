using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.iOS.Effects;

[assembly: ExportEffect(typeof(TransparentBottomBorderSearchBarEffectiOS), "TransparentBottomBorderSearchBarEffect")]
namespace Yol.Punla.iOS.Effects
{
    public class TransparentBottomBorderSearchBarEffectiOS : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
              
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
using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.Effects;
using Yol.Punla.iOS.Effects;
using static Yol.Punla.Effects.FontIconEffect;

[assembly: ExportEffect(typeof(FontIconEffectIos), nameof(FontIconEffect))]
namespace Yol.Punla.iOS.Effects
{
    public class FontIconEffectIos : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var xfControl = Element as Label;

                var control = Control as UILabel;
                if (control == null) return;
                var effect = (FontIconEffect)Element.Effects.FirstOrDefault(e => e is FontIconEffect);

                if (effect != null)
                {
                    if (effect.FontWeight?.ToLower() == "bold")
                        control.Font = UIFont.BoldSystemFontOfSize((float)xfControl.FontSize);

                    switch (effect.IconType)
                    {
                        case FontAwesomeType.SOLID:
                            control.Font = UIFont.FromName("FontAwesome5FreeSolid", (System.nfloat)xfControl.FontSize);
                            break;
                        case FontAwesomeType.REGULAR:
                            control.Font = UIFont.FromName("FontAwesome5FreeRegular", (System.nfloat)xfControl.FontSize);
                            break;
                        default:
                            control.Font = UIFont.FromName("FontAwesomeRegular.ttf", (System.nfloat)xfControl.FontSize);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void OnDetached() { }
    }
}
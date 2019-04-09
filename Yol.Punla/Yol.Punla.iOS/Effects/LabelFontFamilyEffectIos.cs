using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.Effects;

[assembly: ExportEffect(typeof(Yol.Punla.iOS.Effects.LabelFontFamilyEffectIos), nameof(LabelFontFamilyEffect))]
namespace Yol.Punla.iOS.Effects
{
    public class LabelFontFamilyEffectIos : PlatformEffect
    {
        protected override void OnAttached()
        {
            var xfControl = Element as Label;
            var control = Control as UILabel;
            if (control == null) return;
            var effect = (LabelFontFamilyEffect)xfControl.Effects.FirstOrDefault(e => e is LabelFontFamilyEffect);

            if (effect != null)
            {
                if (xfControl.TextColor != null) control.TextColor = xfControl.TextColor.ToUIColor();

                if (effect.FontFamily == LabelFontFamilyEffect.FontType.FONTAWESOME)
                {
                    switch (effect.FontIconType)
                    {
                        case LabelFontFamilyEffect.IconType.SOLID:
                            control.Font = UIFont.FromName("FontAwesome5FreeSolid", (nfloat)xfControl.FontSize);
                            break;
                        case LabelFontFamilyEffect.IconType.REGULAR:
                            control.Font = UIFont.FromName("FontAwesome5FreeRegular", (nfloat)xfControl.FontSize);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (xfControl.FontAttributes)
                    {
                        case FontAttributes.Bold:
                            control.Font = UIFont.FromName("MuseoSans-700", (float)xfControl.FontSize);
                            break;
                        case FontAttributes.Italic:
                            control.Font = UIFont.FromName("MuseoSans-300Italic", (float)xfControl.FontSize);
                            break;
                        default:
                            control.Font = UIFont.FromName("MuseoSansCyrl-300", (float)xfControl.FontSize);
                            break;
                    }
                }
            }
        }

        protected override void OnDetached() { }
    }
}
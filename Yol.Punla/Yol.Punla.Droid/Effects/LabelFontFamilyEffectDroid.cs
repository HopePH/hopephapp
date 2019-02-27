using Android.Graphics;
using Android.Widget;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.Droid.Effects;
using Yol.Punla.Effects;
using static Yol.Punla.Effects.LabelFontFamilyEffect;

[assembly: ExportEffect(typeof(LabelFontFamilyEffectDroid), nameof(LabelFontFamilyEffect))]
namespace Yol.Punla.Droid.Effects
{
    public class LabelFontFamilyEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var control = Control as TextView;
                if (control == null) return;

                var xfControl = Element as Label;

                var effect = (LabelFontFamilyEffect)xfControl.Effects.FirstOrDefault(e => e is LabelFontFamilyEffect);
                if (effect != null)
                {
                    var textColor = xfControl.TextColor.ToAndroid();
                    control.SetTextColor(textColor);

                    if (effect.FontFamily == FontType.FONTAWESOME)
                    {
                        switch (effect.FontIconType)
                        {
                            case IconType.SOLID:
                                control.Typeface = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, "FontAwesomeSolid.otf");
                                break;
                            case IconType.REGULAR:
                                control.Typeface = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, "FontAwesomeRegular.otf");
                                break;
                            default:
                                break;
                        } 
                    }
                    else
                    {
                        var font = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, "MuseoSansCyrl_0.otf");

                        if (xfControl.FontAttributes == FontAttributes.Bold)
                            font = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, "MuseoSansBold.otf");
                        else if (xfControl.FontAttributes == FontAttributes.Italic)
                            font = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, "MuseoSans-300Italic.otf");
                        else
                            font = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, "MuseoSansCyrl_0.otf");

                        control.Typeface = font;
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        protected override void OnDetached()
        {
            
        }
    }
}
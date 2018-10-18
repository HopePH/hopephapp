using Android.Graphics;
using Android.Widget;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.Droid.Effects;
using Yol.Punla.Effects;
using static Yol.Punla.Effects.FontIconEffect;

[assembly: ExportEffect(typeof(FontIconEffectDroid), nameof(FontIconEffect))]
namespace Yol.Punla.Droid.Effects
{
    public class FontIconEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            var control = Control as TextView;
            if (control == null) return;

            var effect = (FontIconEffect)Element.Effects.FirstOrDefault(e => e is FontIconEffect);

            if (effect != null)
            {
                if (effect.FontWeight?.ToLower() == "bold")
                    control.SetTypeface(null, TypefaceStyle.Bold);

                if (!string.IsNullOrEmpty(effect?.HexColor))
                    control.SetTextColor(Xamarin.Forms.Color.FromHex(effect.HexColor).ToAndroid());

                switch (effect.IconType)
                {
                    case FontAwesomeType.SOLID:
                        control.Typeface = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, "FontAwesomeSolid.otf");
                        break;
                    case FontAwesomeType.REGULAR:
                        control.Typeface = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, "FontAwesomeRegular.otf");
                        break;
                    default:
                        control.Typeface = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, "fontawesome-webfont.ttf");
                        break;
                }
            }
        }

        protected override void OnDetached() { }
    }
}
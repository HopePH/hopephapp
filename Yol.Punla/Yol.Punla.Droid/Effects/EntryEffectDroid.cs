using System.Linq;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.Effects;

[assembly: ExportEffect(typeof(Yol.Punla.Droid.Effects.EntryEffectDroid), nameof(EntryEffect))]
namespace Yol.Punla.Droid.Effects
{
    public class EntryEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            var control = Control as EditText;
            if (control == null) return;
            var effect = (EntryEffect)Element.Effects.FirstOrDefault(e => e is EntryEffect);

            if (effect != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                control.SetBackground(gd);
                control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
            }
        }

        protected override void OnDetached()
        {

        }
    }
}
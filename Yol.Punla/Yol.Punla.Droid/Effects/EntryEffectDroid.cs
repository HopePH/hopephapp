using Android.Graphics.Drawables;
using Android.Widget;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.Effects;

[assembly: ExportEffect(typeof(Yol.Punla.Droid.Effects.EntryEffectDroid), nameof(InputViewEffect))]
namespace Yol.Punla.Droid.Effects
{
    public class EntryEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var control = Control as EditText;
                if (control == null) return;
                var effect = (InputViewEffect)Element.Effects.FirstOrDefault(e => e is InputViewEffect);

                if (effect != null)
                {
                    if (effect.NumberOfLines > 0) control.SetMaxLines(effect.NumberOfLines);
                    control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    if (!effect.IsApplyToDroid) return;
                    control.SetBackgroundResource(Resource.Drawable.bottom_line_entry);
                    var itemList = (LayerDrawable)control.Background;
                    GradientDrawable bgShape = (GradientDrawable)itemList.FindDrawableByLayerId(Resource.Id.shape_id);
                    bgShape.SetStroke(effect.Thickness, effect.LineColor.ToAndroid());
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnDetached() { }
    }
}
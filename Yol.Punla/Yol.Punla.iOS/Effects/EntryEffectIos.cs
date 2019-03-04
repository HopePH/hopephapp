using CoreAnimation;
using CoreGraphics;
using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.Effects;

[assembly: ExportEffect(typeof(Yol.Punla.iOS.Effects.EntryEffectIos), nameof(EntryEffect))]
namespace Yol.Punla.iOS.Effects
{
    public class EntryEffectIos : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var xfControl = Element as Entry;
                if (xfControl == null) return;
                var control = Control as UITextField;
                if (control == null) return;

                var effect = (EntryEffect)xfControl.Effects.FirstOrDefault(e => e is EntryEffect);
                if (effect != null)
                {
                    xfControl.SizeChanged += (ctrl, ea) => SetupEffect(effect);
                    if (effect.IsBindableAttached) SetupEffect(effect);
                }
            }
            catch (Exception ex)
            {
              
            }
        }

    
        protected override void OnDetached() { }

        private void SetupEffect(EntryEffect effect)
        {
            var control = Control as UITextField;
            var xfControl = Element as Entry;

            CALayer border = new CALayer();
            float width = 1.0f;
            border.BorderColor = effect.LineColor.ToUIColor().CGColor;
            border.Frame = new CGRect(x: 0, y: ((float)xfControl.Height - width) + effect.PlaceholderToLineDistance, width: (float)xfControl.Width, height: effect.Thickness);
            border.BorderWidth = width;

            control.Layer.AddSublayer(border);
            control.Layer.MasksToBounds = true;
            control.BorderStyle = UITextBorderStyle.None;
            control.BackgroundColor = xfControl.BackgroundColor.ToUIColor();
        }
    }
}
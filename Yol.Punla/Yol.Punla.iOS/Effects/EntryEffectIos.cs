using CoreAnimation;
using CoreGraphics;
using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.Effects;

[assembly: ExportEffect(typeof(Yol.Punla.iOS.Effects.InputViewEffectIos), nameof(InputViewEffect))]
namespace Yol.Punla.iOS.Effects
{
    public class InputViewEffectIos : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var xfControl = Element as InputView;
                if (xfControl == null) return;
                var control = Control as UIView;
                if (control == null) return;

                var effect = (InputViewEffect)xfControl.Effects.FirstOrDefault(e => e is InputViewEffect);
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

        private void SetupEffect(InputViewEffect effect)
        {
            var control = Control as UIView;
            var xfControl = Element as InputView;

            CALayer border = new CALayer();
            float width = 1.0f;
            border.BorderColor = effect.LineColor.ToUIColor().CGColor;
            border.Frame = new CGRect(x: 0, y: ((float)xfControl.Height - width) + effect.PlaceholderToLineDistance, width: (float)xfControl.Width, height: effect.Thickness);
            border.BorderWidth = width;

            control.Layer.AddSublayer(border);
            control.Layer.MasksToBounds = true;
            if (Control is UITextView)
            {
                control.Layer.BorderWidth = 0;
                control.Layer.BorderColor = Color.Transparent.ToCGColor();
            }
            else if(Control is UITextField)
            {
                var textField = Control as UITextField;
                textField.BorderStyle = UITextBorderStyle.None;
            }
                
            control.BackgroundColor = xfControl.BackgroundColor.ToUIColor();
        }
    }
}
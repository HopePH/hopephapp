using CoreAnimation;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.Localized;

[assembly: ResolutionGroupName("Yol.Punla.Effects")]
[assembly: ExportEffect(typeof(Yol.Punla.iOS.Effects.EditorTransparentBottomlineEffectiOS), "EditorTransparentBottomlineEffect")]
namespace Yol.Punla.iOS.Effects
{
    public class EditorTransparentBottomlineEffectiOS : PlatformEffect
    {
        private UITextView nativeControl;
        private string PlaceHolderText = AppStrings.SharePostText;

        protected override void OnAttached()
        {
            try
            {
                if (Control == null) return;
                nativeControl = (UITextView)Control;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }
        
        protected override void OnDetached() { }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            CALayer layer = new CALayer
            {
                BackgroundColor = Color.Transparent.ToCGColor(),
                Frame = new CoreGraphics.CGRect(0.0, nativeControl.Frame.Y + 35.0, nativeControl.Frame.Width, 1.0)
            };
            
            this.Control.Layer.AddSublayer(layer);
        }

    }
}
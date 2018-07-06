using Android.Graphics;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.Droid.Effects;

[assembly: ExportEffect(typeof(SwitchColorEffectDroid), "SwitchColorEffect")]
namespace Yol.Punla.Droid.Effects
{
    public class SwitchColorEffectDroid : PlatformEffect
    {
        Android.Support.V7.Widget.SwitchCompat NativeControl;

        protected override void OnAttached()
        {
            try
            {
                if (Control == null) return;

                NativeControl = (Android.Support.V7.Widget.SwitchCompat)Control;
                
                UpdateSwitchState();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void OnDetached() { }
        
        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            
            NativeControl.CheckedChange += (sender, e) => UpdateSwitchState();
        }

        private void UpdateSwitchState()
        {
            if (NativeControl.Checked)
                NativeControl.ThumbDrawable.SetColorFilter(Android.Graphics.Color.Rgb(136, 176, 75), PorterDuff.Mode.SrcAtop);
            else
                NativeControl.ThumbDrawable.SetColorFilter(Android.Graphics.Color.Gray, PorterDuff.Mode.SrcAtop);

            ((Xamarin.Forms.Switch)Element).IsToggled = NativeControl.Checked;
        }
    }
}
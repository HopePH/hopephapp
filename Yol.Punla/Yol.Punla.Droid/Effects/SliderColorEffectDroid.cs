using Android.Graphics;
using Android.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

// reference : https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/effects/creating/
[assembly: ResolutionGroupName("Yol.Punla.Effects")]
[assembly: ExportEffect(typeof(Yol.Punla.Droid.Effects.SliderColorEffectDroid), "SliderColorEffect")]
namespace Yol.Punla.Droid.Effects
{
    public class SliderColorEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                if (Control == null) return;

                var nativeSlider = (SeekBar)Control;
                nativeSlider.ProgressDrawable.SetColorFilter(Android.Graphics.Color.Rgb(136, 176, 75), PorterDuff.Mode.SrcIn);
                nativeSlider.Thumb.SetColorFilter(Android.Graphics.Color.Rgb(136, 176, 75), PorterDuff.Mode.SrcIn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            
        }
    }
}
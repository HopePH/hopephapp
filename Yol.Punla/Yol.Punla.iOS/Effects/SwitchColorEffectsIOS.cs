using System.ComponentModel;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Yol.Punla.iOS.Effects
{
    public class SwitchColorEffectsIOS : PlatformEffect
    {

        UISwitch nativeSwitch;
        Xamarin.Forms.Switch xfSwitch;

        protected override void OnAttached()
        {
            nativeSwitch = (UISwitch)Control;
            xfSwitch = (Xamarin.Forms.Switch)Element;

            if (nativeSwitch != null)
            {
                UISwitch.Appearance.OnTintColor = UIColor.FromRGB(136, 176, 75);

                UpdateSwitchState();
            }
        }

        protected override void OnDetached() { }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            nativeSwitch.ValueChanged += (sender, e) => UpdateSwitchState();
        }

        private void UpdateSwitchState() => xfSwitch.IsToggled = nativeSwitch.On;
    }
}
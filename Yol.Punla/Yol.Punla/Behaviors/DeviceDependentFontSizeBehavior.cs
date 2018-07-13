using System;
using Unity;
using Xamarin.Forms;
using Yol.Punla.Barrack;
using Yol.Punla.Utility;

namespace Yol.Punla.Behaviors
{
    public class DeviceDependentFontSizeBehavior : Behavior<Label>
    {
        private const int IPhoneWidthInPixels = 375;

        public double DynamicFontSize { get; set; }

        protected override void OnAttachedTo(Label bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.BindingContextChanged += Bindable_BindingContextChanged;
        }
        
        protected override void OnDetachingFrom(Label bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            var deviceScreenSizeService = AppUnityContainer.Instance.Resolve<IDeviceScreenSizeService>();
            
            if(Device.RuntimePlatform == Device.iOS)
            {
                var deviceWidth = deviceScreenSizeService.GetWidth();

                if(deviceWidth < IPhoneWidthInPixels)
                    ((Label)sender).FontSize = DynamicFontSize;
            }
        }
    }
}

using Unity;
using System;
using Xamarin.Forms;
using Yol.Punla.Barrack;
using Yol.Punla.Utility;

namespace Yol.Punla.Behaviors
{
    public class DeviceDependentViewSizeBehavior : Behavior<View>
    {
        public double DynamicWidthRequest { get; set; }
        public double DynamicHeightRequest { get; set; }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SizeChanged += OnViewSizeChange;
        }
        
        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.SizeChanged -= OnViewSizeChange;
        }

        private void OnViewSizeChange(object sender, EventArgs e)
        {
            var view = (View)sender;

            if(Device.RuntimePlatform == Device.iOS)
            {
                // Get Device Width
                var container = AppUnityContainer.Instance.Resolve<IDeviceScreenSizeService>();
                var deviceWidth = container.GetWidth();

                // If device width is less than 375, then it is iPhone 5s and older
                if (deviceWidth < 375)
                {
                    if (DynamicWidthRequest != 0 && view.WidthRequest != DynamicWidthRequest)
                        view.WidthRequest = DynamicWidthRequest;

                    if (DynamicHeightRequest != 0 && view.HeightRequest != DynamicHeightRequest)
                        view.HeightRequest = DynamicHeightRequest;
                }
            }
        }
    }
}

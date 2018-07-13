using System;
using Unity;
using Xamarin.Forms;
using Yol.Punla.Barrack;
using Yol.Punla.Utility;

namespace Yol.Punla.Behaviors
{
    public class DeviceDependentViewBoxBehavior : Behavior<Layout>
    {
        public Thickness DynamicMargin { get; set; }
        public Thickness DynamicPadding { get; set; }

        protected override void OnAttachedTo(Layout bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SizeChanged += Bindable_SizeChanged;
        }

        protected override void OnDetachingFrom(Layout bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.SizeChanged -= Bindable_SizeChanged;
        }

        private void Bindable_SizeChanged(object sender, EventArgs e)
        {
            var layout = (Layout)sender;

            try
            {
                var deviceWidth = (AppUnityContainer.Instance.Resolve<IDeviceScreenSizeService>()).GetWidth();

                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (deviceWidth < 375)
                    {
                        if (DynamicPadding != null && layout.Padding != DynamicPadding)
                            layout.Padding = DynamicPadding;

                        if (DynamicPadding != null && layout.Margin != DynamicMargin)
                            layout.Margin = DynamicMargin;
                    }
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    //prepare here soon...
                }
            }
            catch (Exception) { }
        }
    }
}

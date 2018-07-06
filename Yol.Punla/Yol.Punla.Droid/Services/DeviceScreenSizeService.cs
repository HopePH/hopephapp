using Plugin.CurrentActivity;
using Yol.Punla.Utility;

namespace Yol.Punla.Droid.Services
{
    public class DeviceScreenSizeService : IDeviceScreenSizeService
    {
        public double GetWidth() => CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.WidthPixels;

        public double GetHeight() => CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.HeightPixels;
    }
}
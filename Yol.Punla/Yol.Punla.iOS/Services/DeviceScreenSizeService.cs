using UIKit;
using Yol.Punla.Utility;

namespace Yol.Punla.iOS.Services
{
    public class DeviceScreenSizeService : IDeviceScreenSizeService
    {
        public double GetHeight() => UIScreen.MainScreen.Bounds.Height;

        public double GetWidth() => UIScreen.MainScreen.Bounds.Width;
    }
}
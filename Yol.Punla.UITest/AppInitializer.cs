using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Yol.Punla.UITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                //chito. since xcode update, Xamarin.UITest, Xamarin.TestCloud.Agent it cannot see the below file so need to have the full path instead
                // .AppBundle("../../../Yol.Punla/Yol.Punla.iOS/bin/iPhoneSimulator/Debug/device-builds/iphone10.5-11.0/YolPunlaiOS.app")
                .AppBundle("/Chito.Files/_Work.Dir.RBF/_RentABoyfriend.Mobile/Yol.Punla/Yol.Punla.iOS/bin/iPhoneSimulator/Debug/device-builds/iphone10.5-11.0/YolPunlaiOS.app")
                .StartApp();
        }
    }
}



using Android.App;
using Android.Content;
using Android.OS;

namespace Yol.Punla.Droid
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, Label = "HopePH", NoHistory = false)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}
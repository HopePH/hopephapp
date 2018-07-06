using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.Views;

[assembly: ExportRenderer(typeof(AppViewBase), typeof(Yol.Punla.iOS.CustomRenderers.AppViewBaseRenderer))]
namespace Yol.Punla.iOS.CustomRenderers
{
    internal class AppViewBaseRenderer : PageRenderer
    {
        public override void ViewDidLoad()
        {
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            base.ViewDidLoad();
        }
    }
}
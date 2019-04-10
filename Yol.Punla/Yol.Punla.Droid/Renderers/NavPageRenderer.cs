using Android.Content;
using Prism.Services;
using System;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Yol.Punla.Barrack;
using Yol.Punla.Utility;
using Yol.Punla.Views;

[assembly: ExportRenderer(typeof(NavPage), typeof(SAEventApp.Droid.Renderers.NavPageRenderer))]
namespace SAEventApp.Droid.Renderers
{
    public class NavPageRenderer : NavigationPageRenderer
    {
        public NavPageRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);
            if (Element == null) return;

            if (e.NewElement != null)
            {
                ChangeNavBarProperties();
                e.NewElement.PropertyChanged += NewElement_PropertyChanged;
            }
        }

        private void NewElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Toggle")
                ChangeNavBarProperties();
        }

        private void ChangeNavBarProperties()
        {
            try
            {
                var navBar = Element as NavigationPage;
                navBar.BarTextColor = Color.FromHex("FFFFFF00");
                navBar.BarBackgroundColor = Color.FromHex("FFFFFFFF");
            }
            catch (Exception ex)
            {
                AppUnityContainer.Instance.Resolve<IDependencyService>().Get<ILogger>().Log(ex);
            }
        }
    }
}
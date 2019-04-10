using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Prism.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.Barrack;
using Yol.Punla.Utility;
using Yol.Punla.Views;
using Unity;

[assembly: ExportRenderer(typeof(NavPage), typeof(SAEventApp.iOS.Renderers.NavPageRenderer))]
namespace SAEventApp.iOS.Renderers
{
    public class NavPageRenderer : NavigationRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ChangeNavBarProperties();
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null) e.NewElement.PropertyChanged += NewElement_PropertyChanged;
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

                var gradientLayer = new CAGradientLayer();
                gradientLayer.Bounds = NavigationBar.Bounds;
                gradientLayer.Colors = new CGColor[] { Color.FromHex("FFFFFF").ToCGColor() };
                gradientLayer.StartPoint = new CGPoint(0.0, 0.5);
                gradientLayer.EndPoint = new CGPoint(1.0, 0.5);

                UIGraphics.BeginImageContext(gradientLayer.Bounds.Size);
                gradientLayer.RenderInContext(UIGraphics.GetCurrentContext());
                UIImage image = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                //var textColor = (savedTheme == "#FFFFFFFF") ? Color.FromHex("#45545F").ToUIColor() : Color.White.ToUIColor();
                var textColor = Color.FromHex("FF417505").ToUIColor();
                NavigationBar.SetBackgroundImage(image, UIBarMetrics.Default);
                NavigationBar.TitleTextAttributes = new UIStringAttributes
                {
                    ForegroundColor = textColor
                };

                //just uniform color of elements on navigation because of native ios bug https://stackoverflow.com/questions/45893605/changing-the-tint-colour-of-elements-in-uinavigationbar-ios-11/46033038#46033038
                var toolbarItemColor = Color.FromHex("#FF417505").ToUIColor(); 
                UIBarButtonItem.Appearance.TintColor = toolbarItemColor;
                UIToolbar.Appearance.TintColor = toolbarItemColor;
                NavigationBar.TintColor = toolbarItemColor;
                UINavigationBar.Appearance.TintColor = toolbarItemColor;
                UIButton.AppearanceWhenContainedIn(typeof(UINavigationBar)).TintColor = toolbarItemColor;
                UIButton.Appearance.TintColor = toolbarItemColor;
                UIButton.AppearanceWhenContainedIn(typeof(UINavigationBar)).TintColor = toolbarItemColor;
                NavigationBar.ShadowImage = new UIImage();
            }
            catch (Exception ex)
            {
                AppUnityContainer.Instance.Resolve<IDependencyService>().Get<ILogger>().Log(ex);
            }
        }
    }
}
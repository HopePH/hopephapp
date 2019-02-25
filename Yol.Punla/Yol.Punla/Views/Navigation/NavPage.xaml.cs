using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Unity;
using Yol.Punla.NavigationHeap;
using Yol.Punla.ViewModels;

namespace Yol.Punla.Views
{
    [ModuleView]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavPage : NavigationPage
    {
        public NavPage()
        {
            try
            {
                InitializeComponent();
                Popped += delegate (object sender, NavigationEventArgs e)
                {
                    try
                    {
                        if (!AppUnityContainer.Instance.Resolve<INavigationStackService>().IsDisableNavPagePop)
                            ((NavPageViewModel)BindingContext).TapNavigationBackCommand.Execute(null);
                    }
                    catch { }
                };
            }
            catch (XamlParseException xp)
            {
                if (!xp.Message.Contains("StaticResource not found for key"))
                    throw;
            }
            catch (Exception ex)
            {
                if (!(ex.Source == "FFImageLoading.Forms" || ex.Source == "FFImageLoading.Transformations"))
                    throw;
            }
        }
    }
}
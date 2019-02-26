using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yol.Punla.Barrack;
using Unity;
using Yol.Punla.AttributeBase;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using System;

namespace Yol.Punla.Views
{
    [ModuleView]
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainTabbedPage : Xamarin.Forms.TabbedPage, INavigatingAware
    {
		public MainTabbedPage ()
		{
            try
            {
                InitializeComponent();
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
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

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            System.Diagnostics.Debug.WriteLine($"{Title} OnNavigatingTo");
            var tabs = parameters.GetValues<string>("addTab");

            foreach (var name in tabs)
                AddChild(name, parameters);
        }

        private void AddChild(string name, INavigationParameters parameters)
        {
            var page = AppUnityContainer.Instance.Resolve<object>(name) as Page;
            if (ViewModelLocator.GetAutowireViewModel(page) == null) ViewModelLocator.SetAutowireViewModel(page, true);
            (page as INavigatingAware)?.OnNavigatingTo(parameters);
            (page?.BindingContext as INavigatingAware)?.OnNavigatingTo(parameters);
            Children.Add(page);
        }
    }
}
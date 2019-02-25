using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yol.Punla.Barrack;
using Unity;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Views
{
    [ModuleView]
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainTabbedPage : TabbedPage, INavigatingAware
    {
		public MainTabbedPage ()
		{
			InitializeComponent ();
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
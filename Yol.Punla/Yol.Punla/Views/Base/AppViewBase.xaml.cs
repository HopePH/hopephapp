using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yol.Punla.AttributeBase;
using Yol.Punla.ViewModels;

namespace Yol.Punla.Views
{
    [ModuleIgnore]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppViewBase : ContentPage, IDestructible
    {
        private IViewEventsListener _viewEventsListener;
        private const bool HASNAVBAR = false;

        private bool _turnOnKeyboardAware = false;
		public bool TurnOnKeyboardAware { get { return _turnOnKeyboardAware;} set { _turnOnKeyboardAware = value;} }

        public AppViewBase() => NavigationPage.SetHasNavigationBar(this, HASNAVBAR);

        public void Destroy()
        {
            DetachedPageEvents();
            UnSubcribeMessagingCenter();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                ResetNavigationPageSettings();
                AttachedPageEvents();
                _viewEventsListener = BindingContext as IViewEventsListener;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SubcribeMessagingCenter();

            if (_viewEventsListener != null)
                _viewEventsListener.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UnSubcribeMessagingCenter();

            if (_viewEventsListener != null)
                _viewEventsListener.OnDisappearing();
        }

        protected virtual void ResetNavigationPageSettings() => NavigationPage.SetHasNavigationBar(this, HASNAVBAR);

        protected virtual void AttachedPageEvents() { }

        protected virtual void DetachedPageEvents() { }

        protected virtual void SubcribeMessagingCenter() { }

        protected virtual void UnSubcribeMessagingCenter() { }
    }
}

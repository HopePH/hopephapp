using Prism.Navigation;
using Xamarin.Forms;

namespace Yol.Punla.BugXamarin
{
    public static class NavigationBug
    {
        public static void SetAbsoluteURI(INavigationService navigationService, string rootPage, NavigationParameters parameters = null
           , bool? useModalNavigation = null, bool animated = true)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    for (int i = 0; i < 2; ++i)
                        navigationService.NavigateAsync(rootPage, parameters, useModalNavigation, animated);
                    break;
                case Device.Android:
                case Device.UWP:
                default:
                    navigationService.NavigateAsync(rootPage, parameters, useModalNavigation, animated);
                    break;
            }
        }
    }
}

using Prism.Navigation;

namespace Yol.Punla.Barrack
{
    public interface IAppNavigationAware
    {
        void OnAppNavigatedFrom(NavigationParameters parameters);
        void OnAppNavigatedTo(NavigationParameters parameters, bool shouldInitialize);
    }
}

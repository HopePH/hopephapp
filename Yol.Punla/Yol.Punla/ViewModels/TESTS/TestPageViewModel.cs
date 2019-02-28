using Prism.Events;
using Prism.Navigation;
using Yol.Punla.Barrack;
using CONSTANTS = Yol.Punla.Barrack.Constants;

namespace Yol.Punla.ViewModels
{
    public class TestPageViewModel : ChildViewModelBase
    {
        public string CurrentIcon => CONSTANTS.TABITEM_HEADSET;

        public TestPageViewModel(IEventAggregator eventAggregator, IServiceMapper serviceMapper, IAppUser appUser, INavigationService navigationService) : base(eventAggregator, serviceMapper, appUser, navigationService) { }

    }
}

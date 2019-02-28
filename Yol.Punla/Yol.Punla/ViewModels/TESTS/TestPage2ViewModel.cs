using Prism.Events;
using Prism.Navigation;
using Yol.Punla.Barrack;
using CONSTANTS = Yol.Punla.Barrack.Constants;

namespace Yol.Punla.ViewModels
{
    public class TestPage2ViewModel : ChildViewModelBase
    {
        public string CurrentIcon => CONSTANTS.TABITEM_LOCATIONCITY;

        public TestPage2ViewModel(IEventAggregator eventAggregator, IServiceMapper serviceMapper, IAppUser appUser, INavigationService navigationService) : base(eventAggregator, serviceMapper, appUser, navigationService) {  }
    }
}

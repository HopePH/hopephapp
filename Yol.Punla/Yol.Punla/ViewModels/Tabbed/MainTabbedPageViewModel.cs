using Prism.Navigation;
using PropertyChanged;
using Unity;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class MainTabbedPageViewModel : ViewModelBase, IMainTabPageSelectedTab
    {
        private int _selectedTab;
        public int SelectedTab
        {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
        }

        public MainTabbedPageViewModel(INavigationService navigationService) : base(navigationService) 
            => AppUnityContainer.Instance.RegisterInstance<IMainTabPageSelectedTab>(this, new Unity.Lifetime.ContainerControlledLifetimeManager());

        public override void PreparingPageBindings() {  }

        public void SetSelectedTab(int tabIndex) => SelectedTab = tabIndex;
    }
}

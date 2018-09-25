using Prism;
using Prism.Events;
using Prism.Navigation;
using System;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.Mapper;

namespace Yol.Punla.ViewModels
{
    public class ChildViewModelBase : ViewModelBase, IActiveAware, INavigatingAware, IDestructible
    {
        IEventAggregator _ea { get; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, () => System.Diagnostics.Debug.WriteLine($"{Title} IsActive Changed: {value}")); }
        }

        public event EventHandler IsActiveChanged;

        public ChildViewModelBase(IEventAggregator eventAggregator, 
            IServiceMapper serviceMapper, 
            IAppUser appUser, 
            INavigationService navigationService)
            : base(serviceMapper, appUser)
        {
            _ea = eventAggregator;
            _ea.GetEvent<InitializeTabbedChildrenEvent>().Subscribe(OnInitializationEventFired);
            IsActiveChanged += (sender, e) => System.Diagnostics.Debug.WriteLine($"{Title} IsActive: {IsActive}");
        }

        void OnInitializationEventFired(NavigationParameters parameters)
        {
            System.Diagnostics.Debug.WriteLine($"{Title} Detected an initialization event");
            var message = parameters.GetValue<string>("message");
            Message = $"{Title} Initialized by Event: {message}";
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            System.Diagnostics.Debug.WriteLine($"{Title} is executing OnNavigatingTo");
            var message = parameters.GetValue<string>("message");
            Message = $"{Title} Initialized by OnNavigatingTo: {message}";

            PassingParameters = parameters;
            PreparingPageBindingsChild();
            PassingParameters = new NavigationParameters();
        }

        public void Destroy()
        {
            System.Diagnostics.Debug.WriteLine($"{Title} is being Destroyed!");
            _ea.GetEvent<InitializeTabbedChildrenEvent>().Unsubscribe(OnInitializationEventFired);
        }

        public new virtual void OnAppearing() { }

        public new virtual void OnDisappearing() { }

        public override void PreparingPageBindings() { }

        public virtual void PreparingPageBindingsChild()
        {
        }
    }
}

using Acr.UserDialogs;
using Prism.Commands;
using PropertyChanged;
using System;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.Localized;
using Yol.Punla.Mapper;
using Yol.Punla.Utility;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;

        public string AppVersion { get => "Version " + _keyValueCacheUtility.GetUserDefaultsKeyValue("AppCurrentVersion"); }
        public bool IsShowOfflineMessage { get => !IsInternetConnected; }
        public ICommand EditProfileCommand => new DelegateCommand(ShowUnavailablePopUp);
        public ICommand TermsAndConditionsCommand => new DelegateCommand(ShowUnavailablePopUp);
        public IDisposable DialogResult { get; set; }

        public SettingsPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser) : base(serviceMapper, appUser)
        {
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
            Title = AppStrings.TitleSettings;
        }

        public override void PreparingPageBindings()
        {
            IsShowBackArrow = false;
            //ct0.temp  to ayus pa itsura Title = "Settings";
            IsBusy = false;
        }

        private void ShowUnavailablePopUp() => DialogResult = UserDialogs.Instance.Alert(AppStrings.NotSupportedSettings);
    }
}

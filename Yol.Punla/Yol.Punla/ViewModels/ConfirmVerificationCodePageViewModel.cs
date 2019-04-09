using FluentValidation;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Utility;
using Yol.Punla.Managers;
using Yol.Punla.ViewModels.Validators;
using Yol.Punla.Views;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class ConfirmVerificationCodePageViewModel : ViewModelBase
    {
        private readonly IContactManager _userManager;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;
        private IValidator _validator;
        public string emailAddress;

        public ICommand SendVerificationCodeCommand => new DelegateCommand(async () => await SendVerificationCode());
        public string VerificationCodeEntered1 { get; set; }
        public string VerificationCodeEntered2 { get; set; }
        public string VerificationCodeEntered3 { get; set; }
        public string VerificationCodeEntered4 { get; set; }
        public string VerificationCodeEntered
        {
            get
            {
                try
                {
                    string v1 = VerificationCodeEntered1 ?? "";
                    string v2 = VerificationCodeEntered2 ?? "";
                    string v3 = VerificationCodeEntered3 ?? "";
                    string v4 = VerificationCodeEntered4 ?? "";
                    return v1 + v2 + v3 + v4;
                }
                catch
                {
                    return "";
                }
            }
        }
        public string VerificationCode { get; set; }
        public bool IsLogonIncorrectMessageDisplayed { get; set; }

        public ConfirmVerificationCodePageViewModel(INavigationService navigationService,
            IContactManager userManager) : base(navigationService)
        {        
            _userManager = userManager;
            _keyValueCacheUtility = AppUnityContainer.Instance.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>();
        }

        public override void PreparingPageBindings()
        {
            if (PassingParameters != null && PassingParameters.ContainsKey("VerificationCode"))
                VerificationCode = PassingParameters["VerificationCode"].ToString();

            if (PassingParameters != null && PassingParameters.ContainsKey("EmailAddress"))
                emailAddress = PassingParameters["EmailAddress"].ToString();
                
            IsBusy = false;
        }

        private async Task SendVerificationCode()
        {
            try
            {
                if (VerificationCode.HasValue())
                {
                    _validator = new VerificationCodeValidator(VerificationCodeEntered);

                    if (ProcessValidationErrors(_validator.Validate(this)))
                    {
                        IsBusy = true;
                        var clientFromRemote = await _userManager.GetContact(emailAddress, true);
                        if (clientFromRemote != null) await NavigateSuccess(clientFromRemote);
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForRaygun(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateSuccess(Entity.Contact clientFromRemote)
        {
            if (clientFromRemote != null)
            {
                string newPage = _keyValueCacheUtility.GetUserDefaultsKeyValue("NewPage");
                _keyValueCacheUtility.RemoveKeyObject("NewPage");
                _userManager.SaveNewDetails(clientFromRemote);
                RemoveCacheKeys();
                AddCacheKeys(clientFromRemote);

                if (string.IsNullOrEmpty(newPage))
                    await ChangeRootAndNavigateToPageHelper(nameof(MainTabbedPage) + AddPagesInTab());
                else
                    await ChangeRootAndNavigateToPageHelper(newPage);
            }

            IsBusy = false;
        }
        private string AddPagesInTab()
        {
            string path = "";
            var children = new List<string>();
            children.Add("addTab=PostFeedPage");
            children.Add("addTab=SettingsPage"); 
            path += "?" + string.Join("&", children);
            return path;
        }

        private void RemoveCacheKeys()
        {
            _keyValueCacheUtility.RemoveKeyObject("Username");
            _keyValueCacheUtility.RemoveKeyObject("Password");
            _keyValueCacheUtility.RemoveKeyObject("WasLogin");
            _keyValueCacheUtility.RemoveKeyObject("WasSignUpCompleted");
            _keyValueCacheUtility.RemoveKeyObject("CurrentContactId");
        }

        private void AddCacheKeys(Entity.Contact clientFromRemote)
        {
            _keyValueCacheUtility.GetUserDefaultsKeyValue("Username", clientFromRemote.UserName);
            _keyValueCacheUtility.GetUserDefaultsKeyValue("Password", clientFromRemote.Password);
            _keyValueCacheUtility.GetUserDefaultsKeyValue("WasLogin", "true");
            _keyValueCacheUtility.GetUserDefaultsKeyValue("WasSignUpCompleted", "true");
            _keyValueCacheUtility.GetUserDefaultsKeyValue("CurrentContactId", clientFromRemote.RemoteId.ToString());
        }
    }
}

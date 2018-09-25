using Acr.UserDialogs;
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
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.Extensions;
using Yol.Punla.Localized;
using Yol.Punla.Managers;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;
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
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;
        private IValidator _validator;
        private string emailAddress;

        public ICommand SendVerificationCodeCommand => new DelegateCommand(async () => await SendVerificationCode());
        public ICommand NavigateBackCommand => new DelegateCommand(GoBack);
        public string VerificationCodeEntered1 { get; set; }
        public string VerificationCodeEntered2 { get; set; }
        public string VerificationCodeEntered3 { get; set; }
        public string VerificationCodeEntered4 { get; set; }
        public string VerificationCodeEntered { get; set; }
        public string VerificationCode { get; set; }
        public bool IsLogonIncorrectMessageDisplayed { get; set; }

        public ConfirmVerificationCodePageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationService navigationService,
            INavigationStackService navigationStackService,
            IContactManager userManager) : base(serviceMapper, appUser)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;            
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
            if (VerificationCode.HasValue())
            {
                VerificationCodeEntered = VerificationCodeEntered1 + VerificationCodeEntered2 + VerificationCodeEntered3 + VerificationCodeEntered4;
                _validator = new VerificationCodeValidator(VerificationCodeEntered);

                if (ProcessValidationErrors(_validator.Validate(this), true))
                {
                    try
                    {
                        IsBusy = true;
                        var clientFromRemote =  await _userManager.GetContact(emailAddress, true);
                        if (clientFromRemote != null) NavigateSuccess(clientFromRemote);
                    }
                    catch (Exception ex)
                    {
                        ProcessErrorReportingForHockeyApp(ex);
                    }
                }
            }
        }

        private void NavigateSuccess(Entity.Contact clientFromRemote)
        {
            if (clientFromRemote != null)
            {
                string newPage = _keyValueCacheUtility.GetUserDefaultsKeyValue("NewPage");
                _keyValueCacheUtility.RemoveKeyObject("NewPage");
                _userManager.SaveNewDetails(clientFromRemote);
                _keyValueCacheUtility.GetUserDefaultsKeyValue("Username", clientFromRemote.UserName);
                _keyValueCacheUtility.GetUserDefaultsKeyValue("Password", clientFromRemote.Password);
                _keyValueCacheUtility.GetUserDefaultsKeyValue("WasLogin", "true");
                _keyValueCacheUtility.GetUserDefaultsKeyValue("WasSignUpCompleted", "true");
                _keyValueCacheUtility.GetUserDefaultsKeyValue("CurrentContactId", clientFromRemote.RemoteId.ToString());

                if (string.IsNullOrEmpty(newPage))
                    ChangeRootAndNavigateToPageHelper(nameof(MainTabbedPage) + AddPagesInTab(), _navigationStackService, _navigationService);
                else
                    ChangeRootAndNavigateToPageHelper(newPage, _navigationStackService, _navigationService);
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

        private async Task GetLogonDetailsFromRemoteDBWrongResult(bool isSuccess = true)
        {
            if (isSuccess)
            {
                IsLogonIncorrectMessageDisplayed = await UserDialogs.Instance.ConfirmAsync(AppStrings.LogonIncorrect);
                _keyValueCacheUtility.RemoveKeyObject("WasLogin");
                ChangeRootAndNavigateToPageHelper(nameof(ViewNames.SignUpPage), _navigationStackService, _navigationService);
            }

            IsBusy = false;
        }

        private string ComputeEmailIfTest(string email)
        {
            if (!string.IsNullOrEmpty(email))
                if (email.ToLower().Trim() == Constants.TESTEMAIL1)
                {
                    Guid id = Guid.NewGuid();
                    string newId = id.ToString();

                    string[] splitTwo = email.Split('@');
                    string newEmail = splitTwo[0] + newId + "@" + splitTwo[1];
                    return newEmail;
                }

            return email;
        }
        
        private void GoBack() => NavigateBackHelper(_navigationStackService, _navigationService);
    }
}

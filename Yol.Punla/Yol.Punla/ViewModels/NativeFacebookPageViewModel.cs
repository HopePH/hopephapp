using Acr.UserDialogs;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.FakeEntries;
using Yol.Punla.Localized;
using Yol.Punla.Managers;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;
using Unity;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class NativeFacebookPageViewModel : ViewModelBase
    {
        private const string TESTEMAIL1 = "hynrbf@gmail.com";
        private const string TESTEMAIL2 = "alfeo.salano@gmail.com";
        private const string TESTEMAIL3 = "robertjlima38@gmail.com";

        private readonly INavigationStackService _navigationStackService;
        private readonly INavigationService _navigationService;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;
        private readonly IContactManager _userManager;
        
        public string FacebookId { get; set; }
        public string FacebookFirstName { get; set; }
        public string FacebookLastName { get; set; }
        public string FacebookName { get; set; }
        public string FacebookProfile { get; set; }
        public string FacebookEmail { get; set; }
        public string FacebookPhoto { get; set; }
        public string FacebookBirthday { get; set; }
        public string FacebookGender { get; set; }
        public string FacebookLink { get; set; }
        public string FacebookMobileNumber { get; set; }
        public string FacebookAlias { get; set; }
        public bool FromLogonPage { get; set; }
        public bool IsPopUpDisplayed { get; set; }
        public bool IsLogonIncorrectMessageDisplayed { get; set; }

        public NativeFacebookPageViewModel(IServiceMapper serviceMapper,
            IAppUser appUser,
            IContactManager userManager,
            INavigationService navigationService,
            INavigationStackService navigationStackService) : base(serviceMapper, appUser)
        {
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _userManager = userManager;
        }

        #region PREPARE PAGE BINDINGS

        public override void PreparingPageBindings()
        {
            if (PassingParameters != null && PassingParameters.ContainsKey("ComingFromLogin"))
                FromLogonPage = (bool)PassingParameters["ComingFromLogin"];

            IsBusy = false;
            FacebookLogonCompletedFake();
        }

        [Conditional("FAKE")]
        private void FacebookLogonCompletedFake()
        {
            var fakeEntry = AppUnityContainer.Instance.Resolve<ContactEntry>();

            Entity.Contact fakeProfile = FakeData.FakeUsers.Contacts
                .Where(x => x.EmailAdd == fakeEntry.EmailAddress ||
                       x.MobilePhone == fakeEntry.MobilePhone).FirstOrDefault();

            if (fakeProfile != null)
            {
                FacebookEmail = fakeProfile.EmailAdd;
                FacebookFirstName = fakeProfile.FirstName;
                FacebookLastName = fakeProfile.LastName;
                FacebookPhoto = fakeProfile.PhotoURL;
                FacebookBirthday = fakeProfile.Birthdate;
                FacebookGender = fakeProfile.GenderCode;
                FacebookId = fakeProfile.FBId;
                FacebookLink = fakeProfile.FBLink;
                FacebookMobileNumber = fakeProfile.MobilePhone;
                FacebookAlias = fakeProfile.AliasName;
            }
            else
            {
                FacebookEmail = fakeEntry.EmailAddress;
                FacebookFirstName = fakeEntry.FirstName;
                FacebookLastName = fakeEntry.LastName;
                FacebookPhoto = fakeEntry.PhotoURL;
                FacebookMobileNumber = fakeEntry.MobilePhone;
            }

            if (FromLogonPage)
                GetLogonDetailsFromRemoteDBAsyncFake();
            else
                SaveFacebookProfileAsyncFake();
        }

        #endregion

        public async void SaveFacebookProfileAsync()
        {
            if (!FromLogonPage)
            {
                if (FacebookEmail.ToLower().Trim() == TESTEMAIL1 || FacebookEmail.ToLower().Trim() == TESTEMAIL2)
                {
                    SaveToDBAndNavigateToNextPage();
                    return;
                }

                if (await IsEmailExisting())
                {
                    await RedirectToLogon();
                    return;
                }
                else
                    SaveToDBAndNavigateToNextPage();
            }
            else
            {
                IsBusy = true;
                GetLogonDetailsFromRemoteDBAsync();
            }
        }

        [Conditional("FAKE")]
        public void SaveFacebookProfileAsyncFake()
        {
            if (!FromLogonPage)
            {
                if (FacebookEmail.ToLower().Trim() == TESTEMAIL1 || FacebookEmail.ToLower().Trim() == TESTEMAIL2 || FacebookEmail.ToLower().Trim() == TESTEMAIL3)
                {
                    SaveToDBAndNavigateToNextPage();
                    return;
                }

                if (IsEmailExisting().Result)
                {
                    RedirectToLogon().Wait();
                    return;
                }
                else
                    SaveToDBAndNavigateToNextPage();
            }
            else
            {
                IsBusy = true;
                GetLogonDetailsFromRemoteDBAsyncFake();
            }
        }

        private async void GetLogonDetailsFromRemoteDBAsync()
        {
            try
            {
                var cts = CreateNewHandledTokenSource("NativeFacebookPageViewModel.GetLogonDetailsFromRemoteDBAsync", 20);

                var clientFromRemote = await Task.Run(async () =>
                {
                    var _clientFromRemote = await _userManager.GetContact(this.FacebookEmail, this.FacebookId, true);
                    return _clientFromRemote;
                }, TokenHandler.Token);

                if (clientFromRemote != null)
                    GetLogonDetailsFromRemoteDBResult(clientFromRemote, TokenHandler.IsTokenSourceCompleted());
                else
                    await GetLogonDetailsFromRemoteDBWrongResult(TokenHandler.IsTokenSourceCompleted());
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex);
                await GetLogonDetailsFromRemoteDBWrongResult(true);
            }
        }

        [Conditional("FAKE")]
        private void GetLogonDetailsFromRemoteDBAsyncFake()
        {
            var userFromRemote = _userManager.GetContact(FacebookEmail, FacebookId).Result;

            if (userFromRemote != null)
                GetLogonDetailsFromRemoteDBResult(userFromRemote);
            else
                GetLogonDetailsFromRemoteDBWrongResult().Wait();
        }
        
        private void SaveToDBAndNavigateToNextPage()
        {
            Debug.WriteLine("HOPEPH Saving receiver details from FB.");
            var newDetails = new Entity.Contact
            {
                Id = 0,
                EmailAdd = ComputeEmailIfTest(FacebookEmail),
                Password = "123456Aa@",
                FirstName = FacebookFirstName,
                LastName = FacebookLastName,
                Birthdate = FacebookBirthday ?? "",
                FBLink = FacebookPhoto,
                GenderCode = FacebookGender,
                FBUserLink = FacebookLink,
                FBId = FacebookId,
                MobilePhone = FacebookMobileNumber,
                AliasName = FacebookAlias
            };

            PassingParameters.Add("CurrentContact", newDetails);
            NavigateToPageHelper(nameof(ViewNames.AccountRegistrationPage), _navigationStackService, _navigationService, PassingParameters);   
        }

        private string ComputeEmailIfTest(string email)
        {
            //chito. this is a test user, so put this to make all test email unique
            if (!string.IsNullOrEmpty(email))
                if (email.ToLower().Trim() == TESTEMAIL1 || email.ToLower().Trim() == TESTEMAIL2)
                {
                    Guid id = Guid.NewGuid();
                    string newId = id.ToString();

                    string[] splitTwo = email.Split('@');
                    string newEmail = splitTwo[0] + newId + "@" + splitTwo[1];
                    return newEmail;
                }

            return email;
        }

        private void GetLogonDetailsFromRemoteDBResult(Entity.Contact clientFromRemote, bool isSuccess = true)
        {
            if (clientFromRemote != null && isSuccess)
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
                    ChangeRootAndNavigateToPageHelper(nameof(ViewNames.HomePage), _navigationStackService, _navigationService, PassingParameters);
                else
                    ChangeRootAndNavigateToPageHelper(newPage, _navigationStackService, _navigationService, PassingParameters);
            }

            IsBusy = false;
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

        private async Task<bool> IsEmailExisting()
        {
            try
            {
                var isExisting = await Task.Run(() =>
                {
                    var existingEmail = _userManager.GetContact(FacebookEmail, FacebookId, true);
                    return existingEmail;
                });

                if (isExisting == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex);
                return false;
            }
        }

        private async Task RedirectToLogon()
        {
            IsPopUpDisplayed = await UserDialogs.Instance.ConfirmAsync(AppStrings.DuplicateSignUpBlurb, "Account Exists", "OK");
            _userManager.LogoutUser();
            _keyValueCacheUtility.RemoveKeyObject("CurrentContactId");
            ChangeRootAndNavigateToPageHelper(nameof(ViewNames.LogonPage), _navigationStackService, _navigationService);
        }
    }
}

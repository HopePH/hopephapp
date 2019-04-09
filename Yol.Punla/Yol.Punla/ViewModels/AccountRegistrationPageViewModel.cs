using FluentValidation;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Managers;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class AccountRegistrationPageViewModel : ViewModelBase
    {
        private readonly string picDefaultMale = AppImages.PandaAvatar;
        private readonly string picDefaultFemale = AppImages.PandaAvatar;
        private readonly IValidator _validator;
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;
        private readonly IContactManager _contactManager;

        public string FullName
        {
            get
            {
                if (CurrentContact == null) return "";
                return $"{CurrentContact.FirstName} {CurrentContact.LastName}";
            }
        }

        public ICommand RetakePhotoCommand => new DelegateCommand(TakePhoto);
        public ICommand SignupCommand => new DelegateCommand(ValidationPriorSignupAsync);
        public ICommand ShowOrHideAvatarSelectionCommand => new DelegateCommand<object>(ShowHideAvatarSelection);
        public ICommand SetAvatarUrlCommand => new DelegateCommand<Avatar>(ChangeAvatar);
        public IEnumerable<Avatar> PredefinedAvatars { get; set; }
        public Entity.Contact CurrentContact { get; set; }
        public string Picture { get; set; }
        public bool HasPicture { get; set; }
        public string AliasName { get; set; }
        public string MobilePhoneNo { get; set; }
        public string EmailAddress { get; set; }
        public bool EmailEnabled { get; set; }
        public bool IsAvatarModalVisible { get; set; }

        public IEnumerable<Image> Avatars { get; set; }

        public AccountRegistrationPageViewModel(INavigationService navigationService,
            IContactManager contactManager,
            AccountRegistrationPageValidator validator) : base(navigationService)
        {
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
            _navigationService = navigationService;
            _contactManager = contactManager;
            _validator = validator;
        }
        
        public override void PreparingPageBindings()
        {
            if (!(PassingParameters != null && PassingParameters.ContainsKey(nameof(CurrentContact))))
                throw new ArgumentException("CurrentContact parameter was null in the account registration page");

            CurrentContact = (Entity.Contact)PassingParameters[nameof(CurrentContact)];
            EmailAddress = CurrentContact.EmailAdd;
            HasPicture = true;
            Picture = (CurrentContact.GenderCode.ToLower() == "male") ? picDefaultMale : picDefaultFemale;
            EmailEnabled = (!string.IsNullOrEmpty(EmailAddress)) ? false : true;
            PredefinedAvatars = AppImages.Avatars;
            IsBusy = false;
        }

        #region SIGN UP

        public async void ValidationPriorSignupAsync()
        {
            if (ProcessValidationErrors(_validator.Validate(this), true))
            {
                CurrentContact.AliasName = AliasName;
                CurrentContact.MobilePhone = MobilePhoneNo;
                CurrentContact.EmailAdd = EmailAddress;
                CurrentContact.PhotoURL = Picture;

                await SignupAsync();
            }
        }

        public async Task SignupAsync()
        {
            try
            {
                var resultId = await _contactManager.SaveDetailsToRemoteDB(CurrentContact);
                await SignUpResult(resultId);
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }

        public async Task SignUpResult(int resultId)
        {
            if (resultId > 0)
            {
                CurrentContact.Id = resultId;
                CurrentContact.RemoteId = resultId;
                CurrentContact.UserName = CurrentContact.EmailAdd;

                _contactManager.SaveNewDetails(CurrentContact);
                PassingParameters.Add(nameof(CurrentContact), CurrentContact);

                string newPage = _keyValueCacheUtility.GetUserDefaultsKeyValue("NewPage");
                _keyValueCacheUtility.RemoveKeyObject("NewPage");

                if (string.IsNullOrEmpty(newPage)) await NavigateToRootPage(nameof(Views.MainTabbedPage) + AddPagesInTab());
                else await ChangeRootAndNavigateToPageHelper(newPage, PassingParameters);

                _keyValueCacheUtility.GetUserDefaultsKeyValue("WasLogin", "true");
                _keyValueCacheUtility.GetUserDefaultsKeyValue("WasSignUpCompleted", "true");
                _keyValueCacheUtility.GetUserDefaultsKeyValue("CurrentContactId", resultId.ToString());
            }

            IsBusy = false;
        }

        #endregion

        private void TakePhoto() => IsAvatarModalVisible = true;

        private void ShowHideAvatarSelection(object isVisible) => IsAvatarModalVisible = bool.Parse(isVisible.ToString());

        private void ChangeAvatar(Avatar avatar)
        {
            Picture = avatar.SourceUrl;
            ShowHideAvatarSelection(false);
        }

        private string AddPagesInTab()
        {
            string path = "";
            var children = new List<string>();
            children.Add("addTab=PostFeedPage");
            #region TEMP ONLY
            children.Add("addTab=TestPage");
            children.Add("addTab=TestPage2");
            #endregion
            path += "?" + string.Join("&", children);
            return path;
        }

        private async Task NavigateToRootPage(string page)
        {
            var rootPage = AppSettingsProvider.Instance.GetValue("AppRootURI") + $"{page}";
            _navigationStackService.UpdateStackState(page);
            await _navigationService.NavigateAsync(rootPage);
        }
    }
}

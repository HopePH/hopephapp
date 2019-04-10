using FluentValidation;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using Rg.Plugins.Popup.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Managers;
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
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;
        private readonly IContactManager _contactManager;
        private readonly IPopupNavigation _popUpNavigation;

        public string FullName
        {
            get
            {
                if (CurrentContact == null) return "";
                return $"{CurrentContact.FirstName} {CurrentContact.LastName}";
            }
        }

        public ICommand SignupCommand => new DelegateCommand(async () => await SignUpAsync());

        public ICommand ShowAvatarSelectionCommand => new DelegateCommand(async () => await OpenAvatarPopup());
        public ICommand SetAvatarUrlCommand => new DelegateCommand<Avatar>(ChangeAvatar);
        public IEnumerable<Avatar> PredefinedAvatars { get; set; }
        public Entity.Contact CurrentContact { get; set; } = new Entity.Contact();
        public bool HasPicture { get; set; }       
        public bool EmailEnabled { get; set; }
        public IEnumerable<Image> Avatars { get; set; }

        public AccountRegistrationPageViewModel(INavigationService navigationService,
            IContactManager contactManager,
            IPopupNavigation popupNavigation,
            AccountRegistrationPageValidator validator) : base(navigationService)
        {
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
            _contactManager = contactManager;
            _validator = validator;
            _popUpNavigation = popupNavigation;
        }
        
        public override void PreparingPageBindings()
        {
            try
            {
                if (!(PassingParameters != null && PassingParameters.ContainsKey(nameof(CurrentContact))))
                    throw new ArgumentException("CurrentContact parameter was null in the account registration page");

                CurrentContact = (Entity.Contact)PassingParameters[nameof(CurrentContact)];
                HasPicture = true;
                CurrentContact.PhotoURL = (CurrentContact.GenderCode.ToLower() == "male") ? picDefaultMale : picDefaultFemale;
                EmailEnabled = (!string.IsNullOrEmpty(CurrentContact.EmailAdd)) ? false : true;
                PredefinedAvatars = AppImages.Avatars;
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

        private async Task SignUpAsync()
        {
            try
            {
                if (ProcessValidationErrors(_validator.Validate(this)))
                {
                    if (ProcessInternetConnection())
                    {
                        var resultId = await _contactManager.SaveDetailsToRemoteDB(CurrentContact);

                        if (resultId > 0)
                        {
                            CurrentContact.Id = resultId;
                            CurrentContact.RemoteId = resultId;
                            CurrentContact.UserName = CurrentContact.EmailAdd;
                            _contactManager.SaveNewDetails(CurrentContact);
                            PassingParameters.Add(nameof(CurrentContact), CurrentContact);
                            string newPage = _keyValueCacheUtility.GetUserDefaultsKeyValue("NewPage");
                            RemoveCacheKeys();

                            if (string.IsNullOrEmpty(newPage))
                                await ChangeRootAndNavigateToPageHelper(nameof(Views.MainTabbedPage) + AddPagesInTab());
                            else
                                await ChangeRootAndNavigateToPageHelper(newPage, PassingParameters);

                            AddCacheKeys(resultId);
                        }
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

        private void ChangeAvatar(Avatar avatar)
        {
            try
            {
                CurrentContact.PhotoURL = avatar.SourceUrl;
                RaisePropertyChanged(nameof(CurrentContact));
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForRaygun(ex);
            }
        }

        private async Task OpenAvatarPopup()
        {
            try
            {
                var popup = new Views.AvatarPopup(this);
                await _popUpNavigation.PushAsync(popup);
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForRaygun(ex);
            }
        }

        private string AddPagesInTab()
        {
            string path = "";
            var children = new List<string>();
            children.Add("addTab=PostFeedPage");
            path += "?" + string.Join("&", children);
            return path;
        }

        private void RemoveCacheKeys()
        {
            _keyValueCacheUtility.RemoveKeyObject("NewPage");
            _keyValueCacheUtility.RemoveKeyObject("WasLogin");
            _keyValueCacheUtility.RemoveKeyObject("WasSignUpCompleted");
            _keyValueCacheUtility.RemoveKeyObject("CurrentContactId");
        }

        private void AddCacheKeys(int resultId)
        {
            _keyValueCacheUtility.GetUserDefaultsKeyValue("WasLogin", "true");
            _keyValueCacheUtility.GetUserDefaultsKeyValue("WasSignUpCompleted", "true");
            _keyValueCacheUtility.GetUserDefaultsKeyValue("CurrentContactId", resultId.ToString());
        }
    }
}

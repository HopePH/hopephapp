using FluentValidation;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.Localized;
using Yol.Punla.Managers;
using Yol.Punla.NavigationHeap;
using Yol.Punla.ViewModels.Validators;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class EmailVerificationPageViewModel : ViewModelBase
    {
        private readonly IContactManager _contactManager;
        private readonly INavigationStackService _navigationStackService;
        private readonly INavigationService _navigationService;
        private IValidator _validator;

        public ICommand SendVerificationCodeCommand => new DelegateCommand(async () => await SendVerificationCode());
        public string VerificationCode { get; set; }
        public string EmailAddress { get; set; }
        public string VerificationCodeEntered1 { get; set; }
        public string VerificationCodeEntered2 { get; set; }
        public string VerificationCodeEntered3 { get; set; }
        public string VerificationCodeEntered4 { get; set; }
        public string ConfirmVerificationCode { get; set; }
        public bool IsVerification { get; set; }
        public bool IsVerificationNegation { get; set; }
        public string TitleMessage { get; set; }
        public string TitleContent { get; set; }
        public string PlaceholderTitle { get; set; }

        public EmailVerificationPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser, 
            INavigationStackService navigationStackService, 
            INavigationService navigationService,
            IContactManager contactManager) : base(navigationService)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _contactManager = contactManager;
        }

        public override void PreparingPageBindings()
        {
            TitleMessage = AppStrings.VerificationEmailTitle;
            TitleContent = AppStrings.VerificationEmailMsg;
            PlaceholderTitle = AppStrings.VerificationEmailPlaceholder;
            IsVerificationNegation = !IsVerification;
            IsBusy = false;
        }

        private async Task SendVerificationCode()
        {
            try
            {
                IsBusy = true;
                string emailDuplicate = (await _contactManager.CheckIfEmailExists(EmailAddress, "HopePH")) ? EmailAddress : "Ret45ujhh@gboy.com";

#if (FAKE || DEBUG)
                if (EmailAddress == Constants.TESTEMAIL1) emailDuplicate = "";
#endif

                if (!IsVerification)
                    _validator = new EmailVerificationPageValidator(VerificationCode, IsVerification, emailDuplicate);
                else
                {
                    ConfirmVerificationCode = $"{VerificationCodeEntered1}{VerificationCodeEntered2}{VerificationCodeEntered3}{VerificationCodeEntered4}";
                    _validator = new EmailVerificationPageValidator(ConfirmVerificationCode, IsVerification, emailDuplicate);
                }
                    
                if (ProcessValidationErrors(_validator.Validate(this), true))                  
                    await PrepareNavigationToRegistrationPage(IsVerification);
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task PrepareNavigationToRegistrationPage(bool isVerification)
        {
            if (!isVerification)
            {
                VerificationCode = await _contactManager.SendVerificationCode(EmailAddress);
                IsVerification = (string.IsNullOrEmpty(VerificationCode)) ? false : true;
                IsVerificationNegation = !IsVerification;

                if (IsVerification)
                {
                    TitleMessage = AppStrings.VerifiedEmailTitle;
                    TitleContent = AppStrings.VerifiedEmailMsg;
                    PlaceholderTitle = AppStrings.VerifiedEmailPlaceholder;
                }
            }
            else
            {
                Entity.Contact contact = new Entity.Contact
                {
                    EmailAdd = ComputeEmailIfTest(EmailAddress),
                    UserName = ComputeEmailIfTest(EmailAddress),
                    GenderCode = "undisclosed",
                    FirstName = "Undisclosed",
                    LastName = "Name"
                };

                PassingParameters.Add("CurrentContact", contact);
                await NavigateToPageHelper(nameof(ViewNames.AccountRegistrationPage), PassingParameters);
            }
        }

        private string ComputeEmailIfTest(string email)
        {
            //chito. this is a test user, so put this to make all test email unique
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
    }
}

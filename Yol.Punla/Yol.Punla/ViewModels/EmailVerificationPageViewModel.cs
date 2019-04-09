using FluentValidation;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Localized;
using Yol.Punla.Managers;
using Yol.Punla.ViewModels.Validators;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class EmailVerificationPageViewModel : ViewModelBase
    {
        private readonly IContactManager _contactManager;
        private IValidator _validator;

        public ICommand SendVerificationCodeCommand => new DelegateCommand(async () => await SendVerificationCode());
        public string VerificationCode { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string VerificationCodeEntered1 { get; set; }
        public string VerificationCodeEntered2 { get; set; }
        public string VerificationCodeEntered3 { get; set; }
        public string VerificationCodeEntered4 { get; set; }
        public string ConfirmVerificationCode
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
        public bool IsVerification { get; set; }
        public bool IsVerificationNegation
        {
            get => !IsVerification;
        }
        public string TitleMessage { get; set; }
        public string TitleContent { get; set; }
        public string PlaceholderTitle { get; set; }

        public EmailVerificationPageViewModel(INavigationService navigationService,
            IContactManager contactManager) : base(navigationService)
            => _contactManager = contactManager;

        public override void PreparingPageBindings()
        {
            TitleMessage = AppStrings.VerificationEmailTitle;
            TitleContent = AppStrings.VerificationEmailMsg;
            PlaceholderTitle = AppStrings.VerificationEmailPlaceholder;
            IsBusy = false;
        }

        private async Task SendVerificationCode()
        {
            try
            {
                IsBusy = true;
                string emailDuplicate = (await _contactManager.CheckIfEmailExists(EmailAddress, "HopePH")) ? EmailAddress 
                    : "Ret45ujhh@gboy.com";

#if (FAKE || DEBUG)
                if (EmailAddress == Constants.TESTEMAIL1) emailDuplicate = "";
#endif

                if (!IsVerification)
                    _validator = new EmailVerificationPageValidator(VerificationCode, IsVerification, emailDuplicate);
                else
                    _validator = new EmailVerificationPageValidator(ConfirmVerificationCode, IsVerification, emailDuplicate);
                    
                if (ProcessValidationErrors(_validator.Validate(this)))                  
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
                    GenderCode = "Undisclosed",
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

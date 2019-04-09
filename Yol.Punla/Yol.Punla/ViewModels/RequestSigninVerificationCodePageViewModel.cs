using FluentValidation;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Managers;
using Yol.Punla.ViewModels.Validators;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class RequestSigninVerificationCodePageViewModel : ViewModelBase
    {
        private const string APPNAME = "HopePH";
        private const string FAKEEMAIL = "Ret45ujhh@gboy.com";
        private readonly IContactManager _contactManager;
        private IValidator _validator;

        public ICommand RequestVerificationCodeCommand => new DelegateCommand(async() => await RequestVerificationCode());
        public string EmailAddress { get; set; }
        public string VerificationCode { get; set; }

        public RequestSigninVerificationCodePageViewModel(INavigationService navigationService,
            IContactManager contactManager) : base(navigationService)
            => _contactManager = contactManager;

        public override void PreparingPageBindings() 
            => IsBusy = false;

        private async Task RequestVerificationCode()
        {
            try
            {
                IsBusy = true;
                EmailAddress = (await _contactManager.CheckIfEmailExists(EmailAddress, APPNAME)) ? EmailAddress : FAKEEMAIL;
                _validator = new RequestVerificationCodePageEmailValidator(EmailAddress);

                if (ProcessValidationErrors(_validator.Validate(this)))
                {
                    VerificationCode = await _contactManager.SendVerificationCode(EmailAddress);
                    PassingParameters.Add("VerificationCode", VerificationCode);
                    PassingParameters.Add("EmailAddress", EmailAddress);
                    await NavigateToPageHelper(nameof(ViewNames.ConfirmVerificationCodePage), PassingParameters);
                }
                else
                    EmailAddress = String.Empty;
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForRaygun(ex);
            }
        }
    }
}

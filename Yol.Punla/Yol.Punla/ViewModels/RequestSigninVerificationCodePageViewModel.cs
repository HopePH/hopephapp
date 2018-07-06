using FluentValidation;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Managers;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;
using Yol.Punla.ViewModels.Validators;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class RequestSigninVerificationCodePageViewModel : ViewModelBase
    {
        private readonly INavigationStackService _navigationStackService;
        private readonly INavigationService _navigationService;
        private readonly IContactManager _contactManager;
        private IValidator _validator;

        public ICommand RequestVerificationCodeCommand => new DelegateCommand(RequestVerificationCode);
        public ICommand NavigateBackCommand => new DelegateCommand(GoBack);
        public string EmailAddress { get; set; }
        public string VerificationCode { get; set; }

        public RequestSigninVerificationCodePageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser, 
            INavigationStackService navigationStackService,
            INavigationService navigationService,
            IContactManager contactManager) : base(serviceMapper, appUser)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _contactManager = contactManager;
        }

        public override void PreparingPageBindings()
        {
            IsBusy = false;
        }

        private void GoBack() => NavigateBackHelper(_navigationStackService, _navigationService);

        private async void RequestVerificationCode()
        {
            try
            {
                IsBusy = true;
                string existingEmail = (await _contactManager.CheckIfEmailExists(EmailAddress, "HopePH")) ? EmailAddress : "Ret45ujhh@gboy.com";
                _validator = new RequestVerificationCodePageEmailValidator(EmailAddress, existingEmail);

                if (ProcessValidationErrors(_validator.Validate(this), true))
                {
                    VerificationCode = await _contactManager.SendVerificationCode(EmailAddress);
                    PassingParameters.Add("VerificationCode", VerificationCode);
                    PassingParameters.Add("EmailAddress", EmailAddress);
                    NavigateToPageHelper(nameof(ViewNames.ConfirmVerificationCodePage), _navigationStackService, _navigationService, PassingParameters);
                }
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex);
            }
        }
    }
}

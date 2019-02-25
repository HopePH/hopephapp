﻿using FluentValidation;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.Managers;
using Yol.Punla.NavigationHeap;
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
        private readonly INavigationStackService _navigationStackService;
        private readonly INavigationService _navigationService;
        private readonly IContactManager _contactManager;
        private IValidator _validator;

        public ICommand RequestVerificationCodeCommand => new DelegateCommand(async() => await RequestVerificationCode());
        public ICommand NavigateBackCommand => new DelegateCommand(async ()=> await GoBack());
        public string EmailAddress { get; set; }
        public string VerificationCode { get; set; }

        public RequestSigninVerificationCodePageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser, 
            INavigationStackService navigationStackService,
            INavigationService navigationService,
            IContactManager contactManager) : base(navigationService)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _contactManager = contactManager;
        }

        public override void PreparingPageBindings() => IsBusy = false;

        private async Task GoBack() => await NavigateBackHelper();

        private async Task RequestVerificationCode()
        {
            try
            {
                IsBusy = true;
                EmailAddress = (await _contactManager.CheckIfEmailExists(EmailAddress, APPNAME)) ? EmailAddress : FAKEEMAIL;
                _validator = new RequestVerificationCodePageEmailValidator(EmailAddress);

                if (ProcessValidationErrors(_validator.Validate(this), true))
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
                ProcessErrorReportingForHockeyApp(ex);
            }
        }
    }
}

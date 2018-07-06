using Prism.Unity;
using Should;
using TechTalk.SpecFlow;
using Unity;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.UnitTest.Mocks;
using Yol.Punla.ViewModels;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public class EmailVerificationPageSteps : StepBase
    {
        public EmailVerificationPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [When(@"I tap the Verify Email Button")]
        public void WhenITapTheVerifyEmailButton() 
            => Main.App.Container.GetContainer().Resolve<SignUpPageViewModel>().VerifyEmailCommand.Execute(null);

        [Given(@"I could see the Email and verification code entries are empty")]
        public void GivenICouldSeeTheEmailAndVerificationCodeEntriesAreEmpty()
        {
            var viewModel = Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>();
            viewModel.EmailAddress.ShouldBeNull();
            viewModel.ConfirmVerificationCode.ShouldBeNull();
            viewModel.VerificationCode.ShouldBeNull();
            viewModel.TitleMessage.ShouldEqual("Enter your email address");
            viewModel.TitleContent.ShouldEqual("We need to know how to contact you");
            viewModel.PlaceholderTitle.ShouldEqual("Email");
        }

        [When(@"I type the email ""(.*)"" and tap send verification code button")]
        public void WhenITypeTheEmailAndTapSendVerificationCodeButton(string email)
        {
            var viewModel = Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>();
            viewModel.EmailAddress = email;
            viewModel.SendVerificationCodeCommand.Execute(null);
        }

        [Then(@"I should get a verification code sent to my email")]
        public void ThenIShouldGetAVerificationCodeSentToMyEmail()
        {
            var viewModel = Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>();
            viewModel.TitleMessage.ShouldEqual("Enter your verification code");
            viewModel.TitleContent.ShouldEqual("We sent the verification code in your email");
            viewModel.PlaceholderTitle.ShouldEqual("Verification Code");
            viewModel.VerificationCode.ShouldNotBeEmpty();
        }


        [Then(@"I should get a verification code ""(.*)"" sent to my email ""(.*)""")]
        public void ThenIShouldGetAVerificationCodeSentToMyEmail(string verificationCode, string email)
        {
            var viewModel = Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>();
            viewModel.EmailAddress.ShouldEqual(email);
            viewModel.VerificationCode = verificationCode;
        }
       
        [When(@"I type the verification code ""(.*)"" and tap the submit button")]
        public void WhenITypeTheVerificationCodeAndTapTheSubmitButton(string verificationCode)
        {
            var viewModel = Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>();
            viewModel.ConfirmVerificationCode = verificationCode;
            viewModel.VerificationCode.ShouldEqual(verificationCode);
            viewModel.SendVerificationCodeCommand.Execute(null);
        }
        
        [When(@"I type the verification code ""(.*)"" and tap the submit button using the email ""(.*)"" provided")]
        public void WhenITypeTheVerificationCodeAndTapTheSubmitButtonUsingTheEmailProvided(string verificationCode, string email)
        {
            var viewModel = Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>();
            viewModel.ConfirmVerificationCode = verificationCode;
            viewModel.EmailAddress = email;
            viewModel.SendVerificationCodeCommand.Execute(null);
        }

        [Then(@"I should see an error message ""(.*)""")]
        public void ThenIShouldSeeAnErrorMessage(string errorMessage) 
            => Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().PageErrors.ToString().ShouldEqual(errorMessage);

        [When(@"I tap send verification code button")]
        public void WhenITapSendVerificationCodeButton() 
            => Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().SendVerificationCodeCommand.Execute(null);

        [Then(@"I could see the Fullname ""(.*)""")]
        public void ThenICouldSeeTheFullname(string fullname) 
            => Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().FullName.ShouldEqual(fullname);

        [Then(@"I could see the email ""(.*)""")]
        public void ThenICouldSeeTheEmail(string email) 
            => Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().EmailAddress.ShouldEqual(email);

    }
}

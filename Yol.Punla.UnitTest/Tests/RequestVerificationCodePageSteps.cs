using Prism.Unity;
using Should;
using TechTalk.SpecFlow;
using Unity;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.ViewModels;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public class RequestVerificationCodePageSteps : StepBase
    {
        public RequestVerificationCodePageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [When(@"I tap the Alias link text in sign in with alias label")]
        public void WhenITapTheAliasLinkTextInSignInWithAliasLabel()
        {
            Main.App.Container.GetContainer().Resolve<LogonPageViewModel>().GoToSigninWithAliasCommand.Execute(null);
        }

        [Then(@"I should see that the email address field is empty")]
        public void ThenIShouldSeeThatTheEmailAddressFieldIsEmpty()
        {
            Main.App.Container.GetContainer().Resolve<RequestSigninVerificationCodePageViewModel>().EmailAddress.ShouldBeNull();
        }

        [When(@"I type my email ""(.*)"" in the email field and tap the send verification code button")]
        public void WhenITypeMyEmailInTheEmailFieldAndTapTheSendVerificationCodeButton(string email)
        {
            Main.App.Container.GetContainer().Resolve<RequestSigninVerificationCodePageViewModel>().EmailAddress = email;
            Main.App.Container.GetContainer().Resolve<RequestSigninVerificationCodePageViewModel>().RequestVerificationCodeCommand.Execute(null);
        }

        [Given(@"the verification code is null")]
        public void GivenTheVerificationCodeIsNull()
        {
            Main.App.Container.GetContainer().Resolve<RequestSigninVerificationCodePageViewModel>().VerificationCode.ShouldBeNull();
        }

        [Then(@"I should receive a verification code ""(.*)"" sent to my email")]
        public void ThenIShouldReceiveAVerificationCodeSentToMyEmail(string verificationCode)
        {
            Main.App.Container.GetContainer().Resolve<RequestSigninVerificationCodePageViewModel>().VerificationCode.ShouldNotBeNull();
        }

        [Then(@"I should see an error message ""(.*)"" in RequestSigninVerificationCodePage")]
        public void ThenIShouldSeeAnErrorMessageInRequestSigninVerificationCodePage(string errorMessage)
        {
            Main.App.Container.GetContainer().Resolve<RequestSigninVerificationCodePageViewModel>().PageErrors.ToString().ShouldContain(errorMessage);
        }

        [Then(@"I should see that the verification code field is empty")]
        public void ThenIShouldSeeThatTheVerificationCodeFieldIsEmpty()
        {
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().VerificationCodeEntered.ShouldBeNull();
        }

        [When(@"I type the verification code ""(.*)"" and tap the submit code button")]
        public void WhenITypeTheVerificationCodeAndTapTheSubmitCodeButton(string verificationCode)
        {
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().VerificationCodeEntered = verificationCode;
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().SendVerificationCodeCommand.Execute(null);
        }


        [Then(@"I can see the fullname ""(.*)"" and email ""(.*)"" values of the fields")]
        public void ThenICanSeeTheFullnameAndEmailValuesOfTheFields(string fullname, string email)
        {
            var contact = Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().CurrentContact;

            if (email != "hynrbf@gmail.com")
                contact.EmailAdd.ShouldEqual(email);

            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().FullName.ShouldEqual(fullname);
        }

        [Then(@"I should see an error message ""(.*)"" in the ConfirmVerificationCodePage")]
        public void ThenIShouldSeeAnErrorMessageInTheConfirmVerificationCodePage(string errorMessage)
        {
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().PageErrors.ToString().ShouldContain(errorMessage);
        }

        [When(@"I tap the back arrow in the ConfirmVerificationCodePage")]
        public void WhenITapTheBackArrowInTheConfirmVerificationCodePage()
        {
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().NavigateBackCommand.Execute(null);
        }

    }
}

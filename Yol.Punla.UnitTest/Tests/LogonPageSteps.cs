using Prism.Unity;
using TechTalk.SpecFlow;
using Unity;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.ViewModels;
using Should;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public sealed class LogonPageSteps : StepBase
    {
        public LogonPageSteps(ScenarioContext scenarioContext) : base(scenarioContext) { }

        [When(@"I tap on the Sign In button")]
        public void WhenITapOnTheSignInButton()
        {
            Main.App.Container.GetContainer().Resolve<LogonPageViewModel>().GoToSigninWithAliasCommand.Execute(null);
        }

        [When(@"I enter my email address ""(.*)"" and tap on the submit button")]
        public void WhenIEnterMyEmailAddressAndTapOnTheSubmitButton(string emailAddress)
        {
            Main.App.Container.GetContainer().Resolve<RequestSigninVerificationCodePageViewModel>().EmailAddress = emailAddress;
            Main.App.Container.GetContainer().Resolve<RequestSigninVerificationCodePageViewModel>().RequestVerificationCodeCommand.Execute(null);
        }

        [When(@"I enter verification code ""(.*)"" and tap submit button")]
        public void WhenIEnterVerificationCodeAndTapSubmitButton(string verificationCode)
        {
            string[] splitCodes = verificationCode.Split('-');
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().VerificationCodeEntered1 = splitCodes[0];
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().VerificationCodeEntered2 = splitCodes[1];
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().VerificationCodeEntered3 = splitCodes[2];
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().VerificationCodeEntered4 = splitCodes[3];
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().SendVerificationCodeCommand.Execute(null);
        }

        [Then(@"I should see an error message ""(.*)""")]
        public void ThenIShouldSeeAnErrorMessage(string errorMessage)
        {
            Main.App.Container.GetContainer().Resolve<ConfirmVerificationCodePageViewModel>().PageErrors.ToString().ShouldEqual(errorMessage);
        }
    }
}

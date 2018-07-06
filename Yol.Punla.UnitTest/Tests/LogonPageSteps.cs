using Unity;
using Should;
using TechTalk.SpecFlow;
using Yol.Punla.FakeEntries;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.ViewModels;
using Prism.Unity;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public sealed class LogonPageSteps : StepBase
    {
        public LogonPageSteps(ScenarioContext scenarioContext) : base(scenarioContext) { }

        [When(@"I tap the Login with Facebook Button with account ""(.*)""")]
        public void WhenITapTheLoginWithFacebookButtonWithAccount(string fbEmail)
        {
            Main.App.Container.GetContainer().Resolve<ContactEntry>().EmailAddress = fbEmail;
            Main.App.Container.GetContainer().Resolve<LogonPageViewModel>().FacebookLogonCommand.Execute(null);
        }

        [When(@"I tap the Login with Facebook Button with mobile account ""(.*)""")]
        public void WhenITapTheLoginWithFacebookButtonWithMobileAccount(string fbMobileNumber)
        {
            Main.App.Container.GetContainer().Resolve<ContactEntry>().MobilePhone = fbMobileNumber;
            Main.App.Container.GetContainer().Resolve<LogonPageViewModel>().FacebookLogonCommand.Execute(null);
        }

        [Then(@"I should see not registered account message is displayed")]
        public void ThenIShouldSeeNotRegisteredAccountMessageIsDisplayed()
        {
            Main.App.Container.GetContainer().Resolve<NativeFacebookPageViewModel>().IsLogonIncorrectMessageDisplayed.ShouldBeTrue();
        }
    }
}

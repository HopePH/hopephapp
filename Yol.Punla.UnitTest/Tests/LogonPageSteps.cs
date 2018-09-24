using Prism.Unity;
using Should;
using TechTalk.SpecFlow;
using Unity;
using Yol.Punla.FakeEntries;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.ViewModels;

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
    }
}

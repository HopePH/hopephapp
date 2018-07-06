using TechTalk.SpecFlow;
using Yol.Punla.UnitTest.Barrack;
using Should;
using Yol.Punla.Authentication;
using Prism.Unity;
using Unity;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public class AuthenticationSteps : StepBase
    {
        public AuthenticationSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [Given(@"I am not authenticated")]
        public void GivenIAmNotAuthenticated() => Main.App.Container.GetContainer().Resolve<IAppUser>().IsAuthenticated.ShouldBeFalse();

        [Given(@"I am authenticated")]
        [Then(@"I am authenticated")]
        public void GivenIAmAuthenticated() => Main.App.Container.GetContainer().Resolve<IAppUser>().IsAuthenticated.ShouldBeTrue();
    }
}

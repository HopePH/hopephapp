using TechTalk.SpecFlow;

namespace Yol.Punla.UnitTest.WebAPI.Tests.Common
{
    [Binding]
    public class AuthenticationStep : StepBase
    {
        public AuthenticationStep(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [Given(@"the api client is authenticated")]
        public void GivenTheApiClientIsAuthenticated()
        {

        }
    }
}

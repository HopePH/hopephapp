using TechTalk.SpecFlow;

namespace Yol.Punla.UnitTest.Barrack
{
    public class StepBase
    {
        protected readonly ScenarioContext _scenarioContext;
        public UnitTestApp App => Main.App;

        public StepBase(ScenarioContext scenarioContext)
            => _scenarioContext = scenarioContext;
    }
}

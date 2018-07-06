using Unity;
using Should;
using System.Linq;
using TechTalk.SpecFlow;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.ViewModels;
using Prism.Unity;

namespace Yol.Punla.UnitTest
{
    [Binding]
    public class CrisisHotlineListPageSteps : StepBase
    {
        public CrisisHotlineListPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [When(@"I tap the Crisis icon from the menu detail")]
        public void WhenITapTheCrisisIconFromTheMenuDetail() => Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().CrisisCommand.Execute(null);

        [Given(@"I should see ""(.*)"" as one of the crisis hotline")]
        [Then(@"I should see ""(.*)"" as one of the crisis hotline")]
        public void ThenIShouldSeeAsOneOfTheCrisisHotline(string hotline)
        {
            var hotlines = Main.App.Container.GetContainer().Resolve<CrisisHotlineListPageViewModel>().CrisisHotlines.Where(x => x.PhoneNumber == hotline);
            hotlines.Count().ShouldBeGreaterThan(0);
        }
    }
}

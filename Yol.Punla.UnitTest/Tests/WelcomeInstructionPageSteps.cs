using Prism.Unity;
using Should;
using TechTalk.SpecFlow;
using Unity;
using Yol.Punla.UnitTest.Barrack;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public sealed class WelcomeInstructionPageSteps : StepBase
    {
        public WelcomeInstructionPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }
        
        [Then(@"I should see the back arrow of the page grayed")]
        [Given(@"I should see the back arrow of the page grayed")]
        public void GivenIShouldSeeTheBackArrowOfThePageGrayed()
        {
            var viewModel = Main.App.Container.GetContainer().Resolve<ViewModels.WelcomeInstructionsPageViewModel>();
            viewModel.IsNotFirstInstruction.ShouldBeFalse();
        }

        [Given(@"the back button is grayed")]
        public void GivenTheBackButtonIsGrayed()
        {
            Main.App.Container.GetContainer().Resolve<ViewModels.WelcomeInstructionsPageViewModel>().IsNotFirstInstruction.ShouldBeFalse();
        }

        [Given(@"I could see the back button is enabled")]
        [Then(@"I should see the back button is enabled")]
        public void ThenICouldSeeTheBackButtonIsEnabled()
        {
            Main.App.Container.GetContainer().Resolve<ViewModels.WelcomeInstructionsPageViewModel>().IsNotFirstInstruction.ShouldBeTrue();
        }
        
        [Given(@"I could see the instruction text ""(.*)""")]
        [Then(@"I should see the instruction text ""(.*)""")]
        public void GivenIShouldSeeTheInstructionText(string instructionText)
        {
            Main.App.Container.GetContainer().Resolve<ViewModels.WelcomeInstructionsPageViewModel>().InstructionContent.TextInstruction.ShouldEqual(instructionText);
        }

        [When(@"I tap the forward arrow")]
        public void WhenITapTheForwardArrow()
        {
            Main.App.Container.GetContainer().Resolve<ViewModels.WelcomeInstructionsPageViewModel>().NextCommand.Execute(null);
        }

        [When(@"I tap the forward arrow three times")]
        public void WhenITapTheForwardArrowThreeTimes()
        {
            for (int i = 0; i < 3; i++)
                Main.App.Container.GetContainer().Resolve<ViewModels.WelcomeInstructionsPageViewModel>().NextCommand.Execute(null);
        }

        [When(@"I tap the forward arrow two times")]
        public void WhenITapTheForwardArrowTwoTimes()
        {
            for (int i = 0; i < 2; i++)
                Main.App.Container.GetContainer().Resolve<ViewModels.WelcomeInstructionsPageViewModel>().NextCommand.Execute(null);
        }


        [When(@"I tap the back arrow")]
        public void WhenITapTheBackArrow()
        {
            Main.App.Container.GetContainer().Resolve<ViewModels.WelcomeInstructionsPageViewModel>().PrevCommand.Execute(null);
        }

        [Then(@"I should see the back button is ""(.*)""")]
        public void GivenICouldSeeTheBackButtonIs(bool backButtonState)
        {
            Main.App.Container.GetContainer().Resolve<ViewModels.WelcomeInstructionsPageViewModel>().IsNotFirstInstruction.ShouldEqual(backButtonState);
        }
        
    }
}

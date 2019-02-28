using System;
using TechTalk.SpecFlow;
using Prism.Unity;
using Should;
using System.Linq;
using Unity;
using Yol.Punla.Barrack;
using Yol.Punla.FakeEntries;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.UnitTest.Mocks;
using Yol.Punla.Utility;
using Yol.Punla.ViewModels;

namespace Yol.Punla.UnitTest
{
    [Binding]
    public class SignUpPageSteps : StepBase
    {
        public SignUpPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
           
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            
        }
        
        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
           
        }

        [Given(@"I am just testing yet")]
        public void GivenIAmJustTestingYet()
        {
           
        }

        [Then(@"I am good in testing")]
        public void ThenIAmGoodInTesting()
        {
           
        }

        [When(@"I tap the signup link below")]
        public void WhenITapTheSignupLinkBelow()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I enter my email address ""(.*)"" and tap continue button")]
        public void WhenIEnterMyEmailAddressAndTapContinueButton(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the verification boxes appear")]
        public void ThenTheVerificationBoxesAppear()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I enter my alias and mobile no and tap save button")]
        public void WhenIEnterMyAliasAndMobileNoAndTapSaveButton()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I type my verification code code-a ""(.*)"", code-b ""(.*)"", code-c ""(.*)"", code-d ""(.*)"", and tap the continue button")]
        public void WhenITypeMyVerificationCodeCode_ACode_BCode_CCode_DAndTapTheContinueButton(int p0, int p1, int p2, int p3)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I should see an error message ""(.*)"" in verification page")]
        public void ThenIShouldSeeAnErrorMessageInVerificationPage(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}

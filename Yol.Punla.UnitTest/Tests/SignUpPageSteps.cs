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
            Main.App.Container.GetContainer().Resolve<LogonPageViewModel>().IsOpen = true;
            Main.App.Container.GetContainer().Resolve<LogonPageViewModel>().GoToSignUpCommand.Execute(null);
        }

        [When(@"I enter my email address ""(.*)"" and tap continue button")]
        public void WhenIEnterMyEmailAddressAndTapContinueButton(string emailAdd)
        {
            Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().EmailAddress = emailAdd;
            Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().SendVerificationCodeCommand.Execute(null);
        }

        [Then(@"the verification boxes appear")]
        public void ThenTheVerificationBoxesAppear()
        {
            //Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().IsVerificationNegation.ShouldBeTrue();
        }

        [When(@"I type my verification code code-a ""(.*)"", code-b ""(.*)"", code-c ""(.*)"", code-d ""(.*)"", and tap the continue button")]
        public void WhenITypeMyVerificationCodeCode_ACode_BCode_CCode_DAndTapTheContinueButton(int code1, int code2, int code3, int code4)
        {
            Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().VerificationCodeEntered1 = code1.ToString();
            Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().VerificationCodeEntered2 = code2.ToString();
            Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().VerificationCodeEntered3 = code3.ToString();
            Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().VerificationCodeEntered4 = code4.ToString();
            Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().SendVerificationCodeCommand.Execute(null);
        }

        [Then(@"I should see an error message ""(.*)"" in verification page")]
        public void ThenIShouldSeeAnErrorMessageInVerificationPage(string errorMessage)
            => Main.App.Container.GetContainer().Resolve<EmailVerificationPageViewModel>().ConfirmVerificationCode.ToString().ShouldContain(errorMessage);

        [When(@"I tap the change photo and select unicorn")]
        public void WhenITapTheChangePhotoAndSelectUnicorn()
        {
            string sourceUrl = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/womanavatar.png";
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().SetAvatarUrlCommand.Execute(new Avatar { Name = "Unicorn", SourceUrl = sourceUrl });
        }

        [Then(@"I should see the photo is change to unicorn")]
        public void ThenIShouldSeeThePhotoIsChangeToUnicorn()
            => Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().Picture.ShouldNotBeEmpty();

        [When(@"I enter my alias ""(.*)"" and mobile no ""(.*)"" and tap save button")]
        public void WhenIEnterMyAliasAndMobileNoAndTapSaveButton(int alias, int mobileno)
        {
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().AliasName = alias.ToString();
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().MobilePhoneNo = mobileno.ToString();
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().SignupCommand.Execute(null);
        }
    }
}

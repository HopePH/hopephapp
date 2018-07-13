using Prism.Unity;
using Should;
using System.Linq;
using TechTalk.SpecFlow;
using Unity;
using Yol.Punla.Barrack;
using Yol.Punla.FakeEntries;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.UnitTest.Mocks;
using Yol.Punla.Utility;
using Yol.Punla.ViewModels;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public sealed class SignUpPageSteps : StepBase
    {
        public SignUpPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [When(@"I tap the Sign up link text")]
        public void WhenITapTheSignUpLinkText() => Main.App.Container.GetContainer().Resolve<LogonPageViewModel>().GoToSignUpCommand.Execute(null);

        [When(@"I tap the Sign up with Facebook Button")]
        public void WhenITapTheSignUpWithFacebookButton() => Main.App.Container.GetContainer().Resolve<SignUpPageViewModel>().FacebookSignUpCommand.Execute(null);

        [When(@"I tap the Sign up with Facebook Button using facebook email account ""(.*)""")]
        public void WhenITapTheSignUpWithFacebookButtonUsingFacebookEmailAccount(string fbEmail)
        {
            Main.App.Container.GetContainer().Resolve<ContactEntry>().EmailAddress = fbEmail;
            Main.App.Container.GetContainer().Resolve<SignUpPageViewModel>().FacebookSignUpCommand.Execute(null);
        }

        [Then(@"I can see the fullname ""(.*)"", email ""(.*)"", alias ""(.*)"", mobile phone ""(.*)"" and Photo ""(.*)"" values of the fields")]
        public void ThenICanSeeTheFullnameEmailAliasMobilePhoneAndPhotoValuesOfTheFields(string fullname, string email, string alias, string phone, string photo)
        {
            var contact = Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().CurrentContact;

            if(email != "hynrbf@gmail.com")
                contact.EmailAdd.ShouldEqual(email);

            contact.AliasName.ShouldEqual(alias);
            contact.MobilePhone.ShouldEqual(phone);
            contact.FBLink.ShouldEqual(photo);
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().FullName.ShouldEqual(fullname);
        }

        [Then(@"I could see my default avatar picture ""(.*)""")]
        public void ThenICouldSeeMyDefaultAvatarPicture(string avatarPicture)
        {
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().Picture.ShouldEqual(avatarPicture);
        }

        [When(@"I tap the Change Photo button")]
        public void WhenITapTheChangePhotoButton()
        {
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().ShowOrHideAvatarSelectionCommand.Execute(true);
        }

        [Then(@"I should see the list of avatar images to choose from")]
        public void ThenIShouldSeeTheListOfAvatarImagesToChooseFrom()
        {
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().IsAvatarModalVisible.ShouldBeTrue();
        }

        [When(@"I tap on the avatar image ""(.*)"" that I choose")]
        public void WhenITapOnTheAvatarImageThatIChoose(string avatarImage)
        {
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().SetAvatarUrlCommand.Execute(new Model.Avatar {  SourceUrl = avatarImage });
        }

        [Then(@"I should see that my default picture was changed to ""(.*)""")]
        public void ThenIShouldSeeThatMyDefaultPictureWasChangedTo(string newPicture)
        {
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().Picture.ShouldEqual(newPicture);
        }

        [When(@"I enter email address ""(.*)"", alias ""(.*)"", and mobile no ""(.*)"" I tap the Save and Continue button")]
        public void WhenIEnterEmailAddressAliasAndMobileNoITapTheSaveAndContinueButton(string emailAdd, string aliasName, string mobileNo)
        {
            //chito. since this is a new contact added to datbase I will just pretend that the ID inserted is 1000
            ((KeyValueCacheMock)AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>()).SetCurrentContactIdManually(1000);

            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().EmailAddress = emailAdd;
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().AliasName = aliasName;
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().MobilePhoneNo = mobileNo;
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().SignupCommand.Execute(null);
        }

        [Then(@"I should see registration error message ""(.*)""")]
        public void ThenIShouldSeeRegistrationErrorMessage(string errorMessage) 
            => Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().PageErrors.ToString().ShouldContain(errorMessage);

        [When(@"I tap the Sign up with Facebook Button using facebook mobile no ""(.*)"" and I tap the Save and Continue button")]
        public void WhenIEnterFbMobileNoAndITapTheSaveAndContinueButton(string mobileNo)
        {
            Main.App.Container.GetContainer().Resolve<ContactEntry>().MobilePhone = mobileNo;
            Main.App.Container.GetContainer().Resolve<SignUpPageViewModel>().FacebookSignUpCommand.Execute(null);
        }

        [Then(@"I should see a duplicate pop message is displayed")]
        public void ThenIShouldSeeADuplicatePopMessageIsDisplayed()
        {
            Main.App.Container.GetContainer().Resolve<NativeFacebookPageViewModel>().IsPopUpDisplayed.ShouldBeTrue();
        }

        [When(@"I tap on the ChangePhoto button")]
        public void WhenITapOnTheChangePhotoButton()
        {
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().RetakePhotoCommand.Execute(null);
        }

        [Then(@"I should see the popup that displays the list of avatar")]
        public void ThenIShouldSeeThePopupThatDisplaysTheListOfAvatar()
        {
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().IsAvatarModalVisible.ShouldBeTrue();
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().PredefinedAvatars.Count().ShouldBeGreaterThan(1);
        }

        [When(@"I choose the chicken avatar ""(.*)""")]
        public void WhenIChooseTheChickenAvatar(string avatarName)
        {
            var avatarSelected = Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().PredefinedAvatars.Where(x => x.Name == avatarName).FirstOrDefault();
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().SetAvatarUrlCommand.Execute(avatarSelected);
        }

        [Then(@"I should see that the account photo was changed to ""(.*)""")]
        public void ThenIShouldSeeThatTheAccountPhotoWasChangedTo(string avatarUrl)
        {
            Main.App.Container.GetContainer().Resolve<AccountRegistrationPageViewModel>().Picture.ShouldEqual(avatarUrl);
        }

      
    }
}

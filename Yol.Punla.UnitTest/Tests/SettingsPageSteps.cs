using Prism.Unity;
using Should;
using TechTalk.SpecFlow;
using Unity;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.UnitTest.Mocks;
using Yol.Punla.ViewModels;

namespace Yol.Punla.UnitTest
{
    [Binding]
    public class SettingsPageSteps
    {
        [Then(@"I could see that the settings item in the side menu is not displayed")]
        public void ThenICouldSeeThatTheSettingsItemInTheSideMenuIsNotDisplayed()
        {
            Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().IsShowOnlyWhenLogon.ShouldBeFalse();
        }

        [Then(@"I could see that the settings item in the side menu is displayed")]
        public void ThenICouldSeeThatTheSettingsItemInTheSideMenuIsDisplayed()
        {
            Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().IsShowOnlyWhenLogon.ShouldBeTrue();
        }

        [When(@"I tap the Settings item in the side menu")]
        public void WhenITapTheSettingsItemInTheSideMenu()
        {
            Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().SettingsPageCommand.Execute(null);
        }

        [Then(@"I should see that the dialog is not yet shown")]
        public void ThenIShouldSeeThatTheDialogIsNotYetShown()
        {
            Main.App.Container.GetContainer().Resolve<SettingsPageViewModel>().DialogResult.ShouldBeNull();
        }

        [When(@"I tap on the action item Edit Profile")]
        public void WhenITapOnTheActionItemEditProfile()
        {
            Main.App.Container.GetContainer().Resolve<SettingsPageViewModel>().EditProfileCommand.Execute(null);
        }

        [Then(@"I should see a not yet available message ""(.*)""")]
        public void ThenIShouldSeeANotYetAvailableMessage(string dialogMessage)
        {
            var alertResult = (AlertResult)Main.App.Container.GetContainer().Resolve<SettingsPageViewModel>().DialogResult;
            alertResult.Message.ShouldEqual(dialogMessage);
        }

    }
}

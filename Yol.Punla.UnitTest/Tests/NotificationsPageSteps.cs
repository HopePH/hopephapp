using TechTalk.SpecFlow;
using Yol.Punla.UnitTest.Barrack;
using Should;
using Unity;
using Yol.Punla.FakeEntries;
using System.Linq;
using Prism.Unity;
using Yol.Punla.ViewModels;
using Yol.Punla.UnitTest.Mocks;
using Plugin.Connectivity.Abstractions;

namespace Yol.Punla.UnitTest
{
    [Binding]
    public class NotificationsPageSteps : StepBase
    {
        public NotificationsPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [Given(@"there are no username and password yet stored in the app")]
        public void GivenThereAreNoUsernameAndPasswordYetStoredInTheApp()
        {
            Main.App.UserName.ShouldBeNull();
            Main.App.Password.ShouldBeNull();
        }
        
        [Then(@"I should see that there are no post feeds to be pushed into the local notifications")]
        public void ThenIShouldSeeThatThereAreNoPostFeedsToBePushedIntoTheLocalNotifications()
        {
            Main.App.UnSupportedPostFeeds.ShouldBeNull();
        }

        [Given(@"the username ""(.*)"" and password are stored in the app")]
        public void GivenTheUsernameAndPasswordAreStoredInTheApp(string emailAddress)       
        {
            Main.App.UserName.ShouldNotBeNull();
            Main.App.Password.ShouldNotBeNull();
        }

        [Then(@"I should see that there are post feeds that is pushed into the local notifications")]
        public void ThenIShouldSeeThatThereArePostFeedsThatIsPushedIntoTheLocalNotifications()
        {
            Main.App.UnSupportedPostFeeds.ShouldNotBeNull();
            Main.App.UnSupportedPostFeeds.Count().ShouldBeGreaterThan(0);
        }

        [Then(@"I should see that the username and password are stored in the app")]
        public void ThenIShouldSeeThatTheUsernameAndPasswordAreStoredInTheApp()
        {
            Main.App.UserName.ShouldNotBeNull();
            Main.App.Password.ShouldNotBeNull();
        }

        [Then(@"I should see that the notifications quantity is displayed at the top banner of the WikiPage")]
        public void ThenIShouldSeeThatTheNotificationsQuantityIsDisplayedAtTheTopBannerOfTheWikiPage()
        {
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().NoOfNotifications.ShouldBeGreaterThan(1);
        }

        [Then(@"I should see that the notifications quantity is displayed at the top banner of the WikiPage in only ""(.*)""")]
        public void ThenIShouldSeeThatTheNotificationsQuantityIsDisplayedAtTheTopBannerOfTheWikiPageInOnly(int noOfNotifications)
        {
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().NoOfNotifications.ShouldEqual(noOfNotifications);
        }

        public void WhenITapTheNotificationsFromTheMenuDetail()
          => Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().NotificationsCommand.Execute(null);

        [Then(@"I should see that there are notifications displayed on the page")]
        public void ThenIShouldSeeThatThereAreNotificationsDisplayedOnThePage()
        {
            Main.App.Container.GetContainer().Resolve<NotificationsPageViewModel>().Notifications.ShouldNotBeNull();
            Main.App.Container.GetContainer().Resolve<NotificationsPageViewModel>().Notifications.Count().ShouldBeGreaterThan(1);
        }

        [When(@"I tap the notifications icon but suddenly the internet went off")]
        public void WhenITapTheNotificationsIconButSuddenlyTheInternetWentOff()
        {
            ((CrossConnectivityMock)Main.App.Container.GetContainer().Resolve<IConnectivity>()).SetConnectionManually(false);
            Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().NotificationsCommand.Execute(null);
        }

        [Then(@"I should see that there are no notifications displayed on the page")]
        public void ThenIShouldSeeThatThereAreNoNotificationsDisplayedOnThePage()
        {
            Main.App.Container.GetContainer().Resolve<NotificationsPageViewModel>().Notifications.ShouldBeNull();
        }

        [Then(@"I should see that the offline message is displayed")]
        public void ThenIShouldSeeThatTheOfflineMessageIsDisplayed()
        {
            Main.App.Container.GetContainer().Resolve<NotificationsPageViewModel>().IsShowOfflineMessage.ShouldBeTrue();
        }

        [When(@"I tap the Write Down icon from the menu detail pane")]
        public void WhenITapTheWriteDownIconFromTheMenuDetailPane()
        {
            Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().WriteDownCommand.Execute(null);
        }

        [When(@"I pull to refresh the notifications")]
        public void WhenIPullToRefreshTheNotifications()
        {
            Main.App.Container.GetContainer().Resolve<NotificationsPageViewModel>().RefreshListCommand.Execute(null);
        }

        [Then(@"I should see that the notifications are refreshed")]
        public void ThenIShouldSeeThatTheNotificationsAreRefreshed()
        {
            Main.App.Container.GetContainer().Resolve<NotificationsPageViewModel>().Notifications.ShouldNotBeNull();
        }

        [When(@"I select a post from the notifications list")]
        public void WhenISelectAPostFromTheNotificationsList()
        {
            var selectedPost = Main.App.Container.GetContainer().Resolve<NotificationsPageViewModel>().Notifications.FirstOrDefault();
            Main.App.Container.GetContainer().Resolve<NotificationsPageViewModel>().CurrentPostFeed = selectedPost;
        }
    }
}

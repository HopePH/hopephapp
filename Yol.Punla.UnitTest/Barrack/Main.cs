using System;
using TechTalk.SpecFlow;
using Yol.Punla.UnitTest.Tests;

namespace Yol.Punla.UnitTest.Barrack
{
    [Binding]
    public class Main
    {
        private const string LOGONUSERMOBILE = "09477691857";
        private const string LOGONUSEREMAIL = "hynrbf@gmail.com";

        public static UnitTestApp App { get; private set; }

        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            AuthenticatedAlreadyOrNot(false);
            AppInitsHolder.IsWelcomeInstructionsLoaded = true;
            Xamarin.Forms.Mocks.MockForms.Init();
            App = new UnitTestApp();
        }

        [BeforeScenario("WelcomeInstructionIsNotLoadedYet", Order = 1)]
        public void FirstTimeLaunch()
        {
            //chito.manually set the welcome instruction to be not loaded yet           
            AppInitsHolder.IsWelcomeInstructionsLoaded = false;
            App = new UnitTestApp();
        }

        [BeforeScenario("FacebookLogonViaCrisisMenu", Order = 1)]
        public void FacebookLogonViaCrisisMenu()
        {
            var authenticationSteps = new AuthenticationSteps(ScenarioContext.Current);
            authenticationSteps.GivenIAmNotAuthenticated();

            var navigationSteps = new NavigationSteps(ScenarioContext.Current);
            navigationSteps.ThenIAmRedirectedToThePage("WikiPage");

            var homePageSteps = new HomePageSteps(ScenarioContext.Current);
            homePageSteps.GivenTheMenuDetailIsClosed();
            homePageSteps.WhenITapTheHanburgerIcon();
            homePageSteps.ThenIShouldSeeTheMenuDetailIsOpened();

            var crisisPageSteps = new CrisisHotlineListPageSteps(ScenarioContext.Current);
            crisisPageSteps.WhenITapTheCrisisIconFromTheMenuDetail();
            navigationSteps.ThenIAmRedirectedToThePage("LogonPage");

            var loginSteps = new LogonPageSteps(ScenarioContext.Current);
            loginSteps.WhenITapTheLoginWithFacebookButtonWithAccount(LOGONUSEREMAIL);
        }

        [BeforeScenario("FacebookLogonViaWriteYourThoughtsMenu", Order = 1)]
        public void FacebookLogonViaWriteYourToughtsMenu()
        {
            var authenticationSteps = new AuthenticationSteps(ScenarioContext.Current);
            authenticationSteps.GivenIAmNotAuthenticated();

            var navigationSteps = new NavigationSteps(ScenarioContext.Current);
            navigationSteps.ThenIAmRedirectedToThePage("WikiPage");

            var homePageSteps = new HomePageSteps(ScenarioContext.Current);
            homePageSteps.GivenTheMenuDetailIsClosed();
            homePageSteps.WhenITapTheHanburgerIcon();
            homePageSteps.ThenIShouldSeeTheMenuDetailIsOpened();

            var postFeedPageSteps = new PostFeedPageSteps(ScenarioContext.Current);
            postFeedPageSteps.WhenITapTheWriteDownIconFromTheMenuDetail();
            navigationSteps.ThenIAmRedirectedToThePage("LogonPage");

            var loginSteps = new LogonPageSteps(ScenarioContext.Current);
            loginSteps.WhenITapTheLoginWithFacebookButtonWithMobileAccount(LOGONUSERMOBILE);
        }

        [BeforeScenario("FacebookLogonViaNotificationsMenu", Order = 1)]
        public void FacebookLogonViaNotificationsMenu()
        {
            App = new UnitTestApp();
            var authenticationSteps = new AuthenticationSteps(ScenarioContext.Current);
            authenticationSteps.GivenIAmNotAuthenticated();

            var navigationSteps = new NavigationSteps(ScenarioContext.Current);
            navigationSteps.ThenIAmRedirectedToThePage("WikiPage");

            var homePageSteps = new HomePageSteps(ScenarioContext.Current);
            homePageSteps.GivenTheMenuDetailIsClosed();
            homePageSteps.WhenITapTheHanburgerIcon();
            homePageSteps.ThenIShouldSeeTheMenuDetailIsOpened();

            var notificationsPageSteps = new NotificationsPageSteps(ScenarioContext.Current);
            notificationsPageSteps.WhenITapTheNotificationsFromTheMenuDetail();
            navigationSteps.ThenIAmRedirectedToThePage("LogonPage");

            var loginSteps = new LogonPageSteps(ScenarioContext.Current);
            loginSteps.WhenITapTheLoginWithFacebookButtonWithMobileAccount(LOGONUSERMOBILE);
        }

        //#the OnResume bdd not working good
        [BeforeScenario("OnResume", Order = 1)]
        public void OnResume()
        {
            AuthenticatedAlreadyOrNot(false);
            UserAndPasswordSavedAlreadyOrNot(false);
            App = new UnitTestApp();
            App.OnResumePublic().Wait();
        }

        [BeforeScenario("OnResumeLogon", Order = 1)]
        public void OnResumeLogon()
        {
            AuthenticatedAlreadyOrNot(true);
            UserAndPasswordSavedAlreadyOrNot(true);
            App = new UnitTestApp();
            App.OnResumePublic().Wait();
        }

        [BeforeScenario("OnResumeLogonAndPushedDateNotExpired", Order = 1)]
        public void OnResumeLogonAndPushedDateNotExpired()
        {
            AuthenticatedAlreadyOrNot(true);
            UserAndPasswordSavedAlreadyOrNot(true);
            AppInitsHolder.NotificationsPushedDateSaved = DateTime.Now;
            App = new UnitTestApp();
            App.OnResumePublic().Wait();
        }

        private void AuthenticatedAlreadyOrNot(bool authenticatedAlready)
        {
            AppInitsHolder.WasLogin = authenticatedAlready;
            AppInitsHolder.WasSignUpCompleted = authenticatedAlready;
        }

        private void UserAndPasswordSavedAlreadyOrNot(bool userAndPwSavedOrNot)
        {
            if (userAndPwSavedOrNot)
            {
                AppInitsHolder.UserName = LOGONUSEREMAIL;
                AppInitsHolder.Password = "Mumba7876";
            }
            else
            {
                AppInitsHolder.UserName = null;
                AppInitsHolder.Password = null;
            }
        }
    }
}

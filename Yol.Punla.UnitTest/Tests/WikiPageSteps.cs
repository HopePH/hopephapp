using Unity;
using Should;
using System.Linq;
using TechTalk.SpecFlow;
using Yol.Punla.FakeData;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.UnitTest.Mocks;
using Yol.Punla.Utility;
using Yol.Punla.ViewModels;
using Prism.Unity;
using Yol.Punla.Barrack;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public sealed class WikiPageSteps : StepBase
    {
        public WikiPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [Given(@"I can see a list of Wikis and the wiki with title ""(.*)""")]
        public void GivenICanSeeAListOfWikisAndTheWikiWithTitle(string wikiTitle)
        {
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().Wikis.Count().ShouldBeGreaterThan(0);
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>()
                .Wikis
                    .Where(w => w.Title == wikiTitle)
                        .Count()
                            .ShouldBeGreaterThan(0);
        }

        [When(@"I tap the item with title ""(.*)""")]
        public void WhenITapTheItemWithTitle(string wikiTitle)
        {
            var selectedWiki = Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().Wikis.Where(w => w.Title == wikiTitle).First();
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().ItemNavigationCommand.Execute(selectedWiki);
        }

        [Then(@"I can see the title of the wiki ""(.*)"" with content ""(.*)""")]
        public void ThenICanSeeTheTitleOfTheWikiWithContent(string wikiTitle, string content)
        {
            Main.App.Container.GetContainer().Resolve<WikiDetailsPageViewModel>().ItemDetails.ShouldContain(wikiTitle);
            Main.App.Container.GetContainer().Resolve<WikiDetailsPageViewModel>().ItemDetails.ShouldContain(content);
        }

        [When(@"I tap the back icon from WikiDetailsPage")]
        public void WhenITapTheBackIconFromWikiDetailsPage() => Main.App.Container.GetContainer().Resolve<WikiDetailsPageViewModel>().NavigateBackCommand.Execute(null);

        [Then(@"the wiki sort modal should disappear")]
        public void ThenTheWikiSortModalShouldDisappear() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsShowSortModal.ShouldBeFalse();

        [Then(@"I should see the wiki sort modal appear")]
        public void ThenIShouldSeeTheWikiSortModalAppear() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsShowSortModal.ShouldBeTrue();

        [When(@"I tap the Sort tab above the wiki list")]
        public void WhenITapTheSortTabAboveTheWikiList() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().ShowOrHideSortModal.Execute(true);

        [When(@"I tap the close icon of the wiki sort modal")]
        public void WhenITapTheCloseIconOfTheWikiSortModal() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().ShowOrHideSortModal.Execute(false);

        [When(@"I tap the Filter tab above the wiki list")]
        public void WhenITapTheFilterTabAboveTheWikiList() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().ShowOrHideFilterModal.Execute(true);

        [Then(@"I should see the wiki filter modal appear")]
        public void ThenIShouldSeeTheWikiFilterModalAppear() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsShowFilterModal.ShouldBeTrue();

        [When(@"I tap the close icon of the wiki filter modal")]
        public void WhenITapTheCloseIconOfTheWikiFilterModal() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().ShowOrHideFilterModal.Execute(false);

        [Then(@"the wiki filter modal should disappear")]
        public void ThenTheWikiFilterModalShouldDisappear() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsShowFilterModal.ShouldBeFalse();

        [Given(@"I could see that the wiki sort dialog is not displayed")]
        public void GivenICouldSeeThatTheWikiSortDialogIsNotDisplayed() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsShowSortModal.ShouldBeFalse();

        [Given(@"I could see the first item title ""(.*)""")]
        public void GivenICouldSeeTheFirstItemTitle(string title) => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().Wikis.First().Title.ShouldEqual(title);

        [When(@"I tap the sort button at the top in the wiki page")]
        public void WhenITapTheSortButtonAtTheTopInTheWikiPage() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().ShowOrHideSortModal.Execute(true);

        [Then(@"I should see the wiki sort dialog is displayed")]
        public void ThenIShouldSeeTheWikiSortDialogIsDisplayed() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsShowSortModal.ShouldBeTrue();

        [When(@"I choose the option alphabetically and tap the sort button")]
        public void WhenIChooseTheOptionAlphabeticallyAndTapTheSortButton()
        {
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsSortAlphabetical = true;
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().SortCommand.Execute(null);
        }

        [Then(@"I should see the first item title ""(.*)""")]
        public void ThenIShouldSeeTheFirstItemTitle(string title) => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().Wikis.First().Title.ShouldEqual(title);

        [When(@"I choose the tap the sort button without choosing alphabetical input")]
        public void WhenIChooseTheTapTheSortButtonWithoutChoosingAlphabeticalInput() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().SortCommand.Execute(null);

        [Then(@"I should see the wiki page alert error ""(.*)""")]
        public void ThenIShouldSeeTheWikiPageAlertError(string errorMessage) => ((AlertResult)Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().AlertResult).Message.ShouldEqual(errorMessage);

        [Given(@"I could see the ad message ""(.*)""")]
        public void GivenICouldSeeTheAdMessage(string adMessage)
        {
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().AdMessage.ShouldContain(adMessage);
        }

        [Given(@"I could see the current app version no ""(.*)""")]
        public void GivenICouldSeeTheCurrentAppVersionNo(string currentVersion)
        {
            var appCurrentVersion = ((KeyValueCacheMock)AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>()).GetUserDefaultsKeyValue("AppCurrentVersion");
            appCurrentVersion.ShouldEqual(currentVersion);
        }

        [When(@"I tap the back icon from WikiDetailsPage and the app force to download to version no was updated to ""(.*)"" from server")]
        public void WhenITapTheBackIconFromWikiDetailsPageAndTheAppForceToDownloadToVersionNoWasUpdatedToFromServer(string forceToUpdateVersionTo)       
        {
            var oneOfTheWikis = FakeWikis.Wikis.Where(x => !string.IsNullOrEmpty(x.ForceToVersionNo)).FirstOrDefault();
            oneOfTheWikis.ForceToVersionNo = forceToUpdateVersionTo;
            Main.App.Container.GetContainer().Resolve<WikiDetailsPageViewModel>().NavigateBackCommand.Execute(null);
        }

        [Then(@"I should see the pop message ""(.*)""")]
        public void ThenIShouldSeeThePopMessage(string message)
        {
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsForceUpdateVersion.ShouldBeTrue();
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().AdMessage.ShouldContain(message);
        }
    }
}
                                                                                                                                                                                                                                                                                                                                             
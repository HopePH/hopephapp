using Plugin.Connectivity.Abstractions;
using Prism.Unity;
using Should;
using System.Linq;
using TechTalk.SpecFlow;
using Unity;
using Yol.Punla.Barrack;
using Yol.Punla.Managers;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.UnitTest.Mocks;
using Yol.Punla.Utility;
using Yol.Punla.ViewModels;

namespace Yol.Punla.UnitTest
{
    [Binding]
    public class HomePageSteps : StepBase
    {
        public HomePageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [When(@"I tap the Mental Care Facilities icon from the menu detail")]
        public void WhenITapTheMentalCareFacilitiesIconFromTheMenuDetail()
        {
            Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().MentalCareFacilitiesPageCommand.Execute(null);
        }

        [When(@"I tap the Mental Care Facilities icon from the menu detail but the internet was off")]
        public void WhenITapTheHomePageIconFromTheMenuDetailButTheInternetWasOff()
        {
            ((CrossConnectivityMock)Main.App.Container.GetContainer().Resolve<IConnectivity>()).SetConnectionManually(false);
            Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().MentalCareFacilitiesPageCommand.Execute(null);
        }

        [Then(@"I should see the list of mental health care")]
        public void ThenIShouldSeeTheListOfMentalHealthCare() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().MentalFacilities.Count().ShouldBeGreaterThan(0);

        [Then(@"I should see one of the mental health facility ""(.*)""")]
        public void ThenIShouldSeeOneOfTheMentalHealthFacility(string mentalCareFacility) => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().MentalFacilities.Where(x => x.GroupName == mentalCareFacility).Count().ShouldBeGreaterThan(0);

        [Given(@"I should see the Alpa Adverstisement is displayed")]
        [Then(@"I should see the Alpa Adverstisement is displayed")]
        public void ThenIShouldSeeTheAlpaAdverstisementIsDisplayed() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsShowAlphaAd.ShouldBeTrue();

        [When(@"I tap the hamburger icon")]
        public void WhenITapTheHanburgerIcon() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().ShowSideBarCommand.Execute(null);

        [Then(@"the menu detail is closed")]
        [Given(@"the menu detail is closed")]
        public void GivenTheMenuDetailIsClosed() => Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().IsOpen.ShouldBeFalse();

        [Then(@"I should see the menu detail is opened")]
        public void ThenIShouldSeeTheMenuDetailIsOpened() => Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().IsOpen.ShouldBeTrue();

        [When(@"I tap the Useful Stuff item from the menu detail")]
        public void WhenITapTheUserfulStuffItemFromTheMenuDetail() => Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().InfoPageCommand.Execute(null);

        [When(@"I tap the mental care name ""(.*)"", address ""(.*)"", tel no ""(.*)"" photo url ""(.*)"", latitude ""(.*)"" and longitude ""(.*)""")]
        public void WhenITapTheMentalCareNameAddressTelNoPhotoUrlLatitudeAndLongitude(string name, string address, string telNo, string photoUrl, double latitude, double longitude)
        {
            Main.App.Container.GetContainer().Resolve<HomePageViewModel>().CurrentSelectedItem = new Entity.MentalHealthFacility
            {
                Location = address,
                GroupName = name,
                PhoneNumber = telNo,
                PhotoUrl = photoUrl,
                Latitude = latitude,
                Longitude = longitude
            };
        }

        [Then(@"I should see the name ""(.*)"", address ""(.*)"", tel no ""(.*)"" and photo url ""(.*)"" is displayed")]
        public void ThenIShouldSeeTheNameAddressTelNoAndPhotoUrlIsDisplayed(string name, string address, string telNo, string photoUrl)
        {
            Main.App.Container.GetContainer().Resolve<MentalCareDetailsPageViewModel>().Name.ShouldEqual(name);
            Main.App.Container.GetContainer().Resolve<MentalCareDetailsPageViewModel>().Address.ShouldEqual(address);
            Main.App.Container.GetContainer().Resolve<MentalCareDetailsPageViewModel>().PhoneNo.ShouldEqual(telNo);
            Main.App.Container.GetContainer().Resolve<MentalCareDetailsPageViewModel>().PhotoUrl.ShouldEqual(photoUrl);
        }

        [When(@"I tap item ""(.*)"" Call button")]
        public void WhenITapItemCallButton(string name)
        {
            var item = Main.App.Container.GetContainer().Resolve<HomePageViewModel>().MentalFacilities.Where(x => x.GroupName == name).FirstOrDefault();
            Main.App.Container.GetContainer().Resolve<HomePageViewModel>().ContactFacilityCommand.Execute(item);
        }

        [Then(@"I should see the phone no dialed ""(.*)""")]
        public void ThenIShouldSeeThePhoneNoDialed(string phoneNo) => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().PhoneNoDialed.ShouldEqual(phoneNo);

        [When(@"I dial the hotline no ""(.*)""")]
        public void WhenIDialTheHotlineNo(string telNo)
        {
            var hotlineNo = Main.App.Container.GetContainer().Resolve<CrisisHotlineListPageViewModel>().CrisisHotlines.Where(x => x.PhoneNumber == telNo).FirstOrDefault();
            Main.App.Container.GetContainer().Resolve<CrisisHotlineListPageViewModel>().CurrentSelectedItem = hotlineNo;
        }

        [Then(@"I should see the number ""(.*)"" was added to the dialer")]
        public void ThenIShouldSeeTheNumberWasAddedToTheDialer(string telNo) => Main.App.Container.GetContainer().Resolve<CrisisHotlineListPageViewModel>().HotlineDialed.ShouldEqual(telNo);

        [Then(@"I should see a list of Wikis and the wiki with title ""(.*)""")]
        public void ThenIShouldSeeAListOfWikisAndTheWikiWithTitle(string wikiTitle)
        {
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().Wikis.Count().ShouldBeGreaterThan(0);
            Main.App.Container.GetContainer().Resolve<WikiPageViewModel>()
                .Wikis
                    .Where(w => w.Title == wikiTitle)
                        .Count()
                            .ShouldBeGreaterThan(0);
        }

        [Then(@"I shouid see the ForceToVersionNo in one of the items in wiki")]
        public void ThenIShouidSeeTheForceToVersionNoInOneOfTheItemsInWiki()
        {
            var oneOfTheWikis = Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().Wikis.Where(x => !string.IsNullOrEmpty(x.ForceToVersionNo)).FirstOrDefault();
            oneOfTheWikis.ShouldNotBeNull();
        }

        [When(@"I tap the Filter tab above the list")]
        public void WhenITapTheFilterTabAboveTheList() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().ShowOrHideFilterModal.Execute(true);

        [Then(@"I should see the filter modal appear")]
        public void ThenIShouldSeeTheFilterModalAppear() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().IsShowFilterModal.ShouldBeTrue();

        [When(@"I tap the Sort tab above the list")]
        public void WhenITapTheSortTabAboveTheList() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().ShowOrHideSortModal.Execute(true);

        [Then(@"I should see the sort modal appear")]
        public void ThenIShouldSeeTheSortModalAppear() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().IsShowSortModal.ShouldBeTrue();

        [When(@"I tap the close icon of the filter modal")]
        public void WhenITapTheCloseIconOfTheFilterModal() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().ShowOrHideFilterModal.Execute(false);

        [When(@"I tap the close icon of the sort modal")]
        public void WhenITapTheCloseIconOfTheSortModal() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().ShowOrHideSortModal.Execute(false);

        [Then(@"the filter modal should disapear")]
        public void ThenTheFilterModalShouldDisapear() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().IsShowFilterModal.ShouldBeFalse();

        [Then(@"the sort modal should disapear")]
        public void ThenTheSortModalShouldDisapear() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().IsShowSortModal.ShouldBeFalse();

        [When(@"I tap the close Alpha Advertisement")]
        public void WhenITapTheCloseAlphaAdvertisement() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().ShowOrHideAdModal.Execute(false);

        [Then(@"I should see the Alpha Advertisement is closed")]
        public void ThenIShouldSeeTheAlphaAdvertisementIsClosed() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsShowAlphaAd.ShouldBeFalse();

        [When(@"I tap the back icon from Mental Care Details")]
        public void WhenITapTheBackIconFromMentalCareDetails() => Main.App.Container.GetContainer().Resolve<MentalCareDetailsPageViewModel>().NavigateBackCommand.Execute(null);

        [Then(@"I should see the Alpa Adverstisement is not displayed")]
        public void ThenIShouldSeeTheAlpaAdverstisementIsNotDisplayed() => Main.App.Container.GetContainer().Resolve<WikiPageViewModel>().IsShowAlphaAd.ShouldBeFalse();

        [When(@"I tap the View Map Button")]
        public void WhenITapTheViewMapButton()
        {
            Main.App.Container.GetContainer().Resolve<MentalCareDetailsPageViewModel>().ViewOnMapCommand.Execute(null);
        }

        [Then(@"I should see that the google map app has not launched yet")]
        public void ThenIShouldSeeThatTheGoogleMapAppHasNotLaunchedYet()
        {
            Main.App.Container.GetContainer().Resolve<MentalCareDetailsPageViewModel>().HasLauncedExternalAppMap.ShouldBeFalse();
        }

        [Then(@"I am redirected to the google map with pin on the facility's location")]
        public void ThenIAmRedirectedToTheGoogleMapWithPinOnTheFacilitySLocation()
        {
            Main.App.Container.GetContainer().Resolve<MentalCareDetailsPageViewModel>().HasLauncedExternalAppMap.ShouldBeTrue();
        }

        [When(@"I tap the Write Down icon from the menu detail with authenticated ""(.*)"" and signed up ""(.*)""")]
        public void WhenITapTheWriteDownIconFromTheMenuDetailWithAuthenticatedAndSignedUp(bool authenticated, bool signedUp)
        {
            ((KeyValueCacheMock)AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>()).SetSignUpCompletionManually(signedUp);
            ((KeyValueCacheMock)AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>()).SetAuthenticationManually(authenticated);

            Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().WriteDownCommand.Execute(null);
        }

        [Then(@"I should see a message saying the user to sign up ""(.*)""")]
        public void ThenIShouldSeeAMessageSayingTheUserToSignUp(string signedUp)
        {
            Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().GenericMessage.ShouldEqual(signedUp);
        }

        [When(@"I pull to refresh the mental care list")]
        public void WhenIPullToRefreshTheMentalCareList()
        {
            Main.App.Container.GetContainer().Resolve<HomePageViewModel>().RefreshMentalCareCommand.Execute(null);
        }

        [Then(@"I should see the list of mental health care coming from gateway")]
        public void ThenIShouldSeeTheListOfMentalHealthCareComingFromGateway()
        {
            Main.App.Container.GetContainer().Resolve<HomePageViewModel>().MentalFacilities.Count().ShouldBeGreaterThan(0);
            ((MentalHealthManager)Main.App.Container.GetContainer().Resolve<IMentalHealthManager>()).WasForcedGetToTheRest.ShouldBeTrue();
        }

        [Then(@"I should see that there are ""(.*)"" mental care is displayed within the proximity")]
        public void ThenIShouldSeeThatThereAreMentalCareIsDisplayedWithinTheProximity(int resultsCount)
        {
            Main.App.Container.GetContainer().Resolve<HomePageViewModel>().MentalFacilities.Count.ShouldEqual(resultsCount);
        }

        [When(@"I choose the radius km ""(.*)"" with the current position latitude ""(.*)"", longitude ""(.*)"" and tap the filter button")]
        public void WhenIChooseTheRadiusKmWithTheCurrentPositionLatitudeLongitudeAndTapTheFilterButton(int radiusKm, double latitude, double longitude)
        {
            Main.App.Container.GetContainer().Resolve<HomePageViewModel>().Radius = radiusKm;
            ((KeyValueCacheMock)AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>()).SetPositionManually(latitude, longitude);
            Main.App.Container.GetContainer().Resolve<HomePageViewModel>().FilterCommand.Execute(null);
        }

        [Then(@"I should see the first item mental care name ""(.*)""")]
        public void ThenIShouldSeeTheFirstItemMentalCareName(string facilityName) => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().MentalFacilities.First().GroupName.ShouldEqual(facilityName);

        [When(@"I select the sort by location and tap the sort button")]
        public void WhenISelectTheSortByLocationAndTapTheSortButton()
        {
            Main.App.Container.GetContainer().Resolve<HomePageViewModel>().IsSortByLocation = true;
            Main.App.Container.GetContainer().Resolve<HomePageViewModel>().SortCommand.Execute(null);
        }

        [When(@"I tap the sort button in mental facility page")]
        public void WhenITapTheSortButtonInMentalFacilityPage() => Main.App.Container.GetContainer().Resolve<HomePageViewModel>().SortCommand.Execute(null);

        [Then(@"I should see the mental care error message ""(.*)""")]
        public void ThenIShouldSeeTheMentalCareErrorMessage(string errorMessage) => ((AlertResult)Main.App.Container.GetContainer().Resolve<HomePageViewModel>().AlertResult).Message.ShouldEqual(errorMessage);
    }
}

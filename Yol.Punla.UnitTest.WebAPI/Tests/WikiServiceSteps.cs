using Newtonsoft.Json;
using Should;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TechTalk.SpecFlow;
using Yol.Punla.UnitTest.WebAPI.Barrack;

namespace Yol.Punla.UnitTest.WebAPI
{
    [Binding]
    public class WikiServiceSteps : StepBase
    {
        private IEnumerable<Contract.WikiK> _wikiList = null;

        private readonly string _getWikiList = null;
        private const string GETWIKILIST = "Wiki/GetWikis?companyName={0}";

        public WikiServiceSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            _getWikiList = _baseAPI + GETWIKILIST;
        }

        [Given(@"the wiki list is null")]
        public void GivenTheWikiListIsNull()
        {
            _wikiList.ShouldBeNull();
        }

        [When(@"the client calls the GetWikis endpoint of company ""(.*)""")]
        public void WhenTheClientCallsTheGetWikisEndpointOfCompany(string companyName)
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.GetAsync(string.Format(_getWikiList, companyName)).Result;
            _wikiList = JsonConvert.DeserializeObject<IEnumerable<Contract.WikiK>>(_httpResponse.Content.ReadAsStringAsync().Result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should see the list of wiki")]
        public void ThenIShouldSeeTheListOfWiki()
        {
            _wikiList.Count().ShouldBeGreaterThan(1);
        }

        [Then(@"I shouid see the ForceToVersionNo in one of the items in wiki")]
        public void ThenIShouidSeeTheForceToVersionNoInOneOfTheItemsInWiki()
        {
            var firstWiki = _wikiList.Where(x => !string.IsNullOrEmpty(x.ForceToVersionNo));
            firstWiki.ShouldNotBeNull();
        }
    }
}

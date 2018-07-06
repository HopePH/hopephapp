using System.Configuration;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace Yol.Punla.UnitTest.WebAPI
{
    public class StepBase
    {
        protected string _baseAPI = ConfigurationManager.AppSettings["WepAPIURL"];
        protected readonly ScenarioContext _scenarioContext;
        protected HttpResponseMessage _httpResponse;

        public StepBase(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
    }
}

using Should;
using TechTalk.SpecFlow;
using Yol.Punla.UnitTest.WebAPI.Barrack;

namespace Yol.Punla.UnitTest.WebAPI.Tests.Common
{
    [Binding]
    public class APIResponseStep : StepBase
    {
        public APIResponseStep(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [Then(@"I should see the response code is OK")]
        [Then(@"I should see response code OK")]
        public void ThenIShouldSeeResponseCodeOK()
        {
            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode.ShouldEqual(System.Net.HttpStatusCode.OK);
        }

        [Then(@"I should see a reponse message ""(.*)"" from endpoint")]
        public void ThenIShouldSeeAReponseMessageFromEndpoint(string responseMessage)
        {
            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            var content = apiResponse.HttpResponseMessage.Content.ReadAsStringAsync().Result;
            content.ShouldContain(responseMessage);
        }
    }
}

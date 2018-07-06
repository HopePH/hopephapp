using Newtonsoft.Json;
using Should;
using System;
using System.Net.Http;
using System.Text;
using TechTalk.SpecFlow;
using Yol.Punla.Barrack;
using Yol.Punla.UnitTest.WebAPI.Barrack;

namespace Yol.Punla.UnitTest.WebAPI.Tests
{
    [Binding]
    public class ReceiverServiceSteps : StepBase
    {
        private Contract.ContactK _contact = null;
        private int _newContactId = 0;
        private string _verificationCode = null;

        private readonly string _postDeleteReceiver = null;
        private const string POSTDELETERECEIVER = "Receiver/DeletingReceiver?contactId=";

        private readonly string _postSaveReceiver = null;
        private const string POSTSAVERECEIVER = "Receiver/SavingReceiverDetails";

        private readonly string _getContactByFB = null;
        private const string GETCONTACTBYFB = "Receiver/GetReceiverContactByUserName?userName={0}&facebookId={1}&companyName={2}";

        private readonly string _postVerifyEmail = null;
        private const string POSTVERIFYEMAIL = "Receiver/VerifyEmail?toEmailAddress={0}&aliasName={1}";

        private readonly string _getUserViaEmail = null;
        private const string GETUSERVIAEMAIL = "Receiver/GetUserViaEmail?emailAddress={0}&companyName={1}";

        public ReceiverServiceSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            _postSaveReceiver = _baseAPI + POSTSAVERECEIVER;
            _postDeleteReceiver = _baseAPI + POSTDELETERECEIVER;
            _getContactByFB = _baseAPI + GETCONTACTBYFB;
            _postVerifyEmail = _baseAPI + POSTVERIFYEMAIL;
            _getUserViaEmail = _baseAPI + GETUSERVIAEMAIL;
        }

        [Given(@"the contact is null")]
        public void GivenTheContactIsNull()
        {
            _contact.ShouldBeNull();
        }

        [When(@"the client calls to the SavingReceiverDetails endpoint for saving a new contact id ""(.*)"", first name ""(.*)"", last name ""(.*)"", mobile phone ""(.*)"" is active ""(.*)"", user name ""(.*)"", password ""(.*)"", type code ""(.*)"", gender code ""(.*)"", alias name ""(.*)"", company name ""(.*)"", fbId ""(.*)""")]
        public void WhenTheClientCallsToTheSavingReceiverDetailsEndpointForSavingANewContactIdFirstNameLastNameMobilePhoneIsActiveUserNamePasswordTypeCodeGenderCodeAliasNameCompanyNameFbId(
            int contactId, string firstName, string lastName, string mobilePhone, string isActive, string userName, string password, 
            string typeCode, string genderCode, string aliasName, string companyName, string fbId)
        {
            var contractItem = new Contract.ContactK { Id = contactId, FirstName = firstName, LastName = lastName, MobilePhone = mobilePhone, IsActive = bool.Parse(isActive),
                UserName = userName, Password = password, TypeCode = typeCode, GenderCode = genderCode, AliasName =aliasName, CompanyName = companyName,
                 FBId = fbId, EmailAddress = userName };
            contractItem.DateModified = DateTime.Now.ToString(Constants.DateTimeFormat);
            var jsonString = JsonConvert.SerializeObject(contractItem);
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            _httpResponse = httpClient.PostAsync(_postSaveReceiver, jsonContent).Result; 
            var result = _httpResponse.Content.ReadAsStringAsync().Result;

            if (!string.IsNullOrWhiteSpace(result))
                _newContactId = int.Parse(result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [When(@"the client calls the GetReceiverContactByUserName using the facebook id ""(.*)"", user name ""(.*)"" and company name ""(.*)""")]
        public void WhenTheClientCallsTheGetReceiverContactByUserNameUsingTheFacebookIdUserNameAndCompanyName(string facebookId, string userName, string companyName)
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.GetAsync(string.Format(_getContactByFB, userName, facebookId, companyName)).Result;
            _contact = JsonConvert.DeserializeObject<Contract.ContactK>(_httpResponse.Content.ReadAsStringAsync().Result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should see the receiver or contact is present in the records")]
        public void ThenIShouldSeeTheReceiverOrContactIsPresentInTheRecords()
        {
            _contact.ShouldNotBeNull();
            _contact.AliasName.ShouldNotBeEmpty();
        }

        [When(@"the client calls the DeletingReceiver of the newly added contact")]
        public void WhenTheClientCallsTheDeletingReceiverOfTheNewlyAddedContact()
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.PostAsync($"{_postDeleteReceiver}{_contact.Id}", new StringContent("")).Result;

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should see the receiver or contact is not present in the records already")]
        public void ThenIShouldSeeTheReceiverOrContactIsNotPresentInTheRecordsAlready()
        {
            _contact.ShouldBeNull();
        }

        [Given(@"the verfification code is null")]
        public void GivenTheVerfificationCodeIsNull()
        {
            _verificationCode.ShouldBeNull();
        }

        [When(@"the client calls to the VerifyEmail endpoint for email ""(.*)"" and alias ""(.*)""")]
        public void WhenTheClientCallsToTheVerifyEmailEndpointForEmailAndAlias(string emailAddress, string aliasName)
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.PostAsync(string.Format(_postVerifyEmail, emailAddress, aliasName), new StringContent("")).Result;
            var result = _httpResponse.Content.ReadAsStringAsync().Result;

            if (!string.IsNullOrWhiteSpace(result))
                _verificationCode = result.Replace("\"","");  

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I could see that the verification could is not null")]
        public void ThenICouldSeeThatTheVerificationCouldIsNotNull()
        {
            _verificationCode.ShouldNotBeNull();
            _verificationCode.Length.ShouldEqual(4);
        }

        [Given(@"the user with email is null")]
        public void GivenTheUserWithEmailIsNull()
        {
            _contact.ShouldBeNull();
        }

        [When(@"the client calls to the Http get method for getting the client contact via email address ""(.*)""")]
        public void WhenTheClientCallsToTheHttpGetMethodForGettingTheClientContactViaEmailAddress(string emailAddress)
        {
            var httpClient = new HttpClient();
            string endPoint = string.Format(_getUserViaEmail, emailAddress, "HopePH");
            _httpResponse = httpClient.GetAsync(endPoint).Result;
            var result = _httpResponse.Content.ReadAsStringAsync().Result;
            _contact = JsonConvert.DeserializeObject<Contract.ContactK>(result);
            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should see that the user with email is not null")]
        public void ThenIShouldSeeThatTheUserWithEmailIsNotNull()
        {
            _contact.ShouldNotBeNull();
        }

        [Then(@"I should see that the user with email is null")]
        public void ThenIShouldSeeThatTheUserWithEmailIsNull()
        {
            _contact.ShouldBeNull();
        }
    }
}

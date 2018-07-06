using Newtonsoft.Json;
using Should;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TechTalk.SpecFlow;
using Yol.Punla.UnitTest.WebAPI.Barrack;

namespace Yol.Punla.UnitTest.WebAPI.Tests
{
    [Binding]
    public class PostFeedServiceSteps : StepBase
    {
        private IEnumerable<Contract.PostFeedLikeK> _postFeedLikes;
        private IEnumerable<Contract.PostFeedK> _postFeeds;
        private IEnumerable<Contract.PostFeedK> _comments;
        private Contract.PostFeedK _postFeed;
        private Contract.PostFeedK _postFeedBase;

        private readonly string _postLikeAPost = null;
        private const string POSTLIKEAPOST = "PostFeed/LikeAPost?postFeedId={0}&contactId={1}";

        private readonly string _postUnlikeAPost = null;
        private const string POSTUNLIKEAPOST = "PostFeed/UnLikeAPost?postFeedId={0}&contactId={1}";

        private readonly string _getTopPosts = null;
        private const string GETTOPPOSTS = "PostFeed/GetTopPosts?companyName={0}&isTest={1}";

        private readonly string _getComments = null;
        private const string GETCOMMENTS = "PostFeed/GetTopComments?parentPostFeedId={0}&companyName={1}";

        private readonly string _postNewPostFeed = null;
        private const string POSTNEWPOSTFEED = "PostFeed/AddNewPostFeed";

        private readonly string _getPostFeed = null;
        private const string GETPOSTFEED = "PostFeed/GetPostFeed?companyName={0}&postFeedId={1}";

        private readonly string _getPostFeedLike = null;
        private const string GETPOSTFEEDLIKE = "PostFeed/GetPostFeedLikes?postFeedId=";

        private readonly string _getOwnPosts = null;
        private const string GETOWNPOSTS = "PostFeed/GetOwnPosts?companyName={0}&contactId={1}";

        private readonly string _getPostsWithSpeed = null;
        private const string GETPOSTSWITHSPEED = "PostFeed/GetTopPostsWithSpeed?contactId={0}&isFirstLoad={1}&topPostId={2}&companyName={3}";

        private readonly string _getPostsNotifications = null;
        private const string GETPOSTSNOTIFICATIONS = "PostFeed/GetPostNotifications?companyName={0}&contactId={1}";

        public PostFeedServiceSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            _getTopPosts = _baseAPI + GETTOPPOSTS;
            _getComments = _baseAPI + GETCOMMENTS;
            _postNewPostFeed = _baseAPI + POSTNEWPOSTFEED;
            _getPostFeed = _baseAPI + GETPOSTFEED;
            _getPostFeedLike = _baseAPI + GETPOSTFEEDLIKE;
            _postLikeAPost = _baseAPI + POSTLIKEAPOST;
            _postUnlikeAPost = _baseAPI + POSTUNLIKEAPOST;
            _getOwnPosts = _baseAPI + GETOWNPOSTS;
            _getPostsWithSpeed = _baseAPI + GETPOSTSWITHSPEED;
            _getPostsNotifications = _baseAPI + GETPOSTSNOTIFICATIONS;
        }

        [Given(@"the post feed list is null")]
        public void GivenThePostFeedListIsNull()
        {
            _postFeeds.ShouldBeNull();
        }

        [When(@"the client calls to the GetTopPosts endpoint for company ""(.*)""")]
        public void WhenTheClientPostToTheGetTopPostsEndpointForCompany(string companyName)
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.GetAsync(string.Format(_getTopPosts,companyName,true)).Result;
            _postFeeds = JsonConvert.DeserializeObject<IEnumerable<Contract.PostFeedK>>(_httpResponse.Content.ReadAsStringAsync().Result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should that there are more or less (.*) post the server")]
        public void ThenIShouldThatThereAreMoreOrLessPostTheServer(int qty)
        {
            _postFeeds.ShouldNotBeNull();
            _postFeeds.Count().ShouldBeGreaterThanOrEqualTo(qty);
        }

        [Then(@"I should see that the post has a displayed a contact gender")]
        public void ThenIShouldSeeThatThePostHasADisplayedAContactGender()
        {
            var results = _postFeeds.Where(x => x.ContactGender == "male");
            results.Count().ShouldBeGreaterThan(0);
        }

        [When(@"the client calls to the GetComments endpoint for post id ""(.*)"" company ""(.*)""")]
        public void WhenTheClientCallsToTheGetCommentsEndpointForPostIdCompany(int postId, string companyName)
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.GetAsync(string.Format(_getComments, postId, companyName)).Result;
            _comments = JsonConvert.DeserializeObject<IEnumerable<Contract.PostFeedK>>(_httpResponse.Content.ReadAsStringAsync().Result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [When(@"the client calls to the GetTopPosts endpoint and get the first item for company ""(.*)"" and get the post feed ""(.*)""")]
        public void WhenTheClientCallsToTheGetTopPostsEndpointAndGetTheFirstItemForCompanyAndGetThePostFeed(string companyName, string contentText)
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.GetAsync(string.Format(_getTopPosts, companyName, true)).Result;
            _postFeeds = JsonConvert.DeserializeObject<IEnumerable<Contract.PostFeedK>>(_httpResponse.Content.ReadAsStringAsync().Result);
            _postFeed = _postFeeds.Where(x => x.ContentText == contentText).FirstOrDefault();

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should see one of the item content is ""(.*)""")]
        public void ThenIShouldSeeOneOfTheItemContentIs(string title)
        {
            _postFeed.ContentText.ShouldEqual(title);
        }

        [When(@"the client calls to the AddNewPostFeed endpoint for post id ""(.*)"", company ""(.*)"", title ""(.*)"", content url ""(.*)"", content text ""(.*)"", contact id ""(.*)""")]
        public void WhenTheClientCallsToTheAddNewPostFeedEndpointForPostIdCompanyTitleContentUrlContentTextContactId(int postId, string company, 
            string title, string contentURL, string contentText, int contactId)
        {
            var contractItem = new Contract.PostFeedK { PostFeedID = postId, Title = title, ContentURL = contentURL, ContentText = contentText, ContactId = contactId };
            var jsonString = JsonConvert.SerializeObject(contractItem);
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            _httpResponse = httpClient.PostAsync(_postNewPostFeed, jsonContent).Result;
            var result = _httpResponse.Content.ReadAsStringAsync().Result;

            if (!string.IsNullOrWhiteSpace(result))
                _postFeed = JsonConvert.DeserializeObject<Contract.PostFeedK>(result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should see that the new post feed content ""(.*)""")]
        public void ThenIShouldSeeThatTheNewPostFeedTitle(string contentText)
        {
            _postFeed.ContentText.ShouldEqual(contentText);
        }

        [When(@"the client calls to the GetPostFeedLike endpoint for post id ""(.*)"" company ""(.*)""")]
        public void WhenTheClientCallsToTheGetPostFeedLikeEndpointForPostIdCompany(int postFeedId, string company)
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.GetAsync($"{_getPostFeedLike}{postFeedId}").Result;
            _postFeedLikes = JsonConvert.DeserializeObject<IEnumerable<Contract.PostFeedLikeK>>(_httpResponse.Content.ReadAsStringAsync().Result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should see that there are ""(.*)"" likes")]
        public void ThenIShouldSeeThatThereAreLikes(int likesCount)
        {
            _postFeedLikes.Count().ShouldEqual(likesCount);
        }

        [When(@"I hit one like to the post feed id ""(.*)"" of poster id ""(.*)""")]
        public void WhenIHitOneLikeToThePostFeedIdOfPosterId(int postFeedId, int posterId)
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.PostAsync(string.Format(_postLikeAPost,postFeedId,posterId), new StringContent("")).Result;

            var postResponse = httpClient.GetAsync($"{_getPostFeedLike}{postFeedId}").Result;
            _postFeedLikes = JsonConvert.DeserializeObject<IEnumerable<Contract.PostFeedLikeK>>(postResponse.Content.ReadAsStringAsync().Result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [When(@"I remove one like to the post feed id ""(.*)"" of poster id ""(.*)""")]
        public void WhenIRemoveOneLikeToThePostFeedIdOfPosterId(int postFeedId, int posterId)
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.PostAsync(string.Format(_postUnlikeAPost, postFeedId, posterId), new StringContent("")).Result;

            var postResponse = httpClient.GetAsync($"{_getPostFeedLike}{postFeedId}").Result;
            _postFeedLikes = JsonConvert.DeserializeObject<IEnumerable<Contract.PostFeedLikeK>>(postResponse.Content.ReadAsStringAsync().Result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [When(@"the client calls to the GetOwnPosts endpoint for company ""(.*)"" and poster id ""(.*)""")]
        public void WhenTheClientCallsToTheGetOwnPostsEndpointForCompanyAndPosterId(string company, int posterId)
        {
            var httpClient = new HttpClient();
            _httpResponse = httpClient.GetAsync(string.Format(_getOwnPosts, company, posterId)).Result;
            _postFeeds = JsonConvert.DeserializeObject<IEnumerable<Contract.PostFeedK>>(_httpResponse.Content.ReadAsStringAsync().Result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should see that all posts are from poster id ""(.*)""")]
        public void ThenIShouldSeeThatAllPostsAreFromPosterId(int posterId)
        {
            foreach (var p in _postFeeds)
                p.ContactId.ShouldEqual(posterId);
        }

        [When(@"the client calls to the GetTopPostsWithSpeed endpoint for company ""(.*)"", contact ""(.*)"" and starting post ""(.*)"" for the first time")]
        public void WhenTheClientCallsToTheGetTopPostsWithSpeedEndpointForCompanyContactAndStartingPostForTheFirstTime(string company, int contactId, int basePostId)
        {
            var httpClient = new HttpClient();
            var endPoint = string.Format(_getPostsWithSpeed, contactId, true, basePostId, company);
            _httpResponse = httpClient.GetAsync(endPoint).Result;
            _postFeeds = JsonConvert.DeserializeObject<IEnumerable<Contract.PostFeedK>>(_httpResponse.Content.ReadAsStringAsync().Result);
            _postFeedBase = _postFeeds.Last();

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should that there are ""(.*)"" post fetch from the server")]
        public void ThenIShouldThatThereArePostFetchFromTheServer(int noOfPosts)
        {
            _postFeeds.Count().ShouldEqual(noOfPosts);
        }

        [When(@"the client calls again to fetch the next batch to the GetTopPostsWithSpeed endpoint for company ""(.*)"", contact ""(.*)"" and starting post ""(.*)""")]
        public void WhenTheClientCallsAgainToFetchTheNextBatchToTheGetTopPostsWithSpeedEndpointForCompanyContactAndStartingPost(string company, int contactId, int basePostId)
        {
            var httpClient = new HttpClient();
            var endPoint = string.Format(_getPostsWithSpeed, contactId, false, _postFeedBase.PostFeedID, company);
            _httpResponse = httpClient.GetAsync(endPoint).Result;
            _postFeeds = JsonConvert.DeserializeObject<IEnumerable<Contract.PostFeedK>>(_httpResponse.Content.ReadAsStringAsync().Result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should that there are ""(.*)"" post fetch from the server of the next batch")]
        public void ThenIShouldThatThereArePostFetchFromTheServerOfTheNextBatch(int noOfPosts)
        {
            _postFeeds.Count().ShouldEqual(noOfPosts);
            var firstPostFeed = _postFeeds.FirstOrDefault();
            firstPostFeed.PostFeedID.ShouldEqual(_postFeedBase.PostFeedID);
        }

        [When(@"the client calls to the GetPostNotifications endpoint for company ""(.*)"" and contact ""(.*)""")]
        public void WhenTheClientCallsToTheGetPostNotificationsEndpointForCompanyAndContact(string company, int contactId)
        {
            var httpClient = new HttpClient();
            var endPoint = string.Format(_getPostsNotifications, company, contactId);
            _httpResponse = httpClient.GetAsync(endPoint).Result;
            _postFeeds = JsonConvert.DeserializeObject<IEnumerable<Contract.PostFeedK>>(_httpResponse.Content.ReadAsStringAsync().Result);

            var apiResponse = (APIHttpResponse)AppStart.Kernel.GetService(typeof(APIHttpResponse));
            apiResponse.HttpStatusCode = _httpResponse.StatusCode;
            httpClient.Dispose();
        }

        [Then(@"I should that there post notifications fetch from the server")]
        public void ThenIShouldThatTherePostNotificationsFetchFromTheServer()
        {
            _postFeeds.ShouldNotBeNull();
            _postFeeds.Count().ShouldBeGreaterThan(0);
        }
    }
}

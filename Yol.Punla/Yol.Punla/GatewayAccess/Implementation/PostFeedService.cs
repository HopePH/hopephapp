using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Mapper;

namespace Yol.Punla.GatewayAccess
{
    [DefaultModuleInterfaced(ParentInterface = typeof(IPostFeedService))]
    public class PostFeedService : GatewayServiceBase, IPostFeedService
    {
        private readonly string _getTopPostFeeds = null;
        private const string GETTOPPOSTFEEDS = "PostFeed/GetTopPosts?companyName={0}&contactId={1}";

        private readonly string _getPostFeedComments = null;
        private const string GETPOSTFEEDCOMMENTS = "PostFeed/GetTopComments?parentPostFeedId={0}&companyName={1}&contactId={2}";

        private readonly string _postNewPostFeed = null;
        private const string NEWPOSTFEED = "PostFeed/AddNewPostFeed";

        private readonly string _postUpdatePostFeed = null;
        private const string UPDATEPOSTFEED = "PostFeed/UpdatePostFeed";

        private readonly string _deleteSelfPost = null;
        private const string DELETESELFPOST = "PostFeed/DeletePostFeed?postFeedId={0}&companyName=Hopeph";

        private readonly string _supportPost = null;
        private const string SUPPORTPOST = "PostFeed/LikeAPost?postFeedId={0}&contactId={1}";

        private readonly string _unsupportPost = null;
        private const string UNSUPPORTPOST = "PostFeed/UnLikeAPost?postFeedId={0}&contactId={1}";

        private readonly string _getOwnPosts = null;
        private const string GETOWNPOSTS = "PostFeed/GetOwnPosts?companyName={0}&contactId={1}";

        private readonly string _getPostsNotifications = null;
        private const string GETPOSTSNOTIFICATIONS = "PostFeed/GetPostNotifications?companyName={0}&contactId={1}";

        private readonly string _getPostsWithSpeed = null;
        private const string GETPOSTSWITHSPEED = "PostFeed/GetTopPostsWithSpeed?contactId={0}&isFirstLoad={1}&topPostId={2}&companyName={3}";

        public PostFeedService(IServiceMapper serviceMapper) : base(serviceMapper)
        {
            _getTopPostFeeds = BaseAPI + GETTOPPOSTFEEDS;
            _getPostFeedComments = BaseAPI + GETPOSTFEEDCOMMENTS;
            _postNewPostFeed = BaseAPI + NEWPOSTFEED;
            _postUpdatePostFeed = BaseAPI + UPDATEPOSTFEED;
            _deleteSelfPost = BaseAPI + DELETESELFPOST;
            _supportPost = BaseAPI + SUPPORTPOST;
            _unsupportPost = BaseAPI + UNSUPPORTPOST;
            _getOwnPosts = BaseAPI + GETOWNPOSTS;
            _getPostsNotifications = BaseAPI + GETPOSTSNOTIFICATIONS;
            _getPostsWithSpeed = BaseAPI + GETPOSTSWITHSPEED;
        }
        
        public async Task<IEnumerable<Entity.PostFeed>> GetTopPostFeeds(int posterId)
        {
            var remoteItems = await GetRemoteAsync<IEnumerable<Contract.PostFeedK>>(string.Format(_getTopPostFeeds, CompanyName, posterId));
            var mappedItems = ServiceMapper.Instance.Map<IEnumerable<Entity.PostFeed>>(remoteItems);
            return mappedItems;
        }

        public async Task<IEnumerable<Entity.PostFeed>> GetPostFeedsWithSpeed(int posterId, int postFeedBaseId, bool isFirstLoad)
        {
            var endPoint = string.Format(_getPostsWithSpeed, posterId, isFirstLoad, postFeedBaseId, CompanyName);
            var remoteItems = await GetRemoteAsync<IEnumerable<Contract.PostFeedK>>(endPoint);
            var mappedItems = ServiceMapper.Instance.Map<IEnumerable<Entity.PostFeed>>(remoteItems);
            return mappedItems;
        }

        public async Task<IEnumerable<Entity.PostFeed>> GetPostFeedComments(int postFeedId, int posterId)
        {
            var remoteItems = await GetRemoteAsync<IEnumerable<Contract.PostFeedK>>(string.Format(_getPostFeedComments, postFeedId, CompanyName, posterId));
            var mappedItems = ServiceMapper.Instance.Map<IEnumerable<Entity.PostFeed>>(remoteItems);
            return mappedItems;
        }

        public async Task<HttpStatusCode> DeleteSelfPostFromRemote(int postId)
        {
            Debug.WriteLine("HOPEPH Posting new post feed.");
            var jsonContent = new StringContent("", Encoding.UTF8, "application/json");
            var result = await HttpClient.PostAsync(string.Format(_deleteSelfPost, postId), jsonContent);
            return result.StatusCode;
        }

        public async Task<HttpStatusCode> SupportPost(int postFeedId, int posterId)
        {
            Debug.WriteLine("HOPEPH Supporting a post feed.");
            var jsonContent = new StringContent("", Encoding.UTF8, "application/json");
            var result = await HttpClient.PostAsync(string.Format(_supportPost, postFeedId, posterId), jsonContent);
            return result.StatusCode;
        }

        public async Task<HttpStatusCode> UnSupportPost(int postFeedId, int posterId)
        {
            Debug.WriteLine("HOPEPH Supporting a post feed.");
            var jsonContent = new StringContent("", Encoding.UTF8, "application/json");
            var result = await HttpClient.PostAsync(string.Format(_unsupportPost, postFeedId, posterId), jsonContent);
            return result.StatusCode;
        }

        public async Task<IEnumerable<Entity.PostFeed>> GetOwnPostsFeeds(int posterId)
        {
            var remoteItems = await GetRemoteAsync<IEnumerable<Contract.PostFeedK>>(string.Format(_getOwnPosts, CompanyName, posterId));
            var mappedItems = ServiceMapper.Instance.Map<IEnumerable<Entity.PostFeed>>(remoteItems);
            return mappedItems;
        }

        public async Task<IEnumerable<Entity.PostFeed>> GetPostFeedNotifications(int posterId)
        {
            var remoteItems = await GetRemoteAsync<IEnumerable<Contract.PostFeedK>>(string.Format(_getPostsNotifications, CompanyName, posterId));
            var mappedItems = ServiceMapper.Instance.Map<IEnumerable<Entity.PostFeed>>(remoteItems);
            return mappedItems;
        }

        public async Task<int> SavePostToServer(Entity.PostFeed newPost)
        {
            int newPostId = 0;

            Debug.WriteLine("HOPEPH Posting new post feed.");
            var contractItem = ServiceMapper.Instance.Map<Contract.PostFeedK>(newPost);

            var jsonString = JsonConvert.SerializeObject(contractItem);
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await HttpClient.PostAsync(_postNewPostFeed, jsonContent);

            if (result.IsSuccessStatusCode)
            {
                var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(jsonResult))
                {
                    newPostId = JsonConvert.DeserializeObject<int>(jsonResult);
                }

            }

            return newPostId;
        }
    }
}

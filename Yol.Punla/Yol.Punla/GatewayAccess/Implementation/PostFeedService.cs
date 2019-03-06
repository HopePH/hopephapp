using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using System.Collections.ObjectModel;

namespace Yol.Punla.GatewayAccess
{
    [DefaultModuleInterfaced(ParentInterface = typeof(IPostFeedService))]
    public class PostFeedService : GatewayServiceBase, IPostFeedService
    {
        public PostFeedService(IServiceMapper serviceMapper) : base(serviceMapper)
        {
          
        }
        
        public async Task<IEnumerable<Entity.PostFeed>> GetTopPostFeeds(int posterId)
        {
            await Task.Delay(1);
            var topPostFeeds = FakeData.FakePostFeeds.Posts.Where(p => p.PosterId == posterId);
            return topPostFeeds;
        }

        public async Task<IEnumerable<Entity.PostFeed>> GetPostFeedsWithSpeed(int posterId, int postFeedBaseId, bool isFirstLoad)
        {
            await Task.Delay(1);
            var postFeeds = FakeData.FakePostFeeds.Posts;
            return postFeeds;
        }

        public async Task<IEnumerable<Entity.PostFeed>> GetPostFeedComments(int postFeedId, int posterId)
        {
            await Task.Delay(1);
            //var postFeedComments = FakeData.FakePostFeeds.Posts.Where(p => p.PostFeedID == postFeedId && p.PosterId == posterId).FirstOrDefault();
            //return postFeedComments?.Comments ?? new ObservableCollection<Entity.PostFeed>();
            var postFeedComments = FakeData.FakePostFeeds.Posts.Where(p => p.PostFeedParentId == postFeedId);
            return postFeedComments ?? new ObservableCollection<Entity.PostFeed>();
        }

        public async Task<HttpStatusCode> DeleteSelfPostFromRemote(int postId)
        {
            await Task.Delay(1);
            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> SupportPost(int postFeedId, int posterId)
        {
            await Task.Delay(1);
            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> UnSupportPost(int postFeedId, int posterId)
        {
            await Task.Delay(1);
            return HttpStatusCode.OK;
        }

        public async Task<IEnumerable<Entity.PostFeed>> GetOwnPostsFeeds(int posterId)
        {
            await Task.Delay(1);
            var postFeeds = FakeData.FakePostFeeds.Posts.Where(p => p.PosterId == posterId);
            return postFeeds;
        }

        public async Task<IEnumerable<Entity.PostFeed>> GetPostFeedNotifications(int posterId)
        {
            await Task.Delay(1);
            var postFeeds = FakeData.FakePostFeeds.Posts.Where(p => p.PosterId == posterId);
            return postFeeds;
        }

        public async Task<int> SavePostToServer(Entity.PostFeed newPost)
        {
            await Task.Delay(1);
            return 0;
        }

        public async Task<IEnumerable<string>> GetSupportersAvatars(int postFeedId)
        {
            List<string> photoUrls = new List<string>();
            await Task.Delay(1);
            var ids = FakeData.FakePostFeeds.Posts.FirstOrDefault(p => p.PostFeedID == postFeedId)?.SupportersIdsList;
            foreach (var item in ids)
            {
                var posterPhotoUrl = FakeData.FakeUsers.Contacts.FirstOrDefault(c => c.RemoteId == item).PhotoURL;
                photoUrls.Add(posterPhotoUrl);
            }
            return photoUrls;
        }
    }
}

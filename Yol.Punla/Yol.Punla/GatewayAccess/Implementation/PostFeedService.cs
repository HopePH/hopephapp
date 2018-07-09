using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Mapper;

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
            var postFeedComments = FakeData.FakePostFeeds.Posts.Where(p => p.PostFeedID == postFeedId && p.PosterId == posterId);
            return postFeedComments;
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
    }
}

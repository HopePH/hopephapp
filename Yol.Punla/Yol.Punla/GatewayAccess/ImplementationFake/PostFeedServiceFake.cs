using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Entity;

namespace Yol.Punla.GatewayAccess
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(IPostFeedService))]
    public class PostFeedServiceFake : IPostFeedService
    {
        public Task<HttpStatusCode> DeleteSelfPostFromRemote(int postId) => Task.FromResult(HttpStatusCode.OK);

        public Task<IEnumerable<PostFeed>> GetOwnPostsFeeds(int posterId) => Task.FromResult(FakeData.FakePostFeeds.Posts.Where(x => x.PosterId == posterId));

        public Task<IEnumerable<PostFeed>> GetPostFeedComments(int postFeedId, int posterId)
            => Task.FromResult((IEnumerable<PostFeed>)FakeData.FakePostFeeds.Posts.Where(p => p.PostFeedParentId == postFeedId));

        public Task<IEnumerable<PostFeed>> GetPostFeedNotifications(int posterId) => Task.FromResult(FakeData.FakePostFeeds.Posts);

        public Task<IEnumerable<PostFeed>> GetPostFeedsWithSpeed(int posterId, int postFeedBaseId, bool isFirstLoad)
            => Task.FromResult(FakeData.FakePostFeeds.Posts);

        public Task<IEnumerable<string>> GetSupportersAvatars(int postFeedId)
        {
            List<string> photoUrls = new List<string>();
            var ids = FakeData.FakePostFeeds.Posts.FirstOrDefault(p => p.PostFeedID == postFeedId)?.SupportersIdsList;
            foreach (var item in ids)
            {
                var posterPhotoUrl = FakeData.FakeUsers.Contacts.FirstOrDefault(c => c.RemoteId == item).PhotoURL;
                photoUrls.Add(posterPhotoUrl);
            }
            return Task.FromResult<IEnumerable<string>>(photoUrls);
        }

        public Task<IEnumerable<PostFeed>> GetTopPostFeeds(int posterId) => Task.FromResult(FakeData.FakePostFeeds.Posts);

        public Task<int> SavePostToServer(PostFeed newPost) => Task.FromResult(0);

        public Task<HttpStatusCode> SupportPost(int postFeedId, int posterId) => Task.FromResult(HttpStatusCode.OK);

        public Task<HttpStatusCode> UnSupportPost(int postFeedId, int posterId) => Task.FromResult(HttpStatusCode.OK);
    }
}

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Yol.Punla.GatewayAccess
{
    public interface IPostFeedService
    {
        Task<IEnumerable<Entity.PostFeed>> GetTopPostFeeds(int posterId);
        Task<IEnumerable<Entity.PostFeed>> GetPostFeedsWithSpeed(int posterId, int postFeedBaseId, bool isFirstLoad);
        Task<IEnumerable<Entity.PostFeed>> GetPostFeedComments(int postFeedId, int posterId);
        Task<IEnumerable<Entity.PostFeed>> GetOwnPostsFeeds(int posterId);
        Task<HttpStatusCode> DeleteSelfPostFromRemote(int postId);
        Task<HttpStatusCode> SupportPost(int postFeedId, int posterId);
        Task<HttpStatusCode> UnSupportPost(int postFeedId, int posterId);
        Task<IEnumerable<Entity.PostFeed>> GetPostFeedNotifications(int posterId);
        Task<int> SavePostToServer(Entity.PostFeed newPost);
        Task<IEnumerable<string>> GetSupportersAvatars(int postFeedId);
    }
}

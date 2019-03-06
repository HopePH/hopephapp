using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Yol.Punla.Managers
{
    public interface IPostFeedManager 
    {
        Task<IEnumerable<Entity.PostFeed>> GetAllPosts(bool isForcedGetToTheRest = false, int posterId = 0);
        Task<IEnumerable<Entity.PostFeed>> GetAllPostsWithSpeed(int posterId, int postFeedBaseId, bool isFirstLoad, bool isForcedGetToTheRest = false, bool isForcedGetToLocal = false);
        Task<IEnumerable<Entity.PostFeed>> GetOwnPosts(int posterId = 0);
        Task<IEnumerable<Entity.PostFeed>> GetComments(int postFeedId, bool isForcedGetToTheRest = false, int posterId = 0);
        Task<IEnumerable<string>> GetSupportersAvatars(int postFeedId);
        Task<int> SaveNewPost(Entity.PostFeed newPost);
        Task<HttpStatusCode> DeleteSelfPost(Entity.PostFeed postToDelete);

        IEnumerable<Entity.PostFeed> LikeOrUnlikeSelfPost(int postFeedId, int posterId);
        Entity.PostFeed UpdatePostFeedAndPostFeedLikeToLocal(Entity.PostFeed existingPostFeed, Entity.Contact userWhoLiked);
        Entity.PostFeed GetPostFeed(int postFeedId);

        void SaveNewPostToLocal(Entity.PostFeed newPost);
        void DeletePostInLocal(Entity.PostFeed PostFeed);        
    }
}

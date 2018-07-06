using System.Collections.Generic;

namespace Yol.Punla.Repository
{
    public interface IPostFeedRepository
    {
        IEnumerable<Entity.PostFeed> GetAllPostsFromLocal();
        IEnumerable<Entity.PostFeed> GetCommentsFromLocal(int postFeedId);
        IEnumerable<Entity.PostFeedLike> GetPostFeedLikes();

        Entity.PostFeedLike GetPostFeedLikeByContactId(int contactId, int postFeedId);
        Entity.PostFeed GetPostFeedById(int postFeedId);

        void UpdateItem<T>(T item);
        void DeleteTableByType<T>();      
        void DeleteTable(object objToDelete);
        void DeletePostFeedAndItsComments(Entity.PostFeed postFeed);
    }
}

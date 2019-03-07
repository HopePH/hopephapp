using System.Collections.Generic;
using System.Linq;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Repository
{
    [ModuleIgnore]
    public class PostFeedRepository : Database, IPostFeedRepository
    {
        public PostFeedRepository(string databasePath, bool storeDateTimeAsTicks = true) : base(databasePath, storeDateTimeAsTicks) { }

        public IEnumerable<Entity.PostFeed> GetAllPostsFromLocal()
        {
            var retVal = Query<Entity.PostFeed>("select * from PostFeed");

            foreach (var item in retVal)
            {
                if(item.PosterEmail != null)
                    item.Poster = Query<Entity.Contact>(string.Format("select * from Contact Where EmailAdd = '{0}'", item.PosterEmail)).First();

                if (item.SupportersIdsList == null) item.SupportersIdsList = new List<int>();
                item.SupportersIdsList = GetPostFeedLikesByPostFeedId(item.PostFeedID).Select(i => i.ContactID);
            }

            //chito.order by date created and postfeedid resulted the same. But since integer has lesser bug probability
            //then we just use it.
            return retVal.OrderByDescending(p => p.PostFeedID);
        }

        public IEnumerable<Entity.PostFeed> GetCommentsFromLocal(int parentPostFeedId) => Query<Entity.PostFeed>($"select * from PostFeed Where PostFeedParentId = {parentPostFeedId}");

        public IEnumerable<Entity.PostFeedLike> GetPostFeedLikes() => Query<Entity.PostFeedLike>("select * from PostFeedLike");

        public IEnumerable<Entity.PostFeedLike> GetPostFeedLikesByPostFeedId(int postFeedId) => Query<Entity.PostFeedLike>($"select * from PostFeedLike where PostFeedID = {postFeedId}");

        public Entity.PostFeedLike GetPostFeedLikeByContactId(int contactId, int postFeedId) =>
            Query<Entity.PostFeedLike>(string.Format("select * from PostFeedLike where ContactID = '{0}' and PostFeedID = '{1}'", contactId, postFeedId)).FirstOrDefault();

        public Entity.PostFeed GetPostFeedById(int postFeedId)
        {
            var postFeed = Query<Entity.PostFeed>($"select * from PostFeed where PostFeedID = '{postFeedId}'").FirstOrDefault();

            if (postFeed.PosterEmail != null)
                postFeed.Poster = Query<Entity.Contact>($"select * from Contact Where EmailAdd = '{postFeed.PosterEmail}'").First();

            if (postFeed.SupportersIdsList == null) postFeed.SupportersIdsList = new List<int>();
            postFeed.SupportersIdsList = GetPostFeedLikesByPostFeedId(postFeed.PostFeedID).Select(i => i.ContactID);

            return postFeed;
        }

        public void DeleteTable(object objToDelete) => base.Delete(objToDelete);

        public void DeleteTableByType<T>() => base.DeleteAll<T>();

        public void DeletePostFeedAndItsComments(Entity.PostFeed postFeed)
        {
            var comments = GetCommentsFromLocal(postFeed.PostFeedID);

            foreach (var comment in comments)
                base.Delete(comment);

            base.Delete(postFeed);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Unity;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;
using Yol.Punla.FakeEntries;

namespace Yol.Punla.Repository
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(IPostFeedRepository))]
    public class PostFeedRepositoryFake : IPostFeedRepository
    {
        public void DeleteTableByType<T>() { } 

        public void UpdateItem<T>(T item) { }

        public IEnumerable<PostFeed> GetAllPostsFromLocal()
        {
            if (AppUnityContainer.Instance.Resolve<FakeLoadingMorePost>().IsDuplicatePost)
            {
                List<Entity.PostFeed> dupPosts = new List<Entity.PostFeed>();

                foreach (var item in FakeData.FakePostFeeds.Posts)
                    dupPosts.Add(item);

                foreach (var item in FakeData.FakePostFeeds.Posts)
                    dupPosts.Add(item);

                return dupPosts;
            }

            return FakeData.FakePostFeeds.Posts;
        }

        public IEnumerable<PostFeed> GetCommentsFromLocal(int postFeedId) => FakeData.FakePostFeeds.Posts.Where(p => p.PostFeedParentId == postFeedId);

        PostFeedLike IPostFeedRepository.GetPostFeedLikeByContactId(int contactId, int postFeedId)
        {
            return new PostFeedLike();
        }

        public void DeleteTable(object objToDelete)
        {
            
        }

        public IEnumerable<PostFeedLike> GetPostFeedLikes()
        {
            return new List<PostFeedLike>();
        }

        public PostFeed GetPostFeedById(int postFeedId)
        {
            return FakeData.FakePostFeeds.Posts.Where(p => p.PostFeedID == postFeedId).FirstOrDefault();
        }

        public void DeletePostFeedAndItsComments(PostFeed postFeed)
        {
            
        }
    }
}

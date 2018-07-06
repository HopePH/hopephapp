using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;
using Yol.Punla.GatewayAccess;
using Yol.Punla.Repository;
using Yol.Punla.Utility;

namespace Yol.Punla.Managers
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(IPostFeedManager))]
    [DefaultModuleInterfaced(ParentInterface = typeof(IPostFeedManager))]
    public class PostFeedManager : ManagerBase, IPostFeedManager
    {
        private readonly IPostFeedService _postFeedService;
        private readonly IPostFeedRepository _postFeedRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IKeyValueCacheUtility _keyValueCachedUtility;
        private IEnumerable<PostFeed> _cachedPostFeeds;
        private IEnumerable<PostFeed> _cachedComments;
        private List<PostFeed> _cachedPostFeedsList;
        private PostFeed _updatedPostFeedFromLocal;

        public bool WasForcedGetToTheRest { get; set; }
        public bool WasForcedGetToLocal { get; set; }

        public PostFeedManager(ILocalTableTrackingRepository localTableTrackingRepository,
            IPostFeedService postFeedService,
            IPostFeedRepository postFeedRepository,
            IContactRepository contactRepository) : base(localTableTrackingRepository)
        {
            _postFeedService = postFeedService;
            _postFeedRepository = postFeedRepository;
            _contactRepository = contactRepository;
            _keyValueCachedUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
        }

        public async Task<IEnumerable<PostFeed>> GetAllPosts(bool isForcedGetToTheRest = false, int posterId = 0)
        {
            try
            {
                WasForcedGetToTheRest = isForcedGetToTheRest;
                _cachedPostFeeds = _postFeedRepository.GetAllPostsFromLocal();   // This gets all the post from local db INCLUDING comments

                if (!IsInternetConnected)
                    return _cachedPostFeeds;

                if (CheckingIfLocalTableHasExpired(nameof(PostFeed)) || WasForcedGetToTheRest)
                {
                    _cachedPostFeeds = await _postFeedService.GetTopPostFeeds(posterId); // This will only get all post NOT including comments
                    SavePostFeedsToLocal(_cachedPostFeeds, true);
                    return _cachedPostFeeds;
                }

                if (_cachedPostFeeds == null || (_cachedPostFeeds != null && _cachedPostFeeds.Count() < 1))
                {
                    _cachedPostFeeds = await _postFeedService.GetTopPostFeeds(posterId);
                    SavePostFeedsToLocal(_cachedPostFeeds, true);
                }

                return _cachedPostFeeds;
            }
            catch (SQLite.SQLiteException)
            {
                return _cachedPostFeeds;
            }
        }

        public async Task<IEnumerable<PostFeed>> GetAllPostsWithSpeed(int posterId, int postFeedBaseId, bool isFirstLoad, bool isForcedGetToTheRest = false, bool isForcedGetToLocal = false)
        {
            try
            {
                WasForcedGetToLocal = isForcedGetToLocal;
                WasForcedGetToTheRest = isForcedGetToTheRest;
                _cachedPostFeeds = _postFeedRepository.GetAllPostsFromLocal();
                IEnumerable<PostFeed> tempPostFeeds = null;

                if (!IsInternetConnected || WasForcedGetToLocal)
                {
                    _cachedPostFeedsList = new List<PostFeed>(_cachedPostFeeds);
                    return _cachedPostFeedsList;
                }

                if (CheckingIfLocalTableHasExpired(nameof(PostFeed)) || WasForcedGetToTheRest)
                {
                    isFirstLoad = true;
                    _cachedPostFeeds = await _postFeedService.GetPostFeedsWithSpeed(posterId, postFeedBaseId, isFirstLoad);
                    SavePostFeedsToLocal(_cachedPostFeeds, true);
                    _cachedPostFeedsList = new List<PostFeed>(_cachedPostFeeds);
                    return _cachedPostFeedsList;
                }

                if (_cachedPostFeeds == null || (_cachedPostFeeds != null && _cachedPostFeeds.Count() < 1))
                {
                    isFirstLoad = true;
                    _cachedPostFeeds = await _postFeedService.GetPostFeedsWithSpeed(posterId, postFeedBaseId, isFirstLoad);
                    SavePostFeedsToLocal(_cachedPostFeeds, true);
                    _cachedPostFeedsList = new List<PostFeed>(_cachedPostFeeds);
                    return _cachedPostFeedsList;
                }

                tempPostFeeds = await _postFeedService.GetPostFeedsWithSpeed(posterId, postFeedBaseId, isFirstLoad);
                SavePostFeedsToLocal(tempPostFeeds, isFirstLoad);
                _cachedPostFeeds = _postFeedRepository.GetAllPostsFromLocal();
                _cachedPostFeedsList = new List<PostFeed>(_cachedPostFeeds);
                return _cachedPostFeedsList;
            }
            catch (SQLite.SQLiteException)
            {
                return _cachedPostFeedsList;
            }
        }

        public async Task<IEnumerable<PostFeed>> GetOwnPosts(int posterId = 0)
        {
            return await _postFeedService.GetOwnPostsFeeds(posterId);
        }

        public async Task<IEnumerable<PostFeed>> GetComments(int postFeedId, bool isForcedGetToTheRest = false, int posterId = 0)
        {
            try
            {
                WasForcedGetToTheRest = isForcedGetToTheRest;
                _cachedComments = _postFeedRepository.GetCommentsFromLocal(postFeedId);

                if (!IsInternetConnected)
                    return _cachedComments;

                if (WasForcedGetToTheRest || _cachedComments == null || (_cachedComments != null && _cachedComments.Count() < 1))
                {
                    _cachedComments = await _postFeedService.GetPostFeedComments(postFeedId, posterId);
                    SaveCommentsToLocal(_cachedComments, postFeedId);
                }

                return _cachedComments;
            }
            catch (SQLite.SQLiteException)
            {
                return _cachedComments;
            }
        }

        public IEnumerable<PostFeed> LikeOrUnlikeSelfPost(int postFeedId, int userWhoLikedId)
        {
            try
            {
                var currentPost = _postFeedRepository.GetPostFeedById(postFeedId);
                currentPost.IsSelfSupported = !currentPost.IsSelfSupported;
                currentPost.NoOfSupports = (currentPost.IsSelfSupported) ? currentPost.NoOfSupports + 1 : currentPost.NoOfSupports - 1;
                _postFeedRepository.UpdateItem(currentPost);

                var postFeedLikeInLocal = _postFeedRepository.GetPostFeedLikeByContactId(userWhoLikedId, currentPost.PostFeedID);

                if (postFeedLikeInLocal == null)
                    _postFeedRepository.UpdateItem(new PostFeedLike { ContactID = userWhoLikedId, PostFeedID = currentPost.PostFeedID });
                else
                    _postFeedRepository.DeleteTable(postFeedLikeInLocal);

                _cachedPostFeeds = _postFeedRepository.GetAllPostsFromLocal();
                return _cachedPostFeeds;
            }
            catch (SQLite.SQLiteException)
            {
                return _cachedPostFeeds;
            }           
        }

        public async Task<int> SaveNewPost(PostFeed newPost)
        {
            if (newPost == null) return 0;

            var result = await _postFeedService.SavePostToServer(newPost);

            if (result > 0)
            {
                newPost.PostFeedID = result;
                newPost.IsSelfPosted = true;

                if (newPost.PosterId == int.Parse(_keyValueCachedUtility.GetUserDefaultsKeyValue("CurrentContactId")))
                    newPost.IsSelfPosted = true;

                try
                {
                    _postFeedRepository.UpdateItem(newPost);
                }
                catch (SQLite.SQLiteException)
                {
                }               
            }

            return result;
        }

        public async Task<HttpStatusCode> DeleteSelfPost(PostFeed PostFeed)
        {
            var result = await _postFeedService.DeleteSelfPostFromRemote(PostFeed.PostFeedID);

            if (result == HttpStatusCode.OK)
            {
                PostFeed.IsDelete = true;

                try
                {
                    _postFeedRepository.UpdateItem(PostFeed);
                }
                catch (SQLite.SQLiteException)
                {
                }                
            }

            return result;
        }

        public PostFeed UpdatePostFeedAndPostFeedLikeToLocal(Entity.PostFeed existingPostFeed, Entity.Contact userWhoLiked)
        {
            try
            {
                if (existingPostFeed.PosterId == int.Parse(_keyValueCachedUtility.GetUserDefaultsKeyValue("CurrentContactId")))
                    existingPostFeed.IsSelfPosted = true;

                var postFeedLikeInLocal = _postFeedRepository.GetPostFeedLikeByContactId(userWhoLiked.RemoteId, existingPostFeed.PostFeedID);
                existingPostFeed.NoOfSupports = (postFeedLikeInLocal == null) ? existingPostFeed.NoOfSupports + 1 : existingPostFeed.NoOfSupports - 1;

                if (postFeedLikeInLocal == null)
                    _postFeedRepository.UpdateItem(new PostFeedLike { ContactID = userWhoLiked.RemoteId, PostFeedID = existingPostFeed.PostFeedID });
                else
                    _postFeedRepository.DeleteTable(postFeedLikeInLocal);

                _postFeedRepository.UpdateItem(existingPostFeed);
                _updatedPostFeedFromLocal = _postFeedRepository.GetPostFeedById(existingPostFeed.PostFeedID);
                // 01-11-2018 12:31pm REYNZ 
                // since comments are not saved locally because sqlite cannot saved List, i explicitly added it before returning the updated post to the caller
                //existingPostFeed.Comments = postComments;
                _updatedPostFeedFromLocal.Comments = existingPostFeed.Comments;
                _updatedPostFeedFromLocal.NoOfSupports = existingPostFeed.NoOfSupports;
                return _updatedPostFeedFromLocal;
            }
            catch (SQLite.SQLiteException)
            {
                return _updatedPostFeedFromLocal;
            }           
        }

        public PostFeed GetPostFeed(int postFeedId) => _postFeedRepository.GetPostFeedById(postFeedId);

        public void SavePostFeedsToLocal(IEnumerable<PostFeed> postFeeds, bool tableForceDelete = false)
        {
            try
            {
                if (postFeeds == null || postFeeds.Count() < 1) return;

                if (tableForceDelete)
                {
                    _postFeedRepository.DeleteTableByType<PostFeedLike>();
                    _postFeedRepository.DeleteTableByType<PostFeed>();
                }

                foreach (var item in postFeeds)
                {
                    if (item.PosterId == int.Parse(_keyValueCachedUtility.GetUserDefaultsKeyValue("CurrentContactId")))
                        item.IsSelfPosted = true;

                    if (!string.IsNullOrEmpty(item.ContactsWhoLiked))
                    {
                        string[] contacts = item.ContactsWhoLiked.Split(',');

                        foreach (var c in contacts)
                            _postFeedRepository.UpdateItem(new PostFeedLike { ContactID = int.Parse(c.Trim()), PostFeedID = item.PostFeedID });
                    }

                    _postFeedRepository.UpdateItem(item);
                    _contactRepository.UpdateItem(item.Poster);
                }

                UpdateLocalTableTracking(nameof(PostFeed));
            }
            catch (SQLite.SQLiteException)
            {
            }
        }

        public void SaveNewPostToLocal(PostFeed newPost)
        {
            try
            {
                var localPost = _postFeedRepository.GetPostFeedById(newPost.PostFeedID);

                if (localPost == null)
                    localPost = newPost;
                else
                {
                    var localId = localPost.Id;
                    localPost = newPost;
                    localPost.Id = localId;
                }

                if (localPost.PosterId == int.Parse(_keyValueCachedUtility.GetUserDefaultsKeyValue("CurrentContactId")))
                    localPost.IsSelfPosted = true;

                if (newPost.Poster != null)
                {
                    localPost.Poster = newPost.Poster;
                    localPost.PosterFirstName = newPost.Poster.FirstName;
                    localPost.PosterLastName = newPost.Poster.LastName;
                    _contactRepository.UpdateItem(localPost.Poster);
                }

                _postFeedRepository.UpdateItem(localPost);
            }
            catch (SQLite.SQLiteException)
            {
            }           
        }

        public void DeletePostInLocal(PostFeed PostFeed) => _postFeedRepository.DeletePostFeedAndItsComments(PostFeed);

        private void SaveCommentsToLocal(IEnumerable<PostFeed> comments, int parentPostFeedId)
        {
            if (comments == null || comments.Count() < 1) return;

            foreach (var item in comments)
            {
                if (item.PosterId == int.Parse(_keyValueCachedUtility.GetUserDefaultsKeyValue("CurrentContactId")))
                    item.IsSelfPosted = true;

                try
                {
                    if (!string.IsNullOrEmpty(item.ContactsWhoLiked))
                    {
                        string[] contacts = item.ContactsWhoLiked.Split(',');

                        foreach (var c in contacts)
                            _postFeedRepository.UpdateItem(new PostFeedLike { ContactID = int.Parse(c.Trim()), PostFeedID = item.PostFeedID });
                    }

                    _postFeedRepository.UpdateItem(item);
                }
                catch (SQLite.SQLiteException)
                {
                }
            }
        }
    }
}
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Localized;
using Yol.Punla.Managers;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;
using System.Linq;
using Yol.Punla.Messages;
using Yol.Punla.Barrack;
using Xamarin.Forms;
using Unity;
using Yol.Punla.Utility;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class PostFeedMyselfPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;
        private readonly IContactManager _contactManager;
        private readonly IPostFeedManager _postFeedManager;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;

        private string _busyComments;
        public string BusyComments
        {
            get => _busyComments;
            set => SetProperty(ref _busyComments, value);
        }

        public ICommand NavigateBackCommand => new DelegateCommand(GoBack);
        public ICommand CommentCommand => new DelegateCommand<Entity.PostFeed>(RedirectToPostFeedDetails);
        public ICommand RefreshListCommand => new DelegateCommand(async () => await RefreshPostListAsync());
        public ICommand SupportCommand => new DelegateCommand<Entity.PostFeed>(SupportPost);
        public ObservableCollection<Entity.PostFeed> PostsList { get; set; }
        public Entity.PostFeed CurrentPostFeed { get; set; }
        public Entity.Contact CurrentContact { get; set; }
        public bool IsNavigatingToDetailsPage { get; set; }        
        public bool IsShowPullDownInstruction { get; set; } = false;
        public bool IsShowPostOptions { get; set; } = false;
        public bool IsListRefreshing { get; set; }

        public PostFeedMyselfPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationService navigationService, 
            INavigationStackService navigationStackService,
            IContactManager contactManager,
            IPostFeedManager postFeedManager) : base(serviceMapper, appUser)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _contactManager = contactManager;
            _postFeedManager = postFeedManager;
            _busyComments = AppStrings.LoadingOwnData;
            Title = AppStrings.TitleThoughts;
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
        }

        #region "Prepare Page Bindings"

        public override void PreparingPageBindings()
        {
            if (PassingParameters != null && PassingParameters.ContainsKey("CurrentContact"))
                CurrentContact = (Entity.Contact)PassingParameters["CurrentContact"];
            else
                CurrentContact = _contactManager.GetCurrentContactFromLocal();

            PreparePageBindingsAsync();
            PreparePageBindingsAsyncFAKE();
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private async void PreparePageBindingsAsync()
        {
            try
            {
                var ownPostList = await _postFeedManager.GetOwnPosts(CurrentContact.RemoteId);
                PostsList = new ObservableCollection<Entity.PostFeed>(ownPostList);
                PreparePageBindingResult();
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }

        [Conditional("FAKE")]
        private void PreparePageBindingsAsyncFAKE()
        {
            var ownPostList = _postFeedManager.GetOwnPosts(CurrentContact.RemoteId).Result;
            PostsList = new ObservableCollection<Entity.PostFeed>(ownPostList);
            PreparePageBindingResult();
        }

        private void PreparePageBindingResult()
        {
            IsBusy = false;
        }

        #endregion

        #region GET AND ADD COMMENTS

        private void RedirectToPostFeedDetails(Entity.PostFeed SelectedPost)
        {
            IsNavigatingToDetailsPage = true;
            BusyComments = AppStrings.LoadingComments;
            IsBusy = true;
            CurrentPostFeed = SelectedPost;

            if (ProcessInternetConnection(true))
            {
                GetCommentsAsync();
                GetCommentsAsyncFake();
            }
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private async void GetCommentsAsync()
        {
            try
            {
                CreateNewHandledTokenSource("GetCommentsAsync", 20);

                var results = await Task.Run(() =>
                {
                    Debug.WriteLine("HOPEPH Getting comments");
                    return _postFeedManager.GetComments(CurrentPostFeed.PostFeedID, true, CurrentContact.RemoteId);
                }, TokenHandler.Token);

                GetCommentsResult(results, TokenHandler.IsTokenSourceCompleted());
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }

        [Conditional("FAKE")]
        private void GetCommentsAsyncFake()
        {
            var result = _postFeedManager.GetComments(CurrentPostFeed.PostFeedID, false, CurrentContact.RemoteId).Result;
            GetCommentsResult(result);
        }

        private void GetCommentsResult(IEnumerable<Entity.PostFeed> commentList, bool IsSuccess = true)
        {
            if (IsSuccess)
            {
                //chito. reset if the navigation parameters has values. this happens because of background thread navigating back from other page
                if (PassingParameters != null && PassingParameters.ContainsKey("SelectedPost"))
                    PassingParameters = new NavigationParameters();

                CurrentPostFeed.Comments = new ObservableCollection<Entity.PostFeed>(commentList.Where(p => p.PostFeedParentId == CurrentPostFeed.PostFeedID));
                PassingParameters.Add("CurrentUser", CurrentContact);
                PassingParameters.Add("SelectedPost", CurrentPostFeed);
                NavigateToPageHelper(nameof(ViewNames.PostFeedDetailPage), _navigationStackService, _navigationService, PassingParameters);
            }

            IsBusy = false;
        }

        #endregion

        public void SendErrorToHockeyApp(Exception ex)
        {
            ProcessErrorReportingForHockeyApp(ex, true);
        }

        public void AddOneLikeToThisPostFromLocal(Entity.PostFeed postFeed, Entity.Contact userWhoLiked)
        {
            var postToLike = _postFeedManager.GetPostFeed(postFeed.PostFeedID);

            if (userWhoLiked.RemoteId == CurrentContact.RemoteId)
                postToLike.IsSelfSupported = !postToLike.IsSelfSupported;

            _postFeedManager.UpdatePostFeedAndPostFeedLikeToLocal(postToLike, userWhoLiked);
            var wholeList = _postFeedManager.GetAllPosts(false).Result.Where(p => p.IsDelete == false && p.PostFeedLevel != 1);
            var ownPostList = wholeList.Where(x => x.PosterId == CurrentContact.RemoteId);
            PostsList = new ObservableCollection<Entity.PostFeed>(ownPostList);
        }

        public Entity.PostFeed ReadPostFeedFromLocal(Entity.PostFeed newPost) => _postFeedManager.GetPostFeed(newPost.PostFeedID);

        private async Task RefreshPostListAsync()
        {
            try
            {
                BusyComments = AppStrings.LoadingData;
                IsBusy = true;
                IsListRefreshing = true;
                var postList = await _postFeedManager.GetOwnPosts(CurrentContact.RemoteId);
                PostsList = new ObservableCollection<Entity.PostFeed>(postList);
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
            finally
            {
                IsListRefreshing = false;
                IsBusy = false;
            }
        }

        private void GoBack()
        {
            _keyValueCacheUtility.GetUserDefaultsKeyValue("IsForceToGetToLocal", "true");
            NavigateBackHelper(_navigationStackService, _navigationService);
        }

        private void SupportPost(Entity.PostFeed SelectedPost)
        {
            CurrentPostFeed = SelectedPost;

            if (IsInternetConnected && !IsNavigatingToDetailsPage)
            {
                PostFeedMessage postFeedMessage = new PostFeedMessage
                {
                    CurrentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.PostFeedK>(CurrentPostFeed),
                    CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(CurrentContact)
                };
                MessagingCenter.Send(postFeedMessage, "LikeOrUnlikePostFeedToHub");
                var wholeList = _postFeedManager.LikeOrUnlikeSelfPost(CurrentPostFeed.PostFeedID, CurrentContact.RemoteId);
                var ownPostList = wholeList.Where(x => x.PosterId == CurrentContact.RemoteId);
                PostsList = new ObservableCollection<Entity.PostFeed>(ownPostList);
            }
        }
    }
}

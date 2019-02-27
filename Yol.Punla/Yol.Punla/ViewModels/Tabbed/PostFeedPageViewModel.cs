/*Chito.26/12/2017. The Entity to View binding is allowed to bind directly in this VM for purpose.
 */
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Localized;
using Yol.Punla.Managers;
using Yol.Punla.Messages;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class PostFeedPageViewModel : ChildViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;
        private readonly IPostFeedManager _postFeedManager;
        private readonly IContactManager _contactManager;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;

        private string _busyComments;
        public string BusyComments
        {
            get => _busyComments;
            set => SetProperty(ref _busyComments, value);
        }

        public ICommand ShowPostOptionsCommand => new DelegateCommand<Entity.PostFeed>(ShowPostOptions);
        public ICommand ClosePostOptionsCommand => new DelegateCommand(ClosePostOptions);
        public ICommand SupportCommand => new DelegateCommand<Entity.PostFeed>(SupportPost); 
        public ICommand CommentCommand => new DelegateCommand<Entity.PostFeed>(RedirectToPostFeedDetails);
        public ICommand AddPostCommand => new DelegateCommand(AddNewPost);
        public ICommand EditPostCommand => new DelegateCommand(EditPost);
        public ICommand DeletePostCommand => new DelegateCommand(DeleteSelfPost);
        public ICommand RefreshListCommand => new DelegateCommand(async () => await RefreshPostListAsync());
        public ICommand CameraCommand => new DelegateCommand(async () => await TakeCamera());
        public ICommand RemovePullDownInstructionCommand => new DelegateCommand(RemovePullDownInstruction);
        public ICommand ShowUnavailablePopUpCommand => new DelegateCommand(ShowUnavailablePopUp);
        public ICommand DisplayOwnPostsCommand => new DelegateCommand(RedirectToPostFeedOwn);
        public ICommand LoadMoreCommand => new DelegateCommand(async () => await LoadMorePostListAsync());
        public ObservableCollection<Entity.PostFeed> PostsList { get; set; }
        public Entity.Contact CurrentContact { get; set; }
        public Entity.PostFeed CurrentPostFeed { get; set; }
        public bool IsShowPostOptions { get; set; }
        public bool IsListRefreshing { get; set; }
        public bool IsForceToGetToRest { get; set; }
        public bool IsForceToGetToLocal { get; set; }
        public bool IsDeletePostFeedRequestSentToHub { get; set; }
        public bool IsShowPullDownInstruction { get; set; }
        public int SecondsDelay { get; set; }
        public bool IsNavigatingToDetailsPage { get; set; }

        public PostFeedPageViewModel(IEventAggregator eventAggregator,
            IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationService navigationService,
            INavigationStackService navigationStackService,
            IPostFeedManager postFeedManager,
            IContactManager contactManager) : base(eventAggregator, serviceMapper, appUser, navigationService)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _postFeedManager = postFeedManager;
            _contactManager = contactManager;
            _busyComments = AppStrings.LoadingData;
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
            Title = "Wall";
        }

        #region PREPARE PAGE BINDINGS

        public override void PreparingPageBindingsChild()
        {
            if (PassingParameters != null && PassingParameters.ContainsKey("CurrentContact"))
                CurrentContact = (Entity.Contact)PassingParameters["CurrentContact"];
            else
                CurrentContact = _contactManager.GetCurrentContactFromLocal();

            IsForceToGetToRest = false;
            var isForceToGetFromRestString = _keyValueCacheUtility.GetUserDefaultsKeyValue("IsForceToGetFromRest");

            if (!string.IsNullOrEmpty(isForceToGetFromRestString))
            {
                IsForceToGetToRest = bool.Parse(isForceToGetFromRestString);
                _keyValueCacheUtility.RemoveKeyObject("IsForceToGetFromRest");
            }

            SecondsDelay = 0;
            var secondsDelayString = _keyValueCacheUtility.GetUserDefaultsKeyValue("SecondsDelay");

            if (!string.IsNullOrEmpty(secondsDelayString))
            {
                SecondsDelay = int.Parse(secondsDelayString);
                _keyValueCacheUtility.RemoveKeyObject("SecondsDelay");
            }

            IsForceToGetToLocal = false;
            var isForceToGetLocalString = _keyValueCacheUtility.GetUserDefaultsKeyValue("IsForceToGetToLocal");

            if (!string.IsNullOrEmpty(isForceToGetLocalString))
            {
                IsForceToGetToLocal = bool.Parse(isForceToGetLocalString);
                _keyValueCacheUtility.RemoveKeyObject("IsForceToGetToLocal");
            }

            IsNavigatingToDetailsPage = false;
            BusyComments = AppStrings.LoadingData;
            IsBusy = true;
            CurrentPostFeed = null;
            PreparePageBindingsAsync();
            PreparePageBindingsAsyncFake();
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private async void PreparePageBindingsAsync()
        {
            //chito.22/01/2018. we force to delay for how many seconds when adding/editing a new post so we could get the updated info from fetching from the server 
            if (IsForceToGetToRest && SecondsDelay > 0)
                await Task.Delay(TimeSpan.FromSeconds(SecondsDelay));

            try
            {
                CreateNewHandledTokenSource("PostFeedPageViewModel.PreparePageBindingsAsync", 20);

                var postList = await Task.Run(() =>
                {
                    Debug.WriteLine("HOPEPH Getting all post");
                    return _postFeedManager.GetAllPostsWithSpeed(CurrentContact.RemoteId, 0, true, IsForceToGetToRest, IsForceToGetToLocal);
                }, TokenHandler.Token);
                
                PostsList = new ObservableCollection<Entity.PostFeed>(postList.Where(p => p.IsDelete == false && p.PostFeedLevel != 1));
                PreparePageBindingsResult(TokenHandler.IsTokenSourceCompleted());
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }

        [Conditional("FAKE")]
        private void PreparePageBindingsAsyncFake()
        {
            PostsList = new ObservableCollection<Entity.PostFeed>(_postFeedManager.GetAllPostsWithSpeed(CurrentContact.RemoteId, 0, true).Result.Where(p => p.IsDelete == false && p.PostFeedLevel != 1));
            PreparePageBindingsResult();
        }

        private void PreparePageBindingsResult(bool IsSuccess = true)
        {
            if (IsInternetConnected)
                MessagingCenter.Send(new PostFeedMessage { CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(CurrentContact) }, "LogonPostFeedToHub");

            if (!AppUnityContainer.Instance.Resolve<Advertisements>().HasShownPullDownInstructionAlready)
            {
                IsShowPullDownInstruction = true;
                AppUnityContainer.Instance.Resolve<Advertisements>().HasShownPullDownInstructionAlready = true;
            }

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
                NavigateToPageHelper(nameof(ViewNames.PostFeedDetailPage), PassingParameters);
            }

            IsBusy = false;
        }

        #endregion

        public override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<HttpResponseMessage<int>>(this, "DeletePostFeedToHubResultCode", message =>
            {
                IsBusy = false;

                if (!(message.HttpStatusCode == HttpStatusCode.OK))
                    UserDialogs.Instance.Alert(AppStrings.LoadingErrorPostFeed, "Error", "Ok");
            });
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<HttpResponseMessage<int>>(this, "DeletePostFeedToHubResultCode");
        }

        public void SavePostFeedToLocal(Entity.PostFeed newPost)
        {
            _postFeedManager.SaveNewPostToLocal(newPost);
            var updatedList = _postFeedManager.GetAllPosts(false).Result;
            PostsList = new ObservableCollection<Entity.PostFeed>(updatedList.Where(p => p.IsDelete == false && p.PostFeedLevel != 1));
        }

        public void DeletePostFeedFromLocal(Entity.PostFeed newPost)
        {
            var postToDelete = PostsList.Where(p => p.PostFeedID == newPost.PostFeedID).FirstOrDefault();
            _postFeedManager.DeletePostInLocal(postToDelete);
            var updatedList = _postFeedManager.GetAllPosts(false).Result;
            PostsList = new ObservableCollection<Entity.PostFeed>(updatedList.Where(p => p.IsDelete == false));
        }

        public Entity.PostFeed ReadPostFeedFromLocal(Entity.PostFeed newPost) => _postFeedManager.GetPostFeed(newPost.PostFeedID);

        public void AddOneLikeToThisPostFromLocal(Entity.PostFeed postFeed, Entity.Contact userWhoLiked)
        {
            var postToLike = _postFeedManager.GetPostFeed(postFeed.PostFeedID);

            if (userWhoLiked.RemoteId == CurrentContact.RemoteId)
                postToLike.IsSelfSupported = !postToLike.IsSelfSupported;

            _postFeedManager.UpdatePostFeedAndPostFeedLikeToLocal(postToLike, userWhoLiked);
            var updatedList = _postFeedManager.GetAllPosts(false).Result;
            PostsList = new ObservableCollection<Entity.PostFeed>(updatedList.Where(p => p.IsDelete == false));
        }

        public void SendErrorToHockeyApp(Exception ex)
        {
            ProcessErrorReportingForHockeyApp(ex, true);
        }

        private void AddNewPost()
        {
            PassingParameters.Add("CurrentContact", CurrentContact);
            NavigateToPageHelper(nameof(ViewNames.PostFeedAddEditPage),PassingParameters);
        }

        private void EditPost()
        {
            if (!CurrentPostFeed.IsSelfPosted) return;

            //chito. reset if the navigation parameters has values. this happens because of background thread navigating back from postaddeditpage
            if (PassingParameters != null && PassingParameters.ContainsKey("SelectedPost"))
                PassingParameters = new NavigationParameters();

            PassingParameters.Add("CurrentContact", CurrentContact);
            PassingParameters.Add("SelectedPost", CurrentPostFeed);
            NavigateToPageHelper(nameof(ViewNames.PostFeedAddEditPage), PassingParameters);
            IsShowPostOptions = false;
        }

        private void ShowPostOptions(Entity.PostFeed currentPostFeed)
        {
            CurrentPostFeed = currentPostFeed;
            IsShowPostOptions = true;
        }

        private void ClosePostOptions() => IsShowPostOptions = false;

        private async Task RefreshPostListAsync()
        {
            try
            {
                BusyComments = AppStrings.LoadingData;
                IsBusy = true;
                IsListRefreshing = true;
                var postList =  await _postFeedManager.GetAllPostsWithSpeed(CurrentContact.RemoteId, 0, true);               
                PostsList = new ObservableCollection<Entity.PostFeed>(postList.Where(p => p.IsDelete == false && p.PostFeedLevel != 1));
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
                PostsList = new ObservableCollection<Entity.PostFeed>(_postFeedManager.LikeOrUnlikeSelfPost(CurrentPostFeed.PostFeedID, CurrentContact.RemoteId));
            }
        }

        private void DeleteSelfPost()
        {
            if (!ProcessInternetConnection(true))
                return;

            IsShowPostOptions = false;
            DeletePostFeedFromLocal(CurrentPostFeed);

            PostFeedMessage postFeedMessage = new PostFeedMessage
            {
                CurrentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.PostFeedK>(CurrentPostFeed),
                CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(CurrentContact)
            };

            MessagingCenter.Send(postFeedMessage, "DeletePostFeedToHub");
            IsDeletePostFeedRequestSentToHub = true;
        }

        private async Task TakeCamera()
        {
            //chito. when uploading a photo, compressed it so it becomes only 300KB or lower.
            await UserDialogs.Instance.AlertAsync("Attaching a photo is not supported by this version of the app. Please wait for our next version.", "Not Supported");
        }

        private void RemovePullDownInstruction() => IsShowPullDownInstruction = false;

        private void ShowUnavailablePopUp() => UserDialogs.Instance.Alert(AppStrings.SearchAvailableNextVersionText);

        private void RedirectToPostFeedOwn()
        {
            PassingParameters.Add("CurrentContact", CurrentContact);
            NavigateToPageHelper(nameof(ViewNames.PostFeedMyselfPage), PassingParameters);
        }

        private async Task LoadMorePostListAsync()
        {
            try
            {
                BusyComments = AppStrings.LoadingMoreData;
                IsBusy = true;
                var lastPostItem = PostsList.Last();
                var postList = await _postFeedManager.GetAllPostsWithSpeed(CurrentContact.RemoteId, lastPostItem.PostFeedID, false);
                PostsList = new ObservableCollection<Entity.PostFeed>(postList.Where(p => p.IsDelete == false && p.PostFeedLevel != 1));
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
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
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Localized;
using Yol.Punla.Managers;
using Yol.Punla.Messages;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;
using CONSTANTS = Yol.Punla.Barrack.Constants;

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
        private readonly IEventAggregator _eventAggregator;

        private Entity.PostFeed _currentPostFeed;
        public Entity.PostFeed CurrentPostFeed
        {
            get => _currentPostFeed;
            set
            {
                SetProperty(ref _currentPostFeed, value);
                if (value != null && !IsShowPostOptions) CommentCommand.Execute(_currentPostFeed);
            }
        }

        private string _busyComments;
        public string BusyComments
        {
            get => _busyComments;
            set => SetProperty(ref _busyComments, value);
        }

        public string CurrentIcon => CONSTANTS.TABITEM_LIST; 

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
        public IEnumerable<Entity.PostFeed> CommentList { get; set; } = new List<Entity.PostFeed>();
        public IEnumerable<string> SelectedPostFeedSupportersAvatar { get; set; } = new List<string>();
        public Entity.Contact CurrentContact { get; set; }
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
            _eventAggregator = eventAggregator;
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
        }

        #region PREPARE PAGE BINDINGS

        public async override void PreparingPageBindingsChild()
        {
            if (PassingParameters != null && PassingParameters.ContainsKey(nameof(CurrentContact))) CurrentContact = (Entity.Contact)PassingParameters[nameof(CurrentContact)];
            else CurrentContact = _contactManager.GetCurrentContactFromLocal();

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
            await PreparePageBindingsAsync();
        }

        #endregion
        private async Task PreparePageBindingsAsync()
        {
            if (IsForceToGetToRest && SecondsDelay > 0) await Task.Delay(TimeSpan.FromSeconds(SecondsDelay));

            try
            {
                var postList = await _postFeedManager.GetAllPostsWithSpeed(CurrentContact.RemoteId, 0, true, IsForceToGetToRest, IsForceToGetToLocal);
                SavePostsToLocal(postList);
                PostsList = new ObservableCollection<Entity.PostFeed>(postList.Where(p => p.IsDelete == false && p.PostFeedLevel != 1));
                PreparePageBindingsResult();
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }

        private void SavePostsToLocal(IEnumerable<Entity.PostFeed> posts)
        {
            _postFeedManager.SaveAllPostsToLocal(posts.ToList());
        }

        private void PreparePageBindingsResult()
        {
            if(IsInternetConnected)
                _eventAggregator.GetEvent<LogonPostFeedToHubEventModel>().Publish(new PostFeedMessage { CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(CurrentContact) });

            if (!AppUnityContainer.Instance.Resolve<Advertisements>().HasShownPullDownInstructionAlready)
            {
                IsShowPullDownInstruction = true;
                AppUnityContainer.Instance.Resolve<Advertisements>().HasShownPullDownInstructionAlready = true;
            }

            IsBusy = false;
        }
        
        #region GET AND ADD COMMENTS

        private async void RedirectToPostFeedDetails(Entity.PostFeed SelectedPost)
        {
            IsNavigatingToDetailsPage = true;
            BusyComments = AppStrings.LoadingComments;
            IsBusy = true;
            CurrentPostFeed = SelectedPost;

            if (ProcessInternetConnection(true) && CurrentPostFeed != null)
            {
                SelectedPostFeedSupportersAvatar = GetSupportersAvatarsFromLocal();
                CommentList = await GetCommentsAsync();
                SeePostFeedDetails();
                CurrentPostFeed = null;
            }
        }
        
        private async Task<IEnumerable<Entity.PostFeed>> GetCommentsAsync()
        {
            try
            {
                var results =  await _postFeedManager.GetComments(CurrentPostFeed.PostFeedID, true, CurrentContact.RemoteId);
                return results;
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
                return null;
            }
        }

        private async Task<IEnumerable<string>> GetSupportersAvatarsAsync()
        {
            try
            {
                var results = await _postFeedManager.GetSupportersAvatars(CurrentPostFeed.PostFeedID);
                return results;
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
                return null;
            }
        }

        private IEnumerable<string> GetSupportersAvatarsFromLocal()
        {
            try
            {
                var listOfAvatars = new List<string>();
                var postFeedFromLocal = _postFeedManager.GetPostFeed(CurrentPostFeed.PostFeedID);
                var postFeedLikes = _postFeedManager.GetPostFeedLike(postFeedFromLocal.PostFeedID).ToList();
                return postFeedLikes.Select(p => p.ContactPhotoURL);
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
                return null;
            }
        }

        private async void SeePostFeedDetails()
        {
            try
            {
                if (CurrentPostFeed != null)
                {
                    //chito. reset if the navigation parameters has values. this happens because of background thread navigating back from other page
                    if (PassingParameters != null && PassingParameters.ContainsKey("SelectedPost"))
                        PassingParameters = new NavigationParameters();
                    CurrentPostFeed.Comments = new ObservableCollection<Entity.PostFeed>(CommentList);
                    CurrentPostFeed.NoOfComments = CurrentPostFeed.Comments.Count;
                    PassingParameters.Add("CurrentUser", CurrentContact);
                    PassingParameters.Add("SelectedPost", CurrentPostFeed);
                    PassingParameters.Add("SupportersAvatars", SelectedPostFeedSupportersAvatar.ToList());
                    await NavigateToPageHelper(nameof(ViewNames.PostFeedDetailPage), PassingParameters);
                }

                IsBusy = false;
            }
            catch (Exception e)
            {
                var ex = e;
            }
        }

        #endregion

        public override void OnAppearing()
        {
            base.OnAppearing();
            
            _eventAggregator.GetEvent<DeletePostFeedToHubResultCodeEventModel>().Subscribe((message) =>
            {
                IsBusy = false;

                if (!(message.HttpStatusCode == HttpStatusCode.OK))
                    UserDialogs.Instance.Alert(AppStrings.LoadingErrorPostFeed, "Error", "Ok");
            });
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

        private async void AddNewPost()
        {
            PassingParameters.Add("CurrentContact", CurrentContact);
            await NavigateToPageHelper(nameof(ViewNames.PostFeedAddEditPage),PassingParameters);
        }

        private async void EditPost()
        {
            if (!CurrentPostFeed.IsSelfPosted) return;

            //chito. reset if the navigation parameters has values. this happens because of background thread navigating back from postaddeditpage
            if (PassingParameters != null && PassingParameters.ContainsKey("SelectedPost"))
                PassingParameters = new NavigationParameters();

            PassingParameters.Add("CurrentContact", CurrentContact);
            PassingParameters.Add("SelectedPost", CurrentPostFeed);
            await NavigateToPageHelper(nameof(ViewNames.PostFeedAddEditPage), PassingParameters);
            IsShowPostOptions = false;
        }

        private void ShowPostOptions(Entity.PostFeed currentPostFeed)
        {
            IsShowPostOptions = true;
            CurrentPostFeed = currentPostFeed;
        }

        private void ClosePostOptions()
        {
            IsShowPostOptions = false;
            CurrentPostFeed = null;
        }

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
            if (IsInternetConnected)
            {
                PostFeedMessage postFeedMessage = new PostFeedMessage
                {
                    CurrentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.PostFeedK>(SelectedPost),
                    CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(CurrentContact)
                };

                _eventAggregator.GetEvent<LikeOrUnlikePostFeedToHubEventModel>().Publish(postFeedMessage);

                var result = _postFeedManager.LikeOrUnlikeSelfPost(SelectedPost.PostFeedID, CurrentContact.RemoteId);
                PostsList = new ObservableCollection<Entity.PostFeed>(result);
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
            
            _eventAggregator.GetEvent<DeletePostFeedToHubEventModel>().Publish(postFeedMessage);
            IsDeletePostFeedRequestSentToHub = true;
        }

        private async Task TakeCamera()
        {
            if (IsShowPostOptions) return;
            //chito. when uploading a photo, compressed it so it becomes only 300KB or lower.
            await UserDialogs.Instance.AlertAsync("Attaching a photo is not supported by this version of the app. Please wait for our next version.", "Not Supported");
        }

        private void RemovePullDownInstruction() => IsShowPullDownInstruction = false;

        private void ShowUnavailablePopUp() => UserDialogs.Instance.Alert(AppStrings.SearchAvailableNextVersionText);

        private async void RedirectToPostFeedOwn()
        {
            PassingParameters.Add("CurrentContact", CurrentContact);
            await NavigateToPageHelper(nameof(ViewNames.PostFeedMyselfPage), PassingParameters);
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
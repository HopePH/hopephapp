using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;
using Yol.Punla.GatewayAccess;
using Yol.Punla.Localized;
using Yol.Punla.Managers;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class NotificationsPageViewModel : ViewModelBase
    {
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;
        private readonly IContactManager _contactManager;
        private readonly IPostFeedManager _postFeedManager;
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;

        private PostFeed _currentPostFeed;
        public PostFeed CurrentPostFeed
        {
            get => _currentPostFeed;
            set
            {
                SetProperty(ref _currentPostFeed, value);

                if (value != null)
                {
                    RedirectToPostFeedDetails(value);
                    CurrentPostFeed = null;
                }
            }
        }

        public ICommand RefreshListCommand => new DelegateCommand(async () => await RefreshNotificationsAsync());
        public IEnumerable<PostFeed> Notifications { get; set; }
        public Entity.Contact CurrentContact { get; set; }
        public bool IsListRefreshing { get; set; }
        public bool IsShowOfflineMessage { get; set; }
        public string BusyComments { get; set; }

        public NotificationsPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            IContactManager contactManager,
            IPostFeedManager postFeedManager,
            INavigationService navigationService,
            INavigationStackService navigationStackService) : base(navigationService)
        {
            _contactManager = contactManager;
            _postFeedManager = postFeedManager;
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
        }

        #region Prepare Page Bindings

        public override void PreparingPageBindings()
        {
            if (Notifications != null)
                Notifications = null;

            BusyComments = AppStrings.LoadingNotifications;
            _keyValueCacheUtility.RemoveKeyObject("IsForceToGetToLocal");
            CurrentContact = _contactManager.GetCurrentContactFromLocal();
            IsShowBackArrow = false;
            Title = "Notifications";

            if (IsInternetConnected)
            {
                PreparePageBindingsAsync();
                PreparePageBindingAsyncFake();
            }
            else
            {
                IsBusy = false;
                IsShowOfflineMessage = true;
            }
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private async void PreparePageBindingsAsync()
        {
            try
            {
                CreateNewHandledTokenSource("HomePageViewModel.PreparePageBindingsAsync", 20);

                var results = await Task.Run<IEnumerable<PostFeed>>(() =>
                {
                    return AppUnityContainer.Instance.Resolve<IPostFeedService>().GetPostFeedNotifications(CurrentContact.RemoteId);
                }, TokenHandler.Token);

                PreparePageBindingsResult(results, TokenHandler.IsTokenSourceCompleted());
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }

        [Conditional("FAKE")]
        private void PreparePageBindingAsyncFake()
        {
            var results = AppUnityContainer.Instance.Resolve<IPostFeedService>().GetPostFeedNotifications(CurrentContact.RemoteId).Result;
            PreparePageBindingsResult(results);
        }

        private void PreparePageBindingsResult(IEnumerable<PostFeed> items, bool isSuccess = false)
        {
            if (items != null && items.Count() > 0)
                Notifications = items;

            IsBusy = false;
        }

        #endregion

        #region GET AND ADD COMMENTS

        private void RedirectToPostFeedDetails(Entity.PostFeed SelectedPost)
        {
            BusyComments = AppStrings.LoadingComments;
            IsBusy = true;

            if (ProcessInternetConnection(true))
            {
                GetCommentsAsync(SelectedPost);
                GetCommentsAsyncFake(SelectedPost);
            }
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private async void GetCommentsAsync(Entity.PostFeed SelectedPost)
        {
            try
            {
                CreateNewHandledTokenSource("GetCommentsAsync", 20);

                var results = await Task.Run(() =>
                {
                    Debug.WriteLine("HOPEPH Getting comments");
                    return _postFeedManager.GetComments(SelectedPost.PostFeedID, true, CurrentContact.RemoteId);
                }, TokenHandler.Token);

                GetCommentsResult(results, SelectedPost, TokenHandler.IsTokenSourceCompleted());
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }

        [Conditional("FAKE")]
        private void GetCommentsAsyncFake(Entity.PostFeed SelectedPost)
        {
            var result = _postFeedManager.GetComments(SelectedPost.PostFeedID, true, CurrentContact.RemoteId).Result;
            GetCommentsResult(result, SelectedPost);
        }

        private void GetCommentsResult(IEnumerable<Entity.PostFeed> commentList, Entity.PostFeed SelectedPost, bool IsSuccess = true)
        {
            if (IsSuccess)
            {
                if (PassingParameters != null && PassingParameters.ContainsKey("SelectedPost"))
                    PassingParameters = new NavigationParameters();

                SelectedPost.Comments = new ObservableCollection<Entity.PostFeed>(commentList.Where(p => p.PostFeedParentId == SelectedPost.PostFeedID));
                PassingParameters.Add("CurrentUser", CurrentContact);
                PassingParameters.Add("SelectedPost", SelectedPost);
                NavigateToPageHelper(nameof(ViewNames.PostFeedDetailPage), PassingParameters);
            }

            IsBusy = false;
        }

        #endregion

        private async Task RefreshNotificationsAsync()
        {
            try
            {
                IsBusy = true;
                IsListRefreshing = true;

                if (IsInternetConnected)
                {
                    Notifications = await AppUnityContainer.Instance.Resolve<IPostFeedService>().GetPostFeedNotifications(CurrentContact.RemoteId);
                    IsShowOfflineMessage = false;
                }
                else
                    IsShowOfflineMessage = true;
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
    }
}

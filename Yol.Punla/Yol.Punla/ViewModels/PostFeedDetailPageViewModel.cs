﻿/*Chito.26/12/2017. The Entity to View binding is allowed to bind directly in this VM for purpose.
 */
using Acr.UserDialogs;
using FluentValidation;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;
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
    public class PostFeedDetailPageViewModel : ViewModelBase
    {
        private readonly IValidator _validator;
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;
        private readonly IKeyboardHelper _keyboardHelper;
        private readonly IPostFeedManager _postFeedManager;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;
        private readonly IEventAggregator _eventAggregator;

        public ICommand ClosePostOptionsCommand => new DelegateCommand(ClosePostOptions);
        public ICommand ShowPostOptionsCommand => new DelegateCommand<Entity.PostFeed>(ShowPostOptions);
        public ICommand NavigateBackCommand => new DelegateCommand(GoBack);
        public ICommand WriteCommentCommand => new DelegateCommand(WriteComment);
        public ICommand SupportCommand => new DelegateCommand(SupportPost);
        public ICommand CameraCommand => new DelegateCommand(async () => await TakeCamera());
        public ICommand DeleteCommentCommand => new DelegateCommand(DeleteSelfComment);
        public ICommand EditCommentCommand => new DelegateCommand(EditSelfComment);
        public ObservableCollection<Views.CommentItem> CommentItems { get; set; } = new ObservableCollection<Views.CommentItem>();
        public IEnumerable<string> SupportersAvatars = new List<string>();
        public PostFeed CurrentPostFeed { get; set; }
        public Contact CurrentContact { get; set; }
        public PostFeed Comment { get; set; }
        public PostFeed SelectedComment { get; set; }
        public string PostImageUrl { get; set; }
        public string PlaceholderText { get; set; }
        public string CommentText { get; set; }
        public string DeletingMessage { get; set; }
        public bool HasPostedImage { get; set; }
        public bool IsWritePostEnabled { get; set; }
        public bool IsShowPostOptions { get; set; }
        public bool DoUpdate { get; set; }
        public bool HasIncomingLike { get; set; }
        public int LatestPostUpdatedPostFeedId { get; set; }

        public PostFeedDetailPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            IPostFeedManager postFeedManager,
            INavigationService navigationService,
            INavigationStackService navigationStackService,
            IEventAggregator eventAggregator,
            PostFeedDetailsPageValidator validator) : base(navigationService)
        {
            _postFeedManager = postFeedManager;
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _eventAggregator = eventAggregator;
            _validator = validator;
            _keyboardHelper = AppUnityContainer.InstanceDependencyService.Get<IKeyboardHelper>();
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
            Title = AppStrings.TitleComments;
        }

        public override void PreparingPageBindings()
        {
            _eventAggregator.GetEvent<LikeOrUnLikeAPostFeedSubsEventModel>().Subscribe((message) =>
            {
                try
                {
                    if (!IsBusy)
                    {
                        HasIncomingLike = true;
                        var currentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<PostFeed>(message.CurrentPost);
                        var posterUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contact>(message.CurrentUser);

                        SelectedComment = currentPost;

                        if (ReadPostFeedFromLocal(currentPost) != null)
                            AddDeductOneLikeToThisPostFromLocal(currentPost, posterUser);
                    }
                }
                catch (Exception ex)
                {
                    ProcessErrorReportingForRaygun(ex);
                }
            });

            _eventAggregator.GetEvent<AddUpdatePostFeedToHubResultCodeEventModel>().Subscribe((message) =>
            {
                IsBusy = false;

                if (message.HttpStatusCode == HttpStatusCode.OK)
                {
                    IsWritePostEnabled = true;
                    var newlyAddedComment = ServiceMapper.Instance.Map<PostFeed>(message.Result);
                    LatestPostUpdatedPostFeedId = newlyAddedComment.PostFeedID;
                    _postFeedManager.SaveNewPostToLocal(newlyAddedComment);
                    CurrentPostFeed.Comments.Add(newlyAddedComment);
                    CurrentPostFeed.NoOfComments = CurrentPostFeed.Comments.Count;
                    RaisePropertyChanged(nameof(CurrentPostFeed));
                }
                else
                    UserDialogs.Instance.Alert(AppStrings.LoadingErrorPostFeed, "Error", "Ok");
            });

            _eventAggregator.GetEvent<DeletePostFeedToHubResultCodeEventModel>().Subscribe((message) =>
            {
                DeletingMessage = "";
                IsShowPostOptions = false;

                if (message.HttpStatusCode != HttpStatusCode.OK)
                    UserDialogs.Instance.Alert(AppStrings.DeletingPostNotSuccessful, "Error", "Ok");
            });

            if (PassingParameters != null && PassingParameters.ContainsKey("SelectedPost"))
            {
                CurrentPostFeed = (PostFeed)PassingParameters["SelectedPost"];
                if (!string.IsNullOrEmpty(CurrentPostFeed.ContentURL))
                    HasPostedImage = true;

                CurrentContact = (Contact)PassingParameters["CurrentUser"];
            }

            if(PassingParameters != null && PassingParameters.ContainsKey("SupportersAvatars"))
                SupportersAvatars = PassingParameters["SupportersAvatars"] as List<string>;

            if (IsInternetConnected)
                _eventAggregator.GetEvent<LogonPostFeedToHubEventModel>().Publish(new PostFeedMessage { CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(CurrentContact) });

            IsWritePostEnabled = true;
            IsBusy = false;
            DeletingMessage = "";
        }

        public void SendErrorToHockeyApp(Exception ex)
        {
            ProcessErrorReportingForRaygun(ex);
        }

        public void AddDeductOneLikeToThisPostFromLocal(PostFeed postFeed, Contact userWhoLiked)
        {
            var postToLike = _postFeedManager.GetPostFeed(postFeed.PostFeedID);

            if (userWhoLiked.RemoteId == CurrentContact.RemoteId)
                postToLike.IsSelfSupported = !postToLike.IsSelfSupported;

            var updatePostFeed = _postFeedManager.UpdatePostFeedAndPostFeedLikeToLocal(postToLike, userWhoLiked);

            if (postFeed.PostFeedLevel == 1)
            {
                SelectedComment = updatePostFeed;
                SelectedComment.NoOfSupports = updatePostFeed.NoOfSupports;
            }
            else
                CurrentPostFeed = updatePostFeed;
        }

        public Entity.PostFeed ReadPostFeedFromLocal(PostFeed newPost) => _postFeedManager.GetPostFeed(newPost.PostFeedID);

        public void DeletePostFeedFromLocal(PostFeed newPost, bool getLocalFirst = false)
        {
            if (getLocalFirst)
            {
                var foundLocalComment = _postFeedManager.GetPostFeed(newPost.PostFeedID);
                _postFeedManager.DeletePostInLocal(foundLocalComment);
                return;
            }

            _postFeedManager.DeletePostInLocal(newPost);
        }

        public void SaveCommentToLocal(PostFeed newComment)
        {
            _postFeedManager.SaveNewPostToLocal(newComment);
            CurrentPostFeed.Comments.Add(newComment);
            CurrentPostFeed.NoOfComments = CurrentPostFeed.Comments.Count;
            RaisePropertyChanged(nameof(CurrentPostFeed));
        }

        public void WriteComment()
        {
            if (!ProcessValidationErrors(_validator.Validate(this), false))
                return;

            if (ProcessInternetConnection(true))
            {
                if (DoUpdate)
                {
                    Comment.ContentText = CommentText;
                    Comment.DateModified = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString(Constants.DateTimeFormat);
                    RaisePropertyChanged(nameof(Comment));
                }
                else
                {
                    Comment = new PostFeed
                    {
                        Title = $"A new post from @{CurrentContact.FirstName} {CurrentContact.LastName}",
                        ContentText = CommentText ?? "",
                        ContentURL = PostImageUrl ?? "",
                        Poster = CurrentContact,
                        PosterEmail = CurrentContact.EmailAdd,
                        PosterId = CurrentContact.RemoteId,
                        PosterFirstName = CurrentContact.FirstName,
                        PosterLastName = CurrentContact.LastName,
                        PosterProfilePhotoFB = CurrentContact.FBLink,
                        PosterProfilePhoto = CurrentContact.PhotoURL,
                        DatePosted = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString(Constants.DateTimeFormat),
                        DateModified = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString(Constants.DateTimeFormat),
                        SupportersIdsList = new List<int>(),
                        NoOfSupports = 0,
                        PostFeedLevel = 1,
                        PostFeedParentId = CurrentPostFeed.PostFeedID,
                        AliasName = CurrentContact.AliasName,
                        IsSelfPosted = true
                    };
                }
                
                IsWritePostEnabled = false;
                SendAddEditToBackground(Comment, CurrentContact);   // Updating the UI before sending updated comment to background

                _eventAggregator.GetEvent<UpdateCommentListEventModel>().Publish(Comment);

                _keyboardHelper.HideKeyboard();
                CommentText = string.Empty;
            }
        }

        private void SendAddEditToBackground(Entity.PostFeed postFeed, Entity.Contact contact)
        {
            PostFeedMessage postFeedMessage = new PostFeedMessage
            {
                CurrentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.PostFeedK>(postFeed),
                CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(contact)
            };
            _eventAggregator.GetEvent<AddUpdatePostFeedToHubEventModel>().Publish(postFeedMessage);

            //chito. added this for FAKE only because navigating to PostFeedPage after editing a post is called only after subscribing to messaging center coming from Native.
            AddUpdatePostFeedToHubFake();
        }

        [Conditional("FAKE")]
        private void AddUpdatePostFeedToHubFake()
        {
            IsBusy = false;
            IsWritePostEnabled = true;
            var newlyAddedComment = Comment;
            _postFeedManager.SaveNewPostToLocal(newlyAddedComment);
        }

        private async void GoBack()
        {
            _keyValueCacheUtility.GetUserDefaultsKeyValue("IsForceToGetToLocal", "true");
            _keyboardHelper.HideKeyboard();
            await NavigateBackHelper(PassingParameters);
        }

        private void SupportPost()
        {
            var updatedSelectedPost = _postFeedManager.GetPostFeed(CurrentPostFeed.PostFeedID);
            
            if (ProcessInternetConnection(true))
            {
                PostFeedMessage postFeedMessage = new PostFeedMessage
                {
                    CurrentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.PostFeedK>(updatedSelectedPost),
                    CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(CurrentContact)
                };
                _eventAggregator.GetEvent<LikeOrUnlikePostFeedToHubEventModel>().Publish(postFeedMessage);
                CurrentPostFeed.IsSelfSupported = !CurrentPostFeed.IsSelfSupported;
                RaisePropertyChanged(nameof(CurrentPostFeed.IsSelfSupported));
                var updatePostFeed = _postFeedManager.UpdatePostFeedAndPostFeedLikeToLocal(updatedSelectedPost, CurrentContact);

                if (CurrentPostFeed.Equals(updatedSelectedPost))
                    CurrentPostFeed = updatePostFeed;
                else
                    SelectedComment = updatePostFeed;               
            }
        }

        private async Task TakeCamera()
        {
            //chito. when uploading a photo, compressed it so it becomes only 300KB or lower.
            await UserDialogs.Instance.AlertAsync("Attaching a photo is not supported by this version of the app. Please wait for our next version.", "Not Supported");
        }

        private void ShowPostOptions(Entity.PostFeed currentPostFeed)
        {
            IsShowPostOptions = true;
            Comment = currentPostFeed;
        }

        private void ClosePostOptions() => IsShowPostOptions = false;

        private void DeleteSelfComment()
        {
            if (!ProcessInternetConnection(true))
                return;
            
            PostFeedMessage postFeedMessage = new PostFeedMessage
            {
                CurrentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.PostFeedK>(Comment),
                CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(CurrentContact)
            };

            DeletingMessage = AppStrings.DeletingComment;
            _eventAggregator.GetEvent<DeletePostFeedToHubEventModel>().Publish(postFeedMessage);
            DeletePostFeedFromLocal(Comment);
        }

        private void EditSelfComment()
        {
            CommentText = Comment.ContentText;
            IsShowPostOptions = false;
            IsWritePostEnabled = true;
            DoUpdate = true;
        }
    }
}

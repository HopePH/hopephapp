using Acr.UserDialogs;
using FluentValidation;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.Net;
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
    public class PostFeedAddEditPageViewModel : ViewModelBase
    {
        private readonly IValidator _validator;
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;
        private readonly IPostFeedManager _postFeedManager;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;

        public ICommand SaveOrEditPostCommand => new DelegateCommand(SaveOrEditPost);
        public ICommand NavigateBackCommand => new DelegateCommand(GoBack);
        public Contact CurrentContact { get; set; }
        public PostFeed NewPost { get; set; }
        public PostFeed OldPost { get; set; }
        public string Content { get; set; }
        public string PostImage { get; set; }
        public string ButtonText { get; set; }

        public PostFeedAddEditPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationService navigationService,
            INavigationStackService navigationStackService,
            IPostFeedManager postFeedManager,
            PostFeedAddEditPageValidators validator) : base(navigationService)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _postFeedManager = postFeedManager;
            _validator = validator;
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
            Title = AppStrings.TitleFeelings;
        }

        public override void PreparingPageBindings()
        {
            if(PassingParameters != null && PassingParameters.ContainsKey("CurrentContact"))
                CurrentContact = (Contact)PassingParameters["CurrentContact"];

            if (PassingParameters != null && PassingParameters.ContainsKey("SelectedPost"))
            {
                OldPost = (PostFeed)PassingParameters["SelectedPost"];
                Content = OldPost.ContentText;
                ButtonText = AppStrings.UpdateText;
            }
            else
            {
                Content = "";
                ButtonText = AppStrings.PostText;
            }

            if (IsInternetConnected)
                MessagingCenter.Send(new PostFeedMessage { CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(CurrentContact) }, "LogonPostFeedToHub");
            
            IsBusy = false;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<HttpResponseMessage<Contract.PostFeedK>>(this, "AddUpdatePostFeedToHubResultCode", message =>
            {
                IsBusy = false;

                if (message.HttpStatusCode == HttpStatusCode.OK)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _keyValueCacheUtility.GetUserDefaultsKeyValue("IsForceToGetFromRest", "true");
                        _keyValueCacheUtility.GetUserDefaultsKeyValue("SecondsDelay", "2");
                        NavigateBackHelper(PassingParameters);
                    });
                }
                else
                    UserDialogs.Instance.Alert(AppStrings.LoadingErrorPostFeed, "Error", "Ok");
            });
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<HttpResponseMessage<Contract.PostFeedK>>(this, "AddUpdatePostFeedToHubResultCode");
        }

        private void SaveOrEditPost()
        {
            if (ButtonText == AppStrings.PostText)
                SavePost();
            else
                EditPost();
        }

        private void EditPost()
        {
            if (!ProcessInternetConnection(true))
                return;

            if (ProcessValidationErrors(_validator.Validate(this), true))
            {
                IsBusy = true;
                NewPost = new PostFeed
                {
                    Id = OldPost.Id,
                    PostFeedID = OldPost.PostFeedID,
                    ContentText = Content,
                    ContentURL = PostImage ?? (OldPost.ContentURL ?? ""),
                    Poster = CurrentContact,
                    PosterEmail = CurrentContact.EmailAdd,
                    PosterId = CurrentContact.RemoteId,
                    PosterFirstName = CurrentContact.FirstName,
                    PosterLastName = CurrentContact.LastName,
                    PosterProfilePhotoFB = CurrentContact.FBLink,
                    PosterProfilePhoto = CurrentContact.PhotoURL,
                    DatePosted = OldPost.DatePosted,
                    DateModified = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString(Constants.DateTimeFormat),
                    Title = $"New Post from @{CurrentContact.AliasName}",
                    SupportersIdsList = OldPost.SupportersIdsList,
                    NoOfSupports = OldPost.NoOfSupports,
                    Comments = OldPost.Comments,
                    NoOfComments = OldPost.NoOfComments,
                    PostFeedLevel = 0,
                    PostFeedParentId = 0
                };
                SendAddEditToBackground(NewPost, CurrentContact);
            }
        }

        private void SavePost()
        {
            if (!ProcessInternetConnection(true))
                return;

            if (ProcessValidationErrors(_validator.Validate(this), true))
            {
                IsBusy = true;
                NewPost = new PostFeed
                {
                    ContentText = Content ?? "",
                    ContentURL = PostImage ?? "",
                    Poster = CurrentContact,
                    PosterEmail = CurrentContact.EmailAdd,
                    PosterId = CurrentContact.RemoteId,
                    PosterFirstName = CurrentContact.FirstName,
                    PosterLastName = CurrentContact.LastName,
                    PosterProfilePhotoFB = CurrentContact.FBLink,
                    PosterProfilePhoto = CurrentContact.PhotoURL,
                    DatePosted = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString(Constants.DateTimeFormat),
                    DateModified = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString(Constants.DateTimeFormat),
                    Title = $"New Post from @{CurrentContact.AliasName}",
                    SupportersIdsList = new System.Collections.Generic.List<int>(),
                    NoOfSupports = 0,
                    Comments = new System.Collections.ObjectModel.ObservableCollection<PostFeed>(),
                    NoOfComments = 0,
                    PostFeedLevel = 0,
                    PostFeedParentId = 0,
                    Id = 0
                };
                SendAddEditToBackground(NewPost, CurrentContact);
            }
        }
        
        private void SendAddEditToBackground(Entity.PostFeed postFeed, Entity.Contact contact)
        {
            PostFeedMessage postFeedMessage = new PostFeedMessage
            {
                CurrentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.PostFeedK>(postFeed),
                CurrentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Contract.ContactK>(contact)
            };
            MessagingCenter.Send(postFeedMessage, "AddUpdatePostFeedToHub");

            // 01-12-2018 12:06pm REYNZ: 
            // added this for FAKE only because navigating to PostFeedPage after editing 
            // a post is called only after subscribing to messaging center coming from
            // Native.
            AddUpdatePostFeedToHubFake();   
        }

        [Conditional("FAKE")]
        private void AddUpdatePostFeedToHubFake()
        {
            if (ButtonText == AppStrings.PostText)
                FakeData.FakePostFeeds.AddingNewPostFeedContent(NewPost);
            else
                FakeData.FakePostFeeds.EditingPostFeedContent(NewPost.PostFeedID, Content);

            IsBusy = false;
            PassingParameters.Add("IsForceToGetFromRest", true);
            NavigateBackHelper(PassingParameters);
        }

        private void GoBack() => NavigateBackHelper();
    }
}

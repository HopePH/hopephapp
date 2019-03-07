using System;
using System.Diagnostics;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Messages;
using Yol.Punla.ViewModels;

namespace Yol.Punla.Views
{
    [ModuleView]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostFeedPage : AppViewBase
    {
        private  PostFeedPageViewModel viewModel;

        public PostFeedPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (XamlParseException xp)
            {
                if (!xp.Message.Contains("StaticResource not found for key"))
                    throw;
            }
            catch (Exception ex)
            {
                if (!(ex.Source == "FFImageLoading.Forms" || ex.Source == "FFImageLoading.Transformations"))
                    throw;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if(BindingContext != null)
                viewModel = BindingContext as PostFeedPageViewModel;
        }

        protected override void SubcribeMessagingCenter()
        {
            //chito. if the current user is not the one who post feed then run this. research locking
            //the messenger subscribe works best onappearing method as constructor is never always called in prism scenario
            base.SubcribeMessagingCenter();            
            MessagingCenter.Subscribe<PostFeedMessage>(this, "AddUpdatePostSubs", message =>
            {
                try
                {
                    if (!viewModel.IsBusy)
                    {
                        var currentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Entity.PostFeed>(message.CurrentPost);
                        var currentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Entity.Contact>(message.CurrentUser);
                        currentPost.PosterId = message.CurrentUser.Id;

                        if (currentPost.PostFeedLevel == 0)
                            UpdateAddPost(currentUser, currentPost);
                    }
                }
                catch (Exception ex)
                {
                    viewModel.SendErrorToHockeyApp(ex);
                }
            });

            MessagingCenter.Subscribe<PostFeedMessage>(this, "DeletePostFeedSubs", message =>
            {
                try
                {
                    if (!viewModel.IsBusy)
                    {
                        var currentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Entity.PostFeed>(message.CurrentPost);
                        var currentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Entity.Contact>(message.CurrentUser);
                        currentPost.PosterId = message.CurrentUser.Id;

                        if (ReadIfExistingPost(currentPost))
                            DeletePost(currentUser, currentPost);
                    }
                }
                catch (Exception ex)
                {
                    viewModel.SendErrorToHockeyApp(ex);
                }
            });

            MessagingCenter.Subscribe<PostFeedMessage>(this, "LikeOrUnLikeAPostFeedSubs", message =>
            {
                try
                {
                    if (!viewModel.IsBusy)
                    {
                        var currentPost = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Entity.PostFeed>(message.CurrentPost);
                        var posterUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Entity.Contact>(message.CurrentUser);

                        if (ReadIfExistingPost(currentPost))
                            viewModel.AddOneLikeToThisPostFromLocal(currentPost, posterUser);
                    }
                }
                catch (Exception ex)
                {
                    viewModel.SendErrorToHockeyApp(ex);
                }
            });
        }

        protected override void UnSubcribeMessagingCenter()
        {
            base.UnSubcribeMessagingCenter();
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "AddUpdatePostSubs");
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "DeletePostFeedSubs");
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "LikeOrUnLikeAPostFeedSubs");
        }

        private void UpdateAddPost(Entity.Contact posterContact, Entity.PostFeed currentPost)
        {
            currentPost.Poster = posterContact;
            currentPost.PosterProfilePhotoFB = currentPost.Poster.FBLink;
            viewModel.SavePostFeedToLocal(currentPost);
        }
        
        private void DeletePost(Entity.Contact posterContact, Entity.PostFeed currentPost)
        {
            currentPost.Poster = posterContact;
            viewModel.DeletePostFeedFromLocal(currentPost);
        }

        private bool ReadIfExistingPost(Entity.PostFeed currentPost)
        {
            if (viewModel.ReadPostFeedFromLocal(currentPost) == null)
                return false;

            return true;
        }
    }
}
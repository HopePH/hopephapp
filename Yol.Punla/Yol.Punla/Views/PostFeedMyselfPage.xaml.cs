using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Mapper;
using Yol.Punla.Messages;
using Yol.Punla.ViewModels;
using Unity;

namespace Yol.Punla.Views
{
    [ModuleView]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostFeedMyselfPage : AppViewBase
    {
        private PostFeedMyselfPageViewModel viewModel;

        public PostFeedMyselfPage()
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

            if (BindingContext != null)
                viewModel = BindingContext as PostFeedMyselfPageViewModel;
        }

        protected override void AttachedPageEvents()
        {
            base.AttachedPageEvents();

            try
            {
                PostsList.ItemSelected += PostsList_ItemSelected;
            }
            catch (Exception ex)
            {

            }
        }

        protected override void DetachedPageEvents()
        {
            base.DetachedPageEvents();

            try
            {
                PostsList.ItemSelected -= PostsList_ItemSelected;
            }
            catch (Exception ex)
            {

            }
        }

        protected override void SubcribeMessagingCenter()
        {   
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
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "LikeOrUnLikeAPostFeedSubs");
        }

        private bool ReadIfExistingPost(Entity.PostFeed currentPost)
        {
            if (viewModel.ReadPostFeedFromLocal(currentPost) == null)
                return false;

            return true;
        }

        private void PostsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            viewModel.CommentCommand.Execute(e.SelectedItem);
        }
    }
}
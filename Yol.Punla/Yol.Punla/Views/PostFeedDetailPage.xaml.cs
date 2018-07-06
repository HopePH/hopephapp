using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Mapper;
using Yol.Punla.Messages;
using Yol.Punla.Templates;
using Yol.Punla.ViewModels;

namespace Yol.Punla.Views
{
    [ModuleView]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostFeedDetailPage : AppViewBase
    {
        private PostFeedDetailPageViewModel viewModel;
        private List<CommentsIndexValue> commentsIndexValueList = null;

        public PostFeedDetailPage()
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetupComments();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                viewModel = BindingContext as PostFeedDetailPageViewModel;
                viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        protected override void AttachedPageEvents()
        {
            base.AttachedPageEvents();

            try
            {
                stackCommentGesture.Tapped += StackCommentGesture_Tapped;
                deleteCommentOption.Tapped += DeleteCommentOption_Tapped;
                editPostStackLayout.Tapped += EditPostStackLayout_Tapped;
                multiEntry.Focused += MultiEntry_Focused;
                multiEntry.Unfocused += MultiEntry_Unfocused;
                hidingStack.PropertyChanged += HidingStack_PropertyChanged;

                // HACK (REYNZ): Added this for Post button in iOS to works
                if (Device.RuntimePlatform == Device.iOS)
                    btnPost.Pressed += BtnPost_Pressed;
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    throw;
            }
        }

        protected override void DetachedPageEvents()
        {
            base.DetachedPageEvents();
            stackCommentGesture.Tapped -= StackCommentGesture_Tapped;
            deleteCommentOption.Tapped -= DeleteCommentOption_Tapped;
            editPostStackLayout.Tapped -= EditPostStackLayout_Tapped;
            multiEntry.Focused -= MultiEntry_Focused;
            multiEntry.Unfocused -= MultiEntry_Unfocused;
            hidingStack.PropertyChanged -= HidingStack_PropertyChanged;

            if (Device.RuntimePlatform == Device.iOS)
                btnPost.Pressed -= BtnPost_Pressed;
        }

        protected override void SubcribeMessagingCenter()
        {
            base.SubcribeMessagingCenter();
            
            MessagingCenter.Subscribe<PostFeedMessage>(this, "DeletePostFeedSubs", message =>
            {
                try
                {
                    if (!viewModel.IsBusy)
                    {
                        var currentComment = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Entity.PostFeed>(message.CurrentPost);
                        var currentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Entity.Contact>(message.CurrentUser);
                        currentComment.PosterId = message.CurrentUser.Id;
                        viewModel.DeletePostFeedFromLocal(currentComment, true);
                    }
                }
                catch (Exception ex)
                {
                    viewModel.SendErrorToHockeyApp(ex);
                }
            });

            MessagingCenter.Subscribe<PostFeedMessage>(this, "AddUpdatePostSubs", message =>
            {
                try
                {
                    if (!viewModel.IsBusy)
                    {
                        var addedComment = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Entity.PostFeed>(message.CurrentPost);
                        var currentUser = AppUnityContainer.Instance.Resolve<IServiceMapper>().Instance.Map<Entity.Contact>(message.CurrentUser);
                        addedComment.PosterId = message.CurrentUser.Id;

                        if (addedComment.PostFeedLevel == 1)
                        {
                            addedComment.Poster = currentUser;
                            addedComment.PosterProfilePhotoFB = addedComment.Poster.FBLink;

                            AddCommentToView(addedComment);
                            UpdateAddComment(addedComment);
                        }
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
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "DeletePostFeedSubs");
            MessagingCenter.Unsubscribe<PostFeedMessage>(this, "AddUpdatePostSubs");
        }

        private void SetupComments()
        {
            try
            {
                var comments = viewModel.CurrentPostFeed.Comments;

                if (comments != null && comments.Count > 0)
                {
                    var commentItemsControl = ScrollViewElement.FindByName<StackLayout>("CommentItems");
                    commentsIndexValueList = new List<CommentsIndexValue>();
                    int index = 0;
                    CommentItemTemplate widget;

                    foreach (var comment in comments)
                    {
                        commentsIndexValueList.Add(new CommentsIndexValue { Comment = comment, Index = index , IsLocallySaved = true});
                        ++index;

                        widget = new CommentItemTemplate { BindingContext = comment };

                        commentItemsControl.Children.Add(widget);

                        var supportGesture = widget.FindByName<TapGestureRecognizer>("commentItemSupportGesture");
                        supportGesture.Command = viewModel.SupportCommand;
                        supportGesture.CommandParameter = comment;
                        supportGesture.Tapped += SupportGesture_Tapped;

                        var moreOptionsGesture = widget.FindByName<TapGestureRecognizer>("moreOptionsSupportGesture");
                        moreOptionsGesture.Command = viewModel.ShowPostOptionsCommand;
                        moreOptionsGesture.CommandParameter = comment;
                    }
                }
            }
            catch (Exception ex)
            {
                viewModel.SendErrorToHockeyApp(ex);
            }
        }

        private void MultiEntry_Unfocused(object sender, FocusEventArgs e)
        {
            multiEntry.HasFocus = e.IsFocused;
            mainGrid.RowDefinitions[2].Height = 90;
        }

        private void MultiEntry_Focused(object sender, FocusEventArgs e)
        {
            multiEntry.HasFocus = e.IsFocused;
            mainGrid.RowDefinitions[2].Height = 140;
        }

        private void HidingStack_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsVisible")
            {
                if (multiEntry.HasFocus)
                {
                    switch (Device.RuntimePlatform)
                    {
                        case Device.Android:
                            multiEntry.Focus();
                            break;
                        case Device.iOS:
                            mainGrid.RowDefinitions[2].Height = 410;
                            multiEntry.Focus(); // HACK: Forces the multiEntry to receive focus in iOS       
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    mainGrid.RowDefinitions[2].Height = 80;
                }
            }
        }
        
        private void BtnPost_Pressed(object sender, EventArgs e) => viewModel.WriteComment();
        
        private void EditPostStackLayout_Tapped(object sender, EventArgs e) => multiEntry.HasFocus = true;

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Comment":
                    CommentPropertyChanged();
                    break;
                case "CurrentPostFeed":
                    CurrentPostFeedPropertyChanged();
                    break;
                case "SelectedComment":
                    SelectedCommentPropertyChanged();
                   break;
                default:
                    break;
            }
        }

        private void CommentPropertyChanged()
        {
            try
            {
                if (viewModel.Comment != null)
                {
                    if (!viewModel.DoUpdate && !viewModel.IsShowPostOptions)
                    {
                        var comment = viewModel.Comment;
                        AddCommentToView(comment);
                    }
                    else if (viewModel.DoUpdate && !viewModel.IsShowPostOptions)
                    {
                        var commentItemsControl = ScrollViewElement.FindByName<StackLayout>("CommentItems");
                        var commentFound = commentsIndexValueList.Where(x => x.Comment.PostFeedID == viewModel.Comment.PostFeedID).FirstOrDefault();
                        if (commentFound != null)
                            commentFound.Comment = viewModel.Comment;

                        commentItemsControl.Children[commentFound.Index].FindByName<Label>("CommentLabel").Text = commentFound.Comment.ContentText;
                        viewModel.DoUpdate = false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(ex.Source == "Xamarin.Forms.Core"))
                    throw;
            }
        }

        private void CurrentPostFeedPropertyChanged()
        {
            try
            {
                if (viewModel.LatestPostUpdatedPostFeedId > 0)
                {
                    commentsIndexValueList.ToList().Last().Comment.PostFeedID = viewModel.LatestPostUpdatedPostFeedId;
                    commentsIndexValueList.ToList().Last().Comment = viewModel.CurrentPostFeed.Comments.Last();
                    viewModel.LatestPostUpdatedPostFeedId = 0;

                    var commentStack = ScrollViewElement.FindByName<StackLayout>("CommentItems");

                    var ellipsisContainer = commentStack.Children[commentsIndexValueList.ToList().Last().Index].FindByName<ContentView>("EllipsisContainer");
                    var ellipsis = ellipsisContainer.FindByName<Label>("Ellipsis");

                    var commentLabel = commentStack.Children[commentsIndexValueList.ToList().Last().Index].FindByName<Label>("CommentLabel");
                    commentsIndexValueList.ToList().Last().IsLocallySaved = true;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var grid = (CommentItemTemplate)ellipsisContainer.Parent;
                        grid.IsEnabled = true;
                        grid.Opacity = 1;
                        ellipsis.IsVisible = true;
                    });
                }

                var mainPostLikeCount = ScrollViewElement.FindByName<Label>("MainPostLikeCounter");
                mainPostLikeCount.Text = viewModel.CurrentPostFeed.NoOfSupports.ToString() + " Supports";
            }
            catch (Exception ex)
            {
                if (!(ex.Source == "Xamarin.Forms.Core"))
                    throw;
            }
        }

        private void SelectedCommentPropertyChanged()
        {
            //chito. if the like is the parent post feed inside the details page, the no of supports doesnt work
            var commentItemsControl3 = ScrollViewElement.FindByName<StackLayout>("CommentItems");

            if (commentsIndexValueList != null)
            {
                var commentFound3 = commentsIndexValueList.Where(x => x.Comment.PostFeedID == viewModel.SelectedComment.PostFeedID).FirstOrDefault();

                if (commentFound3 != null)
                {
                    commentFound3.Comment = viewModel.SelectedComment;
                    commentItemsControl3.BindingContext = viewModel.SelectedComment;
                    commentItemsControl3.Children[commentFound3.Index].FindByName<Label>("supportCount").Text = viewModel.SelectedComment.NoOfSupports.ToString() + " Supports";
                }
            }
        }

        private void StackCommentGesture_Tapped(object sender, EventArgs e)
        {
            // Check first if DoUpdate flag is true. If so set it to false
            if (viewModel.DoUpdate)
                viewModel.DoUpdate = false;

            multiEntry.MultiText = "";
            multiEntry.HasFocus = true;

            if (Device.RuntimePlatform == Device.Android)
                multiEntry.Focus();
        }

        private void SupportGesture_Tapped(object sender, EventArgs e)
        {
            try
            {
                var updateStack = (StackLayout)sender;
                var labelHeart = updateStack.FindByName<Label>("labelHeart");
                var currentColor = labelHeart.TextColor.ToString();

                updateStack.FindByName<Label>("supportCount").Text = viewModel.SelectedComment.NoOfSupports.ToString() + " Supports";

                if (currentColor.Contains("0.690196096897125")) // which means green dim
                {
                    labelHeart.Text = (string)Application.Current.Resources["fa-heart-o"];
                    labelHeart.TextColor = Color.FromHex("#c7c7c7");
                }
                else
                {
                    labelHeart.Text = (string)Application.Current.Resources["fa-heart"];
                    labelHeart.TextColor = (Color)Application.Current.Resources["GreenColorDim"];
                }
            }
            catch (Exception ex)
            {
                viewModel.SendErrorToHockeyApp(ex);
            }
        }

        private void DeleteCommentOption_Tapped(object sender, EventArgs e)
        {
            try
            {
                var commentItemsControl = ScrollViewElement.FindByName<StackLayout>("CommentItems");
                var commentFound = commentsIndexValueList.Where(x => x.Comment.PostFeedID == viewModel.Comment.PostFeedID).FirstOrDefault();

                var childrenStack = commentItemsControl.Children;
                commentItemsControl.Children.Remove(childrenStack[commentFound.Index]);
                commentsIndexValueList.Remove(commentFound);
                
                var mainPostCommentCount = ScrollViewElement.FindByName<Label>("parentCommentCount");
                mainPostCommentCount.Text = childrenStack.Count + " Comments";
            }
            catch (Exception ex)
            {
                viewModel.SendErrorToHockeyApp(ex);
            }
        }

        private bool ReadIfExistingPost(Entity.PostFeed currentPost)
        {
            if (viewModel.ReadPostFeedFromLocal(currentPost) == null)
                return false;

            return true;
        }

        private void AddCommentToView(Entity.PostFeed currentComment)
        {
            try
            {
                var commentItemsControl = ScrollViewElement.FindByName<StackLayout>("CommentItems");
                commentItemsControl.Children.Add(new CommentItemTemplate { BindingContext = currentComment });

                var moreOptionsGesture = commentItemsControl.Children[commentItemsControl.Children.Count - 1].FindByName<TapGestureRecognizer>("moreOptionsSupportGesture");
                moreOptionsGesture.Command = viewModel.ShowPostOptionsCommand;
                moreOptionsGesture.CommandParameter = currentComment;

                var supportgesture = commentItemsControl.Children[commentItemsControl.Children.Count - 1].FindByName<TapGestureRecognizer>("commentItemSupportGesture");
                supportgesture.Command = viewModel.SupportCommand;
                supportgesture.CommandParameter = currentComment;
                supportgesture.Tapped += SupportGesture_Tapped;

                if (commentsIndexValueList == null) commentsIndexValueList = new List<CommentsIndexValue>();
                commentsIndexValueList.Add(new CommentsIndexValue { Comment = currentComment, Index = commentsIndexValueList.Count, IsLocallySaved = false });
                
                var ellipsisContainer = commentItemsControl.Children.Last().FindByName<ContentView>("EllipsisContainer");
                var ellipsisiCon = ellipsisContainer.FindByName<Label>("Ellipsis");
                ellipsisiCon.IsVisible = false;

                if (currentComment.IsSelfPosted)
                {
                    var grid = (CommentItemTemplate)ellipsisContainer.Parent;
                    grid.IsEnabled = false;
                    grid.Opacity = 0.25;
                }
            }
            catch (Exception ex)
            {
                viewModel.SendErrorToHockeyApp(ex);
            }
        }

        private void UpdateAddComment(Entity.PostFeed currentPost) => viewModel.SaveCommentToLocal(currentPost);        
    }
}

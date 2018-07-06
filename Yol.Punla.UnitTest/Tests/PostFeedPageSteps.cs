using Unity;
using Should;
using System.Linq;
using TechTalk.SpecFlow;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.ViewModels;
using Prism.Unity;
using Yol.Punla.FakeEntries;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public sealed class PostFeedPageSteps : StepBase
    {
        public PostFeedPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [When(@"I tap the Write Down icon from the menu detail")]
        public void WhenITapTheWriteDownIconFromTheMenuDetail() 
            => Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().WriteDownCommand.Execute(null);

        [Then(@"I should see a Post posted by ""(.*)"", with title ""(.*)"", and content ""(.*)""")]
        public void ThenIShouldSeeAPostPostedByWithTitleAndContent(string postedBy, string postTitle, string content)
        {
            var post = Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().PostsList.Where(p => p.Title == postTitle).FirstOrDefault();
            post.ShouldNotBeNull();
            post.Title.ShouldEqual(postTitle);
            post.ContentText.ShouldEqual(content);
            postedBy.ShouldEqual((post.PosterFullName));
        }

        [When(@"I tap the comment icon of the post of ""(.*)"" with content ""(.*)"" and postFeedId ""(.*)""")]
        public void WhenITapTheCommentIconOfThePostOfWithContentAndPostFeedId(string authorName, string content, int postId)
        {
            var stringArray = authorName.Split(' ');

            var postFeed = new Entity.PostFeed
            {
                PostFeedID = postId,
                PosterFirstName = stringArray[0],
                PosterLastName = stringArray[1],
                ContentText = content
            };

            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().CommentCommand.Execute(postFeed);
        }

        [Then(@"I can see the details of the post with author ""(.*)"" and content ""(.*)""")]
        public void ThenICanSeeTheDetailsOfThePostWithAuthorAndContent(string authorName, string content)
        {
            Main.App.Container.GetContainer().Resolve<PostFeedDetailPageViewModel>().CurrentPostFeed.PosterFullName.ShouldEqual(authorName);
            Main.App.Container.GetContainer().Resolve<PostFeedDetailPageViewModel>().CurrentPostFeed.ContentText.ShouldEqual(content);
        }

        [When(@"I tap the ellipsis icon to show the post option modal at the left of the post of ""(.*)"" with postFeedId ""(.*)""")]
        public void WhenITapTheEllipsisIconToShowThePostOptionModalAtTheLeftOfThePostOfWithPostFeedId(string author, int postFeedId)
        {
            var post = Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().PostsList.Where(p => p.PostFeedID == postFeedId).FirstOrDefault();
            post.IsSelfPosted = true;
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().ShowPostOptionsCommand.Execute(post);
        }

        [Then(@"the PostOptions modal will appear")]
        public void ThenThePostOptionsModalWillAppear() => Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().IsShowPostOptions.ShouldBeTrue();

        [When(@"I tap the close icon of the PostOptions Modal")]
        public void WhenITapTheCloseIconOfThePostOptionsModal() => Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().ClosePostOptionsCommand.Execute(null);

        [Then(@"I should see the PostOptions modal disappear")]
        public void ThenIShouldSeeTheFilterModalDisappear() => Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().IsShowPostOptions.ShouldBeFalse();

        [When(@"I tap the Edit menu")]
        public void WhenITapTheEditMenu() => Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().EditPostCommand.Execute(null);

        [When(@"I type edit the message to ""(.*)"" and tap the update button")]
        public void WhenITypeEditTheMessageToAndTapTheUpdateButton(string editedContent)
        {
            Main.App.Container.GetContainer().Resolve<PostFeedAddEditPageViewModel>().Content = editedContent;
            Main.App.Container.GetContainer().Resolve<PostFeedAddEditPageViewModel>().SaveOrEditPostCommand.Execute(null);
        }

        [Then(@"I should see my updated post with content ""(.*)"", and author ""(.*)"" and post id ""(.*)""")]
        public void ThenIShouldSeeMyUpdatedPostWithContentAndAuthorAndPostId(string updatedContent, string author, int postId)
        {
            var updatedPost = FakeData.FakePostFeeds.Posts.Where(p => p.PostFeedID == postId).FirstOrDefault();
            updatedPost.ShouldNotBeNull();
            updatedPost.PosterFullName.ShouldEqual(author);
            updatedPost.ContentText.ShouldEqual(updatedContent);
        }

        [When(@"I tap the Delete menu")]
        public void WhenITapTheDeleteMenu() => Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().DeletePostCommand.Execute(null);

        [Then(@"The delete post feed request should be sent to the hub")]
        public void ThenTheDeletePostFeedRequestShouldBeSentToTheHub() 
            => Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().IsDeletePostFeedRequestSentToHub.ShouldBeTrue();
        
        //chito. from postfeedadd feature. just merge with the postfeed feature instead
        [When(@"I tap the text Share an article, photo, video or idea")]
        public void WhenITapTheTextShareAnArticlePhotoVideoOrIdea() => Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().AddPostCommand.Execute(null);

        [Then(@"I can see the photo Url of the current user is ""(.*)""")]
        public void ThenICanSeeThePhotoUrlOfTheCurrentUserIs(string photoUrl)
        {
            Main.App.Container.GetContainer().Resolve<PostFeedAddEditPageViewModel>().CurrentContact.FBLink.ShouldEqual(photoUrl);
        }

        [When(@"I tap the close icon at the top left corner of the screen")]
        public void WhenITapTheCloseIconAtTheTopLeftCornerOfTheScreen() => Main.App.Container.GetContainer().Resolve<PostFeedAddEditPageViewModel>().NavigateBackCommand.Execute(null);

        [When(@"I type ""(.*)"" and tap the Post button")]
        public void WhenITypeAndTapThePostButton(string postMessage)
        {
            Main.App.Container.GetContainer().Resolve<PostFeedAddEditPageViewModel>().Content = postMessage;
            Main.App.Container.GetContainer().Resolve<PostFeedAddEditPageViewModel>().SaveOrEditPostCommand.Execute(null);
        }

        [Then(@"I can see that the button text is ""(.*)""")]
        public void ThenICanSeeThatTheButtonTextIs(string buttonText)
            => Main.App.Container.GetContainer().Resolve<PostFeedAddEditPageViewModel>().ButtonText.ShouldEqual(buttonText);


        [Then(@"I can see my newly posted postfeed with title ""(.*)"", post message ""(.*)"" and my fullname ""(.*)""")]
        public void ThenICanSeeMyNewlyPostedPostfeedWithTitlePostMessageAndMyFullname(string title, string postMessage, string fullName)
        {
            var newPost = Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().PostsList.Where(p => p.ContentText == postMessage).FirstOrDefault();
            newPost.ShouldNotBeNull();
            newPost.Title.ShouldEqual(title);
            newPost.ContentText.ShouldEqual(postMessage);
            newPost.PosterFullName.ShouldEqual(fullName);
        }

        [Then(@"I should see an empty post errror message ""(.*)""")]
        public void ThenIShouldSeeAnEmptyPostErrrorMessage(string errorMessage)
            => Main.App.Container.GetContainer().Resolve<PostFeedAddEditPageViewModel>().PageErrors.ToString().Contains(errorMessage).ShouldBeTrue();

        [Then(@"I could see the pull down to refresh instruction is displayed again")]
        [Given(@"I could see the pull down to refresh instruction is displayed")]
        public void GivenICouldSeeThePullDownToRefreshInstructionIsDisplayed()
        {
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().IsShowPullDownInstruction.ShouldBeTrue();
        }

        [When(@"I tap on the pull down to refresh instruction")]
        public void WhenITapOnThePullDownToRefreshInstruction()
        {
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().RemovePullDownInstructionCommand.Execute(null);
        }

        [Then(@"I should see that the instruction is not displayed")]
        public void ThenIShouldSeeThatTheInstructionIsNotDisplayed()
        {
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().IsShowPullDownInstruction.ShouldBeFalse();
        }

        [Then(@"I could see the pull down to refresh instruction is not displayed anymore")]
        public void ThenICouldSeeThePullDownToRefreshInstructionIsNotDisplayedAnymore()
        {
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().IsShowPullDownInstruction.ShouldBeFalse();
        }

        [Given(@"I could see that the IsNavigatingDetailsPage mark is ""(.*)""")]
        public void GivenICouldSeeThatTheIsNavigatingDetailsPageMarkIf(bool booleanValue)
        {
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().IsNavigatingToDetailsPage.ShouldEqual(booleanValue);
        }

        [Then(@"I could see that the IsNavigatingDetailsPage mark is ""(.*)""")]
        public void ThenICouldSeeThatTheIsNavigatingDetailsPageMarkIf(bool booleanValue)
        {
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().IsNavigatingToDetailsPage.ShouldEqual(booleanValue);
        }

        [When(@"I tap the load more post at the end")]
        public void WhenITapTheLoadMorePostAtTheEnd()
        {
            Main.App.Container.GetContainer().Resolve<FakeLoadingMorePost>().IsNotExpiredTime = true;
            Main.App.Container.GetContainer().Resolve<FakeLoadingMorePost>().IsDuplicatePost = true;
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().LoadMoreCommand.Execute(null);
        }

        [Then(@"I should that posts are added more")]
        public void ThenIShouldThatPostsAreAddedMore()
        {
            var fakeDataCountTwice = FakeData.FakePostFeeds.Posts.Where(x => x.PostFeedLevel != 1).Count() * 2;
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().PostsList.Count.ShouldBeGreaterThanOrEqualTo(fakeDataCountTwice);
        }

        [When(@"I pull to refresh the page")]
        public void WhenIPullToRefreshThePage()
        {
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().RefreshListCommand.Execute(null);
        }

        [Then(@"I should see that post list are refreshed from the server again")]
        public void ThenIShouldSeeThatPostListAreRefreshedFromTheServerAgain()
        {
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().PostsList.ShouldNotBeNull();
        }
    }
}

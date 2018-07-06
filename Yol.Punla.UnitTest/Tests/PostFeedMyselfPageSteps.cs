using Unity;
using Should;
using System.Linq;
using TechTalk.SpecFlow;
using Yol.Punla.FakeEntries;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.ViewModels;
using Prism.Unity;

namespace Yol.Punla.UnitTest
{
    [Binding]
    public class PostFeedMyselfPageSteps : StepBase
    {
        public PostFeedMyselfPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [When(@"I tap the write down what you want to say icon")]
        public void WhenITapTheWriteDownWhatYouWantToSayIcon()
        {
            Main.App.Container.GetContainer().Resolve<AppMasterPageViewModel>().WriteDownCommand.Execute(null);
        }

        [When(@"I tap the logon for facebook button with mobile no ""(.*)""")]
        public void WhenITapTheLogonForFacebookButtonWithMobileNo(string fbMobileNumber)
        {
            Main.App.Container.GetContainer().Resolve<ContactEntry>().MobilePhone = fbMobileNumber;
            Main.App.Container.GetContainer().Resolve<LogonPageViewModel>().FacebookLogonCommand.Execute(null);
        }

        [When(@"I tap the avatar icon at the top right corner")]
        public void WhenITapTheAvatarIconAtTheTopRightCorner()
        {
            Main.App.Container.GetContainer().Resolve<PostFeedPageViewModel>().DisplayOwnPostsCommand.Execute(null);
        }

        [Then(@"I should see the current contact first name ""(.*)"" and last name ""(.*)""")]
        public void ThenIShouldSeeTheCurrentContactFirstNameAndLastName(string firstName, string lastName)
        {
            Main.App.Container.GetContainer().Resolve<PostFeedMyselfPageViewModel>().CurrentContact.FirstName.ShouldEqual(firstName);
            Main.App.Container.GetContainer().Resolve<PostFeedMyselfPageViewModel>().CurrentContact.LastName.ShouldEqual(lastName);
        }

        [Then(@"I should see the user own post message ""(.*)""")]
        public void ThenIShouldSeeTheUserOwnPostMessage(string ownPostMessage)
        {
            var ownPost = Main.App.Container.GetContainer().Resolve<PostFeedMyselfPageViewModel>().PostsList.Where(x => x.ContentText == ownPostMessage);
            ownPost.Count().ShouldBeGreaterThan(0);
        }

        [When(@"I tap the comment icon of the post of ""(.*)"" with content ""(.*)"" and postFeedId ""(.*)"" from my own post page")]
        public void WhenITapTheCommentIconOfThePostOfWithContentAndPostFeedIdFromMyOwnPostPage(string authorName, string content, int postFeedId)
        {
            var stringArray = authorName.Split(' ');
            var postFeed = new Entity.PostFeed
            {
                PostFeedID = postFeedId,
                PosterFirstName = stringArray[0],
                PosterLastName = stringArray[1],
                ContentText = content
            };

            Main.App.Container.GetContainer().Resolve<PostFeedMyselfPageViewModel>().CommentCommand.Execute(postFeed);
        }

        [When(@"I tap the back icon from the post feed detail page")]
        public void WhenITapTheBackIconFromThePostFeedDetailPage()
        {
            Main.App.Container.GetContainer().Resolve<PostFeedDetailPageViewModel>().NavigateBackCommand.Execute(null);
        }
    }
}

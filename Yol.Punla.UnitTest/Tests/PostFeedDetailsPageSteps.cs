using Unity;
using TechTalk.SpecFlow;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.ViewModels;
using Prism.Unity;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public sealed class PostFeedDetailsPageSteps : StepBase
    {
        public PostFeedDetailsPageSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [When(@"I tap the back icon from PostFeedDetailsPage")]
        public void WhenITapTheBackIconFromPostFeedDetailsPage() => Main.App.Container.GetContainer().Resolve<PostFeedDetailPageViewModel>().NavigateBackCommand.Execute(null);

        [When(@"I type comment ""(.*)"" and tap the Post button")]
        public void WhenITypeCommentAndTapThePostButton(string CommentText)
        {
            Main.App.Container.GetContainer().Resolve<PostFeedDetailPageViewModel>().CommentText = CommentText;
            Main.App.Container.GetContainer().Resolve<PostFeedDetailPageViewModel>().WriteCommentCommand.Execute(null);
        }

        [Then(@"I should see an empty comment message ""(.*)""")]
        public void ThenIShouldSeeAnEmptyCommentMessage(string errorMessage) 
            => Main.App.Container.GetContainer().Resolve<PostFeedDetailPageViewModel>().PageErrors.ToString().Contains(errorMessage);
    }
}

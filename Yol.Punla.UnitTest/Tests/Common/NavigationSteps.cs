using TechTalk.SpecFlow;
using Yol.Punla.UnitTest.Barrack;
using Unity;
using Should;
using Yol.Punla.NavigationHeap;
using System.Reflection;
using Yol.Punla.ViewModels;
using Prism.Unity;

namespace Yol.Punla.UnitTest.Tests
{
    [Binding]
    public class NavigationSteps : StepBase
    {
        public NavigationSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [Then(@"I should stay on the same page ""(.*)""")]
        [Given(@"I am on the page ""(.*)""")]
        [Then(@"I am redirected to the page ""(.*)""")]
        public void ThenIAmRedirectedToThePage(string pageName)
        {
            var navigationStackService = Main.App.Container.GetContainer().Resolve<INavigationStackService>();
            var currentStack = navigationStackService.CurrentStack;
            navigationStackService.CurrentStack.ShouldEqual(pageName);

            string fullTypeName = "Yol.Punla.ViewModels." + pageName + "ViewModel";
            var viewModelType = typeof(App).GetTypeInfo().Assembly.GetType(fullTypeName);
            ViewModelBase viewModel = (ViewModelBase)Main.App.Container.GetContainer().Resolve(viewModelType);
            viewModel.IsBusy.ShouldBeFalse();
        }
    }
}

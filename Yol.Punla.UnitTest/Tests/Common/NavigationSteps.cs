using Prism.Unity;
using Should;
using System.Reflection;
using TechTalk.SpecFlow;
using Unity;
using Yol.Punla.NavigationHeap;
using Yol.Punla.UnitTest.Barrack;
using Yol.Punla.ViewModels;

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

            if (viewModelType != null)
            {
                ViewModelBase viewModel = (ViewModelBase)Main.App.Container.GetContainer().Resolve(viewModelType);
                //alfon.do not check this because of _navigationService.GoBackAsync, sometimes dont hit
                //sometimes work, sometimes bad
                viewModel.IsBusy.ShouldBeFalse();
            }
        }
    }
}

using Ninject;
using TechTalk.SpecFlow;

namespace Yol.Punla.UnitTest.WebAPI.Barrack
{
    [Binding]
    public class AppStart
    {
        public static IKernel Kernel { get; private set; }

        [BeforeScenario(Order = 0)]
        public void InitApp()
        {
            Kernel = new StandardKernel();
            Kernel.Bind<APIHttpResponse>().ToSelf().InSingletonScope();
        }
    }
}

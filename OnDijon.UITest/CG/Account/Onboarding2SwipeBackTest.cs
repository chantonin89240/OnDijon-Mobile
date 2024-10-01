using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Onboarding2SwipeBackTests
    {
        IApp app;
        Platform platform;

        public Onboarding2SwipeBackTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
        }

        [Test]
        [Category("no_login")]
        public void Onboarding2SwipeBackTest()
        {
            app.WaitForElement("LoginPage1");

            app.SwipeRightToLeft();

            app.WaitForElement("LoginPage2");

            app.SwipeLeftToRight();

            //affichage du dashboard ?
            AppResult[] Onboarding2SwipeBackResults = app.WaitForElement("LoginPage1");
            Assert.IsTrue(Onboarding2SwipeBackResults.Any());
        }
    }
}

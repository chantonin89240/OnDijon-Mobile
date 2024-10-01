using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LoginSwipeBackTests
    {
        IApp app;
        Platform platform;

        public LoginSwipeBackTests(Platform platform)
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
        public void LoginSwipeBackTest()
        {
            FastAccess.Onboarding(app);

            app.SwipeLeftToRight();

            //affichage de la page de connexion ?
            AppResult[] LoginSwipeBackResults = app.WaitForElement("LoginPage2");
            Assert.IsTrue(LoginSwipeBackResults.Any());
        }
    }
}
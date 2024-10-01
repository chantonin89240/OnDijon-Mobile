using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ProfileBackTests
    {
        IApp app;
        Platform platform;

        public ProfileBackTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
        }

        [Test]
        [Category("login")]
        public void ProfileBackTest()
        {
            FastAccess.Profile(app);

            app.Tap("NavBarBack");

            //affichage de la page de connexion ?
            AppResult[] ProfileBackResults = app.WaitForElement("DashboardView");
            Assert.IsTrue(ProfileBackResults.Any());
        }
    }
}
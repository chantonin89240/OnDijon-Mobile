using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangeProfileInfoBackTests
    {
        IApp app;
        Platform platform;

        public ChangeProfileInfoBackTests(Platform platform)
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
        public void ChangeProfileInfoBackTest()
        {
            FastAccess.UpdateProfile(app);

            app.Tap("NavBarBack");

            //affichage de la page de connexion ?
            AppResult[] ChangeProfileInfoBackResults = app.WaitForElement("ProfileView");
            Assert.IsTrue(ChangeProfileInfoBackResults.Any());
        }
    }
}

using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ProfileLogOutTests
    {
        IApp app;
        Platform platform;

        public ProfileLogOutTests(Platform platform)
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
        public void ProfileLogOutTest()
        {
            FastAccess.Profile(app);

            app.Tap("LogoutButton");

            //affichage de la page de connexion ?
            AppResult[] ProfileLogOutResults = app.WaitForElement("LoginView");
            Assert.IsTrue(ProfileLogOutResults.Any());
        }
    }
}
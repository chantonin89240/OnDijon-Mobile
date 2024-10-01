using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ProfileTests
    {
        IApp app;
        Platform platform;

        public ProfileTests(Platform platform)
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
        public void ProfileTest()
        {
            FastAccess.Profile(app);

            //affichage de la page de connexion ?
            AppResult[] ProfileResults = app.WaitForElement("ProfileView");
            Assert.IsTrue(ProfileResults.Any());
        }
    }
}

using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ProfileBackPhoneTests
    {
        IApp app;
        Platform platform;

        public ProfileBackPhoneTests(Platform platform)
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
        public void ProfileBackPhoneTest()
        {
            FastAccess.Profile(app);

            app.Back();

            //affichage de la page de connexion ?
            AppResult[] ProfileBackPhoneResults = app.WaitForElement("DashboardView");
            Assert.IsTrue(ProfileBackPhoneResults.Any());
        }
    }
}

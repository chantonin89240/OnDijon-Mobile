using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LocalisationBackTests
    {
        IApp app;
        Platform platform;

        public LocalisationBackTests(Platform platform)
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
        public void LocalisationBackTest()
        {
            FastAccess.Localisation(app);

            app.Tap("NavBarBack");

            //affichage de la partie type de signalement ?
            AppResult[] LocalisationBackResults = app.WaitForElement("ReportTypePage");
            Assert.IsTrue(LocalisationBackResults.Any());
        }
    }
}

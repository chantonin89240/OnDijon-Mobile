using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LocalisationTests
    {
        IApp app;
        Platform platform;

        public LocalisationTests(Platform platform)
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
        public void LocalisationTest()
        {
            FastAccess.Localisation(app);

            //affichage de la partie localisation ?
            AppResult[] LocalisationResults = app.WaitForElement("Localisation");
            Assert.IsTrue(LocalisationResults.Any());
        }
    }
}

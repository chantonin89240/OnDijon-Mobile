using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LocalisationBackPhoneTests
    {
        IApp app;
        Platform platform;

        public LocalisationBackPhoneTests(Platform platform)
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
        public void LocalisationBackPhoneTest()
        {
            FastAccess.Localisation(app);

            app.Back();

            //affichage de la partie type de signalement ?
            AppResult[] LocalisationBackPhoneResults = app.WaitForElement("ReportTypePage");
            Assert.IsTrue(LocalisationBackPhoneResults.Any());
        }
    }
}

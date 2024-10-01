using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignalementTests
    {
        IApp app;
        Platform platform;

        public SignalementTests(Platform platform)
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
        public void SignalementTest()
        {
            FastAccess.Signalement(app);

            //affichage de la partie type de signalement ?
            AppResult[] SignalementViewResults = app.WaitForElement("ReportsHomePage");
            Assert.IsTrue(SignalementViewResults.Any());
        }
    }
}
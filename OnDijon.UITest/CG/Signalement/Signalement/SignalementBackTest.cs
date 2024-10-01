using NUnit.Framework;
using OnDijon.UITest.Utils;
using System;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignalementBackTests
    {
        IApp app;
        Platform platform;

        public SignalementBackTests(Platform platform)
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
        public void SignalementBackTest()
        {
            FastAccess.Signalement(app);

            app.Tap("NavBarBack");

            //Sélection de la bonne adresse? ?
            AppResult[] SignalementBackTestResults = app.WaitForElement("DashboardView");
            Assert.IsTrue(SignalementBackTestResults.Any());
        }
    }
}
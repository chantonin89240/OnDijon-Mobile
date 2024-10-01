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
    public class ConfirmationBackTests
    {
        IApp app;
        Platform platform;

        public ConfirmationBackTests(Platform platform)
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
        public void ConfirmationBackTest()
        {
            FastAccess.Validation(app);

            app.Tap("NavBarBack");

            //Retour à la partie type de description
            AppResult[] ConfirmationBackResults = app.WaitForElement("ReportDescriptionPage");
            Assert.IsTrue(ConfirmationBackResults.Any());
        }
    }
}

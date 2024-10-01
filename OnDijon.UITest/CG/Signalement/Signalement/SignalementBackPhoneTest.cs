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
    public class SignalementBackPhoneTests
    {
        IApp app;
        Platform platform;

        public SignalementBackPhoneTests(Platform platform)
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
        public void SignalementBackPhoneTest()
        {
            FastAccess.Signalement(app);

            app.Back();

            //Sélection de la bonne adresse? ?
            AppResult[] SignalementBackPhoneResults = app.WaitForElement("DashboardView");
            Assert.IsTrue(SignalementBackPhoneResults.Any());
        }
    }
}
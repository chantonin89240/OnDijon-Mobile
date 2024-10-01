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
    public class ConfirmationBackPhoneTests
    {
        IApp app;
        Platform platform;

        public ConfirmationBackPhoneTests(Platform platform)
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
        public void ConfirmationBackPhoneTest()
        {
            FastAccess.Validation(app);

            app.Back();

            //Retour à la partie type de description
            AppResult[] ConfirmationBackPhoneResults = app.WaitForElement("ReportDescriptionPage");
            Assert.IsTrue(ConfirmationBackPhoneResults.Any());
        }
    }
}

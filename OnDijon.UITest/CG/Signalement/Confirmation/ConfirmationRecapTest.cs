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
    public class ConfirmationRecapTests
    {
        IApp app;
        Platform platform;

        public ConfirmationRecapTests(Platform platform)
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
        public void ConfirmationRecapTest()
        {
            FastAccess.Validation(app);

            //Infos du récap cohérentes?
            AppResult[] ConfirmationAddressResults = app.WaitForElement("AddressResult");
            Assert.AreEqual(ConfirmationAddressResults[0].Text, "40 Avenue du Drapeau, 21000, Dijon");
            AppResult[] ConfirmationDescriptionResults = app.WaitForElement("DescriptionResult");
            Assert.AreEqual(ConfirmationDescriptionResults[0].Text, "Description Test");
            AppResult[] ConfirmationTypeResults = app.WaitForElement("TypeResult");
            Assert.AreEqual(ConfirmationTypeResults[0].Text, FastAccess.Style);
        }
    }
}


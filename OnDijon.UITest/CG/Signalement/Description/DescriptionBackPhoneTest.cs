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
    public class DescriptionBackPhoneTests
    {
        IApp app;
        Platform platform;

        public DescriptionBackPhoneTests(Platform platform)
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
        public void DescriptionBackPhoneTest()
        {
            FastAccess.Description(app);

            app.Back();

            //Sélection de la bonne adresse? ?
            AppResult[] DescriptionBackPhoneResults = app.WaitForElement("Localisation");
            Assert.IsTrue(DescriptionBackPhoneResults.Any());
        }
    }
}

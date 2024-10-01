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
    public class DescriptionBackTests
    {
        IApp app;
        Platform platform;

        public DescriptionBackTests(Platform platform)
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
        public void DescriptionBackTest()
        {
            FastAccess.Description(app);

            app.Back();

            //Sélection de la bonne adresse? ?
            AppResult[] DescriptionBackTestResults = app.WaitForElement("Localisation");
            Assert.IsTrue(DescriptionBackTestResults.Any());
        }
    }
}
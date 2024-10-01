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
    public class DescriptionTests
    {
        IApp app;
        Platform platform;

        public DescriptionTests(Platform platform)
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
        public void DescriptionTest()
        {
            FastAccess.Description(app);

            //Sélection de la bonne adresse? ?
            AppResult[] LocalisationResults = app.WaitForElement("ReportDescriptionPage");
            Assert.IsTrue(LocalisationResults.Any());
        }
    }
}

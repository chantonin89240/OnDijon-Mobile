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
    public class TypeNoValidationExternalZoneTests
    {
        IApp app;
        Platform platform;

        public TypeNoValidationExternalZoneTests(Platform platform)
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
        public void TypeNoValidationExternalZoneTest()
        {
            FastAccess.TypeSignalement(app);

            app.Back();

            //Sélection de la bonne adresse? ?
            AppResult[] TypeNoValidationExternalZoneResults = app.WaitForElement("ReportTypePage");
            Assert.IsTrue(TypeNoValidationExternalZoneResults.Any());
        }
    }
}
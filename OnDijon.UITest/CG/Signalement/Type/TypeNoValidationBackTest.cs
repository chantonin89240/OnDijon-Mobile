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
    public class TypeNoValidationBackTests
    {
        IApp app;
        Platform platform;

        public TypeNoValidationBackTests(Platform platform)
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
        public void TypeNoValidationBackTest()
        {
            FastAccess.TypeSignalement(app);

            app.Tap("NavBarBack");

            //Sélection de la bonne adresse? ?
            AppResult[] TypeNoValidationBackResults = app.WaitForElement("ReportTypePage");
            Assert.IsTrue(TypeNoValidationBackResults.Any());
        }
    }
}
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
    public class TypeBackTests
    {
        IApp app;
        Platform platform;

        public TypeBackTests(Platform platform)
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
        public void TypeBackTest()
        {
            FastAccess.TypeSignalement(app);

            app.Tap(c => c.Marked("Continuer"));

            app.Tap("NavBarBack");

            //Sélection de la bonne adresse? ?
            AppResult[] TypeBackResults = app.WaitForElement("ReportsHomePage");
            Assert.IsTrue(TypeBackResults.Any());
        }
    }
}
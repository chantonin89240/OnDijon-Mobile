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
    public class TypeBackPhoneTests
    {
        IApp app;
        Platform platform;

        public TypeBackPhoneTests(Platform platform)
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
        public void TypeBackPhoneTest()
        {
            FastAccess.TypeSignalement(app);

            app.Tap(c => c.Marked("Continuer"));

            app.WaitForElement("Type0");

            app.Back();

            //Sélection de la bonne adresse? ?
            AppResult[] TypeBackPhoneResults = app.WaitForElement("ReportsHomePage");
            Assert.IsTrue(TypeBackPhoneResults.Any());
        }
    }
}
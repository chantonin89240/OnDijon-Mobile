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
    public class LocalisationSelectionTests
    {
        IApp app;
        Platform platform;

        public LocalisationSelectionTests(Platform platform)
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
        public void LocalisationSelectionTest()
        {
            FastAccess.Localisation(app);

            AppResult[] Map = app.WaitForElement("Map");

            app.TapCoordinates(Map[0].Rect.CenterX, Map[0].Rect.CenterY);

            //Sélection de la bonne adresse? ?
            app.WaitForElement(query => query.Text("40 Avenue du Drapeau, 21000, Dijon"));
            AppResult[] LocalisationSelectionResults = app.WaitForElement("SearchInput");
            Assert.AreEqual(LocalisationSelectionResults[0].Text, "40 Avenue du Drapeau, 21000, Dijon");
        }
    }
}

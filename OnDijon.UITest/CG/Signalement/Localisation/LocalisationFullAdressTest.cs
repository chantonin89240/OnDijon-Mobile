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
    public class LocalisationFullAdressTests
    {
        IApp app;
        Platform platform;

        public LocalisationFullAdressTests(Platform platform)
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
        public void LocalisationFullAdressTest()
        {
            FastAccess.Localisation(app);

            app.EnterText("AdressInput", "40 ");

            app.EnterText("AdressInput", ",Avenue du Drapeau 21000, Dijon");

            AppResult[] Liste = app.WaitForElement("AdressList");

            app.Tap(Liste[0].Text);

            //Sélection de la bonne adresse ?
            app.WaitForElement(query => query.Text("40 Avenue du Drapeau, 21000, Dijon"));
            AppResult[] LocalisationFullAdressResults = app.WaitForElement("SearchInput");
            Assert.AreEqual(LocalisationFullAdressResults[0].Text, "40 Avenue du Drapeau, 21000, Dijon");
        }
    }
}

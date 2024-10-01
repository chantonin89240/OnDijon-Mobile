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
    public class LocalisationPartAdressTests
    {
        IApp app;
        Platform platform;

        public LocalisationPartAdressTests(Platform platform)
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
        public void LocalisationPartAdressTest()
        {
            FastAccess.Localisation(app);

            app.EnterText("AdressInput", "4 rue de l'E");

            app.EnterText("AdressInput", "g");

            app.WaitForElement(query => query.Text("4 Rue de l'Egalite, Dijon"));

            app.Tap(query => query.Text("4 Rue de l'Egalite, Dijon"));

            //Affichage de l'adresse attendu ?
            app.WaitForNoElement("AdressList");
            app.WaitForElement(query => query.Text("4 Rue de l'Egalite, Dijon"));
            AppResult[] LocalisationPartAdressResults = app.WaitForElement("SearchInput");
            Assert.AreEqual(LocalisationPartAdressResults[0].Text, "4 Rue de l'Egalite, Dijon");
        }
    }
}

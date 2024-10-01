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
    public class LocalisationPartAdressNotFirstTests
    {
        IApp app;
        Platform platform;

        public LocalisationPartAdressNotFirstTests(Platform platform)
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
        public void LocalisationPartAdressNotFirstTest()
        {
            FastAccess.Localisation(app);

            app.EnterText("AdressInput", "4 rue ");

            app.WaitForElement("4 Rue Abbe Chanlon, Fenay");

            app.EnterText("AdressInput", "de");

            app.DismissKeyboard();

            app.WaitForElement("4 Rue de Bastogne, Saint-Apollinaire");

            app.ScrollDownTo("4 Rue de Bellevue, Talant");

            app.Tap(query => query.Text("4 Rue de Bellevue, Talant"));

            //Affichage de l'adresse attendu ?
            app.WaitForNoElement("AdressList");
            app.WaitForElement(query => query.Text("4 Rue de Bellevue, Talant"));
            AppResult[] LocalisationPartAdressNotFirstResults = app.WaitForElement("SearchInput");
            Assert.AreEqual(LocalisationPartAdressNotFirstResults[0].Text, "4 Rue de Bellevue, Talant");
        }
    }
}

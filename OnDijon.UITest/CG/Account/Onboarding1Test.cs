using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Onboarding1Tests
    {
        IApp app;
        Platform platform;

        public Onboarding1Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
        }

        [Test]
        [Category("no_login")]
        public void Onboarding1Test()
        {
            app.WaitForElement("LoginPage1");

            //affichage de la popup mot de passe invalide ?
            AppResult[] Text1Results = app.WaitForElement("Text1");
            Assert.IsTrue(Equals(Text1Results[0].Text, "Bienvenue sur OnDijon !"));

            AppResult[] Text2Results = app.WaitForElement("Text2");
            Assert.IsTrue(Equals(Text2Results[0].Text, "L'application citoyenne de la ville de Dijon imaginée pour toutes les Dijonnaises et les Dijonnais"));
        }
    }
}

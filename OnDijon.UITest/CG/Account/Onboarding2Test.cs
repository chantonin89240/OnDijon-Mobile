using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Onboarding2Tests
    {
        IApp app;
        Platform platform;

        public Onboarding2Tests(Platform platform)
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
        public void Onboarding2Test()
        {
            app.WaitForElement("LoginPage1");

            app.SwipeRightToLeft();

            app.WaitForElement("LoginPage2");

            //affichage de la popup mot de passe invalide ?
            AppResult[] Text1Results = app.WaitForElement("Text1");
            Assert.IsTrue(Equals(Text1Results[0].Text, "Tous les services de la ville à portée de main"));

            AppResult[] Text2Results = app.WaitForElement("Text2");
            Assert.IsTrue(Equals(Text2Results[0].Text, "Consulter les menus de la cantine, suivre ses emprunts de la bibliothèque, signaler la panne d'un lampadaire et bien plus encore !"));
        }
    }
}

using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpBackTests
    {
        IApp app;
        Platform platform;

        public SignUpBackTests(Platform platform)
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
        public void SignUpBackTest()
        {
            FastAccess.Onboarding(app);

            app.Tap("SignUpButton");

            app.Screenshot("SignUpView");

            app.Tap("NavBarBack");

            app.Screenshot("LoginView");

            //affichage de la page de connexion ?
            app.WaitForNoElement("SignUpView");
            AppResult[] SignUpViewResults = app.WaitForElement("LoginView");
            Assert.IsTrue(SignUpViewResults.Any());
        }
    }
}

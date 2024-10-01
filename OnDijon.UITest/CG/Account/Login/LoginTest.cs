using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LoginTests
    {
        IApp app;
        Platform platform;

        public LoginTests(Platform platform)
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
        public void LoginTest()
        {
            FastAccess.Onboarding(app);

            //affichage de la page de connexion ?
            AppResult[] LoginViewResults = app.WaitForElement("LoginView");
            Assert.IsTrue(LoginViewResults.Any());
        }
    }
}

using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LoginNoAccountMailTests
    {
        IApp app;
        Platform platform;

        public LoginNoAccountMailTests(Platform platform)
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
        public void LoginNoAccountMailTest()
        {
            FastAccess.Onboarding(app);

            app.EnterText("EmailEntry", "testnodijon@gmail.com");

            app.EnterText("PasswordEntry", "OnDijon2020");

            app.Tap("LoginButton");

            //affichage de la popup mail invalide ?
            TestClass.TestPopupError("Il n'existe aucun compte avec l'adresse e-mail : testnodijon@gmail.com.\n", app, "LoginButton", "LoginView");
        }
    }
}

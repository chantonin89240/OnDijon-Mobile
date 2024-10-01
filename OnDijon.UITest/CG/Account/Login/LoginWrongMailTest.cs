using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LoginWrongMailTests
    {
        IApp app;
        Platform platform;

        public LoginWrongMailTests(Platform platform)
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
        public void WrongMailTest()
        {
            FastAccess.Onboarding(app);

            app.EnterText("EmailEntry", "Mailinvalide");

            app.EnterText("PasswordEntry", "WrongPassword");

            app.Tap("LoginButton");

            //affichage de la popup mail invalide ?
            //TestClass.TestPopupError("Veuillez renseigner une adresse email correcte", app, "LoginButton", "LoginView");
            TestClass.TestErrorMessage(app, "Email incorrect");
        }
    }
}

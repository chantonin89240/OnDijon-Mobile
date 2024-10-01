using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LoginWrongPasswordTests
    {
        IApp app;
        Platform platform;

        public LoginWrongPasswordTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        //Méthode permettant de réinitialiser l'application
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
        }

        [Test]
        //Classification du test dans les tests à effectuer sans connexion
        [Category("no_login")]
        public void LoginWrongPasswordTest()
        {
            FastAccess.Onboarding(app);

            //Saisie de l'adresse email dans le champs "EmailEntry"
            app.EnterText("EmailEntry", "testondijon@gmail.com");

            //Saisie du mot de passe érroné dans le champs "EmailEntry"
            app.EnterText("PasswordEntry", "WrongPassword");

            //Clic sur le bouton "LoginButton"
            app.Tap("LoginButton");

            //affichage de la popup mot de passe invalide ?
            TestClass.TestPopupError("Le mot de passe n'est pas valide\n", app, "LoginButton", "LoginView");
        }
    }
}

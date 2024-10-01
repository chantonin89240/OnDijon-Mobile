using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LoginAllTests
    {
        IApp app;
        Platform platform;

        public LoginAllTests(Platform platform)
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
        public void LoginAllTest()
        {
            FastAccess.Onboarding(app);

            app.Tap("LoginButton");

            //affichage de la page de connexion ?
            AppResult[] LoginViewResults = app.WaitForElement("LoginView");
            Assert.IsTrue(LoginViewResults.Any());

            app.SwipeLeftToRight();

            //affichage de la page de connexion ?
            AppResult[] LoginSwipeBackResults = app.WaitForElement("LoginPage2");
            Assert.IsTrue(LoginSwipeBackResults.Any());

            app.SwipeRightToLeft();

            app.EnterText("PasswordEntry", "OnDijon2020");

            app.DismissKeyboard();

            //Impossible de cliquer sur valider
            TestClass.TestNoValidation(app, "LoginButton");

            app.EnterText("EmailEntry", "testnodijon@gmail.com");

            app.DismissKeyboard();

            app.Tap("LoginButton");

            //affichage de la popup mail invalide ?
            TestClass.TestPopupError("Il n'existe aucun compte avec l'adresse e-mail : testnodijon@gmail.com.\n", app, "LoginButton", "LoginView");

            app.ClearText("EmailEntry");

            app.EnterText("EmailEntry", "Mailinvalide");

            app.DismissKeyboard();

            app.Tap("LoginButton");

            //affichage de la popup mail invalide ?
            TestClass.TestPopupError("Veuillez renseigner une adresse email correcte", app, "LoginButton", "LoginView");

            app.ClearText("EmailEntry");

            app.EnterText("EmailEntry", "testondijon@gmail.com");

            app.ClearText("PasswordEntry");

            app.DismissKeyboard();

            //Impossible de cliquer sur valider
            TestClass.TestNoValidation(app, "LoginButton");

            app.EnterText("PasswordEntry", "WrongPassword");

            app.DismissKeyboard();

            app.Tap("LoginButton");

            //affichage de la popup mot de passe invalide ?
            TestClass.TestPopupError("Le mot de passe n'est pas valide\n", app, "LoginButton", "LoginView");

            app.ClearText("PasswordEntry");

            app.EnterText("PasswordEntry", "OnDijon2020");

            app.DismissKeyboard();

            app.Tap("LoginButton");

            //affichage du dashboard ?
            AppResult[] DashboardViewResults = app.WaitForElement("DashboardView");
            Assert.IsTrue(DashboardViewResults.Any());
        }
    }
}
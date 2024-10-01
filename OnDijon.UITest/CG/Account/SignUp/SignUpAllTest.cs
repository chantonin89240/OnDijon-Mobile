using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpAllTests
    {
        IApp app;
        Platform platform;

        public SignUpAllTests(Platform platform)
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
        public void SignUpAllTest()
        {
            FastAccess.SignUp(app);

            app.ScrollUpTo("Name");

            app.ClearText("Name");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //Impossible de valider
            TestClass.TestNoValidation(app, "Validate");

            app.ScrollUpTo("Name");

            app.EnterText("Name", "Test2020");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup mail invalide ?
            TestClass.TestPopupError("Nom invalide", app, "Validate", "SignUpView");

            app.ScrollUpTo("Name");

            app.ClearText("Name");

            app.EnterText("Name", "Test");

            app.ClearText("Firstname");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //Impossible de valider
            TestClass.TestNoValidation(app, "Validate");

            app.ScrollUpTo("Firstname");

            app.EnterText("Firstname", "Test2020");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup mail invalide ?
            TestClass.TestPopupError("Prénom invalide", app, "Validate", "SignUpView");

            app.ScrollUpTo("Firstname");

            app.ClearText("Firstname");

            app.EnterText("Firstname", "Test");

            app.ClearText("Email");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //Impossible de valider
            TestClass.TestNoValidation(app, "Validate");

            app.ScrollUpTo("Email");

            app.EnterText("Email", "MailInvalide");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup mail invalide ?
            TestClass.TestPopupError("Email invalide", app, "Validate", "SignUpView");

            app.ScrollUpTo("Email");

            app.ClearText("Email");

            app.EnterText("Email", "testondijon@gmail.com");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup mail invalide ?
            TestClass.TestPopupError("Il existe déjà un compte avec cette adresse mail.\n", app, "Validate", "SignUpView");

            app.ScrollUpTo("Email");

            app.ClearText("Email");

            app.EnterText("Email", "testnodijon@gmail.com");

            app.ScrollDownTo("Password");

            app.ClearText("Password");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //Impossible de valider
            TestClass.TestNoValidation(app, "Validate");

            app.ScrollUpTo("Password");

            app.EnterText("Password", "OnDijon2020");

            app.DismissKeyboard();

            app.ClearText("PasswordConfirmation");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //Impossible de valider
            TestClass.TestNoValidation(app, "Validate");

            app.ScrollUpTo("PasswordConfirmation");

            app.EnterText("PasswordConfirmation", "OnDijon2021");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup mail invalide ?
            TestClass.TestPopupError("Les mots de passes sont différents", app, "Validate", "SignUpView");

            app.ScrollUpTo("Password");

            app.ClearText("Password");

            app.EnterText("Password", "dijon");

            app.DismissKeyboard();

            app.ClearText("PasswordConfirmation");

            app.EnterText("PasswordConfirmation", "dijon");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup mail invalide ?
            TestClass.TestPopupError("Mot de passe invalide", app, "Validate", "SignUpView");

            app.Back();

            app.Screenshot("LoginView");

            //affichage de la page de connexion ?
            app.WaitForNoElement("SignUpView");
            AppResult[] SignUpViewResults = app.WaitForElement("LoginView");
            Assert.IsTrue(SignUpViewResults.Any());

            app.Tap("SignUpButton");

            app.Tap("NavBarBack");

            //affichage de la page de connexion ?
            app.WaitForNoElement("SignUpView");
            AppResult[] SignUpViewResults2 = app.WaitForElement("LoginView");
            Assert.IsTrue(SignUpViewResults2.Any());
        }
    }
}

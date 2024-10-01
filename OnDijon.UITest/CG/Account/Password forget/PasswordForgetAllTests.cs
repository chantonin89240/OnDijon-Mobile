using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;


namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class PasswordForgetAllTests
    {
        IApp app;
        Platform platform;

        public PasswordForgetAllTests(Platform platform)
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
        public void PasswordForgetAllTest()
        {
            FastAccess.ForgetPassword(app);

            //affichage de la page de récupération de mot de passe ?
            AppResult[] ChangePasswordPopupViewResults = app.WaitForElement("ResetPasswordView");
            Assert.IsTrue(ChangePasswordPopupViewResults.Any());

            app.EnterText("Email", "MailInvalide");

            app.DismissKeyboard();

            app.Tap("Validate");

            //affichage de la popup Mail invalide ?
            TestClass.TestPopupError("Veuillez renseigner une adresse email correcte", app, "Validate", "ResetPasswordView");

            app.Back();

            //affichage de la page de connexion?
            AppResult[] PasswordForgetBackPhoneResults = app.WaitForElement("LoginView");
            Assert.IsTrue(PasswordForgetBackPhoneResults.Any());

            app.Tap("ForgetPasswordButton");

            app.Tap("NavBarBack");

            //affichage de la page de connexion ?
            AppResult[] LoginViewResults = app.WaitForElement("LoginView");
            Assert.IsTrue(LoginViewResults.Any());
        }
    }
}
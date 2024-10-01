using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;


namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class PasswordForgetWrongMailTests
    {
        IApp app;
        Platform platform;

        public PasswordForgetWrongMailTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            //Fermeture et redémarage de l'application
            app = AppInitializer.StartApp(platform, true);
        }

        [Test]
        [Category("no_login")]
        public void PasswordForgetWrongMailTest()
        {
            FastAccess.ForgetPassword(app);
            //Saisie d'une adresse mail non valide
            app.EnterText("Email", "MailInvalide");
            //Disparition du clavier pour accéder aux boutons
            app.DismissKeyboard();
            //Clic sur le bouton valider
            app.Tap("Validate");

            //affichage de la popup adresse mail incorrecte ?
            //TestClass.TestPopupError("Veuillez renseigner une adresse email correcte", app, "Validate", "ResetPasswordView");
            TestClass.TestErrorMessage(app, "Email incorrect");
        }
    }
}


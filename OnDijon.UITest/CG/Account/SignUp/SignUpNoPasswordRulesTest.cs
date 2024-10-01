using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpNoPasswordRulesTests
    {
        IApp app;
        Platform platform;

        public SignUpNoPasswordRulesTests(Platform platform)
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
        public void SignUpNoPasswordRulesTest()
        {
            FastAccess.SignUp(app);

            app.ScrollUpTo("Password");

            app.ClearText("Password");

            app.ClearText("PasswordConfirmation");

            app.EnterText("Password", "dijon");

            app.EnterText("PasswordConfirmation", "dijon");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup nom non renseigné ?
            //TestClass.TestPopupError("Mot de passe invalide", app, "Validate", "SignUpView");
            TestClass.TestErrorMessage(app, "Le mot de passe doit contenir au moins 8 caractères dont au moins une majuscule, une minuscule et un chiffre");
        }
    }
}

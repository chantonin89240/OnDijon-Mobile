using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangePasswordNoRulesTests
    {
        IApp app;
        Platform platform;

        public ChangePasswordNoRulesTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
        }

        [Test]
        [Category("login")]
        public void ChangePasswordNoRulesTest()
        {
            FastAccess.UpdateProfile(app);

            app.ScrollDownTo("Validate");

            app.EnterText("OldPassword", "OnDijon2020");

            app.DismissKeyboard();

            app.EnterText("NewPassword", "dijon");

            app.DismissKeyboard();

            app.EnterText("NewPasswordConfirmation", "dijon");

            app.DismissKeyboard();

            app.Tap("Validate");

            //Affichage de la popup mots de passe ne respecte pas les règles ?
            //TestClass.TestPopupError("Saisir un mot de passe valide", app, "Validate", "ChangeProfileInfoView");
            TestClass.TestErrorMessage(app, "Le mot de passe doit contenir au moins 8 caractères dont au moins une majuscule, une minuscule et un chiffre");
        }
    }
}

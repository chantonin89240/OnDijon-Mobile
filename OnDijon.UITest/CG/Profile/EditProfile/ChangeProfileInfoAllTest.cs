using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangeProfileInfoAllTests
    {
        IApp app;
        Platform platform;

        public ChangeProfileInfoAllTests(Platform platform)
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
        public void ChangeProfileInfoAllTest()
        {
            FastAccess.UpdateProfile(app);

            //affichage de la page de connexion ?
            AppResult[] ChangeProfileInfoResults = app.WaitForElement("ChangeProfileInfoView");
            Assert.IsTrue(ChangeProfileInfoResults.Any());

            app.Back();

            //affichage de la page de connexion ?
            AppResult[] ChangeProfileInfoBackPhoneResults = app.WaitForElement("ProfileView");
            Assert.IsTrue(ChangeProfileInfoBackPhoneResults.Any());

            app.Tap("UpdateProfile");

            app.Tap("NavBarBack");

            //affichage de la page de connexion ?
            AppResult[] ChangeProfileInfoBackResults = app.WaitForElement("ProfileView");
            Assert.IsTrue(ChangeProfileInfoBackResults.Any());

            app.Tap("UpdateProfile");

            app.ClearText("FirstName");

            app.EnterText("FirstName", "Test2020");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            // Affichage de la popup nom ne respecte pas les règles?
            TestClass.TestPopupError("Prénom invalide", app, "Validate", "ChangeProfileInfoView");

            app.ScrollUpTo("Name");
            
            app.ClearText("FirstName");

            app.EnterText("FirstName", "Test");

            app.ClearText("Email");

            app.EnterText("Email", "MailInvalide");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            // Affichage de la popup nom ne respecte pas les règles?
            TestClass.TestPopupError("Email invalide", app, "Validate", "ChangeProfileInfoView");

            app.ScrollUpTo("Name");

            app.ClearText("Email");

            app.EnterText("Email", "testondijon@gmail.com");

            app.ClearText("Name");

            app.EnterText("Name", "Test2020");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            // Affichage de la popup nom ne respecte pas les règles?
            TestClass.TestPopupError("Nom invalide", app, "Validate", "ChangeProfileInfoView");

            app.ScrollUpTo("Name");

            app.ClearText("Name");

            app.EnterText("Name", "Test");

            app.ClearText("PhoneNumber");

            app.EnterText("PhoneNumber", "InvalidePhone");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            // Affichage de la popup nom ne respecte pas les règles?
            TestClass.TestPopupError("Téléphone invalide", app, "Validate", "ChangeProfileInfoView");

            app.ScrollUpTo("Name");

            app.ClearText("PhoneNumber");

            app.EnterText("PhoneNumber", "0606060606");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.EnterText("OldPassword", "OnDijon2020");

            app.DismissKeyboard();

            app.EnterText("NewPassword", "dijon");

            app.DismissKeyboard();

            app.EnterText("NewPasswordConfirmation", "dijon");

            app.DismissKeyboard();

            app.Tap("Validate");

            //Affichage de la popup mots de passe ne respecte pas les règles ?
            TestClass.TestPopupError("Saisir un mot de passe valide", app, "Validate", "ChangeProfileInfoView");

            app.ClearText("OldPassword");

            app.EnterText("OldPassword", "OnDijon2020");

            app.DismissKeyboard();

            app.ClearText("NewPassword");

            app.EnterText("NewPassword", "OnDijon2021");

            app.DismissKeyboard();

            app.ClearText("NewPasswordConfirmation");

            app.EnterText("NewPasswordConfirmation", "OnDijon2022");

            app.DismissKeyboard();

            app.Tap("Validate");

            //Affichage de la popup mots de passe différents? ?
            TestClass.TestPopupError("Les mots de passe sont différents", app, "Validate", "ChangeProfileInfoView");

            app.ClearText("OldPassword");

            app.EnterText("OldPassword", "OnDijon2019");

            app.DismissKeyboard();

            app.ClearText("NewPasswordConfirmation");

            app.EnterText("NewPasswordConfirmation", "OnDijon2021");

            app.DismissKeyboard();

            app.Tap("Validate");

            //Affichage de la popup mots de passe différents? ?
            TestClass.TestPopupError("L'ancien mot de passe est erroné.\n", app, "Validate", "ChangeProfileInfoView");

            app.ClearText("OldPassword");

            app.EnterText("OldPassword", "OnDijon2020");

            app.DismissKeyboard();

            app.Tap("Validate");

            //Changement de mot de passe effectué? ?
            TestClass.TestPopupSuccess("Le mot de passe a été changé avec succès (ok).\n", app);

            app.Tap(query => query.Text("Revenir à mon profil"));

            app.Tap("UpdateProfile");

            app.ScrollDownTo("Validate");

            app.ClearText("OldPassword");

            app.ClearText("NewPassword");

            app.ClearText("NewPasswordConfirmation");

            app.EnterText("OldPassword", "OnDijon2021");

            app.DismissKeyboard();

            app.EnterText("NewPassword", "OnDijon2020");

            app.DismissKeyboard();

            app.EnterText("NewPasswordConfirmation", "OnDijon2020");

            app.DismissKeyboard();

            app.Tap("Validate");

            //Changement de mot de passe effectué? ?
            TestClass.TestPopupSuccess("Le mot de passe a été changé avec succès (ok).\n", app);
        }
    }
}

using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangePasswordDoneTests
    {
        IApp app;
        Platform platform;

        public ChangePasswordDoneTests(Platform platform)
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
        public void ChangePasswordDoneTest()
        {
            FastAccess.UpdateProfile(app);

            app.ScrollDownTo("Validate");

            app.EnterText("OldPassword", "OnDijon2020");

            app.DismissKeyboard();

            app.EnterText("NewPassword", "OnDijon2021");

            app.DismissKeyboard();

            app.EnterText("NewPasswordConfirmation", "OnDijon2021");

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

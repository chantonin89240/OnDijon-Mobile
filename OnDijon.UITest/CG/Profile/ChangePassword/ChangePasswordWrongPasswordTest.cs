using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangePasswordWrongPasswordTests
    {
        IApp app;
        Platform platform;

        public ChangePasswordWrongPasswordTests(Platform platform)
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
        public void ChangePasswordWrongPasswordTest()
        {
            FastAccess.UpdateProfile(app);

            app.ScrollDownTo("Validate");

            app.EnterText("OldPassword", "OnDijon2019");

            app.DismissKeyboard();

            app.EnterText("NewPassword", "OnDijon2021");

            app.DismissKeyboard();

            app.EnterText("NewPasswordConfirmation", "OnDijon2021");

            app.DismissKeyboard();

            app.Tap("Validate");

            //Affichage de la popup mot de passe incorrect ?
            TestClass.TestPopupError("L'ancien mot de passe est erroné.\n", app, "Validate", "ChangeProfileInfoView");
        }
    }
}

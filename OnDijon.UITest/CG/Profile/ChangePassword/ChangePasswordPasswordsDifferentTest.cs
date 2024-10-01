using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangePasswordPasswordsDifferentTests
    {
        IApp app;
        Platform platform;

        public ChangePasswordPasswordsDifferentTests(Platform platform)
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
        public void ChangePasswordPasswordsDifferentTest()
        {
            FastAccess.UpdateProfile(app);

            app.ScrollDownTo("Validate");

            app.EnterText("OldPassword", "OnDijon2020");

            app.DismissKeyboard();

            app.EnterText("NewPassword", "OnDijon2021");

            app.DismissKeyboard();

            app.EnterText("NewPasswordConfirmation", "OnDijon2022");

            app.DismissKeyboard();

            app.Tap("Validate");

            //Affichage de la popup mots de passe différents? ?
            //TestClass.TestPopupError("Les mots de passe sont différents", app, "Validate", "ChangeProfileInfoView");
            TestClass.TestErrorMessage(app, "Les mots de passe sont différents");
        }
    }
}

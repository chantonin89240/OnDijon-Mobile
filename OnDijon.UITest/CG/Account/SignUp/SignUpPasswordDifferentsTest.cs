using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpPasswordDifferentsTests
    {
        IApp app;
        Platform platform;

        public SignUpPasswordDifferentsTests(Platform platform)
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
        public void SignUpPasswordDifferentsTest()
        {
            FastAccess.SignUp(app);

            app.ScrollUpTo("Password");

            app.ClearText("Password");

            app.ClearText("PasswordConfirmation");

            app.EnterText("Password", "OnDijon2020");

            app.EnterText("PasswordConfirmation", "OnDijon2021");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup mots de passe différents ?
            //TestClass.TestPopupError("Les mots de passes sont différents", app, "Validate", "SignUpView");
            TestClass.TestErrorMessage(app, "Les mots de passe sont différents");
        }
    }
}

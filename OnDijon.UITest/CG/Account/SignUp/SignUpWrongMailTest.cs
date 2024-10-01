using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpWrongMailTests
    {
        IApp app;
        Platform platform;

        public SignUpWrongMailTests(Platform platform)
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
        public void SignUpWrongMailTest()
        {
            FastAccess.SignUp(app);

            app.ScrollUpTo("Email");

            app.ClearText("Email");

            app.EnterText("Email", "MailInvalide");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup mail invalide ?
            //TestClass.TestPopupError("Email invalide", app, "Validate", "SignUpView");
            TestClass.TestErrorMessage(app, "Email invalide");
        }
    }
}

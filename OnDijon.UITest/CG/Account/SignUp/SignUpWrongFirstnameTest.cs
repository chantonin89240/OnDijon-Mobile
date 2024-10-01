using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpWrongFirstnameTests
    {
        IApp app;
        Platform platform;

        public SignUpWrongFirstnameTests(Platform platform)
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
        public void SignUpWrongFirstnameTest()
        {
            FastAccess.SignUp(app);

            app.ScrollUpTo("Firstname");

            app.ClearText("Firstname");

            app.EnterText("Firstname", "Test2020");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup mail invalide ?
            //TestClass.TestPopupError("Prénom invalide", app, "Validate", "SignUpView");
            TestClass.TestErrorMessage(app, "Prénom invalide");
        }
    }
}

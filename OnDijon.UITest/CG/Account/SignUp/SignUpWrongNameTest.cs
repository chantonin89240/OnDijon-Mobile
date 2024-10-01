using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpWrongNameTests
    {
        IApp app;
        Platform platform;

        public SignUpWrongNameTests(Platform platform)
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
        public void SignUpWrongNameTest()
        {
            FastAccess.SignUp(app);

            app.ScrollUpTo("Name");

            app.ClearText("Name");

            app.EnterText("Name", "Test2020");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup mail invalide ?
            //TestClass.TestPopupError("Nom invalide", app, "Validate", "SignUpView");
            TestClass.TestErrorMessage(app, "Nom invalide");
        }
    }
}

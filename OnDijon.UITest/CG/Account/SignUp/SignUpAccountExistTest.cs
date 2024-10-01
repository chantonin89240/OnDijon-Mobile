using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpAccountExistTests
    {
        IApp app;
        Platform platform;

        public SignUpAccountExistTests(Platform platform)
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
        public void SignUpAccountExistTest()
        {
            FastAccess.SignUp(app);

            app.ScrollUpTo("Email");

            app.ClearText("Email");

            app.EnterText("Email", "testondijon@gmail.com");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //affichage de la popup nom non renseigné ?
            TestClass.TestPopupError("Il existe déjà un compte avec cette adresse mail.\n", app, "Validate", "SignUpView");
        }
    }
}



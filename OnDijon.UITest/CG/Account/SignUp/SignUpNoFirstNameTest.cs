using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpNoFirstNameTests
    {
        IApp app;
        Platform platform;

        public SignUpNoFirstNameTests(Platform platform)
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
        public void SignUpNoFirstNameTest()
        {
            FastAccess.SignUp(app);

            app.ScrollUpTo("Firstname");

            app.ClearText("Firstname");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            //Impossible de valider
            TestClass.TestNoValidation(app, "Validate");
        }
    }
}

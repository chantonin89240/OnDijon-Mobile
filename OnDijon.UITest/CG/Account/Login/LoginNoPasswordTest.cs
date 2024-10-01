using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LoginNoPasswordTests
    {
        IApp app;
        Platform platform;

        public LoginNoPasswordTests(Platform platform)
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
        public void LoginNoPasswordTest()
        {
            FastAccess.Onboarding(app);

            app.EnterText("EmailEntry", "testondijon@gmail.com");

            app.DismissKeyboard();

            //Impossible de cliquer sur valider
            TestClass.TestNoValidation(app, "LoginButton");
        }
    }
}

using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LoginNoMailTests
    {
        IApp app;
        Platform platform;

        public LoginNoMailTests(Platform platform)
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
        public void LoginNoAccountMailTest()
        {
            FastAccess.Onboarding(app);

            app.EnterText("PasswordEntry", "OnDijon2020");

            //Impossible de cliquer sur valider
            TestClass.TestNoValidation(app, "LoginButton");
        }
    }
}

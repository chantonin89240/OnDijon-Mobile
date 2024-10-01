using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpBackPhoneTests
    {
        IApp app;
        Platform platform;

        public SignUpBackPhoneTests(Platform platform)
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
        public void SignUpBackPhoneTest()
        {
            FastAccess.Onboarding(app);

            app.Tap("SignUpButton");

            app.Screenshot("SignUpView");

            app.Back();

            app.Screenshot("LoginView");

            //affichage de la page de connexion ?
            app.WaitForNoElement("SignUpView");
            AppResult[] SignUpBackPhoneResults = app.WaitForElement("LoginView");
            Assert.IsTrue(SignUpBackPhoneResults.Any());
        }
    }
}

using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;


namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class PasswordForgetBackPhoneTests
    {
        IApp app;
        Platform platform;

        public PasswordForgetBackPhoneTests(Platform platform)
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
        public void PasswordForgetBackPhoneTest()
        {
            FastAccess.ForgetPassword(app);

            app.Back();

            //affichage de la page de connexion?
            AppResult[] PasswordForgetBackPhoneResults = app.WaitForElement("LoginView");
            Assert.IsTrue(PasswordForgetBackPhoneResults.Any());
        }
    }
}

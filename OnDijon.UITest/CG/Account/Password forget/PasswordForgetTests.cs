using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;


namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class PasswordForgetTests
    {
        IApp app;
        Platform platform;

        public PasswordForgetTests(Platform platform)
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
        public void PasswordForgetTest()
        {
            FastAccess.ForgetPassword(app);

            //affichage de la page de récupération de mot de passe ?
            AppResult[] ChangePasswordPopupViewResults = app.WaitForElement("ResetPasswordView");
            Assert.IsTrue(ChangePasswordPopupViewResults.Any());
        }
    }
}

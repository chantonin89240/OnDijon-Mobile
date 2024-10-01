using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SignUpTests
    {
        IApp app;
        Platform platform;

        public SignUpTests(Platform platform)
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
        public void SignUpTest()
        {
            FastAccess.SignUp(app);

            //affichage de la page de création de compte ?
            AppResult[] SignUpViewResults = app.WaitForElement("SignUpView");
            Assert.IsTrue(SignUpViewResults.Any());
        }
    }
}

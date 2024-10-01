using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangeProfileInfoBackPhoneTests
    {
        IApp app;
        Platform platform;

        public ChangeProfileInfoBackPhoneTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform, true);
        }

        [Test]
        [Category("login")]
        public void ChangeProfileInfoBackPhoneTest()
        {
            FastAccess.UpdateProfile(app);

            app.Back();

            //affichage de la page de connexion ?
            AppResult[] ChangeProfileInfoBackPhoneResults = app.WaitForElement("ProfileView");
            Assert.IsTrue(ChangeProfileInfoBackPhoneResults.Any());
        }
    }
}

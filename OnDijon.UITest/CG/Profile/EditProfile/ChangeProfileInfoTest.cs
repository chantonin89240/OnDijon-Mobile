using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangeProfileInfoTests
    {
        IApp app;
        Platform platform;

        public ChangeProfileInfoTests(Platform platform)
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
        public void ChangeProfileInfoTest()
        {
            FastAccess.UpdateProfile(app);

            //affichage de la page de connexion ?
            AppResult[] ChangeProfileInfoResults = app.WaitForElement("ChangeProfileInfoView");
            Assert.IsTrue(ChangeProfileInfoResults.Any());
        }
    }
}

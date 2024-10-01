using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class RightLoginTests
    {
        IApp app;
        Platform platform;

        public RightLoginTests(Platform platform)
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
        public void RightLoginTest()
        {
            FastAccess.Login(app);

            //affichage du dashboard ?
            AppResult[] DashboardViewResults = app.WaitForElement("DashboardView");
            Assert.IsTrue(DashboardViewResults.Any());
        }
    }
}

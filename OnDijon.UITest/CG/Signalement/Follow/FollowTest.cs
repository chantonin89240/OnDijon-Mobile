using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class FollowTests
    {
        IApp app;
        Platform platform;

        public FollowTests(Platform platform)
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
        public void TypeTest()
        {
            FastAccess.FollowReport(app);

            //affichage de la partie type de signalement ?
            AppResult[] FollowTestResults = app.WaitForElement("ReportUserView");
            Assert.IsTrue(FollowTestResults.Any());
        }
    }
}


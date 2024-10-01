using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class TypeTests
    {
        IApp app;
        Platform platform;

        public TypeTests(Platform platform)
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
            FastAccess.TypeSignalement(app);

            //affichage de la partie type de signalement ?
            AppResult[] TypeViewResults = app.WaitForElement("ReportTypePage");
            Assert.IsTrue(TypeViewResults.Any());
        }
    }
}

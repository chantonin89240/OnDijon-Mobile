using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangeProfileInfoWrongNameTests
    {
        IApp app;
        Platform platform;

        public ChangeProfileInfoWrongNameTests(Platform platform)
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
        public void ChangeProfileInfoWrongNameTest()
        {
            FastAccess.UpdateProfile(app);

            app.ClearText("Name");

            app.EnterText("Name", "Test2020");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            // Affichage de la popup nom ne respecte pas les règles?
            //TestClass.TestPopupError("Nom invalide", app, "Validate", "ChangeProfileInfoView");
            TestClass.TestErrorMessage(app, "Nom invalide");
        }
    }
}

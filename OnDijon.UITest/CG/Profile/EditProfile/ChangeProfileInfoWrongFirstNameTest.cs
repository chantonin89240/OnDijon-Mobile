using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangeProfileInfoWrongFirstNameTests
    {
        IApp app;
        Platform platform;

        public ChangeProfileInfoWrongFirstNameTests(Platform platform)
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
        public void ChangeProfileInfoWrongFirstNameTest()
        {
            FastAccess.UpdateProfile(app);

            app.ClearText("FirstName");

            app.EnterText("FirstName", "Test2020");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            // Affichage de la popup nom ne respecte pas les règles?
            //TestClass.TestPopupError("Prénom invalide", app, "Validate", "ChangeProfileInfoView");
            TestClass.TestErrorMessage(app, "Prénom invalide");
        }
    }
}

using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangeProfileInfoWrongPhoneTests
    {
        IApp app;
        Platform platform;

        public ChangeProfileInfoWrongPhoneTests(Platform platform)
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
        public void ChangeProfileInfoWrongPhoneTest()
        {
            FastAccess.UpdateProfile(app);

            app.ClearText("PhoneNumber");

            app.EnterText("PhoneNumber", "12345");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            // Affichage de la popup nom ne respecte pas les règles?
            //TestClass.TestPopupError("Téléphone invalide", app, "Validate", "ChangeProfileInfoView");
            TestClass.TestErrorMessage(app, "Téléphone invalide");
        }
    }
}
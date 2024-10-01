using NUnit.Framework;
using OnDijon.UITest.Utils;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class ChangeProfileInfoWrongMailTests
    {
        IApp app;
        Platform platform;

        public ChangeProfileInfoWrongMailTests(Platform platform)
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
        public void ChangeProfileInfoWrongMailTest()
        {
            FastAccess.UpdateProfile(app);

            app.ClearText("Email");

            app.EnterText("Email", "MailInvalide");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.Tap("Validate");

            // Affichage de la popup nom ne respecte pas les règles?
            //TestClass.TestPopupError("Email invalide", app, "Validate", "ChangeProfileInfoView");
            TestClass.TestErrorMessage(app, "Email invalide");
        }
    }
}
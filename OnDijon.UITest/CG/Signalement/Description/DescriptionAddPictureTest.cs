using NUnit.Framework;
using OnDijon.UITest.Utils;
using System;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.CG
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class DescriptionAddPictureTests
    {
        IApp app;
        Platform platform;

        public DescriptionAddPictureTests(Platform platform)
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
        public void DescriptionAddPictureTest()
        {
            FastAccess.Description(app);

            app.WaitForNoElement("TakePhotoButton");
            app.WaitForNoElement("GaleryButton");

            app.Tap("AddPhotoButton");

            //Sélection de la bonne adresse? ?
            AppResult[] TakePhotoButtonResults = app.WaitForElement("TakePhotoButton");
            Assert.IsTrue(TakePhotoButtonResults.Any());
            AppResult[] GaleryResults = app.WaitForElement("GaleryButton");
            Assert.IsTrue(GaleryResults.Any());
        }
    }
}

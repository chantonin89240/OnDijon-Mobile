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
    public class LocalisationMovementMapTests
    {
        IApp app;
        Platform platform;

        public LocalisationMovementMapTests(Platform platform)
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
        public void LocalisationMovementMapTest()
        {
            FastAccess.Localisation(app);

            AppResult[] Map = app.WaitForElement("Map");

            app.DragCoordinates(Map[0].Rect.CenterX, Map[0].Rect.CenterY, Map[0].Rect.CenterX+200, Map[0].Rect.CenterY);

            app.DragCoordinates(Map[0].Rect.CenterX, Map[0].Rect.CenterY, Map[0].Rect.CenterX, Map[0].Rect.CenterY+100);

            app.DragCoordinates(Map[0].Rect.CenterX, Map[0].Rect.CenterY, Map[0].Rect.CenterX-100, Map[0].Rect.CenterY);

            app.DragCoordinates(Map[0].Rect.CenterX, Map[0].Rect.CenterY, Map[0].Rect.CenterX, Map[0].Rect.CenterY-200);

            app.DragCoordinates(Map[0].Rect.CenterX, Map[0].Rect.CenterY, Map[0].Rect.CenterX-100, Map[0].Rect.CenterY);

            app.DragCoordinates(Map[0].Rect.CenterX, Map[0].Rect.CenterY, Map[0].Rect.CenterX, Map[0].Rect.CenterY+100);

            app.TapCoordinates(Map[0].Rect.CenterX, Map[0].Rect.CenterY);

            //Retour à l'adresse de départ ?
            app.WaitForElement(query => query.Text("40 Avenue du Drapeau, 21000, Dijon"));
            AppResult[] LocalisationMovementMapResults = app.WaitForElement("SearchInput");
            Assert.AreEqual(LocalisationMovementMapResults[0].Text,"40 Avenue du Drapeau, 21000, Dijon");
        }
    }
}

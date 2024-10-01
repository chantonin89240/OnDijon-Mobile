using Esri.ArcGISRuntime.Geometry;
using NUnit.Framework;
using OnDijon.CG.Services.Interfaces;
using OnDijon.UnitTest.Utils.Helpers;
using System;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.Services.Tests
{
    class ReportServiceTests
    {
        private IReportService _reportService;

        [SetUp]
        public void Setup()
        {
            //init IOC container
            var locator = new Locator();

            //get services
            _reportService = locator.GetInstance<IReportService>();
        }

        [Test]
        public async Task GetReportTypesTest()
        {
            var response = await _reportService.GetReportTypes();
            Console.WriteLine(response);

            Assert.IsTrue(response.IsSuccessful());
            Assert.IsNotEmpty(response.ReportTypes);

            foreach (var type in response.ReportTypes)
            {
                Assert.IsNotNull(type.Id);
                AssertHelper.IsNotNullAndNotEmpty(type.Code);
                AssertHelper.IsNotNullAndNotEmpty(type.Name);
                AssertHelper.IsNotNullAndNotEmpty(type.ImageKey);
            }
        }

        [TestCase("parc")]
        [TestCase("21000")]
        [TestCase("Avenue du Drapeau")]
        [TestCase("Avenue du Drapeau, 21000")]
        [TestCase("Avenue du Drapeau, 21000, Dijon")]
        public async Task GetSuggestionsTest(string address)
        {
            var response = await _reportService.GetSuggestions(address);

            Assert.IsTrue(response.IsSuccessful());
            Assert.IsNotEmpty(response.Suggestions);
            Assert.LessOrEqual(response.Suggestions.Count, 5);

            foreach (var suggestion in response.Suggestions)
            {
                AssertHelper.IsNotNullAndNotEmpty(suggestion);
            }
        }

        [TestCase(5.041415, 47.321276, 4326, ExpectedResult = "15 Place de la Liberation, 21000, Dijon")]
        [TestCase(854180, 6693209, 2154, ExpectedResult = "15 Place de la Liberation, 21000, Dijon")]
        public async Task<string> GetAddressFromCoordinatesTest(double longitude, double latitude, int srID)
        {
            var spatialRef = SpatialReference.Create(srID);
            var coordinates = new MapPoint(longitude, latitude, spatialRef);

            var response = await _reportService.GetAddressFromCoordinates(coordinates);

            Assert.IsTrue(response.IsSuccessful());

            return response.Address;
        }

        [TestCase("15 Place de la Liberation, Dijon", 5.0413764461413315, 47.320949523511629)]
        [TestCase("15 Place de la Liberation, 21000", 5.0413764461413315, 47.320949523511629)]
        [TestCase("15 Place de la Liberation, 21000, Dijon", 5.0413764461413315, 47.320949523511629)]
        public async Task GetCoordinatesFromAddressTest(string address, double expectedX, double expectedY)
        {
            var response = await _reportService.GetCoordinatesFromAddress(address);

            Assert.IsTrue(response.IsSuccessful());
            Assert.AreEqual(expectedX, response.X);
            Assert.AreEqual(expectedY, response.Y);
        }
    }
}

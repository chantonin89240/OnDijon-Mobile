using Esri.ArcGISRuntime.Geometry;
using NUnit.Framework;
using OnDijon.CG.Entities.Dto.Reporting;
using OnDijon.CG.Entities.Request.Reporting;
using OnDijon.CG.Services;
using OnDijon.CG.Services.Interfaces;
using OnDijon.CG.Utils.UI;
using OnDijon.CG.ViewModels.Reporting;
using OnDijon.UnitTest.CG.Services.Mocks;
using OnDijon.UnitTest.Common.Services.Mocks;
using OnDijon.UnitTest.Utils;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.ViewModels.Tests
{
    class ReportLocalisationViewModelTests
    {
        private NavigationMockService _navigationService;
        private ISession _session;
        private ReportLocalisationViewModel _reportLocalisationVM;

        private void InitViewModel()
        {
            _navigationService = new NavigationMockService();
            _session = new Session(new CacheMockService());
            _reportLocalisationVM = new ReportLocalisationViewModel(_navigationService, new TranslationService(), new PopupMockService(), _session, new ReportMockService(), new GeolocationMockService(), new UserIdMockService());
        }

        [SetUp]
        public void Setup()
        {
            InitViewModel();
            _session.ReportRequest = new ReportRequest { ReportTypeCode = "Test" };
        }

        [Test]
        public void ConstructorTest()
        {
            InitViewModel();
            Assert.IsNotNull(_reportLocalisationVM.GetCurrentLocationCommand);
            Assert.IsNotNull(_reportLocalisationVM.GetReportsCommand);
            Assert.IsNotNull(_reportLocalisationVM.GoToReportCommand);
            Assert.IsNotNull(_reportLocalisationVM.GoToNextPageCommand);
        }

        [Test]
        public void InitMapTest()
        {
            _reportLocalisationVM.InitMap();
            Assert.IsNotNull(_reportLocalisationVM.Map);
        }

        [Test]
        public async Task GetSuggestionsTest()
        {
            bool propertyChanged = await _reportLocalisationVM.WaitForPropertyChanged(nameof(_reportLocalisationVM.Suggestions),
                () => _reportLocalisationVM.GetSuggestions("test"));
            Assert.IsTrue(propertyChanged);
        }

        [Test]
        public async Task GetCoordinatesFromAddressTest()
        {
            var properties = new string[] { nameof(_reportLocalisationVM.Address), nameof(_reportLocalisationVM.CurrentPosition) };
            bool propertiesChanged = await _reportLocalisationVM.WaitForPropertiesChanged(properties,
                () => _reportLocalisationVM.GetCoordinatesFromAddress("test"));
            Assert.IsTrue(propertiesChanged);
            Assert.IsNotNull(_session.ReportRequest.Address);
            Assert.IsNotNull(_session.ReportRequest.Position);
        }

        [Test]
        public async Task GetAddressFromCoordinatesTest()
        {
            bool propertyChanged = await _reportLocalisationVM.WaitForPropertyChanged(nameof(_reportLocalisationVM.Address),
                () => _reportLocalisationVM.GetAddressFromCoordinates(MapUtils.VALID_AREA.GetCenter()));
            Assert.IsTrue(propertyChanged);
            Assert.IsNotNull(_session.ReportRequest.Address);
        }

        [Test]
        public async Task GetCurrentLocationTest()
        {
            bool propertyChanged = await _reportLocalisationVM.WaitForPropertyChanged(nameof(_reportLocalisationVM.CurrentPosition),
                () => _reportLocalisationVM.GetCurrentLocationCommand.Execute(null));
            Assert.IsTrue(propertyChanged);
            Assert.IsNotNull(_session.ReportRequest.Position);
        }

        [Test]
        public async Task GetReportsTest()
        {
            bool propertyChanged = await _reportLocalisationVM.WaitForPropertyChanged(nameof(_reportLocalisationVM.Reports),
                () => _reportLocalisationVM.GetReportsCommand.Execute(new MapPoint(0, 0)));
            Assert.IsTrue(propertyChanged);
        }

        [Test]
        public async Task GoToReportTest()
        {
            _navigationService.Configure(Locator.ReportDetailView, typeof(MockPageWithParam<ReportDto>));
            _reportLocalisationVM.GoToReportCommand.Execute(new ReportDto());
            await Task.Delay(100);
            Assert.AreEqual(Locator.ReportDetailView, _navigationService.CurrentPageKey);
        }

        [Test]
        public async Task GoToNextPageTest()
        {
            _navigationService.Configure(Locator.ReportDescriptionView, typeof(MockPage));
            _reportLocalisationVM.GoToNextPageCommand.Execute(null);
            await Task.Delay(100);
            Assert.AreEqual(Locator.ReportDescriptionView, _navigationService.CurrentPageKey);
        }

        [Test]
        public void CleanupTest()
        {
            InitMapTest();
            _reportLocalisationVM.Cleanup();
            Assert.IsNull(_reportLocalisationVM.Map);
        }
    }
}

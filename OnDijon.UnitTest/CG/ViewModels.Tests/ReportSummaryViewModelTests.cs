using NUnit.Framework;
using OnDijon.CG.Entities.Request.Reporting;
using OnDijon.CG.Services;
using OnDijon.CG.Services.Interfaces;
using OnDijon.CG.ViewModels.Reporting;
using OnDijon.UnitTest.CG.Services.Mocks;
using OnDijon.UnitTest.Common.Services.Mocks;
using OnDijon.UnitTest.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.ViewModels.Tests
{
    class ReportSummaryViewModelTests
    {
        private NavigationMockService _navigationService;
        private PopupMockService _popupService;
        private ISession _session;
        private ReportSummaryViewModel _reportSummaryVM;

        private void InitViewModel()
        {
            _navigationService = new NavigationMockService();
            _popupService = new PopupMockService();
            _session = new Session(new CacheMockService());
            _reportSummaryVM = new ReportSummaryViewModel(_navigationService, new TranslationService(), _popupService, _session, new ReportMockService(), new UserIdMockService());
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
            Assert.IsNotNull(_reportSummaryVM.SendReportCommand);
        }

        [Test]
        public async Task UpdatePhotoSourceTest()
        {
            Assert.IsFalse(_reportSummaryVM.PhotoAvailable);
            Assert.IsNull(_reportSummaryVM.PhotoSource);

            _session.ReportRequest.Photos = new List<byte[]>() { new byte[0] };

            var properties = new string[] { nameof(_reportSummaryVM.PhotoAvailable), nameof(_reportSummaryVM.PhotoSource) };
            bool propertiesChanged = await _reportSummaryVM.WaitForPropertiesChanged(properties,
                () => _reportSummaryVM.UpdatePhotoSource());
            Assert.IsTrue(propertiesChanged);

            Assert.IsTrue(_reportSummaryVM.PhotoAvailable);
            Assert.IsNotNull(_reportSummaryVM.PhotoSource);
        }

        [Test]
        public async Task SendReportTest()
        {
            _navigationService.Configure(Locator.ReportSuccessView, typeof(MockPage));
            _reportSummaryVM.SendReportCommand.Execute(null);

            await Task.Delay(100);

            Assert.AreEqual(Locator.ReportSuccessView, _navigationService.CurrentPageKey);
            Assert.IsNull(_reportSummaryVM.PhotoSource);
            Assert.IsNull(_session.ReportRequest);
        }

        [Test]
        public async Task CleanupTest()
        {
            await UpdatePhotoSourceTest();
            _reportSummaryVM.Cleanup();
            Assert.IsNull(_reportSummaryVM.PhotoSource);
            Assert.IsNull(_session.ReportRequest);
        }
    }
}

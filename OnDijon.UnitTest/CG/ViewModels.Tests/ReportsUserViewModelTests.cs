using NUnit.Framework;
using OnDijon.CG.Entities.Dto.Reporting;
using OnDijon.CG.Services;
using OnDijon.CG.ViewModels.Reporting;
using OnDijon.UnitTest.CG.Services.Mocks;
using OnDijon.UnitTest.Common.Services.Mocks;
using OnDijon.UnitTest.Utils;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.ViewModels.Tests
{
    class ReportsUserViewModelTests
    {
        private NavigationMockService _navigationService;
        private ReportsUserViewModel _reportsUserVM;

        private void InitViewModel()
        {
            _navigationService = new NavigationMockService();
            _reportsUserVM = new ReportsUserViewModel(_navigationService, new TranslationService(), new PopupMockService(), new ReportMockService(), new UserIdMockService());
        }

        [SetUp]
        public void Setup()
        {
            InitViewModel();
        }

        [Test]
        public void ConstructorTest()
        {
            InitViewModel();
            Assert.IsNotNull(_reportsUserVM.GetReportsCommand);
            Assert.IsNotNull(_reportsUserVM.GoToReportCommand);
        }

        [Test]
        public void InitMapTest()
        {
            _reportsUserVM.InitMap();
            Assert.IsNotNull(_reportsUserVM.Map);
        }

        [Test]
        public async Task GetReportsTest()
        {
            bool propertyChanged = await _reportsUserVM.WaitForPropertyChanged(nameof(_reportsUserVM.Reports),
                () => _reportsUserVM.GetReportsCommand.Execute(null));
            Assert.IsTrue(propertyChanged);
        }

        [Test]
        public async Task GoToReportTest()
        {
            _navigationService.Configure(Locator.ReportDetailView, typeof(MockPageWithParam<ReportDto>));
            _reportsUserVM.GoToReportCommand.Execute(new ReportDto());
            await Task.Delay(100);
            Assert.AreEqual(Locator.ReportDetailView, _navigationService.CurrentPageKey);
        }

        [Test]
        public void CleanupTest()
        {
            InitMapTest();
            _reportsUserVM.Cleanup();
            Assert.IsNull(_reportsUserVM.Map);
        }
    }
}

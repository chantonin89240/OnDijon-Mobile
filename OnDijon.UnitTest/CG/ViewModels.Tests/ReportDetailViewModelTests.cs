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
    class ReportDetailViewModelTests
    {
        private NavigationMockService _navigationService;
        private PopupMockService _popupService;
        private ReportDetailViewModel _reportDetailVM;

        private void InitViewModel()
        {
            _navigationService = new NavigationMockService();
            _popupService = new PopupMockService();
            _reportDetailVM = new ReportDetailViewModel(_navigationService, new TranslationService(), _popupService, new ReportMockService(), new UserIdMockService());
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
            Assert.IsNotNull(_reportDetailVM.SubscribeCommand);
        }

        [Test]
        public async Task GetReportTest()
        {
            var properties = new string[] { nameof(_reportDetailVM.Report), nameof(_reportDetailVM.PhotoAvailable) };
            bool propertiesChanged = await _reportDetailVM.WaitForPropertiesChanged(properties,
                () => _reportDetailVM.GetReport("test"));
            Assert.IsTrue(propertiesChanged);
        }

        [Test]
        public async Task SubscribeToReportTest()
        {
            _navigationService.Configure(Locator.DashboardView, typeof(MockPage));
            _reportDetailVM.Report = new ReportDto { Id = 0 };
            _reportDetailVM.SubscribeCommand.Execute(null);
            await Task.Delay(100);
            _popupService.ConfirmButtonAction.Invoke();
            await Task.Delay(100);

            Assert.AreEqual(Locator.DashboardView, _navigationService.CurrentPageKey);
        }
    }
}

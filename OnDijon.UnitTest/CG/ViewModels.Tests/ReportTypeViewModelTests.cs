using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using OnDijon.CG.Entities.Dto.Reporting;
using OnDijon.CG.Services;
using OnDijon.CG.Services.Interfaces;
using OnDijon.CG.ViewModels.Reporting;
using OnDijon.UnitTest.CG.Services.Mocks;
using OnDijon.UnitTest.Common.Services.Mocks;
using OnDijon.UnitTest.Utils;

namespace OnDijon.UnitTest.CG.ViewModels.Tests
{
    class ReportTypeViewModelTests
    {
        private NavigationMockService _navigationService;
        private ISession _session;
        private ReportTypeViewModel _reportTypeVM;

        private void InitViewModel()
        {
            _navigationService = new NavigationMockService();
            _session = new Session(new CacheMockService());
            _reportTypeVM = new ReportTypeViewModel(_navigationService, new TranslationService(), new PopupMockService(), new ReportMockService(), _session);
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

            Assert.IsNotNull(_reportTypeVM.GoToNextPageCommand);
        }

        [Test]
        public async Task QueryReportTypesTest()
        {
            bool propertyChanged = await _reportTypeVM.WaitForPropertyChanged(nameof(_reportTypeVM.ReportTypes),
                () => _reportTypeVM.QueryReportTypes());
            Assert.IsTrue(propertyChanged);
        }

        [Test]
        public async Task GoToNextPageTest()
        {
            _navigationService.Configure(Locator.ReportLocalisationView, typeof(MockPage));
            _reportTypeVM.GoToNextPageCommand.Execute(new ReportTypeDto());
            await Task.Delay(100);
            Assert.AreEqual(Locator.ReportLocalisationView, _navigationService.CurrentPageKey);
        }

        [Test]
        public void CleanupTest()
        {
            _reportTypeVM.ReportTypes = new List<ReportTypeDto>();
            _reportTypeVM.Cleanup();
            Assert.IsNull(_reportTypeVM.ReportTypes);
        }
    }
}

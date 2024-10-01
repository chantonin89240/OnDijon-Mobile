using NUnit.Framework;
using OnDijon.CG.Entities.Request.Reporting;
using OnDijon.CG.Services;
using OnDijon.CG.Services.Interfaces;
using OnDijon.CG.ViewModels.Reporting;
using OnDijon.UnitTest.CG.Services.Mocks;
using OnDijon.UnitTest.Common.Services.Mocks;
using OnDijon.UnitTest.Utils;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OnDijon.UnitTest.CG.ViewModels.Tests
{
    class ReportDescriptionViewModelTests
    {
        private NavigationMockService _navigationService;
        private ISession _session;
        private ReportDescriptionViewModel _reportDescriptionVM;

        private void InitViewModel()
        {
            _navigationService = new NavigationMockService();
            _session = new Session(new CacheMockService());
            _reportDescriptionVM = new ReportDescriptionViewModel(_navigationService, new TranslationService(), new PopupMockService(), _session, new PhotoMockService());
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
            Assert.IsNotNull(_reportDescriptionVM.AddPhotoCommand);
            Assert.IsNotNull(_reportDescriptionVM.RemovePhotoCommand);
            Assert.IsNotNull(_reportDescriptionVM.GoToNextPageCommand);
        }

        [Test]
        public void SetDescriptionTest()
        {
            _reportDescriptionVM.Description = "test";
            Assert.AreEqual(_reportDescriptionVM.Description, _session.ReportRequest.Description);
        }

        [Test]
        public async Task AddPhotoTest()
        {
            Assert.IsTrue(_reportDescriptionVM.CanAddPhoto);

            bool propertyChanged = await _reportDescriptionVM.WaitForPropertyChanged(nameof(_reportDescriptionVM.PhotoSources),
                () => _reportDescriptionVM.AddPhotoCommand.Execute(null));
            Assert.IsTrue(propertyChanged);

            Assert.IsFalse(_reportDescriptionVM.CanAddPhoto);
            Assert.IsNotNull(_session.ReportRequest.Photos);
            Assert.IsNotEmpty(_session.ReportRequest.Photos);
        }

        [Test]
        public async Task RemovePhotoTest()
        {
            await AddPhotoTest();

            var mockImageSource = new StreamImageSource { StyleId = "0" };

            bool propertyChanged = await _reportDescriptionVM.WaitForPropertyChanged(nameof(_reportDescriptionVM.PhotoSources),
                () => _reportDescriptionVM.RemovePhotoCommand.Execute(mockImageSource));
            Assert.IsTrue(propertyChanged);

            Assert.IsTrue(_reportDescriptionVM.CanAddPhoto);
            Assert.IsEmpty(_session.ReportRequest.Photos);
        }

        [Test]
        public async Task GoToNextPageTest()
        {
            _navigationService.Configure(Locator.ReportSummaryView, typeof(MockPage));
            _reportDescriptionVM.GoToNextPageCommand.Execute(null);
            await Task.Delay(100);
            Assert.AreEqual(Locator.ReportSummaryView, _navigationService.CurrentPageKey);
        }

        [Test]
        public async Task CleanupTest()
        {
            await AddPhotoTest();
            _reportDescriptionVM.Cleanup();
            Assert.IsNull(_reportDescriptionVM.PhotoSources);
        }
    }
}

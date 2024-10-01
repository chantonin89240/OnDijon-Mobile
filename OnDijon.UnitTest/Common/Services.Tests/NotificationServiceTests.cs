using NUnit.Framework;
using OnDijon.Common.Notifications.Services;
using OnDijon.Common.Notifications.Services.Interfaces;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.UnitTest.CG.Services.Mocks;
using OnDijon.UnitTest.Common.Services.Mocks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.Common.Services.Tests
{
    class NotificationServiceTests
    {
        private NavigationMockService _navigationService;
        private INotificationService _notificationService;

        [SetUp]
        public void Setup()
        {
            //init IOC container
            var locator = new Locator();

            //init mock navigation service
            _navigationService = new NavigationMockService();
            _navigationService.Configure(Locator.ReportDetailView, typeof(MockPageWithParam<string>));

            //get services
            _notificationService = new NotificationService(_navigationService, locator.GetInstance<IHttpService>(), new UserIdMockService());
        }

        [TestCase("SIGNALEMENT", "123456", ExpectedResult = Locator.ReportDetailView)]
        [TestCase("SIGNALEMENT", null, ExpectedResult = null)]
        [TestCase("INVALID", null, ExpectedResult = null)]
        public async Task<string> RelayWithServiceIdAndItemIdTest(string serviceId, string itemId)
        {
            var notificationData = new Dictionary<string, object>
            {
                { "serviceId", serviceId },
                { "itemId", itemId }
            };
            _notificationService.Relay(notificationData);

            await Task.Delay(100);

            //check that itemId is passed to the page
            if (_navigationService.CurrentPage != null)
            {
                var currentPage = _navigationService.CurrentPage as MockPageWithParam<string>;
                Assert.AreEqual(itemId, currentPage.Parameter);
            }

            return _navigationService.CurrentPageKey;
        }

        [TestCase("SIGNALEMENT", ExpectedResult = null)]
        [TestCase("INVALID", ExpectedResult = null)]
        public string RelayWithServiceIdTest(string serviceId)
        {
            var notificationData = new Dictionary<string, object>
            {
                { "serviceId", serviceId }
            };
            _notificationService.Relay(notificationData);

            return _navigationService.CurrentPageKey;
        }
    }
}

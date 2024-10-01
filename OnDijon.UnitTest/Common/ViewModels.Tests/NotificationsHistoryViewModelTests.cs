using NUnit.Framework;
using OnDijon.CG.Services;
using OnDijon.Common.Notifications.Entities.Dto;
using OnDijon.Common.Notifications.ViewModels;
using OnDijon.UnitTest.CG.Services.Mocks;
using OnDijon.UnitTest.Common.Services.Mocks;
using OnDijon.UnitTest.Utils;
using System;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.Common.ViewModels.Tests
{
    class NotificationsHistoryViewModelTests
    {
        private NotificationsHistoryViewModel _notificationsHistoryVM;

        private void InitViewModel()
        {
            _notificationsHistoryVM = new NotificationsHistoryViewModel(new NavigationMockService(), new TranslationService(), new PopupMockService(), new NotificationMockService());
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
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(_notificationsHistoryVM.GetNotificationsCommand);
                Assert.IsNotNull(_notificationsHistoryVM.RelayNotificationCommand);
            });
        }

        [Test]
        public async Task GetNotificationsTest()
        {
            bool propertyChanged = await _notificationsHistoryVM.WaitForPropertyChanged(nameof(_notificationsHistoryVM.Notifications),
                () => _notificationsHistoryVM.GetNotificationsCommand.Execute(null));
            Assert.IsTrue(propertyChanged);
        }

        [Test]
        public void RelayNotificationTest()
        {
            var notif = new NotificationDto { Id = 1, Title = "Title", Body = "Body", ServiceId = "TEST", ItemId = "1", DateSent = DateTime.Now, IsRead = false };
            Assert.DoesNotThrow(() =>
            {
                _notificationsHistoryVM.RelayNotificationCommand.Execute(notif);
            });
        }
    }
}

using OnDijon.CG.Enums;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Notifications.Entities.Dto;
using OnDijon.Common.Notifications.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.Common.Services.Mocks
{
    class NotificationMockService : INotificationService
    {
        public async Task<DtoListResponse<NotificationDto>> GetNotifications()
        {
            Console.WriteLine("NotificationMockService: GetNotifications()");
            await Task.Delay(100);

            var notif1 = new NotificationDto { Id = 1, Title = "Title 1", Body = "Body 1", ServiceId = "SIGNALEMENT", ItemId = "1", DateSent = DateTime.Now, IsRead = false };
            var notif2 = new NotificationDto { Id = 2, Title = "Title 2", Body = "Body 2", ServiceId = "SIGNALEMENT", ItemId = "2", DateSent = DateTime.Now, IsRead = false };
            var notif3 = new NotificationDto { Id = 3, Title = "Title 3", Body = "Body 3", ServiceId = "SIGNALEMENT", ItemId = "3", DateSent = DateTime.Now, IsRead = false };

            var data = new NotificationDto[] { notif1, notif2, notif3 }.ToList();
            return new DtoListResponse<NotificationDto> { State = CallStatusEnum.Success, Data = data };
        }

        public async Task MarkAsRead(int notificationId, bool isRead = true)
        {
            Console.WriteLine($"NotificationMockService: MarkAsRead({notificationId}, {isRead})");
            await Task.Delay(100);
        }

        public void Relay(IDictionary<string, object> notificationData)
        {
            Console.WriteLine($"NotificationMockService: Relay({string.Join(", ", notificationData)})");
        }
    }
}

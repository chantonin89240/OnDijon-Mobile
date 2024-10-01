using System.Collections.Generic;
using System.Threading.Tasks;
using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Notifications.Entities.Dto;
using OnDijon.Modules.Notifications.Entities.Response;

namespace OnDijon.Modules.Notifications.Services.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        /// Relay notification data to the target service
        /// </summary>
        void Relay(IDictionary<string, object> notificationData);

        /// <summary>
        /// Get all notifications sent to this device
        /// </summary>
        Task<NotificationListResponse> GetNotifications();

        Task<NotificationListDto> GetNotificationsAsync();


        /// <summary>
        /// Get the number of notifications sent to this device not read yet
        /// </summary>
        Task<NotificationCountResponse> GetNotificationCount();

        Task<NotificationCountDto> GetNotificationCountAsync();

        /// <summary>
        /// Mark notification as read
        /// </summary>
        Task<Response> MarkAsRead(int notificationId, bool isRead = true);
        Task<WsDMDto> MarkAsReadAsync(int notificationId, bool isRead = true);


        /// <summary>
        /// Init Firebase subscription and events
        /// </summary>
        void InitFirebase();

        /// <summary>
        /// Get Firebase token to subscribe to notifications
        /// </summary>
        /// <returns></returns>
        string GetFirebaseToken();
    }
}

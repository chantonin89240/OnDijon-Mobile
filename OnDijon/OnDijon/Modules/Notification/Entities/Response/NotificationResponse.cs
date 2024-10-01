using OnDijon.Modules.Notifications.Entities.Models;

namespace OnDijon.Modules.Notifications.Entities.Response
{
    public class NotificationResponse : Common.Entities.Response.Response
    {
        public NotificationModel Notification { get; set; }
    }
}

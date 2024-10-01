using OnDijon.Modules.Notifications.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.Notifications.Entities.Response
{
    public class NotificationListResponse : Common.Entities.Response.Response
    {
        public List<NotificationModel> Notifications { get; set; }
    }
}

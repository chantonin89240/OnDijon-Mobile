using System;
using System.Collections.Generic;

namespace OnDijon.Modules.Notifications.Entities.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string ServiceId { get; set; }

        public string ItemId { get; set; }

        public DateTime DateSent { get; set; }

        public bool IsRead { get; set; }
    }

    public class NotificationListModel
    {
        public IList<NotificationModel> NotificationsList { get; set; }
    }
}


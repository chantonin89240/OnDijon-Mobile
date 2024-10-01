using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using System;
using System.Collections.Generic;

namespace OnDijon.Modules.Notifications.Entities.Dto
{
    public class NotificationDto : Common.Entities.Dto.Dto
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "Body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "ServiceId")]
        public string ServiceId { get; set; }

        [JsonProperty(PropertyName = "ItemId")]
        public string ItemId { get; set; }

        [JsonProperty(PropertyName = "DateSent")]
        public DateTime DateSent { get; set; }

        [JsonProperty(PropertyName = "IsRead")]
        public bool IsRead { get; set; }
    }

    public class NotificationListDto : WsDMDto
    {
        [JsonProperty(PropertyName = "Notifications")]
        public IList<NotificationDto> NotificationsList { get; set; }
    }
    
    public class NotificationCountDto : WsDMDto
    {
        [JsonProperty(PropertyName = "NotificationsCount")]
        public int NotificationsCount { get; set; }
    }
}

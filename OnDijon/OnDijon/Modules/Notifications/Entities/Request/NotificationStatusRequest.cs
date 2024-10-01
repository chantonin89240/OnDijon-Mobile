using Newtonsoft.Json;
using OnDijon.Common.Entities.Request;

namespace OnDijon.Modules.Notifications.Entities.Request
{
    public class NotificationStatusRequest : DtoRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }
        
        [JsonProperty(PropertyName = "NotificationId")]
        public int NotificationId { get; set; }

        [JsonProperty(PropertyName = "IsRead")]
        public bool IsRead { get; set; }
        
        [JsonProperty(PropertyName = "ProfileEditId")]
        public string ProfileEditId { get; set; }
    }
}

using Newtonsoft.Json;
using OnDijon.Common.Entities.Request;

namespace OnDijon.Modules.Notifications.Entities.Request
{
    public class NotificationRequest : DtoRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("ProfileEditId")]
        public string ProfileEditId { get; set; }
        [JsonProperty("RegistrationToken")]
        public string RegistrationToken { get; set; }
    }
}

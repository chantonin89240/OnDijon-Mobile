using Newtonsoft.Json;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class ChangePasswordRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "EditId")]
        public string EditId { get; set; }

        [JsonProperty(PropertyName = "OldPassword")]
        public string OldPassword { get; set; }

        [JsonProperty(PropertyName = "NewPassword")]
        public string NewPassword { get; set; }
    }
}

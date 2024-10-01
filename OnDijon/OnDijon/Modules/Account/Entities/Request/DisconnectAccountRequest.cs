using Newtonsoft.Json;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class DisconnectAccountRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "EditId")]
        public string UserEditId { get; set; }

        [JsonProperty(PropertyName = "DeviceId")]
        public string DeviceId { get; set; }
    }
}

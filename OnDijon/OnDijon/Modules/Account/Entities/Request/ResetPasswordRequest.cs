using Newtonsoft.Json;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class ResetPasswordRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "Mail")]
        public string Mail { get; set; }
    }
}

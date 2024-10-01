using Newtonsoft.Json;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class AuthenticationRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "UserEditId")]
        public string UserEditId { get; set; }

        [JsonProperty(PropertyName = "Mail")]
        public string Mail { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }
    }
}

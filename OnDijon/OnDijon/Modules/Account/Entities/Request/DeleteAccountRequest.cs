using Newtonsoft.Json;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class DeleteAccountRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "EditId")]
        public string UserEditId { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }
    }
}

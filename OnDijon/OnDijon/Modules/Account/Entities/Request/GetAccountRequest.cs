using Newtonsoft.Json;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class GetAccountRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "Profile")]
        public ProfilRequest Profile { get; set; }
    }

    public class ProfilRequest
    {
        [JsonProperty(PropertyName = "EditId")]
        public string Guid { get; set; }
    }
}

using Newtonsoft.Json;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class CguRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("ProfileEditId")]
        public string ProfileEditId { get; set; }
    }
}

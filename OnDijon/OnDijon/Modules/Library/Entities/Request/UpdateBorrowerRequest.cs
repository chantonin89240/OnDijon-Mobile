using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
    public class UpdateBorrowerRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("BorrowerEditId")]
        public string BorrowerEditId { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

    }
}

using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
    public class AccountBorrowerRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("BorrowerEditId")]
        public string BorrowerEditId { get; set; }

    }
}

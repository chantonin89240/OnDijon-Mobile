using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
    public class DissociateReaderAccountRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }

    }
}

using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
    public class AssociateReaderAccountRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("EditId")]
        public string EditId { get; set; }

        [JsonProperty("Login")]
        public string Login { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

    }
}

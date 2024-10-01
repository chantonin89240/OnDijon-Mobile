using Newtonsoft.Json;
using OnDijon.Modules.Account.Entities.Models;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class UpdateRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "UserEditId")]
        public string UserEditId { get; set; }

        [JsonProperty(PropertyName = "Profile")]
        public ProfileModel Profile { get; set; }
    }
}

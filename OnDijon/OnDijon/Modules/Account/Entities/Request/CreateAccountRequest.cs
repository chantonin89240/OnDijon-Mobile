using Newtonsoft.Json;
using OnDijon.Modules.Account.Entities.Models;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class CreateAccountRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }


        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }


        [JsonProperty(PropertyName = "Profile")]
        public ProfileModel Profile { get; set; }
    }
}

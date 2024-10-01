using Newtonsoft.Json;
using OnDijon.Modules.Account.Entities.Models;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class UpdateMobileRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "Profile")]
        public ProfileModel Profile { get; set; }

        [JsonProperty(PropertyName = "MobileRegistration")]
        public MobileRegistrationRequest MobileRegistration { get; set; }
    }

    public class MobileRegistrationRequest
    {

        [JsonProperty(PropertyName = "RegistrationToken")]
        public string RegistrationToken { get; set; }

        [JsonProperty(PropertyName = "DeviceId")]
        public string DeviceId { get; set; }
    }
}

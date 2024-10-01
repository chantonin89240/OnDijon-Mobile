using Newtonsoft.Json;

namespace OnDijon.Modules.Account.Entities.Dto
{
    public class MobileRegistrationDto
    {
        [JsonProperty(PropertyName = "RegistrationToken")]
        public string RegistrationToken { get; set; }

        [JsonProperty(PropertyName = "DeviceId")]
        public string DeviceId { get; set; }
    }
}

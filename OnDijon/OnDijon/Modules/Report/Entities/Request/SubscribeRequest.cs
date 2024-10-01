using Newtonsoft.Json;
using OnDijon.Common.Entities.Request;

namespace OnDijon.Modules.Report.Entities.Request
{
    public class SubscribeRequest : DtoRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "ProfileEditId")]
        public string ProfileEditId { get; set; }

        [JsonProperty(PropertyName = "ReportId")]
        public int ReportId { get; set; }

        [JsonProperty(PropertyName = "RegistrationToken")]
        public string RegistrationToken { get; set; }
    }
}

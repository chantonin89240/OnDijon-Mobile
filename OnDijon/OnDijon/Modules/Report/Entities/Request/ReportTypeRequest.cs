using Newtonsoft.Json;
using OnDijon.Common.Entities.Request;

namespace OnDijon.Modules.Report.Entities.Request
{
    class ReportTypeRequest : DtoRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("ProfileEditId")]
        public string ProfileEditId { get; set; }
    }
}

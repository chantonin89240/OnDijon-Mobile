using Newtonsoft.Json;
using OnDijon.Common.Entities.Request;

namespace OnDijon.Modules.Report.Entities.Request
{
    public class ReportGetRequest : DtoRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("UserId")]
        public string UserId { get; set; }
        [JsonProperty("ReportId")]
        public int ReportId { get; set; }
    }
}

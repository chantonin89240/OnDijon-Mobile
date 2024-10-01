using Newtonsoft.Json;
using OnDijon.Common.Entities.Request;

namespace OnDijon.Modules.Report.Entities.Request
{
    class ReportsListRequest : DtoRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("UserId")]
        public string UserId { get; set; }
    }
}

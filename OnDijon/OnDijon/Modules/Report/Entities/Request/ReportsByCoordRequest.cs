using Newtonsoft.Json;

namespace OnDijon.Modules.Report.Entities.Request
{
    class ReportsByCoordRequest : ReportsListRequest
    {
        [JsonProperty("Longitude")]
        public string Longitude { get; set; }

        [JsonProperty("Latitude")]
        public string Latitude { get; set; }
    }
}

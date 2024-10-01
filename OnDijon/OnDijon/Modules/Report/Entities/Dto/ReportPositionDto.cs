using Newtonsoft.Json;

namespace OnDijon.Modules.Report.Entities.Dto
{
    public class ReportPositionDto : Common.Entities.Dto.Dto
    {
        [JsonProperty(PropertyName = "lon")]
        public double X { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public double Y { get; set; }
    }
}

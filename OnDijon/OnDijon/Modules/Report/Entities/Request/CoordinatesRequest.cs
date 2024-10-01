using Newtonsoft.Json;

namespace OnDijon.Modules.Report.Entities.Request
{
    public class CoordinatesRequest
    {
        [JsonProperty(PropertyName = "Lon")]
        public double X { get; set; }

        [JsonProperty(PropertyName = "Lat")]
        public double Y { get; set; }
    }
}

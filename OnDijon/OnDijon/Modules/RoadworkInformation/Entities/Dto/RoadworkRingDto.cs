using Newtonsoft.Json;

namespace OnDijon.Modules.RoadworkInformation.Entities.Dto
{
    public class RoadworkRingDto
    {
        [JsonProperty("Longitude")]
        public double Longitude;
        [JsonProperty("Latitude")]
        public double Latitude;
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;

namespace OnDijon.Modules.RoadworkInformation.Entities.Dto
{
    public class RoadworkInfoDto
    {
        [JsonProperty("EditId")]
        public string EditId;
        [JsonProperty("Title")]
        public string Title;
        [JsonProperty("Executant")]
        public string Executant;
        [JsonProperty("Applicant")]
        public string Applicant;
        [JsonProperty("ObjectType")]
        public string ObjectType;
        [JsonProperty("DateBeginRoadwork")]
        public string DateBeginRoadwork;
        [JsonProperty("DateEndRoadwork")]
        public string DateEndRoadwork;
        [JsonProperty("Latitude")]
        public double X;
        [JsonProperty("Longitude")]
        public double Y;
        [JsonProperty("State")]
        public string State;
        [JsonProperty("Area")]
        public List<RoadworkRingDto> Area;
    }
}

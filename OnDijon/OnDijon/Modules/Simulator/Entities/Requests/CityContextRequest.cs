using Newtonsoft.Json;

namespace OnDijon.Modules.Simulator.Entities.Requests
{
    public class CityContextRequest
    {
        [JsonProperty("Key")]
        public string Key;
        [JsonProperty("ServiceId")]
        public string ServiceId;
    }
}
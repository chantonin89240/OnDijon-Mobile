using Newtonsoft.Json;

namespace OnDijon.Modules.Dashboard.Entities.Dto
{
    public class WorkDataDto
    {
        [JsonProperty("Count")]
        public int Count;
        [JsonProperty("State")]
        public string State;
    }
}

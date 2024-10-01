using Newtonsoft.Json;


namespace OnDijon.Modules.Strike.Entities.DTO
{
    public class NurseryStrikeInfoDto
    {
        [JsonProperty("Id")]
        public string EditId;
        [JsonProperty("Name")]
        public string Name;
        [JsonProperty("Comment")]
        public string Detail;
    }
}

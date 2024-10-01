using Newtonsoft.Json;

namespace OnDijon.Modules.Dashboard.Entities.Dto.Card
{
    public class CardTagDto : Common.Entities.Dto.Dto
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
    }
}

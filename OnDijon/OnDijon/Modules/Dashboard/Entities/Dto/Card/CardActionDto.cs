using Newtonsoft.Json;

namespace OnDijon.Modules.Dashboard.Entities.Dto.Card
{
    public class CardActionDto : Common.Entities.Dto.Dto
    {
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "Target")]
        public string Target { get; set; }

        [JsonProperty(PropertyName = "Parameter")]
        public string Parameter { get; set; }
    }
}

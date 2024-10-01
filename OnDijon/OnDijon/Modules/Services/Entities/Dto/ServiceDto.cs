using Newtonsoft.Json;

namespace OnDijon.Modules.Services.Entities.Dto
{
    public class ServiceDto : Common.Entities.Dto.Dto
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("ItemCode")]
        public string Code { get; set; }

        [JsonProperty("Favorite")]
        public virtual bool IsFavourite { get; set; }

        [JsonProperty("Picture")]
        public string Icon { get; set; }

        [JsonProperty("MaintenanceMessage")]
        public string MaintenanceMessage { get; set; }

        [JsonProperty("RequiredConnexion")]
        public bool IsRequiredConnection { get; set; }

        [JsonProperty("Visibility")]
        public string Visibility { get; set; }
    }
}

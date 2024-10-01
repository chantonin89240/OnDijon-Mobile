using System.Collections.Generic;
using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Services.Entities.Dto
{
    public class FavoriteServiceListDto : WsDMDto
    {
        [JsonProperty("Favorites")]
        public IList<ServiceDto> Services { get; set; }
        [JsonProperty("ScopeFavorites")]
        public List<Scope> Scopes { get; set; }
        public bool HasAlertIdentity { get; set; }

    }

    public class Scope
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Favorite")]
        public bool Checked { get; set; }
    }
}

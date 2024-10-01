using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Favorites.Entities.Dto
{
    public class ProfilDto : Common.Entities.Dto.Dto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        
        [JsonProperty("Nom")]
        public string Nom { get; set; }

        [JsonProperty("Prenom")]
        public string Prenom { get; set; }

        [JsonProperty("Guid")]
        public string Guid { get; set; }

    }
}

using Newtonsoft.Json;

namespace OnDijon.Modules.Favorites.Entities.Dto
{
    public class AdressDto : Common.Entities.Dto.Dto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        
        [JsonProperty("ProfilId")]
        public int ProfilId { get; set; }
        
        [JsonProperty("Latitude")]
        public double Latitude { get; set; }
        
        [JsonProperty("Longitude")]
        public double Longitude { get; set; }

        [JsonProperty("CodePostal")]
        public int CodePostal { get; set; }

        [JsonProperty("Ville")]
        public string Ville { get; set; }

        [JsonProperty("Rue")]
        public string Rue { get; set; }

        [JsonProperty("Pays")]
        public string Pays { get; set; }
    }
}

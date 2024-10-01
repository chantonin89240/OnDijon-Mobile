using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Abris.Entities.Dto
{
    public class AbrisDto : Common.Entities.Dto.Dto
    {
        [JsonProperty("DataSetId")]
        public string DataSetId { get; set; }

        [JsonProperty("RecordId")]
        public string RecordId { get; set; }

        [JsonProperty("Extensible")]
        public string Extensible { get; set; }

        [JsonProperty("Quartier")]
        public string Quartier { get; set; }

        [JsonProperty("Nom")]
        public string Nom { get; set; }

        [JsonProperty("Aire")]
        public int Aire { get; set; }

        [JsonProperty("GeoPointLat")]
        public double GeoPointLat { get; set; }

        [JsonProperty("GeoPointLon")]
        public double GeoPointLon { get; set; }

        [JsonProperty("NbPlaces")]
        public int NbPlaces { get; set; }

        [JsonProperty("NbPlacesInitial")]
        public int NbPlacesInitial { get; set; }

        [JsonProperty("CodComm")]
        public string CodComm { get; set; }
    }
}

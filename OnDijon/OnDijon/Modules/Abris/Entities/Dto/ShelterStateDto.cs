using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Abris.Entities.Dto
{
    public class ShelterStateDto : Common.Entities.Dto.Dto
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("IdAbris")]
        public string IdAbris { get; set; }

        [JsonProperty("DateTimeRefresh")]
        public string DateTimeRefresh { get; set; }

        [JsonProperty("Available")]
        public string Available { get; set; }
    }
}

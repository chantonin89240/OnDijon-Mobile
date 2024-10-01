using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Entities.Request;
using OnDijon.Common.Utils.Converters;
using System;
using System.Collections.Generic;

namespace OnDijon.Modules.Account.Entities.Dto
{
    public class ProfileAzureDto : Common.Entities.Dto.Dto
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Nom")]
        public string Nom { get; set; }

        [JsonProperty(PropertyName = "Prenom")]
        public string Prenom { get; set; }

        [JsonProperty(PropertyName = "Guid")]
        public string Guid { get; set; }
    }
}

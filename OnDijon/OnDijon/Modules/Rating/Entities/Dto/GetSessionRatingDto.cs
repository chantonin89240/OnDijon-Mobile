using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using System;

namespace OnDijon.Modules.Rating.Entities.Dto
{
    public class GetSessionRatingDto : WsDMDto
    {
        [JsonProperty(PropertyName = "EditId")]
        public string EditId { get; set; }
        [JsonProperty(PropertyName = "PublicationDate")]
        public DateTime? PublicationDate { get; set; }
        [JsonProperty(PropertyName = "BeginDatePublication")]
        public DateTime? BeginDatePublication { get; set; }
        [JsonProperty(PropertyName = "EndDatePublication")]
        public DateTime? EndDatePublication { get; set; }
        [JsonProperty(PropertyName = "NumberVisitDashboard")]
        public string NumberVisitDashboard { get; set; }
        [JsonProperty(PropertyName = "Incrementation")]
        public string Incrementation { get; set; }
        [JsonProperty(PropertyName = "HasSession")]
        public bool HasSession { get; set; }
    }
}

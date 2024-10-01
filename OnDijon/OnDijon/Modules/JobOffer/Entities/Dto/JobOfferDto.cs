using System;
using Newtonsoft.Json;

namespace OnDijon.Modules.JobOffer.Entities.Dto
{
    public class JobOfferDto
    {
        [JsonProperty("EditId")]
        public string EditId;

        [JsonProperty("Title")]
        public string Title;

        [JsonProperty("SubTitle")]
        public string Subtitle;

        [JsonProperty("EndPublicationDate")]
        public DateTime LimitDate;

        [JsonProperty("Picture")]
        public string Picture;

        [JsonProperty("Contents")]
        public string Content;

        [JsonProperty("Terms")]
        public string Conditions;

        [JsonProperty("Type")]
        public string Type;

        [JsonProperty("City")]
        public string City;
    }
}

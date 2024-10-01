using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OnDijon.Modules.JobOffer.Entities.Dto
{
    public class ListJobOfferDto : WsDMDto
    {
        [JsonProperty("externalJobOffer")]
        public List<JobOfferDto> JobOffers;
    }
}

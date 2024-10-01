using OnDijon.Common.Entities.Response;
using OnDijon.Modules.JobOffer.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.JobOffer.Entities.Responses
{
    public class ListJobOfferResponse : Response
    {
        public List<JobOfferModel> JobOfferList { get; set; }
    }
}

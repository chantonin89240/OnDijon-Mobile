using OnDijon.Common.Entities.Response;
using System.Collections.Generic;


namespace OnDijon.Modules.JobOffer.Entities.Responses
{
    public class ListTypeJobOfferResponse : Response
    {
        public List<string> TypeJobOfferList { get; set; }
    }
}

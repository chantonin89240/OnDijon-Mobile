
using OnDijon.Common.Entities.Response;
using OnDijon.Modules.UsefulContact.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.UsefulContact.Entities.Responses
{
    public class OpeningTimeResponse : Response
    {
        public string EditId { get; set; }
        public List<ContactOpeningPeriodModel> OpeningTime { get; set; }
    }
}

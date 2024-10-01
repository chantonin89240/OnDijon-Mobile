using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Booking.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.Booking.Entities.Responses
{
    public class InstitutionListResponse : Response
    {
        public List<InstitutionModel> Institutions { get; set; }
        public string SessionEditId { get; set; }
    }
}

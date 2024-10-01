using OnDijon.Modules.Library.Entities.Dto.Model;
using OnDijon.Modules.Library.Entities.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Response
{
    public class BorrowerInformationResponse : Common.Entities.Response.Response
    {
        public string ImageUri { get; set; }

        public UserInformation UserInformation { get; set; }

        public List<LoanDto> Loans { get; set; }
      
        public List<ReservationDto> Reservations { get; set; }
    }
}
 
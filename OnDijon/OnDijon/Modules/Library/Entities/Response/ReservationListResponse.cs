using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Library.Entities.Dto.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Response
{
    public class ReservationListResponse : DMResponse
    {
        public List<ReservationDto> ReservationList { get; set; }
    }
}
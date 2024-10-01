using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Booking.Entities.Dto
{
    public class InstitutionListDto : WsDMDto
    {
        public List<InstitutionDto> Institutions { get; set; }
        public string SessionEditId { get; set; }
    }
}

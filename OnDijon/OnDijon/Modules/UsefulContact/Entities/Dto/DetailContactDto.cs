using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.UsefulContact.Entities.Dto
{
    public class DetailContactDto : WsDMDto
    {
        public string EditId;
        public List<ContactOpeningPeriodDto> OpeningTime;
    }
}

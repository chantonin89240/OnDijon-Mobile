using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Booking.Entities.Dto
{
    public class ScheduleListDto : WsDMDto
    {
        public List<ScheduleDto> Schedules;
    }
}

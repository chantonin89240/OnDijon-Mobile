using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.WedAlsh.Entities.Dto
{
    public class WedAlshSchedulesDto : WsDMDto
    {
        public List<WedAlshScheduleDto> Schedules { get; set; }
    }
}

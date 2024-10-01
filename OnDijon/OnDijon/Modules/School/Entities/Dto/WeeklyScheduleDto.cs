using OnDijon.Common.Entities.Dto;
using System;
using System.Collections.Generic;

namespace OnDijon.Modules.School.Entities.Dto
{
    public class WeeklyScheduleDto : WsDMDto
    {
        public DateTime? StartDate;
        public DateTime? EndDate;
        public string EditId;
        public IEnumerable<CalendarActivityDto> Schedule;
    }

    public class CalendarActivityDto
    {
        public string ActivityEditId;
        public string ActivityTitle;
        public string ActivityDay;
        public string ActivityCode;
        public string EditId;
        public int? Order;
        public bool IsCheck;
    }
}

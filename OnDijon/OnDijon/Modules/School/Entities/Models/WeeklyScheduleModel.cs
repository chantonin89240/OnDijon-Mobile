using System;
using System.Collections.Generic;

namespace OnDijon.Modules.School.Entities.Models
{
    public class WeeklyScheduleModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EditId { get; set; }
        public IEnumerable<CalendarActivityModel> Schedule;
    }
}

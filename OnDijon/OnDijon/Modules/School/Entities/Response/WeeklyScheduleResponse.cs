using System;
using System.Collections.Generic;
using OnDijon.Modules.School.Entities.Models;

namespace OnDijon.Modules.School.Entities.Response
{
    public class WeeklyScheduleResponse : Common.Entities.Response.Response
    {
        public DateTime? StartDate;
        public DateTime? EndDate;
        public string EditId;
        public string ChildEditId;
        public IEnumerable<CalendarActivityModel> Schedule;
    }
}

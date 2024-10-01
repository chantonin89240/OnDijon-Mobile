
using System;
using System.Collections.Generic;

namespace OnDijon.Modules.School.Entities.Models
{
    public class AppointmentWeekModel
    {
        public string Title { get; set; }
        public DateTime FirstWeekDay { get; set; }
        public DateTime LastWeekDay { get; set; }
        public IList<AppointmentModel> Appointments { get; set; }
    }
}

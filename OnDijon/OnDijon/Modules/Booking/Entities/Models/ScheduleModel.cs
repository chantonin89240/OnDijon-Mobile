using System;
using System.Collections.Generic;

namespace OnDijon.Modules.Booking.Entities.Models
{
    public class ScheduleModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EditId { get; set; }
    }

    public class ViewScheduleModel
    {
        public DateTime Day { get; set; }
        public IEnumerable<ScheduleModel> ScheduleModels { get; set; }
    }
}

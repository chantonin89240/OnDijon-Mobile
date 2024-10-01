using System;

namespace OnDijon.Modules.WedAlsh.Entities.Dto
{
    public class WedAlshScheduleDto
    {
        public string ScheduleType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EditId { get; set; }
        public string State { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsClosed { get; set; }
        public bool IsBooked { get; set; }
        public string IsClosedLabel { get; set; }
    }
}

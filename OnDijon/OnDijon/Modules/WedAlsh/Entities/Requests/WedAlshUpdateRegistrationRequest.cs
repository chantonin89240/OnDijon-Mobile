using System.Collections.Generic;

namespace OnDijon.Modules.WedAlsh.Entities.Request
{
    public class WedAlshUpdateRegistrationRequest
    {
        public string Key { get; set; }
        public string RegistrationEditId { get; set; }
        public List<ScheduleAction> SchedulesChoice { get; set; }
    }

    public class ScheduleAction
    {
        public string EditId { get; set; }
        public bool Checked { get; set; }
    }
}

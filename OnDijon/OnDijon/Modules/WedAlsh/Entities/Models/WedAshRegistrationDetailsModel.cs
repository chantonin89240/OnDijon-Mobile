using System.Collections.Generic;

namespace OnDijon.Modules.WedAlsh.Entities.Models
{
    public class WedAshRegistrationDetailsModel
    {
        public string EditId { get; set; }
        public WedAlshRecreationCenterModel CentreAccueil { get; set; }
        public List<WedAlshScheduleModel> Schedules { get; set; }
    }
}

using System.Collections.Generic;

namespace OnDijon.Modules.WedAlsh.Entities.Dto
{
    public class WedAlshRegistrationDetailsDto
    {
        public string EditId { get; set; }
        public WedAlshRecreationCenterDto CentreAccueil { get; set; }
        public List<WedAlshScheduleDto> Schedules { get; set; }
    }
}

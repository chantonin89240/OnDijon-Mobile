using OnDijon.Modules.School.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.School.Entities.Request
{
    public class AppointmentsUpdateByCityContextRequest
    {
        public string Key;
        public List<AppointmentUpdateDto> Appointments;
        public string CityContext;
    }
}

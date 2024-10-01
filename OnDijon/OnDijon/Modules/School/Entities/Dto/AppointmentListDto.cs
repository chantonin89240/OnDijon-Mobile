using System.Collections.Generic;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.School.Entities.Dto { 
    public class AppointmentListDto : WsDMDto
    {
        public string childEditId { get; set; }
        public List<AppointmentDto> Appointments { get; set; }
    }
}

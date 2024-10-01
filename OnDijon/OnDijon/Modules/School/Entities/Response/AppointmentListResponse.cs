using System.Collections.Generic;
using OnDijon.Modules.School.Entities.Models;

namespace OnDijon.Modules.School.Entities.Response
{
    public class AppointmentListResponse : Common.Entities.Response.Response
    {
        public string childEditId { get; set; }
        public IList<AppointmentModel> AppointmentList { get; set; }
    }
}

using OnDijon.Modules.WedAlsh.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.WedAlsh.Entities.Response
{
    public class WedAlshSchedulesResponse : Common.Entities.Response.Response
    {
        public List<WedAlshScheduleModel> Schedules { get; set; }
    }
}

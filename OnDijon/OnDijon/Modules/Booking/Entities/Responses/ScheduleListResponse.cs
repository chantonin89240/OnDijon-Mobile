using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Booking.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.Booking.Entities.Responses
{
    public class ScheduleListResponse : Response
    {
        public List<ScheduleModel> Schedules { get; set; }
    }
}

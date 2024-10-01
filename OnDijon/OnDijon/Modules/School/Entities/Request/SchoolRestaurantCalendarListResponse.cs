using System.Collections.Generic;
using OnDijon.Modules.School.Models;

namespace OnDijon.Modules.School.Entities.Response
{
    public class SchoolRestaurantCalendarListResponse : Common.Entities.Response.Response
    {
        public IList<SchoolRestaurantCalendar> SchoolRestaurantCalendarList { get; set; }
    }
}

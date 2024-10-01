using OnDijon.Modules.Diary.Entities.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Diary.Entities.Response
{
    public class EventListResponse : Common.Entities.Response.Response
    {
        public List<EventModel> Events { get; set; }
        public int NumberOfResults { get; set; }
        public int Page { get; set; }
        public int PageMax { get; set; }
    }
}

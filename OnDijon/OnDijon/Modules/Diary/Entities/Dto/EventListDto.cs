using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.Diary.Entities.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Diary.Entities.Dto
{
    public class EventListDto : WsDMDto
    {
        public List<EventModel> Events { get; set; }
        public int NumberOfResults { get; set; }
        public int Page { get; set; }
        public int PageMax { get; set; }
    }
}

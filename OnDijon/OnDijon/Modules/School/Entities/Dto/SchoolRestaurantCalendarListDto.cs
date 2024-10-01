using System.Collections.Generic;
using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.School.Entities.Dto
{
    public class SchoolRestaurantCalendarListDto : WsDMDto
    {
        [JsonProperty(PropertyName = "Menus")]
        public IList<SchoolRestaurantCalendarDto> SchoolRestaurantCalendarDays { get; set; }
        
    }
}

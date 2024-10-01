using Newtonsoft.Json;
using OnDijon.Common.Utils.Converters;
using System;
using System.Collections.Generic;

namespace OnDijon.Modules.School.Entities.Request
{
    public class UpdateWeeklyScheduleByCityContextRequest
    {
        public string Key;
        [JsonProperty(PropertyName = "StartDate")]
        [JsonConverter(typeof(DateTimeJsonConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime? StartDate;
        [JsonProperty(PropertyName = "EndDate")]
        [JsonConverter(typeof(DateTimeJsonConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime? EndDate;
        public string EditId;
        public IEnumerable<CalendarActivityRequest> PlannedSchedule;
        public string CityContext;
    }

    public class CalendarActivityRequest
    {
        public string EditId;
        public bool IsCheck;
    }
}

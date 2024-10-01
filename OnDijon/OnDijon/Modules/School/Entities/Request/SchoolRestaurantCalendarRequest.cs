using Newtonsoft.Json;
using System;

namespace OnDijon.Modules.School.Request
{
    public class SchoolRestaurantCalendarRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("BeginningDate")]
        public DateTime BeginningDate { get; set; }
        [JsonProperty("EndingDate")]
        public DateTime EndingDate { get; set; }

    }
}

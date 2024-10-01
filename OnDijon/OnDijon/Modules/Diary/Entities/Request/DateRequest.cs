using Newtonsoft.Json;
using System;

namespace OnDijon.Modules.Diary.Entities.Request
{
    public class DateRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("StartDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("EndDate")]
        public DateTime EndDate { get; set; }
        [JsonProperty("PageNumber")]
        public int PageNumber { get; set; }
        [JsonProperty("ResultSize")]
        public int ResultSize { get; set; }
    }
}

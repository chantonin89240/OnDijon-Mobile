using Newtonsoft.Json;
using System;

namespace OnDijon.Modules.Diary.Entities.Request
{
    public class RequestRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("Request")]
        public string Request { get; set; }
        [JsonProperty("PageNumber")]
        public int PageNumber { get; set; }
        [JsonProperty("ResultSize")]
        public int ResultSize { get; set; }
        [JsonProperty("StartDate")]
        public DateTime StartDate { get; set; }
    }
}

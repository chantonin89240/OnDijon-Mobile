using Newtonsoft.Json;
using System;

namespace OnDijon.Modules.Diary.Entities.Request
{
    public class SuggestDiaryRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("Request")]
        public string Request { get; set; }
        [JsonProperty("StartDate")]
        public DateTime StartDate { get; set; }
    }
}

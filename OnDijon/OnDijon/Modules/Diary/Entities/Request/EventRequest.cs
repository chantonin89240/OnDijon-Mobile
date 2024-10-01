using Newtonsoft.Json;

namespace OnDijon.Modules.Diary.Entities.Request
{
    public class EventRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("EventEditId")]
        public string EventEditId { get; set; }
    }
}

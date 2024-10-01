using Newtonsoft.Json;

namespace OnDijon.Modules.Rating.Entities.Request
{
    public class PostSessionRatingRequest
    {
        [JsonProperty("Key")]
        public string Key;
        [JsonProperty("EditIdUser")]
        public string EditIdUser;
        [JsonProperty("EditIdSession")]
        public string EditIdSession;
        [JsonProperty("Note")]
        public string Note;
        [JsonProperty("Comment")]
        public string Comment;
    }
}

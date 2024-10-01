using Newtonsoft.Json;

namespace OnDijon.Modules.Strike.Entities.DTO
{
    public class SchoolStrikeInfoDto
    {
        [JsonProperty("Id")]
        public string EditId;
        [JsonProperty("Name")]
        public string Name;
        [JsonProperty("Closed")]
        public string SchoolStatus;
        [JsonProperty("MorningExtracurricular")]
        public string MorningExtracurricular;
        [JsonProperty("NoomReception")]
        public string NoonReception;
        [JsonProperty("SchoolRestaurant")]
        public string SchoolRestaurant;
        [JsonProperty("TAP")]
        public string TAP;
        [JsonProperty("EveningExtracurricular")]
        public string EveningExtracurricular;
        [JsonProperty("Comment")]
        public string Comment;
        [JsonProperty("EducationLevel")]
        public string EducationLevel;

    }
}

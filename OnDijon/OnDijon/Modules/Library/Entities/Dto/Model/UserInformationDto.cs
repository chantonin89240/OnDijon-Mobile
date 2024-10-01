using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Dto.Model
{
    public class UserIdentityInformationDto
    {
        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }
        [JsonProperty("BarCode")]
        public string BarCode { get; set; }
        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }
        [JsonProperty("Gender")]
        public string Gender { get; set; }       


    }


}

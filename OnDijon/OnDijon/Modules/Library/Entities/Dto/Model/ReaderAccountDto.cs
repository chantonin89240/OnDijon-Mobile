using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Dto.Model
{
    public class ReaderAccountDto
    {
        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }

        [JsonProperty("Civility")]
        public string Civility { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("Uid")]
        public string Uid { get; set; }

        [JsonProperty("BarCode")]
        public string BarCode { get; set; }
    }
}

using Newtonsoft.Json;
using System;

namespace OnDijon.Modules.Library.Entities.Model
{
    public class UserInformation
    {
        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("Age")]
        public string Age { get; set; }

        [JsonProperty("AccountExpires")]
        public DateTime AccountExpires { get; set; }

        [JsonProperty("BarCode")]
        public string BarCode { get; set; }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("Gender")]
        public string Gender { get; set; }

        [JsonProperty("HomePostalAddress")]
        public string HomePostalAddress { get; set; }

        [JsonProperty("Uid")]
        public string Uid { get; set; }
    }
}

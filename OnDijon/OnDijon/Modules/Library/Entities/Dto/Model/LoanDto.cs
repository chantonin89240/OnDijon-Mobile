using Newtonsoft.Json;
using System;

namespace OnDijon.Modules.Library.Entities.Dto.Model
{
    public class LoanDto
    {
        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("Author")]
        public string Author { get; set; }

        [JsonProperty("CanRenew")]
        public bool CanRenew { get; set; }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Isbn")]
        public string Isbn { get; set; }

        [JsonProperty("IsLate")]
        public bool IsLate { get; set; }

        [JsonProperty("IsSoonLate")]
        public bool IsSoonLate { get; set; }

        [JsonProperty("LoanDate")]
        public DateTime LoanDate { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("Publisher")]
        public string Publisher { get; set; }

        [JsonProperty("ReturnDate")]
        public DateTime ReturnDate { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("TypeOfDocument")]
        public string TypeOfDocument { get; set; }

        [JsonProperty("WhyCannotRenew")]
        public string WhyCannotRenew { get; set; }

        [JsonProperty("RecordId")]
        public string RecordId { get; set; }
    }
}

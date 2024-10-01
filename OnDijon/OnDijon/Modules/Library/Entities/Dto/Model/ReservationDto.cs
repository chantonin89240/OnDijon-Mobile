using Newtonsoft.Json;
using System;

namespace OnDijon.Modules.Library.Entities.Dto.Model
{
    public class ReservationDto
    {
        [JsonProperty("Author")]
        public string Author { get; set; }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("HoldingPlace")]
        public string HoldingPlace { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Isbn")]
        public string Isbn { get; set; }

        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }

        [JsonProperty("LocationLabel")]
        public string LocationLabel { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Publisher")]
        public string Publisher { get; set; }

        [JsonProperty("Rank")]
        public string Rank { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("TypeOfDocument")]
        public object TypeOfDocument { get; set; }

        [JsonProperty("WhenAvailableEnd")]
        public DateTime WhenAvailableEnd { get; set; }

        [JsonProperty("WhenAvailableStart")]
        public DateTime WhenAvailableStart { get; set; }

        [JsonProperty("WhenCreated")]
        public DateTime WhenCreated { get; set; }
        [JsonProperty("RecordId")]
        public string RecordId { get; set; }
    }
}

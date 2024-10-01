using Newtonsoft.Json;
using OnDijon.Modules.Library.Entities.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Dto.Model
{
    public class BorrowerDto
    {
        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }
        [JsonProperty("BarCode")]
        public string BarCode { get; set; }
        [JsonProperty("Loans")]
        public List<LoanDto> Loans { get; set; }
        [JsonProperty("Reservations")]
        public List<ReservationDto> Reservations { get; set; }
        //public List<SubscriptionBean> Subscriptions { get; set; }
        [JsonProperty("UserInformation")]
        public UserInformation UserInformation { get; set; }

    }
}

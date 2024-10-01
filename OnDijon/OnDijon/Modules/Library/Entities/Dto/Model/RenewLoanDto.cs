using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Dto.Model
{
    public class RenewLoanDto 
    {
        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("IsRenew")]
        public bool IsRenewed { get; set; }

        [JsonProperty("LoanId")]
        public string LoanId { get; set; }

        [JsonProperty("NotRenewedReason")]
        public string NotRenewedReason { get; set; }

        [JsonProperty("Loan")]
        public LoanDto Loan { get; set; }
    }
}

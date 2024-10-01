using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
   public class RenewLoanRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("EditId")]
        public string EditId { get; set; }

        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }

        [JsonProperty("IdLoan")]
        public string IdLoan { get; set; }

        [JsonProperty("IdRecord")]
        public string IdRecord { get; set; }
        
    }
}

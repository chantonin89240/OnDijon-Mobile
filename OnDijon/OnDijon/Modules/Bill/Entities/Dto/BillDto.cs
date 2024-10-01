using Newtonsoft.Json;

namespace OnDijon.Modules.Bill.Entities.Dto
{
    public class BillDto
    {
        [JsonProperty("Title")]
        public string Title;
        [JsonProperty("Number")]
        public string Number;
        [JsonProperty("LevyDate")]
        public string LevyDate;
        [JsonProperty("ToPay")]
        public string ToPay;
        [JsonProperty("State")]
        public string State;
        [JsonProperty("EditId")]
        public string EditId;
        [JsonProperty("FamilyNumber")]
        public string FamilyNumber;
        [JsonProperty("DematBill")]
        public string DematBill;
        [JsonProperty("TipiReference")]
        public string TipiReference;
        [JsonProperty("LevyType")]
        public string LevyType;
        [JsonProperty("PayLink")]
        public string PayLink;
        [JsonProperty("DownloadLink")]
        public string DownloadLink;
    }
}

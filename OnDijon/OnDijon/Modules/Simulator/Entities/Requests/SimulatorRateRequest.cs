using Newtonsoft.Json;

namespace OnDijon.Modules.Simulator.Entities.Requests
{
    public class SimulatorRateRequest
    {
        [JsonProperty("Key")]
        public string Key;
        [JsonProperty("ChildNumber")]
        public int ChildNumber;
        [JsonProperty("AnnualIncome")]
        public decimal AnnualIncome;
        [JsonProperty("QF")]
        public decimal QF;
        [JsonProperty("Resident")]
        public bool Resident;
        [JsonProperty("CityContextId")]
        public string CityContextId;
    }
}

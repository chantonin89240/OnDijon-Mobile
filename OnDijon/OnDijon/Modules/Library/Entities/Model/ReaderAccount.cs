using Newtonsoft.Json;
using Xamarin.Forms;

namespace OnDijon.Modules.Library.Entities.Model
{
    public class ReaderAccount
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

        [JsonIgnore]
        public string FullName
        {
            get
            {
                return FirstName + " " + Name.Substring(0, 1) + ".";
            }
        }

        [JsonIgnore]
        public BmCardType TypeAccount { get; set; }


        [JsonIgnore]
        public ImageSource ImageSource { get; set; }
        [JsonIgnore]
        public string  ImageUri { get; set; }
        [JsonIgnore]
        public string Color { get; internal set; }
    }

    public enum BmCardType
    {
        Borrower,
        NewCard

    }
}

using Newtonsoft.Json;
using OnDijon.Common.Utils.Converters;
using System;

namespace OnDijon.Modules.Account.Entities.Dto
{
    public class ProfileDto : Common.Entities.Dto.Dto
    {
        [JsonProperty(PropertyName = "EditId")]
        public string Guid { get; set; }

        [JsonProperty(PropertyName = "FirstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "Gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "Mail")]
        public string Mail { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "FixPhoneNumber")]
        public string FixPhoneNumber { get; set; }

        [JsonProperty(PropertyName = "Birthday")]
        [JsonConverter(typeof(DateTimeJsonConverter), "dd/MM/yyyy")]
        public DateTime? Birthday { get; set; }

        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "PostalCode")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "Street")]
        public string Street { get; set; }

        [JsonProperty(PropertyName = "StreetNumber")]
        public string StreetNumber { get; set; }

        [JsonProperty(PropertyName = "StreetNumberComplement")]
        public string StreetNumberComplement { get; set; }

        [JsonProperty(PropertyName = "AddressComplement")]
        public string AddressComplement { get; set; }

        [JsonProperty(PropertyName = "Scope")]
        public string Scope { get; set; }
    }
}

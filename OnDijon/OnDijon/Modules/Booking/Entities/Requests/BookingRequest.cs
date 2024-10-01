using Newtonsoft.Json;
using OnDijon.Common.Utils.Converters;
using System;
using System.Collections.Generic;

namespace OnDijon.Modules.Booking.Entities.Requests
{
    public class BookingRequest
    {
        public string Key { get; set; }
        public string DocumentCivility { get; set; }
        public string DocumentName { get; set; }
        public string DocumentFirstName { get; set; }
        [JsonProperty(PropertyName = "DocumentBirthDate")]
        [JsonConverter(typeof(DateTimeJsonConverter), "yyyy-MM-dd")]
        public DateTime? DocumentBirthDate { get; set; }
        public string ApplicantCivility { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantPhone { get; set; }
        public string ApplicantAddressNumber { get; set; }
        public string ApplicantAddressStreet { get; set; }
        public string ApplicantAddressPostalCode { get; set; }
        public string ApplicantAddressCity { get; set; }
        public IEnumerable<string> RequestReason { get; set; }
        public string UserId { get; set; }
        public string ConfigurationSite { get; set; }
        public string SessionEditId { get; set; }
        public string ScheduleEditId { get; set; }
        public int NbOfPerson { get; set; }
    }
}

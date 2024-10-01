using Newtonsoft.Json;
using OnDijon.Common.Utils.Converters;
using System;

namespace OnDijon.Modules.Booking.Entities.Requests
{
    public class ScheduleRequest
    {
        public string Key;
        [JsonProperty(PropertyName = "StartDate")]
        [JsonConverter(typeof(DateTimeJsonConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime? StartDate;
        [JsonProperty(PropertyName = "EndDate")]
        [JsonConverter(typeof(DateTimeJsonConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime? EndDate;
        public string SessionEditId;
        public string InstitutionEditId;
        public int NbOfPerson;
    }
}

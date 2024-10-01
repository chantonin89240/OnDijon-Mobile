using Newtonsoft.Json;
using OnDijon.Modules.UsefulContact.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.UsefulContact.Entities.Dto
{
    public class ContactInfosDto
    {
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public string PictureUrl { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public List<ContactOpeningPeriodModel> OpeningTime { get; set; }
    }
}

using OnDijon.Common.Entities.Response;
using OnDijon.Modules.UsefulContact.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.UsefulContact.Entities.Responses
{
    public class ContactResponse : Response
    {
        public string EditId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public string PictureUrl { get; set; }
        public List<ContactOpeningPeriodModel> OpeningTime { get; set; }
    }
}

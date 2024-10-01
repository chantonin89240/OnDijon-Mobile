using System.Collections.Generic;

namespace OnDijon.Modules.UsefulContact.Entities.Models
{
    public class ContactInfosModel
    {
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public string PictureUrl { get; set; }
        public string Description { get; set; }
        public List<ContactOpeningPeriodModel> OpeningTime { get; set; }
    }
}

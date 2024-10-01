using System;

namespace OnDijon.Modules.JobOffer.Entities.Models
{
    public class JobOfferModel 
    {
        public string EditId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public DateTime LimitDate { get; set; }
        public string Picture { get; set; }
        public string Content { get; set; }
        public string Conditions { get; set; }
        public string Type { get; set; }
        public string City { get; set; }

    }

}

using System;

namespace OnDijon.Modules.Rating.Entities.Response
{
    public class GetSessionRatingResponse : Common.Entities.Response.Response
    {
        public string EditId { get; set; }
        public DateTime? PublicationDate { get; set; }
        public DateTime? BeginDatePublication { get; set; }
        public DateTime? EndDatePublication { get; set; }
        public int NumberVisitDashboard { get; set; }
        public int Incrementation { get; set; }
        public bool HasSession { get; set; }
    }
}

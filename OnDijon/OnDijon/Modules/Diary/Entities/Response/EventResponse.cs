using System;
using System.Collections.Generic;

namespace OnDijon.Modules.Diary.Entities.Response
{
    public class EventResponse : Common.Entities.Response.Response
    {
        public string Title { get; set; }
        public string EditId { get; set; }
        public string Image { get; set; }
        public string ImageThumbnail { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PricingInfo { get; set; }
        public string District { get; set; }
        public string InfoLink { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string DiaryEditId { get; set; }
        public string DiaryName { get; set; }
        public IEnumerable<string> Scope { get; set; }
    }
}

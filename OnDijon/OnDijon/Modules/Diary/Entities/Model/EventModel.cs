using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OnDijon.Modules.Diary.Entities.Model
{
    public class EventModel
    {
        public string Title { get; set; }
        public string EditId { get; set; }
        public string Image { get; set; }
        public string ImageThumbnail { get; set; }
        //A enlever dés que possible
        public string ImageThumbnailCorrect { get { return !string.IsNullOrEmpty(ImageThumbnail) && ImageThumbnail.Contains("http://cibul.s3.amazonaws.com/evtb") ? "http://cibul.s3.amazonaws.com/" + ImageThumbnail.Substring(34) : ImageThumbnail; }}
        public string Summary { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string TagsString { get { return (Tags != null && Tags.Any() ? String.Join(", ", Tags).TrimEnd(','): "") + (!string.IsNullOrEmpty(District) ? ", " + District : ""); } }
        public string Location { get; set; }
        public DateTime? StartDate { get; set; }
        public string DateString { get { return StartDate?.ToString("dddd d MMMM yyyy", CultureInfo.CreateSpecificCulture("fr-FR")) + ((((DateTime)StartDate).Hour + ((DateTime)StartDate).Minute) > 0 ? StartDate?.ToString("\" à\" HH:mm", CultureInfo.CreateSpecificCulture("fr-FR")) : ""); } }
        public string DateShortString { 
            get 
            { 
                if(EndDate != null && ((DateTime)EndDate).Day != ((DateTime)StartDate).Day)
                {
                    return "Du " + StartDate?.ToString("dd\"/\"MM")+ " au " + EndDate?.ToString("dd\"/\"MM");
                }
                else
                {
                    return StartDate?.ToString("dd\"/\"MM"); 
                }
            } 
        }
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
        public bool IsFirstOfDate { get; set; } = false;
        public string FirstOfDateString { get; set; }
    }
}

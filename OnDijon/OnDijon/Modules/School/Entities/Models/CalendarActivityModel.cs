
namespace OnDijon.Modules.School.Entities.Models
{
    public class CalendarActivityModel
    {
        public string ActivityEditId { get; set; }
        public string ActivityTitle { get; set; }
        public string ActivityDay { get; set; }
        public string ActivityCode { get; set; }
        public string EditId { get; set; }
        public int? Order { get; set; }
        public bool IsCheck { get; set; }
    }
}

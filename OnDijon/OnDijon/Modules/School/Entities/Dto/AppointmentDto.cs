using System;

namespace OnDijon.Modules.School.Entities.Dto { 
    public class AppointmentDto
    {
        public string ActivityEditId { get; set; }
        public string ActivityTitle { get; set; }
        public DateTime Date { get; set; }
        public string CalendarEditId { get; set; }
        public string RegistrationEditId { get; set; }
        public bool Scheduled { get; set; }
        public bool IsClosed { get; set; }
        public string ActivityCode { get; set; }
        public string SpecialDayLabel { get; set; }



    }

   
}

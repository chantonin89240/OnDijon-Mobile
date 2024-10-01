using OnDijon.Common.Entities.Response;

namespace OnDijon.Modules.Booking.Entities.Responses
{
    public class BookingInformationsResponse : Response
    {
        public string Civility { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Institution { get; set; }
        public string Day { get; set; }
        public string NbOfPerson { get; set; }
    }
}

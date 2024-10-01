namespace OnDijon.Modules.Booking.Entities.Models
{
    public class BookingInformationsModel
    {
        public string Civility { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Institution { get; set; }
        public string Day { get; set; }
        public string NbOfPerson { get; set; }
        public string FullCivility { get => string.Format("{0} {1} {2}", Civility, Name, FirstName); }
    }
}

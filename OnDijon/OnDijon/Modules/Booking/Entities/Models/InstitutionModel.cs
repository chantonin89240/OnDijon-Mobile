namespace OnDijon.Modules.Booking.Entities.Models
{
    public class InstitutionModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public bool MultiplePerson { get; set; }
        public int MaxNumberOfPerson { get; set; }
        public int OpeningTime { get; set; }
        public string EditId { get; set; }
    }
}

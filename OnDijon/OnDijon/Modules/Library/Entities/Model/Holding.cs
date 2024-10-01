namespace OnDijon.Modules.Library.Entities.Model
{
    public class Holding
    {
        public string Barcode { get; set; }
        public string BookingTooltip { get; set; }
        public object Category { get; set; }
        public string HoldingId { get; set; }
        public object HoldingPlace { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsConsultable { get; set; }
        public bool IsExpo { get; set; }
        public bool IsLoanable { get; set; }
        public bool IsReservable { get; set; }
        public bool IsSerial { get; set; }
        public bool IsTransmissible { get; set; }
        public string Localisation { get; set; }
        public int NbResa { get; set; }
        public bool NewItem { get; set; }
        public string RecordId { get; set; }
        public string Section { get; set; }
        public string SectionCode { get; set; }
        public string Site { get; set; }
        public string SiteCode { get; set; }
        public string Statut { get; set; }
        public string Type { get; set; }
        public string WhenBack { get; set; }
        public string SiteAddress { get; set; }
    }
}

namespace OnDijon.Modules.Bill.Entities.Models
{
    public class BillModel 
    {
        public string Title { get; set; }
        public string Number { get; set; }
        public string LevyDate { get; set; }
        public string ToPay { get; set; }
        public string State { get; set; }
        public string EditId { get; set; }
        public string FamilyNumber { get; set; }
        public string DematBill { get; set; }
        public string TipiReference { get; set; }
        public string LevyType { get; set; }
        public string PayLink { get; set; }
        public string DownloadLink { get; set; }
        public bool IsFirstBill { get; set; }
    }
}

namespace OnDijon.Modules.Demands.Entities.Models
{
    public class DemandModel
    {
        public string Category { get; set; }
        public string Date { get; set; }
        public DescriptionModel FirstDescription { get; set; }
        public DescriptionModel SecondDescription { get; set; }
        public DescriptionModel ThirdDescription { get; set; }
        public ActionModel ActionDemand { get; set; }
        public string ServiceCode { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string CityContext { get; set; }
    }
}

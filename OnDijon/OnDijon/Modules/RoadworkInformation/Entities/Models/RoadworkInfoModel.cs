using System.Collections.Generic;

namespace OnDijon.Modules.RoadworkInformation.Entities.Models
{
    public class RoadworkInfoModel
    {
        public string EditId { get; set; }
        public string Title { get; set; }
        public string Executant { get; set; }
        public string Applicant { get; set; }
        public string DateBeginRoadwork { get; set; }
        public string DateEndRoadwork { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string ObjectType { get; set; }
        public string State { get; set; }
        public List<RoadworkRingModel> Area { get; set; }
    }
}

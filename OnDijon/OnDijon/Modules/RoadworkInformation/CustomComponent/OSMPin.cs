using Mapsui.UI.Forms;
using OnDijon.Modules.RoadworkInformation.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.RoadworkInformation.CustomComponent
{
    public class OSMPin : Pin
    {
        public string EditId { get; set; }
        public string ObjectType { get; set; }
        public List<RoadworkRingModel> Area { get; set; }
    }
}

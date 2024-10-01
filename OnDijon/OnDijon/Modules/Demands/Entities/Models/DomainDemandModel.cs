using System.Collections.Generic;

namespace OnDijon.Modules.Demands.Entities.Models
{
    public class DomainDemandModel
    {
        public string Title { get; set; }
        public List<DemandModel> Demands { get; set; }
        public string CityContext { get; set; }
    }
}

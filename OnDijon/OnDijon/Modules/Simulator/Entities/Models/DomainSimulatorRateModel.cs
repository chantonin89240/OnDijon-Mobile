using System.Collections.Generic;

namespace OnDijon.Modules.Simulator.Entities.Models
{
    public class DomainSimulatorRateModel
    {
        public string Title { get; set; }
        public List<CategorySimulatorRateModel> Categories { get; set; }
    }
}

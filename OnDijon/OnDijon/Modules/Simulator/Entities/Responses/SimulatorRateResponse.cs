using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Simulator.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.Simulator.Entities.Responses
{
    public class SimulatorRateResponse : Response
    {
        public string Title;
        public string WarningMessage;
        public List<DomainSimulatorRateModel> DomainSimulatorRate;
    }
}

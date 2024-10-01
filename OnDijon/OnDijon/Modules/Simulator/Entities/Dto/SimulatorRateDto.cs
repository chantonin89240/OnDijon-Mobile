using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Simulator.Entities.Dto
{
    public class SimulatorRateDto : WsDMDto
    {
        public string Title;
        public string WarningMessage;
        public List<DomainSimulatorRateDto> DomainSimulatorRate;
    }
}

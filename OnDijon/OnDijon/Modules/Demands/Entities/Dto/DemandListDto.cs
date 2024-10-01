using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Demands.Entities.Dto
{
    public class DemandListDto : WsDMDto
    {
        public List<DemandDto> Demands;
        public string CityContext;
    }
}

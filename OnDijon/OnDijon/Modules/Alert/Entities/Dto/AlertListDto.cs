using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Alert.Entities.Dto
{
    public class AlertListDto : WsDMDto
    {
        public List<AlertDto> Alerts { get; set; }
    }
}

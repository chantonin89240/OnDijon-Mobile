using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.WedAlsh.Entities.Dto
{
    public class WedAlshRegistrationsDto : WsDMDto
    {
        public List<WedAlshChildDto> Childs { get; set; }
    }
}

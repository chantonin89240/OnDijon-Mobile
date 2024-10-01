using System.Collections.Generic;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.School.Entities.Dto
{
    public class ChildListDto : WsDMDto
    {
        public IList<ChildDto> Childs { get; set; }
        public string SessionScheduledHelper { get; set; }
    }
}

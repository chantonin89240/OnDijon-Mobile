using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.Library.Entities.Dto.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class SearchDto : WsDMDto
    {
        public List<ResourceDto> Resources { get; set; }
        public int Page { get; set; }
        public int PageMax { get; set; }
    }
}

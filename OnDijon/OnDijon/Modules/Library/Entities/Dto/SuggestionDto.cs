using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class SuggestionDto : WsDMDto
    {
        public List<string> Suggestions { get; set; }
    }
}

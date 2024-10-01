using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Diary.Entities.Dto
{
    public class DiarySuggestionDto : WsDMDto
    {
        public IEnumerable<string> Results { get; set; }
    }
}

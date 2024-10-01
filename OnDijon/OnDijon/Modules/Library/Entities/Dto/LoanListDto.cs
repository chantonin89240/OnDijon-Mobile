using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.Library.Entities.Dto.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class LoanListDto : WsDMDto
    {
        [JsonProperty("Loans")]
        public List<LoanDto> Loans { get; set; }
    }
}

using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.Library.Entities.Dto.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class HoldingsDto : WsDMDto
    {
        [JsonProperty("Holdings")]
        public List<HoldingDto> Holdings { get; set; }
    }
}

using OnDijon.Common.Entities.Dto;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace OnDijon.Modules.Strike.Entities.DTO
{
    public class NurserySessionStrikeDto : WsDMDto
    {
        [JsonProperty("Date")]
        public DateTime DateStrike;
        [JsonProperty("StrikesCreche")]
        public List<NurseryStrikeInfoDto> Strikes;
    }
}

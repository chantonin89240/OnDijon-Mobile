using System.Collections.Generic;
using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Dashboard.Entities.Dto.Card
{
    public class CardListDto : WsDMDto
    {
        [JsonProperty("Cards")]
        public IList<CardDto> Cards { get; set; }
    }
}

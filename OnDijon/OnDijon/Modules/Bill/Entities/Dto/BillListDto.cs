using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Bill.Entities.Dto
{
    public class BillListDto : WsDMDto
    {
        [JsonProperty("Bills")]
        public List<BillDto> Bills;
    }
}

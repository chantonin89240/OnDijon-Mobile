using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Dashboard.Entities.Dto
{
    public class WorkDataListDto : WsDMDto
    {
        [JsonProperty("WorkDatas")]
        public List<WorkDataDto> WorkDatas;
    }
}

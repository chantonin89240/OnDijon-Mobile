using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.RoadworkInformation.Entities.Dto
{
    public class RoadworkInfoListDto : WsDMDto
    {
        [JsonProperty("WorkInfos")]
        public List<RoadworkInfoDto> RoadworkList;
    }
}

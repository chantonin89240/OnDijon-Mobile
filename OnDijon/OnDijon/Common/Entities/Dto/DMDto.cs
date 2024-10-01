using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnDijon.Common.Entities.Dto
{
    public class WsDMDto : OnDijon.Common.Entities.Dto.Dto
    {
        [JsonProperty("StatusCodes")]
        public List<string> StatusCodes { get; set; }

        [JsonProperty("StatusMessages")]
        public List<StatusMessage> StatusMessages { get; set; }
    }

}

using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class AssociateReaderAccountDto : WsDMDto
    {
        [JsonProperty("Success")]
        public bool Success { get; set; }
    }
}

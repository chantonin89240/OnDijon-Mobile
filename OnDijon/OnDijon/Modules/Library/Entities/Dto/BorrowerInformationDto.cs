using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.Library.Entities.Dto.Model;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class BorrowerInformationDto : WsDMDto
    {
        [JsonProperty("UserAccount")]
        public BorrowerDto UserAccount { get; set; }
    }
}

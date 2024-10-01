using System.Collections.Generic;
using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Services.Entities.Dto
{
    public class ServiceListDto : WsDMDto
    {
        [JsonProperty("MenuItems")]
        public IList<ServiceDto> Services { get; set; }

    }
}

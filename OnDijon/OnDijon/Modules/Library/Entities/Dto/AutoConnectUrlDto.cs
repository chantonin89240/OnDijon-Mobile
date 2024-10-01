using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class AutoConnectUrlDto : WsDMDto
    {
        [JsonProperty("UrlAutoConnect")]
        public string Url { get; set; }
    }
}

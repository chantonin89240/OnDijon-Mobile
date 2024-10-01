using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class LibraryDocDto : WsDMDto
    {
        [JsonProperty("Picture")]
        public string Picture { get; set; }
    }
}

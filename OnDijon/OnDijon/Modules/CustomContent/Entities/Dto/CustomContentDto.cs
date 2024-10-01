using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.CustomContent.Entities.Dto
{
    public class CustomContentDto : WsDMDto
    {
        [JsonProperty("Title")]
        public string Title;
        [JsonProperty("Description")]
        public string Description;
        [JsonProperty("Image")]
        public string Image;
        [JsonProperty("Video")]
        public string Video;
        [JsonProperty("ExternalLinkTitle")]
        public string ExternalLinkTitle;
        [JsonProperty("ExternalLink")]
        public string ExternalLink;
    }
}
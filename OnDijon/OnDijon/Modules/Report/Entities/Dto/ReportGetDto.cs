using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Report.Entities.Dto
{
    public class ReportGetDto : WsDMDto
    {
        [JsonProperty(PropertyName = "Report")]
        public ReportDto Report { get; set; }
    }
}

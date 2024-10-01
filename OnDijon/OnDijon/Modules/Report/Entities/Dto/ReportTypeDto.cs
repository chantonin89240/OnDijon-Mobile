using System.Collections.Generic;
using Newtonsoft.Json;
using OnDijon.Common.Utils;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Report.Entities.Dto
{
    public class ReportTypeDto : Common.Entities.Dto.Dto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "imageKey")]
        public string ImageKey { get; set; }

        public string ImageUrl
        {
            get
            {
                return string.Concat(Constants.API_URL, Constants.REPORT_SERVICES, Constants.API_REPORT_ICONS, "?Mode=", Common.Utils.Constants.BUILD_CONFIGURATION, "&IconKey=", ImageKey);
            }
        }
    }

    public class ReportTypeListDto : WsDMDto
    {
        [JsonProperty(PropertyName = "ReportTypes")]
        public IList<ReportTypeDto> ReportTypesList { get; set; }
    }

}

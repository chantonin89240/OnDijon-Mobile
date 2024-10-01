using OnDijon.Modules.Report.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Report.Entities.Response
{
    public class ReportTypesListResponse : Common.Entities.Response.Response
    {
        public IEnumerable<ReportTypeDto> ReportTypes { get; set; }
    }
}

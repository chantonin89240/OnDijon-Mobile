using System.Collections.Generic;
using OnDijon.Modules.Dashboard.Entities.Models;

namespace OnDijon.Modules.Dashboard.Entities.Response
{
    public class WorkDataResponse : Common.Entities.Response.Response
    {
        public List<WorkDataModel> WorkDataList { get; set; }
    }
}

using OnDijon.Common.Entities.Response;
using OnDijon.Modules.RoadworkInformation.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.RoadworkInformation.Entities.Responses
{
    public class RoadworkInfoResponse : Response
    {
        public List<RoadworkInfoModel> RoadworkList { get; set; }
    }
}

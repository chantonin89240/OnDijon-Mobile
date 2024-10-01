using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Demands.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.Demands.Entities.Responses
{
    public class DemandListResponse : Response
    {
        public List<DemandModel> DemandList;
        public string CityContext;
    }
}

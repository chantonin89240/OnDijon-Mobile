using OnDijon.Common.Entities.Response;
using OnDijon.Modules.UsefulContact.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.UsefulContact.Entities.Responses
{
    public class ServiceListResponse : Response
    {
        public List<ServiceModel> ServiceList { get; set; }
    }
}

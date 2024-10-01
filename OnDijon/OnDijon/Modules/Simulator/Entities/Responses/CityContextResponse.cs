using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Simulator.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.Simulator.Entities.Responses
{
    public class CityContextResponse : Response
    {
        public string Title { get;set; }
        public string WarningMessage { get; set; }
        public List<CityContextModel> Cities { get; set; }
    }
}
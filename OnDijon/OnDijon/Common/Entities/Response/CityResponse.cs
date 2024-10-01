using OnDijon.Common.Entities.Model;
using System.Collections.Generic;

namespace OnDijon.Common.Entities.Response
{
    public class CityResponse : Response
    {
        public List<CityModel> CityModels { get; set; }
    }
}

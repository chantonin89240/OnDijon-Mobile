
using OnDijon.Modules.School.Entities.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDijon.Modules.SchoolServices.Interfaces
{
    public interface ISchoolCardConfigurationService
    {
        Task<ChildResponse> GetChilds(string id, IDictionary<string, string> sessionEditIdCityContext);
    }
}

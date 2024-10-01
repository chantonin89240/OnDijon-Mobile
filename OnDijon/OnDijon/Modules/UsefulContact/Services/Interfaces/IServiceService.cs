using OnDijon.Modules.UsefulContact.Entities.Models;
using OnDijon.Modules.UsefulContact.Entities.Responses;
using System.Threading.Tasks;

namespace OnDijon.Modules.UsefulContact.Services.Interfaces
{
    public interface IServiceService
    {
        Task<ServiceListResponse> GetServices();
    }
}
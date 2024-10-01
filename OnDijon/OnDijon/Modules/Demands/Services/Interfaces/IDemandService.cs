using OnDijon.Modules.Demands.Entities.Responses;
using System.Threading.Tasks;

namespace OnDijon.Modules.Demands.Services.Interfaces
{
    public interface IDemandService
    {
        Task<DemandListResponse> GetDemands(string city, string idUser);
    }
}

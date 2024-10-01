using OnDijon.Modules.RoadworkInformation.Entities.Responses;
using System.Threading.Tasks;

namespace OnDijon.Modules.RoadworkInformation.Services.Interfaces
{
    public interface IRoadworkInfoService
    {
        Task<RoadworkInfoResponse> GetRoadworks(string UserId, string ObjectType);
    }
}

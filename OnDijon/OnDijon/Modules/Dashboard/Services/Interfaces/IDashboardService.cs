using System.Threading.Tasks;
using OnDijon.Modules.Dashboard.Entities.Dto.Card;
using OnDijon.Modules.Dashboard.Entities.Response;
using OnDijon.Common.Entities.Response;

namespace OnDijon.Modules.Dashboard.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DtoListResponse<CardDto>> GetAllCards();
        Task<WorkDataResponse> GetWorkData(string UserId);
    }
}

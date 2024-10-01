using System;
using System.Threading.Tasks;
using OnDijon.Modules.Services.Entities.Dto;
using OnDijon.Modules.Services.Entities.Request;
using OnDijon.Modules.Services.Entities.Response;
using OnDijon.Common.Entities.Response;

namespace OnDijon.Modules.Services.Services.Interfaces
{
    public interface IServicesService
    {
        Task<DtoListResponse<ServiceDto>> GetAllServices();

        Task<Response> UpdateFavourite(UpdateFavouriteServiceRequest request);
        Task<FavoriteServiceListResponse> GetFavouriteServices(string userId, bool getFromCache = false);
        Task<FavoriteServiceListResponse> GetFavouriteServices(Guid userId, bool getFromCache = false);
    }
}

using OnDijon.Modules.Account.Entities.Dto;
using OnDijon.Modules.Account.Entities.Response;
using System.Threading.Tasks;

namespace OnDijon.Modules.Account.Services.Interfaces
{
    public interface ICguService
    {
        Task<CguResponse> GetCgu();

        Task<GetCguDto> GetCguAsync();
    }
}

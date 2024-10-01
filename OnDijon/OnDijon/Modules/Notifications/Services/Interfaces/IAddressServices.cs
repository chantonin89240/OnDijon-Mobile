using System.Threading.Tasks;
using OnDijon.Common.Entities.Model;
using OnDijon.Common.Entities.Response;

namespace OnDijon.Modules.Notifications.Services.Interfaces
{
    public interface IAddressServices
    {
        
        Task<CityResponse> GetCities(string pattern, bool IsMetropolitanCities = false);

        Task<AddressResponse> GetAddressFromCity(CityModel model,string pattern);

    }
}

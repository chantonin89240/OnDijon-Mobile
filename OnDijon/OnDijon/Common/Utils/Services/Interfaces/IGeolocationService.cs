using System.Threading.Tasks;
using Xamarin.Essentials;

namespace OnDijon.Common.Utils.Services.Interfaces
{
    public interface IGeolocationService
    {
        Task<Location> GetCurrentLocation();
    }
}

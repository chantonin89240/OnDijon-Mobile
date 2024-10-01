using OnDijon.Common.Utils.Services.Interfaces;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace OnDijon.Common.Utils.Services
{
    public class GeolocationService : IGeolocationService
    {
        public async Task<Location> GetCurrentLocation()
        {
            return await Geolocation.GetLocationAsync();
        }
    }
}

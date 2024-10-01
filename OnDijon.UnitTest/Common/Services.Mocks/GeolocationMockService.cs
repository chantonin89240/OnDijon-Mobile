using OnDijon.Common.Utils.Services.Interfaces;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace OnDijon.UnitTest.Common.Services.Mocks
{
    class GeolocationMockService : IGeolocationService
    {
        public async Task<Location> GetCurrentLocation()
        {
            return await Task.Run(() => { return new Location(); });
        }
    }
}

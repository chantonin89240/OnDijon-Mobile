using OnDijon.Common.Utils.Services.Interfaces;
using System.Threading.Tasks;
using OnDijon.Modules.Account.Entities.Models;

namespace OnDijon.Common.Utils.Services
{
    public class UserIdService : IUserIdService
    {
        private const string ProfileKey = "Profile";

        private readonly ICacheService _cacheService;

        public UserIdService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<string> GetUserId()
        {
            ProfileModel profile = await _cacheService.Get<ProfileModel>(ProfileKey, CacheType.User);
            return profile.Guid;
        }
    }
}

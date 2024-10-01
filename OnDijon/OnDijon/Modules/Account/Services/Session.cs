using OnDijon.Modules.Report.Entities.Request;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Utils.Services.Interfaces;
using System.Threading.Tasks;
using OnDijon.Modules.Account.Entities.Models;

namespace OnDijon.Modules.Account.Services
{
    public class Session : ISession
    {
        private const string ProfileKey = "Profile";

        private readonly ICacheService _cacheService;

        public Session(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public ProfileModel Profile
        {
            get
            {
                return Task.Run(async () => await _cacheService.Get<ProfileModel>(ProfileKey, CacheType.User)).Result;
            }
            set
            {
                _cacheService.Delete<ProfileModel>(ProfileKey, CacheType.User);
                _cacheService.Put(ProfileKey, value, CacheType.User);
            }
        }

        public bool IsConnected()
        {
            return Profile != null;
        }

        public ReportRequest ReportRequest { get; set; }

        /// <summary>
        /// Cleanup this instance.
        /// </summary>
        public void Cleanup()
        {
            ReportRequest = null;
        }
    }
}

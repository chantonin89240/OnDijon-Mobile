using OnDijon.Common.Utils.Services.Interfaces;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.Common.Services.Mocks
{
    class UserIdMockService : IUserIdService
    {
        public Task<string> GetUserId()
        {
            return Task.Run(() => { return "test"; });
        }
    }
}

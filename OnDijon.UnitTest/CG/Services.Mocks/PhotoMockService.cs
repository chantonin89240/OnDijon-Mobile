using OnDijon.CG.Enums;
using OnDijon.CG.Services.Interfaces.Front;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.CG.Services.Mocks
{
    class PhotoMockService : IPhotoService
    {
        public async Task<byte[]> Open(PhotoSourceEnum source)
        {
            return await Task.Run(() => { return new byte[0]; });
        }
    }
}

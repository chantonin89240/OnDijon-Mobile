using System.Threading.Tasks;
using OnDijon.Common.Utils.Enums;

namespace OnDijon.Common.Services.Interfaces.Front
{
    public interface IPhotoService
    {
        Task<byte[]> Open(PhotoSourceEnum source);
    }
}

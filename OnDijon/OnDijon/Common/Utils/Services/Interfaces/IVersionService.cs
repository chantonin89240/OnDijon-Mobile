using System.Threading.Tasks;

namespace OnDijon.Common.Utils.Services.Interfaces
{
    public interface IVersionService
    {
        Task<AppVersionStateResponse> GetAppVersionState(string lastVersion);
    }
}

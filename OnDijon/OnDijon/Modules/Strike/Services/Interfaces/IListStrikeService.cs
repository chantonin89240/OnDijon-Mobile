using OnDijon.Modules.Strike.Entities.Responses;
using System.Threading.Tasks;

namespace OnDijon.Modules.Strike.Services.Interfaces
{
    public interface IListStrikeService
    {
        Task<SessionStrikeResponse> GetSessionStrike(string editId);

        Task<NurserySessionStrikeResponse> GetNurserySessionStrike(string editId);
    }
}

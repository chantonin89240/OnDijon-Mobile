using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Rating.Entities.Response;
using System.Threading.Tasks;

namespace OnDijon.Modules.Rating.Services.Interfaces
{
    public interface IRatingService
    {
        Task<GetSessionRatingResponse> GetActualRatingSession();
        Task<Response> SendRatingSession(string sessionEditId, int note, string comment);
    }
}

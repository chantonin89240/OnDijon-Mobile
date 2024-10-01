using System.Threading.Tasks;

namespace OnDijon.Common.Utils.Services.Interfaces
{
    public interface IUserIdService
    {
        /// <summary>
        /// Return an anonymous user id for notifications
        /// </summary>
        Task<string> GetUserId();
    }
}

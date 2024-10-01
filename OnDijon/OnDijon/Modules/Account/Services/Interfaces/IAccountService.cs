using System.Threading.Tasks;
using OnDijon.Modules.Account.Entities.Response;
using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Account.Entities.Models;

namespace OnDijon.Modules.Account.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Authenticate the user
        /// </summary>
        Task<AuthenticationResponse> Authenticate(string mail, string password);

        /// <summary>
        /// Create a new user account
        /// </summary>
        Task<CreateAccountResponse> Create(string password, ProfileModel profile);

        /// <summary>
        /// Update the user account
        /// </summary>
        Task<Response> Update(ProfileModel profile, string userEditId);

        /// <summary>
        /// Update the user mobile tokens
        /// </summary>
        Task<Response> AddMobileToken(string token, ProfileModel profile);

        /// <summary>
        /// Get the user account
        /// </summary>
        Task<GetAccountResponse> Get(string guid);

        /// <summary>
        /// Change the user password
        /// </summary>
        Task<ChangePasswordResponse> ChangePassword(string editId, string oldPassword, string newPassword);

        /// <summary>
        /// Reset the user password
        /// </summary>
        Task<Response> ResetPassword( string mail);

        /// <summary>
        /// Delete user
        /// </summary>
        Task<Response> Delete(string userEditId, string password);

        /// <summary>
        /// Disconnect user
        /// </summary>
        Task<Response> Disconnect(string userEditId);

        /// <summary>
        /// Disconnect the user (local operation)
        /// </summary>
        Task Disconnect();
    }
}

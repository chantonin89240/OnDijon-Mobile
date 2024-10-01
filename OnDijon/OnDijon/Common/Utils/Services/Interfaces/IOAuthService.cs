using System;
using System.Threading.Tasks;

namespace OnDijon.Common.Utils.Services.Interfaces
{
    public interface IOAuthService
    { 
        string IdentityToken { get; set; }
        string RefreshToken { get; set; }
        string AccessToken { get; set; }
        DateTimeOffset? TokenExpirationDate { get; set; }
        bool IsTokenValid { get; }
        string Login { get; set; }
        
        Task<bool> Refresh();
        Task<bool> GetToken();
        Task<bool> Logout();
    }
}
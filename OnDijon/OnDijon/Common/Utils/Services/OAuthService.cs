using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using OnDijon.Common.Utils.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OnDijon.Common.Utils.Services
{
    public class OAuthService : IOAuthService
    {
        #region Privates

        private readonly IBrowser _browser;

        private OidcClientOptions _options;
        private OidcClient _oidcClient;

        #endregion

        public OAuthService()
        {
            _browser = DependencyService.Get<IBrowser>();
        }

        #region Properties

        public string IdentityToken { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset? TokenExpirationDate { get; set; }
        public bool IsTokenValid => !string.IsNullOrWhiteSpace(AccessToken) && TokenExpirationDate > DateTimeOffset.Now;
        public string Login { get; set; }

        #endregion

        /// <summary>
        /// Try refreshing tokens
        /// </summary>
        /// <returns>True if tokens are refreshed or still valid, false otherwise</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Refresh()
        {
            try
            {
                RefreshToken = Preferences.Get("RT", "");
                
                if (_oidcClient == null)
                    InitOidcClient();
                
                var resultRefresh = await _oidcClient.RefreshTokenAsync(RefreshToken);
                if (!(TokenExpirationDate <= DateTimeOffset.Now)) return true;

                if (resultRefresh.IsError)
                {
                    AccessToken = string.Empty;
                    throw new Exception(resultRefresh.Error);
                }

                AccessToken = resultRefresh.AccessToken;
                RefreshToken = resultRefresh.RefreshToken;
                IdentityToken = resultRefresh.IdentityToken;
                TokenExpirationDate = resultRefresh.AccessTokenExpiration;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Open login page and callback result
        /// </summary>
        /// <returns>True if logged, false otherwise</returns>
        public async Task<bool> GetToken()
        {
            try
            {
                
                if (_oidcClient == null)
                    InitOidcClient();

                var loginRequest = new LoginRequest();
                var loginResult = await _oidcClient.LoginAsync(loginRequest);

                if (loginResult.IsError)
                    return false;

                AccessToken = loginResult.AccessToken;
                IdentityToken = loginResult.IdentityToken;
                RefreshToken = loginResult.RefreshToken;
                TokenExpirationDate = loginResult.AccessTokenExpiration;
                Login = loginResult.User?.Identity?.Name ?? ""; //TODO : Change with needed prop 

                Preferences.Set("IT", IdentityToken);
                Preferences.Set("RT", RefreshToken);
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Logout current connected user
        /// </summary>
        /// <returns>True if user logged out, false otherwise</returns>
        public async Task<bool> Logout()
        {
            try
            {
                IdentityToken = Preferences.Get("IT", "");

                if (_oidcClient == null)
                    InitOidcClient();
                
                
                var logoutRequest = new LogoutRequest();
                logoutRequest.IdTokenHint = IdentityToken;
                var logoutResult = await _oidcClient.LogoutAsync(logoutRequest);

                if (logoutResult.IsError)
                    throw new Exception(logoutResult.Error);

                AccessToken = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void InitOidcClient()
        {
            _options = new OidcClientOptions()
            {
                Authority = OAuthConstants.Authority,
                ClientId = OAuthConstants.ClientId,
                Scope = OAuthConstants.Scope,
                RedirectUri = OAuthConstants.RedirectUri,
                Policy = new Policy()
                {
                    Discovery =
                    {
                        RequireHttps = false,
                        ValidateIssuerName = false,
                        AuthorityValidationStrategy =
                            new StringComparisonAuthorityValidationStrategy(StringComparison.OrdinalIgnoreCase),
                    }
                },
                Browser = _browser
            };

            _oidcClient = new OidcClient(_options);
        }
    }

    internal static class OAuthConstants
    {
        public const string Authority = "https://dijon-metro-poc-keycloak.atolcd.show/realms/metro-dijon";

        public const string ClientId = "app-ondijon";

        public const string Scope =
            "openid phone profile offline_access web-origins acr roles email address microprofile-jwt";

        public const string RedirectUri = "fr.dm.ondijon://*";
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDijon.Common.Utils.Enums;
using OnDijon.Modules.Account.Entities.Dto;
using OnDijon.Modules.Account.Entities.Request;
using OnDijon.Modules.Account.Entities.Response;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Model;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils.Services.Interfaces;
using Newtonsoft.Json;
using Microsoft.AppCenter.Crashes;
using OnDijon.Modules.Account.Entities.Models;
using OnDijon.Common.Entities.Dto;
using Xamarin.Forms.Internals;
using OnDijon.Common.Utils;
using OnDijon.Modules.Services.Entities.Dto;
using System.Net.Http;

namespace OnDijon.Modules.Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly Common.Utils.Services.Interfaces.IHttpService _httpService;
        private readonly ISession _session;
        private readonly ICacheService _cacheService;

        private const string MobileRegistrationsKey = "MobileRegistrations";

        public AccountService(IHttpService httpService, ISession session, ICacheService cacheService)
        {
            _httpService = httpService;
            _session = session;
            _cacheService = cacheService;
        }


        #region Get
        public async Task<GetAccountResponse> Get(string guid)
        {
            GetAccountDto sources = await GetAsync(guid);

            GetAccountResponse response = Common.Entities.Utils.Translate<GetAccountResponse, GetAccountDto>(sources);
            response.Profile = new ProfileModel()
            {
                AddressComplement = sources.Profile.AddressComplement,
                Birthday = sources.Profile.Birthday,
                City = sources.Profile.City,
                Country = sources.Profile.Country,
                FirstName = sources.Profile.FirstName,
                FixPhoneNumber = sources.Profile.FixPhoneNumber,
                Gender = sources.Profile.Gender,
                Guid = sources.Profile.Guid,
                Mail = sources.Profile.Mail,
                Name = sources.Profile.Name,
                PhoneNumber = sources.Profile.PhoneNumber,
                PostalCode = sources.Profile.PostalCode,
                Scope = sources.Profile.Scope,
                Street = sources.Profile.Street,
                StreetNumber = sources.Profile.StreetNumber,
                StreetNumberComplement = sources.Profile.StreetNumberComplement
            };
            response.MobileRegistrations = new List<MobileRegistrationModel>();
            sources.MobileRegistrations.ForEach(
                (item) => response.MobileRegistrations.Add(new MobileRegistrationModel() { DeviceId = item.DeviceId, RegistrationToken = item.RegistrationToken })
            );
            if (response.IsSuccessful() && response.Profile != null)
            {
                response.Profile.Guid = _session.Profile.Guid;
                _session.Profile = response.Profile;

                List<ProfileAzureDto> _UsersList = new List<ProfileAzureDto>();

                _UsersList = await _httpService.GetBisAsync<List<ProfileAzureDto>>(Constants.API_DIIAGE + Constants.Profil, HttpMethod.Get);

                if (_UsersList.Count > 0)
                {

                    ProfileAzureDto profilExist = _UsersList.FirstOrDefault(p => p.Guid == _session.Profile.Guid);
                    
                    if (profilExist == null)
                    {

                        var testProfil = new ProfileAzureDto
                        {
                            Nom = _session.Profile.Name,
                            Prenom = _session.Profile.FirstName,
                            Guid = _session.Profile.Guid
                        };

                        string jsonProfile = JsonConvert.SerializeObject(testProfil);
                        var responseToPostProfile = await _httpService.PostBisAsync<ProfileAzureDto>(Constants.API_DIIAGE + Constants.Profil, HttpMethod.Post, jsonProfile);
                    }
                }

                await _cacheService.Put(MobileRegistrationsKey, response.MobileRegistrations, CacheType.User);
            }
            else
            {
                response.Message = "Impossible de récupérer les informations du compte";
                _session.Profile = null;
            }
            return response;
        }

        private async Task<GetAccountDto> GetAsync(string guid)
        {
            GetAccountDto response = new GetAccountDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ACCOUNT_SERVICES, Constants.ACCOUNT_GET);
                GetAccountRequest r = new GetAccountRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Profile = new ProfilRequest() { Guid = guid }
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<GetAccountDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
        #endregion

        #region Authenticate
        public async Task<AuthenticationResponse> Authenticate(string mail, string password)
        {
            AuthenticationDto sources = await AuthenticateAsync(mail, password);

            AuthenticationResponse response = Common.Entities.Utils.Translate<AuthenticationResponse, AuthenticationDto>(sources);
            if (response.IsSuccessful())
            {
                response.Guid = sources.Guid;
                response.AuthenticationUrl = sources.AuthenticationUrl;
            }
            return response;
        }

        private async Task<AuthenticationDto> AuthenticateAsync(string mail, string password)
        {
            AuthenticationDto response = new AuthenticationDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ACCOUNT_SERVICES, Constants.ACCOUNT_AUTHENTICATION);
                AuthenticationRequest r = new AuthenticationRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Mail = mail,
                    Password = password,
                    UserEditId = null
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<AuthenticationDto>(new Uri(url), json).ConfigureAwait(false);
                     
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
        #endregion

        #region Create
        public async Task<CreateAccountResponse> Create(string password, ProfileModel profile)
        {

            CreateAccountDto sources = await CreateAsync(password, profile);

            CreateAccountResponse response = Common.Entities.Utils.Translate<CreateAccountResponse, CreateAccountDto>(sources);
            if (!response.IsSuccessful())
            {
                response.Message = "Impossible de créer le compte";
            }
            return response;
        }

        private async Task<CreateAccountDto> CreateAsync(string password, ProfileModel profile)
        {
            CreateAccountDto response = new CreateAccountDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ACCOUNT_SERVICES, Constants.ACCOUNT_CREATE);
                CreateAccountRequest r = new CreateAccountRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Password = password,
                    Profile = profile
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<CreateAccountDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
        #endregion

        #region Update
        public async Task<Response> Update(ProfileModel profile, string userEditId)
        {

            WsDMDto sources = await UpdateAsync(profile, userEditId);

            UpdateAccountResponse response = Common.Entities.Utils.Translate<UpdateAccountResponse, WsDMDto>(sources);
            if (response.IsSuccessful())
            {
                _session.Profile = profile;
            }
            else
            {
                //dtoResponse successful here means error message was already parsed from http response body
                if (!response.IsSuccessful())
                {
                    response.Message = "Impossible de modifier le compte";
                }
            }

            return response;
        }

        private async Task<WsDMDto> UpdateAsync(ProfileModel profile, string userEditId)
        {
            WsDMDto response = new WsDMDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ACCOUNT_SERVICES, Constants.ACCOUNT_UPDATE);
                UpdateRequest r = new UpdateRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Profile = profile,
                    UserEditId = userEditId
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
        #endregion

        #region AddMobileToken
        public async Task<Response> AddMobileToken(string token, ProfileModel profile)
        {
            IList<MobileRegistrationModel> registrations = await _cacheService.Get<IList<MobileRegistrationModel>>(MobileRegistrationsKey, CacheType.User);
            if (registrations != null && registrations.FirstOrDefault(mr => mr.RegistrationToken == token) != null)
            {
                //token is already registered
                return new Response { State = CallStatusEnum.Success };
            }
            else
            {

                UpdateMobileDto sources = await AddMobileTokenAsync(token, profile);

                Response response = Common.Entities.Utils.Translate<Response, UpdateMobileDto>(sources);
                if (response.IsSuccessful())
                {
                    //update mobile tokens cache
                    registrations = registrations ?? new List<MobileRegistrationModel>();
                    registrations.Add(new MobileRegistrationModel()
                    {
                        DeviceId = $"{Xamarin.Essentials.DeviceInfo.Manufacturer} {Xamarin.Essentials.DeviceInfo.Model}",
                        RegistrationToken = token
                    });
                    await _cacheService.Put(MobileRegistrationsKey, registrations, CacheType.User);
                }
                else
                {
                    response.Message = "Impossible de modifier le profil mobile";
                }

                return response;
            }
        }

        private async Task<UpdateMobileDto> AddMobileTokenAsync(string token, ProfileModel profile)
        {
            UpdateMobileDto response = new UpdateMobileDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ACCOUNT_SERVICES, Constants.ACCOUNT_UPDATE_MOBILE);
                UpdateMobileRequest r = new UpdateMobileRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Profile = profile,
                    MobileRegistration = new MobileRegistrationRequest()
                    {
                        DeviceId = $"{Xamarin.Essentials.DeviceInfo.Manufacturer} {Xamarin.Essentials.DeviceInfo.Model}",
                        RegistrationToken = token
                    }

                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<UpdateMobileDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
        #endregion

        #region ChangePassword
        public async Task<ChangePasswordResponse> ChangePassword(string editId, string oldPassword, string newPassword)
        {

            ChangePasswordDto sources = await ChangePasswordAsync(editId, oldPassword, newPassword);

            ChangePasswordResponse response = Common.Entities.Utils.Translate<ChangePasswordResponse, ChangePasswordDto>(sources);
            if (!response.IsSuccessful())
            {
                response.AuthenticationUrl = sources.AuthenticationUrl;
                response.Guid = sources.Guid;
                response.Message = "Impossible de modifier le mot de passe";
            }
            return response;
        }

        private async Task<ChangePasswordDto> ChangePasswordAsync(string editId, string oldPassword, string newPassword)
        {
            ChangePasswordDto response = new ChangePasswordDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ACCOUNT_SERVICES, Constants.ACCOUNT_CHANGE_PASSWORD);
                ChangePasswordRequest r = new ChangePasswordRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = editId,
                    OldPassword = oldPassword,
                    NewPassword = newPassword
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<ChangePasswordDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
        #endregion

        #region ResetPassword
        public async Task<Response> ResetPassword(string mail)
        {
            WsDMDto sources = await ResetPasswordAsync(mail);

            Response response = Common.Entities.Utils.Translate<Response, WsDMDto>(sources);
            //dtoResponse successful here means error message was already parsed from http response body
            if (!response.IsSuccessful())
            {
                response.Message = "Impossible de réinitialiser le mot de passe";
            }
            return response;
        }

        private async Task<WsDMDto> ResetPasswordAsync(string mail)
        {
            WsDMDto response = new WsDMDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ACCOUNT_SERVICES, Constants.ACCOUNT_RESET_PASSWORD);
                ResetPasswordRequest r = new ResetPasswordRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Mail = mail,
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
        #endregion

        #region Delete
        public async Task<Response> Delete(string userEditId, string password)
        {
            WsDMDto sources = await DeleteAsync(userEditId, password);

            Response response = Common.Entities.Utils.Translate<Response, WsDMDto>(sources);
            return response;
        }
        private async Task<WsDMDto> DeleteAsync(string userEditId, string password)
        {
            WsDMDto response = new WsDMDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ACCOUNT_SERVICES, Constants.ACCOUNT_DELETE);
                DeleteAccountRequest r = new DeleteAccountRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    UserEditId = userEditId,
                    Password = password,
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
        #endregion

        #region Disconnect
        public async Task<Response> Disconnect(string userEditId)
        {
            WsDMDto sources = await DisconnectAsync(userEditId);

            Response response = Common.Entities.Utils.Translate<Response, WsDMDto>(sources);
            return response;
        }
        private async Task<WsDMDto> DisconnectAsync(string userEditId)
        {
            WsDMDto response = new WsDMDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ACCOUNT_SERVICES, Constants.ACCOUNT_DISCONNECT);
                DisconnectAccountRequest r = new DisconnectAccountRequest()
                {
                    DeviceId = $"{Xamarin.Essentials.DeviceInfo.Manufacturer} {Xamarin.Essentials.DeviceInfo.Model}",
                    Key = Constants.ONDIJON_KEY,
                    UserEditId = userEditId
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
        #endregion

        public async Task Disconnect()
        {

            // On vide le cache des services (pour gérer le cas de si on se reconnecte avec un autre compte) 
            await _cacheService.Delete<IList<ServiceDto>>(Common.Utils.Constants.ServicesListKey, CacheType.InMemory);
            await _cacheService.Delete<IList<ServiceDto>>(Common.Utils.Constants.FavoriteServicesListKey, CacheType.InMemory);
            await _cacheService.Delete<bool>(Common.Utils.Constants.FavoriteScopesAlertIdentityKey, CacheType.InMemory);
            await _cacheService.Delete<IList<CheckboxModel>>(Common.Utils.Constants.FavoriteScopesListKey, CacheType.InMemory);
            await _cacheService.Delete<bool>(Common.Utils.Constants.CguAccepted);

            _session.Profile = null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Modules.Services.Entities.Dto;
using OnDijon.Modules.Services.Entities.Request;
using OnDijon.Modules.Services.Entities.Response;
using OnDijon.Modules.Services.Services.Interfaces;
using OnDijon.Modules.Account.Entities.Request;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Entities.Model;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using static OnDijon.Common.Entities.Status;
using OnDijon.Common.Utils.Enums;

namespace OnDijon.Modules.Services.Services
{
    public class ServicesService : IServicesService
    {
        private readonly Common.Utils.Services.Interfaces.IHttpService _httpService;
        private readonly ICacheService _cacheService;
        private readonly ISession _session;

        public ServicesService(Common.Utils.Services.Interfaces.IHttpService httpService,
                                ICacheService cacheService,
                                 ISession session)
        {
            _httpService = httpService;
            _cacheService = cacheService;
            _session = session;
        }



        public async Task<DtoListResponse<ServiceDto>> GetAllServices()
        {
            DtoListResponse<ServiceDto> response = new DtoListResponse<ServiceDto> { State = CallStatusEnum.InternalServerError };

            //try services from cache
            var serviceList = await _cacheService.Get<IList<ServiceDto>>(Constants.ServicesListKey, CacheType.InMemory);

            if (serviceList != null && serviceList.Any())
            {
                response.State = CallStatusEnum.Success;
                response.Data = serviceList;
            }
            else //get services from server if not in cache
            {
                string url = string.Concat(Constants.API_URL, Constants.ONDIJON_SERVICES, Constants.SERVICE_GET_ALL);

                string userId = _session.IsConnected() ? _session.Profile.Guid : string.Empty;

                AccountRequest data = new AccountRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    UserEditId = userId
                };
                string payload = JsonConvert.SerializeObject(data);
                var result = await _httpService.PostAsync<ServiceListDto>(new Uri(url), payload);


                if (result.StatusCodes != null && result.StatusCodes.Contains(Code.Success))
                {
                    response.State = CallStatusEnum.Success;
                    response.Data = result.Services;
                    //put services in cache
                    await _cacheService.Put(Constants.ServicesListKey, response.Data, TimeSpan.FromMinutes(Constants.CacheServiceDuration), CacheType.InMemory);
                }
                response.Message = result.StatusMessages?.FirstOrDefault()?.Value;
            }
            return response;



        }

        public async Task<Response> UpdateFavourite(UpdateFavouriteServiceRequest request)
        {
            Response response = new Response { State = CallStatusEnum.InternalServerError };
            string url = string.Concat(Constants.API_URL, Constants.ONDIJON_SERVICES, Constants.SERVICE_UPDATE_USER_FAVORITE);

            request.Key = Constants.ONDIJON_KEY;
            string payload = JsonConvert.SerializeObject(request);

            var result = await _httpService.PostAsync<WsDMDto>(new Uri(url), payload);

            if (result.StatusCodes != null && result.StatusCodes.Contains(Code.Success))
            {
                //delete favourite services in cache
                await _cacheService.Delete<IList<ServiceDto>>(Constants.FavoriteServicesListKey, CacheType.InMemory);
                response.State = CallStatusEnum.Success;
            }
            response.Message = result.StatusMessages?.FirstOrDefault()?.Value;

            return response;
        }

        public async Task<FavoriteServiceListResponse> GetFavouriteServices(string userId, bool getFromCache = false)
        {
            FavoriteServiceListResponse response = new FavoriteServiceListResponse();
            if (getFromCache)
            {
                var serviceList = await _cacheService.Get<IList<ServiceDto>>(Constants.FavoriteServicesListKey, CacheType.InMemory);
                var ScopeList = await _cacheService.Get<List<CheckboxModel>>(Constants.FavoriteScopesListKey, CacheType.InMemory);
                var hasAlertIdentity = await _cacheService.Get<bool>(Constants.FavoriteScopesAlertIdentityKey, CacheType.InMemory);

                if (serviceList != null && serviceList.Any() &&
                    ScopeList != null && ScopeList.Any())
                {
                    response.Services = serviceList;
                    response.Scopes = ScopeList;
                    response.State = CallStatusEnum.Success;
                    response.HasAlertIdentity = hasAlertIdentity;
                }
            }
            if (!getFromCache || response.Services == null)
            {
                FavoriteServiceListDto sources = await GetFavouriteServicesAsync(userId);

                response = OnDijon.Common.Entities.Utils.Translate<FavoriteServiceListResponse, FavoriteServiceListDto>(sources);
                if (response.IsSuccessful())
                {
                    response.Services = sources.Services;
                    response.Scopes = new List<CheckboxModel>();
                    response.HasAlertIdentity = sources.HasAlertIdentity;
                    sources.Scopes.ForEach(s => response.Scopes.Add(new CheckboxModel() { Title = s.Title, Checked = s.Checked }));
                    await _cacheService.Put(Constants.FavoriteServicesListKey, response.Services, CacheType.InMemory);
                    await _cacheService.Put(Constants.FavoriteScopesListKey, response.Scopes, CacheType.InMemory);
                    await _cacheService.Put(Constants.FavoriteScopesAlertIdentityKey, response.HasAlertIdentity, CacheType.InMemory);
                }
            }
            return response;
        }

        private async Task<FavoriteServiceListDto> GetFavouriteServicesAsync(string userId)
        {
            FavoriteServiceListDto response = new FavoriteServiceListDto();

            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ONDIJON_SERVICES, Constants.SERVICE_GET_FAVOURITE);
                FavouriteServicesRequest data = new FavouriteServicesRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    UserEditId = userId
                };
                string json = JsonConvert.SerializeObject(data);
                response = await _httpService.PostAsync<FavoriteServiceListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }

            return response;


        }

        public Task<FavoriteServiceListResponse> GetFavouriteServices(Guid userId, bool getFromCache = false)
        {
            return GetFavouriteServices(userId.ToString(), getFromCache);
        }
    }
}

using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.Utils;
using OnDijon.Modules.UsefulContact.Entities.Dto;
using OnDijon.Modules.UsefulContact.Entities.Models;
using OnDijon.Modules.UsefulContact.Entities.Responses;
using OnDijon.Modules.UsefulContact.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnDijon.Modules.UsefulContact.Entities.Requests;
using System.Linq;
using OnDijon.Common.Entities;
using Microsoft.AppCenter.Crashes;

namespace OnDijon.Modules.UsefulContact.Services
{
    public class ServiceService : IServiceService
    {

        private readonly IHttpService _httpService;

        public ServiceService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        // Récupération de la liste des domaines de contact
        public async Task<ServiceListResponse> GetServices()
        {

            ServiceListDto sources = await GetServicesAsync();
            var response = Utils.Translate<ServiceListResponse, ServiceListDto>(sources);

            if (response.IsSuccessful())
            {
                if (sources.Services != null)
                {
                    response.ServiceList = sources.Services.Select(item =>
                    {
                        return new ServiceModel()
                        {
                            EditId = item.EditId,
                            Description = item.Description,
                            Logo = item.Logo,
                            PhoneNumber = item.PhoneNumber,
                            ShortDescription = item.ShortDescription,
                            Titre = item.Titre,
                            UrlApple = item.UrlApple,
                            UrlGoogle = item.UrlGoogle,
                            UrlSite = item.UrlSite,
                        };
                    }).ToList();
                }
            }
            return response;
        }

        private async Task<ServiceListDto> GetServicesAsync()
        {
            ServiceListDto _allServices = new ServiceListDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.UC_SERVICES, Constants.UC_GET_SERVICESLIST);
                DemandRequest data = new DemandRequest()
                {
                    Key = Constants.ONDIJON_KEY
                };
                string json = JsonConvert.SerializeObject(data);
                _allServices = await _httpService.PostAsync<ServiceListDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return _allServices;
        }


    }
}

using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Entities.Model;
using OnDijon.Common.Entities.Request;
using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Notifications.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnDijon.Modules.Notifications.Services
{
    public class AddressServices : IAddressServices
    {

        readonly OnDijon.Common.Utils.Services.Interfaces.IHttpService _httpService;

        public AddressServices(OnDijon.Common.Utils.Services.Interfaces.IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<AddressResponse> GetAddressFromCity(CityModel city, string pattern)
        {
            LanesDto sources = await GetLanesAsync(city, pattern);
            var response = OnDijon.Common.Entities.Utils.Translate<AddressResponse, LanesDto>(sources);

            if (response.IsSuccessful())
            {
                response.AddressModel = sources.Lanes.Select(item =>
                {
                    return new AddressModel()
                    {
                        City = item.City,
                        CodCom = item.CodCom,
                        ExtAd = item.ExtAd,
                        FullAddress = item.FullAddress,
                        LaneName = item.LaneName,
                        Num = item.Num,
                        PostalCode = item.PostalCode,
                        Provider = item.Provider,
                        Rivoli = item.Rivoli,
                        X = item.X,
                        Y = item.Y

                    };
                }).ToList();
            }
            return response;
        }


        private async Task<LanesDto> GetLanesAsync(CityModel city, string pattern)
        {
            LanesDto result = new LanesDto();
            try
            {
                string url = "https://eservices.dijon.fr/_vti_bin/Json/AddressService.svc/GetAddressFromQuery";
                LaneRequest data = new LaneRequest()
                {
                    Key = "B0FC4DE0-9C27-418B-B7A9-020AFD76B1B2",
                    Query = pattern,
                    CodCom = city.CodComm,
                };
                string json = JsonConvert.SerializeObject(data);
                result = await _httpService.PostAsync<LanesDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return result;
        }


        public async Task<CityResponse> GetCities(string pattern, bool IsMetropolitanCities = false)
        {
            CitiesDto sources = await GetCitiesAsync(pattern, IsMetropolitanCities);
            var response = OnDijon.Common.Entities.Utils.Translate<CityResponse, CitiesDto>(sources);

            if (response.IsSuccessful())
            {
                response.CityModels = sources.Cities.Select(item =>
                {
                    return new CityModel()
                    {
                        CodComm = item.codcomm,
                        Name = item.communes,
                        CodePost = item.ccodep
                        
                    };
                }).ToList();
            }
            return response;
        }


        private async Task<CitiesDto> GetCitiesAsync(string pattern, bool IsMetropolitanCities)
        {
            CitiesDto result = new CitiesDto();
            try
            {
                //string url = string.Concat(Constants.WEBSERVICES_URL, Constants.ADR_SERVICES, Constants.ADDRESS_GET_CITIES);
                string url = "https://eservices.dijon.fr/_vti_bin/Json/AddressService.svc/GetCitiesFromQuery";
                CityRequest data = new CityRequest()
                {
                    Key = "B0FC4DE0-9C27-418B-B7A9-020AFD76B1B2",
                    Query = pattern,
                    IsMetropolitanCities = IsMetropolitanCities,
                };
                string json = JsonConvert.SerializeObject(data);
                result = await _httpService.PostAsync<CitiesDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return result;
        }

    }
}

using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Demands.Entities.Dto;
using OnDijon.Modules.Demands.Entities.Models;
using OnDijon.Modules.Demands.Entities.Requests;
using OnDijon.Modules.Demands.Entities.Responses;
using OnDijon.Modules.Demands.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnDijon.Modules.Demands.Services
{
    class DemandService : IDemandService
    {
        readonly IHttpService _httpService;

        public DemandService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        private string GetCityApiUrl(string city, string method)
        {
            return string.Concat(Constants.API_URL, city, "/", Constants.DE_SERVICES, method);
        }

        public async Task<DemandListResponse> GetDemands(string city, string idUser)
        {
            var sources = await GetDemandsAsync(city, idUser);
            DemandListResponse response = Utils.Translate<DemandListResponse, DemandListDto>(sources);

            if (response.IsSuccessful())
            {
                response.CityContext = sources.CityContext;
                response.DemandList = sources.Demands.Select(item =>
                {
                    return new DemandModel()
                    {
                        Category = item.Category,
                        Date = item.Date,
                        FirstDescription = item.FirstDescription != null ? new DescriptionModel()
                        {
                            Class = item.FirstDescription.Class,
                            Title = item.FirstDescription.Title,
                            Value = item.FirstDescription.Value
                        } : null,
                        SecondDescription = item.SecondDescription != null ? new DescriptionModel()
                        {
                            Class = item.SecondDescription.Class,
                            Title = item.SecondDescription.Title,
                            Value = item.SecondDescription.Value
                        } : null,
                        ThirdDescription = item.ThirdDescription != null ? new DescriptionModel()
                        {
                            Class = item.ThirdDescription.Class,
                            Title = item.ThirdDescription.Title,
                            Value = item.ThirdDescription.Value
                        } : null,
                        ActionDemand = item.ActionDemand != null ? new ActionModel()
                        {
                            Title = item.ActionDemand.Title,
                            ObjectEditId = item.ActionDemand.ObjectEditId,
                            CityContext = item.ActionDemand.CityContext
                        } : null,
                        ServiceCode = item.ServiceCode,
                        State = item.State,
                        Title = item.Title,
                        CityContext = item.CityContext,
                    };
                }).ToList();
            }
            return response;
        }


        private async Task<DemandListDto> GetDemandsAsync(string city, string idUser)
        {
            DemandListDto _allDomains = new DemandListDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.DE_SERVICES, Constants.DE_GET_DEMANDSLIST);
                //string url = GetCityApiUrl(city, Constants.DE_GET_DEMANDSLIST);
                DemandRequest data = new DemandRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Id = idUser

                };
                string json = JsonConvert.SerializeObject(data);
                _allDomains = await _httpService.PostAsync<DemandListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
            }
            return _allDomains;
        }
    }
}

using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Simulator.Entities.Dto;
using OnDijon.Modules.Simulator.Entities.Models;
using OnDijon.Modules.Simulator.Entities.Requests;
using OnDijon.Modules.Simulator.Entities.Responses;
using OnDijon.Modules.Simulator.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnDijon.Modules.Simulator.Services
{
    public class SimulatorRateService : ISimulatorRateService
    {
        readonly IHttpService _httpService;

        public SimulatorRateService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<CityContextResponse> GetAllCityContext(string service)
        {
            CityContextListDto sources = await GetAllCityContextAsync(service);

            CityContextResponse response = Utils.Translate<CityContextResponse, CityContextListDto>(sources);

            if (response.IsSuccessful())
            {
                response.Cities = sources.Cities.Select(item =>
                {
                    CityContextModel city = new CityContextModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        IsDoubleCompute = item.IsDoubleCompute,
                    };

                    return city;
                }).ToList();
            }

            return response;
        }

        public async Task<CityContextListDto> GetAllCityContextAsync(string service)
        {
            CityContextListDto _cityContext = new CityContextListDto();

            try
            {
                string url = string.Concat(Constants.API_URL, Constants.SR_SERVICES, Constants.SR_GET_ALLCITYCONTEXT);

                CityContextRequest data = new CityContextRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ServiceId = service,
                };

                string json = JsonConvert.SerializeObject(data);
                _cityContext = await _httpService.PostAsync<CityContextListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _cityContext;
        }

        public async Task<SimulatorRateResponse> GetSimulatorRate(int childNumber, decimal annualIncome, bool dijon, decimal qF, string cityContextId)
        {
            SimulatorRateDto sources = await GetSimulatorRateAsync(childNumber, annualIncome, dijon, qF, cityContextId);

            SimulatorRateResponse response = Utils.Translate<SimulatorRateResponse, SimulatorRateDto>(sources);

            if (response.IsSuccessful())
            {
                response.DomainSimulatorRate = sources.DomainSimulatorRate.Select(item =>
                {
                    DomainSimulatorRateModel domaine = new DomainSimulatorRateModel()
                    {
                        Title = item.Title,
                    };

                    if (item.Categories != null)
                    {
                        domaine.Categories = item.Categories.Select(cItem =>
                        {
                            return new CategorySimulatorRateModel()
                            {
                                Title = cItem.Title,
                                Detail = cItem.Detail,
                                Rate = cItem.Rate
                            };
                        }).ToList();
                    }

                    return domaine;
                }).ToList();
            }

            return response;
        }

        private async Task<SimulatorRateDto> GetSimulatorRateAsync(int childNumber, decimal annualIncome, bool dijon, decimal qF, string cityContextId)
        {
            SimulatorRateDto _simulatorRate = new SimulatorRateDto();

            try
            {
                string url = string.Concat(Constants.API_URL, Constants.SR_SERVICES, Constants.SR_GET_SIMULATORRATE);

                SimulatorRateRequest data = new SimulatorRateRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ChildNumber = childNumber,
                    AnnualIncome = annualIncome,
                    Resident = dijon,
                    QF = qF,
                    CityContextId = cityContextId
                };

                string json = JsonConvert.SerializeObject(data);
                _simulatorRate = await _httpService.PostAsync<SimulatorRateDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _simulatorRate;
        }
    }
}
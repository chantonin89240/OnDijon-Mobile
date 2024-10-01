using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.Utils;
using OnDijon.Modules.JobOffer.Entities.Dto;
using OnDijon.Modules.JobOffer.Entities.Models;
using OnDijon.Modules.JobOffer.Entities.Responses;
using OnDijon.Modules.JobOffer.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnDijon.Modules.JobOffer.Entities.Requests;
using System.Linq;
using OnDijon.Common.Entities;
using Microsoft.AppCenter.Crashes;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.JobOffer.Services
{
    public class JobOfferService : IJobOfferService
    {
        readonly IHttpService _httpService;

        public JobOfferService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<ListJobOfferResponse> GetJobOffer()
        {
           
            ListJobOfferDto sources = await GetJobOfferAsync();
            var response = Utils.Translate<ListJobOfferResponse, ListJobOfferDto>(sources);

            if (response.IsSuccessful())
            {
                if (sources.JobOffers != null)
                {
                    response.JobOfferList = sources.JobOffers.Select(item =>
                    {
                        return new JobOfferModel()
                        {
                            Content = item.Content,
                            Conditions = item.Conditions,
                            EditId = item.EditId,
                            LimitDate = item.LimitDate,
                            Picture = item.Picture,
                            Subtitle = item.Subtitle,
                            Title = item.Title,
                            Type = item.Type,
                            City = item.City,
                        };
                    }).ToList();
                }
            }

            return response;
        }

        public async Task<ListJobOfferDto> GetJobOfferAsync()
        {
            ListJobOfferDto _ListJobOffer = new ListJobOfferDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.JO_SERVICES, Constants.JOB_OFFER_GET_LIST);
                ListJobOfferRequest data = new ListJobOfferRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                };
                string json = JsonConvert.SerializeObject(data);
                _ListJobOffer = await _httpService.PostAsync<ListJobOfferDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return _ListJobOffer;
        }

        public async Task<ListTypeJobOfferResponse> GetTypeJobOffer(string city)
        {
            ListTypeJobOfferDto sources = await GetTypeJobOfferAsync(city);
            var response = Utils.Translate<ListTypeJobOfferResponse, ListTypeJobOfferDto>(sources);

            if (response.IsSuccessful())
            {
                if (sources.TypeList != null)
                {
                    response.TypeJobOfferList = sources.TypeList;
                }
            }

            return response;
        }

        public async Task<ListTypeJobOfferDto> GetTypeJobOfferAsync(string city)
        {
            ListTypeJobOfferDto _ListTypeJobOffer = new ListTypeJobOfferDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.JO_SERVICES, Constants.JOB_OFFER_GET_TYPE);
                ListTypeJobOfferRequest data = new ListTypeJobOfferRequest()
                {
                    City = city,
                    Key = Constants.ONDIJON_KEY,
                };
                string json = JsonConvert.SerializeObject(data);
                _ListTypeJobOffer = await _httpService.PostAsync<ListTypeJobOfferDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return _ListTypeJobOffer;
        }

        public async Task<Response> SendApplication(JobApplicationRequest data)
        {
            WsDMDto sources = await SendApplicationAsync(data);
            return Utils.Translate<Response, WsDMDto>(sources);

        }

        private async Task<WsDMDto> SendApplicationAsync(JobApplicationRequest data)
        {
            WsDMDto res = new WsDMDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.JO_SERVICES, Constants.SEND_JOB_OFFER_APPLICATION);
                data.Key = Constants.ONDIJON_KEY;
                string json = JsonConvert.SerializeObject(data);
                res = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return res;
        }
    }
}

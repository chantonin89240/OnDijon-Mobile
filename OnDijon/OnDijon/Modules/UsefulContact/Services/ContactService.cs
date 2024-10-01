using OnDijon.Modules.UsefulContact.Entities.Dto;
using OnDijon.Modules.UsefulContact.Entities.Models;
using OnDijon.Modules.UsefulContact.Entities.Responses;
using OnDijon.Modules.UsefulContact.Services.Interfaces;
using OnDijon.Common.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.UsefulContact.Entities.Requests;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using System.Linq;
using System.Globalization;
using Microsoft.AppCenter.Crashes;

namespace OnDijon.Modules.UsefulContact.Services
{
    public class ContactService : IContactService
    {
        readonly IHttpService _httpService;

        public ContactService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<OpeningTimeResponse> GetOpeningTime(string id)
        {
            var sources = await GetOpeningTimeAsync(id);

            var response = Utils.Translate<OpeningTimeResponse, DetailContactDto>(sources);
            if (response.IsSuccessful())
            {
                if (sources.OpeningTime != null)
                {
                    CultureInfo provider = new CultureInfo("fr-FR");
                    response.OpeningTime = sources.OpeningTime.Select(item =>
                    {
                        return new ContactOpeningPeriodModel()
                        {
                            Day = DateTime.Parse(item.Day, provider),
                            BeginPeriod = int.Parse(item.BeginTime),
                            EndPeriod = int.Parse(item.EndTime)
                        };
                    }).ToList();
                }
                else
                {
                    response.OpeningTime = new List<ContactOpeningPeriodModel>();
                }
            }
            return response;
        }

        private async Task<DetailContactDto> GetOpeningTimeAsync(string id)
        {
            DetailContactDto _contact = new DetailContactDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.UC_SERVICES, Constants.UC_GET_OPENINGTIME);
                DemandWithIdRequest r = new DemandWithIdRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = id
                };
                string json = JsonConvert.SerializeObject(r);
                _contact = await _httpService.PostAsync<DetailContactDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return _contact;
        }
    }
}

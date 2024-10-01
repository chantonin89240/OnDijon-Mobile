using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.Utils;
using OnDijon.Modules.Strike.Entities.DTO;
using OnDijon.Modules.Strike.Entities.Model;
using OnDijon.Modules.Strike.Entities.Responses;
using OnDijon.Modules.Strike.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnDijon.Modules.Strike.Entities.Request;
using OnDijon.Common.Entities;
using Microsoft.AppCenter.Crashes;

namespace OnDijon.Modules.Strike.Services
{
    public class ListStrikeService : IListStrikeService
    {
        readonly IHttpService _httpService;

        public ListStrikeService(IHttpService httpService)
        {
            _httpService = httpService;
        }



        public async Task<SessionStrikeResponse> GetSessionStrike(string idSession)
        {
            SessionStrikeDto sources = await GetSessionStrikeAsync(idSession);
            var response = Utils.Translate<SessionStrikeResponse, SessionStrikeDto>(sources);

            if (response.IsSuccessful())
            {
                if (sources.Strikes != null)
                {
                    var listStrikes = new List<SchoolStrikeInfoModel>();
                    foreach (var item in sources.Strikes)
                    {
                        listStrikes.Add(new SchoolStrikeInfoModel()
                        {
                            Comment = item.Comment,
                            EditId = item.EditId,
                            EveningExtracurricular = item.EveningExtracurricular,
                            MorningExtracurricular = item.MorningExtracurricular,
                            Name = item.Name,
                            NoonReception = item.NoonReception,
                            SchoolRestaurant = item.SchoolRestaurant,
                            SchoolStatus = item.SchoolStatus,
                            TAP = item.TAP,
                            EducationLevel = item.EducationLevel,

                        });

                    }

                    response.SessionStrike = new SessionStrikeModel()
                    {
                        DateStrike = sources.DateStrike,
                        Strikes = listStrikes

                    };
                }
            }

            return response;

        }

        public async Task<SessionStrikeDto> GetSessionStrikeAsync(string idSession)
        {
            SessionStrikeDto _strikeSelected = new SessionStrikeDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.GRE_SERVICES, Constants.GRE_GET_SCHOOL_STRIKESLIST);
                StrikeListRequest data = new StrikeListRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = idSession,
                };
                string json = JsonConvert.SerializeObject(data);
                _strikeSelected = await _httpService.PostAsync<SessionStrikeDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return _strikeSelected;

        }


        public async Task<NurserySessionStrikeResponse> GetNurserySessionStrike(string idSession)
        {
            NurserySessionStrikeDto sources = await GetNurserySessionStrikeAsync(idSession);
            var response = Utils.Translate<NurserySessionStrikeResponse, NurserySessionStrikeDto>(sources);

            if (response.IsSuccessful())
            {
                if (sources.Strikes != null)
                {
                    var listStrikes = new List<NurseryStrikeInfoModel>();
                    foreach (var item in sources.Strikes)
                    {
                        listStrikes.Add(new NurseryStrikeInfoModel()
                        {
                            Name = item.Name,
                            EditId = item.EditId,
                            Detail = item.Detail

                        });

                    }

                    response.NurserySessionStrike = new NurserySessionStrikeModel()
                    {
                        DateStrike = sources.DateStrike,
                        Strikes = listStrikes

                    };
                }
            }

            return response;
        }

        public async Task<NurserySessionStrikeDto> GetNurserySessionStrikeAsync(string idSession)
        {
            NurserySessionStrikeDto _strikeSelected = new NurserySessionStrikeDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.GRE_SERVICES, Constants.GRE_GET_NURSE_STRIKESLIST);
                StrikeListRequest data = new StrikeListRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = idSession,
                };
                string json = JsonConvert.SerializeObject(data);
                _strikeSelected = await _httpService.PostAsync<NurserySessionStrikeDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return _strikeSelected;
        }
    }
}
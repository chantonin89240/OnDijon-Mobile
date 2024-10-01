using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.RoadworkInformation.Entities.Dto;
using OnDijon.Modules.RoadworkInformation.Entities.Models;
using OnDijon.Modules.RoadworkInformation.Entities.Requests;
using OnDijon.Modules.RoadworkInformation.Entities.Responses;
using OnDijon.Modules.RoadworkInformation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDijon.Modules.RoadworkInformation.Services
{
    public class RoadworkInfoService : IRoadworkInfoService
    {
        readonly IHttpService _httpService;

        public RoadworkInfoService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        //Récupérer tout les travaux
        public async Task<RoadworkInfoResponse> GetRoadworks(string UserId, string ObjectType)
        {
            var sources = await GetRoadworksAsync(UserId,ObjectType);
            var response = Utils.Translate<RoadworkInfoResponse, RoadworkInfoListDto>(sources);
            if (response.IsSuccessful())
            {
                if (sources.RoadworkList != null)
                {
                    response.RoadworkList = sources.RoadworkList.Select(item =>
                    {
                        var templist = new List<RoadworkRingModel>();
                        foreach (var ring in item.Area)
                        {
                            templist.Add(new RoadworkRingModel { Latitude = ring.Latitude, Longitude = ring.Longitude });
                        }

                        return new RoadworkInfoModel()
                        {
                            EditId = item.EditId,
                            Title = item.Title,
                            ObjectType = item.ObjectType,
                            Applicant = item.Applicant,
                            Executant = item.Executant,
                            DateBeginRoadwork = item.DateBeginRoadwork,
                            DateEndRoadwork = item.DateEndRoadwork,
                            X = item.X,
                            Y = item.Y,
                            State = item.State,
                            Area = templist
                        };
                    }).ToList();
                }
            }
            return response;
        }

        private async Task<RoadworkInfoListDto> GetRoadworksAsync(string UserId, string ObjectType)
        {
            RoadworkInfoListDto _RoadworkList = new RoadworkInfoListDto();
            try
            {
                 string url = string.Concat(Constants.API_URL, Constants.WORK_SERVICES, Constants.WORK_GET_ALL);
                RoadworkInfoRequest data = new RoadworkInfoRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ObjectType = ObjectType,
                    UserEditId = UserId
                };
                string json = JsonConvert.SerializeObject(data);
                _RoadworkList = await _httpService.PostAsync<RoadworkInfoListDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _RoadworkList;
        }
    }
}
